using UnityEngine;

namespace DebugManager {
    // TODO: Create more overloads
    public class Spheres {
        public static void Draw(Vector3 center, float radius, Color color) {
            if (Config.inst.isOn) {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Renderer render = sphere.GetComponent<Renderer>();
                Material material = sphere.GetComponent<Material>();

                sphere.transform.position = center;
                sphere.transform.localScale = new Vector3(radius, radius, radius);
                sphere.layer = 2; // Ignore Raycast

                render.material = new Material(Shader.Find("Specular"));
                render.material.color = color;

                Collider collider = sphere.GetComponent<Collider>();
                Config.inst.DestroyImmediate(collider);

                Config.inst.Destroy(sphere);
            }
        }

        /** Overloads */

        public static void Draw(Vector3 center)
        { if (Config.inst.isOn) Draw(center, Config.inst.size, Config.inst.color); }

        public static void Draw(Vector3 center, float radius)
        { if (Config.inst.isOn) Draw(center, radius, Config.inst.color); }

        public static void Draw(Vector3 center, Color color)
        { if (Config.inst.isOn) Draw(center, Config.inst.size, color); }
    }
}