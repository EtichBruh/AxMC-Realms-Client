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
        byte id;
        string Name = "Sussy portal";
        public Portal(int x, int y)
        {
            Rect.X = x;
            Rect.Y = y;
            Rect.Width = 50;
            Rect.Height = 50;
            SrcRect = new(45, 0, 8, 8);
        }
    }
}
