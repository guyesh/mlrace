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
    enum GroundTypes { wall, road}
    class Map
    {
        #region data
        public Texture2D tex { get; private set; }
        #endregion
        GroundTypes[,] gt;
        public GroundTypes this[Vector2 position]
        {
            get
            {
                int x = (int)(position.X / G.bgscale);
                int y = (int)(position.Y / G.bgscale);
                if(x < 0 || x >= gt.GetLength(0) || y < 0 || y >= gt.GetLength(1))
                {
                    return GroundTypes.wall;
                }
                return gt[x, y];
            }

        }

        public Map(string maskname)
        {
            tex = G.cm.Load<Texture2D>(maskname);
            gt = new GroundTypes[tex.Width, tex.Height];
            Color[] c = new Color[tex.Width * tex.Height];
            tex.GetData<Color>(c);
            Color wall = c[1];
            Color road = c[0];
            for (int y = 0; y < tex.Height; y++)
            {
                for (int x = 0; x < tex.Width; x++)
                {
                    if (c[x + y * tex.Width] == wall) gt[x, y] = GroundTypes.wall;
                    if (c[x + y * tex.Width] == road) gt[x, y] = GroundTypes.road;
                }

            }
            Game1.drawing += draw;
        }
        public GroundTypes getground(Vector2 pos)
        {
            return this[pos];
        }
        void draw()
        {
            G.sb.Draw(tex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, G.bgscale, 0, 0);
        }
        public float getraylen(Vector2 pos, float rot)
        {
            float res = 0;
            while (getground(pos + G.direction_from_rot(rot) * res) == GroundTypes.road)
            {
                res += 20;
            }
            while (res > 0 && getground(pos + G.direction_from_rot(rot) * res) != GroundTypes.road)
            {
                res -= 2;
            }
            return res;
        }



    }
}
