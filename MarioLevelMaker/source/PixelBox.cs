using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MarioLevelMaker.source
{
    public class PixelBox : PictureBox
    {
        public int tileID = 0;
        private int tempID = -1;

        public PixelBox()
        {

        }

        public PixelBox(int x, int y)
        {
            this.Name = "gridSquare_" + x.ToString() + "_" + y.ToString();
            this.Size = new Size(64, 64);
            this.Location = new Point(x * 64, y * 64);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Image = (Image)MarioLevelMaker.Properties.Resources.ResourceManager.GetObject(tileNames[tileID]);
        }

        public void PixelBox_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(this.tileID, DragDropEffects.Copy | DragDropEffects.Move);
        }

        public void PixelBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(int)) != null)
            {
                this.tempID = this.tileID;
                this.tileID = ((int)e.Data.GetData(typeof(int)));
                updateImage();
                e.Effect = DragDropEffects.Copy;
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

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            base.OnPaint(pe);
        }

        public void updateImage()
        {
            this.Image = (Image)Properties.Resources.ResourceManager.GetObject(tileNames[this.tileID]);
            Refresh();
        }

        private static string[] tileNames = new string[] { "empty", "brick", "brick_empty", "brick_falling", "brick_floor", "brick_ice", "brick_invisible", "brick_music", "brick_pow", "brick_question", "brick_solid", "cloud", "coin", "enemy_ball", "enemy_bomb", "enemy_fish", "enemy_ghost", "enemy_goomba", "enemy_helmet", "enemy_mucher", "enemy_spike", "enemy_threespike", "pipe_1", "pipe_2", "pipe_3", "pipe_4", "spikes", "spring", "mario", "mario_jump", "luigi", "luigi_jump" };
        public Level level;

        public static string[] TileNames
        {
            get
            {
                return tileNames;
            }
        }
    }
}
