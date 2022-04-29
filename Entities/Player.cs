﻿using AxMC.Camera;
using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Graphics;
using AxMC_Realms_Client.Networking;
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
        public static Bullet _bullet;
        public static Point TiledPos = Point.Zero;
        public static Point xyCount;
        public static int SquareOfSightStartIndex;
        public static ProgressBar HPbar;
        int HP = 1000;
        int[] Stats = { 2000, 1, 20 };

        public Player(Texture2D spriteSheet, Texture2D BulletTexture) :
            base(spriteSheet, 3, 5, 0)
        {
            Input.setKeys();
            TiledPos = (Position / 50).ToPoint();
            GetSquareOfSight();
            _bullet = new(BulletTexture);
            Width = 50;
            Height = 50;
            HPbar = new(spriteSheet.GraphicsDevice);
        }
        public override void Update(GameTime gameTime, List<SpriteAtlas> spritesToAdd)
        {
            PreviousFrame = CurrentFrame;
            for (int i = 0; i < Game1._bullets.Length; i++)
            {
                if (Game1._bullets[i].parent is Enemy && (Game1._bullets[i].Position - Position).Length() < 50)
                {
                    HP--;
                    Game1._bullets.RemoveAt(i);
                    i--;
                }
                if (isRemoved = (HP <= 0)) break;
            }
            if (!isRemoved)
            {
                HPbar.ProgressValue = HP / (Stats[0] / 100);
                Move();
                Enemy.NearestPlayer = Position;
                HPbar.Update((int)Position.X, (int)(Position.Y + Height * 0.5f));
                Shoot(spritesToAdd);
                if (PreviousFrame != CurrentFrame)
                {
                    var columns = (Texture.Width / _width);
                    _srcRect.X = _width * (CurrentFrame % columns);
                    _srcRect.Y = _height * (CurrentFrame / columns);
                }
                RotateZoom();
            }
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
                Position += Direction;
                if (Position.X < 0 || Position.X > Map.Map.MapSize.X * 50) Position.X -= Direction.X;
                if (Position.Y < 0 || Position.Y > Map.Map.MapSize.Y * 50) Position.Y -= Direction.Y;
                TiledPos = (Position / 50).ToPoint();
                GetSquareOfSight();
                /*for (int i = 0; i <xyCount.X; i++)
                {
                    for (int j = 0; j < xyCount.Y; j++)
                    {
                        var index = Player.SquareOfSightStartIndex + i + j * Map.Map.MapSize.X;
                        if (index < 0 || index > Game1.MapTiles.Length) continue;
                        if (Game1.MapTiles[index] is null)
                        {
                            if (Position.X > (Game1.MapBlocks[index].X + 0.5f) * 50 &&
                        Position.X < (Game1.MapBlocks[index].X - 0.5f) * 50) Position.X -= Direction.X;

                            if ( Position.Y > (Game1.MapBlocks[index].Y + 0.5f) * 50 &&
                                Position.Y < (Game1.MapBlocks[index].Y - 0.5f) * 50) Position.Y -= Direction.Y;

                        }
                    }
                }
                for (int i = 0; i < Bag.Bags.Length; i++)
                {
                    if ((Bag.Bags[i].Rect.Location.ToVector2() - Position).Length() > 100) {
                        Bag.Bags[i].isChoosed = false;
                        Bag.NearestBag = -1;
                        
                    }
                    else { Bag.NearestBag = i; break; }
                }*/
                Direction = Vector2.Zero;
                //Connection.SendPosition(Position.ToByte());
            }
        }
        private void Shoot(List<SpriteAtlas> spritesToAdd)
        {
               /* if (Bag.NearestBag != -1 &&Input.MState.LeftButton == ButtonState.Pressed && !Bag.Bags[Bag.NearestBag].isChoosed)
                {
                    Bag.Bags[Bag.NearestBag].isChoosed = true;
            }*/

            else if (Input.MState.LeftButton == ButtonState.Pressed)
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
        /// Calculates the start index for square of sighte
        /// </summary>
        /// <returns>Start index of square of sight</returns>
        private void GetSquareOfSight(int DrawRadius = 9)
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
