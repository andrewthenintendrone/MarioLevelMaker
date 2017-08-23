using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MarioLevelMaker.source
{
    public class Tile : PictureBox
    {
        const int borderWidth = 2;
        public int tileID = 0;
        private int tempID = 0;

        public Tile()
        {

        }

        public Tile(int x, int y)
        {
            this.Name = "gridSquare_" + x.ToString() + "_" + y.ToString();
            this.Size = new Size(64, 64);
            this.Location = new Point(x * 64, y * 64);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        public void PixelBox_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(this.tileID, DragDropEffects.Copy | DragDropEffects.Move);
        }

        public void PixelBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(int)))
            {
                tempID = tileID;
                tileID = ((int)e.Data.GetData(typeof(int)));
                updateImage();
                e.Effect = DragDropEffects.Copy;

                if (Control.ModifierKeys == Keys.Shift)
                {
                    if ((int)e.Data.GetData(typeof(int)) != tempID)
                    {
                        if (level.queuePos < level.actionQueue.Count - 1)
                        {
                            level.actionQueue.RemoveRange(level.queuePos + 1, level.actionQueue.Count - level.queuePos - 1);
                        }
                        this.tileID = (int)e.Data.GetData(typeof(int));
                        level.actionQueue.Add(new Action(this, this.tempID, this.tileID));
                        level.queuePos++;
                        this.tempID = this.tileID;
                        updateImage();
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        public void PixelBox_DragLeave(object sender, EventArgs e)
        {
            if(this.tempID != -1)
            {
                this.tileID = this.tempID;
                this.tempID = -1;
                updateImage();
            }
        }

        public void PixelBox_DragDrop(object sender, DragEventArgs e)
        {
            if((int)e.Data.GetData(typeof(int)) != tempID)
            {
                if (level.queuePos < level.actionQueue.Count - 1)
                {
                    level.actionQueue.RemoveRange(level.queuePos + 1, level.actionQueue.Count - level.queuePos - 1);
                }
                this.tileID = (int)e.Data.GetData(typeof(int));
                level.actionQueue.Add(new Action(this, this.tempID, this.tileID));
                level.queuePos++;
                this.tempID = this.tileID;
                updateImage();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            base.OnPaint(e);
        }

        // turns grid on and off
        public void toggleGrid()
        {
            BorderStyle = (BorderStyle == BorderStyle.None ? BorderStyle.Fixed3D : BorderStyle.None);
        }

        public void updateImage()
        {
            this.Image = level.tileGraphics[tileID];
            Refresh();
        }

        public Level level;
    }
}
