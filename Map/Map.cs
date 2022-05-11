using AxMC_Realms_Client;
using Microsoft.Xna.Framework;
using System.IO;
using System.Text.Json;

namespace Map
{
    public static class Map
    {
        /// <summary>
        /// X is map width, Y is map height
        /// </summary>
        public static Point Size;
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
        public static void Load(string path)
        {
            using (JsonDocument jsonData = JsonDocument.Parse(File.ReadAllText(path)))
            {
                byteMap = JsonSerializer.Deserialize<byte[]>(jsonData.RootElement.GetProperty("Data").GetRawText());
                Size.X = jsonData.RootElement.GetProperty("width").GetInt32();
            }
            Size.Y = byteMap.Length / Size.X;
            Game1.MapTiles = new Tile[byteMap.Length];
            Game1.MapBlocks = new Vector2[byteMap.Length];
            for (int i = 0; i < byteMap.Length; i++)
            {
                byte number = byteMap[i];
                if (number == 255) continue;
                if (number == 5)
                {
                    Game1.MapBlocks[i].X = 1;
                    Game1.MapBlocks[i].Y = 1;
                    continue;
                }
                if (Game1.MapTiles[i] != null)
                {
                    Game1.MapTiles[i].SrcRect.X = 16 * (number % 5); // 16 is the px width of 1 tile
                    Game1.MapTiles[i].SrcRect.Y = 16 * (int)(number * .2f);
                }
                else
                {
                    Game1.MapTiles[i] = new Tile();
                    Game1.MapTiles[i].SrcRect.X = 16 * (number % 5); // 16 is the px width of 1 tile
                    Game1.MapTiles[i].SrcRect.Y = 16 * (int)(number * .2f);
                }

            }
        }
    }
}
/*// 16 is the px height of 1 tile
            int index
            for (int x = 0; x < Size.X; x++)
                for (int y = 0; y < Size.Y; y++)
                {
                    index = y * Size.X + x;
                    byte number = byteMap[index];
                    if (number == 255) continue;
                    if (number == 5)
                    {
                        Game1.MapBlocks[index].X = x + 0.5f;
                        Game1.MapBlocks[index].Y = y + 0.5f;
                        continue;
                    }
                    Game1.MapTiles[index] = new Tile();
                    Game1.MapTiles[index].SrcRect.X = 16 * (number % 5); // 16 is the px width of 1 tile
                    Game1.MapTiles[index].SrcRect.Y = 16 * (int)(number * .2f); // 16 is the px height of 1 tile
                }*/