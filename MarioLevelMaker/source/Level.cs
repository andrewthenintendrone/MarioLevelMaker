using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Drawing;

namespace MarioLevelMaker.source
{
    public class Level
    {
        // default constructor sets all tile ids to 0
        public Level()
        {
            // obtain graphics from spritesheet
            Bitmap blankBitmap = new Bitmap(64, 64);
            tileGraphics.Add(blankBitmap);
            for(int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    tileGraphics.Add(ImageTiler.tileFromSpriteSheet(x, y));
                }
            }

            // create tiles
            for (int y = 0; y < levelHeight; y++)
            {
                for (int x = 0; x < levelWidth; x++)
                {
                    Tile newTile = new Tile(x, y);
                    ((Control)newTile).AllowDrop = true;
                    newTile.MouseDown += new MouseEventHandler(newTile.PixelBox_MouseDown);
                    newTile.DragEnter += new DragEventHandler(newTile.PixelBox_DragEnter);
                    newTile.DragLeave += new EventHandler(newTile.PixelBox_DragLeave);
                    newTile.DragDrop += new DragEventHandler(newTile.PixelBox_DragDrop);
                    newTile.tileID = 1;
                    newTile.level = this;
                    newTile.updateImage();
                    tiles.Add(newTile);
                }
            }
        }

        // copys data from another level to this one
        public void copyData(List<int> data)
        {
            for(int i = 0; i < levelHeight * levelWidth; i++)
            {
                tiles[i].tileID = data[i];
                tiles[i].updateImage();
            }
        }

        // undoes a change to the level
        public void UndoAction()
        {
            if(actionQueue.Count > 0)
            {
                actionQueue[queuePos].pixelBox.tileID = actionQueue[queuePos].previousState;
                actionQueue[queuePos].pixelBox.updateImage();
                queuePos--;
                if (queuePos < 0)
                {
                    queuePos = 0;
                }
            }
        }

        // redoes a change to the level
        public void RedoAction()
        {
            if(actionQueue.Count > 0)
            {
                queuePos++;
                if (queuePos > actionQueue.Count - 1)
                {
                    queuePos = actionQueue.Count - 1;
                }
                actionQueue[queuePos].pixelBox.tileID = actionQueue[queuePos].currentState;
                actionQueue[queuePos].pixelBox.updateImage();
            }
        }

        public string filePath = "";
        public bool binaryFileMode = true;
        public int levelWidth = 40;
        public int levelHeight = 24;
        public List<Bitmap> tileGraphics = new List<Bitmap>();
        public List<Tile> tiles = new List<Tile>();
        public int queuePos = -1;
        public List<Action> actionQueue = new List<Action>();
    }
}
