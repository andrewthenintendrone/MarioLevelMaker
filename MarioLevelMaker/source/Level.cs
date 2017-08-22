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
            // split spritesheet into graphics
            Bitmap blankBitmap = new Bitmap(64, 64);
            tileGraphics.Add(blankBitmap);
            for(int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    tileGraphics.Add(ImageTiler.tileFromSpriteSheet(x, y));
                }
            }

            tiles = new Tile[levelWidth * levelHeight];

            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    Tile newTile = new Tile(x, y);
                    ((Control)newTile).AllowDrop = true;
                    newTile.MouseDown += new MouseEventHandler(newTile.PixelBox_MouseDown);
                    newTile.DragEnter += new DragEventHandler(newTile.PixelBox_DragEnter);
                    newTile.DragLeave += new EventHandler(newTile.PixelBox_DragLeave);
                    newTile.DragDrop += new DragEventHandler(newTile.PixelBox_DragDrop);
                    newTile.tileID = 0;
                    newTile.level = this;
                    newTile.updateImage();
                    tiles[y * levelWidth + x] = newTile;
                }
            }
        }

        // serializes the level
        public void Serialize(bool saveAs)
        {   
            if(saveAs || filePath == "")
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Mario Level Maker Levels (*.mlm)|*.mlm|Mario Level Maker xml Levels (*.xml)|*.xml";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        int[] tileIDs = new int[tiles.Length];
                        for (int i = 0; i < tiles.Length; i++)
                        {
                            tileIDs[i] = tiles[i].tileID;
                        }
                        if(dialog.FilterIndex == 1)
                        {
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            binaryFormatter.Serialize(stream, tileIDs);
                        }
                        else if(dialog.FilterIndex == 2)
                        {
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(int[]));
                            xmlSerializer.Serialize(stream, tileIDs);
                        }
                        stream.Close();
                    }
                }
                else
                {
                    return;
                }
            }
        }

        // deserializes the level
        public void Deserialize()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Mario Level Maker Levels (*.mlm)|*.mlm|Mario Level Maker xml Levels (*.xml)|*.xml";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    int[] deserializedTileIDs = new int[tiles.Length];
                    this.filePath = dialog.FileName;
                    if (dialog.FilterIndex == 1)
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        deserializedTileIDs = (int[])binaryFormatter.Deserialize(stream);
                    }
                    else if(dialog.FilterIndex == 2)
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(int[]));
                        deserializedTileIDs = (int[])xmlSerializer.Deserialize(stream);
                    }
                    stream.Close();
                    for (int i = 0; i < tiles.Length; i++)
                    {
                        tiles[i].tileID = deserializedTileIDs[i];
                        tiles[i].updateImage();
                    }
                    this.actionQueue.Clear();
                    this.queuePos = -1;
                    stream.Close();
                }
            }
        }

        public static int LevelWidth
        {
            get
            {
                return levelWidth;
            }
        }

        public static int LevelHeight
        {
            get
            {
                return levelHeight;
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

        // constant level size
        private const int levelWidth = 20;
        private const int levelHeight = 12;
        public List<Bitmap> tileGraphics = new List<Bitmap>();
        public Tile[] tiles;
        public int queuePos = -1;
        public List<Action> actionQueue = new List<Action>();
    }
}
