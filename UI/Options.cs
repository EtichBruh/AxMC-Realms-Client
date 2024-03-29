﻿using AxMC_Realms_Client.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC_Realms_Client.UI
{
    class Options
    {
        public bool Active = false;
        const int width = 250;

        Button MoveUp, MoveDown, MoveLeft, MoveRight,
            SlotSize,
            Title;
        public Vector2 Position;

        public Options(int sw, int sh, UI ui)
        {
            MoveUp = new(ButtonType.Small, Input.MoveUp.ToString());
            MoveDown = new(ButtonType.Small, Input.MoveDown.ToString());
            MoveLeft = new(ButtonType.Small, Input.MoveLeft.ToString());
            MoveRight = new(ButtonType.Small, Input.MoveRight.ToString());


            Position = new((sw - width) * .5f, (sh - 300) * .5f);
            // sorry for hardcode 15 is height of top corner in Panel
            Title = new((sw - 64) / 2,(int)Position.Y - 15 - 6, 64,32,ButtonType.Big, "Options");

            SlotSize = new((sw + width) / 2 - 42, (int)Position.Y + 16,28,28,ButtonType.Small, ui.SlotSizeMultiplier.ToString());
        }
        public void Resize(int sw, int sh)
        {
            Position = new((sw - width) * .5f, (sh - 300) * .5f);
            // sorry for hardcode 15 is height of top corner in Panel
            Title = new((sw - 64) / 2, (int)Position.Y - 16 - 6, 64, 32, ButtonType.Big, "Options");

            SlotSize.rect = new((sw + width) / 2 - 42, (int)Position.Y + 16, 28, 28);


        }
        public void Update(UI ui)
        {
            if (SlotSize.Update())
            {
                float size = ui.SlotSizeMultiplier;
                ui.SlotSizeMultiplier += size == 2 ? -1 : 0.5f;
                SlotSize.SetText = ui.SlotSizeMultiplier.ToString();
            }

        }
        public void Draw(SpriteBatch sb)
        {
            Panel.Draw(sb, UI.SlotSprite, Position, width, 300);
            sb.DrawString(Game1.Arial, "Inventory slot size", new(Position.X + 12, SlotSize.rect.Y ), Color.White,0,Vector2.Zero,0.12f,0,0);
            SlotSize.Draw(sb);
            Title.Draw(sb);
        }
    }
}