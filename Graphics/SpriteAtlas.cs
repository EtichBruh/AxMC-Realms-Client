using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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
        #region unused
        static Texture2D AddPadding(Texture2D tex, int width, int height, int columns, int rows)
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
        #endregion
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture, new((int)Position.X, (int)Position.Y, Width, Height),
                _srcRect, Color.White, Rotation, Origin, Effect, 1);
        }
    }
}