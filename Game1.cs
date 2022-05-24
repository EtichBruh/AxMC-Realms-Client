using AxMC.Camera;
using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Entities;
using AxMC_Realms_Client.Graphics;
using AxMC_Realms_Client.UI;
using Discord;
using Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using nekoT;
using Nez;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Versioning;

namespace AxMC_Realms_Client
{
    public class Game1 : Game
    {

        //public static FastList<Bullet> _bullets;
        public static Tile[] MapTiles;
        public static Vector2[] NetworkPlayers, MapBlocks;
        public static Matrix _projectionMatrix;
        public static SpriteFont Arial;

        public static FastList<SpriteAtlas> _sprites;
        private List<SpriteAtlas> _spritesToAdd;
        private Effect _outline, _colorMask;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Model model;
        UI.UI _UI;

        Discord.Discord ds = new(975495189948923975, 0); // (ulong)Discord.CreateFlags.Default;
        Activity activity = new()
        {
            State = "Playing in world Ship",
            Details = "nekoT, 99999 Fame ",

            Timestamps =
            {
                Start = DateTimeOffset.Now.ToUnixTimeSeconds(),
            },

            Assets =
            {
                LargeImage = "logo",
                LargeText = "Sussy Game",
                SmallImage = "crewmate",
                SmallText = "Crewmate - LVL 1"
            },

            Party =
            {
                Id = "ae488379-351d-4a4f-ad32-2b9b01c91657",
                Size = {CurrentSize = 1,MaxSize = 2},
            }
        };

        //private string[] WindowTitleAddition = new string[3] { "ZAMN!", "Daaamn what you know about rollin down in the deep?", "13yo kid moment" };
        public Game1()
        {

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            //Window.Title = Window.Title + ": " + WindowTitleAddition[rand.Next(0, 2)];
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 900;
            _graphics.PreferredBackBufferHeight = 696;
            _graphics.ApplyChanges();
            ProgressBar.Init(GraphicsDevice);

            _spritesToAdd = new List<SpriteAtlas>();
            _sprites = new FastList<SpriteAtlas>();
            MapTiles = Array.Empty<Tile>();

            var sWidth = GraphicsDevice.Viewport.Width * .01f; // its 1 / (50 * 2), before it was / 50 / 2
            var sHeight = GraphicsDevice.Viewport.Height * 0.0089445438282648f;// its 1 / (55.9 * 2), before it was / 55.9 / 2
            _projectionMatrix = Matrix.CreateOrthographicOffCenter(-sWidth, sWidth, -sHeight, sHeight, -200f, 5000f);
            Camera.View = GraphicsDevice.Viewport;

            ds.ActivityManagerInstance = ds.GetActivityManager();
            //activity.Timestamps.Start = DateTimeOffset.Now.ToUnixTimeSeconds();

           // activity.Assets.LargeImage = "logo";
            //activity.Assets.LargeText = "Sussy game";
            //activity.Assets.SmallImage = "crewmate";
            //activity.Assets.SmallText = "Crewmate - lvl 1";
            //activity.Party.Id = "ae488379-351d-4a4f-ad32-2b9b01c91657";
           // activity.Party.Size = new PartySize() { CurrentSize = 1 };

            ds.ActivityManagerInstance.UpdateActivity(activity, ActivityCheck);

            var client = new HttpClient();
            client.GetStringAsync("https://api.countapi.xyz/hit/AxMCRealms/visits");
            client.GetStringAsync("https://api.countapi.xyz/update/AxMCRealms/players?amount=1");

            base.Initialize();
        }
        void ActivityCheck(Result r)
        {
            if (r != Result.Ok)
            {
                Console.WriteLine("Result not Ok");
            }
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>("cursor"), 16, 16));


            model = Content.Load<Model>("Wall");
            var basiceff = ((BasicEffect)model.Meshes[0].Effects[0]);
            basiceff.TextureEnabled = true;
            basiceff.Texture = Content.Load<Texture2D>("bedrock");
            basiceff.View = Matrix.CreateLookAt(new Vector3(0, 45, 90), Vector3.Zero, Vector3.UnitZ);

