using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Xml.Serialization;

namespace basic
{
    public class Too1
    {
        public string i = "9";
        public string ii = "98";
        public string iui = "988";
    }
    public class Too2
    {
        public int i = 9;
        public float f = 2.5f;
        public Too1 g = new Too1();
    }
    public class NNwrite
    {
        public float[] kin, kout;
        public float[] bin, bout;
        public Too2 t = new Too2();
        public NNwrite()
        {
            kin = new float[G.INPUTNUM * G.HIDDENNUM];
            kout = new float[G.OUTPUTNUM * G.HIDDENNUM];
            bin = new float[G.HIDDENNUM];
            bout = new float[G.OUTPUTNUM];
        }
    }

    class NeuralNet
    {
        float[,] kin, kout;
        float[] bin, bout;
        int InputNum, HiddenNum, OutputNum;



        public NeuralNet(int InputNum, int HiddenNum, int OutputNum)
        {
            this.InputNum = InputNum;
            this.HiddenNum = HiddenNum;
            this.OutputNum = OutputNum;
            kin = new float[InputNum, HiddenNum];
            kout = new float[HiddenNum, OutputNum];
            bin = new float[HiddenNum];
            bout = new float[OutputNum];
            for (int i = 0; i < HiddenNum; i++)
            {
                for (int j = 0; j < InputNum; j++)
                {
                    kin[j, i] = (50 - G.rnd.Next(100)) * 0.1f;
                }
                bin[i] = (50 - G.rnd.Next(100)) * 0.1f;
                for (int j = 0; j < OutputNum; j++)
                {
                    kout[i, j] = (50 - G.rnd.Next(100)) * 0.1f;
                }
            }
            for (int i = 0; i < OutputNum; i++)
                bout[i] = (50 - G.rnd.Next(100)) * 0.1f;
        }
        public NeuralNet(NeuralNet nn)
        {
            this.InputNum = nn.InputNum;
            this.HiddenNum = nn.HiddenNum;
            this.OutputNum = nn.OutputNum;
            kin = new float[InputNum, HiddenNum];
            kout = new float[HiddenNum, OutputNum];
            bin = new float[HiddenNum];
            bout = new float[OutputNum];
            for (int i = 0; i < HiddenNum; i++)
            {
                for (int j = 0; j < InputNum; j++)
                {
                    kin[j, i] = nn.kin[j, i];
                }
                bin[i] = nn.bin[i];
                for (int j = 0; j < OutputNum; j++)
                {
                    kout[i, j] = nn.kout[i, j];
                }
            }
            for (int i = 0; i < OutputNum; i++)
                bout[i] = nn.bout[i];
        }
        float relu(float x)
        {
            return (float)Math.Tanh(x / 2);
            //return 0.5f + 0.5f * (float)Math.Tanh(x / 2);//sigmoid
            //return Math.Max(x, 0);
            //return x;
        }
        public float[] calculate(float[] input)
        {
            float[] h = new float[HiddenNum];
            for (int i = 0; i < HiddenNum; i++)
            {
                h[i] = 0;
                for (int j = 0; j < InputNum; j++)
                    h[i] += input[j] * kin[j, i];
                h[i] += bin[i];
                h[i] = relu(h[i]);
            }
            float[] res = new float[OutputNum];
            for (int i = 0; i < OutputNum; i++)
            {
                res[i] = 0;
                for (int j = 0; j < HiddenNum; j++)
                    res[i] += h[j] * kout[j, i];
                res[i] += bout[i];
            }
            return res;
        }
        float get_num(float k)
        {
            int r = G.rnd.Next(12);
            float ch = 0.1f;
            float ch2 = 1.03f;
            if (G.roundcntr > 5)
            {
                r = G.rnd.Next(16);
                ch = 0.05f;
                ch2 = 1.01f;
            }
            if (r < 4) return k;
            if (r == 4) return k + ch;
            if (r == 5) return k - ch;
            if (r == 6) return k * ch2;
            return k / ch2;
        }
        public void mutate()
        {
            for (int i = 0; i < HiddenNum; i++)
            {
                for (int j = 0; j < InputNum; j++)
                {
                    kin[j, i] = get_num(kin[j, i]);
                }
                bin[i] = get_num(bin[i]);
                for (int j = 0; j < OutputNum; j++)
                {
                    kout[i, j] = get_num(kout[i, j]);
                }
            }
            for (int i = 0; i < OutputNum; i++)
                bout[i] = get_num(bout[i]);
        }

        public void writetofile(string filename)
        {
            XmlSerializer writer = new XmlSerializer(typeof(NNwrite));
            NNwrite nnw = new NNwrite();
            for (int i = 0; i < G.HIDDENNUM; i++)
            {
                nnw.bin[i] = bin[i];
            }
            for (int i = 0; i < G.OUTPUTNUM; i++)
            {
                nnw.bout[i] = bout[i];
            }
            for (int y = 0; y < G.HIDDENNUM; y++)
            {
                for (int x = 0; x < G.INPUTNUM; x++)
                {
                    nnw.kin[x + y * G.INPUTNUM] = kin[x, y];
                }
            }
            for (int y = 0; y <G.OUTPUTNUM ; y++)
            {
                for (int x = 0; x <G.HIDDENNUM ; x++)
                {
                    nnw.kout[x + y * G.OUTPUTNUM] = kout[x, y];
                }
            }
            String d = Directory.GetCurrentDirectory();
            FileStream f = File.Create(filename);
            writer.Serialize(f, nnw);
            f.Close();

        }


    }
}
