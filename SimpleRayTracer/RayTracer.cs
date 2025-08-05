using System;
using System.Numerics;

namespace SimpleRayTracer
{
	public class RayTracer
	{
		public RayTracer()
		{
        }
        public static ColorRGB[,] ComputeRayTracing(int minX, int minY, int maxX, int maxY, Camera camera, Scene scene)
        {
            ColorRGB[,] image = new ColorRGB[maxY - minY, maxX - minX];
            // Traverses each pixel of the screen calculates a ray and
            // casts the ray on the scene and saves the result in image.
            for (int y = minY; y < maxY; y++)
            {
                for (int x = minX; x < maxX; x++)
                {
                    // Ray between the camera and the screen
                    Ray ray = camera.GetRay(x, y);
                    // resultut of the ray tracing for this pixel
                    Vector3 color = scene.Trace(ray);
                    // stock the result in the image
                    image[y - minY, x - minX] = new ColorRGB(color);
                }
            }
            return image;
        }
    }
}

