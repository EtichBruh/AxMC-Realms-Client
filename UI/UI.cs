using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Entities;
using AxMC_Realms_Client.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nekoT;
using Nez;
using System;

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
            if (BasicEntity.GetNear() is Portal portal)
            {
                if (Input.KState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) || Input.MState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                    PEnterUIRect.Intersects(new Rectangle(Input.MState.Position, new Point(1, 1))))
                {
                    string a = ((Map.Maps)portal.id).ToString();
                    Map.Map.Load(a, entities);
                }
            }
        }
        const float factor = 1f / (100f / 92f);
        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(Game1.Arial, Player.Stats[2].ToString(), new Vector2(StatsRect.Right, StatsRect.Y), Color.LightGreen, 0, new(0, 50), 0.16f, 0, 0);
            sb.Draw(StatIcons, StatsRect, new Rectangle(0, 0, 16, 16), Color.White, 0, new(0, 16), 0, 0);

            StatsRect.Y += 32;
            sb.Draw(StatIcons, StatsRect, new Rectangle(32, 0, 16, 16), Color.White, 0, new(0, 16), 0, 0);
            sb.DrawString(Game1.Arial, Player.Stats[1].ToString(), new Vector2(StatsRect.Right, StatsRect.Y), Color.DarkRed, 0, new(0, 50), 0.16f, 0, 0);

            StatsRect.Y -= 64;
            sb.DrawString(Game1.Arial, Player.Stats[3].ToString(), new Vector2(StatsRect.Right, StatsRect.Y), Color.Gray, 0, new(0, 50), 0.16f, 0, 0);
            sb.Draw(StatIcons, StatsRect, new Rectangle(64, 0, 16, 16), Color.White, 0, new(0, 16), 0, 0);

            StatsRect.Y += 32;// Reset back

            if (Player.XP > 0)
            {
                int height = (int)(Player.XP * factor);
                sb.Draw(ProgressBar.Pixel, new Rectangle(ExpJarRect.X, ExpJarRect.Bottom - height - 10, ExpJarRect.Width, height), Color.LightGray);
            }
            sb.Draw(ExpJar, ExpJarRect, Color.White); // Jar is in front of liquid

            for (int i = 0; i < Invetory.Length; i++)
            {
                Invetory[i].Draw(sb, SlotSprite);// Draw inv slot
            }

            if (BasicEntity.NInteract != -1)
            {
                if (BasicEntity.GetNear() is Bag bag)
                {
                    int len = bag.items.Length;
                    if (len != 2)
                    {
                        int prevX = BagUIRect.X, prevY = BagUIRect.Y;
                        int prevSW = 15 * (len - 1); // Get 1st segment position

                        BagUIRect.Width = BagUISRect.Width = 18; // Since by default src rect XY is 0 only set width

                        BagUIRect.X += prevSW;
                        BagUISRect.X = 46;
                        sb.Draw(BagUI, BagUIRect, BagUISRect, Color.White); // Draw last segment

                        BagUISRect.X = 0;
                        BagUIRect.X = prevX - prevSW - 18;
                        sb.Draw(BagUI, BagUIRect, BagUISRect, Color.White); // Draw 1st segment

                        BagUIRect.Y += 2;
                        BagUISRect.Y += 2;
                        BagUIRect.X += 18 - (BagUIRect.Width = BagUIRect.Height = 30);
                        var slotRect = new Rectangle(17, 2, 30, 30);
                        Vector2 itempos = new(BagUIRect.X, BagUIRect.Y + BagUI.Height * .5f); // im not sure but its made to optimize
                        var mouse = new Rectangle(Input.MState.Position, new Point(1, 1));

                        for (int i = 0; i < len; i++)
                        {
                            BagUIRect.X += BagUIRect.Width;
                            itempos.X += BagUIRect.Width;
                            if (i < len - 1) sb.Draw(BagUI, BagUIRect, slotRect, Color.White); // Draw slot

                            bag.items[i].Draw(sb, itempos, 30, new Rectangle((int)itempos.X - 18,BagUIRect.Y,30,30).Intersects(mouse));
                        }
                        BagUISRect = new(0, 0, 64, 32);
                        BagUIRect = new(prevX, prevY, 64, 32);// reset rects back
                    }
                    else if (len == 2)
                    {
                        sb.Draw(BagUI, BagUIRect, null,Color.White,0, new(BagUI.Width * .5f,0),0,0);
                        float y = BagUIRect.Y + BagUI.Height * .5f;
                        bag.items[0].Draw(sb, new(BagUIRect.X - BagUIRect.Width * .25f,y), 30, false);
                        bag.items[1].Draw(sb, new(BagUIRect.X + BagUIRect.Width * .25f,y), 30, false);
                    }
                }
                //else sb.Draw(PEnterUI, PEnterUIRect, Color.White);
            }
            if (isDrag)
            {
                if (DraggingItem != null)
                {
                    DraggingItem.Draw(sb, Input.MState.Position.ToVector2(), 32, false);
                }
                else
                    Invetory[DraggingSlot].item.Draw(sb, Input.MState.Position.ToVector2(), 32, false);
            }
        }
        Item DraggingItem = null;
        void HoveringOnSlot()
        {
            var index = new Rectangle(Input.MState.Position, new Point(1, 1));
            if (BasicEntity.NInteract != -1 && BasicEntity.InteractEnt[BasicEntity.NInteract] is Bag bag)
            {
                var itemRect = BagUIRect;
                itemRect.X -= 15 * (bag.items.Length -1 ) + 30 + 18; // + 18 because 1 slot is 2 drawn slot segment and theyre offset by 1st segment width
                for (int i = 0; i < bag.items.Length; i++)
                {
                    if (!isDrag && Input.MState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed & itemRect.Intersects(index))
                    {
                        DraggingItem = bag.items[i];
                        isDrag = true;
                        if (bag.items.Length == 1)
                        {
                            BasicEntity.InteractEnt.RemoveAt(BasicEntity.NInteract);
                            BasicEntity.NInteract = -1;
                            break;
                        }
                        bag.items.RemoveAt(i);
                        break;
                    };
                    itemRect.X += 30;
                }
            }
            for (int i = 0; i < Invetory.Length; i++)
            {
                var slot = Invetory[i];
                var r = slot.Rect;
                r.X -= r.Width / 2;
                r.Y -= r.Height / 2;
                if (slot.mouseHoverOn = r.Intersects(index))
                {
                    if (!isDrag && slot.item != null && DraggingItem == null &&
                    Input.MState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        isDrag = true;

                        DraggingSlot = i;
                    }
                }
                if (isDrag && DraggingSlot != i && Input.MState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    if (DraggingItem != null && slot.mouseHoverOn && slot.item == null) // this is for dragging for non inv slot
                    {
                        slot.item = DraggingItem;
                        if (i < 4)
                        {
                            for (int j = 0; j < Player.Stats.Length; j++) Player.Stats[j] += slot.item.Stats[j];
                        }
                        DraggingItem = null;
                        isDrag = false;
                        continue;
                    }
                    else if (slot.mouseHoverOn)
                    {
                        Item temp = Invetory[DraggingSlot].item;

                        Invetory[DraggingSlot].item = slot.item;
                        slot.item = temp;

                        isDrag = false;

                        if (i < 4 && DraggingSlot >= 4)
                        {
                            if (Invetory[DraggingSlot].item != null) for (int j = 0; j < Player.Stats.Length; j++) Player.Stats[j] -= Invetory[DraggingSlot].item.Stats[j];
                            for (int j = 0; j < Player.Stats.Length; j++) Player.Stats[j] += slot.item.Stats[j];
                        }
                        else if (i >= 4 && DraggingSlot < 4)
                        {
                            if (Invetory[DraggingSlot].item != null) for (int j = 0; j < Player.Stats.Length; j++) Player.Stats[j] += Invetory[DraggingSlot].item.Stats[j];
                            for (int j = 0; j < Player.Stats.Length; j++) Player.Stats[j] -= slot.item.Stats[j];
                        }
                    }
                    /*else
                    {
                        BasicEntity.Add(new Bag((int)Game1._sprites[0].Position.X, (int)Game1._sprites[0].Position.Y) { items = new[] { DraggingItem ?? Invetory[DraggingSlot].item } });
                        if (DraggingItem == null) Invetory[DraggingSlot].item = null;
                        else DraggingItem = null;
                    }*/
                }
            }
        }
    }
}