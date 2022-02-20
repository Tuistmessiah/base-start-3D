using UnityEngine;

namespace DebugManager {
    public class Lines {

        /** Simulate raycasts */
        #region RayCast Overloads

        /** Start "white" -> end "black" */
        public static void DrawRayCast(Vector3 origin, Vector3 direction, float maxDistance) { 
            if (Config.inst.isOn) {
                Debug.DrawRay(origin, direction.normalized * maxDistance, Config.inst.color, Config.inst.time);
                Spheres.Draw(origin, 1f, Color.white);
                Spheres.Draw(origin + direction.normalized * maxDistance, 0.2f, Color.black);
            }
        }

        #endregion 

        /** Draw verical ray from "start" in "yDir" (-1, 1) with "height" */
        #region DrawRayVert Overloads

        public static void DrawRayVert(Vector3 start, float yDir, float height) 
        { if (Config.inst.isOn) Debug.DrawRay(start, new Vector3(0f, yDir * height, 0f), Config.inst.color, Config.inst.time); }

        public static void DrawRayVert(Vector3 start, float yDir, float height, Color color) 
        { if (Config.inst.isOn) Debug.DrawRay(start, new Vector3(0f, yDir * height, 0f), color, Config.inst.time); }

        public static void DrawRayVert(Vector3 start, float yDir, float height, float time) 
        { if (Config.inst.isOn) Debug.DrawRay(start, new Vector3(0f, yDir * height, 0f), Config.inst.color, time); }

        public static void DrawRayVert(Vector3 start, float yDir, float height, bool depthTest) 
        { if (Config.inst.isOn)  Debug.DrawRay(start, new Vector3(0f, yDir * height, 0f), Config.inst.color, Config.inst.time, depthTest); }

        public static void DrawRayVert(Vector3 start, float yDir, float height, Color color, float time) 
        { if (Config.inst.isOn) Debug.DrawRay(start, new Vector3(0f, yDir * height, 0f), color, time); }

        public static void DrawRayVert(Vector3 start, float yDir, float height, Color color, bool depthTest) 
        { if (Config.inst.isOn) Debug.DrawRay(start, new Vector3(0f, yDir * height, 0f), color, Config.inst.time, depthTest); }

        public static void DrawRayVert(Vector3 start, float yDir, float height, float time, bool depthTest) 
        { if (Config.inst.isOn) Debug.DrawRay(start, new Vector3(0f, yDir * height, 0f), Config.inst.color, time, depthTest); }

        public static void DrawRayVert(Vector3 start, float yDir, float height, Color color, float time, bool depthTest) 
        { if (Config.inst.isOn) Debug.DrawRay(start, new Vector3(0f, yDir * height, 0f), color, time, depthTest); }

        #endregion

        /** Draw Debug.Line */
        #region DrawLine Overloads

        public static void DrawLine(Vector3 start, Vector3 end) 
        { if (Config.inst.isOn) Debug.DrawLine(start, end, Config.inst.color, Config.inst.time); }

        public static void DrawLine(Vector3 start, Vector3 end, Color color) 
        { if (Config.inst.isOn) Debug.DrawLine(start, end, color, Config.inst.time); }

        public static void DrawLine(Vector3 start, Vector3 end, float time) 
        { if (Config.inst.isOn) Debug.DrawLine(start, end, Config.inst.color, time); }

        public static void DrawLine(Vector3 start, Vector3 end, bool depthTest) 
        { if (Config.inst.isOn) Debug.DrawLine(start, end, Config.inst.color, Config.inst.time, depthTest); }

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float time) 
        { if (Config.inst.isOn) Debug.DrawLine(start, end, color, time); }

        public static void DrawLine(Vector3 start, Vector3 end, Color color, bool depthTest) 
        { if (Config.inst.isOn) Debug.DrawLine(start, end, color, Config.inst.time, depthTest); }

        public static void DrawLine(Vector3 start, Vector3 end, float time, bool depthTest) 
        { if (Config.inst.isOn) Debug.DrawLine(start, end, Config.inst.color, time, depthTest); }

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float time, bool depthTest) 
        { if (Config.inst.isOn) Debug.DrawLine(start, end, color, time, depthTest); }

        #endregion

        /** Draw Debug.Ray */
        #region DrawRay Overloads

        public static void DrawRay(Vector3 start, Vector3 dir) 
        { if (Config.inst.isOn) Debug.DrawRay(start, dir, Config.inst.color, Config.inst.time); }

        public static void DrawRay(Vector3 start, Vector3 dir, Color color) 
        { if (Config.inst.isOn) Debug.DrawRay(start, dir, color, Config.inst.time); }

        public static void DrawRay(Vector3 start, Vector3 dir, float time) 
        { if (Config.inst.isOn) Debug.DrawRay(start, dir, Config.inst.color, time); }

        public static void DrawRay(Vector3 start, Vector3 dir, bool depthTest) 
        { if (Config.inst.isOn) Debug.DrawRay(start, dir, Config.inst.color, Config.inst.time, depthTest); }

        public static void DrawRay(Vector3 start, Vector3 dir, Color color, float time) 
        { if (Config.inst.isOn) Debug.DrawRay(start, dir, color, time); }

        public static void DrawRay(Vector3 start, Vector3 dir, Color color, bool depthTest) 
        { if (Config.inst.isOn) Debug.DrawRay(start, dir, color, Config.inst.time, depthTest); }

        public static void DrawRay(Vector3 start, Vector3 dir, float time, bool depthTest) 
        { if (Config.inst.isOn) Debug.DrawRay(start, dir, Config.inst.color, time, depthTest); }

        public static void DrawRay(Vector3 start, Vector3 dir, Color color, float time, bool depthTest) 
        { if (Config.inst.isOn) Debug.DrawRay(start, dir, color, time, depthTest); }

        #endregion

    }
}