using System;
using System.Collections.Generic;
using System.Text;
using AxMC_Realms_Client;

namespace AxMC_Realms_Client.Map
{
    class Map
    {
        public static int MapWidth, MapHeight;
        public static void MapLoad(byte?[,] map)
        {
            MapWidth = map.GetLength(1);
            MapHeight = map.GetLength(0);
            Game1.MapTiles = new Tile[MapWidth * MapHeight];
            Game1.MapBlocks = new Microsoft.Xna.Framework.Matrix[MapWidth * MapHeight];
            //Matrix.CreateTranslation((-3 * 50 + 25) + _sprites[0].Position.X, 5 * 50 - _sprites[0].Position.Y, 0);
            for (int x = 0; x < MapWidth; x++)
                for (int y = 0; y < MapHeight; y++)
                {
                    byte number = map[y, x] ?? 255;
                    if (number == 255) continue;
                    if( number == 5)
                    {
                        Game1.MapBlocks[y * MapWidth + x] = Microsoft.Xna.Framework.Matrix.CreateTranslation(-x * 50 - 25, y*50 +25, 0);
                        continue;
                    }
                    //if(number == null)continue;

                    Game1.MapTiles[y * MapWidth + x] = new Tile()
                    {
                        Rect = new(x * 50, y * 50, 50, 50),
                        DestRect = new(16 * (number % 5), 16 * (number / 5), 16, 16)
                    };
                }
        }
    }
}
