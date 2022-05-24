using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxMC_Realms_Client.Entities
{
    public class Portal : BasicEntity
    {
        public byte id = 1;
        public Portal(int x, int y)
        {
            SpriteSheetID = 1; // Portal spritesheet
            Rect.X = x;
            Rect.Y = y;
            Rect.Width = 24;
            Rect.Height = 24;
            SrcRect = new(0, 0, 8, 8);
        }
    }
}
