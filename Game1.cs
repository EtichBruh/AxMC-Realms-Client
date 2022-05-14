using AxMC.Camera;
using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Entities;
using AxMC_Realms_Client.Graphics;
using AxMC_Realms_Client.UI;
using Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using nekoT;
using Nez;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;


namespace AxMC_Realms_Client
{
    public class Game1 : Game
    {
        
        public static FastList<Bullet> _bullets;
        public static Tile[] MapTiles;
        public static Vector2[] NetworkPlayers, MapBlocks;
        public static Matrix _projectionMatrix;
        public static SpriteFont Arial;

        private FastList<SpriteAtlas> _sprites;
        private List<SpriteAtlas> _spritesToAdd;
        private Effect _outline, _colorMask;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Model model;
        UI.UI _UI;
        //private string[] WindowTitleAddition = new string[3] { "ZAMN!", "Daaamn what you know about rollin down in the deep?", "13yo kid moment" };
        public Game1()
        {

            _graphics = new GraphicsDeviceManager(this);
            Random rand = new();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            //Window.Title = Window.Title + ": " + WindowTitleAddition[rand.Next(0, 2)];
        }

        protected override void Initialize()
        {

            // TODO: Add your initialization logic here
            //cube = new(GraphicsDevice);
            ProgressBar.Init(GraphicsDevice);

            _spritesToAdd = new List<SpriteAtlas>();
            _sprites = new FastList<SpriteAtlas>();
            _bullets = new FastList<Bullet>();
            _graphics.PreferredBackBufferWidth = 900;
            _graphics.PreferredBackBufferHeight = 696;
            _graphics.ApplyChanges();

            var sWidth = GraphicsDevice.Viewport.Width * .01f; // its 1 / (50 * 2), before it was / 50 / 2
            var sHeight = GraphicsDevice.Viewport.Height * 0.0089445438282648f;// its 1 / (55.9 * 2), before it was / 55.9 / 2
            _projectionMatrix = Matrix.CreateOrthographicOffCenter(-sWidth, sWidth, -sHeight, sHeight, -200f, 5000f);
            Camera.View = GraphicsDevice.Viewport;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>("cursor"), 16, 16));

            model = Content.Load<Model>("Wall");
            ((BasicEffect)model.Meshes[0].Effects[0]).TextureEnabled = true;
            ((BasicEffect)model.Meshes[0].Effects[0]).Texture = Content.Load<Texture2D>("bedrock");
            ((BasicEffect)model.Meshes[0].Effects[0]).View = Matrix.CreateLookAt(new Vector3(0, 45, 90), Vector3.Zero, Vector3.UnitZ);

            _outline = Content.Load<Effect>("outline");
            _colorMask = Content.Load<Effect>("ColorMask");
            Arial = Content.Load<SpriteFont>("File");

            Tile.TileSet = Content.Load<Texture2D>("MCRTile");
            Map.Map.Load("map.json");

            Item.SpriteSheet = Content.Load<Texture2D>("DripJacket");
            Bag.SpriteSheet = Content.Load<Texture2D>("DripSusBag");
            Portal.SpriteSheet = Content.Load<Texture2D>("SussyPortals");

            _spritesToAdd.Add(new Player(Content.Load<Texture2D>("CrewMateMASK"), Content.Load<Texture2D>("ElecticBullet")) { Position = new(150, 0) });
            _spritesToAdd.Add(new Enemy(Content.Load<Texture2D>("ImpostorMask")) { Position = new(200, 0) });

            _UI = new(GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                Content.Load<Texture2D>("slotconcept"),
                Content.Load<Texture2D>("slotequip"),
                Content.Load<Texture2D>("DropBagUI"),
                Content.Load<Texture2D>("EnterButton"));
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            //_colorMask.Parameters["t"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            if (IsActive)
            {
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
                _UI.Update();
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
            Camera.View = GraphicsDevice.Viewport;

            var sWidth = GraphicsDevice.Viewport.Width * .01f; // its 1 / (50 * 2), before it was / 50 / 2
            var sHeight = GraphicsDevice.Viewport.Height * 0.0089445438282648f;// its 1 / (55.9 * 2), before it was / 55.9 / 2
            _projectionMatrix = Matrix.CreateOrthographicOffCenter(-sWidth, sWidth, -sHeight, sHeight, -200f, 5000f);

            _UI.Resize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

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
        const float factor = 1f / 2.55f;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(transformMatrix: Camera.Transform, samplerState: SamplerState.PointClamp);
            for (int i = 0; i < Player.xyCount.X; i++)
            {
                for (int j = 0; j < Player.xyCount.Y; j++)
                {
                    var index = Player.SquareOfSightStartIndex + i + j * Map.Map.Size.X;
                    if (index > MapTiles.Length) continue;
                    if (MapTiles[index] is null) continue;
                    Vector2 pos = new Vector2(Player.TiledPos.X + i, Player.TiledPos.Y + j) * 50;
            byte col = (byte)(255 - (Math.Abs((pos - _sprites[0].Position).Length()) * factor)) ;
                    _spriteBatch.Draw(Tile.TileSet, pos, MapTiles[index].SrcRect, new Color(col, col, col, byte.MaxValue), 0, Vector2.Zero, scale: 3.125f, 0, 0);
                }
            }
            if (NetworkPlayers != null)
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
            for(int i = 0; i < BasicEntity.InteractEnt.Length; i++)
            {
                if(BasicEntity.InteractEnt[i] is Bag)
                {
                    BasicEntity.InteractEnt[i].Draw(_spriteBatch, Bag.SpriteSheet);
                }else if(BasicEntity.InteractEnt[i] is Portal)
                {
                    BasicEntity.InteractEnt[i].Draw(_spriteBatch, Portal.SpriteSheet);
                }
                    
            }
            for (int i = 0; i < _bullets.Length; i++)
            {
                _bullets[i].Draw(_spriteBatch);
            }
            Player.HPbar.Draw(_spriteBatch);
            _spriteBatch.End();

            var rot = Matrix.CreateRotationZ(-Camera.RotDegr);
            var ppos = _sprites[0].Position * 0.02f;
            var mesh = (BasicEffect)model.Meshes[0].Effects[0];
            mesh.Projection = _projectionMatrix * Matrix.CreateScale(Camera.CamZoom);
            for (int i = 0; i < Player.xyCount.X; i++)
            {
                for (int j = 0; j < Player.xyCount.Y; j++)
                {
                    var index = Player.SquareOfSightStartIndex + i + j * Map.Map.Size.X;
                    if (index > MapTiles.Length) continue;
                    if (MapTiles[index] is not null) continue;

                    mesh.World =
                    Matrix.CreateTranslation( ppos.X- Player.TiledPos.X - i - 0.5f, Player.TiledPos.Y + j + 0.5f - ppos.Y, 0) * rot;
                    model.Meshes[0].Draw();
                }
            }
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.DrawString(Arial, (1 / gameTime.ElapsedGameTime.TotalSeconds).ToString()[..2], Vector2.Zero, Color.IndianRed);

            _UI.Draw(_spriteBatch);

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