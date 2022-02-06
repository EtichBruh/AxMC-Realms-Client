using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nekoT;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxMC_Realms_Client.Entities
{
    public class Bullet : SpriteAtlas
    {
        public float LifeSpan;
        public float Speed;
        
        public Bullet(Texture2D spriteSheet)
        :base(spriteSheet, 1,1,0){
            Width = 32;
            Height = 32;
        }
        public override void Update(GameTime gameTime, List<SpriteAtlas> spritesToAdd)
        {
            LifeSpan -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(LifeSpan <= 0)
            {
                isRemoved = true;
            }
            else
            {
                Position += Direction * Speed;
            }
        }
    }
}
