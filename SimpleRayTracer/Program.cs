using System.Numerics;
using System.Diagnostics;

namespace SimpleRayTracer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Start Simple Ray Tracer");
            Stopwatch sw = Stopwatch.StartNew();  

            // Definition of the comera with its position and screen size
            Camera camera = new Camera(new Vector3(0, 1.2f, -1),1000,1000);
            Scene scene = InitScene();

            sw.Stop(); 
            Console.WriteLine($"Init Scene : {sw.ElapsedMilliseconds} ms");

            // Render the scene using multiple parallel ray tracing instances.
            sw = Stopwatch.StartNew();

            ColorRGB[,] image = RayTracer.ComputeRayTracingParallel(0, 0, camera.Width, camera.Height, camera, scene);

            sw.Stop();
            Console.WriteLine($"Compute // raytracing : {sw.ElapsedMilliseconds} ms");

            // Write processing image in a file
            WriteImageInFile(image , camera.Width, camera.Height, "output.ppm");
            Console.WriteLine($"Stop Simple Ray Tracer");
        }

        static Scene InitScene()
        {
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

            return scene;
        }

        static ColorRGB[,] MergeImages(int maxTileX, int maxTileY,int maxX,int maxY,Camera camera, ColorRGB[][,] images)
        {
            ColorRGB[,] completeImage = new ColorRGB[camera.Width, camera.Height];
            int currentTile = 0;
            for (int tilesX = 0; tilesX < maxTileX; tilesX++)
            {
                for (int tilesY = 0; tilesY < maxTileY; tilesY++)
                {
                    for (int y = 0; y < maxX; y++)
                    {
                        for (int x = 0; x < maxX; x++)
                        {
                            completeImage[x + tilesX * maxX, y + tilesY * maxY] = images[currentTile][x, y];
                        }
                    }
                    currentTile++;
                }
            }

            return completeImage;
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
