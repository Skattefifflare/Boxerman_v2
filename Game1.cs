using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static System.Formats.Asn1.AsnWriter;
using System.Threading;
using System.Linq;

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
            p2 = new Boxer(false, 0);

            effect1 = Content.Load<Effect>("HueShader2");
            //effect1.Parameters["hueChange"].SetValue(4f);

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
            if (p1.spritematrix[1].Contains(p1.currentsprite) || p1.spritematrix[2].Contains(p1.currentsprite)) {
                Hitcheck(ref p1, ref p2);
            }
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
            float hitterGloveX = 0;
            float punchedHeadX = 0;

            Color glovecolor = new Color(153, 78, 65);
            Color headcolor = new Color(88, 129, 87);

            Color[] hitterpixels = new Color[hitter.currentsprite.Width * hitter.currentsprite.Height];
            hitter.currentsprite.GetData(hitterpixels);
            Color[] punchedpixels = new Color[punched.currentsprite.Width * punched.currentsprite.Height];
            punched.currentsprite.GetData(hitterpixels);


            Color[] buffer;

            for (int i = 0; i < hitterpixels.Length; i += hitter.currentsprite.Width) {
                buffer = new Color[hitter.currentsprite.Width];
                Array.Copy(hitterpixels, i, buffer, 0, hitter.currentsprite.Width);
                Array.Reverse(buffer, 0, buffer.Length);
                
                int X = Array.IndexOf(buffer, glovecolor);
                if (X > hitterGloveX) {
                    hitterGloveX = X;
                }
            }

            for (int i = 0; i < punchedpixels.Length; i += 100) {
                buffer = new Color[punched.currentsprite.Width];
                Array.Copy(punchedpixels, i, buffer, 0, punched.currentsprite.Width);
                Array.Reverse(buffer, 0, buffer.Length);
                
                int X = Array.IndexOf(buffer, headcolor);
                if (X > punchedHeadX) {
                    punchedHeadX = X;
                }
            }

            if (hitter.facingright) {
                if (hitter.pos + hitterGloveX >= punched.pos + punchedHeadX * 2) {
                    punched.health -= 100;
                }
            }

            // kontrukta array med arrays som är alla horisontella segment av spriten och sedan hitta den array med en r'tt f'rgad pixel längst ut
        }

        // SUCKER PUNCH!!!
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(new Color(41, 44, 51));


            // unshadade grejer
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(scale));
            _spriteBatch.Draw(p1.currentsprite, new Vector2(p1.pos, 60 - 22), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(p1.bigbar, new Vector2(7, 4), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(p1.blip, new Vector2(7.3f, 4.3f), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f * p1.health, 0.3f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(p1.smallbar, new Vector2(7, 7), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(p1.smallbar, new Vector2(7, 9), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.None, 0f);

            _spriteBatch.Draw(p2.bigbar, new Vector2(42, 4), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.FlipHorizontally, 0f);
            _spriteBatch.Draw(p2.blip, new Vector2(42.3f, 4.3f), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f * p2.health, 0.3f), SpriteEffects.FlipHorizontally, 0f);
            _spriteBatch.Draw(p2.smallbar, new Vector2(57, 7), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.FlipHorizontally, 0f);
            _spriteBatch.Draw(p2.smallbar, new Vector2(57, 9), null, Color.White, 0f, Vector2.Zero, new Vector2(0.3f, 0.3f), SpriteEffects.FlipHorizontally, 0f);
            _spriteBatch.End();

            // stamina
            effect1.Parameters["rChange"].SetValue(1.28787f);
            effect1.Parameters["gChange"].SetValue(3.0909f);
            effect1.Parameters["bChange"].SetValue(1.21428f);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, effect1, Matrix.CreateScale(scale));           
            _spriteBatch.Draw(p1.blip, new Vector2(7.3f, 7.3f), null, Color.White, 0f, Vector2.Zero, new Vector2(0.15f * p1.stamina, 0.18f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(p2.blip, new Vector2(72.3f, 7.5f), null, Color.White, 0f, Vector2.One, new Vector2(0.15f * p2.stamina, 0.18f), SpriteEffects.FlipHorizontally, 0f);
            _spriteBatch.End();

            // resilience
            effect1.Parameters["rChange"].SetValue(0.15656f);
            effect1.Parameters["gChange"].SetValue(3f);
            effect1.Parameters["bChange"].SetValue(5.3095f);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, effect1, Matrix.CreateScale(scale));
            _spriteBatch.Draw(p1.blip, new Vector2(7.3f, 9.3f), null, Color.White, 0f, Vector2.Zero, new Vector2(0.15f * (100 - p1.resilience), 0.18f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(p2.blip, new Vector2(72.3f, 9.5f), null, Color.White, 0f, Vector2.One, new Vector2(0.15f * (100 - p1.resilience), 0.18f), SpriteEffects.FlipHorizontally, 0f);
            _spriteBatch.End();

            // p2
            effect1.Parameters["rChange"].SetValue(1f);
            effect1.Parameters["gChange"].SetValue(0.8682f);
            effect1.Parameters["bChange"].SetValue(1.5057f);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, effect1, Matrix.CreateScale(scale));
            _spriteBatch.Draw(p2.currentsprite, new Vector2(p2.pos, 60 - 22), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
