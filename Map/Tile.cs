using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Map
{
    public class Tile
    {
        public static Texture2D TileSet;
        public static Vector2 SharedPos = Vector2.Zero;
        public Rectangle SrcRect = new(0,0,16,16);
    }
}