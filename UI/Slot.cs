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
        public Item item;
        public Rectangle Rect = new(0,0,48,48);
        public Rectangle SrcRect = new(0, 0, 32, 32);
        public bool mouseHoverOn;
        //public bool DrawStats;
        public Slot(byte? itemid = null)
        {
            if(itemid != null)
            {
                item = new(itemid ?? 0);
            }
        }
        public void Draw(SpriteBatch sb, Texture2D slotsprite)
        {
            sb.Draw(slotsprite, Rect, SrcRect, new(127,127,127,127), 0, new Vector2(16, 16), 0, 0); // Draw slot
            if (item != null)
            {
                item.Draw(sb, Rect.Location.ToVector2(),32, mouseHoverOn);
            }
        }
    }
}
/*Corner.X = 16;
Corner.Width = 10;
var a = Game1.Arial.MeasureString(slotitem.Description);
for(int x = 0; x < a.X; x++)
{
    Rect.X += x;
    sb.Draw(UI.SlotSprite, Rect, Corner, Color.White);
}
sb.Draw(UI.SlotSprite, Rect, Corner, Color.White);
sb.Draw(UI.SlotSprite, Rect, Corner, Color.White);*/ //something for future desc background