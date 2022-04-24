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
        public ProgressBar HPBar, MPBar;
        Slot[] invetory = new Slot[4];
        Slot[] Equipment = new Slot[4];
        public UI(int screenWidth,int screenHeight, Texture2D _slots, Texture2D _Eslots)
        {
            SlotSprite = _slots;
            EquipmentSlotSprite = _Eslots;
            screenWidth /= 2;
            var screenFourth = screenWidth / 2;
            int screenHCenter = (int)(screenHeight / 1.25);
            for (int x = 0; x < invetory.Length; x++)
            {
                invetory[x] = new();
                invetory[x].Rect.Y = screenHCenter;
                invetory[x].Rect.X = (x-1) * invetory[x].Rect.Width - invetory[x].Rect.Width/2 + screenWidth ;
            }
        }
        public void Resize(int ScreenWidth,int screenHeight)
        {
            ScreenWidth /= 2;
            var screenFourth = ScreenWidth / 2;
            int screenHCenter = (int)(screenHeight / 1.25);
            for (int x = 0; x < invetory.Length; x++)
            {
                invetory[x].Rect.Y = screenHCenter;
                invetory[x].Rect.X = (x-1) * (invetory[x].Rect.Width) - invetory[x].Rect.Width/2 + ScreenWidth;
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
