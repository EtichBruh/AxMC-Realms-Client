using System;
using System.Collections.Generic;
using System.Text;
using AxMC_Realms_Client;
using Microsoft.Xna.Framework;

namespace AxMC_Realms_Client.Map
{
    class Map
    {
        public static Point MapSize;
        public static void MapLoad(byte?[,] map)
        {
            MapSize.X = map.GetLength(1);
            MapSize.Y = map.GetLength(0);
            Game1.MapTiles = new Tile[MapSize.X * MapSize.Y];
            Game1.MapBlocks = new Microsoft.Xna.Framework.Matrix[MapSize.X * MapSize.Y];
            for (int x = 0; x < MapSize.X; x++)
                for (int y = 0; y < MapSize.Y; y++)
                {
                    byte number = map[y, x] ?? 255;
                    if (number == 255) continue;
                    if( number == 5)
                    {
                        Game1.MapBlocks[y * MapSize.X + x] = Microsoft.Xna.Framework.Matrix.CreateTranslation(-x * 50 - 25, -y * 50 - 25, -0);
                        continue;
                    }
                    var index = y * MapSize.X + x;
                    Game1.MapTiles[index].Rect.X = x * 50;
                    Game1.MapTiles[index].Rect.Y = y * 50;
                    Game1.MapTiles[index].Rect.Width = 50;
                    Game1.MapTiles[index].Rect.Height = Game1.MapTiles[index].Rect.Width;
                    Game1.MapTiles[index].DestRect.X = 16 * (number % 5);
                    Game1.MapTiles[index].DestRect.Y =  16 * (number / 5);
                    Game1.MapTiles[index].DestRect.Width =16;
                    Game1.MapTiles[index].DestRect.Height = Game1.MapTiles[index].DestRect.Width;
                }
        }
    }
}
