using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace nekoT
{
    public abstract class SpriteAtlas : IDisposable
    {
        public Texture2D Texture;
        public SpriteAtlas parent;
        public Vector2 Direction;
        public Vector2 Position;
        public Vector2 Origin { get; private set; }
        public float Rotation;
        public int CurrentFrame, PreviousFrame = 0;
        public int Width, Height = 1;
        public bool isRemoved;
        public SpriteEffects Effect;
        protected Rectangle _srcRect;
        public SpriteAtlas(Texture2D spritesheet, int rows, int columns, int frame)
        //quick note about rows and columns
        /*Lets imagine small spritesheet 3x3
         * Row | Column | Column
         * Row | Column | Column
         * Row | Column | Column
         */
        {
            var _width = spritesheet.Width / columns;
            var _height = spritesheet.Height / rows;
            Texture = AddPadding(spritesheet, _width, _height, columns, rows);

            _width += 2;
            _height += 2;

            Origin = new(_width * 0.5f, _height * 0.5f);
            CurrentFrame = PreviousFrame = frame;
            _srcRect = new(_width * (frame % columns), _height * (frame / columns), _width, _height);
        }
        public virtual void Update(GameTime gameTime, List<SpriteAtlas> spritesToAdd) { }
        public object Clone()
        {
            return MemberwiseClone();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #region Paddings
        public static Texture2D AddPadding(Texture2D tex, int width, int height, int columns, int rows)
        {
            var output = new Texture2D(tex.GraphicsDevice, tex.Width + (2 * columns), tex.Height + (2 * rows));

            Color[][] datas = new Color[columns * rows][];

            for (int x = 0; x < tex.Width; x += width)
                for (int y = 0; y < tex.Height; y += height)
                {
                    var i = (x / width) + (y / height) * columns;
                    datas[i] = new Color[width * height];

                    tex.GetData(0, new Rectangle(x, y, width, height), datas[i], 0, width * height);

                }
            int w = width + 2;
            int h = height + 2;
            for (int i = 0; i < columns * rows; i++)
            {
                output.SetData(0, new(w * (i % columns) + 1, h * (i / columns) + 1, width, height), datas[i], 0, width * height);
            }
            return output;
        }
        public static Texture2D AddPadding(Texture2D tex,ref Rectangle[] Sources)
        {
            int fw = tex.Width;
            int fh = tex.Height;

            for (int i = 0; i < Sources.Length; i++)
            {
                if(Sources[i].X == 0)
                {
                    fh += 2;
                }
                if (Sources[i].Y == 0)
                {
                    fw += 2;
                }
            }

            var output = new Texture2D(tex.GraphicsDevice, fw, fh);


            for(int i =0; i < Sources.Length; i++)
            {
                var data = new Color[Sources[i].Width * Sources[i].Height];

                tex.GetData(0, Sources[i], data, 0, data.Length);

                if (Sources[i].X > 0)
                {
                    if (Sources[i - 1].Right != fw)
                    {
                        Sources[i].X = Sources[i - 1].Right;
                    }
                }
                if (Sources[i].Y > 0)
                {
                    Sources[i].Y+=2;
                }

                Sources[i].X++;
                Sources[i].Y++;

                output.SetData(0, Sources[i], data, 0, data.Length);

                Sources[i].X--;
                Sources[i].Y--;
                Sources[i].Width += 2;
                Sources[i].Height+= 2;
            }

            MemoryStream ms = new();
            output.SaveAsPng(ms, output.Width,output.Height);

            ms.Seek(0, SeekOrigin.Begin);

            System.Drawing.Bitmap.FromStream(ms).Save("TestOutput.png");

            return output;
        }
        #endregion
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture, new((int)Position.X, (int)Position.Y, Width, Height),
                _srcRect, Color.White, Rotation, Origin, Effect, 1);
        }
    }
}