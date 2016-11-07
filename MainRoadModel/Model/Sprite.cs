using System;
using System.Drawing;

namespace MainRoadModel.Model
{
    [Serializable]
    class Sprite
    {
        public string Name;

        [NonSerialized]
        public Bitmap Bitmap;
    }
}