using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        }

        Action currentaction;

        public void Boxerloop(KeyboardState kstate) {
            float t = Game1.timedif;

            currentaction();
        }

        

        void Jab() {

        }
        void Uppercut() {

        }
        void Hook() {

        }
        void Block() {

        }
        void Backward() {

        }
        void Forward() {

        }
        void Dodge() {

        }

        
    }
}
