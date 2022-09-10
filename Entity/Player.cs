using AxMC.Camera;
using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using nekoT;
using System;
using System.Collections.Generic;

namespace AxMC_Realms_Client.Entity
{
    public class Player : SpriteAtlas
    {
        public static Bullet _bullet;
        public static Point TiledPos = Point.Zero;
        public static Point xyCount;
        public static int SquareOfSightStartIndex;
        public static ProgressBar HPbar;
        public static int XP = 0, Level = 0;
        public static int[] Stats = { 2000, 200, 100, 20, 0, 1 }; // HP, Mana, Damage, agility, armor, bullets
        public static int Mana = 200;
        double AnimTimer = 1;
        double Shootcd = 1;
        Point[] Frames;

        public Player(Texture2D spriteSheet, Texture2D BulletTexture) :
            base(spriteSheet, 6, 5, 0)
        {
            Input.setKeys();
            Position.X = 150;
            Position.Y = 150;
            TiledPos = (Position / 50).ToPoint();
            GetSquareOfSight();
            _bullet = new(BulletTexture);
            Width = 50;
            Height = 50;
            HPbar = new(Color.LimeGreen, true, true, 2000, 8);
            HPbar.SetFactor(Stats[0], 30);
            int columns = 5;
            int rows = 3;
            Frames = new Point[columns * rows];
            for (int i = 0; i < Frames.Length; i++)
            {
                Frames[i].X = _srcRect.Width * (i % columns);
                Frames[i].Y = _srcRect.Height * (i / columns);
            }
        }
        const float agilityfactor = 1f / 20f;
        public override void Update(GameTime gameTime, List<SpriteAtlas> spritesToAdd)
        {
            for (int i = 0; i < Game1._EnemyBullets.Length; i++)
            {
                Bullet b = Game1._EnemyBullets[i];
                if ((b.Position - Position).LengthSquared() <= 2500)
                {
                    isRemoved = ((HPbar.Progress -= b.Damage) <= 0);
                    UI.UI.HPBar.Progress = HPbar.Progress;
                    b.isRemoved = true;
                }
                if (isRemoved) { break; }
            }
            if (!isRemoved)
            {
                PreviousFrame = CurrentFrame;
                Move(gameTime);
                Enemy.NearestPlayer = Position;
                HPbar.Update(Position.X, Position.Y);
                if (AnimTimer > 0) AnimTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (Shootcd > 0) Shootcd -= gameTime.ElapsedGameTime.TotalSeconds * (Stats[3] * agilityfactor);

                if (AnimTimer < 0.8 && CurrentFrame == 4 || CurrentFrame == 9 || CurrentFrame == 14)
                {
                    CurrentFrame--;
                }
                if (Shootcd <= 0)
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
                    Direction.X = MathF.Cos(rot);// agility / 20
                    Direction.Y = MathF.Sin(rot);
                }
                Direction *= Stats[3] * 0.05f;
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
                //Connection.SendPosition(Position.ToByte());
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
                var Dir = Vector2.Normalize(Vector2.Transform(Input.MState.Position.ToVector2(), Matrix.Invert(Camera.Transform)) - b.Position);
                float r = MathF.Atan2(Dir.Y, Dir.X) + Bullet.TexOffset;
                if (r > 0 && r < Bullet.TexOffset * 2)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 3;
                }
                else if (r > Bullet.TexOffset * 2 && r < Bullet.TexOffset * 4)
                {
                    Effect = SpriteEffects.None;
                    CurrentFrame = 8;
                }
                else if (r < 0 && r > -Bullet.TexOffset * 2)
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
                b.LifeSpan = 2;
                b.Damage = Stats[1];
                Shootcd = 1;
                float someoffset = -1.5f + Stats[5] * 0.5f;
                for (int i = 0; i < Stats[5]; i++)
                {
                    Bullet bb = b.Clone() as Bullet;
                    bb.Direction.X = MathF.Cos(r - Bullet.TexOffset * (i - someoffset));
                    bb.Direction.Y = MathF.Sin(r - Bullet.TexOffset * (i - someoffset));
                    bb.Rotation = MathF.Atan2(bb.Direction.Y, bb.Direction.X) + Bullet.TexOffset;
                    Game1._PlayerBullets.Add(bb);
                }
            }
        }
        /// <summary>
        /// Calculates the start index for square of sighte
        /// </summary>
        /// <returns>Start index of square of sight</returns>
        private void GetSquareOfSight()
        {
            int DrawRadius = 20;
            float ratio = (float)Camera.View.Width / Camera.View.Height;

            xyCount.X = Math.Min(DrawRadius, TiledPos.X) + Math.Min(DrawRadius + 1, Map.Map.Size.X - TiledPos.X);
            xyCount.X = (int)(xyCount.X * ratio);

            xyCount.Y = Math.Min(DrawRadius, TiledPos.Y) + Math.Min(DrawRadius + 1, Map.Map.Size.Y - TiledPos.Y);
            xyCount.Y = (int)(xyCount.Y * ratio);


            TiledPos.X = Math.Max(0, TiledPos.X - (int)(DrawRadius * ratio));
            TiledPos.Y = Math.Max(0, TiledPos.Y - DrawRadius);

            SquareOfSightStartIndex = TiledPos.X + TiledPos.Y * Map.Map.Size.X;
        }
        private void RotateZoom()
        {
            if (Input.KState.IsKeyDown(Input.ZoomIn))
            {
                Camera.CamZoom += 0.4f;
            }
            if (Input.KState.IsKeyDown(Input.ZoomOut))
            {
                Camera.CamZoom -= 0.2f;
                Camera.CamZoom = Camera.CamZoom <= 0.6f ? Camera.CamZoom = 0.6f : Camera.CamZoom;
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
