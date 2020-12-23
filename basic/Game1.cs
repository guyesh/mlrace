using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
namespace basic
{
     public class Game1 : Game
    {
        public static DlgDraw drawing;
        public static DlgUpdate updating;
        GraphicsDeviceManager graphics;
        Camera cam;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = G.SCREENW;
            graphics.PreferredBackBufferHeight = G.SCREENH;
            graphics.ApplyChanges();
             base.Initialize();
        }
        protected override void LoadContent()
        {
            G.init(GraphicsDevice, Content);
            for (int i = 0; i < 30; i++)
            {
                G.cars.Add(new Car(G.xstart, G.ystart));
            }
            cam = new Camera(G.cars[0]);
        }
        int carcompare(Car c1, Car c2)
        {
            return c2.Score - c1.Score;
        }

        void restart_cars()
        {
            G.cars.Sort(carcompare);
            G.cars[0].writetofile("bestcar");

            List<Car> tmp = new List<Car>();

            for (int i = 0; i < 10; i++)
            {
                tmp.Add(new Car(G.cars[i], G.xstart, G.ystart));
                G.cars[i].mutate();
                tmp.Add(new Car(G.cars[i], G.xstart, G.ystart));
                G.cars[i].mutate();
                tmp.Add(new Car(G.cars[i], G.xstart, G.ystart));
            }
            for (int i = 0; i < G.cars.Count; i++)
            {
                G.cars[i].kill();
            }
            G.cars = tmp;
        }

        protected override void Update(GameTime gameTime)
        {
            updating?.Invoke();
            base.Update(gameTime);
            G.FrameCntr++;
            if (G.FrameCntr == 80 + G.IterationCntr * 200 || G.Carnum == 0)
            {
                restart_cars();
                G.FrameCntr = 0;
                G.IterationCntr++;
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            G.sb.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, cam.Mat);
            drawing?.Invoke();
            G.sb.End();
            base.Draw(gameTime);
        }
    }
}
