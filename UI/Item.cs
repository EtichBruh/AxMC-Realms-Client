using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxMC_Realms_Client.UI
{
    public class Item
    {
        public int[] Stats = { 500, 50, 20, 10 }; // HP Damage agility armor
        public static Texture2D SpriteSheet;
        public byte id = 0;
        public static string[] Desc= new string[] { "Feel the Drip", "It feels you w power", "Sussy staff lmao" };
    }
}
