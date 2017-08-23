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
                    tiles.Add(newTile);
                }
            }
        }

        public void copyData(List<int> data)
        {
            levelHeight = data[0];
            levelWidth = data[1];
            for(int i = 0; i < levelHeight * levelWidth; i++)
            {
                tiles[i].tileID = data[2 + i];
                tiles[i].updateImage();
            }
        }

        // serializes the level
        public void Serialize(bool saveAs)
        {
            // store level size and data in a new list
            List<int> sizeAndData = new List<int>();
            sizeAndData.Add(levelWidth);
            sizeAndData.Add(levelHeight);

            foreach (Tile currentTile in tiles)
            {
                sizeAndData.Add(currentTile.tileID);
            }

            // save as
            if (saveAs || filePath == "")
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Mario Level Maker Levels (*.mlm)|*.mlm|Mario Level Maker xml Levels (*.xml)|*.xml";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        this.filePath = dialog.FileName;

                        if(dialog.FilterIndex == 1)
                        {
                            binaryFileMode = true;
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            binaryFormatter.Serialize(stream, sizeAndData);
                        }
                        else if(dialog.FilterIndex == 2)
                        {
                            binaryFileMode = false;
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(int[]));
                            xmlSerializer.Serialize(stream, sizeAndData);
                        }
                        stream.Close();
                    }
                }
                else
                {
                    return;
                }
            }
            // regular save
            else
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Truncate))
                {
                    if (binaryFileMode)
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        binaryFormatter.Serialize(stream, sizeAndData);
                    }
                    else
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(int[]));
                        xmlSerializer.Serialize(stream, sizeAndData);
                    }
                    stream.Close();
                }
            }
        }

        // deserializes the level
        public void Deserialize()
        {
            // create new list to store level size and tile data
            List<int> sizeAndData = new List<int>();

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Mario Level Maker Levels (*.mlm)|*.mlm|Mario Level Maker xml Levels (*.xml)|*.xml";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    this.filePath = dialog.FileName;
                    if (dialog.FilterIndex == 1)
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        copyData((List<int>)binaryFormatter.Deserialize(stream));
                    }
                    else if(dialog.FilterIndex == 2)
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(int[]));
                        copyData((List<int>)xmlSerializer.Deserialize(stream));
                    }
                    stream.Close();
                    this.actionQueue.Clear();
                    this.queuePos = -1;
                    stream.Close();
                }
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

        public int levelWidth = 20;
        public int levelHeight = 12;
        public List<Bitmap> tileGraphics = new List<Bitmap>();
        public List<Tile> tiles = new List<Tile>();
        public int queuePos = -1;
        public List<Action> actionQueue = new List<Action>();
    }
}
