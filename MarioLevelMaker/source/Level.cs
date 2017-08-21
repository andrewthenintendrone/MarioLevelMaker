﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace MarioLevelMaker.source
{
    public class Level
    {
        // constant level size
        public const int levelWidth = 20;
        public const int levelHeight = 12;
        private string filePath = "";

        // array of ints for storing tile ids
        public int[] tileIDs = new int[levelWidth * levelHeight];

        // default constructor sets all tile ids to 0
        public Level()
        {
            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    tileIDs[y * levelWidth + x] = 0;
                }
            }
        }

        // serializes the level
        public void Serialize(bool saveAs)
        {   
            if(saveAs || filePath == "")
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Mario Levels (*.xml)|*.xml";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = dialog.FileName;
                }
            }

            XmlSerializer mySerializer = new XmlSerializer(typeof(Level));
            StreamWriter streamWriter = new StreamWriter(filePath);
            mySerializer.Serialize(streamWriter, this);
            streamWriter.Close();
        }

        // deserializes the level
        public void Deserialize()
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(Level));
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Mario Levels (*.xml)|*.xml";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    Level deserializedLevel = (Level)mySerializer.Deserialize(stream);
                    this.tileIDs = deserializedLevel.tileIDs;
                    this.filePath = dialog.FileName;
                }
            }
        }

        // allow access to fileName without making it public
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
            }
        }
    }
}
