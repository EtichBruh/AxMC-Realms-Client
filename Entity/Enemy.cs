using AxMC.Camera;
using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nekoT;
using System;
using System.Collections.Generic;

namespace AxMC_Realms_Client.Entity
{
    class Enemy : SpriteAtlas
    {
        byte id = 0;
        int MaxHP = 100;
        public static Vector2 NearestPlayer;
        public static Texture2D tempText;
        public ProgressBar HPbar;
        private double timer, timera = 1;
        const int FramesOffset = 15; // 15 is length of player frames

        public Enemy(Texture2D SpriteSheet)
            : base(SpriteSheet, 6, 5, FramesOffset)
        {
            Width = 50;
            Height = 50;
            HPbar = new(Color.Red,true, true, 100,8);
            HPbar.SetFactor(MaxHP,30);
        }

        public override void Update(GameTime gameTime, List<SpriteAtlas> spritesToAdd)
        {
            for (int i = 0; i < Game1._PlayerBullets.Length; i++)
            {
                Bullet b = Game1._PlayerBullets[i];
                if (!b.enemy && (b.Position - Position).LengthSquared() < 2501)
                {
                    isRemoved = ((HPbar.Progress -= b.Damage) <= 0);
                    b.isRemoved = true;
                    if (isRemoved)
                    {
                        BasicEntity.Add(new Bag((int)Position.X, (int)Position.Y));
                        Random r = new();
                        Position.X -= 25;
                        Position.Y -= 25;
                        BasicEntity.Add(new Portal(0, r.Next((int)Position.X, (int)Position.X + 50), r.Next((int)Position.Y, (int)Position.Y + 50)));
                        Player.XP += 10;
                        Dispose();
                        return;
                    }
                }
            }
            PreviousFrame = CurrentFrame;
            Rotation = -Camera.RotDegr;
            timer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (timer < 0.8 && CurrentFrame == 4 + FramesOffset || CurrentFrame == 9 + FramesOffset || CurrentFrame == 14 + FramesOffset)
            {
                CurrentFrame--;
            }
            if (timer <= 0)
            {
                timer = timera;
                Shoot(spritesToAdd, 3);
            }
            HPbar.Update(Position.X, Position.Y);
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
                var Direction = Vector2.Normalize(NearestPlayer - Position);
                float Rotation = MathF.Atan2(Direction.Y, Direction.X) + Bullet.TexOffset;
                if (Rotation > 0 && Rotation < Bullet.TexOffset * 2)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 4 + FramesOffset;
                }
                else if (Rotation > Bullet.TexOffset * 2 && Rotation < Bullet.TexOffset * 4)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 9 + FramesOffset;
                }
                else if (Rotation < 0 && Rotation > -Bullet.TexOffset * 2)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 14 + FramesOffset;
                }
                else
                {
                    Effect = SpriteEffects.FlipHorizontally;
                    CurrentFrame = 4 + FramesOffset;
                }
                b.Speed = 5;
                b.LifeSpan = 2;
                b.enemy = true;
                b.Damage = 10;

                float someoffset = -1 + bulllets * 0.5f;
                for (int i = 0; i <= bulllets; i++)
                {
                    Bullet bb = b.Clone() as Bullet;
                    bb.Position = Position;
                    bb.Direction.X = MathF.Cos(Rotation - Bullet.TexOffset * (i - someoffset));
                    bb.Direction.Y = MathF.Sin(Rotation - Bullet.TexOffset * (i - someoffset));
                    bb.Rotation = MathF.Atan2(bb.Direction.Y, bb.Direction.X) + Bullet.TexOffset;
                    Game1._EnemyBullets.Add(bb);
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
                if (b.Rotation > 0 && b.Rotation < Bullet.TexOffset * 2)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 4;
                }
                else if (b.Rotation > Bullet.TexOffset * 2 && b.Rotation < Bullet.TexOffset * 4)
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
                b.enemy = true;
                b.Damage = 27;
                Game1._EnemyBullets.Add(b);
                //_spawnedBullets += 0.1f;
            }
        }
    }
}
