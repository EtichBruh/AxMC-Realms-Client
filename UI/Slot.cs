using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC_Realms_Client.UI
{
    class Slot
    {
        public int item = -1;
        public Rectangle Rect = new(0, 0, 25, 25);
        public Rectangle SrcRect = new(89, 0, 25, 25);
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