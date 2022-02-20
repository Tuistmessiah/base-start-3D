using System.Collections;
using UnityEngine;
using DebugManager;

public class ObjectDummy : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(RunEveryInterval(2f));
    }

    IEnumerator RunEveryInterval(float _time)
    {
        Console.Log("RunEveryInterval");
        yield return new WaitForSeconds(_time);
    }
}
