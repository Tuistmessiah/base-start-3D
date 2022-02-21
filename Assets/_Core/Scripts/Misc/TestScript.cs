using UnityEngine;
using System.Collections;
using PoolerManager;
using DebugManager;

public class TestScript : MonoBehaviour
{
    float xPos = 12f;
    int iteration = 0;

    PoolItem a, b, c, d, e, f, g;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2);

        if(this.iteration == 0) a = PoolStore.Instantiate("Cube", new Vector3(this.xPos, 0, 0), Quaternion.identity);
        if(this.iteration == 1) b = PoolStore.Instantiate("Cube", new Vector3(this.xPos, 0, 0), Quaternion.identity);
        if(this.iteration == 2) c = PoolStore.Instantiate("Cube", new Vector3(this.xPos, 0, 0), Quaternion.identity);
        if(this.iteration == 3) d = PoolStore.Instantiate("Cube", new Vector3(this.xPos, 0, 0), Quaternion.identity);
        if(this.iteration == 4) e = PoolStore.Instantiate("Cube", new Vector3(this.xPos, 0, 0), Quaternion.identity);
        if(this.iteration == 5) f = PoolStore.Instantiate("Cube", new Vector3(this.xPos, 0, 0), Quaternion.identity);
        if(this.iteration == 6) g = PoolStore.Instantiate("Cube", new Vector3(this.xPos, 0, 0), Quaternion.identity);
        if(this.iteration == 7) PoolStore.Destroy(a);
        if(this.iteration == 8) PoolStore.Destroy(c);
        if(this.iteration == 9) b = PoolStore.Instantiate("Cube", new Vector3(this.xPos, 0, 0), Quaternion.identity);
        if(this.iteration == 10) PoolStore.Destroy(f);
        if(this.iteration == 11) PoolStore.Destroy(d);

        if(this.iteration <= 11) {
            this.xPos += 2;
            this.iteration++;
            StartCoroutine(Spawn());
        }
    }

    void Update()
    {
        
    }
}
