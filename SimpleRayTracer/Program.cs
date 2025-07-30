using System.Drawing;
using System.Numerics;
using System.Diagnostics;

namespace SimpleRayTracer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();  

            // Definition of the comera with its position and screen size
            Camera camera = new Camera(new Vector3(0, 1.2f, -1),1000,1000);
            Scene scene = InitScene();

            sw.Stop(); 
            Console.WriteLine($"Init Scene : {sw.ElapsedMilliseconds} ms");

            // Render the scene in a single ray tracing passe
            sw = Stopwatch.StartNew();
            ColorRGB[,] image = ComputeRayTracing(0,0,camera.Width,camera.Height,camera, scene);
            sw.Stop();
            Console.WriteLine($"Compute raytracing : {sw.ElapsedMilliseconds} ms");

            // Render the scene using multiple parallel ray tracing instances.
            sw = Stopwatch.StartNew();

            // A list of tasks that enables parallel ray tracing on different parts of the image.
            List<Task<ColorRGB[,]>> tasks = new List<Task<ColorRGB[,]>>();

            // Tile properties (16) 
            int maxTileX = 4;
            int maxTileY = 4;
            int maxX = camera.Width / maxTileX;
            int maxY = camera.Height / maxTileY;

            // Creating computation tasks based on each tile's characteristics.
            for (int tileY = 0; tileY < maxTileY; tileY++)
            {
                for (int tileX = 0; tileX < maxTileX; tileX++)
                {
                    int x1 = tileX * maxX;
                    int y1 = tileY * maxY;
                    int x2 = (tileX + 1) * maxX;
                    int y2 = (tileY + 1) * maxY;
                    tasks.Add(Task.Run(() => ComputeRayTracing( x1, y1, x2, y2, camera, scene)));
                }
            }
            // Start the computations on all tiles and wait for their completion
            var resultats = await Task.WhenAll(tasks);

            // Merge the different images into one
            ColorRGB[,] completeImage = MergeImages(maxTileX, maxTileY, maxX, maxY, camera, resultats);
            sw.Stop();
            Console.WriteLine($"Compute // raytracing : {sw.ElapsedMilliseconds} ms");

            // Write image in a file
            WriteImageInFile(image, camera.Width, camera.Height, "output.ppm");
            // Write image in a file
            WriteImageInFile(completeImage , camera.Width, camera.Height, "outputP.ppm");
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

        static ColorRGB[,] ComputeRayTracing(int minX, int minY, int maxX, int maxY, Camera camera, Scene scene)
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
