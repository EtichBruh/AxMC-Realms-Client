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
        public int CurrentFrame,PreviousFrame = 0;
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
            Origin = new(_width * 0.5f, _height * 0.5f);
            Texture = spritesheet;
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
        /*private Texture2D Slice(Texture2D org, int x, int y) // x is position on sprite sheet, same as y
        {
            Texture2D tex = new(org.GraphicsDevice, width, height);
            var data = new Color[width * height];
            org.GetData(0, new(width * (CurrentFrame % columns), height * (CurrentFrame / columns), width, height), data, 0, data.Length);
            tex.SetData(data);
            return tex;
        }*/ // currently implemented in class UPD: not being used atall
        #endregion
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture, new((int)Position.X, (int)Position.Y, Width, Height),
                _srcRect, Color.White, Rotation, Origin, Effect, 1);
        }
    }
}