using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nekoT;

namespace AxMC_Realms_Client.Inventory
{
    public class UI
    {
        public static Texture2D SlotSprite;
        public static Texture2D EquipmentSlotSprite;
        public static Texture2D BagUI;
        public Rectangle BagUIRect = new(0,0,64,32);
        public Rectangle BagUISrcRect = new(0,0,64,32);
        public ProgressBar HPBar, MPBar;
        Slot[] invetory = new Slot[4];
        Slot[] Equipment = new Slot[4];
        public UI(int screenWidth,int screenHeight, Texture2D _slots, Texture2D _Eslots)
        {
            SlotSprite = _slots;
            EquipmentSlotSprite = _Eslots;
            screenWidth /= 2;
            int screenHCenter = (int)(screenHeight / 1.25);
            BagUIRect.X = screenWidth;
            BagUIRect.Y = screenHeight;
            for (int x = 0; x < invetory.Length; x++)
            {
                invetory[x] = new();
                invetory[x].Rect.Y = screenHCenter;
                invetory[x].Rect.X = (x-1) * invetory[x].Rect.Width - invetory[x].Rect.Width/2 + screenWidth ;
            }
        }
        public void Resize(int screenWidth,int screenHeight)
        {

            screenWidth /= 2;
            int screenHCenter = (int)(screenHeight / 1.25);
            BagUIRect.X = screenWidth;
            BagUIRect.Y = screenHeight - BagUI.Height;
            for (int x = 0; x < invetory.Length; x++)
            {
                invetory[x].Rect.Y = screenHCenter;
                invetory[x].Rect.X = (x-1) * (invetory[x].Rect.Width) - invetory[x].Rect.Width/2 + screenWidth;
            }
        }
        public void Update()
        {
            HoveringOnSlot();
        }
        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < invetory.Length; i++)
            {
                invetory[i].Draw(sb);
            }
            if(Entities.Bag.NearestBag != -1)
            {
                int prevX, prevY;
                prevX = BagUIRect.X;
                prevY = BagUIRect.Y;
                for (int i = 0; i < Entities.Bag.Bags[Entities.Bag.NearestBag].items.Length; i++)
                {
                    if (i == 0) { BagUISrcRect.Width /= 2; }
                    else if (i == Entities.Bag.Bags[Entities.Bag.NearestBag].items.Length - 1) { BagUISrcRect.Width /= 2; BagUISrcRect.X = 32; }
                    else
                    {
                        BagUISrcRect.X = 17;
                        BagUISrcRect.Width = 30;
                    }
                    BagUIRect.X += (i - 1) * BagUISrcRect.Width;
                    BagUIRect.Width = 32;
                    sb.Draw(BagUI, BagUIRect, BagUISrcRect, Color.White);
                    BagUIRect.X = prevX;
                    BagUISrcRect.X = 0;
                    BagUISrcRect.Width = 64;
                }
            }
            
        }
        void HoveringOnSlot()
        {
            var index = new Rectangle(Input.MState.Position, new Point(1,1));
            for (int i = 0; i < invetory.Length; i++)
            {
                var r = invetory[i].Rect;
                r.X -= 32;
                r.Y -= 32;
                if (r.Intersects(index)) { invetory[i].mouseHoverOn = true; }
                else { invetory[i].mouseHoverOn = false; }
                //else if (Equipment[i].mouseHoverOn = index == invetory[i].Rect.Location.DivideBy(60)) break;
            }

        }
        
    }
}
