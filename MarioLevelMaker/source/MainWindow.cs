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
    // Main Window class
    public partial class MainWindow : Form
    {
        // constructor
        public MainWindow()
        {
            InitializeComponent();
        }

        // set up
        private void MainWindow_Load(object sender, EventArgs e)
        {
            // set the object pane as the active control to enable scrolling
            this.ActiveControl = this.TileShelf;


            foreach(Tile currentTile in level.tiles)
            {
                this.LevelPane.Controls.Add(currentTile);
            }

            // Add one of each tile to the tile shelf
            for (int i = 0; i < this.level.tileGraphics.Count; i++)
            {
                Tile newPixelBox = new Tile();
                newPixelBox.MouseDown += new MouseEventHandler(newPixelBox.PixelBox_MouseDown);
                newPixelBox.level = this.level;
                newPixelBox.Name = "shelfTile_" + i.ToString();
                newPixelBox.Size = new Size(64, 64);
                newPixelBox.SizeMode = PictureBoxSizeMode.StretchImage;
                newPixelBox.tileID = i;
                newPixelBox.updateImage();
                this.TileShelf.Controls.Add(newPixelBox);
            };
        }

        // create new level
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (NewFileDialog dialog = new NewFileDialog())
            {
                dialog.ShowDialog();
                if (dialog.DialogResult == DialogResult.OK)
                {
                    createNewLevel(dialog.width, dialog.height);
                }
            }
        }
        
        private void createNewLevel(int width, int height)
        {
            this.LevelPane.Controls.Clear();
            this.level = new Level(width, height);
            foreach (Tile currentTile in level.tiles)
            {
                this.LevelPane.Controls.Add(currentTile);
            }
        }

        // updates the windows title bar with the name of the current file
        public void updateTitleBar()
        {
            // only include the dash if there is a file
            if (level.filePath != "")
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
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|All files (*.*)|*.*";
            dialog.FilterIndex = 4;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap screenshot = new Bitmap(level.levelWidth * 64, level.levelHeight * 64);
                using (Graphics graphics = Graphics.FromImage(screenshot))
                {
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(147, 187, 236)))
                    {
                        graphics.FillRectangle(brush, 0, 0, screenshot.Width, screenshot.Height);
                    }
                    for(int x = 0; x < level.levelWidth; x++)
                    {
                        for (int y = 0; y < level.levelHeight; y++)
                        {
                            Bitmap currentTileGraphic = level.tileGraphics[level.tiles[y * level.levelWidth + x].tileID];
                            Rectangle currentRectangle = new Rectangle(x * 64, y * 64, 64, 64);
                            graphics.DrawImage(currentTileGraphic, currentRectangle);
                        }
                    }
                }
                using (Stream stream = dialog.OpenFile())
                {
                    screenshot.Save(stream, imageFormatOrder[dialog.FilterIndex]);
                }
            }
        }

        // save level
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(level.filePath == "")
            {
                LevelSerializer.SaveLevelAs(level);
            }
            else
            {
                LevelSerializer.SaveLevel(level);
            }
            updateTitleBar();
        }

        // open level
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LevelSerializer.Deserialize(level);
            updateTitleBar();
        }

        // save level as
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LevelSerializer.SaveLevelAs(level);
            updateTitleBar();
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

        Level level = new Level(20, 12);
        Tile[] shelf = new Tile[98];
        ImageFormat[] imageFormatOrder = new ImageFormat[5] { ImageFormat.Bmp, ImageFormat.Jpeg, ImageFormat.Gif, ImageFormat.Png, ImageFormat.Png };

        // adjust to size
        private void MainWindow_Resize(object sender, EventArgs e)
        {
            this.TileShelf.Size = new Size(this.ClientSize.Width - 1280, 768);
        }

        // toggle grid display
        private void displayGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(Tile currentTile in this.level.tiles)
            {
                currentTile.toggleGrid();
            }
        }
    }
}
