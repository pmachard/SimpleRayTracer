using System.Drawing;
using System.Numerics;
using System.Diagnostics;

namespace SimpleRayTracer
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();  

            // Definition of the comera with its position and screen size
            Camera camera = new Camera(new Vector3(0, 1.2f, -1),1000,1000);

            // Definition of the scene with the objects that compose it
            // (spheres and plane) as well as the lights that illuminate the scene.
            Scene scene = new Scene();
            scene.Objets3D.Add(new Sphere(new Vector3(0, 0, 3), 1, new Vector3(1, 0, 0), 0.3f));
            scene.Objets3D.Add(new Sphere(new Vector3(-1, 0, 4), 1, new Vector3(0, 1, 0), 0.2f));
            scene.Objets3D.Add(new Sphere(new Vector3(1, 0, 4), 1, new Vector3(0, 0, 1), 0.5f));
            scene.Objets3D.Add(new Sphere(new Vector3(0, 1.75f, 3.5f), 1, new Vector3(1, 1, 1), 0.5f));
            scene.Objets3D.Add(new Plane(new Vector3(0, -1, 0), new Vector3(0, 1, 0), new Vector3(0.1f, 0.1f, 0.1f), 0.2f));

            scene.Lights.Add(new Vector3(5, 5, -10));
            scene.Lights.Add(new Vector3(-5, 5, -8));
            scene.Lights.Add(new Vector3(5, 5, -6));

            sw.Stop(); 
            Console.WriteLine($"Init Scene : {sw.ElapsedMilliseconds} ms");

            sw = Stopwatch.StartNew();
            // Compute the raytracing of the scene
            ColorRGB[,] image = ComputeRayTracing(camera, scene);
            sw.Stop();
            Console.WriteLine($"Compute raytracing : {sw.ElapsedMilliseconds} ms");

            sw = Stopwatch.StartNew();
            // Write image in a file
            WriteImageInFile(image, camera.Width, camera.Height, "output.ppm");
            sw.Stop();
            Console.WriteLine($"Write image in file : {sw.ElapsedMilliseconds} ms");
        }

        static ColorRGB[,] ComputeRayTracing(Camera camera, Scene scene)
        {
            ColorRGB[,] image = new ColorRGB[camera.Width,camera.Height];
            // Traverses each pixel of the screen calculates a ray and
            // casts the ray on the scene and saves the result in image.
            for (int y = 0; y < camera.Height; y++)
            {
                for (int x = 0; x < camera.Width; x++)
                {
                    // Ray between the camera and the screen
                    Ray ray = camera.GetRay(x, y);
                    // resultut of the ray tracing for this pixel
                    Vector3 color = scene.Trace(ray);
                    // stock the result in the image
                    image[y, x] = new ColorRGB(color);
                }
            }
            return image;
        }

        static void WriteImageInFile(ColorRGB[,] image, int w, int h, String fileName)
        {
            // Create output file is written to the image header
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("P3");
                writer.WriteLine($"{w} {h}");
                writer.WriteLine("255");

                // write each pixel of the image
                foreach (ColorRGB c in image)
                {
                    writer.WriteLine($"{c.R} {c.G} {c.B}");
                }
            }
        }
    }
}
