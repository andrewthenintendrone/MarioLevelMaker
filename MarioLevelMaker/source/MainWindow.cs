using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace MarioLevelMaker.source
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // set up window
        private void MainWindow_Load(object sender, EventArgs e)
        {
            // set up grid
            for (int x = 0; x < Level.levelWidth; x++)
            {
                for (int y = 0; y < Level.levelHeight; y++)
                {
                    PixelBox newTile = new PixelBox(x, y);
                    newTile.Image = (Image)MarioLevelMaker.Properties.Resources.ResourceManager.GetObject(objectNames[level.tileIDs[y * Level.levelWidth + x]]);
                    this.LevelPane.Controls.Add(newTile);
                }
            }
            // set up shelf
            for (int i = 0; i < objectNames.Length; i++)
            {
                PixelBox newPixelBox = new PixelBox();
                newPixelBox.Name = "shelfTile_" + objectNames[i];
                newPixelBox.Size = new Size(64, 64);
                newPixelBox.SizeMode = PictureBoxSizeMode.StretchImage;
                newPixelBox.Image = (Image)Properties.Resources.ResourceManager.GetObject(objectNames[i]);
                this.ObjectPane.Controls.Add(newPixelBox);
            }
        }

        // reset grid tiles
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            level = new Level();
            updatePixelBoxes();
        }
        
        // updates all pixel boxes to match the level
        private void updatePixelBoxes()
        {
            for (int x = 0; x < Level.levelWidth; x++)
            {
                for (int y = 0; y < Level.levelHeight; y++)
                {
                    foreach (PixelBox currentPixelBox in this.LevelPane.Controls.Find("gridSquare_" + x.ToString() + "_" + y.ToString(), false))
                    {
                        currentPixelBox.Image = (Image)Properties.Resources.ResourceManager.GetObject(objectNames[level.tileIDs[y * Level.levelWidth + x]]);
                    }
                }
            }
        }

        // exit the program
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // save a screenshot
        private void takeScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap screenshot = new Bitmap(Level.levelWidth * 64, Level.levelHeight * 64);
            this.LevelPane.DrawToBitmap(screenshot, this.LevelPane.ClientRectangle);
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|All files (*.*)|*.*";
            dialog.FilterIndex = 4;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    screenshot.Save(stream, imageFormatOrder[dialog.FilterIndex]);
                }
            }
        }

        // save level
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            level.Serialize();
        }

        // open level
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            level.Deserialize();
            updatePixelBoxes();
        }

        Level level = new Level();
        ImageFormat[] imageFormatOrder = new ImageFormat[5] { ImageFormat.Bmp, ImageFormat.Jpeg, ImageFormat.Gif, ImageFormat.Png, ImageFormat.Png };
        string[] objectNames = new string[] { "empty", "brick", "brick_question", "brick_solid", "brick_music", "brick_empty", "coin" };
    }
}
