using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC_Realms_Client.UI
{
    public class Item
    {
        public int[] Stats = { 500, 50, 20, 10 }; // HP Damage agility armor
        public static Texture2D SpriteSheet;
        public byte id = 0;
        public static string[] Desc = new string[] { "DRIP SUSSY AMOGUS SUPREME", "It fills you w power", "Sussy staff lmao" };
        const float scalefactor = 1f / 16f;
        public Item(byte _id)
        {
            id = _id;
        }
        public void Draw(SpriteBatch sb, Vector2 Position, int size, bool DrawDesc)
        {
            sb.Draw(SpriteSheet, Position, new(16 * id, 0, 16, 16), Color.White, 0, new Vector2(8, 8), size* scalefactor, 0, 0); // Draw Item
            if (DrawDesc)
            {
                var a = Game1.Arial.MeasureString(Desc[id]) * .01f * 12 * 0.5f; // * .01f because the font size is 100 so i scale it to 1 then multiply by size and make it centred
                a.Y += size;
                sb.DrawString(Game1.Arial, Desc[id], Position - a, Color.White, 0, Vector2.Zero, 0.12f, 0, 0); // Draw item Description
            }
        }
    }
}
