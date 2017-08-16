using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarioLevelMaker.source
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int SizeX = 32;
                    int SizeY = 32;
                    PixelBox picture = new PixelBox();
                    picture.Image = Properties.Resources.brick;
                    picture.Size = new System.Drawing.Size(SizeX, SizeY);
                    picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                    picture.Location = new System.Drawing.Point(i * SizeX, 24 + j * SizeY);
                    picture.Name = "object_brick_" + (i * 2 + j).ToString();
                    picture.Margin = new Padding(0);
                    picture.Padding = new Padding(0);
                    picture.BorderStyle = BorderStyle.None;
                    this.Controls.Add(picture);
                }
            }
        }
    }
}
