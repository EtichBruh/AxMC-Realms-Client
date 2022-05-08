using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Entities;
using AxMC_Realms_Client.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nekoT;

namespace AxMC_Realms_Client.Inventory
{
    public class UI
    {
        public static Texture2D SlotSprite, EquipmentSlotSprite, BagUI, PEnterUI;
        public Rectangle BagUIRect = new(0,0,64,32);
        public Rectangle PEnterUIRect = new(0,0,32,16);
        public Rectangle BagUISrcRect = new(0,0,64,32);
        public ProgressBar HPBar, MPBar;
        Slot[] invetory = new Slot[4];
        Slot[] Equipment = new Slot[4];
        public UI(int screenWidth,int screenHeight, Texture2D _slots, Texture2D _Eslots, Texture2D bagui, Texture2D penterui)
        {
            SlotSprite = _slots;
            EquipmentSlotSprite = _Eslots;
            BagUI = bagui;
            PEnterUI = penterui;
            screenWidth /= 2;
            int screenHCenter = (int)(screenHeight / 1.25);
            BagUIRect.X = screenWidth;
            BagUIRect.Y = screenHeight - BagUIRect.Height;
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
            BagUIRect.Y = screenHeight - BagUIRect.Height;
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
            if(BasicEntity.NInteract != -1 )
            {
                if (BasicEntity.InteractEnt[BasicEntity.NInteract] is Bag)
                {
                    int len = ((Bag)BasicEntity.InteractEnt[BasicEntity.NInteract]).items.Length;
                    if (len != 2)
                    {
                        int prevX, prevY;
                        prevX = BagUIRect.X;
                        prevY = BagUIRect.Y;
                        int prevSW = 0;
                        for (int i = 0; i <= len; i++)
                        {
                            if (i == 0)
                            {
                                (BagUIRect.Width, BagUISrcRect.Width) = (18, 18);
                                prevSW = -15 * (len - 1) - 18;
                            }
                            else if (i == len)
                            {
                                (BagUIRect.Width, BagUISrcRect.Width) = (18, 18);
                                BagUISrcRect.X = 46;
                                BagUISrcRect.Y = 0;
                                BagUISrcRect.Height = 32;
                                BagUIRect.Height = 32;
                            }
                            else if (prevSW != 30)
                            {
                                BagUISrcRect.X = 17;
                                BagUISrcRect.Y = 2;
                                BagUISrcRect.Height = 30;
                                (BagUIRect.Width, BagUIRect.Height, BagUISrcRect.Width) = (30, 30, 30);
                            }
                            BagUIRect.X += prevSW;
                            BagUIRect.Y += BagUISrcRect.Y;
                            sb.Draw(BagUI, BagUIRect, BagUISrcRect, Color.White);
                            BagUIRect.Y = prevY;
                            prevSW = BagUISrcRect.Width;
                        }
                        BagUISrcRect.X = 0;
                        BagUISrcRect.Width = 64;
                        BagUIRect.X = prevX;
                    }
                    else if (len == 2)
                    {
                        sb.Draw(BagUI, BagUIRect, Color.White);
                    }
                }
                else
                {
                    sb.Draw(PEnterUI, PEnterUIRect, Color.White);
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
