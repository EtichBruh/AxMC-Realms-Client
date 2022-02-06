using AxMC.Camera;
using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Entities;
using AxMC_Realms_Client.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using nekoT;
using Nez;
using System;
using System.Collections.Generic;

namespace AxMC_Realms_Client
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private FastList<SpriteAtlas> _sprites;
        private List<SpriteAtlas> _spritesToAdd;
        private Effect _outline;
        private Effect _colorMask;
        public static Tile[] MapTiles;
        public static Matrix[] MapBlocks;
        Matrix _projectionMatrix;
        Matrix _viewMatrix;
        Block3D cube;
        //private string[] WindowTitleAddition = new string[3] { "ZAMN!", "Daaamn what you know about rollin down in the deep?", "13yo kid moment" };
        public Game1()
        {

            _graphics = new GraphicsDeviceManager(this);
            Random rand = new();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            //  Window.Title = Window.Title + ": " + WindowTitleAddition[rand.Next(0, 2)];

        }

        protected override void Initialize()
        {

            // TODO: Add your initialization logic here
            cube = new();
            cube.BlockInit(GraphicsDevice);
            _spritesToAdd = new List<SpriteAtlas>();
            _sprites = new FastList<SpriteAtlas>();
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();
            // _viewMatrix = Matrix.CreateLookAt(_cameraPos, Vector3.Forward, Vector3.Up);
            _viewMatrix = Matrix.CreateLookAt(new(0, 45, 100), Vector3.Zero, Vector3.UnitZ);
            _projectionMatrix = Matrix.CreateOrthographic(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0f, 1000f);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>("cursor"), 16, 16));
            _outline = Content.Load<Effect>("outline");
            _colorMask = Content.Load<Effect>("ColorMask");
            Tile.TileSet = Content.Load<Texture2D>("MCRTile");
            Map.Map.MapLoad(new byte?[,] {
            { 5, 1, 4, 1, 5 },
            { 4, 5, 3, 5, 4 },
            { 2, 0, 5, 0, 2 },
            { 4, 5, 3, 5, 4 },
            { 5, 1, 4, 1, 5 }
            });
            var player = new Player(Content.Load<Texture2D>("CrewMateMASK"), Content.Load<Texture2D>("bullet")) { Position = Vector2.One * 10 };
            _spritesToAdd.Add(player);
            cube.BasiceCubeEff.Texture = Content.Load<Texture2D>("bedrock");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (IsActive)
            {
                Camera.CamView = GraphicsDevice.Viewport;
                Input.KState = Keyboard.GetState();
                Input.MState = Mouse.GetState();
                if (_spritesToAdd.Count != 0) foreach (var item in _spritesToAdd) _sprites.Add(item);
                _spritesToAdd.Clear();
                for (int i = 0; i < _sprites.Length; i++)
                {
                    _sprites[i].Update(gameTime, _spritesToAdd);
                    if (!_sprites[i].isRemoved) continue;
                    _sprites.RemoveAt(i);
                    i--;
                }
                //, ,
                _viewMatrix = Matrix.CreateLookAt(new(0, 45, 100), Vector3.Zero, Vector3.UnitZ);
                //cube.World
                Camera.Follow(_sprites[0].Position);
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(transformMatrix: Camera.CamTransform, samplerState: SamplerState.PointClamp, effect:_colorMask);




            for (int i = 0; i < Player.xyCount.X; i++)
            {
                for (int j = 0; j < Player.xyCount.Y; j++)
                {
                    var index = Player.SquareOfSightStartIndex + i + j * Map.Map.MapWidth;
                    if (index < 0 || index > MapTiles.Length) continue;
                    _spriteBatch.Draw(Map.Tile.TileSet, MapTiles[index].Rect, MapTiles[index].DestRect, Color.White);
                }
            }
            for (int i = 0; i < _sprites.Length; i++)
            {
                _sprites[i].Draw(_spriteBatch);
            }

            _spriteBatch.End();
            cube.BasiceCubeEff.View = _viewMatrix * Matrix.CreateScale(Camera.CamZoom) * Matrix.CreateRotationZ(-Camera.CamRotationDegrees);
            cube.BasiceCubeEff.Projection = _projectionMatrix;
            //cube.World = Matrix.CreateTranslation((-3 * 50 + 25) + _sprites[0].Position.X, 5 * 50 - _sprites[0].Position.Y, 0);
            //cube.BasiceCubeEff.World = cube.World;
            GraphicsDevice.SetVertexBuffer(cube.VertexBuffer);
            GraphicsDevice.Indices = cube.IndexBuffer;

            foreach (EffectPass pass in cube.BasiceCubeEff.CurrentTechnique.Passes)
            {
                foreach (Matrix cubePos in MapBlocks)
                {
                    cube.BasiceCubeEff.World = cubePos * Matrix.CreateTranslation(_sprites[0].Position.X, -_sprites[0].Position.Y, 0);
                    pass.Apply();
                    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12); // la cube drawing
                }
            }
            base.Draw(gameTime);
        }
    }
}
