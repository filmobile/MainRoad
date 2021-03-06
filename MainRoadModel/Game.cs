﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using MainRoadModel.Model;

namespace MainRoadModel
{
    public static class Game
    {
        /// <summary>
        /// State
        /// </summary>
        public static GameState State { get; set; }

        public static CarsController CarsController { get; set; }

        /// <summary>
        /// Size of tile grid
        /// </summary>
        public const int GRID_SIZE = 120;
        /// <summary>
        /// Size of cell in meters
        /// </summary>
        public const float METERS_PER_UNIT = 12.8f;
        /// <summary>
        /// Tile size in pixels
        /// </summary>
        public const float TILE_SIZE = 128f;
        /// <summary>
        /// Node step (GRID_SIZE must be divided by NODE_STEP)
        /// </summary>
        public const int NODE_STEP = 6;

        /// <summary>
        /// Total car count on the roads
        /// </summary>
        public static int CarCount = 500;

        const string DEF_FILENAME = "mainroad.bin";

        public static void Load(string name = DEF_FILENAME)
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + name;
            if (File.Exists(file))
                try
                {
                    using (var str = File.OpenRead(file))
                    {
                        //load saved state
                        State = (GameState)new BinaryFormatter().Deserialize(str);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Write(ex);
                }
        }

        public static void Save(string name = DEF_FILENAME)
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + name;
            try
            {
                using (var str = File.OpenWrite(file))
                {
                    new BinaryFormatter().Serialize(str, State);
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }
    }
}
