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
    class Car
    {
        #region data
        List<Vector2> wheelpoints;
        List<Vector2> wheelpointsMoving;
        Engine eng;
        Keycontrol kc = new Keycontrol();
        public Texture2D Texture { get; private set; }
        public Vector2 position;
        public float rot = 0;
        public Color color;
        public Vector2 Origin { get; private set; }
        public Vector2 scale;
        public float layerDepth;
        float[] raylen = new float[5];
        const float ang1 = G.PI * 0.25f;
        float[] rayangle = {-2*ang1, -ang1, 0, ang1, 2*ang1 };
        NeuralNet nn;
        public int Score { get; private set; }
        Vector2 last_loc;


     

        #endregion
        #region ctor
        void startstuff()                               
        {
            G.Carnum++;
            wheelpoints = new List<Vector2>();
            wheelpointsMoving = new List<Vector2>();
            for (int i = 0; i < G.carwheelx.Length; i++)
            {
                wheelpoints.Add(new Vector2(G.carwheelx[i], G.carwheely[i]));
                wheelpointsMoving.Add(Vector2.Zero);
            }
            Origin = new Vector2(400, 145);
            Texture = G.cm.Load<Texture2D>("car");
            color = Color.White;
            scale = new Vector2(0.2f);
            layerDepth = 0;
            Game1.drawing += draw;
            Game1.updating += update;
            this.eng = new Engine(G.engine1);
            Score = 0;
            last_loc = position;
        }
        public Car(int x, int y)
        {
            position = new Vector2(x, y);
            startstuff();  //dont swap lines!
            nn = new NeuralNet(G.INPUTNUM, G.HIDDENNUM, G.OUTPUTNUM);
        }
        void fill_movingwheelpoints()
        {
            for (int i = 0; i < wheelpoints.Count; i++)
            {
                Vector2 delta = wheelpoints[i] - Origin;
                wheelpointsMoving[i] = position + G.rotate_vec(delta, rot, scale.X);
            }
        }

        public Car(Car b, int x, int y)
        {
            position = new Vector2(x, y);
            startstuff();  //dont swap lines!
            nn = new NeuralNet(b.nn);
        }

        #endregion
        #region funcs

        public void writetofile(string s)
        {
            nn.writetofile(s);
        }

        int sandpoints()
        {
            int res = 0;
            for (int i = 0; i < wheelpointsMoving.Count; i++)
                if (G.map.getground(wheelpointsMoving[i]) != GroundTypes.road)
                    res++;
            return res;
        }

        bool is_road(int l, float angle)
        {
            Vector2 v = position + l * G.direction_from_rot(rot+angle);
            return G.map.getground(v) == GroundTypes.road;
        }

        void calculate_raylen()
        {
            for (int i = 0; i < 5; i++)
            {
                int l = 0;
                while (is_road(l + G.SANDJUMP, rayangle[i]))
                {
                    l += G.SANDJUMP;
                }
                while (is_road(l + 1, rayangle[i]))
                {
                    l += 1;
                }
                raylen[i] = l;

            }
        }
        public void mutate()
        {
            nn.mutate();
        }
        public void kill()
        {
            Game1.drawing -= draw;
            Game1.updating -= update;
            G.Carnum--;
        }

        public void update()
        {
            if ((position - last_loc).Length() > G.MIN_TRAVEL)
            {
                Score++;
                last_loc = position;
            }

            kc.up = true;
            kc.down = false;

            float[] nn_input = new float[6];
            for (int i = 0; i < 5; i++)
            {
                nn_input[i] = raylen[i];
            }
            float ans1 = nn.calculate(nn_input)[0];
            for (int i = 0; i < 5; i++)
            {
                nn_input[i] = raylen[4 - i];
            }
            float ans2 = nn.calculate(nn_input)[0];

            float real_ans = ans1 - ans2;


            kc.left = real_ans > 0.5;
            kc.right = real_ans < -0.5;

            int sp = sandpoints();
            if (sp == 6)
            {
                kill();
                return;
            }
            eng.engine_update(kc, sp);
            rot += eng.wheelrot * eng.speed * 0.02f;
            position += eng.speed * 0.2f * G.direction_from_rot(rot);
            calculate_raylen();
            fill_movingwheelpoints();
        }
        public void draw()
        {
            color = new Color(100,100,100);
            Vector2 drawscale = scale;
            G.sb.Draw(Texture, position, null, color, rot,
            Origin, drawscale, 0, 100 * 0.000001f);
            for (int i = 0; i < 5; i++)
            {
                G.drawline(position, rot+rayangle[i], raylen[i]);
            }
        }
        #endregion
    }
}
