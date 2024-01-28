using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Boxerman_v2 {
    internal class Boxer {
        public GraphicsDevice graphicsDevice = Game1.gd;
        Action currentaction;
        Thread actionthread;
        public bool hasDoneHit;
        public bool gotHit;
        public bool isBlocking;
        KeyboardState kstate;

        public bool facingright;
        public float pos;

        public int health;
        public int stamina;
        public int resilience;

        public Texture2D smallbar;
        public Texture2D bigbar;
        public Texture2D blip;

        List<List<Texture2D>> spritematrix;

        public Boxer(bool dir, float pos) {
            currentaction = Idle;
            actionthread = new Thread(() => currentaction());
            hasDoneHit = false;
            isBlocking = false;
            

            facingright = dir;
            this.pos = pos;

            health = 100;
            stamina = 100;
            resilience = 0;

            smallbar = Texture2D.FromFile(graphicsDevice, "../../../boxer/smallbar.png");
            bigbar = Texture2D.FromFile(graphicsDevice, "../../../boxer/bigbar.png");
            blip = Texture2D.FromFile(graphicsDevice, "../../../boxer/blip.png");

            spritematrix = new List<List<Texture2D>> {
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Idle/Idle0.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Idle/Idle1.png")
                },// 0 : Idle
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Jab/Jab0.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Jab/Jab1.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Jab/Jab2.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Jab/Jab3.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Jab/Jab4.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Jab/Jab5.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Jab/Jab6.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Jab/Jab7.png")
                },// 1 : Jab
                
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut0.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut1.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut2.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut3.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut4.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut5.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut6.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut7.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut8.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut9.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut10.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut11.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut12.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut13.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Uppercut/Uppercut14.png")

                },// 2 : Uppercut           
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Idle/Idle0.png"),
                },// 3 : Hook
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Block/Block0.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Block/Block1.png"),
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Block/Block2.png")
                },// 4 : Block
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Idle/Idle0.png"),
                },// 5 : Forward
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, "../../../boxer/Idle/Idle0.png"),
                },// 6 : Backward
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, $"../../../boxer/Idle/Idle0.png"),
                } // 7 : Dodge              
            }; 
        }

        
        public Texture2D currentsprite;
        
        
        public void Boxerloop() { 
            if (gotHit && currentaction != GotHit) {
                currentaction = GotHit;
                actionthread = new Thread(() => currentaction());
                actionthread.Start();
            }
            else if (!actionthread.IsAlive || currentaction == Idle) {
                kstate = Game1.kstate;

                switch (kstate) {
                    case var _ when (kstate.IsKeyDown(Keys.E) && facingright) || ((kstate.IsKeyDown(Keys.U)) && !facingright):                        
                        if (stamina > 10) {
                            currentaction = Jab;
                        }
                        break;                                                    
                    case var _ when (kstate.IsKeyDown(Keys.Q) && facingright) || ((kstate.IsKeyDown(Keys.O)) && !facingright):
                        currentaction = Uppercut;
                        break;
                    case var _ when (kstate.IsKeyDown(Keys.W) && facingright) || ((kstate.IsKeyDown(Keys.I)) && !facingright):
                        currentaction = Hook;
                        break;
                    case var _ when (kstate.IsKeyDown(Keys.LeftShift) && facingright) || ((kstate.IsKeyDown(Keys.RightShift)) && !facingright):
                        currentaction = Block;
                        break;
                    case var _ when (kstate.IsKeyDown(Keys.D) && facingright) || ((kstate.IsKeyDown(Keys.J)) && !facingright):
                        currentaction = (facingright) ? Right : Left;
                        break;
                    case var _ when (kstate.IsKeyDown(Keys.A) && facingright) || ((kstate.IsKeyDown(Keys.L)) && !facingright):
                        currentaction = (facingright) ? Left : Right;
                        break;
                    case var _ when (kstate.IsKeyDown(Keys.LeftControl) && facingright) || ((kstate.IsKeyDown(Keys.Space)) && !facingright):
                        currentaction = Dodge;
                        break;
                    default:                    
                        if (!actionthread.IsAlive) {
                            currentaction = Idle;
                        }
                        break;
                }
                if (!actionthread.IsAlive || currentaction != Idle) {
                    actionthread = new Thread(() => currentaction());
                    actionthread.Start();
                }                            
            }                       
        }
        // actionmetoderna kommer kallas som trådar och köras tills de är klara
        

        void Idle() {
            currentsprite = spritematrix[0][0];
            Thread.Sleep(400);
            currentsprite = spritematrix[0][1];
            Thread.Sleep(400);
        }
        void Jab() {
            int i = 0;
            while (!hasDoneHit && i <= 7) {
                currentsprite = spritematrix[1][i];
                Thread.Sleep(20);
                i++;
            }
            i--;
            while (i > 0) {
                currentsprite = spritematrix[1][i];
                Thread.Sleep(20);
                i--;
            }
            Thread.Sleep(100);
            // hitcheckern måste på något sätt säga till Jab att den har träffat och att animationen ska börja spelas i reverse
            // spriten ska börja reversas när man träffar
        }
        void Uppercut() {
            int i = 0;
            while (!hasDoneHit && i <= 14) {
                currentsprite = spritematrix[2][i];
                Thread.Sleep(25);
                i++;
            }          
            Thread.Sleep(100);
        }
        void Hook() {

        }
        void Block() {
            currentsprite = spritematrix[4][0];
            Thread.Sleep(10);
            currentsprite = spritematrix[4][1];
            Thread.Sleep(10);
            currentsprite = spritematrix[4][2];  
            isBlocking = true;
            while ((kstate.IsKeyDown(Keys.LeftShift) &&facingright) || (kstate.IsKeyDown(Keys.RightShift) && !facingright)) {
                kstate = Game1.kstate;
                currentsprite = spritematrix[4][2]; // behövs för ett edge-case
            }
            isBlocking = false;
            currentsprite = spritematrix[4][1];
            Thread.Sleep(10);
            currentsprite = spritematrix[4][0];
            
        }
        void Right() {
            float init_pos = pos;
            while (pos < init_pos + 4) {
                pos += 0.08f;
                Thread.Sleep(2);
            }  
        }
        void Left() {
            float init_pos = pos;
            while (pos > init_pos - 4) {
                pos -= 0.08f;
                Thread.Sleep(2);
            }
        }       
        void Dodge() {

        }
        void GotHit() {
            // kalla left / right
            // stun

            gotHit = false;
        }
    }
    
}
