using System.Numerics;

namespace SimpleRayTracer
{
	public struct ColorRGB
	{
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public ColorRGB()
        {
            R = G = B = 0;
        }

        public ColorRGB(byte r, byte g, byte b)
		{
			R = r;
			G = g;
			B = b;
		}

        public ColorRGB(float r, float g, float b)
        {
            R = (byte)(255 * r);
            G = (byte)(255 * g);
            B = (byte)(255 * b);
        }

        public ColorRGB(Vector3 c) : this(c.X,c.Y,c.Z)
        {
        }
    }
}

