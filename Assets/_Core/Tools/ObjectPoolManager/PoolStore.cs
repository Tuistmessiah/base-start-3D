using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugManager;

namespace PoolerManager {

    [System.Serializable]
    public class PoolSetup {
        public string tag;
        public GameObject prefab;
        public int size;
        public int limit;
    }

    public class PoolItem {
        public string name;
        public GameObject instance;
        public bool isDestroyed = false;
        public PoolItem(string _name, GameObject _instance) {
            name = _name;
            instance = _instance;
        }
    }

    /**
     * Static "PoolStore" singleton will emulate the Object.Instantiate/destroy methods, but using the pooling technique
     * @usage Import and use method "PoolStore.Instantiate(<tag>, ...)" with corresponding tag. Handle PoolItem in your code instead of simpyl gameobjects
     * @obs Tags that do not exist will default to normal Object.Instantiate(...)
     */
    public class PoolStore : MonoBehaviour 
    {
        public static PoolStore singleton;
        public static Dictionary<string, List<PoolItem>> poolObjects;
        public static List<PoolSetup> pools;

        [SerializeField] private List<PoolSetup> poolsEditorSetup;

        void Awake() {
            if(!singleton) singleton = this;
            pools = poolsEditorSetup;
        }

        void Start() {
            poolObjects = new Dictionary<string, List<PoolItem>>();

            foreach (PoolSetup pool in pools) {
                List<PoolItem> objectPool = new List<PoolItem>();

                for (int i = 0; i < pool.size; i++) {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.transform.parent = this.gameObject.transform;
                    obj.SetActive(false);
                    objectPool.Add(new PoolItem(tag + "-" + i, obj));
                }

                poolObjects.Add(pool.tag, objectPool);
            }
        }

        /* Instantiate an object directly and create a new pool with automatic tag generation */
        public static PoolItem Instantiate(GameObject newPrefab, Vector3 position, Quaternion rotation) {
            PoolSetup newPool = new PoolSetup();
            newPool.tag = newPrefab.name + "-" + ToolUtils.RandomString(5);
            newPool.prefab = newPrefab;
            newPool.size = 1;

            List<PoolItem> objectPool = new List<PoolItem>();
            GameObject newObj = Object.Instantiate(newPrefab, position, rotation);
            newObj.transform.parent = singleton.gameObject.transform;
            newObj.SetActive(true);
            PoolItem newPoolItem = new PoolItem(newPool.tag + "-" + ToolUtils.RandomString(3), newObj);
            objectPool.Add(newPoolItem);

            poolObjects.Add(newPool.tag, objectPool);

            return newPoolItem;
        }

        /* "Instantiate" by using pool setup. */
        public static PoolItem Instantiate(string tag, Vector3 position, Quaternion rotation) {
            if(!poolObjects.ContainsKey(tag)) {
                Console.Log("Pool with tag " + tag + " does not exist!");
                return null;
            }

            PoolItem objPeeked = poolObjects[tag][0];
            PoolItem objToSpawn = null;

            if(objPeeked.instance.activeInHierarchy) {
                // Increase List size if all objects are being used
                PoolSetup pool = GetPool(tag);
                if(pool == null) {
                    Console.Log("Pool setup with tag " + tag + " does not exist!");
                    return null;
                }
                if(pool.limit != 0 && poolObjects[tag].Count >= pool.limit) {
                    Console.Log("Pool of objects with tag " + tag + " has reached max limit of " + pool.limit + "!");
                    return null;
                }
                GameObject newInstance = Object.Instantiate(pool.prefab, position, rotation);
                newInstance.transform.parent = singleton.gameObject.transform;
                newInstance.SetActive(true);
                objToSpawn = new PoolItem(tag + "-" + ToolUtils.RandomString(3), newInstance);
            } else {
                // Use existing inactive object
                objToSpawn = objPeeked;
                poolObjects[tag].RemoveAt(0);
                objToSpawn.isDestroyed = false;
                objToSpawn.instance.SetActive(true);
                objToSpawn.instance.transform.position = position;
                objToSpawn.instance.transform.rotation = rotation;
            }
            Console.Log(objToSpawn.name);

            poolObjects[tag].Add(objToSpawn);
            return objToSpawn;
        }

        public static void Destroy(GameObject obj) {
            if(obj == null) return;
            obj.SetActive(false);
            IPooledObject pooledObject = obj.GetComponent<IPooledObject>();
            if(pooledObject != null) pooledObject.OnDisable();
        }

        public static void Destroy(PoolItem item) {
            if(item == null) return;
            item.instance.SetActive(false);
            item.isDestroyed = true;
            IPooledObject pooledObject = item.instance.GetComponent<IPooledObject>();
            if(pooledObject != null) pooledObject.OnDisable();
        }

        private static PoolSetup GetPool(string tag) {
            for (int i = 0; i < pools.Count; i++) {
                if (pools[i].tag == tag) return pools[i];
            }
            return null;
        }
    }
}



