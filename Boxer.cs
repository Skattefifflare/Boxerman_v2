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

       
        public bool facingright;
        public float pos;

        int health;
        int stamina;
        int resilience;      

        public Boxer(bool dir, float pos) { 
            health = 100;
            stamina = 100;
            resilience = 0;

            facingright = dir;
            this.pos = pos;

            string folder = (dir) ? "p1" : "p2";

            List<List<Texture2D>> spritematrix = new List<List<Texture2D>> {
                new List<Texture2D>{
                    Texture2D.FromFile(graphicsDevice, $"{folder}/Idle/Idle0.png")
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
                
                },//  2 : Uppercut
                new List<Texture2D>{
                
                },// 3 : Hook
                new List<Texture2D>{
                
                },// 4 : Block
                new List<Texture2D>{
                
                },// 5 : Forward
                new List<Texture2D>{
                
                },// 6 : Backward
                new List<Texture2D>{
                
                },
                new List<Texture2D> {

                }// 7 : Dodge

            };
            Thread boxerloop = new Thread(new ThreadStart(Boxerloop));
            boxerloop.Start();
            Thread animate = new Thread(new ThreadStart(Animate));
            animate.Start();


        }

        Action currentaction;
        public Texture2D currentsprite;

        public void Boxerloop() {
            while (true) {
                float t = Game1.timedif;
                var kstate = Game1.kstate;

                switch (kstate) {
                    case var _ when kstate.IsKeyDown(Keys.E) :
                        break;
                }
                currentaction();
            }
            
        }

        
        
        void Idle() {

        }
        void Jab() {

        }
        void Uppercut() {

        }
        void Hook() {

        }
        void Block() {

        }
        void Forward() {

        }
        void Backward() {

        }       
        void Dodge() {

        }
        void Animate() {
            foreach (var sprite in )
                
        }
        

    }
    
}
