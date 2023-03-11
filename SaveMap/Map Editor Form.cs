using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SaveMap
{
    public enum EditAction
    {
        Save, Load, Nothing
    }

    public enum MapType
    {
        Path, Forest, Mountain, Shrubland, Snow
    }

    public partial class SaveMapForm : Form
    {
        public string FileDirectory { get; private set; }
        public EditAction editAction;
        public MapType MapType { get; private set; }

        public SaveMapForm()
        {
            InitializeComponent();
            editAction = EditAction.Nothing;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }


        //Skriver nivån till en fil
        private void SaveMapButton_Click(object sender, EventArgs e)
        {
            if (FileDirectory != null)
            {
                editAction = EditAction.Save;
                Hide();
            }
        }

        //Läser nivån från en fil
        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (FileDirectory != null)
            {
                editAction = EditAction.Load;
                Hide();
            }
        }


        //Väljer skogsstigens fil
        private void ForestPath_Click(object sender, EventArgs e)
        {
            FileDirectory = @"Content\Txt\Paths\ForestPath.txt";
            MapType = MapType.Path;
        }
        //Väljer snöstigens fil
        private void SnowPath_Click(object sender, EventArgs e)
        {
            FileDirectory = @"Content\Txt\Paths\SnowPath.txt";
            MapType = MapType.Path;
        }
        //Väljer högra buskskogsstigens fil
        private void ShrubPathRight_Click(object sender, EventArgs e)
        {
            FileDirectory = @"Content\Txt\Paths\ShrublandPathRight.txt";
            MapType = MapType.Path;
        }
        //Väljer vänstra buskskogsstigens fil
        private void ShrubPathLeft_Click(object sender, EventArgs e)
        {
            FileDirectory = @"Content\Txt\Paths\ShrublandPathLeft.txt";
            MapType = MapType.Path;
        }
        //Väljer övre bergsstigens fil
        private void MountainPathUpper_Click(object sender, EventArgs e)
        {
            FileDirectory = @"Content\Txt\Paths\MountainPathUpper.txt";
            MapType = MapType.Path;
        }
        //Väljer undre bergsstigens fil
        private void MountainPathLower_Click(object sender, EventArgs e)
        {
            FileDirectory = @"Content\Txt\Paths\MountainPathLower.txt";
            MapType = MapType.Path;
        }
        //Väljer skogskartans fil
        private void ForestMapButton_CheckedChanged(object sender, EventArgs e)
        {
            FileDirectory = @"Content\Txt\Maps\ForestMap.txt";
            MapType = MapType.Forest;
        }
        //Väljer snökartans fil
        private void SnowMapButton_CheckedChanged(object sender, EventArgs e)
        {
            FileDirectory = @"Content\Txt\Maps\SnowMap.txt";
            MapType = MapType.Snow;
        }
        //Väljer bergskartans fil
        private void MountainMapButton_CheckedChanged(object sender, EventArgs e)
        {
            FileDirectory = @"Content\Txt\Maps\MountainMap.txt";
            MapType = MapType.Mountain;
        }
        //Väljer buskskogskartans fil
        private void ShrubMapButton_CheckedChanged(object sender, EventArgs e)
        {
            FileDirectory = @"Content\Txt\Maps\ShrublandMap.txt";
            MapType = MapType.Shrubland;
        }
    }
}
