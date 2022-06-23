using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Map
{
    public class Tile
    {
        public static Texture2D TileSet;
        public Rectangle SrcRect = new(16, 0, 16, 16);
        public Tile(int id)
        {
            SrcRect.X *= (id % 7); // 7 is the amount of tiles on MCRTile
        }
    }
}