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

        int health;
        int stamina;
        int resilience;

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
          
            string folder = (dir) ? "p1" : "p2";

            spritematrix = new List<List<Texture2D>> {
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Idle/Idle0.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Idle/Idle1.png")
                },// 0 : Idle
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Jab/Jab0.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Jab/Jab1.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Jab/Jab2.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Jab/Jab3.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Jab/Jab4.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Jab/Jab5.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Jab/Jab6.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Jab/Jab7.png")
                },// 1 : Jab
                
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut0.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut1.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut2.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut3.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut4.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut5.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut6.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut7.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut8.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut9.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut10.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut11.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut12.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut13.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Uppercut/Uppercut14.png")

                },// 2 : Uppercut           
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Idle/Idle0.png"),
                },// 3 : Hook
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Block/Block0.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Block/Block1.png"),
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Block/Block2.png")
                },// 4 : Block
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Idle/Idle0.png"),
                },// 5 : Forward
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Idle/Idle0.png"),
                },// 6 : Backward
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Idle/Idle0.png"),
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
                    case var _ when kstate.IsKeyDown(Keys.E):                        
                        if (stamina > 10) {
                            currentaction = Jab;
                        }
                        break;                   // Jab 
                    case var _ when kstate.IsKeyDown(Keys.Q):                     // Uppercut
                        currentaction = Uppercut;
                        break;
                    case var _ when kstate.IsKeyDown(Keys.W):                   // Hook
                        currentaction = Hook;
                        break;
                    case var _ when kstate.IsKeyDown(Keys.LeftShift):         // Block
                        currentaction = Block;
                        break;
                    case var _ when kstate.IsKeyDown(Keys.D):               // Forward
                        currentaction = (facingright) ? Right : Left;
                        break;
                    case var _ when kstate.IsKeyDown(Keys.A):             // Backward
                        currentaction = (facingright) ? Left : Right;
                        break;
                    case var _ when kstate.IsKeyDown(Keys.LeftControl): // Dodge
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
            while (kstate.IsKeyDown(Keys.LeftShift)) {
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
            // kod...

            gotHit = false;
        }
    }
    
}
