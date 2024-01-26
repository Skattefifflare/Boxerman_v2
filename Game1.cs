using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static System.Formats.Asn1.AsnWriter;
using System.Threading;

namespace Boxerman_v2 {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private float scale;

        public static float timedif;
        public static GraphicsDevice gd;
        public static KeyboardState kstate;

        Boxer p1;
        Boxer p2;

        Effect effect1;


        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            gd = GraphicsDevice;
            p1 = new Boxer(true, 10);
            p2 = new Boxer(false, 60);

            effect1 = Content.Load<Effect>("HueShader2");

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            int referenceWidth = 80;
            int referenceHeight = 60;
            _graphics.PreferredBackBufferWidth = 80 * 10;
            _graphics.PreferredBackBufferHeight = 60 * 10;
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

            
            kstate = Keyboard.GetState();
            p1.Boxerloop();
            p2.Boxerloop();

            //p2.Boxerloop();
            /*
            Color[] pixels = new Color[p1.currentsprite.Width * p1.currentsprite.Height];
            p1.currentsprite.GetData(pixels);
            for (int y = 0; y < p1.currentsprite.Height; y++) {

                for (int x = p1.currentsprite.Width - 1; x >= 0; x--) {
                    Color pixelColor = pixels[y * p1.currentsprite.Width + x];

                    if (pixelColor.PackedValue == targetColor) {
                        rightmostX = x;
                        break;
                    }
                }

                if (rightmostX != -1)
                    break; // Found the rightmost pixel, no need to continue searching
            }
            // använd lambda för att hitta pixeln med rätt färg som är sist i arrayen och sedan gör mafs för att lista ut dess x-position
            */

            base.Update(gameTime);
        }

        void Hitcheck(ref Boxer hitter, ref Boxer punched) {

        }
        // SUCKER PUNCH!!!
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(new Color(41, 44, 51));

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, effect1, Matrix.CreateScale(scale));



            _spriteBatch.Draw(p1.bigbar, new Vector2(7, 4), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(p1.blip, new Vector2(7.3f, 4.3f), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f * p1.health, 0.3f), SpriteEffects.None, 0f);

            _spriteBatch.Draw(p1.smallbar, new Vector2(7, 7), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(p1.blip, new Vector2(7.3f, 7.3f), null, Color.White, 0f, Vector2.Zero, new Vector2(0.15f * p1.stamina, 0.18f), SpriteEffects.None, 0f);


            _spriteBatch.Draw(p2.bigbar, new Vector2(42, 4), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.FlipHorizontally, 0f);
            _spriteBatch.Draw(p2.blip, new Vector2(42.3f, 4.3f), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f * p2.health, 0.3f), SpriteEffects.FlipHorizontally, 0f);

            _spriteBatch.Draw(p2.smallbar, new Vector2(57, 7), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.FlipHorizontally, 0f);
            _spriteBatch.Draw(p2.blip, new Vector2(57.3f, 7.3f), null, Color.White, 0f, Vector2.Zero, new Vector2(0.15f * p2.stamina, 0.18f), SpriteEffects.FlipHorizontally, 0f);


            _spriteBatch.Draw(p1.currentsprite, new Vector2(p1.pos, 60 - 22), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(p2.currentsprite, new Vector2(p2.pos, 60 - 22), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);

            

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
