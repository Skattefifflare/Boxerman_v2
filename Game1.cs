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

        SpriteFont f1;


        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            gd = GraphicsDevice;
            p1 = new Boxer(true, 10);
            p2 = new Boxer(false, 40);

            effect1 = Content.Load<Effect>("HueShader2");

            f1 = Content.Load<SpriteFont>("Font1");

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
            float gloveX = 0;
            float headX = 0;

            Color glovecolor = new Color(153, 78, 65);
            Color headcolor = new Color(88, 129, 87);
            Texture2D hittersprite_temp = hitter.currentsprite;
            Texture2D punchedsprite_temp = punched.currentsprite;

            Color[] hitterpixels = new Color[hittersprite_temp.Width * hittersprite_temp.Height];
            hittersprite_temp.GetData(hitterpixels);
            Color[] punchedpixels = new Color[punchedsprite_temp.Width * punchedsprite_temp.Height];
            punchedsprite_temp.GetData(hitterpixels);


            
            //gloveX = hitterpixels.Select(a => a == glovecolor).Max(a => Array.IndexOf(hitterpixels, a) % 22);

            foreach (var pixel in hitterpixels) {
                if(pixel == glovecolor) {
                    if (Array.IndexOf(hitterpixels, pixel) % hittersprite_temp.Width > gloveX) {
                        gloveX = Array.IndexOf(hitterpixels, pixel) % hittersprite_temp.Width;
                    }
                }
            }
            //headX = punchedpixels.Select(a => a == headcolor).Max(a => Array.IndexOf(punchedpixels, a) % 22);

            foreach (var pixel in punchedpixels) {
                if (pixel == headcolor) {
                    if (Array.IndexOf(punchedpixels, pixel) % punchedsprite_temp.Width > headX) {
                        headX = Array.IndexOf(punchedpixels, pixel) % punchedsprite_temp.Width;
                    }
                } 
            }


            
            /*
            Color[] buffer;

            for (int i = 0; i < hittersprite_temp.Height; i++) {
                
                buffer = new Color[hittersprite_temp.Width];
                Array.Copy(hitterpixels, i * hittersprite_temp.Width, buffer, 0, hittersprite_temp.Height);
                int X = Array.LastIndexOf(buffer, glovecolor) + 1;
                if (X > hitterGloveX) {
                    hitterGloveX = X;
                }
            }

            for (int i = 0; i < punchedsprite_temp.Height; i++) {
                buffer = new Color[punchedsprite_temp.Width];
                Array.Copy(punchedpixels, i * punchedsprite_temp.Width, buffer, 0, punchedsprite_temp.Height);
                int X = Array.LastIndexOf(buffer, headcolor) + 1;
                if (X > punchedHeadX) {
                    punchedHeadX = X;
                }
            }

            
            for (int i = 0; i < hitterpixels.Length; i += hitter.currentsprite.Width) {
                buffer = new Color[hitter.currentsprite.Width];
                Array.Copy(hitterpixels, i, buffer, 0, hitter.currentsprite.Width);

                
                int X = Array.LastIndexOf(buffer, glovecolor) + 1;
                if (X > hitterGloveX) {
                    hitterGloveX = X;
                }
            }
            
            for (int i = 0; i < punchedpixels.Length; i += punched.currentsprite.Width) {
                buffer = new Color[punched.currentsprite.Width];
                Array.Copy(punchedpixels, i, buffer, 0, punched.currentsprite.Width);
                
                int X = Array.LastIndexOf(buffer, headcolor) + 1;
                if (X > punchedHeadX) {
                    punchedHeadX = X;
                }
            }
            */

            if (hitter.facingright) {
                if (hitter.pos + gloveX >= 30) {
                    punched.health -= 1;
                    hitter.hasDoneHit = true;
                }
            }
            else if (!hitter.facingright) {
                if (hitter.pos - gloveX < punched.pos + headX) {
                    punched.health -= 1;
                    hitter.hasDoneHit = true;
                }
            }
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

            _spriteBatch.DrawString(f1, $"{1 / timedif}", new Vector2(1, 0), Color.White, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(f1, " |\nV", new Vector2(30, 12), Color.White, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 0f);

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
