using System.Numerics;

namespace SimpleRayTracer
{
	public interface IObject3D
	{
        Vector3 Center { get; set; }
        Vector3 Color { get; set; }
        float Reflectivity { get; set; }

        bool Intersect(Ray ray, out float t);
        Vector3 HitNormal(Vector3 hitPoint);
        Vector3 GetColor(Vector3 point);
    }
}

