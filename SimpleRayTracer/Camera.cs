using System.Numerics;

namespace SimpleRayTracer
{
	public class Camera
	{
		private float PseudoFocal = 0.75f;

		public Vector3 Origin { get; }
        public int Width { get; }
		public int Height { get; }

		public Camera(Vector3 origin, int width, int height)
		{
			Origin = origin;
			Width = width;
			Height = height;
		}

		public Ray GetRay(int x, int y)
		{
            // Retrieve the ray that passes through a specific pixel on the screen
            float u = (x - Width / 2f) / Width;
            float v = (Height / 2f - y) / Height;
            Vector3 dir = Vector3.Normalize(new Vector3(u, v, PseudoFocal));
			return new Ray(Origin, dir);
		}
    }
}

