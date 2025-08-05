using System.Numerics;

namespace SimpleRayTracer
{
    class Plane : IObject3D
    {
        public Vector3 Origine { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Color { get; set; }
        public float Reflectivity { get; set; }

        public Plane(Vector3 point, Vector3 normal, Vector3 color, float reflectivity = 0f)
        {
            Origine = point;
            Normal = Vector3.Normalize(normal);
            Color = color;
            Reflectivity = reflectivity;
        }

        public bool Intersect(Ray ray, out float t)
        {
            float denom = Vector3.Dot(Normal, ray.Direction);
            if (Math.Abs(denom) > 1e-6)
            {
                Vector3 p0l0 = Origine - ray.Origin;
                t = Vector3.Dot(p0l0, Normal) / denom;
                return t >= 0;
            }
            t = 0;
            return false;
        }

        public Vector3 GetColor(Vector3 point)
        {
            int x = (int)Math.Floor(point.X + 10f);
            int z = (int)Math.Floor(point.Z + 10f);
            bool isWhite = (x + z) % 2 == 0;
            return isWhite ? new Vector3(1, 1, 1) : Color;
        }

        public Vector3 HitNormal(Vector3 hitPoint)
        {
            return Normal;
        }
    }
}

