using Microsoft.Xna.Framework;
using nekoT;
using System;

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
            Game1.MapBlocks = new Vector2[MapSize.X * MapSize.Y];
            for (byte x = 0; x < MapSize.X; x++)
                for (byte y = 0; y < MapSize.Y; y++)
                {
                    byte number = map[y, x] ?? 255;
                    if (number == 255) continue;
                    if (number == 5)
                    {
                        Game1.MapBlocks[y * MapSize.X + x] = new(x + 0.5f , y + 0.5f );
                        // Matrix.CreateTranslation(-x * 50 - 25f, y * 50 + 25f, -0);
                        continue;
                    }
                    var index = y * MapSize.X + x;
                    Game1.MapTiles[index] = new Tile
                    {
                        X = x,
                        Y = y
                    };
                    Game1.MapTiles[index].DestRect.X = 16 * (number % 5);
                    Game1.MapTiles[index].DestRect.Y = 16 * (number / 5);
                }
            for (int i = 0; i < Game1.MapTiles.Length; i++)
            {
                if (Game1.MapTiles[i] is null)
                {
                    Array.Copy(Game1.MapTiles, i + 1, Game1.MapTiles, i, Game1.MapTiles.Length - 1 - i);
                    i--;
                }
            }
        }
    }
}