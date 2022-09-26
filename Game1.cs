using AxMC.Camera;
using AxMC_Realms_Client.Classes;
using AxMC_Realms_Client.Entity;
using AxMC_Realms_Client.Graphics;
using AxMC_Realms_Client.UI;
using Discord;
using Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using nekoT;
using Nez;
using Penumbra;
using System;
using System.Collections.Generic;
using System.IO;

namespace AxMC_Realms_Client
{
    public class Game1 : Game
    {

        //public static FastList<Bullet> _bullets;
        public static Tile[] MapTiles;
        public static Vector2[] NetworkPlayers;
        public static byte[] MapBlocks;
        public static Matrix _projectionMatrix;
        public static SpriteFont Arial;

        public static FastList<Bullet> _PlayerBullets;
        public static FastList<Bullet> _EnemyBullets;
        private FastList<SpriteAtlas> _sprites;
        private List<SpriteAtlas> _spritesToAdd;
        private Effect _outline;//, _colorMask;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Model model;
        UI.UI _UI;
        public static PenumbraComponent penumbra;

        /*Discord.Discord ds = new(975495189948923975, 0); // (ulong)Discord.CreateFlags.Default;
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
        };*/

        //private string[] WindowTitleAddition = new string[3] { "ZAMN!", "Daaamn what you know about rollin down in the deep?", "14yo kid moment" };
        public Game1()
        {

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            penumbra = new(this)
            {
                AmbientColor = new Color(new Vector3(0.1f))
            };
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Services.AddService(penumbra);
            //Window.Title = Window.Title + ": " + WindowTitleAddition[rand.Next(0, 2)];
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 900;
            _graphics.PreferredBackBufferHeight = 696;
            _graphics.ApplyChanges();
            ProgressBar.Init(GraphicsDevice);
            Item.Load();

            _spritesToAdd = new List<SpriteAtlas>(50);
            _sprites = new FastList<SpriteAtlas>(50);
            _PlayerBullets = new();
            _EnemyBullets = new();
            MapTiles = Array.Empty<Tile>();
            Tile.Initialize();

            var sWidth = GraphicsDevice.Viewport.Width * .01f; // its 1 / (50 * 2), before it was / 50 / 2
            var sHeight = GraphicsDevice.Viewport.Height * 0.0089445438282648f;// its 1 / (55.9 * 2), before it was / 55.9 / 2
            _projectionMatrix = Matrix.CreateOrthographicOffCenter(-sWidth, sWidth, -sHeight, sHeight, -200f, 5000f);
            Camera.View = GraphicsDevice.Viewport;

            string path = $"{Environment.CurrentDirectory}\\ScreenShots";

            Directory.CreateDirectory(path);
            //ds.ActivityManagerInstance = ds.GetActivityManager();
            //ds.ActivityManagerInstance.UpdateActivity(activity, ActivityCheck);


            base.Initialize();
        }
        void ActivityCheck(Result r)
        {
            if (r != Result.Ok)
            {
                Console.WriteLine("Result not Ok");
            }
        }
        Texture2D table, wood, dirt;
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>("cursor"), 16, 16));

            model = Content.Load<Model>("Wall");
            var basiceff = ((BasicEffect)model.Meshes[0].Effects[0]);
            basiceff.TextureEnabled = true;
            basiceff.Texture = table = Content.Load<Texture2D>("bedrock");
            basiceff.View = Matrix.CreateLookAt(new Vector3(0, 45, 90), Vector3.Zero, Vector3.UnitZ);

            wood = Content.Load<Texture2D>("Canopy");
            dirt = Content.Load<Texture2D>("dirt");

            _outline = Content.Load<Effect>("outline");
            //_colorMask = Content.Load<Effect>("ColorMask");
            Arial = Content.Load<SpriteFont>("File");

            // Order here matters
            Tile.TileSet = Content.Load<Texture2D>("MCRTile");
            Enemy.tempText = Content.Load<Texture2D>("Characters");
            BasicEntity.SpriteSheet = new Texture2D[] {
                SpriteAtlas.AddPadding(Content.Load<Texture2D>("Forest"),ref BasicEntity.SRect),
            };
            Map.Map.Load("Ship", _spritesToAdd);
            _sprites.Add(new Player(Content.Load<Texture2D>("Characters"), Content.Load<Texture2D>("ElecticBullet")));

            Item.SpriteSheet = SpriteAtlas.AddPadding(Content.Load<Texture2D>("Items"), 16, 16, 4, 1);


            _UI = new(GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                _graphics,
                Content.Load<Texture2D>("slotconcept"),
                Content.Load<Texture2D>("DropBagUI"),
                Content.Load<Texture2D>("StatIcons"),
                _sprites[0] as Player);
            penumbra.Initialize();

            penumbra.Lights.Add((_sprites[0] as Player).Light);

            // TODO: use this.Content to load your game content here
        }
        const float tcfactor = 1f / (2.55f * 1.5f); // tile color factor

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }
            //ds.RunCallbacks();
            //ds.ActivityManagerInstance.UpdateActivity(activity, ActivityCheck);

            //_colorMask.Parameters["t"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            if (IsActive)
            {
                Input.PKState = Input.KState;
                Input.KState = Keyboard.GetState();
                Input.MState = Mouse.GetState();

                if (_spritesToAdd.Count != 0)
                {
                    foreach (var item in _spritesToAdd)
                    {
                        _sprites.Add(item);
                    }
                }

                _spritesToAdd.Clear();

                _UI.Update(_sprites, _graphics);

                for (int i = 0; i < _sprites.Length; i++)
                {
                    _sprites[i].Update(gameTime, _spritesToAdd);
                    if (!_sprites[i].isRemoved) continue;
                    _sprites.RemoveAt(i);
                    i--;
                }
                for (int i = 0; i < _EnemyBullets.Length; i++)
                {
                    _EnemyBullets[i].Update(gameTime, _spritesToAdd);
                    if (!_EnemyBullets[i].isRemoved) continue;
                    _EnemyBullets.RemoveAt(i);
                    i--;
                }
                for (int i = 0; i < _PlayerBullets.Length; i++)
                {
                    _PlayerBullets[i].Update(gameTime, _spritesToAdd);
                    if (!_PlayerBullets[i].isRemoved) continue;
                    _PlayerBullets.RemoveAt(i);
                    i--;
                }
                for (int i = 5; i <= 6; i++)
                {
                    if ((Tile.SRects[i].Y += 16) >= 512) Tile.SRects[i].Y = 0;
                }

                Camera.Follow(_sprites[0].Position);
                penumbra.Transform = Camera.Transform;

                if (Input.PKState.IsKeyDown(Keys.NumPad5) && !Input.KState.IsKeyDown(Keys.NumPad5))
                    TakeScreenshot(gameTime);
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            using (BinaryWriter bw = new(File.OpenWrite("Options")))
            {
                bw.Write(_UI.SlotSizeMultiplier);
                if (_sprites[0] is Player player)
                {
                    bw.Write(player.AllowRotation);
                }
                bw.Write(_graphics.SynchronizeWithVerticalRetrace);
            }
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
        public void TakeScreenshot(GameTime gameTime)
        {
            using (Texture2D tex = new(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight))
            {
                var data = new Color[tex.Width * tex.Height];
                GraphicsDevice.GetBackBufferData(data);
                tex.SetData(data);
                using (var stream = File.Create($"{Environment.CurrentDirectory}\\ScreenShots\\{DateTime.Now.ToString().Replace(':', '~')}.png"))
                {
                    tex.SaveAsPng(stream, tex.Width, tex.Height);
                }
            }
        }
        #endregion
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            var ppos = _sprites[0].Position;
            var rot = Matrix.CreateRotationZ(-Camera.RotDegr);
            var mesh = (BasicEffect)model.Meshes[0].Effects[0];
            mesh.Projection = _projectionMatrix * Matrix.CreateScale(Camera.CamZoom);

            //var roty = Matrix.CreateRotationY(MathHelper.ToRadians(-180));
            /*

            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            for (int i = 0; i < Player.xyCount.X; i++)
            {
                for (int j = 0; j < Player.xyCount.Y; j++)
                {
                    var index = Player.SquareOfSightStartIndex + i + j * Map.Map.Size.X;
                    if (index > MapTiles.Length) continue;
                    if (MapTiles[index] is not null)
                    {
                        mesh.Texture = dirt;
                        mesh.World =
                        Matrix.CreateTranslation(-ppos.X - Player.TiledPos.X + i + 0.5f, Player.TiledPos.Y + j + 0.5f - ppos.Y, 0) * rot * roty;
                        model.Meshes[0].Draw();
                        continue;
                    };
                }
            }*/
            penumbra.BeginDraw();

            _spriteBatch.Begin(transformMatrix: Camera.Transform, samplerState: SamplerState.PointClamp);

            for (int i = 0; i < Player.xyCount.X; i++)
            {
                var pos = new Vector2((Player.TiledPos.X + i) * 50, Player.TiledPos.Y * 50);

                for (int j = 0; j < Player.xyCount.Y; j++)
                {
                    var index = Player.SquareOfSightStartIndex + i + j * Map.Map.Size.X;

                    if (index > MapTiles.Length) continue;
                    if (MapTiles[index] is null) { pos.Y += 50; continue; }

                    _spriteBatch.Draw(Tile.TileSet, pos, Tile.SRects[Map.Map.byteMap[index]], Color.White, 0, Vector2.Zero, scale: 3.125f, 0, 0);
                    pos.Y += 50;
                }
            }

            Player.HPbar.Draw(_spriteBatch);

            _spriteBatch.End();
            _spriteBatch.Begin(transformMatrix: Camera.Transform, samplerState: SamplerState.PointClamp, effect: _outline);
            if (NetworkPlayers != null)
            {
                for (int i = 0; i < NetworkPlayers.Length; i++)
                {
                    _spriteBatch.Draw(_sprites[0].Texture, NetworkPlayers[i], new(0, 0, 9, 9), Color.White);
                }
            }

            for (int i = 0; i < BasicEntity.InteractEnt.Length; i++)
            {
                //    _outline.Parameters["uvPix"].SetValue(Vector2.One / (BasicEntity.InteractEnt[i].Rect.Size.ToVector2() * 4));
                BasicEntity.InteractEnt[i].Draw(_spriteBatch);
            }

            //_outline.Parameters["uvPix"].SetValue(new Vector2(1f / (32 * 4), 1f / (32 * 4)));

            for (int i = 0; i < _PlayerBullets.Length; i++)
            {
                _PlayerBullets[i].Draw(_spriteBatch);
            }

            for (int i = _sprites.Length - 1; i > -1; i--)
            {
                _sprites[i].Draw(_spriteBatch);
                if (_sprites[i] is Enemy e)
                {
                    e.HPbar.Draw(_spriteBatch);
                }
            }


            for (int i = 0; i < _EnemyBullets.Length; i++)
            {
                _EnemyBullets[i].Draw(_spriteBatch);
            }

            //_outline.Parameters["uvPix"].SetValue(new Vector2(1f / (50 * 5.5f), 1f / (50 * 5.5f)));

            _spriteBatch.End();

            // some 3D
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            ppos *= 0.02f;

            int pid = 0;

            for (int i = 0; i < Player.xyCount.X; i++)
            {
                for (int j = 0; j < Player.xyCount.Y; j++)
                {
                    var index = Player.SquareOfSightStartIndex + i + j * Map.Map.Size.X;
                    if (index > MapTiles.Length) continue;
                    if (MapTiles[index] is not null) continue;
                    if (index > 0 && pid != MapBlocks[index])
                    {
                        if (MapBlocks[index] == 7) mesh.Texture = table;
                        else if (MapBlocks[index] == 8) mesh.Texture = wood;
                    }
                    pid = MapBlocks[index];

                    mesh.World =
                    Matrix.CreateTranslation(ppos.X - Player.TiledPos.X - i - 0.5f, Player.TiledPos.Y + j + 0.5f - ppos.Y, 0) * rot;
                    model.Meshes[0].Draw();
                }
            }
            // not some 3D :(
            penumbra.Draw(gameTime);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.DrawString(Arial, "FPS | " + Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds).ToString(), Vector2.Zero, Color.IndianRed, 0, Vector2.Zero, 0.16f, 0, 0);

            _UI.Draw(_spriteBatch, _outline);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}