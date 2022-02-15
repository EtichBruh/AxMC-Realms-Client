using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace nekoT
{
    public class SpriteAtlas
    {
        public Texture2D Texture { get; set; }
        public SpriteAtlas parent;
        public Vector2 Direction;
        public Vector2 Position;
        public Vector2 Origin { get; private set; }
        public bool isRemoved = false;
        public float Rotation { get; set; }
        public int CurrentFrame = 0;
        public int Width, Height = 1;
        public SpriteEffects Effect;
        protected Rectangle _destRect;
        protected readonly int _width, _height;
        public SpriteAtlas(Texture2D spritesheet, int rows, int columns, int frame)
            //quick note about rows and columns
            /*Lets imagine small spritesheet 3x3
             * Row | Column | Column
             * Row | Column | Column
             * Row | Column | Column
             */
        {
            _width = spritesheet.Width / columns;
            _height = spritesheet.Height / rows;
            Origin = new(_width * 0.5f, _height * 0.5f);
            Texture = spritesheet;
            _destRect = new(_width * (frame % columns), _height * (frame / columns), _width, _height);
        }
        public virtual void Update(GameTime gameTime, List<SpriteAtlas> spritesToAdd) {
            //(width * (CurrentFrame % columns), height * (CurrentFrame / columns)

        }
        public object Clone()
        {
            return MemberwiseClone();
        }
        /*private Texture2D Slice(Texture2D org, int x, int y) // x is position on sprite sheet, same as y
        {
            Texture2D tex = new(org.GraphicsDevice, width, height);
            var data = new Color[width * height];
            org.GetData(0, new(width * (CurrentFrame % columns), height * (CurrentFrame / columns), width, height), data, 0, data.Length);
            tex.SetData(data);
            return tex;
        }*/ // currently implemented in class
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture, new((int)Position.X, (int)Position.Y, Width, Height), _destRect, Color.White, Rotation, Origin, Effect, 1);
        }
    }
}