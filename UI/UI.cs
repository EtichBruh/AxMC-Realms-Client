using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Entities;
using AxMC_Realms_Client.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nekoT;
using Nez;

namespace AxMC_Realms_Client.UI
{
    public class UI
    {
        public Texture2D SlotSprite, BagUI, PEnterUI, ExpJar, StatIcons;

        public Rectangle BagUISRect, BagUIRect = new(0, 0, 64, 32);
        public Rectangle ExpJarRect = new(0, 0, 32, 92);
        public Rectangle PEnterUIRect = new(0, 0, 32, 16);
        public Rectangle StatsRect = new(0, 0, 32, 32);

        public ProgressBar HPBar, MPBar;

        Slot[] Invetory = new Slot[8];
        //Slot[] Equipment = new Slot[4];

        bool isDrag = false;
        int DraggingSlot;
        public UI(int swidth, int sheight, Texture2D slot, Texture2D bag, Texture2D PortalEnter, Texture2D expjar, Texture2D stats)
        {
            SlotSprite = slot;
            BagUI = bag;
            PEnterUI = PortalEnter;
            ExpJar = expjar;
            StatIcons = stats;
            BagUISRect = BagUIRect;
            for (int i = 0; i < Invetory.Length; i++)
            {
                if (i > 3 && i < 7) Invetory[i] = new((byte)(i - 4));
                else Invetory[i] = new();
                Invetory[i].SrcRect.X = i < 4 ? 32 : 0;
            }
            Resize(swidth, sheight);
        }
        public void Resize(int SWidth, int SHeight)
        {
            SWidth /= 2;
            int sHCenter = (int)(SHeight * 0.8f);

            StatsRect.Y = SHeight / 2;

            BagUIRect.X = SWidth;
            BagUIRect.Y = SHeight - BagUIRect.Height;

            var smth = -Invetory[0].Rect.Width / 2 + SWidth;
            for (int x = 0; x < Invetory.Length; x++)
            {
                if (x < 4)
                {
                    Invetory[x].Rect.Y = sHCenter;
                    Invetory[x].Rect.X = (x - 1) * (Invetory[x].Rect.Width) + smth;
                }
                else
                {
                    Invetory[x].Rect.Y = sHCenter + 48;
                    Invetory[x].Rect.X = (x - 4 - 1) * (Invetory[x].Rect.Width) + smth;
                }
            }

            var temp = Invetory[4].Rect.Size.MultiplyBy(.5f);
            ExpJarRect.X = Invetory[4].Rect.X - temp.X - ExpJar.Width;
            ExpJarRect.Y = Invetory[4].Rect.Y + temp.Y - ExpJar.Height;
        }
        public void Update(FastList<SpriteAtlas> entities)
        {
            HoveringOnSlot();
            if (BasicEntity.NInteract != -1 && BasicEntity.InteractEnt[BasicEntity.NInteract] is Portal)
            {
                if (Input.KState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) || Input.MState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                    PEnterUIRect.Intersects(new Rectangle(Input.MState.Position, new Point(1, 1))))
                {
                    string a = ((Map.Maps)((Portal)BasicEntity.InteractEnt[BasicEntity.NInteract]).id).ToString();
                    Map.Map.Load(a, entities);
                }
            }
        }
        const float factor = 1f / (100f / 92f);
        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(Game1.Arial, Player.Stats[2].ToString(), new Vector2(StatsRect.Right, StatsRect.Y), Color.LightGreen, 0, new(0, 50), 0.16f, 0, 0);
            sb.Draw(StatIcons, StatsRect, new Rectangle(0, 0, 32, 32), Color.White, 0, new(0, 16), 0, 0);

            StatsRect.Y += 32;
            sb.Draw(StatIcons, StatsRect, new Rectangle(32, 0, 32, 32), Color.White, 0, new(0, 16), 0, 0);
            sb.DrawString(Game1.Arial, Player.Stats[1].ToString(), new Vector2(StatsRect.Right, StatsRect.Y), Color.DarkRed, 0, new(0, 50), 0.16f, 0, 0);

            StatsRect.Y -= 64;
            sb.DrawString(Game1.Arial, Player.Stats[3].ToString(), new Vector2(StatsRect.Right, StatsRect.Y), Color.Gray, 0, new(0, 50), 0.16f, 0, 0);
            sb.Draw(StatIcons, StatsRect, new Rectangle(64, 0, 32, 32), Color.White, 0, new(0, 16), 0, 0);

            StatsRect.Y += 32;// Reset back

            if (Player.XP > 0)
            {
                sb.Draw(ProgressBar.Pixel, new Rectangle(ExpJarRect.X, ExpJarRect.Bottom - (int)(Player.XP * factor) - 11, ExpJarRect.Width, (int)(Player.XP * factor)), Color.White);
            }
            sb.Draw(ExpJar, ExpJarRect, Color.White); // Jar is in front of liquid

