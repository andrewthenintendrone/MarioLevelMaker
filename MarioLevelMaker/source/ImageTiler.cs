using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MarioLevelMaker.source
{
    class ImageTiler
    {
        public static Bitmap tileFromSpriteSheet(int x, int y)
        {
            int tileSizeX = 64;
            int tileSizeY = 64;
            Bitmap src = (Bitmap)Properties.Resources.ResourceManager.GetObject("mariotiles_final");
            Bitmap tile = src.Clone(new Rectangle(tileSizeX * x, tileSizeY * y, tileSizeX, tileSizeY), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            return tile;
        }
    }
}
