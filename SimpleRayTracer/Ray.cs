using System.Numerics;

namespace SimpleRayTracer
{
    public class Ray
    {
        public Vector3 Origin;
        public Vector3 Direction;
        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = Vector3.Normalize(direction);
        }
    }
}

