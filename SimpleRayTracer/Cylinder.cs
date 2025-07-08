// Ajout d'un cylindre au moteur de ray tracing existant
using System;
using System.Numerics;

namespace SimpleRayTracer
{
    class Cylinder : IObject3D
    {
        public Vector3 Center;
        public float Radius;
        public float Height;
        public Vector3 Axis;
        public Vector3 Color { get; set; }
        public float Reflectivity { get; set; }

        public Cylinder(Vector3 center, float radius, float height, Vector3 axis, Vector3 color, float reflectivity = 0f)
        {
            Center = center;
            Radius = radius;
            Height = height;
            Axis = Vector3.Normalize(axis);
            Color = color;
            Reflectivity = reflectivity;
        }

        public bool Intersect(Ray ray, out float t)
        {
            // Aligner le cylindre sur Y
            Vector3 d = ray.Direction;
            Vector3 o = ray.Origin - Center;

            // Cylindre aligné Y
            float a = d.X * d.X + d.Z * d.Z;
            float b = 2 * (o.X * d.X + o.Z * d.Z);
            float c = o.X * o.X + o.Z * o.Z - Radius * Radius;

            float discriminant = b * b - 4 * a * c;
            t = 0;
            if (discriminant < 0) return false;

            float sqrtDisc = (float)Math.Sqrt(discriminant);
            float t0 = (-b - sqrtDisc) / (2 * a);
            float t1 = (-b + sqrtDisc) / (2 * a);

            // Choisir plus proche intersection valide
            float y0 = o.Y + t0 * d.Y;
            float y1 = o.Y + t1 * d.Y;
            if (t0 > 0 && y0 >= 0 && y0 <= Height) { t = t0; return true; }
            if (t1 > 0 && y1 >= 0 && y1 <= Height) { t = t1; return true; }
            return false;
        }

        public Vector3 GetNormal(Vector3 point)
        {
            Vector3 p = point - Center;
            p.Y = 0;
            return Vector3.Normalize(p);
        }

        public Vector3 HitNormal(Vector3 hitPoint)
        {
            return Vector3.Normalize(hitPoint - Center);
        }

        public Vector3 GetColor(Vector3 point)
        {
            return Color;
        }
    }
}

