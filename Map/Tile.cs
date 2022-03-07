using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxMC_Realms_Client.Map
{
    public class Tile
    {
        public static Texture2D TileSet;
        public static Vector2 SharedPos = Vector2.Zero;
        public byte X = 0;
        public byte Y = 0;
        public Rectangle DestRect = new(0,0,16,16);
    }
}
