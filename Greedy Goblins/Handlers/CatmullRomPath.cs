using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CatmullRom
{
    public class CatmullRomPath
    {
        private class TriangleStripBuffer
        {
            private VertexBuffer buffer;
            private BasicEffect effect;
            private int nbrVertices;

            public void Update(GraphicsDevice device, VertexPositionTexture[] vertices)
            {
                buffer = new VertexBuffer(
                    device,
                    typeof(VertexPositionTexture),
                    vertices.Length,
                    BufferUsage.WriteOnly);
                buffer.SetData<VertexPositionTexture>(vertices);
                nbrVertices = vertices.Length;

                // Set up effect
                float w = (float)device.Viewport.Width;
                float h = (float)device.Viewport.Height;
                effect = new BasicEffect(device)
                {
                    World = Matrix.Identity,
                    View = Matrix.CreateLookAt(new Vector3(w / 2, h / 2, -10.0f), new Vector3(w / 2, h / 2, 0), -Vector3.Up),
                    Projection = Matrix.CreateOrthographic(w, h, 1.0f, 10.0f)
                };
                //effect.VertexColorEnabled = true; // When vertex colors are used instead of a texture
            }

            public bool IsValid()
            {
                return buffer != null;
            }

            public void Draw(GraphicsDevice device, Texture2D texture)
            {
                device.BlendState = BlendState.AlphaBlend;
                device.DepthStencilState = DepthStencilState.Default;
                device.SamplerStates[0] = SamplerState.LinearWrap;
                //g.RasterizerState = RasterizerState.CullNone;

                effect.Texture = texture;
                effect.TextureEnabled = true;

                effect.CurrentTechnique.Passes[0].Apply();
                device.SetVertexBuffer(buffer);
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, nbrVertices - 2);
            }
        }

        private List<Vector2> controlPoints;
        // tension
        private float t;

        // 1x1 texture for drawing control points
        private Texture2D texture_controlPoints;

        // Wrapper for a triangle-strip VertexBuffer
        TriangleStripBuffer tristripBuffer;

        public CatmullRomPath(GraphicsDevice gd, float tension = 0.5f)
        {
            if (tension < 0.0f || tension > 1.0f)
                throw new System.ArgumentException("@tension must be in the [0,1] range");

            controlPoints = new List<Vector2>();
            t = tension;

            
            texture_controlPoints = new Texture2D(gd, 1, 1, false, SurfaceFormat.Color);

            tristripBuffer = new TriangleStripBuffer();
            //fillDrawState = new FillDrawState();

            Clear();
        }

        public void AddPoint(Vector2 point)
        {
            controlPoints.Insert(controlPoints.Count - 1, point);

            UpdateEndPoints();
            //isUpToDate = false;
        }

        public void SetPoint(int i, Vector2 point)
        {
            if (i < 0 || i > controlPoints.Count - 2)
                throw new ArgumentException("i out of bounds");

            controlPoints[i+1] = point;

            UpdateEndPoints();
            //isUpToDate = false;
        }

        public Vector2[] GetPoints()
        {
            Vector2[] points = new Vector2[controlPoints.Count - 2];
            for (int i = 1; i < controlPoints.Count - 1; i++)
                points[i-1] = controlPoints[i];
            return points;
        }

        public void Clear()
        {
            controlPoints.Clear();
            controlPoints.Add(new Vector2());
            controlPoints.Add(new Vector2());
            //isUpToDate = false;
        }

        // @points is >3 control points
        // @x and @tension must be be in [0,1] range
        // Returns an interpolated Vector2
        //
        // tension = 0: sharper turns
        // tension = 0.5: moderately sharp turns
        // tension = 1: smooth turns
        public Vector2 EvaluateAt(float x)
        {
            if (controlPoints.Count < 4)
                throw new System.InvalidOperationException("Must add at least two control points");
            if (x < 0.0f || x > 1.0f)
                throw new System.ArgumentException("@x must be in the [0,1] range");


            float f = x * (controlPoints.Count - 3);
            int i = (int)System.Math.Floor(f);
            float frac;// = f - i;

            if (x < 1.0f)
                frac = f - i;
            else
            {
                // x is exactly 1.0f
                i--;
                frac = 1.0f;
            }   

            return EvalCatmullRomSpline(
                controlPoints[i + 0],
                controlPoints[i + 1],
                controlPoints[i + 2],
                controlPoints[i + 3],
                frac,
                t);
        }

        public Vector2 EvaluateTangentAt(float x)
        {
            if (controlPoints.Count < 4)
                throw new System.InvalidOperationException("Must add at least two control points");
            if (x < 0.0f || x > 1.0f)
                throw new System.ArgumentException("@x must be in the [0,1] range");

            float f = x * (controlPoints.Count - 3);
            int i = (int)System.Math.Floor(f);
            float frac;// = f - i;

            if (x < 1.0f)
                frac = f - i;
            else
            {
                // x is exactly 1.0f
                i--;
                frac = 1.0f;
            }

            return EvalCatmullRomSplineTangent(
                controlPoints[i + 0],
                controlPoints[i + 1],
                controlPoints[i + 2],
                controlPoints[i + 3],
                frac,
                t);

            // The above code returns the 'analytical' tangent.
            // We can also calculate the tangent numerically by
            // taking the difference between two nearby positions
            // on the curve. This technique works for any spline,
            // even ones we don't have the mathematical expression for.
            //
            //float eps = 0.01f;
            //Vector2 point = EvaluateAt(x);
            //Vector2 point_next, dir;
            //if (x + eps > 1.0f)
            //{
            //    point_next = EvaluateAt(x - eps);
            //    dir = point - point_next;
            //}
            //else
            //{
            //    point_next = EvaluateAt(x + eps);
            //    dir = point_next - point;
            //}
            //return dir;
        }

        public void DrawPoints(SpriteBatch sb, Color color, int size)
        {
            if (controlPoints.Count < 4)
                throw new System.InvalidOperationException("Must add at least two control points");

            texture_controlPoints.SetData<Color>(new Color[] { color });

            sb.Begin();
            for (int i = 1; i < controlPoints.Count - 1; i++)
            {
                Vector2 pos = new Vector2((int)controlPoints[i].X, (int)controlPoints[i].Y);
                Vector2 origo = new Vector2(0.5f, 0.5f);
                Vector2 sizev = new Vector2(size, size);
                sb.Draw(texture_controlPoints, pos, null, Color.White, 0, origo, sizev, SpriteEffects.None, 1);
            }
            sb.End();
        }

        public void DrawFillSetup(GraphicsDevice device, uint radius, uint textureRepeat = 1, uint subdivisions = 256)
        {
            if (controlPoints.Count < 4)
                throw new InvalidOperationException("Must add at least two control points");

            // Calculate fill mesh and use it to set up the fill VertexBuffer
            VertexPositionTexture[] vertices = new VertexPositionTexture[2 * subdivisions];
            for (int i = 0; i < subdivisions; i++)
            {
                float f = (float)i / (subdivisions - 1);
                Vector2 pos = EvaluateAt(f);
                Vector2 tangent = EvaluateTangentAt(f);
                Vector2 normal = new Vector2(tangent.Y, -tangent.X);
                normal.Normalize();
                Vector2 left = pos - radius * normal;
                Vector2 right = pos + radius * normal;
                Vector3 left3 = new Vector3(left.X, left.Y, 0.0f);
                Vector3 right3 = new Vector3(right.X, right.Y, 0.0f);
                Vector2 uvleft = new Vector2(0, f * textureRepeat);
                Vector2 uvright = new Vector2(1, f * textureRepeat);

                vertices[2 * i + 0] = new VertexPositionTexture(left3, uvleft);
                vertices[2 * i + 1] = new VertexPositionTexture(right3, uvright);
            }
            tristripBuffer.Update(device, vertices);
        }

        public void DrawFill(GraphicsDevice device, Texture2D texture)
        {
            if (controlPoints.Count < 4)
                throw new System.InvalidOperationException("Must add at least two control points");
            if (!tristripBuffer.IsValid())
                throw new InvalidOperationException("Call DrawFillSetup before Draw");

            tristripBuffer.Draw(device, texture);
            return;
        }

        // Private methods

        private void UpdateEndPoints()
        {
            if (controlPoints.Count >= 4)
            {
                int c = controlPoints.Count;
                controlPoints[0] = controlPoints[1] + 0.5f * (controlPoints[1] - controlPoints[2]);
                controlPoints[c - 1] = controlPoints[c - 2] + 0.5f * (controlPoints[c - 2] - controlPoints[c - 3]);
            }
        }

        /*
         * Catmull-Rom spline, Q(x)
         * Special case of a cubic Hermite spline,
         * commonly used to interpolate animation keyframes, camera paths etc.
         * 
         * t = 'tension'
         *  t = 0   'uniform' spline, equivalent to linear intrpolation between p1 and p2. 
         *  t = 0.5 'centripetal' Carmull-Rom, bends smoothly.
         *  t = 1.0 'chordal' spline, bends very smoothly. 
         * 
         * Mathematical definition
         * 							    |	0	1	0		0	|	|	p0	|
         *  Q(x) = | 1  x   x^2 x^3 | * |	-t	0	t		0	| * |	p1	|
         * 						    	|	2t	t-3	3-2t	-t	|	|	p2	|
         * 							    |	-t	2-t	t-2		t	|	|	p3	|
         */
        private Vector2 EvalCatmullRomSpline(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float x, float t)
        {
            float x2 = x * x;
            float x3 = x2 * x;

            Vector2 Q = (p0 * (-t * x + 2.0f * t * x2 - t * x3) +
                    p1 * (1.0f + (t - 3.0f) * x2 + (2.0f - t) * x3) +
                    p2 * (t * x + (3.0f - 2.0f * t) * x2 + (t - 2.0f) * x3) +
                    p3 * (-t * x2 + t * x3));
            return Q;
        }

        /*
         * Catmull-Rom tangent
         * Derivative of Q with respect to x.
         * 
         *          dQ(x)
         * T(x) =   -----
         *          dx
         * 			
         */
        private Vector2 EvalCatmullRomSplineTangent(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float x, float t)
        {
            float x2 = x * x;

            Vector2 T = (p0 * (-t + 4.0f * t * x - 3.0f * t * x2) +
                        p1 * (2.0f * (t - 3.0f) * x + 3.0f * (2.0f - t) * x2) +
                        p2 * (t + 2.0f * (3.0f - 2.0f * t) * x + 3.0f * (t - 2.0f) * x2) +
                        p3 * (-2.0f * t * x + 3.0f * t * x2));

            return T;
        }
    }
}
