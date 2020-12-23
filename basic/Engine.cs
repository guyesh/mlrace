using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace basic
{
    class Engine
    {
        const float MAXRPM = 6000;
        const float RPMINC = 250;
        const float FRICTION = 0.002f;
        const float SANDFACTOR = 4;
        const float MINRPM = 1000;


        #region data
        public float wheelrot { get; private set; }
        public float speed { get; private set; }
        float rpm;
        float power;
        float maxpower;
        float maxsteer;
        float maxspeed;
        float breakpower = 1;

        #endregion
        #region ctor
        public Engine(Engine e)
        {
            maxpower = e.maxpower;
            maxsteer = e.maxsteer;
            maxspeed = e.maxspeed;
            breakpower = e.breakpower;
            wheelrot = 0;
            rpm = 0;
            power = 0;
            speed = 0;

        }
        public Engine(float responsetime, float maxpower,
            float maxsteer, float maxspeed, float maxbreak)
        {
            this.maxpower = maxpower;
            this.maxsteer = maxsteer;
            this.maxspeed = maxspeed;
            this.breakpower = maxbreak;
            wheelrot = 0;
            rpm = 0;
            power = 0;
            speed = 0;
        }

        #endregion
        #region func
        public void engine_update(Keycontrol keys, int sandpoints)
        {
            if (keys.right)
            {
                wheelrot = Math.Min(maxsteer, wheelrot + G.STEERSPEED);
                if (wheelrot < 0)
                    wheelrot = Math.Min(maxsteer, wheelrot + G.STEERSPEED);

            }
            else if (keys.left)
            {
                wheelrot = Math.Max(-maxsteer, wheelrot - G.STEERSPEED);
                if (wheelrot > 0)
                    wheelrot = Math.Max(-maxsteer, wheelrot - G.STEERSPEED);
            }
            else wheelrot = 0;
            power = 0;
            if (keys.up)
            {
                rpm += RPMINC;
                if (rpm > MAXRPM) rpm = MAXRPM;
                power = rpm / MAXRPM * maxpower / RPMINC;
            }
            if (keys.down)
            {
                rpm = MINRPM;
                speed = speed - breakpower;
            }
            float actual_friction = FRICTION * (sandpoints * SANDFACTOR + 1);
            speed = speed - speed * actual_friction + power / (speed + 0.2f);
            if (speed < 0) speed = 0;
            if (speed > maxspeed) speed = maxspeed;
        }
        #endregion
    }
}
