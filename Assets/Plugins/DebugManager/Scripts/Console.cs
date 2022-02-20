using UnityEngine;

namespace DebugManager {

    public class Console : MonoBehaviour {
        public static void Log(string message) {
            if (Config.inst.isOn) {
                Debug.Log($"-> {message}");
            }
        }

        /** Right a bunch of values with an identifier */
        public static void Log(string[] messages, string id = "") {
            if (Config.inst.isOn) {
                Debug.Log($"-> :{id}: {string.Join(", ", messages)}");
            }
        }

    }

}