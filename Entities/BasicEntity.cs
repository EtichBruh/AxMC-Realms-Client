using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxMC_Realms_Client.Entities
{
    public abstract class BasicEntity
    {
        public static Texture2D SpriteSheet;
        /// <summary>
        /// interactable entities
        /// </summary>
        public static FastList<BasicEntity> InteractEnt = new();
        /// <summary>
        /// nearest interactable entity ID in InteractEnt list
        /// </summary>
        public static int NInteract = -1;
        public Rectangle SrcRect;
        public Rectangle Rect;
         
        public void Draw(SpriteBatch sb)
        {
            
            sb.Draw(SpriteSheet, Rect, SrcRect, Color.White, 0, SrcRect.Size.ToVector2() * .5f, 0, 0);
        }
    }
}
