using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC_Realms_Client.UI
{
    class Options
    {
        public bool Active = false;
        const int width = 300;
        const int height = 300;

        Button MoveUp, MoveDown, MoveLeft, MoveRight,
            SlotSize,
            AllowRotate,
            Title;
        public Vector2 Position;

        public Options(int sw, int sh, UI ui, Player player)
        {
            MoveUp = new(ButtonType.Small, Input.MoveUp.ToString());
            MoveDown = new(ButtonType.Small, Input.MoveDown.ToString());
            MoveLeft = new(ButtonType.Small, Input.MoveLeft.ToString());
            MoveRight = new(ButtonType.Small, Input.MoveRight.ToString());

            Position = new((sw - width) * .5f, (sh - height) * .5f);
            // sorry for hardcode 15 is height of top corner in Panel
            Title = new((sw - 64) / 2,(int)Position.Y - 15 - 6, 64,32,ButtonType.Big, "Options");

            // 40 is offset from right side
            SlotSize = new((sw + width) / 2 - 40, (int)Position.Y + 16,28,28,ButtonType.Small, ui.SlotSizeMultiplier.ToString());
            AllowRotate = new((sw + width) / 2 - 40, SlotSize.rect.Bottom + 4, 44,36,ButtonType.Medium, player.AllowRotation.ToString());
        }
        public void Resize(int sw, int sh)
        {
            Position = new((sw - width) * .5f, (sh - height) * .5f);
            // sorry for hardcode 15 is height of top corner in Panel
            Title = new((sw - 64) / 2, (int)Position.Y - 16 - 6, 64, 32, ButtonType.Big, "Options");

            SlotSize.rect = new((sw + width) / 2 - 40, (int)Position.Y + 16, 28, 28);

            AllowRotate.rect = new((sw + width) / 2 - 44 - 4, SlotSize.rect.Bottom + 4, 44, 36);

        }
        public void Update(UI ui, Player player)
        {
            if (SlotSize.Update())
            {
                float size = ui.SlotSizeMultiplier;
                ui.SlotSizeMultiplier += size == 2 ? -1 : 0.5f;
                SlotSize.SetText = ui.SlotSizeMultiplier.ToString();
            }
            if (AllowRotate.Update())
            {
                player.AllowRotation = !player.AllowRotation;
                AllowRotate.SetText = player.AllowRotation.ToString();
            }
        }
        public void Draw(SpriteBatch sb)
        {
            Panel.Draw(sb, UI.SlotSprite, Position, width, height);

            sb.DrawString(Game1.Arial, "Inventory slot size", new(Position.X + 10, SlotSize.rect.Y ), Color.White,0,new(0,-50),0.12f,0,0);
            sb.DrawString(Game1.Arial, "Allow Camera Rotation", new(Position.X + 10, AllowRotate.rect.Y ), Color.White,0,new(0,-50),0.12f,0,0);

            SlotSize.Draw(sb);
            AllowRotate.Draw(sb);

            Title.Draw(sb);
        }
    }
}