using Microsoft.Xna.Framework;
using AxMC_Realms_Client;
using System.IO;
using System.Text.Json;

namespace Map
{
    public static class Map
    {
        public static Point MapSize;
        static byte[] byteMap;
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
        public static void MapLoad(string path)
        {
            
            using (JsonDocument jsonData = JsonDocument.Parse(File.ReadAllText(path)))
            {
                JsonElement root = jsonData.RootElement;
                byteMap = JsonSerializer.Deserialize<byte[]>(root.GetProperty("Data").GetRawText());
                MapSize.X = root.GetProperty("width").GetInt32();
            }
            MapSize.Y = byteMap.Length / MapSize.X;
            Game1.MapTiles = new Tile[MapSize.X * MapSize.Y];
            Game1.MapBlocks = new Vector2[MapSize.X * MapSize.Y];
            for (int x = 0; x < MapSize.X; x++)
                for (int y = 0; y < MapSize.Y; y++)
                {
                    byte number = byteMap[y * MapSize.X + x];
                    if (number == 255) continue;
                    if (number == 5)
                    {
                        Game1.MapBlocks[y * MapSize.X + x] = new(x + 0.5f , y + 0.5f);
                        continue;
                    }
                    var index = y * MapSize.X + x;
                    Game1.MapTiles[index] = new Tile();
                    Game1.MapTiles[index].SrcRect.X = 16 * (number % 5);
                    Game1.MapTiles[index].SrcRect.Y = 16 * (number / 5);
                }
        }
    }
}