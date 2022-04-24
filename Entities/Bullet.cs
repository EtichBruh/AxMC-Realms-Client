using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nekoT;
using System.Collections.Generic;

namespace AxMC_Realms_Client.Entities
{
    public class Bullet : SpriteAtlas
    {
        public byte id = 0;
        public double LifeSpan;
        public float Speed;

        public Bullet(Texture2D spriteSheet)
        : base(spriteSheet, 1, 1, 0)
        {
            Width = 32;
            Height = 32;
        }
        public override void Update(GameTime gameTime, List<SpriteAtlas> spritesToAdd)
        {
            LifeSpan -= gameTime.ElapsedGameTime.TotalSeconds;
            if (isRemoved = (LifeSpan <= 0)) { }
            else
            {
                Position += Direction * Speed;
                int index = (int)Position.X / 50 + ((int)Position.Y / 50) * Map.Map.MapSize.X;
                isRemoved = (index < Game1.MapTiles.Length && index > -1 && Game1.MapTiles[index] is null);
            }
        }
    }
}
