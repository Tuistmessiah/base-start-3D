using System.Collections;
using UnityEngine;
using VfxManager;

public class ObjectDummy : MonoBehaviour {
    void Start() {
        StartCoroutine(RunEveryInterval(2f));
    }

    IEnumerator RunEveryInterval(float _time) {
        yield return new WaitForSeconds(_time);

        VfxTrigger.TriggerCombo("BlizzardSurface", transform);
        StartCoroutine(RunEveryInterval(2f));
    }
}
