using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace basic
{
    public delegate void DlgUpdate();
    public delegate void DlgDraw();

    struct Ray
    {
        public float direction;
        public float length;
    }
    struct Keycontrol
    {
        public bool up;
        public bool down;
        public bool right;
        public bool left;
    }

    static class G
    {
        public const float MIN_TRAVEL = 200;
        public const float PI = 3.1415926535f;
        public const int SANDJUMP = 10;
        public const int SCREENW = 2000;
        public const int SCREENH = 1200;
        public const int MAXPOP = 230;
        public const float TRIPSTEPS = 200;
        public static int roundcntr = 0;
        public static Texture2D pixel;
        public static SpriteBatch sb;
        public static KeyboardState kb;
        public static ContentManager cm;
        public static List<Car> cars;
        public static Random rnd = new Random();
        public static float bgscale = 1.0000005f;
        public static Map map;
        public const float STEERSPEED = 0.005f;
        public static int W, H;
        public static Engine engine1;
        public static Engine engine2;
        public const int INPUTNUM = 5;
        public const int HIDDENNUM = 3;
        public const int OUTPUTNUM = 1;
        public static int[] carwheelx = { 603, 483, 483, 115, 115, 0 };
        public static int[] carwheely = { 153, 23, 282, 27, 278, 153 };
        public static int xstart = (int)(837 * bgscale);
        public static int ystart = (int)(931 * bgscale);
        public static int FrameCntr = 0;
        public static int IterationCntr = 0;
        public static int Carnum = 0;

        public static void init(GraphicsDevice gd, ContentManager cm)
        {
            G.cm = cm;
            cars = new List<Car>();
            sb = new SpriteBatch(gd);
            Game1.updating += update;
            pixel = new Texture2D(gd, 1, 1);
            pixel.SetData<Color>(new Color[1] { Color.White });
            map = new Map("track");
            rnd = new Random(0);
            W = 1200;
            H = 700;
            // engine1 = new Engine(0, 10, 3f, 10, 0.02f);
            engine1 = new Engine(0, 20, 4f, 20, 0.08f);
            engine2 = new Engine(0, 1200, 0.2f, 22, 100);

        }
        public static Vector2 direction_from_rot(float rot)
        {
            return Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(rot));
        }
        public static Vector2 rotate_vec(Vector2 vec, float rot, float scale = 1)
        {
            return Vector2.Transform(vec, Matrix.CreateRotationZ(rot) *
                Matrix.CreateScale(scale));
        }
        public static void drawline(Vector2 o, float azimut, float len = 200, 
            float thickness = 5)
        {
            sb.Draw(pixel, o, null, Color.IndianRed, azimut + PI*0.5f, new Vector2(0.5f, 1),
                new Vector2(thickness, len), 0, 0);
        }
        private static void update()
        {
            kb = Keyboard.GetState();
        }
    }
}
