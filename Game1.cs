using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace Boxerman_v2 {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private float scale;

        public static float timedif;
        public static GraphicsDevice gd;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            int referenceWidth = 80;
            int referenceHeight = 60;
            _graphics.PreferredBackBufferWidth = 80 * 8;
            _graphics.PreferredBackBufferHeight = 60 * 8;
            // _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            UpdateScale(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, referenceWidth, referenceHeight);
        }
        private void UpdateScale(int currentWidth, int currentHeight, int referenceWidth, int referenceHeight) {
            float scaleX = (float)currentWidth / referenceWidth;
            float scaleY = (float)currentHeight / referenceHeight;

            // Use the minimum scale to maintain aspect ratio
            scale = Math.Min(scaleX, scaleY);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            timedif = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var kstate = Keyboard.GetState();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(new Color(41, 44, 51));

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(scale));

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
