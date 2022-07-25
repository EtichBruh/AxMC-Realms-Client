using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Text.Json;

namespace AxMC_Realms_Client.UI
{
    public class Item
    {
        //public int[] Stats = { 500, 0, 50, 20, 10 }; // HP MP Strength agility Intelligence
        public static Texture2D SpriteSheet;
        public byte id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int[] Stats { get; set; }
        public Rectangle Source { get; set; }

        const float scalefactor = 1f / 16f;

        public static Item[] items;
        public static void Load()
        {
            items = JsonSerializer.Deserialize<Item[]>(File.ReadAllText("Items.json"), new JsonSerializerOptions{ IncludeFields =true, WriteIndented = true  });
        }

        public static void Draw(SpriteBatch sb, Vector2 Position, int size, bool DrawDesc, int id)
        {
            var item = items[id];
            sb.Draw(SpriteSheet, Position, item.Source, Color.White, 0, item.Source.Size.ToVector2() * .5f, size* scalefactor, 0, 0); // Draw Item
            if (DrawDesc)
            {
                var Desc = item.Name +
                    '\n' + item.Description;
                Desc += item.Stats[0] == 0 ?"": "\n + " + item.Stats[0] + "Max Health";
                Desc += item.Stats[1] == 0 ?"": "\n + " + item.Stats[1] + "Max Mana";
                Desc += item.Stats[2] == 0 ?"": "\n + " + item.Stats[2] + "Strength";
                Desc += item.Stats[3] == 0 ?"": "\n + " + item.Stats[3] + "Agility";
                Desc += item.Stats[4] == 0 ?"": "\n + " + item.Stats[4] + "Intelligence";

                var a = Game1.Arial.MeasureString(Desc) * .01f * 12 * 0.5f; // * .01f because the font size is 100 so i scale it to 1 then multiply by size and make it centred
                a.Y *= 2;
                a.Y += size;
                Panel.Draw(sb, UI.SlotSprite, Position - a, (int)(a.X * 2.25f), (int)((a.Y - size) * 1.25f));
                sb.DrawString(Game1.Arial, Desc, Position - a, Color.White, 0, Vector2.Zero, 0.12f, 0, 0); // Draw item Description
            }
        }
    }
}
