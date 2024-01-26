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
                }// 1 : Jab
                /*
                new List<Texture2D>{
                    // Texture2D.FromFile(graphicsDevice, $"{folder}/")
                },// 2 : Uppercut
                new List<Texture2D>{
                
                },// 3 : Hook
                new List<Texture2D>{
                
                },// 4 : Block
                new List<Texture2D>{
                
                },// 5 : Forward
                new List<Texture2D>{
                
                },// 6 : Backward
                new List<Texture2D>{
                
                } // 7 : Dodge
                */
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
                var kstate = Game1.kstate;
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
            Console.Beep(600, 20);
            Console.Beep(500, 20);
            Console.Beep(400, 20);
            Console.Beep(300, 20);
            Console.Beep(200, 20);
            while (i > 0) {
                currentsprite = spritematrix[1][i];
                Thread.Sleep(20);
                i--;
            }
            Thread.Sleep(200);
            // hitcheckern måste på något sätt säga till Jab att den har träffat och att animationen ska börja spelas i reverse
            // spriten ska börja reversas när man träffar
        }
        void Uppercut() {

        }
        void Hook() {

        }
        void Block() {

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
