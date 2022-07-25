using AxMC_Realms_Client;
using AxMC_Realms_Client.Entity;
using Microsoft.Xna.Framework;
using nekoT;
using Nez;
using System.Collections.Generic;
using System.IO;

namespace Map
{
    public static class Map
    {
        /// <summary>
        /// X is map width, Y is map height
        /// </summary>
        public static Point Size;
        public static byte[] byteMap;
        public static void Load(string path, FastList<SpriteAtlas> entities)
        {
            byte[] entids;
            using (BinaryReader br = new BinaryReader(File.OpenRead(path + ".bm")))
            {
                Size.X = br.ReadInt32();
                byteMap = br.ReadBytes(br.ReadInt32());
                entids = br.ReadBytes(byteMap.Length);
            }
            Size.Y = byteMap.Length / Size.X;
            Game1.MapTiles = new Tile[byteMap.Length];
            Game1.MapBlocks = new byte[byteMap.Length];
            BasicEntity.InteractEnt.Clear();

            for (int i = 1; i < entities.Length; i++)
            {
                entities.RemoveAt(i);
            }
            for (int i = 0; i < byteMap.Length; i++)
            {
                byte number = byteMap[i];
                if (number == 255) continue;
                if (number == 7 || number == 8)
                {
                    Game1.MapBlocks[i] = number;
                    continue;
                }
                Game1.MapTiles[i] = new Tile();
            }
            if (entids.Length > 0)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    int _x = 50 * x + 25;
                    for (int y = 0; y < Size.Y; y++)
                    {
                        int index = x + y * Size.X;
                        int id = entids[index];
                        if (entids[index] == 255) continue;
                        int _y = 50 * y + 25;
                        if (id < 1)
                        {
                            entities.Add(new Enemy(Enemy.tempText) { Position = new Vector2(_x, _y) });
                        }
                        else { BasicEntity.Add(id < 2 ? new Portal(id, _x, _y) : new Obstacle(id, _x, _y)); }
                    }
                }
            }
        }
        // called on contentload
        public static void Load(string path, List<SpriteAtlas> ToAdd)
        {
            byte[] entids;
            using (BinaryReader br = new(File.OpenRead(path + ".bm")))
            {
                Size.X = br.ReadInt32();
                byteMap = br.ReadBytes(br.ReadInt32());
                entids = br.ReadBytes(byteMap.Length);
            }
            Size.Y = byteMap.Length / Size.X;
            Game1.MapTiles = new Tile[byteMap.Length];
            Game1.MapBlocks = new byte[byteMap.Length];
            BasicEntity.InteractEnt.Clear();
            for (int i = 0; i < byteMap.Length; i++)
            {
                byte number = byteMap[i];
                if (number == 255) continue;
                if (number == 7 || number == 8)
                {
                    Game1.MapBlocks[i] = number;
                    continue;
                }
                Game1.MapTiles[i] = new Tile();
            }
            if (entids is not null)
            {
                Vector2 Pos;
                for (int x = 0; x < Size.X; x++)
                {
                    Pos.X = 50 * x + 25;
                    for (int y = 0; y < Size.Y; y++)
                    {
                        int index = x + y * Size.X;
                        int id = entids[index];
                        if (entids[index] == 255) continue;
                        Pos.Y = 50 * y + 25;
                        if (id < 1)
                        {
                            ToAdd.Add(new Enemy(Enemy.tempText) { Position = Pos });
                        }
                        else { BasicEntity.Add(--id < 2 ? new Portal(id, (int)Pos.X, (int)Pos.Y) : new Obstacle(id, (int)Pos.X, (int)Pos.Y)); }
                    }
                }
            }
        }
    }
}