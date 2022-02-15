using Microsoft.Xna.Framework.Graphics;
using nekoT;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxMC_Realms_Client.Entities
{
    class Enemy : SpriteAtlas
    {
        public Enemy(Texture2D SpriteSheet)
            : base(SpriteSheet, 0, 0, 0)
        {

        }
    }
}
