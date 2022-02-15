using AxMC.Camera;
using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Entities;
using AxMC_Realms_Client.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using nekoT;
using Nez;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;

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
        private SpriteFont Arial;
        public static Tile[] MapTiles;
        public static Matrix[] MapBlocks;
        Matrix _projectionMatrix;
        Matrix _viewMatrix;
        Block3D cube;
        //private string[] WindowTitleAddition = new string[3] { "ZAMN!", "Daaamn what you know about rollin down in the deep?", "13yo kid moment" };
        public Game1()
        {

            _graphics = new GraphicsDeviceManager(this);
            //Random rand = new();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            //  Window.Title = Window.Title + ": " + WindowTitleAddition[rand.Next(0, 2)];

        }

        protected override void Initialize()
        {

            // TODO: Add your initialization logic here
            cube = new(GraphicsDevice);
            _spritesToAdd = new List<SpriteAtlas>();
            _sprites = new FastList<SpriteAtlas>();
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();
            // _viewMatrix = Matrix.CreateLookAt(_cameraPos, Vector3.Forward, Vector3.Up);
            _viewMatrix = Matrix.CreateLookAt(new(0, 50, -50f), Vector3.Zero, Vector3.UnitZ);
            var sWidth = GraphicsDevice.Viewport.Width * 0.5f;
            var sHeight = GraphicsDevice.Viewport.Height * 0.5f;
            _projectionMatrix = Matrix.CreateOrthographicOffCenter(-sWidth, sWidth, -sHeight, sHeight, 0f, 4000f);
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
            { 5, 1, 4, 1, 1 },
            { 4, 1, 5, 1, 4 },
            { 2, 0, 0, 0, 5 },
            { 4, 1, 3, 0, 4 },
            { 1, 1, 5, 1, 0 },
            { 1, 1, 4, 0, 1 },
            { 5, 1, 4, 1, 1 },
            { 1, 1, 4, 1, 1 },
            { 1, 2, 5, 1, 5 },
            { 1, 2, 5, 1, 5 },
            { 1, 2, 5, 1, 5 },
            { 1, 2, 5, 1, 5 },
            { 1, 2, 5, 1, 5 },
            { 1, 2, 5, 1, 5 },
            { 1, 2, 5, 1, 5 },
            { 1, 2, 5, 1, 0},
            { 1, 2, 0, 1, 0 },
            { 1, 2, 0, 1, 0 },
            { 1, 2, 0, 1, 5 },
            { 5, 2, 0, 1, 0 },
            { 1, 2, 0, 1, 0 },
            { 1, 2, 5, 1, 0 }
            });
            var player = new Player(Content.Load<Texture2D>("ImpostorMask"), Content.Load<Texture2D>("bullet")) { Position = Vector2.One * 10 };
            _spritesToAdd.Add(player);
            cube.BasiceCubeEff.Texture = Content.Load<Texture2D>("cubetexture");
            Arial = Content.Load<SpriteFont>("File");

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
                // _viewMatrix = Matrix.CreateLookAt(new(0, 45, -100), Vector3.Forward, Vector3.Up);
                //cube.World
                Camera.Follow(_sprites[0].Position);
                Player.HPbar.Update((int)_sprites[0].Position.X, (int)(_sprites[0].Position.Y + _sprites[0].Height * 0.5f));

                if (Input.KState.IsKeyDown(Keys.NumPad5))
                {
                    if (OperatingSystem.IsWindows())
                    {
                        TakeScreenshot(gameTime);
                    }
                }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        [SupportedOSPlatform("windows")]
        public void TakeScreenshot(GameTime gameTime)
        {
            RenderTarget2D screenshot;
            screenshot = new RenderTarget2D(GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Bgr565, DepthFormat.None);
            GraphicsDevice.SetRenderTarget(screenshot);
            Draw(gameTime);
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Present();
            byte[] imageData = new byte[2* screenshot.Width * screenshot.Height];
            screenshot.GetData<byte>(imageData);

            System.Drawing.Bitmap bitmap = new(screenshot.Width, screenshot.Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
            System.Drawing.Imaging.BitmapData bmData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, screenshot.Width, screenshot.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr pnative = bmData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(imageData, 0, pnative, 2 * screenshot.Width * screenshot.Height);
            bitmap.UnlockBits(bmData);
            bitmap.Save("Screenshot.png", System.Drawing.Imaging.ImageFormat.Png);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(transformMatrix: Camera.CamTransform, samplerState: SamplerState.PointClamp);

            for (int i = 0; i < Player.xyCount.X; i++)
            {
                for (int j = 0; j < Player.xyCount.Y; j++)
                {
                    var index = Player.SquareOfSightStartIndex + i + j * Map.Map.MapSize.X;
                    if (index < 0 || index > MapTiles.Length) continue;
                    _spriteBatch.Draw(Map.Tile.TileSet, MapTiles[index].Rect, MapTiles[index].DestRect, Color.White);
                }
            }
            for (int i = 0; i < _sprites.Length; i++)
            {
                _sprites[i].Draw(_spriteBatch);
            }
            Player.HPbar.Draw(_spriteBatch); //new(_sprites[0].Position.X - Arial.MeasureString(_sprites[0].Position.ToString()).X*0.5f, _sprites[0].Position.Y - _sprites[0].Height *0.5f)
            _spriteBatch.DrawString(Arial, _sprites[0].Position.ToString(), Vector2.Zero, Color.Black, -Camera.CamRotationDegrees, Arial.MeasureString(_sprites[0].Position.ToString()), 1 / Camera.CamZoom, SpriteEffects.None, 0);
            _spriteBatch.End();
            cube.BasiceCubeEff.View = _viewMatrix * Matrix.CreateTranslation(-_sprites[0].Position.X, _sprites[0].Position.Y, 0) * Matrix.CreateScale(Camera.CamZoom);//* Matrix.CreateScale(1/ MathF.Cos(MathF.Atan(45 / 100))) * 
            cube.BasiceCubeEff.Projection = _projectionMatrix;//* Matrix.CreateRotationX(-MathHelper.ToRadians(45));
            GraphicsDevice.SetVertexBuffer(cube.VertexBuffer);
            GraphicsDevice.Indices = cube.IndexBuffer;

            foreach (EffectPass pass in cube.BasiceCubeEff.CurrentTechnique.Passes)
            {
                foreach (Matrix cubePos in MapBlocks)
                {
                    cube.BasiceCubeEff.World = cubePos; //* Matrix.CreateTranslation(_sprites[0].Position.X, _sprites[0].Position.Y, 0);
                    pass.Apply();
                    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Block3D.SmthIndicesDevidedByThree); // la cube drawing
                }
            }
            base.Draw(gameTime);
        }
    }
}