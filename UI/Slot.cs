﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC_Realms_Client.UI
{
    class Slot
    {
        public int item = -1;
        public Rectangle Rect = new(0, 0, 23, 25);
        public Rectangle SrcRect = new(28, 0, 23, 25);
        public bool mouseHoverOn;
        //public bool DrawStats;
        public Slot(int itemid = -1,int sourceY = 4)
        {
            item = itemid;
            SrcRect.Y += sourceY;
        }
        public Slot(int x, int y, int width, int height)
        {
            Rect = SrcRect = new(x, y, width, height);
        }
        public void Draw(SpriteBatch sb, Texture2D slotsprite)
        {
            sb.Draw(slotsprite, Rect, SrcRect, new(127, 127, 127, 127)); // Draw slot
            if (item != -1)
            {
                Item.Draw(sb, Rect.Center.ToVector2(), 16 , mouseHoverOn, item);
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