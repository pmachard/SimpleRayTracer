using System.Drawing;
using System.Numerics;

namespace SimpleRayTracer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Definition of the comera with its position and screen size
            Camera camera = new Camera(new Vector3(0, 1.2f, -1),1000,1000);

            // Definition of the scene with the objects that compose it
            // (spheres and plane) as well as the lights that illuminate the scene.
            Scene scene = new Scene();
            scene.Objets3D.Add(new Sphere(new Vector3(0, 0, 3), 1, new Vector3(1, 0, 0), 0.3f));
            scene.Objets3D.Add(new Sphere(new Vector3(-1, 0, 4), 1, new Vector3(0, 1, 0), 0.2f));
            scene.Objets3D.Add(new Sphere(new Vector3(1, 0, 4), 1, new Vector3(0, 0, 1), 0.5f));
            scene.Objets3D.Add(new Sphere(new Vector3(0, 1.8f, 3), 1, new Vector3(1, 1, 1), 0.5f));
            scene.Objets3D.Add(new Plane(new Vector3(0, -1, 0), new Vector3(0, 1, 0), new Vector3(0.1f, 0.1f, 0.1f), 0.2f));

            scene.Lights.Add(new Vector3(5, 5, -10));
            scene.Lights.Add(new Vector3(-5, 5, -10));
            scene.Lights.Add(new Vector3(5, -5, -10));

            // Create output file is written to the image header
            using (StreamWriter writer = new StreamWriter("output.ppm"))
            {
                writer.WriteLine("P3");
                writer.WriteLine($"{camera.Width} {camera.Height}");
                writer.WriteLine("255");

                int r,g,b;
                // Traverses each pixel of the screen calculates a ray and
                // casts the ray on the scene and saves the result in the file.
                for (int y = 0; y < camera.Height; y++)
                {
                    for (int x = 0; x < camera.Width; x++)
                    {
                        Vector3 color = scene.Trace(camera.GetRay(x,y));

                        r = (int)(255 * color.X);
                        g = (int)(255 * color.Y);
                        b = (int)(255 * color.Z);
                        writer.WriteLine($"{r} {g} {b}");
                    }
                }
            }

            Console.WriteLine("Image generated: output.ppm");
        }
    }
}
