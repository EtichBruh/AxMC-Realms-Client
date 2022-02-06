using nekoT;
using AxMC_Realms_Client.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using AxMC.Camera;

namespace AxMC_Realms_Client.Entities
{
    public class Player : SpriteAtlas
    {
        private Bullet _bullet;
        public static Point TiledPos;
        public static Point xyCount;
        public static int SquareOfSightStartIndex;
        public Player(Texture2D spriteSheet, Texture2D BulletTexture):
            base(spriteSheet,3,5,0)
        {
            Input.setKeys();
            _bullet = new(BulletTexture);
            TiledPos = (Position / 50).ToPoint();
            GetSquareOfSight();
            Width = 64;
            Height = 64;
        }
        public override void Update(GameTime gameTime, List<SpriteAtlas> spritesToAdd)
        {
            Move();
            
            Shoot(spritesToAdd);
            RotateZoom();
           // base.Update(gameTime, spritesToAdd);
        }
        private void Move()
        {
            if (Input.KState.IsKeyDown(Input.MoveUp))
            {
                Direction.Y = -1;
            }
             if (Input.KState.IsKeyDown(Input.MoveRight))
            {
                Direction.X = 1;
            }
             if (Input.KState.IsKeyDown(Input.MoveLeft))
            {
                Direction.X = -1;
            }
             if (Input.KState.IsKeyDown(Input.MoveDown))
            {
                Direction.Y = 1;
            }
            Position += Direction;
            TiledPos = (Position / 50).ToPoint();
            GetSquareOfSight();
            Direction = Vector2.Zero;
        }
        private void Shoot(List<SpriteAtlas> spritesToAdd)
        {
            if(Input.MState.LeftButton == ButtonState.Pressed)
            {
                Bullet b = _bullet.Clone() as Bullet;
                b.Position = Position;
                //if (_spawnedBullets > 360) _spawnedBullets = 0;
               // b.Direction.X += MathF.Cos(_spawnedBullets);
                //b.Direction.Y += MathF.Sin(_spawnedBullets);
                b.Direction = Vector2.Normalize(Vector2.Transform(new(Input.MState.X, Input.MState.Y), Matrix.Invert(Camera.CamTransform)) - Position);
                b.Rotation = MathF.Atan2(b.Direction.Y, b.Direction.X) + MathF.Atan(0.9f);
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
            xyCount.X = Math.Min(DrawRadius, TiledPos.X) + Math.Min(DrawRadius + 1, Map.Map.MapWidth - TiledPos.X);
            xyCount.Y = Math.Min(DrawRadius, TiledPos.Y) + Math.Min(DrawRadius + 1, Map.Map.MapHeight - TiledPos.Y);

            TiledPos.X = Math.Max(0, TiledPos.X - DrawRadius);
            TiledPos.Y = Math.Max(0, TiledPos.Y - DrawRadius);
            

            SquareOfSightStartIndex = TiledPos.X + TiledPos.Y * Map.Map.MapWidth;
        }
        private void RotateZoom()
        {
            if (Input.KState.IsKeyDown(Input.ZoomIn))
            {
                Camera.CamZoom+= 0.1f;
            }
            if (Input.KState.IsKeyDown(Input.ZoomOut))
            {
                Camera.CamZoom -= 0.1f;
            }
            if (Input.KState.IsKeyDown(Input.RotateCameraLeft))
            {
                Camera.CamRotationDegrees -= MathHelper.ToRadians(1);
            }
            if (Input.KState.IsKeyDown(Input.RotateCameraRight))
            {
                Camera.CamRotationDegrees += MathHelper.ToRadians(1);
            }

        }
    }
}
