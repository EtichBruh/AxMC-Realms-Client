using AxMC.Camera;
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
        private double WalkAnim = 0;
        const int FramesOffset = 15; // 15 is length of player frames
        static Point[] Frames;

        public Enemy(Texture2D SpriteSheet)
            : base(SpriteSheet, 6, 5, FramesOffset)
        {
            Width = 50;
            Height = 50;
            HPbar = new(Color.Red, true, true, 100, 8);
            HPbar.SetFactor(MaxHP, 30);
            int columns = 5;
            int rows = 3;
            Frames = new Point[columns * rows];
            for (int i = 0; i < Frames.Length; i++)
            {
                Frames[i].X = _srcRect.Width * ((i + FramesOffset) % columns);
                Frames[i].Y = _srcRect.Height * ((i + FramesOffset) / columns);
            }
        }
        public override void Update(GameTime gameTime, List<SpriteAtlas> spritesToAdd)
        {
            for (int i = 0; i < Game1._PlayerBullets.Length; i++)
            {
                Bullet b = Game1._PlayerBullets[i];
                if (!b.isRemoved && (b.Position - Position).LengthSquared() < 2501)
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
            if (timer < 0.8)
            {
                if (CurrentFrame == 4 || CurrentFrame == 9 || CurrentFrame == 14)
                    CurrentFrame--;
            }
            if (WalkAnim >= 0.2)
            {
                if (CurrentFrame == 1 || CurrentFrame == 6 || CurrentFrame == 11)
                    CurrentFrame++;
            }
            bool seeplayer = (NearestPlayer - Position).LengthSquared() < 500 * 500;
            if (timer < 0)
            {
                timer = timera;
                if (seeplayer) Shoot(spritesToAdd, 3);
                else
                {
                    Direction.X = (float)new Random().NextDouble() * 2 - 1;
                    Direction.Y = (float)new Random().NextDouble() * 2 - 1;
                    if (Position.X < 0 || Position.X > Map.Map.Size.X * 50) Direction.X = -Direction.X;
                    if (Position.Y < 0 || Position.Y > Map.Map.Size.Y * 50) Direction.Y = -Direction.Y;
                }
            }
            if (seeplayer)
            {
                Direction = Vector2.Normalize(NearestPlayer - Position);
                float Rotation = MathF.Atan2(Direction.Y, Direction.X) + Bullet.TexOffset;
                if (Rotation > 0 && Rotation < Bullet.TexOffset * 2)
                {
                    Effect = SpriteEffects.None;
                    if (WalkAnim < 0.2)
                        CurrentFrame = 1;
                }
                else if (Rotation > Bullet.TexOffset * 2 && Rotation < Bullet.TexOffset * 4)
                {
                    Effect = SpriteEffects.None;
                    if (WalkAnim < 0.2)
                        CurrentFrame = 6;
                }
                else if (Rotation < 0 && Rotation > -Bullet.TexOffset * 2)
                {
                    Effect = SpriteEffects.None;
                    if (WalkAnim < 0.2)
                        CurrentFrame = 11;
                }
                else
                {
                    Effect = SpriteEffects.FlipHorizontally;
                    if (WalkAnim < 0.2)
                        CurrentFrame = 1;
                }

                if (WalkAnim > 0.4)
                {
                    WalkAnim = 0;
                }
                WalkAnim += gameTime.ElapsedGameTime.TotalSeconds;

            }
            Position += Direction;
            HPbar.Update(Position.X, Position.Y);
            if (PreviousFrame != CurrentFrame)
            {
                _srcRect.Location = Frames[CurrentFrame];
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
                    CurrentFrame = 4;
                }
                else if (Rotation > Bullet.TexOffset * 2 && Rotation < Bullet.TexOffset * 4)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 9;
                }
                else if (Rotation < 0 && Rotation > -Bullet.TexOffset * 2)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 14;
                }
                else
                {
                    Effect = SpriteEffects.FlipHorizontally;
                    CurrentFrame = 4;
                }
                b.Speed = 5;
                b.LifeSpan = 2;
                b.Damage = 10;

                float someoffset = -1.5f + bulllets * 0.5f;
                for (int i = 0; i < bulllets; i++)
                {
                    Bullet bb = b.Clone() as Bullet;
                    bb.Position = Position;
                    bb.Direction.X = MathF.Cos(Rotation - Bullet.TexOffset * (i - someoffset));
                    bb.Direction.Y = MathF.Sin(Rotation - Bullet.TexOffset * (i - someoffset));
                    bb.Rotation = MathF.Atan2(bb.Direction.Y, bb.Direction.X) + Bullet.TexOffset;
                    Game1._EnemyBullets.Add(bb);
                }
            }
        }
    }
}
