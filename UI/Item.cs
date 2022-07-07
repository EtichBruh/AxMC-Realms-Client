using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Text.Json;

namespace AxMC_Realms_Client.UI
{
    public class Item
    {
        //public int[] Stats = { 500, 0,50, 20, 10 }; // HP Damage agility armor
        public static Texture2D SpriteSheet;
        public byte id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int[] Stats { get; set; }
        public Rectangle Source { get; set; }

        //public static string[] Desc = new string[] { "DRIP SUSSY AMOGUS SUPREME", "It fills you w power", "Sussy staff lmao", "DIAMONDZZZ" };
        const float scalefactor = 1f / 16f;
        static Rectangle[] srect = new Rectangle[] {
        new(0,1,16 + 2,15 + 3),
        new(17,0,16+2,16+2),
        new(34,0,16+2,16+2),
        new(52+1,2,12+2,13+2)
        };
        public static Item[] items;
        public static void Load()
        {
            items = JsonSerializer.Deserialize<Item[]>(File.ReadAllText("Items.json"));
        }

        const float origin = 17f / 2f;
        public static void Draw(SpriteBatch sb, Vector2 Position, int size, bool DrawDesc, int id)
        {
            sb.Draw(SpriteSheet, Position, items[id].Source, Color.White, 0, items[id].Source.Size.ToVector2() * .5f, size* scalefactor, 0, 0); // Draw Item
            if (DrawDesc)
            {
                var Desc = items[id].Description;
                var a = Game1.Arial.MeasureString(Desc) * .01f * 12 * 0.5f; // * .01f because the font size is 100 so i scale it to 1 then multiply by size and make it centred
                a.Y += size;
                sb.DrawString(Game1.Arial, Desc, Position - a, Color.White, 0, Vector2.Zero, 0.12f, 0, 0); // Draw item Description
            }
        }
    }
}
