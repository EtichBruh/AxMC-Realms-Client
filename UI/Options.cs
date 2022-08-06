using AxMC_Realms_Client.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxMC_Realms_Client.UI
{
    class Options
    {
        Button MoveUp, MoveDown, MoveLeft, MoveRight,
            SlotSize,
            Title;
        Vector2 Position;
        public bool Active = false;
        const int width = 250;
        public Options(int sw, int sh)
        {
            MoveUp = new(ButtonType.Small, Input.MoveUp.ToString());
            MoveDown = new(ButtonType.Small, Input.MoveDown.ToString());
            MoveLeft = new(ButtonType.Small, Input.MoveLeft.ToString());
            MoveRight = new(ButtonType.Small, Input.MoveRight.ToString());

            SlotSize = new(ButtonType.Small, UI.SlotSizeMultiplier.ToString());

            Position = new((sw - width) * .5f, (sh - 300) * .5f);
            // sorry for hardcode 15 is height of top corner in Panel
            Title = new((sw - 64) / 2,(int)Position.Y - 15 - 6, 64,32,ButtonType.Big, "Options");
        }
        public void Update()
        {
            if (SlotSize.Update())
            {
                float size = UI.SlotSizeMultiplier;
                UI.SlotSizeMultiplier += size == 2 ? -1 : 0.5f;
                SlotSize.SetText = UI.SlotSizeMultiplier.ToString();
            }

        }
        public void Draw(SpriteBatch sb)
        {
            Panel.Draw(sb, UI.SlotSprite, Position, width, 300);
            Title.Draw(sb);
        }
    }
}