using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nekoT;
using System;
using System.Collections.Generic;

namespace AxMC_Realms_Client.Entities
{
    public class Bullet : SpriteAtlas
    {
        public byte id = 0;
        public double LifeSpan;
        public float Speed;
        public const float TexOffset = 0.7853982f; //MathHelper.ToRadians(45)
        public int Damage = 0;
        public bool enemy;

        public Bullet(Texture2D spriteSheet)
        : base(spriteSheet, 1, 1, 0)
        {
            Width = 32;
            Height = 32;
        }
        public override void Update(GameTime gameTime, List<SpriteAtlas> spritesToAdd)
        {
            if (isRemoved == true) return;
            LifeSpan -= gameTime.ElapsedGameTime.TotalSeconds;
            if (isRemoved = LifeSpan <= 0) {
            }
            else
            {
                
                Position += Direction * Speed;
                if(Position.X < 0 || Position.Y < 0) { isRemoved = true;return; }
                int index = (int)Position.X / 50 + ((int)Position.Y / 50) * Map.Map.Size.X;
                isRemoved = (index < Game1.MapTiles.Length && index > -1 && Game1.MapTiles[index] is null);
            }
        }
    }
}
