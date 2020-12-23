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

    class Camera
    {
        public Matrix Mat { get; private set; }
        Viewport vp;
        Vector2 pos;
        Car car;
        float zoom;
        public Camera(Car car)
        {
            this.car = car;
            this.vp = new Viewport(0,0,G.SCREENW, G.SCREENH);
            this.pos = Vector2.Zero;
            this.zoom = 0.6f;
            Game1.updating += update;
        }
        void update()
        {
            //Mat = Matrix.CreateTranslation(-pos.X, -pos.Y, 0) *
            Mat = Matrix.CreateTranslation(-1000, -1000, 0) *
                Matrix.CreateScale(zoom) * Matrix.CreateRotationZ(0) *
                Matrix.CreateTranslation(vp.X + vp.Width / 2, vp.Y + vp.Height / 2, 0);
            pos = Vector2.Lerp(pos, car.position, 0.03f);

        }
    }
}
