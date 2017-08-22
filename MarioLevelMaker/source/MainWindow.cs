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
            foreach(PixelBox currentTile in level.tiles)
            {
                this.LevelPane.Controls.Add(currentTile);
            }

            // set up shelf
            for (int i = 0; i < PixelBox.TileNames.Length; i++)
            {
                PixelBox newPixelBox = new PixelBox();
                newPixelBox.MouseDown += new MouseEventHandler(newPixelBox.PixelBox_MouseDown);
                newPixelBox.Name = "shelfTile_" + PixelBox.TileNames[i];
                newPixelBox.Size = new Size(64, 64);
                newPixelBox.SizeMode = PictureBoxSizeMode.StretchImage;
                newPixelBox.tileID = i;
                newPixelBox.updateImage();
                this.ObjectPane.Controls.Add(newPixelBox);
            };
        }

        // create new level
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            level.filePath = "";
            foreach(PixelBox currentPixelBox in level.tiles)
            {
                currentPixelBox.tileID = 0;
                currentPixelBox.updateImage();
            }
            this.level.actionQueue.Clear();
            this.level.queuePos = -1;
            updateLevel();
        }
        
        // updates all pixel boxes to match the level
        // displays the level file path in the title bar
        public void updateLevel()
        {
            // update title bar with file name
            if(level.filePath != "")
            {
                this.Text = "MarioLevelMaker - " + level.filePath;
            }
            else
            {
                this.Text = "MarioLevelMaker";
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
            // update level just in case
            updateLevel();

            Bitmap screenshot = new Bitmap(Level.LevelWidth * 64, Level.LevelHeight * 64);
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
            level.Serialize(false);
            updateLevel();
        }

        // open level
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            level.Deserialize();
            updateLevel();
        }

        // save level as
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            level.Serialize(true);
            updateLevel();
        }

        // undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.level.UndoAction();
        }

        // redo
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.level.RedoAction();
        }

        Level level = new Level();
        PixelBox[] shelf = new PixelBox[PixelBox.TileNames.Length];
        ImageFormat[] imageFormatOrder = new ImageFormat[5] { ImageFormat.Bmp, ImageFormat.Jpeg, ImageFormat.Gif, ImageFormat.Png, ImageFormat.Png };

        // adjust to size
        private void MainWindow_Resize(object sender, EventArgs e)
        {
            this.ObjectPane.Size = new Size(this.ClientSize.Width - 1280, this.ClientSize.Height);
        }

        // draw gridLines on the level pane
        public void LevelPane_Paint(object sender, PaintEventArgs e)
        {
            Pen myPen = new Pen(Color.FromArgb(255, 50, 97, 168), 2);
            for (int y = 0; y < Level.LevelHeight * 64; y += 64)
            {
                e.Graphics.DrawLine(myPen, new Point(0, y), new Point(Level.LevelWidth * 64, y));
            }
            for (int x = 0; x < Level.LevelWidth * 64; x += 64)
            {
                e.Graphics.DrawLine(myPen, new Point(x, 0), new Point(x, Level.LevelHeight * 64));
            }
            base.OnPaint(e);
        }

        // toggle grid display
        private void displayGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(PixelBox currentTile in this.level.tiles)
            {
                currentTile.toggleGrid();
            }
        }
    }
}
