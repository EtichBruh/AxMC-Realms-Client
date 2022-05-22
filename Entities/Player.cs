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
        public static Bullet _bullet;
        public static Point TiledPos = Point.Zero;
        public static Point xyCount;
        public static int SquareOfSightStartIndex;
        public static ProgressBar HPbar;
        int HP = 1000;
        int[] Stats = { 2000, 10, 20 };
        double AnimTimer = 1;
        Point[] Frames;

        public Player(Texture2D spriteSheet, Texture2D BulletTexture) :
            base(spriteSheet, 3, 5, 0)
        {
            Input.setKeys();
            Position.X = 150;
            Position.Y = 150;
            TiledPos = (Position / 50).ToPoint();
            GetSquareOfSight();
            _bullet = new(BulletTexture);
            Width = 50;
            Height = 50;
            HPbar = new() { Progress = HP };
            HPbar.SetFactor(Stats[0]);
            int columns = 5;
            int rows = 3;
            Frames = new Point[columns * rows];
            for (int i = 0; i < Frames.Length; i++)
            {
                Frames[i].X = _srcRect.Width * (i % columns);
                Frames[i].Y = _srcRect.Height * (i / columns);
            }
        }
        public override void Update(GameTime gameTime, List<SpriteAtlas> spritesToAdd)
        {
            PreviousFrame = CurrentFrame;
            for (int i = 0; i < Game1._bullets.Length; i++)
            {
                if (Game1._bullets[i].parent is Enemy && (Game1._bullets[i].Position - Position).LengthSquared() <= 2500)
                {
                    HPbar.Progress = HP-= Game1._bullets[i].Damage;
                    Game1._bullets.RemoveAt(i);
                    i--;
                }
                if (isRemoved = (HP <= 0)) break;
            }
            if (!isRemoved)
            {
                Move(gameTime);
                Enemy.NearestPlayer = Position;
                HPbar.Update(Position.X, Position.Y + Height * 0.5f);
                if(AnimTimer > 0) AnimTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                if (AnimTimer < 0.8 && CurrentFrame == 4 || CurrentFrame == 9 || CurrentFrame == 14)
                {
                    CurrentFrame--;
                }
                if (AnimTimer <= 0)
                {
                    Shoot(spritesToAdd);
                }
                if (PreviousFrame != CurrentFrame)
                {
                    _srcRect.Location = Frames[CurrentFrame];
                }
                RotateZoom();
            }
        }
        private void Move(GameTime gt)
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
                if (Rotation != 0)
                {
                    var rot = MathF.Atan2(Direction.Y, Direction.X) + Rotation;
                    Direction.X = MathF.Cos(rot);
                    Direction.Y = MathF.Sin(rot);
                }
                Position += Direction;
                if (Position.X < 0 || Position.X > Map.Map.Size.X * 50) Position.X -= Direction.X;
                if (Position.Y < 0 || Position.Y > Map.Map.Size.Y * 50) Position.Y -= Direction.Y;
                TiledPos = (Position * .02f).ToPoint();
                GetSquareOfSight();
                /*for (int i = 0; i <xyCount.X; i++) // this is collision code that doesnt work
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
                }*/
                if (BasicEntity.InteractEnt.Length > 0)
                {
                    for (int i = 0; i < BasicEntity.InteractEnt.Length; i++)
                    {
                        if ((BasicEntity.InteractEnt[i].Rect.Location.ToVector2() - Position).LengthSquared() < 900) // 30is the hitbox size
                        {
                            BasicEntity.NInteract = i; break;
                        }
                        else BasicEntity.NInteract = -1;
                    }
                }
                Direction = Vector2.Zero;
                //Connection.SendPosition(Position.ToByte());
            }
        }
        /*if (Bag.Bags.Length > 0)
{
    if (Bag.NearestBag != -1 && Input.MState.LeftButton == ButtonState.Pressed && !Bag.Bags[Bag.NearestBag].isChoosed)
    {
        Bag.Bags[Bag.NearestBag].isChoosed = true;
    }
}*/
        private void Shoot(List<SpriteAtlas> spritesToAdd)
        {
            if (Input.MState.LeftButton == ButtonState.Pressed)
            {
                Bullet b = _bullet.Clone() as Bullet;
                b.Position = Position;
                b.Direction = Vector2.Normalize(Vector2.Transform(Input.MState.Position.ToVector2(), Matrix.Invert(Camera.Transform)) - Position);
                b.Rotation = MathF.Atan2(b.Direction.Y, b.Direction.X) + Bullet.TexOffset;
                if (b.Rotation > 0 && b.Rotation < Bullet.TexOffset * 2)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 3;
                }
                else if (b.Rotation > Bullet.TexOffset * 2 && b.Rotation < Bullet.TexOffset * 4)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 8;
                }
                else if (b.Rotation < 0 && b.Rotation > -Bullet.TexOffset * 2)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 13;
                }
                else
                {
                    Effect = SpriteEffects.FlipHorizontally;
                    CurrentFrame = 3;
                }
                if (PreviousFrame == CurrentFrame)
                {
                    CurrentFrame++;
                }
                b.Speed = 5;
                b.LifeSpan = 4;
                b.Damage = Stats[1];
                b.parent = this;
                spritesToAdd.Add(b);
                AnimTimer = 1;
            }
        }
        /// <summary>
        /// Calculates the start index for square of sighte
        /// </summary>
        /// <returns>Start index of square of sight</returns>
        private void GetSquareOfSight(int DrawRadius = 9)
        {
            xyCount.X = Math.Min(DrawRadius, TiledPos.X) + Math.Min(DrawRadius + 1, Map.Map.Size.X - TiledPos.X);
            xyCount.Y = Math.Min(DrawRadius, TiledPos.Y) + Math.Min(DrawRadius + 1, Map.Map.Size.Y - TiledPos.Y);

            TiledPos.X = Math.Max(0, TiledPos.X - DrawRadius);
            TiledPos.Y = Math.Max(0, TiledPos.Y - DrawRadius);


            SquareOfSightStartIndex = TiledPos.X + TiledPos.Y * Map.Map.Size.X;
        }
        private void RotateZoom()
        {
            if (Input.KState.IsKeyDown(Input.ZoomIn))
            {
                Camera.CamZoom += 0.2f;
            }
            if (Input.KState.IsKeyDown(Input.ZoomOut))
            {
                Camera.CamZoom -= 0.2f;
                Camera.CamZoom = Camera.CamZoom <= 0.2f ? Camera.CamZoom = 0.2f : Camera.CamZoom;
            }
            if (Input.KState.IsKeyDown(Input.RotateCameraLeft))
            {
                Camera.RotDegr += 0.017453292f;
                Camera.RotDegr = Camera.RotDegr >= 360 ? 0 : Camera.RotDegr;
                Rotation = -Camera.RotDegr;
            }
            if (Input.KState.IsKeyDown(Input.RotateCameraRight))
            {
                Camera.RotDegr -= 0.017453292f;
                Rotation = -Camera.RotDegr;
            }
            if (Input.KState.IsKeyDown(Input.ResetRotation))
            {
                Camera.RotDegr = 0;
                Rotation = 0;
                Camera.CamZoom = 1;
            }
        }
    }
}
