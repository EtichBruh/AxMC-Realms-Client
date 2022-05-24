using AxMC_Realms_Client.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxMC_Realms_Client.UI
{
    class Slot
    {
        Item slotitem = new();
        public Rectangle Rect = new(0,0,64,64);
        public static Rectangle SrcRect = new(0,0,16,16);
        public bool mouseHoverOn;
        public bool DrawStats;

        public void Draw(SpriteBatch sb)
        {

            Rect.Width = 48;
            Rect.Height = Rect.Width;
            sb.Draw(Item.SpriteSheet, Rect, SrcRect, Color.White, 0, new Vector2(8, 8), 0, 0);
            Rect.Width = 64;
            Rect.Height = Rect.Width;

            if (mouseHoverOn && slotitem is not null)
            {
                var a = Game1.Arial.MeasureString(slotitem.Description) *.01f;
                a.X += 20;
                a.Y += 20;
                var r = Rect;
                r.Width += (int)a.X;
                r.Height += (int)a.Y;
                a.Y = r.Height * .5f;
                sb.DrawString(Game1.Arial, slotitem.Description,r.Location.ToVector2() - a ,Color.White,0, Vector2.Zero, 0.12f,0,0);
                /*Corner.X = 16;
                Corner.Width = 10;
                var a = Game1.Arial.MeasureString(slotitem.Description);
                for(int x = 0; x < a.X; x++)
                {
                    Rect.X += x;
                    sb.Draw(UI.SlotSprite, Rect, Corner, Color.White);
                }
                sb.Draw(UI.SlotSprite, Rect, Corner, Color.White);
                sb.Draw(UI.SlotSprite, Rect, Corner, Color.White);*/
            }
        }
    }
}
