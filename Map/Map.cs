using Microsoft.Xna.Framework;
using nekoT;
using System;
using System.IO;
using System.Text.Json;

namespace AxMC_Realms_Client.Map
{
    class Map
    {
        public static Point MapSize;
        private static void MapWriter(byte[] map, int width)
        {
            using (FileStream stream = File.OpenWrite("map.json"))
            {
                Utf8JsonWriter writer = new(stream);
                writer.WriteStartObject();
                writer.WritePropertyName("Data");
                JsonSerializer.Serialize(writer, map);
                writer.WriteNumber("width", width);
                writer.WriteEndObject();
                writer.Flush();
                stream.Close();
            }
        }
        public static void MapLoad(byte[] map, int width)
        {
            
            using (JsonDocument jsonData = JsonDocument.Parse(File.ReadAllText("map.json")))
            {
                JsonElement root = jsonData.RootElement;
                map = JsonSerializer.Deserialize<byte[]>(root.GetProperty("Data").GetRawText());
                width = root.GetProperty("width").GetInt32();
            }
            MapSize.X = width;
            MapSize.Y = map.Length / width;
            Game1.MapTiles = new Tile[MapSize.X * MapSize.Y];
            Game1.MapBlocks = new Vector2[MapSize.X * MapSize.Y];
            for (int x = 0; x < MapSize.X; x++)
                for (int y = 0; y < MapSize.Y; y++)
                {
                    byte number = map[y * MapSize.X + x];
                    if (number == 255) continue;
                    if (number == 5)
                    {
                        Game1.MapBlocks[y * MapSize.X + x] = new(x + 0.5f , y + 0.5f);
                        // Matrix.CreateTranslation(-x * 50 - 25f, y * 50 + 25f, -0);
                        continue;
                    }
                    var index = y * MapSize.X + x;
                    Game1.MapTiles[index] = new Tile();
                    Game1.MapTiles[index].SrcRect.X = 16 * (number % 5);
                    Game1.MapTiles[index].SrcRect.Y = 16 * (number / 5);
                }
            /*for (int i = 0; i < Game1.MapTiles.Length; i++)
            {
                if (Game1.MapTiles[i] is null)
                {
                    Array.Copy(Game1.MapTiles, i + 1, Game1.MapTiles, i, Game1.MapTiles.Length - 1 - i);
                    i--;
                }
            }*/
        }
    }
}