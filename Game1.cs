using AxMC.Camera;
using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Entities;
using AxMC_Realms_Client.Inventory;
using AxMC_Realms_Client.Map;
using AxMC_Realms_Client.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using nekoT;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

namespace AxMC_Realms_Client
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private FastList<SpriteAtlas> _sprites;
        public static FastList<Bullet> _bullets;
        private List<SpriteAtlas> _spritesToAdd;
        private Effect _outline;
        private Effect _colorMask;
        public static SpriteFont Arial;
        public static Tile[] MapTiles;
        public static Vector2[] NetworkPlayers;
        public static Vector2[] MapBlocks;
        public static Matrix _projectionMatrix;
        public static Matrix _viewMatrix;
        //Block3D cube;
        Model model;
        UI _ui;
        //private string[] WindowTitleAddition = new string[3] { "ZAMN!", "Daaamn what you know about rollin down in the deep?", "13yo kid moment" };
        public Game1()
        {

            _graphics = new GraphicsDeviceManager(this);
            //Random rand = new();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            //  Window.Title = Window.Title + ": " + WindowTitleAddition[rand.Next(0, 2)];

        }

        protected override void Initialize()
        {

            // TODO: Add your initialization logic here
            //cube = new(GraphicsDevice);

            _spritesToAdd = new List<SpriteAtlas>();
            _sprites = new FastList<SpriteAtlas>();
            _bullets = new FastList<Bullet>();
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();
            // _viewMatrix = Matrix.CreateLookAt(_cameraPos, Vector3.Forward, Vector3.Up);

            //cube.BasiceCubeEff.Projection = _projectionMatrix;//* Matrix.CreateRotationX(-MathHelper.ToRadians(45));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>("cursor"), 16, 16));
            model = Content.Load<Model>("Wall");
            ((BasicEffect)model.Meshes[0].Effects[0]).TextureEnabled = true;
            ((BasicEffect)model.Meshes[0].Effects[0]).Texture = Content.Load<Texture2D>("bedrock");
            _viewMatrix = Matrix.CreateLookAt(new Vector3(0, 45, 90), Vector3.Zero, Vector3.UnitZ);
            ((BasicEffect)model.Meshes[0].Effects[0]).View = _viewMatrix;
            var sWidth = model.Meshes[0].Effects[0].GraphicsDevice.Viewport.Width / 50f / 2f;
            var sHeight = model.Meshes[0].Effects[0].GraphicsDevice.Viewport.Height / 55.9f / 2f;
            _projectionMatrix = Matrix.CreateOrthographicOffCenter(-sWidth, sWidth, -sHeight, sHeight, -200f, 5000f);
            _outline = Content.Load<Effect>("outline");
            _colorMask = Content.Load<Effect>("ColorMask");
            Tile.TileSet = Content.Load<Texture2D>("MCRTile");
            Map.Map.MapLoad(new byte[] {
             2, 1, 4, 1, 0, 1, 3 ,
             2, 1, 4, 1, 0, 1, 3 ,
             2, 1, 4, 5, 3, 5, 3 ,
             2, 1, 4, 5, 3, 5, 3 ,
             2, 1, 4, 5, 5, 5, 3 ,
             2, 1, 4, 1, 0, 1, 3 ,
             2, 1, 4, 1, 0, 1, 3 ,
            }, 7);
            var player = new Player(Content.Load<Texture2D>("CrewMateMASK"), Content.Load<Texture2D>("bullet")) { Position = new(150,0) };
            var enemy = new Enemy(Content.Load<Texture2D>("ImpostorMask")) { Position = new(200,0) };
            Bag bag = new(Content.Load<Texture2D>("DripSusBag"));
            _spritesToAdd.Add(player);
            _spritesToAdd.Add(enemy);
            //cube.BasiceCubeEff.Texture = Content.Load<Texture2D>("cubetexture");
            Arial = Content.Load<SpriteFont>("File");
            Item.SpriteSheet = Content.Load<Texture2D>("DripJacket");
            _ui = new(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Content.Load<Texture2D>("slotconcept"), Content.Load<Texture2D>("slotequip"));
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            _colorMask.Parameters["t"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            if (IsActive)
            {
                Camera.CamView = GraphicsDevice.Viewport;
                Input.KState = Keyboard.GetState();
                Input.MState = Mouse.GetState();
                if (_spritesToAdd.Count != 0) foreach (var item in _spritesToAdd)
                    {
                        if (item is not Bullet) _sprites.Add(item);
                        else {
                            _bullets.Add((Bullet)item);
                        }
                    }
                _spritesToAdd.Clear();
                for (int i = 0; i < _bullets.Length; i++)
                {
                    _bullets[i].Update(gameTime, _spritesToAdd);
                    if (!_bullets[i].isRemoved) continue;
                    _bullets.RemoveAt(i);
                    i--;
                }
                for (int i = 0; i < _sprites.Length; i++)
                {
                    _sprites[i].Update(gameTime, _spritesToAdd);
                    if (!_sprites[i].isRemoved) continue;
                    _sprites.RemoveAt(i);
                    i--;
                }
                _ui.Update();
                Camera.Follow(_sprites[0].Position);
                if (OperatingSystem.IsWindows())
                {
                    if (Input.KState.IsKeyDown(Keys.NumPad5))
                    {
                        TakeScreenshot(gameTime);
                    }
                }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            var sWidth = model.Meshes[0].Effects[0].GraphicsDevice.Viewport.Width / 50f / 2f;
            var sHeight = model.Meshes[0].Effects[0].GraphicsDevice.Viewport.Height / 55.9f / 2f;
            _projectionMatrix = Matrix.CreateOrthographicOffCenter(-sWidth, sWidth, -sHeight, sHeight, -200f, 5000f);
            _ui.Resize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //((BasicEffect)model.Meshes[0].Effects[0]).Projection = _projectionMatrix;
            //cube.BasiceCubeEff.Projection = _projectionMatrix;
        }
        #region screenshot
        [SupportedOSPlatform("windows")]
        public void TakeScreenshot(GameTime gameTime)
        {
            RenderTarget2D screenshot;
            screenshot = new RenderTarget2D(GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Bgr565, DepthFormat.None);
            GraphicsDevice.SetRenderTarget(screenshot);
            Draw(gameTime);
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Present();
            byte[] imageData = new byte[2 * screenshot.Width * screenshot.Height];
            screenshot.GetData<byte>(imageData);

            System.Drawing.Bitmap bitmap = new(screenshot.Width, screenshot.Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
            System.Drawing.Imaging.BitmapData bmData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, screenshot.Width, screenshot.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr pnative = bmData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(imageData, 0, pnative, 2 * screenshot.Width * screenshot.Height);
            bitmap.UnlockBits(bmData);
            bitmap.Save("Screenshot.png", System.Drawing.Imaging.ImageFormat.Png);
        }
        #endregion
        bool collide = false;
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
                    if (MapTiles[index] is null) continue;
                    Tile.SharedPos.X = Player.TiledPos.X + i;
                    Tile.SharedPos.Y = Player.TiledPos.Y + j;
                    _spriteBatch.Draw(Tile.TileSet, Tile.SharedPos * 50, MapTiles[index].SrcRect, Color.White, 0, Vector2.Zero, scale: 3.125f, 0, 0);
                }
            }
            if (NetworkPlayers is not null)
            {
                for (int i = 0; i < NetworkPlayers.Length; i++)
                {
                    _spriteBatch.Draw(_sprites[0].Texture, NetworkPlayers[i], new(0, 0, 9, 9), Color.White);
                }
            }
            for (int i = 0; i < _sprites.Length; i++)
            {
                _sprites[i].Draw(_spriteBatch);
                if(_sprites[i] is Enemy e)
                {
                    e.HPbar.Draw(_spriteBatch);
                }
            }
            for (int i = 0; i < _bullets.Length; i++)
            {
                _bullets[i].Draw(_spriteBatch);
            }
            Player.HPbar.Draw(_spriteBatch);
            _spriteBatch.DrawString(Arial, collide.ToString(), Vector2.Zero, Color.Black);
            _spriteBatch.DrawString(Arial, _sprites[0].Position.ToString(), Vector2.Zero, Color.Black, -Camera.CamRotationDegrees, Arial.MeasureString(_sprites[0].Position.ToString()), 1 / Camera.CamZoom, SpriteEffects.None, 0);
            _spriteBatch.End();// actually 256x256 is pretty big im not sure it will load it

            for (int i = 0; i < Player.xyCount.X; i++)
            {
                for (int j = 0; j < Player.xyCount.Y; j++)
                {
                    var index = Player.SquareOfSightStartIndex + i + j * Map.Map.MapSize.X;
                    if (index < 0 || index > MapTiles.Length) continue;
                    if (MapTiles[index] is not null) continue;
                    Tile.SharedPos.X = Player.TiledPos.X + i + 0.5f;
                    Tile.SharedPos.Y = Player.TiledPos.Y + j + 0.5f;
                    ((BasicEffect)model.Meshes[0].Effects[0]).World =
                    Matrix.CreateTranslation(-Tile.SharedPos.X + (_sprites[0].Position.X / 50f), Tile.SharedPos.Y - (_sprites[0].Position.Y / 50f), 0)
                    * Matrix.CreateRotationZ(-Camera.CamRotationDegrees);
                    ((BasicEffect)model.Meshes[0].Effects[0]).Projection = _projectionMatrix * Matrix.CreateScale(Camera.CamZoom);
                    model.Meshes[0].Draw();
                }
            }
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _ui.Draw(_spriteBatch);

            _spriteBatch.End();
            /*cube.BasiceCubeEff.View = _viewMatrix * Matrix.CreateTranslation(-_sprites[0].Position.X, _sprites[0].Position.Y, 0) * Matrix.CreateScale(Camera.CamZoom);


foreach (EffectPass pass in cube.BasiceCubeEff.CurrentTechnique.Passes)
{
    foreach (Matrix cubePos in MapBlocks)
    {
        cube.BasiceCubeEff.World = cubePos;
        pass.Apply();
        GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Block3D.SmthIndicesDevidedByThree); // la cube drawing
    }
}*/
            base.Draw(gameTime);
        }
    }
}