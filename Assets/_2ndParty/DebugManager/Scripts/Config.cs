using System.Collections;
using UnityEngine;

namespace DebugManager {

    /** Config singleton consumed by static scripts. 
     * Values can be changed in the DebugManager prefab object in the scene.
     */
    // TODO: Add mechanism to allow objects to appear and disappear on a time period (10s on, 5s off, for example)
    public class Config : MonoBehaviour {

        public static Config inst;
        public bool isOn = true;
        public float time = 20f;
        public Color color = Color.red;
        public float size = 1f;

        private void Awake() {
            if (inst == null) inst = this;
            else if (inst != this) Destroy(this);
        }

        // * Methods

        /** Destroy a collider object to avoid interference with scene*/
        public void DestroyImmediate(Collider debugGameObject) {
            Destroy(debugGameObject);
        }

        public void Destroy(GameObject debugGameObject) {
            StartCoroutine(DestroyAfter(debugGameObject));
        }

        private IEnumerator DestroyAfter(GameObject _gameObject) {
            yield return new WaitForSeconds(this.time);
            Destroy(_gameObject);
        }
    }
}