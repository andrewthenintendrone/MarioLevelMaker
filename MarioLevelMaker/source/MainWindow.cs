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

namespace MarioLevelMaker.source
{
    ImageFormat ImageFormatOrder(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 1:
                return ImageFormat.Png;
            case 2:
                return ImageFormat.Jpeg;
            case 3:
                return ImageFormat.Gif;
            case 4:
                return ImageFormat.Bmp;
        }
    }

    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            // set up grid
            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    PixelBox newTile = new PixelBox(x, y);
                    newTile.Image = MarioLevelMaker.Properties.Resources.brick;
                    this.LevelPane.Controls.Add(newTile);
                }
            }
            // set up shelf
            for (int i = 0; i < m_objectNames.Length; i++)
            {
                PixelBox newPixelBox = new PixelBox();
                newPixelBox.Name = "shelfTile_" + m_objectNames[i];
                newPixelBox.Size = new Size(64, 64);
                newPixelBox.SizeMode = PictureBoxSizeMode.StretchImage;
                newPixelBox.Image = (Image)Properties.Resources.ResourceManager.GetObject(m_objectNames[i]);
                this.ObjectPane.Controls.Add(newPixelBox);
            }
        }

        const int levelWidth = 20;
        const int levelHeight = 12;
        string[] m_objectNames = new string[] { "empty", "brick", "brick_question", "brick_solid", "brick_music", "brick_empty", "coin" };

        // reset grid tiles
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    foreach(PixelBox currentPixelBox in this.LevelPane.Controls.Find("gridSquare_" + x.ToString() + "_" + y.ToString(), false))
                    {
                        currentPixelBox.Image = Properties.Resources.empty;
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
            Bitmap screenshot = new Bitmap(levelWidth * 64, levelHeight * 64);
            this.LevelPane.DrawToBitmap(screenshot, this.LevelPane.ClientRectangle);
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "png files (*.png)|*.png|jpeg files (*.jpg)|*.jpg|gif files (*.gif)|*.gif|bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;

            ImageFormat selectedFormat;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                
                // Can use dialog.FileName
                using (Stream stream = dialog.OpenFile())
                {
                    screenshot.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
        }
    }
}
