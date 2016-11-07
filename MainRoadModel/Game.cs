using System;
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
    static class Game
    {
        public static GameState State { get; set; }
        /// <summary>
        /// Size of tile grid
        /// </summary>
        public const int GRIDSIZE = 256;
        /// <summary>
        /// Size of cell in meters
        /// </summary>
        public const float METERS_PER_UNIT = 12.8f;

        public const string DEF_FILENAME = "mainroad.bin";

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
