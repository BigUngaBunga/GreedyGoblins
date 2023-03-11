using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Greedy_Goblins
{
    static class AssetManager
    {
        public static Dictionary<string, Texture2D> textures;

        public static Dictionary<string, SpriteFont> fonts;

        public static Dictionary<string, SoundEffect> soundEffects;
        public static Dictionary<string, SoundEffectInstance> soundInstances;
        public static Dictionary<string, SoundEffectInstance> musicInstances;
        public static Dictionary<string, float> audioVolume;

        public static void LoadContent(ContentManager content)
        {
            textures = new Dictionary<string, Texture2D>();
            fonts = new Dictionary<string, SpriteFont>();
            soundEffects = new Dictionary<string, SoundEffect>();
            soundInstances = new Dictionary<string, SoundEffectInstance>();
            musicInstances = new Dictionary<string, SoundEffectInstance>();
            audioVolume = new Dictionary<string, float>();



            //General textures      textures.Add("", content.Load<Texture2D>(@"Objects\"));
            textures.Add("button", content.Load<Texture2D>(@"Objects\WhiteButton"));
            textures.Add("borderless button", content.Load<Texture2D>(@"Objects\BorderlessWhiteButton"));
            textures.Add("circle", content.Load<Texture2D>(@"Objects\Neues Ball"));
            textures.Add("borderless circle", content.Load<Texture2D>(@"Objects\WhiteCircle"));
            textures.Add("large circle", content.Load<Texture2D>(@"Objects\Big Circle"));
            textures.Add("pixel", content.Load<Texture2D>(@"Objects\SinglePixel"));
            textures.Add("knife", content.Load<Texture2D>(@"Objects\Knife"));
            textures.Add("paper", content.Load<Texture2D>(@"Objects\lapp bakgrund"));
            textures.Add("wooden board", content.Load<Texture2D>(@"Objects\Wooden Board"));
            textures.Add("speech", content.Load<Texture2D>(@"Objects\Speech Bubble"));
            textures.Add("fire spritesheet", content.Load<Texture2D>(@"Objects\Fire Spritesheet"));
            textures.Add("explosion spritesheet", content.Load<Texture2D>(@"Objects\Explosions Spritesheet"));
            //Enemies
            textures.Add("goblin", content.Load<Texture2D>(@"Objects\GoblinSpritesheet"));
            textures.Add("miner", content.Load<Texture2D>(@"Objects\GruvarbetareSpritesheet"));
            textures.Add("fighter", content.Load<Texture2D>(@"Objects\DwarfFighterSpritesheet"));
            textures.Add("soldier", content.Load<Texture2D>(@"Objects\DwarvenSoldierSpritesheet"));
            textures.Add("king", content.Load<Texture2D>(@"Objects\DwarvenKingSpritesheet"));
            //Towers
            textures.Add("cannon", content.Load<Texture2D>(@"Objects\Kanon"));
            textures.Add("crossbowman", content.Load<Texture2D>(@"Objects\Goblin_med_amborst"));
            textures.Add("flamethrower", content.Load<Texture2D>(@"Objects\Goblin_med_eldkastare"));
            //Projectiles
            textures.Add("quarrel", content.Load<Texture2D>(@"Objects\Skakta"));
            textures.Add("cannonball", content.Load<Texture2D>(@"Objects\Kanonkula"));


            //Map textures      textures.Add("", content.Load<Texture2D>(@"Map\"));
            //Backgrounds
            textures.Add("forest map", content.Load<Texture2D>(@"Map\Skogskarta"));
            textures.Add("shrubland map", content.Load<Texture2D>(@"Map\Buskskogskarta"));
            textures.Add("mountain map", content.Load<Texture2D>(@"Map\Bergskarta"));
            textures.Add("snow map", content.Load<Texture2D>(@"Map\Snokarta"));
            //Paths
            textures.Add("dirt path", content.Load<Texture2D>(@"Map\Stig"));
            textures.Add("snow path", content.Load<Texture2D>(@"Map\Snostig"));
            //Plants
            textures.Add("large pinetree", content.Load<Texture2D>(@"Map\Stort_barrtrad"));
            textures.Add("large tree", content.Load<Texture2D>(@"Map\Stort_lovtrad"));
            textures.Add("small tree", content.Load<Texture2D>(@"Map\Litet_lovtrad"));
            textures.Add("large shrub", content.Load<Texture2D>(@"Map\Stor_buske"));
            textures.Add("small shrub", content.Load<Texture2D>(@"Map\Liten_buske"));
            //Rocks
            textures.Add("large rock", content.Load<Texture2D>(@"Map\Stor_sten"));
            textures.Add("small rock", content.Load<Texture2D>(@"Map\Liten_sten"));
            textures.Add("tiny rock 1", content.Load<Texture2D>(@"Map\Sma_sten_1"));
            textures.Add("tiny rock 2", content.Load<Texture2D>(@"Map\Sma_sten_2"));
            textures.Add("tiny rock 3", content.Load<Texture2D>(@"Map\Sma_sten_3"));


            //Background images  textures.Add("", content.Load<Texture2D>(@"Backgrounds\"));
            textures.Add("editor info", content.Load<Texture2D>(@"Backgrounds\MapEditorInfo"));
            textures.Add("options background", content.Load<Texture2D>(@"Backgrounds\Button Background"));
            textures.Add("chalk board", content.Load<Texture2D>(@"Backgrounds\Chalk Board"));
            textures.Add("title screen", content.Load<Texture2D>(@"Backgrounds\Greedy Goblins Main Screen"));
            textures.Add("game info", content.Load<Texture2D>(@"Backgrounds\Play Info"));
            textures.Add("credits", content.Load<Texture2D>(@"Backgrounds\Credits"));


            //Fonts     fonts.Add("", content.Load<SpriteFont>(@"Fonts\"));
            fonts.Add("button", content.Load<SpriteFont>(@"Fonts\ButtonFont"));
            fonts.Add("editor", content.Load<SpriteFont>(@"Fonts\MapPointFont"));
            fonts.Add("title", content.Load<SpriteFont>(@"Fonts\TitleFont"));
            fonts.Add("information", content.Load<SpriteFont>(@"Fonts\InformationFont"));


            //Sound effects   soundEffects.Add("", content.Load<SoundEffect>(@"Audio\"));
            soundEffects.Add("crossbowman", content.Load<SoundEffect>(@"Audio\Archer"));
            soundEffects.Add("cannon", content.Load<SoundEffect>(@"Audio\Cannon Fire"));
            soundEffects.Add("flamethrower", content.Load<SoundEffect>(@"Audio\Flamethrower"));
            soundEffects.Add("quarrel hit", content.Load<SoundEffect>(@"Audio\Click Enemy"));
            soundEffects.Add("cannonball explosion", content.Load<SoundEffect>(@"Audio\Cannonball Explosion"));

            //Sound instances         soundInstances.Add("", content.Load<SoundEffect>(@"Audio\").CreateInstance());
            soundInstances.Add("select button", content.Load<SoundEffect>(@"Audio\Button Select").CreateInstance());
            soundInstances.Add("click button", content.Load<SoundEffect>(@"Audio\Button Click").CreateInstance());
            soundInstances.Add("place tower", content.Load<SoundEffect>(@"Audio\Place Tower").CreateInstance());
            soundInstances.Add("cant place tower", content.Load<SoundEffect>(@"Audio\Cant Place Tower").CreateInstance());


            //Music        musicInstances.Add("", content.Load<SoundEffect>(@"Audio\").CreateInstance());
            musicInstances.Add("menu theme", content.Load<SoundEffect>(@"Audio\Greedy Goblins Main Theme 2 Electric Boogaloo").CreateInstance());
            musicInstances.Add("battle theme", content.Load<SoundEffect>(@"Audio\Greedy Goblins Battle Theme").CreateInstance());

            LoopSongs();
            ConfiguerVolume();
        }

        private static void LoopSongs()
        {
            foreach (SoundEffectInstance song in musicInstances.Values)
                song.IsLooped = true;
        }

        private static void ConfiguerVolume()
        {   
            float musicVolume = 0.25f;
            musicInstances["menu theme"].Volume = musicVolume;
            musicInstances["battle theme"].Volume = musicVolume;

            float buttonVolume = 0.3f;
            soundInstances["click button"].Volume = buttonVolume;
            soundInstances["select button"].Volume = buttonVolume;

            float towerVolume = 0.6f;
            soundInstances["place tower"].Volume = towerVolume;
            soundInstances["cant place tower"].Volume = towerVolume;

            foreach (KeyValuePair<string, SoundEffectInstance> sound in soundInstances)
                audioVolume.Add(sound.Key, sound.Value.Volume);
            foreach (KeyValuePair<string, SoundEffectInstance> sound in musicInstances)
                audioVolume.Add(sound.Key, sound.Value.Volume);
        }
    }
}
