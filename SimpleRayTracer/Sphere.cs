using System.Numerics;

namespace SimpleRayTracer
{
    class Sphere : IObject3D
    {
        public Vector3 Center;
        public float Radius;
        public Vector3 Color { get; set; }
        public float Reflectivity { get; set; }
        public float Transparency { get; set; }

        public Sphere(Vector3 center, float radius, Vector3 color, float reflectivity = 0f)
        {
            Center = center;
            Radius = radius;
            Color = color;
            Reflectivity = reflectivity;
        }

 
        public bool Intersect(Ray ray, out float t)
        {
            Vector3 oc = ray.Origin - Center;
            float a = Vector3.Dot(ray.Direction, ray.Direction);
            float b = 2.0f * Vector3.Dot(oc, ray.Direction);
            float c = Vector3.Dot(oc, oc) - Radius * Radius;
            float discriminant = b * b - 4 * a * c;

            if (discriminant > 0)
            {
                float sqrtDisc = (float)Math.Sqrt(discriminant);
                float t0 = (-b - sqrtDisc) / (2.0f * a);
                float t1 = (-b + sqrtDisc) / (2.0f * a);

                t = t0 < 0 ? t1 : t0;
                return t > 0;
            }
            t = 0;
            return false;
        }

        public Vector3 GetColor(Vector3 point)
        {
            return Color;
        }


        public Vector3 HitNormal(Vector3 hitPoint)
        {
            return Vector3.Normalize(hitPoint - Center);
        }
    }
}

