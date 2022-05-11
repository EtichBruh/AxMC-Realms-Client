using AxMC.Camera;
using AxMC_Realms_Client.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nekoT;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxMC_Realms_Client.Entities
{
    class Enemy : SpriteAtlas
    {
        byte id = 0;
        int MaxHP = 100,HP = 100;
        public static Vector2 NearestPlayer;
        public ProgressBar HPbar;
        private double timer,timera = 1;

        public Enemy(Texture2D SpriteSheet)
            : base(SpriteSheet, 3, 5, 0)
        {
            Width = 50;
            Height = 50;
            HPbar = new(SpriteSheet.GraphicsDevice);
        }


        public override void Update(GameTime gameTime, List<SpriteAtlas> spritesToAdd)
        {
            PreviousFrame = CurrentFrame;
            Rotation = -Camera.RotDegr;
            for (int i = 0; i < Game1._bullets.Length; i++)
            {
                if (Game1._bullets[i].parent is not Enemy && (Game1._bullets[i].Position - Position).LengthSquared() <= 2500) {
                    HP--;
                    HPbar.ProgressValue = HP / (MaxHP / 100);
                    Game1._bullets.RemoveAt(i);
                    i--;
                }
                if (isRemoved = (HP <= 0)) {
                    BasicEntity.InteractEnt.Add(new Portal((int)Position.X, (int)Position.Y));
                    Dispose();
                    break; }
            }
            timer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (timer <= 0)
            {
                timer = timera;
                Shoot(spritesToAdd,3);
            }
            HPbar.Update((int)Position.X, (int)(Position.Y + Height * 0.5f));
            if (PreviousFrame != CurrentFrame)
            {
                var columns = (Texture.Width / _srcRect.Width);
                _srcRect.X = _srcRect.Width * (CurrentFrame % columns);
                _srcRect.Y = _srcRect.Height * (CurrentFrame / columns);
            }
        }
        private void Shoot(List<SpriteAtlas> spritesToAdd, int bulllets)
        {
            if (NearestPlayer != Vector2.Zero)
            {
                Bullet b = Player._bullet.Clone() as Bullet;
                b.Position = Position;
                b.Direction = Vector2.Normalize(NearestPlayer - Position);
                b.Rotation = MathF.Atan2(b.Direction.Y, b.Direction.X) + Bullet.TexOffset;
                if (b.Rotation > 0 && b.Rotation < Bullet.TexOffset *2)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 4;
                }
                else if (b.Rotation > Bullet.TexOffset *2 && b.Rotation < Bullet.TexOffset * 4)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 9;
                }
                else if (b.Rotation < 0 && b.Rotation > -Bullet.TexOffset * 2)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 13;
                }
                else
                {
                    Effect = SpriteEffects.FlipHorizontally;
                    CurrentFrame = 4;
                }
                b.Speed = 5;
                b.LifeSpan = 2;
                b.parent = this;

                float someoffset = -1 + bulllets * 0.5f;
                    for (int i = 0; i <= bulllets; i++)
                {
                    Bullet bb = b.Clone() as Bullet;
                    bb.Direction.X = MathF.Cos(bb.Rotation - Bullet.TexOffset * (i - someoffset));
                    bb.Direction.Y = MathF.Sin(bb.Rotation - Bullet.TexOffset * (i - someoffset));
                    bb.Rotation = MathF.Atan2(bb.Direction.Y, bb.Direction.X) + Bullet.TexOffset;
                    spritesToAdd.Add(bb);
                }
                //spritesToAdd.Add(b);
            }
        }
        private void Shoot(List<SpriteAtlas> spritesToAdd)
        {
            if (NearestPlayer != Vector2.Zero)
            {
                Bullet b = Player._bullet.Clone() as Bullet;
                b.Position = Position;
                b.Direction = Vector2.Normalize(NearestPlayer - Position);
                b.Rotation = MathF.Atan2(b.Direction.Y, b.Direction.X) + Bullet.TexOffset;
                if (b.Rotation > 0 && b.Rotation < Bullet.TexOffset *2)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 4;
                }
                else if (b.Rotation > Bullet.TexOffset *2 && b.Rotation < Bullet.TexOffset * 4)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 9;
                }
                else if (b.Rotation < 0 && b.Rotation > -Bullet.TexOffset *2)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 13;
                }
                else
                {
                    Effect = SpriteEffects.FlipHorizontally;
                    CurrentFrame = 4;
                }
                b.Speed = 5;
                b.LifeSpan = 2;
                b.parent = this;
                spritesToAdd.Add(b);
                //_spawnedBullets += 0.1f;
            }
        }
    }
}
