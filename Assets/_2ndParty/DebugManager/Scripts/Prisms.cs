using UnityEngine;

// TODO: Create more overloads
// TODO: Have more primitives
// GameObject plane  = GameObject.CreatePrimitive(PrimitiveType.Plane);
// GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
// GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
namespace DebugManager {

    public class Prisms {

        #region Cubes
        public static void DrawCube(Vector3 center, float side, Color color) {
            if (Config.inst.isOn) {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Renderer render = cube.GetComponent<Renderer>();
                Material material = cube.GetComponent<Material>();

                cube.transform.position = center;
                cube.transform.localScale = new Vector3(side, side, side);
                cube.layer = 2; // Ignore Raycast

                render.material = new Material(Shader.Find("Specular"));
                render.material.color = color;

                Collider collider = cube.GetComponent<Collider>();
                Config.inst.DestroyImmediate(collider);

                Config.inst.Destroy(cube);
            }
        }
        #endregion

        #region Wireframes
        public static void Draw3DRange(Vector3 center, float radius, Color color) {
            if (Config.inst.isOn) {
                Lines.DrawLine(center + new Vector3(-radius, 0, 0), center + new Vector3(radius, 0, 0), color);
                Spheres.Draw(center + new Vector3(-radius, 0, 0), radius / 10, color);
                Spheres.Draw(center + new Vector3(radius, 0, 0), radius / 10, color);
                Lines.DrawLine(center + new Vector3(0, -radius, 0), center + new Vector3(0, radius, 0), color);
                Spheres.Draw(center + new Vector3(0, -radius, 0), radius / 10, color);
                Spheres.Draw(center + new Vector3(0, radius, 0), radius / 10, color);
                Lines.DrawLine(center + new Vector3(0, 0, -radius), center + new Vector3(0, 0, radius), color);
                Spheres.Draw(center + new Vector3(0, 0, -radius), radius / 10, color);
                Spheres.Draw(center + new Vector3(0, 0, radius), radius / 10, color);
            }
        }

        public static void DrawCubeWireframe(Vector3 center, float radius, Color color) {
            if (Config.inst.isOn) {
                // TODO: Make method to draw quads and call it here instead
                // y = -1
                Lines.DrawLine(center + new Vector3(-radius, -radius, -radius), center + new Vector3(radius, -radius, -radius), color);
                Lines.DrawLine(center + new Vector3(-radius, -radius, -radius), center + new Vector3(-radius, -radius, radius), color);
                Lines.DrawLine(center + new Vector3(radius, -radius, -radius), center + new Vector3(radius, -radius, radius), color);
                Lines.DrawLine(center + new Vector3(-radius, -radius, radius), center + new Vector3(radius, -radius, radius), color);
                Spheres.Draw(center + new Vector3(-radius, -radius, -radius), radius / 10, color);
                Spheres.Draw(center + new Vector3(radius, -radius, -radius), radius / 10, color);
                Spheres.Draw(center + new Vector3(-radius, -radius, radius), radius / 10, color);
                Spheres.Draw(center + new Vector3(radius, -radius, radius), radius / 10, color);

                // y = 1
                Lines.DrawLine(center + new Vector3(radius, radius, radius), center + new Vector3(-radius, radius, radius), color);
                Lines.DrawLine(center + new Vector3(radius, radius, radius), center + new Vector3(radius, radius, -radius), color);
                Lines.DrawLine(center + new Vector3(radius, radius, -radius), center + new Vector3(-radius, radius, -radius), color);
                Lines.DrawLine(center + new Vector3(-radius, radius, radius), center + new Vector3(-radius, radius, -radius), color);
                Spheres.Draw(center + new Vector3(radius, radius, radius), radius / 10, color);
                Spheres.Draw(center + new Vector3(-radius, radius, radius), radius / 10, color);
                Spheres.Draw(center + new Vector3(-radius, radius, -radius), radius / 10, color);
                Spheres.Draw(center + new Vector3(radius, radius, -radius), radius / 10, color);

                // z = 1
                Lines.DrawLine(center + new Vector3(radius, radius, radius), center + new Vector3(radius, -radius, radius), color);
                Lines.DrawLine(center + new Vector3(-radius, radius, radius), center + new Vector3(-radius, -radius, radius), color);

                // z = -1
                Lines.DrawLine(center + new Vector3(radius, radius, -radius), center + new Vector3(radius, -radius, -radius), color);
                Lines.DrawLine(center + new Vector3(-radius, radius, -radius), center + new Vector3(-radius, -radius, -radius), color);
            }
        }
        #endregion
    }
}