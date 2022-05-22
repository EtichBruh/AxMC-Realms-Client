using AxMC_Realms_Client;
using AxMC_Realms_Client.Entities;
using Microsoft.Xna.Framework;
using nekoT;
using Nez;
using System;
using System.Collections.Generic;
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
        private static void Save(byte[] map, int width)
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
        public static void Load(string path, FastList<SpriteAtlas> entities)
        {
            byte[] entids;
            using (JsonDocument jsonData = JsonDocument.Parse(File.ReadAllText(path + ".json")))
            {
                byteMap = JsonSerializer.Deserialize<byte[]>(jsonData.RootElement.GetProperty("Data").GetRawText());
                try
                {
                    entids = JsonSerializer.Deserialize<byte[]>(jsonData.RootElement.GetProperty("Entities").GetRawText());
                }catch
                {
                    entids = Array.Empty<byte>();
                }
                Size.X = jsonData.RootElement.GetProperty("width").GetInt32();
            }
            Size.Y = byteMap.Length / Size.X;
            Game1.MapTiles = new Tile[byteMap.Length];
            Game1.MapBlocks = new Vector2[byteMap.Length];
            BasicEntity.InteractEnt.Clear();

            for (int i = 1; i < entities.Length; i++)
            {
                entities.RemoveAt(i);
            }
            for (int i = 0; i < byteMap.Length; i++)
            {
                byte number = byteMap[i];
                if (number == 255) continue;
                if(number == 6)
                {
                    Game1.MapBlocks[i] = Vector2.One;
                }
                Game1.MapTiles[i] = new Tile();
                Game1.MapTiles[i].SrcRect.X = 16 * (number % 5);/* = new()
                {
                    id = entity,
                    SpriteId = entity < 1 ? 0 : 1
                };*/
            }
            if (entids.Length > 0)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    for (int y = 0; y < Size.Y; y++)
                    {
                        int index = x + y * Size.X;
                        int id = entids[index];
                        if (entids[index] == 255) continue;
                        if (id < 1)
                        {
                            entities.Add(new Enemy(Enemy.tempText) { Position = new Vector2(50 * x + 25, 50 * y + 25) });
                        }
                        else { BasicEntity.InteractEnt.Add(new Portal(x * 50 + 25, y * 50 + 25)); }
                    }
                }
            }
        }
        public static void Load(string path, List<SpriteAtlas> ToAdd)
        {
            byte[] entids;
            using (JsonDocument jsonData = JsonDocument.Parse(File.ReadAllText(path + ".json")))
            {
                byteMap = JsonSerializer.Deserialize<byte[]>(jsonData.RootElement.GetProperty("Data").GetRawText());
                try
                {
                    entids = JsonSerializer.Deserialize<byte[]>(jsonData.RootElement.GetProperty("Entities").GetRawText());
                }
                catch { entids = Array.Empty<byte>(); }
                Size.X = jsonData.RootElement.GetProperty("width").GetInt32();
            }
            Size.Y = byteMap.Length / Size.X;
            Game1.MapTiles = new Tile[byteMap.Length];
            Game1.MapBlocks = new Vector2[byteMap.Length];
            BasicEntity.InteractEnt.Clear();
            for (int i = 0; i < byteMap.Length; i++)
            {
                byte number = byteMap[i];
                if (number == 255) continue;
                if (number == 6)
                {
                    Game1.MapBlocks[i] = Vector2.One;
                }
                Game1.MapTiles[i] = new Tile();
                Game1.MapTiles[i].SrcRect.X = 16 * (number % 5);/* = new()
                {
                    id = entity,
                    SpriteId = entity < 1 ? 0 : 1
                };*/
            }
            if (entids.Length > 0)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    for (int y = 0; y < Size.Y; y++)
                    {
                        int index = x + y * Size.X;
                        int id = entids[index];
                        if (entids[index] == 255) continue;
                        if (id < 1)
                        {
                            ToAdd.Add(new Enemy(Enemy.tempText) { Position = new Vector2(50 * x + 25, 50 * y + 25) });
                        }
                        else { BasicEntity.InteractEnt.Add(new Portal(x * 50 + 25, y * 50 + 25)); }
                    }
                }
            }
        }
    }
}
/*            using (JsonDocument jsonData = JsonDocument.Parse(File.ReadAllText(path)))
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
                    Game1.MapTiles[i] = new Tile();
                    Game1.MapTiles[i].SrcRect.X *= (number % 5); // 16 is the px width of 1 tile
                    Game1.MapTiles[i].SrcRect.Y *= (int)(number * .2f);
            }
// 16 is the px height of 1 tile
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