            for (int i = 0; i < Invetory.Length; i++)
            {
                sb.Draw(SlotSprite, Invetory[i].Rect, Invetory[i].SrcRect, Color.White, 0, new Vector2(16, 16), 0, 0);
                if (Invetory[i].slotitem != null) Invetory[i].Draw(sb);
                // this draws item, not slot ( i was surprised tho )
            }

            if (BasicEntity.NInteract != -1)
            {
                if (BasicEntity.InteractEnt[BasicEntity.NInteract] is Bag)
                {
                    int len = ((Bag)BasicEntity.InteractEnt[BasicEntity.NInteract]).items.Length;
                    if (len != 2)
                    {
                        int prevX = BagUIRect.X, prevY = BagUIRect.Y;
                        int prevSW = -15 * (len - 1); // Get 1st segment position

                        BagUIRect.Width = BagUISRect.Width = 18; // Since by default src rect XY is 0 only set width
                        BagUIRect.X += prevSW - 18; // Making it centred 
                        sb.Draw(BagUI, BagUIRect, BagUISRect, Color.White); // Draw 1st segment

                        BagUIRect.X += 30 * (len - 1) + 18; // Get last segment position
                        BagUISRect.X = 46; // Set SourceRect to draw last segment ( width are same as 1st segment)
                        sb.Draw(BagUI, BagUIRect, BagUISRect, Color.White); // Draw last segment

                        BagUISRect.X = 17;
                        BagUIRect.Y += BagUISRect.Y = 2;
                        BagUIRect.X = prevX + prevSW -
                            (BagUIRect.Width = BagUIRect.Height = BagUISRect.Width = BagUISRect.Height = 30);// Get position after 1st segment + Set SourceRect to draw slot

                        Vector2 itempos = new Vector2(BagUIRect.X, BagUIRect.Y + BagUI.Height * .5f); // im not sure but its made to optimize
                        for (int i = 0; i < len; i++)
                        {
                            BagUIRect.X += BagUIRect.Width; // because slots shouldnt be on same pos, the next slot pos changed by its width
                            itempos.X += BagUIRect.Width;
                            if (i < len - 1) sb.Draw(BagUI, BagUIRect, BagUISRect, Color.White); // Draw slot's 

                            sb.Draw(Item.SpriteSheet,
                                    itempos,
                                    new(0, 0, 16, 16), Color.White, 0, new Vector2(8, 8), 1.75f, 0, 0); //Draw item
                        }

                        BagUISRect.X = BagUISRect.Y = 0; // reset rectangles back
                        BagUISRect.Width = 64;
                        BagUISRect.Height = 32;
                        BagUIRect = BagUISRect;
                        BagUIRect.X = prevX;
                        BagUIRect.Y = prevY;// reset rectangles back
                    }
                    else if (len == 2) sb.Draw(BagUI, BagUIRect, Color.White);
                }
                //else sb.Draw(PEnterUI, PEnterUIRect, Color.White);
            }
            if (isDrag)
            {
                sb.Draw(Item.SpriteSheet, Input.MState.Position.ToVector2(), new(16 * Invetory[DraggingSlot].slotitem.id, 0, 16, 16), Color.White, 0, new Vector2(8, 8), 2, 0, 0);
            }

        }
        void HoveringOnSlot()
        {
            var index = new Rectangle(Input.MState.Position, new Point(1, 1));
            for (int i = 0; i < Invetory.Length; i++)
            {
                var slot = Invetory[i];
                    var r = slot.Rect;
                    r.X -= r.Width / 2; // its probably centred 
                    r.Y -= r.Height / 2;
                if (r.Intersects(index))
                {
                    slot.mouseHoverOn = true;
                    if (!isDrag && slot.slotitem != null &&
                    Input.MState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        isDrag = true;
                        DraggingSlot = i;
                    }
                    else if (isDrag && Input.MState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        Item temp = Invetory[DraggingSlot].slotitem;
                        Invetory[DraggingSlot].slotitem = slot.slotitem;
                        slot.slotitem = temp;
                        isDrag = false;
                        if (i < 4 && slot.slotitem != Invetory[DraggingSlot].slotitem) for(int j =0; j < Player.Stats.Length;j++) Player.Stats[j] += slot.slotitem.Stats[j];
                        if (i >=4 && Invetory[DraggingSlot].slotitem != slot.slotitem) for(int j =0; j < Player.Stats.Length;j++) Player.Stats[j] -= slot.slotitem.Stats[j];
                    }
                }
                else { slot.mouseHoverOn = false; }
                    //else if (Equipment[i].mouseHoverOn = index == invetory[i].Rect.Location.DivideBy(60)) break;
                }
            }
        }
    }