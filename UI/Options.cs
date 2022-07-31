using AxMC_Realms_Client.Classes;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxMC_Realms_Client.UI
{
    class Options
    {
        Button MoveUp, MoveDown, MoveLeft, MoveRight;
        public Options()
        {
            MoveUp = new() { SetText = Input.MoveUp.ToString() };
            MoveDown = new() { SetText = Input.MoveDown.ToString() };
            MoveLeft = new() { SetText = Input.MoveLeft.ToString() };
            MoveRight = new() { SetText = Input.MoveRight.ToString() };
        }
        public void Draw(SpriteBatch sb)
        {
            Panel.Draw(sb, UI.SlotSprite, new(200, 150), 400, 300);
        }
    }
}