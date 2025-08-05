using System.Numerics;


namespace SimpleRayTracer
{
    class Scene
    {
        private Vector3 BackgroundColor = new Vector3(0.2f, 0.7f, 1.0f);
        private int MaxDepth = 3;
        private float ShadowCoef = 0.05f;

        public ICollection<IObject3D> Objets3D { get; set; } = new List<IObject3D>();
        public ICollection<Vector3> Lights { get; set; } = new List<Vector3>();

        public bool IsInShadow(Ray ray)
        {
            // Checks if the ray is in the shadow of an object

            bool inShadow = false;
            foreach (var object3d in Objets3D)
            {
                if (object3d.Intersect(ray, out float shadowT))
                {
                    inShadow = true;
                    break;
                }
            }
            return inShadow;
        }

        public Vector3 ComputeLocalColor(Vector3 hitPoint, Vector3 hitNormal, Vector3 hitColor)
        {
            // Calculates the color of the local pixel taking into account both
            // highlights and shadows

            Vector3 localColor = Vector3.Zero;
            foreach (var light in Lights)
            {
                Vector3 toLight = Vector3.Normalize(light - hitPoint);
                Ray shadowRay = new Ray(hitPoint + hitNormal * 0.001f, toLight);

                if (!IsInShadow(shadowRay))
                {
                    float diffuse = Math.Max(0.0f, Vector3.Dot(hitNormal, toLight));
                    localColor += hitColor * diffuse;
                }
                else
                {
                    localColor += hitColor * ShadowCoef;
                }
            }

            return Vector3.Clamp(localColor, Vector3.Zero, Vector3.One);
        }


        public Vector3 Trace(Ray ray, int depth = 0)
        {
            // Ray tracing for a specific ray

            if (depth > MaxDepth) return BackgroundColor;

            float closestT = float.MaxValue;
            Vector3 hitColor = BackgroundColor;
            Vector3 hitPoint = Vector3.Zero;
            Vector3 hitNormal = Vector3.Zero;
            float hitReflectivity = 0.0f;

            // 1 - Calculates the intersection points with all objects and
            //     keeps the closest ones.
            foreach (var object3D in Objets3D)
            {
                if (object3D.Intersect(ray, out float t) && t < closestT)
                {
                    closestT = t;
                    hitPoint = ray.Origin + t * ray.Direction;
                    hitNormal = object3D.HitNormal(hitPoint);
                    hitColor = object3D.GetColor(hitPoint);
                    hitReflectivity = object3D.Reflectivity;
                }
            }

            // 2 - Calculates the color of the resulting pixel, taking into
            //     account the object's color, shadows, and highlights.
            if (closestT == float.MaxValue) return BackgroundColor;
            Vector3 localColor = ComputeLocalColor(hitPoint, hitNormal, hitColor);

            // 3 - In the case of a reflection, re - casts a new ray.
            if (hitReflectivity > 0.0f)
            {
                Vector3 reflectedDir = ray.Direction - 2.0f * Vector3.Dot(ray.Direction, hitNormal) * hitNormal;

                Ray reflectedRay = new Ray(hitPoint + hitNormal * 0.001f, reflectedDir);
                Vector3 reflectedColor = Trace(reflectedRay, depth + 1);
                localColor = Vector3.Lerp(localColor, reflectedColor, hitReflectivity);
            }

            return Vector3.Clamp(localColor, Vector3.Zero, Vector3.One);
        }
    }
}

