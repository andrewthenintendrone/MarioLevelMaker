using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;

namespace MarioLevelMaker.source
{
    class LevelSerializer
    {
        // save the level with a file picker
        public static void SaveLevelAs(Level level)
        {
            // store level size and tile IDs in a new list
            List<int> sizeAndData = new List<int>();
            sizeAndData.Add(level.levelWidth);
            sizeAndData.Add(level.levelHeight);
            foreach (Tile currentTile in level.tiles)
            {
                sizeAndData.Add(currentTile.tileID);
            }

            // open a file picker
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Mario Level Maker Levels (*.mlm)|*.mlm|Mario Level Maker xml Levels (*.xml)|*.xml";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;
            // if the user picks a location
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // open the chosen level file
                using (Stream stream = dialog.OpenFile())
                {
                    // tell the level its new filepath
                    level.filePath = dialog.FileName;

                    // write .mlm file format
                    if (dialog.FilterIndex == 1)
                    {
                        level.binaryFileMode = true;
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        binaryFormatter.Serialize(stream, sizeAndData);
                    }
                    // write .xml file format
                    else if (dialog.FilterIndex == 2)
                    {
                        level.binaryFileMode = false;
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(int[]));
                        xmlSerializer.Serialize(stream, sizeAndData);
                    }
                    // close the level file
                    stream.Close();
                }
            }
        }

        // save the level without a file picker
        public static void SaveLevel(Level level)
        {
            // store level size and tile IDs in a new list
            List<int> sizeAndData = new List<int>();
            sizeAndData.Add(level.levelWidth);
            sizeAndData.Add(level.levelHeight);
            foreach (Tile currentTile in level.tiles)
            {
                sizeAndData.Add(currentTile.tileID);
            }

            // open the level file
            using (FileStream stream = new FileStream(level.filePath, FileMode.Truncate))
            {
                // write differently based on whether the level file mode
                if (level.binaryFileMode)
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(stream, sizeAndData);
                }
                else
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(int[]));
                    xmlSerializer.Serialize(stream, sizeAndData);
                }
                // close the level file
                stream.Close();
            }
        }

        // loads the level and
        public static void Deserialize(Level level)
        {
            // create new list to store level size and tile data
            List<int> sizeAndData = new List<int>();

            // open a file picker
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Mario Level Maker Levels (*.mlm)|*.mlm|Mario Level Maker xml Levels (*.xml)|*.xml";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;

            // if the user picks a file
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // open the level file
                using (Stream stream = dialog.OpenFile())
                {
                    // read differently based on the file type
                    if (dialog.FilterIndex == 1)
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        sizeAndData = (List<int>)binaryFormatter.Deserialize(stream);
                    }
                    else if (dialog.FilterIndex == 2)
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(int[]));
                        sizeAndData = (List<int>)xmlSerializer.Deserialize(stream);
                    }
                    // close the file
                    stream.Close();
                }
            }
        }
    }
}
