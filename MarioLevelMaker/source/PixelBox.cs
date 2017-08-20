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
        public PixelBox()
        {

        }

        public PixelBox(int x, int y)
        {
            this.Name = "gridSquare_" + x.ToString() + "_" + y.ToString();
            this.Size = new Size(64, 64);
            this.Location = new Point(x * 64, y * 64);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Image = MarioLevelMaker.Properties.Resources.empty;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            base.OnPaint(pe);
        }
    }
}