            _outline = Content.Load<Effect>("outline");
            _colorMask = Content.Load<Effect>("ColorMask");
            Arial = Content.Load<SpriteFont>("File");

            Tile.TileSet = Content.Load<Texture2D>("MCRTile");// Order here matters
            Enemy.tempText = Content.Load<Texture2D>("ImpostorMask");// Order here matters
            Map.Map.Load("Ship", _spritesToAdd);// Order here matters
            _sprites.Add(new Player(Content.Load<Texture2D>("CrewMateMASK"), Content.Load<Texture2D>("ElecticBullet")));// Order here matters

            Item.SpriteSheet = Content.Load<Texture2D>("DripJacket");
            BasicEntity.SpriteSheet = new Texture2D[] { Content.Load<Texture2D>("DripSusBag"), Content.Load<Texture2D>("SussyPortals")};

            _UI = new(GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                Content.Load<Texture2D>("slotconcept"),
                Content.Load<Texture2D>("slotequip"),
                Content.Load<Texture2D>("DropBagUI"),
                Content.Load<Texture2D>("EnterButton"),
                Content.Load<Texture2D>("ExpirienceJar"),
                Content.Load<Texture2D>("StatIcons"));
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            ds.RunCallbacks();
            //ds.ActivityManagerInstance.UpdateActivity(activity, ActivityCheck);

            //_colorMask.Parameters["t"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            if (IsActive)
            {
                Input.KState = Keyboard.GetState();
                Input.MState = Mouse.GetState();

                if (_spritesToAdd.Count != 0) foreach (var item in _spritesToAdd) _sprites.Add(item);

                _spritesToAdd.Clear();/*
                for (int i = 0; i < _bullets.Length; i++)
                {
                    _bullets[i].Update(gameTime, _spritesToAdd);
                    if (!_bullets[i].isRemoved) continue;
                    _bullets.RemoveAt(i);
                    i--;
                }*/
                for (int i = 0; i < _sprites.Length; i++)
                {
                    _sprites[i].Update(gameTime, _spritesToAdd);
                    if (!_sprites[i].isRemoved) continue;
                    _sprites.RemoveAt(i);
                    i--;
                }

                _UI.Update(_sprites);
                Camera.Follow(_sprites[0].Position);

                if (OperatingSystem.IsWindows() && Input.KState.IsKeyDown(Keys.NumPad5)) TakeScreenshot(gameTime);
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        protected override void OnExiting(object sender, EventArgs args)
        {
            new HttpClient().GetStringAsync("https://api.countapi.xyz/update/AxMCRealms/players?amount=1");

            base.OnExiting(sender, args);
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
        #endregion //Code taken from stackoverflow
        const float tcfactor = 1f / 2.55f; // tile color factor
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
                    byte col = (byte)(255 - (Math.Abs((pos - _sprites[0].Position).Length()) * tcfactor));
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

            for (int i = 0; i < BasicEntity.InteractEnt.Length; i++)
            {
                BasicEntity.InteractEnt[i].Draw(_spriteBatch);
            }/*
            for (int i = 0; i < _bullets.Length; i++)
            {
                _bullets[i].Draw(_spriteBatch);
            }*/
            for (int i = _sprites.Length - 1; i > -1; i--)
            {
                _sprites[i].Draw(_spriteBatch);
                if (_sprites[i] is Enemy e)
                {
                    e.HPbar.Draw(_spriteBatch);
                }
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
                    Matrix.CreateTranslation(ppos.X - Player.TiledPos.X - i - 0.5f, Player.TiledPos.Y + j + 0.5f - ppos.Y, 0) * rot;
                    model.Meshes[0].Draw();
                }
            }

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.DrawString(Arial, Math.Round(1f / gameTime.ElapsedGameTime.TotalSeconds).ToString(), Vector2.Zero, Color.IndianRed, 0, Vector2.Zero, 0.16f, 0, 0);
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