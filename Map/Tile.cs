using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Map
{
    public class Tile
    {
        public static Texture2D TileSet;
        public static Rectangle[] SRects = new Rectangle[byte.MaxValue];
        public byte Lightness;
        public static void Initialize()
        {
            for (int i = 0; i < byte.MaxValue; i++)
            {
                SRects[i] = new(16 * i, 0, 16, 16);
            }
        }
        public Tile()
        {
            //SrcRect.X *= id % 7; // 7 is the amount of tiles on MCRTile
        }
    }
}