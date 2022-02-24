using UnityEngine;
using DebugManager;

/** Example drawing debug
 * @usage Assign this script to any instanced game object in scene to visualize examples
 */
public class Test : MonoBehaviour {
    void Start() {
        Lines.DrawRay(new Vector3(0,2,0), new Vector3(0,0,10));
        Lines.DrawRay(new Vector3(0,2,0), new Vector3(0,0,10), 30f);
        Lines.DrawRayCast(new Vector3(5,2,5), new Vector3(-1,-1,-1), 5);

        Lines.DrawRayVert(new Vector3(10,10,10), -1, 10, Color.blue);
        Lines.DrawRayVert(new Vector3(20,10,10), -1, 10, Color.cyan);
        Lines.DrawRayVert(new Vector3(20,10,20), -1, 10, Color.green);

        Spheres.Draw(new Vector3(5,5,5));
        Spheres.Draw(new Vector3(-5,5,5), 3f, Color.magenta);

        Prisms.DrawCube(new Vector3(10,5,5), 2f, Color.black);
        Prisms.Draw3DRange(new Vector3(10,10,5), 5f, Color.yellow);
        Prisms.DrawCubeWireframe(new Vector3(-10,10,-5), 5f, Color.green);

        string[] names = {"Matt", "Joanne", "Robert"};
        Console.Log(names, "2");
    }
}
