using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Entity;
using AxMC_Realms_Client.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nekoT;
using Nez;

namespace AxMC_Realms_Client.UI
{
    public class UI
    {
        public static Texture2D SlotSprite;
        public static ProgressBar HPBar, MPBar;
        public static Rectangle MRect;

        public Texture2D BagUI, PEnterUI, ExpJar, StatIcons;
        public Rectangle BagUISRect, BagUIRect = new(0, 0, 64, 32);
        public Rectangle ExpJarRect = new(0, 0, 32, 92);
        public Rectangle PEnterUIRect = new(0, 0, 32, 16);
        public Rectangle StatsRect = new(0, 0, 16, 16);

        Slot[] Invetory = new Slot[12];

        //Slot[] Equipment = new Slot[4];

        bool isDrag = false;
        bool DrawPortalButt = false;
        int DragSlot = -1;
        int DragItem = -1;

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
            HPBar = new(Color.Red, false, false, Player.HPbar.Progress, 14);
            MPBar = new(Color.Blue, false, false, 200, 14);

            HPBar.SetFactor(Player.Stats[0], 32 * 4);
            MPBar.SetFactor(Player.Stats[1], 32 * 4);

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
                    Invetory[x].Rect.Y = sHCenter + 32; // 32 is height of rect
                    Invetory[x].Rect.X = (x - 4 - 3) * (Invetory[x].Rect.Width) + smth;
                }
            }

            var temp = Invetory[4].Rect.Size.MultiplyBy(.5f);

            ExpJarRect.X = Invetory[4].Rect.X - temp.X - ExpJar.Width;
            ExpJarRect.Y = Invetory[4].Rect.Y + temp.Y - ExpJar.Height;

            int HpBarY = Invetory[0].Rect.Y - 14 - 16;
            int HpBarX = ExpJarRect.Right;

            HPBar.Update(HpBarX, HpBarY);
            MPBar.Update(HpBarX + 32 * 4, HpBarY);

            PEnterUIRect = new(Invetory[3].Rect.X + 16, HpBarY + 14, Invetory[0].Rect.Width * 2, 32);
        }
        public void Update(FastList<SpriteAtlas> entities)
        {
            MRect = new Rectangle(Input.MState.Position, new Point(1, 1));
            HoveringOnSlot(entities);

            if (BasicEntity.GetNear() is Portal portal)
            {
                DrawPortalButt = true;
                if (Input.KState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) || Input.MState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                    PEnterUIRect.Intersects(new Rectangle(Input.MState.Position, new Point(1, 1))))
                {
                    string a = ((Map.Maps)portal.id).ToString();
                    Map.Map.Load(a, entities);
                }
            }
            else { DrawPortalButt = false; }
        }
        const float factor = 1f / (100f / 92f);
        public void Draw(SpriteBatch sb, Effect outline)
        {
            // 50
            sb.DrawString(Game1.Arial, Player.Stats[3].ToString(), new Vector2(StatsRect.Right, StatsRect.Y), Color.LightGreen, 0, new(0, 50), 0.1f, 0, 0);
            sb.Draw(StatIcons, StatsRect, new Rectangle(0, 0, 32, 32), Color.White, 0, new(0, 16), 0, 0);
            // 74
            StatsRect.Y += 16 + 8;
            sb.Draw(StatIcons, StatsRect, new Rectangle(32, 0, 32, 32), Color.White, 0, new(0, 16), 0, 0);
            sb.DrawString(Game1.Arial, Player.Stats[2].ToString(), new Vector2(StatsRect.Right, StatsRect.Y), Color.DarkRed, 0, new(0, 50), 0.1f, 0, 0);
            // 
            StatsRect.Y -= 32 + 16;
            sb.DrawString(Game1.Arial, Player.Stats[4].ToString(), new Vector2(StatsRect.Right, StatsRect.Y), Color.Gray, 0, new(0, 50), 0.1f, 0, 0);
            sb.Draw(StatIcons, StatsRect, new Rectangle(64, 0, 32, 32), Color.White, 0, new(0, 16), 0, 0);

            StatsRect.Y += 16 + 8;// Reset back

            if (Player.XP > 0)
            {
                int height = (int)(Player.XP * factor);
                sb.Draw(ProgressBar.Pixel, new Rectangle(ExpJarRect.X, ExpJarRect.Bottom - height - 10, ExpJarRect.Width, height), Color.LightGray);
            }
            if (Player.HPbar.Progress > 0)
            {
                //var rect = new Rectangle(ExpJarRect.Right, Invetory[0].Rect.Y - 14 - 16, (int)(((float)Player.HPbar.Progress / Player.Stats[0]) * 32 * 4), 14);
                //sb.Draw(ProgressBar.Pixel, rect, Color.Red);
                HPBar.Draw(sb);
                MPBar.Draw(sb);
                var str = $"{ Player.HPbar.Progress}/{Player.Stats[0]}";
                sb.DrawString(Game1.Arial, str, new Vector2(HPBar.Pos.X + 64 - Game1.Arial.MeasureString(str).X * .05f, HPBar.Pos.Y + 2), Color.White, 0, Vector2.Zero, 0.1f, 0, 0);
                str = $"100/100";
                sb.DrawString(Game1.Arial, str, new Vector2(MPBar.Pos.X + 64 - Game1.Arial.MeasureString(str).X * .05f, HPBar.Pos.Y + 2), Color.White, 0, Vector2.Zero, 0.1f, 0, 0);
            }
            if (DrawPortalButt)
            {
                sb.Draw(PEnterUI, PEnterUIRect, Color.White);
            }
            sb.Draw(ExpJar, ExpJarRect, Color.White); // Jar is in front of liquid
            sb.End();
            outline.Parameters["OutlineCol"].SetValue(Color.Black.ToVector4());
            sb.Begin(samplerState: SamplerState.PointClamp, effect: outline);
            for (int i = 0; i < Invetory.Length; i++)
            {
                Invetory[i].Draw(sb, SlotSprite);// Draw inv slot
            }
            sb.End();
            outline.Parameters["OutlineCol"].SetValue(Color.Black.ToVector4());
            sb.Begin(samplerState: SamplerState.PointClamp);
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

                        for (int i = 0; i < len; i++)
                        {
                            BagUIRect.X += BagUIRect.Width;
                            itempos.X += BagUIRect.Width;
                            if (i < len - 1) sb.Draw(BagUI, BagUIRect, slotRect, Color.White); // Draw slot

                            Item.Draw(sb, itempos, 30, new Rectangle((int)itempos.X - 18, BagUIRect.Y, 30, 30).Intersects(MRect), bag.items[i]);
                        }
                        BagUISRect = new(0, 0, 64, 32);
                        BagUIRect = new(prevX, prevY, 64, 32);// reset rects back
                    }
                    else if (len == 2)
                    {
                        sb.Draw(BagUI, BagUIRect, null, Color.White, 0, new(BagUI.Width * .5f, 0), 0, 0);
                        float y = BagUIRect.Y + BagUI.Height * .5f;
                        Item.Draw(sb, new(BagUIRect.X - BagUIRect.Width * .25f, y), 30, false, bag.items[0]);
                        Item.Draw(sb, new(BagUIRect.X + BagUIRect.Width * .25f, y), 30, false, bag.items[1]);
                    }
                }
                //else sb.Draw(PEnterUI, PEnterUIRect, Color.White);
            }
            if (isDrag)
            {
                if (DragItem != -1)
                {
                    Item.Draw(sb, Input.MState.Position.ToVector2(), 32, false, DragItem);
                }
                else
                {
                    Item.Draw(sb, Input.MState.Position.ToVector2(), 32, false, Invetory[DragSlot].item);
                }
            }
        }
        void HoveringOnSlot(FastList<SpriteAtlas> ents)
        {
            bool LBpressed = Input.MState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            bool LBreleased = Input.MState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released;
            if (BasicEntity.NInteract != -1 && BasicEntity.InteractEnt[BasicEntity.NInteract] is Bag bag)
            {
                var itemRect = BagUIRect;
                itemRect.X -= 15 * (bag.items.Length - 1) + 30 + 18; // + 18 because 1 slot is 2 drawn slot segment and theyre offset by 1st segment width
                for (int i = 0; i < bag.items.Length; i++)
                {
                    if (!isDrag && LBpressed && itemRect.Intersects(MRect))
                    {
                        DragItem = bag.items[i];
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
                if (slot.mouseHoverOn = r.Intersects(MRect))
                {
                    if (!isDrag && slot.item != -1 && DragItem == -1 && LBpressed)
                    {
                        isDrag = true;

                        DragSlot = i;
                    }
                }
                if (isDrag && DragSlot != i && LBreleased)
                {
                    if (DragItem != -1 && slot.mouseHoverOn && slot.item == -1) // this is for dragging for non inv slot
                    {
                        slot.item = DragItem;
                        if (i < 4)
                        {
                            for (int j = 0; j < Player.Stats.Length; j++) Player.Stats[j] += Item.items[DragItem].Stats[j];
                        }
                        DragItem = -1;
                        isDrag = false;

                        continue;
                    }
                    else if (slot.mouseHoverOn && DragItem == -1)
                    {
                        var SwapItem = Invetory[DragSlot].item;

                        Invetory[DragSlot].item = slot.item;
                        slot.item = SwapItem;

                        isDrag = false;

                        if (i < 4 && DragSlot >= 4)
                        {// this is when dragged in equip slots
                            CalcStats(slot.item, true);
                        }
                        else if (i >= 4 && DragSlot < 4)
                        {//this is when dragged out of equip slots
                            CalcStats(slot.item, false);
                        }
                    }
                }
            }
            if (isDrag && DragItem == -1 && LBreleased)
            {
                var DragItem = Invetory[DragSlot].item;
                if ( BasicEntity.GetNear() is Bag _bag)
                {
                    _bag.items.Add(DragItem);
                }
                else
                {
                    BasicEntity.Add(new Bag((int)ents[0].Position.X, (int)ents[0].Position.Y) { items = new() { Length = 1, Buffer = new[] { DragItem } } });
                }
                Invetory[DragSlot].item = -1;
                if (DragSlot < 4)
                {
                    CalcStats(DragItem, false);
                }
                isDrag = false;
            }
        }
        void CalcStats(int item, bool DraggedInEquip)
        {
            var _item = Invetory[DragSlot].item;
            if (DraggedInEquip)
            {
                if (Invetory[DragSlot].item != -1) for (int j = 0; j < Player.Stats.Length; j++) Player.Stats[j] -= Item.items[_item].Stats[j];
                for (int j = 0; j < Player.Stats.Length; j++) Player.Stats[j] += Item.items[item].Stats[j];
                return;
            }
            if (Invetory[DragSlot].item != -1) for (int j = 0; j < Player.Stats.Length; j++) Player.Stats[j] += Item.items[_item].Stats[j];
            for (int j = 0; j < Player.Stats.Length; j++) Player.Stats[j] -= Item.items[item].Stats[j];
        }
    }
}