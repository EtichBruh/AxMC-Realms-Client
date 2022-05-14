using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Entities;
using AxMC_Realms_Client.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nekoT;
using System;
using System.Collections;

namespace AxMC_Realms_Client.UI
{
    public class UI
    {
        public static Texture2D SlotSprite, EquipmentSlotSprite, BagUI, PEnterUI;
        public Rectangle BagUISRect,BagUIRect = new(0,0,64,32);
        public Rectangle PEnterUIRect = new(0,0,32,16);
        public ProgressBar HPBar, MPBar;
        Slot[] Invetory = new Slot[4];
        Slot[] Equipment = new Slot[4];
        public UI(int SWidth,int SHeight, Texture2D _slots, Texture2D _Eslots, Texture2D bagui, Texture2D penterui)
        {
            SlotSprite = _slots;
            EquipmentSlotSprite = _Eslots;
            BagUI = bagui;
            PEnterUI = penterui;
            SWidth /= 2;
            int SHeightC = (int)(SHeight / 1.25);// Height center + half of center
            BagUISRect = BagUIRect;
            BagUIRect.X = SWidth;
            BagUIRect.Y = SHeight - BagUIRect.Height;
            for (int x = 0; x < Invetory.Length; x++)
            {
                Invetory[x] = new();
                Invetory[x].Rect.Y = SHeightC;
                Invetory[x].Rect.X = (x-1) * Invetory[x].Rect.Width - Invetory[x].Rect.Width/2 + SWidth ;
            }
        }
        public void Resize(int SWidth,int SHeight)
        {
            SWidth /= 2;

            int screenHCenter = (int)(SHeight * 0.8f);
            BagUIRect.X = SWidth;
            BagUIRect.Y = SHeight - BagUIRect.Height;
            for (int x = 0; x < Invetory.Length; x++)
            {
                Invetory[x].Rect.Y = screenHCenter;
                Invetory[x].Rect.X = (x-1) * (Invetory[x].Rect.Width) - Invetory[x].Rect.Width/2 + SWidth;
            }
        }
        public void Update()
        {
            HoveringOnSlot();
            if(BasicEntity.NInteract != -1 && BasicEntity.InteractEnt[BasicEntity.NInteract] is Portal)
            {
                if(Input.KState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) || Input.MState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                    PEnterUIRect.Intersects(new Rectangle(Input.MState.Position, new Point(1, 1))))
                {
                    string a = ((Map.Maps)((Portal)BasicEntity.InteractEnt[BasicEntity.NInteract]).id).ToString();
                    Map.Map.Load(a + ".json");
                }
            }
        }
        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < Invetory.Length; i++)
            {
                Invetory[i].Draw(sb);
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
                                (BagUIRect.Width, BagUISRect.Width) = (18, 18);
                                prevSW = -15 * (len - 1) - 18;
                            }
                            else if (i == len)
                            {
                                (BagUIRect.Width, BagUISRect.Width) = (18, 18);
                                BagUISRect.X = 46;
                                BagUISRect.Y = 0;
                                BagUISRect.Height = 32;
                                BagUIRect.Height = 32;
                            }
                            else if (prevSW != 30)
                            {
                                BagUISRect.X = 17;
                                BagUISRect.Y = 2;
                                BagUISRect.Height = 30;
                                (BagUIRect.Width, BagUIRect.Height, BagUISRect.Width) = (30, 30, 30);
                            }
                            BagUIRect.X += prevSW;
                            BagUIRect.Y += BagUISRect.Y;
                            sb.Draw(BagUI, BagUIRect, BagUISRect, Color.White);
                            BagUIRect.Y = prevY;
                            if( i == 1 || prevSW == 30)
                            {
                                sb.Draw(Item.SpriteSheet,
                                    new Vector2(BagUIRect.X, BagUIRect.Y + BagUI.Height * .5f),
                                    Slot.SrcRect, Color.White, 0, new Vector2(8, 8), 1.75f, 0, 0); // This draws item
                            }
                            prevSW = BagUISRect.Width;

                        }
                        BagUISRect.X = 0;
                        BagUISRect.Width = 64;
                        BagUIRect.X = prevX;
                    }
                    else if (len == 2) sb.Draw(BagUI, BagUIRect, Color.White);
                }
                //else sb.Draw(PEnterUI, PEnterUIRect, Color.White);
            }
            
        }
        void HoveringOnSlot()
        {
            var index = new Rectangle(Input.MState.Position, new Point(1,1));
            for (int i = 0; i < Invetory.Length; i++)
            {
                var r = Invetory[i].Rect;
                r.X -= 32;
                r.Y -= 32;
                if (r.Intersects(index)) { Invetory[i].mouseHoverOn = true; }
                else { Invetory[i].mouseHoverOn = false; }
                //else if (Equipment[i].mouseHoverOn = index == invetory[i].Rect.Location.DivideBy(60)) break;
            }

        }
        
    }
}
