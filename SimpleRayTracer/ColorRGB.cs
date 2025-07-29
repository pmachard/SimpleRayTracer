using System;

using System.Numerics;


namespace SimpleRayTracer
{
	public struct ColorRGB
	{
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public ColorRGB()
        {
            R = G = B = 0;
        }

        public ColorRGB(int r, int g, int b)
		{
			R = r;
			G = g;
			B = b;
		}

        public ColorRGB(float r, float g, float b)
        {
            R = (int)(255 * r);
            G = (int)(255 * g);
            B = (int)(255 * b);
        }

        public ColorRGB(Vector3 c) : this(c.X,c.Y,c.Z)
        {
        }
    }
}

