using AxMC.Camera;
using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using nekoT;
using System;
using System.Collections.Generic;

namespace AxMC_Realms_Client.Entities
{
    public class Player : SpriteAtlas
    {
        private Bullet _bullet;
        public static Point TiledPos;
        public static Point xyCount;
        public static int SquareOfSightStartIndex;
        public static ProgressBar HPbar;
        public Player(Texture2D spriteSheet, Texture2D BulletTexture) :
            base(spriteSheet, 3, 5, 0)
        {
            Input.setKeys();
            _bullet = new(BulletTexture);
            TiledPos = (Position / 50).ToPoint();
            GetSquareOfSight();
            Width = 64;
            Height = 64;
            HPbar = new(spriteSheet.GraphicsDevice);
        }
        public override void Update(GameTime gameTime, List<SpriteAtlas> spritesToAdd)
        {
            Move();
            Shoot(spritesToAdd);
            if (CurrentFrame >= 0)
            {
                var columns = (Texture.Width / _width);
                _destRect.X = _width * (CurrentFrame % columns);
                _destRect.Y = _height * (CurrentFrame / columns);
            }
            RotateZoom();
            // base.Update(gameTime, spritesToAdd);
        }
        private void Move()
        {
            if (Input.KState.IsKeyDown(Input.MoveDown))
            {
                Direction.Y = 1;
                CurrentFrame = 5;
                Effect = SpriteEffects.None;
            }
            if (Input.KState.IsKeyDown(Input.MoveUp))
            {
                Direction.Y = -1;
                CurrentFrame = 10;
                Effect = SpriteEffects.None;
            }
            if (Input.KState.IsKeyDown(Input.MoveLeft))
            {
                Direction.X = -1;
                CurrentFrame = 0;
                Effect = SpriteEffects.FlipHorizontally;
            }
            if (Input.KState.IsKeyDown(Input.MoveRight))
            {
                Direction.X = 1;
                CurrentFrame = 0;
                Effect = SpriteEffects.None;
            }
            if (Direction != Vector2.Zero)
            {
                var FuturePos = Position + Direction;
                if (FuturePos.X < 0 || FuturePos.X > Map.Map.MapSize.X * 50) Direction.X = 0;
                if (FuturePos.Y < 0 || FuturePos.Y > Map.Map.MapSize.Y * 50) Direction.Y = 0;
                Position += Direction;
                TiledPos = (Position / 50).ToPoint();
                GetSquareOfSight();
                Direction = Vector2.Zero;
            }
        }
        private void Shoot(List<SpriteAtlas> spritesToAdd)
        {
            if (Input.MState.LeftButton == ButtonState.Pressed)
            {
                Bullet b = _bullet.Clone() as Bullet;
                b.Position = Position;
                b.Direction = Vector2.Normalize(Vector2.Transform(new(Input.MState.X, Input.MState.Y), Matrix.Invert(Camera.CamTransform)) - Position);
                b.Rotation = MathF.Atan2(b.Direction.Y, b.Direction.X) + MathF.Atan(0.9f);
                if (b.Rotation > 0 && b.Rotation < MathF.Atan(0.9f) + MathF.Atan(0.9f))
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 4;
                }
                else if (b.Rotation > MathF.Atan(0.9f) + MathF.Atan(0.9f) && b.Rotation < MathF.Atan(0.9f) * 4)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 9;
                }
                else if (b.Rotation < 0 && b.Rotation > -MathF.Atan(0.9f) - MathF.Atan(0.9f))
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
        /// <summary>
        /// Calculates the start index for square of sight
        /// </summary>
        /// <returns>Start index of square of sight</returns>
        private void GetSquareOfSight(int DrawRadius = 4)
        {
            xyCount.X = Math.Min(DrawRadius, TiledPos.X) + Math.Min(DrawRadius + 1, Map.Map.MapSize.X - TiledPos.X);
            xyCount.Y = Math.Min(DrawRadius, TiledPos.Y) + Math.Min(DrawRadius + 1, Map.Map.MapSize.Y - TiledPos.Y);

            TiledPos.X = Math.Max(0, TiledPos.X - DrawRadius);
            TiledPos.Y = Math.Max(0, TiledPos.Y - DrawRadius);


            SquareOfSightStartIndex = TiledPos.X + TiledPos.Y * Map.Map.MapSize.X;
        }
        private void RotateZoom()
        {
            if (Input.KState.IsKeyDown(Input.ZoomIn))
            {
                Camera.CamZoom += 0.1f;
            }
            if (Input.KState.IsKeyDown(Input.ZoomOut))
            {
                Camera.CamZoom -= 0.1f;
            }
            if (Input.KState.IsKeyDown(Input.RotateCameraLeft))
            {
                Camera.CamRotationDegrees -= MathHelper.ToRadians(1);
                Rotation = -Camera.CamRotationDegrees;
            }
            if (Input.KState.IsKeyDown(Input.RotateCameraRight))
            {
                Camera.CamRotationDegrees += MathHelper.ToRadians(1);
                Rotation = -Camera.CamRotationDegrees;
            }
            if (Input.KState.IsKeyDown(Input.ResetRotation))
            {
                Camera.CamRotationDegrees = 0;
                Rotation = 0;
            }
        }
    }
}
