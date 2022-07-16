using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC_Realms_Client.UI
{
    class Slot
    {
        public int item;
        public Rectangle Rect = new(0, 0, 32, 32);
        public Rectangle SrcRect = new(0, 0, 32, 32);
        public bool mouseHoverOn;
        //public bool DrawStats;
        public Slot(int itemid = -1)
        {
            item = itemid;
        }
        public void Draw(SpriteBatch sb, Texture2D slotsprite)
        {
            sb.Draw(slotsprite, Rect, SrcRect, new(127, 127, 127, 127), 0, new Vector2(16, 16), 0, 0); // Draw slot
            if (item != -1)
            {
                Item.Draw(sb, Rect.Location.ToVector2(), 26, mouseHoverOn, item);
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