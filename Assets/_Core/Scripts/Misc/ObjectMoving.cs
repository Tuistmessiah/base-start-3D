using System.Collections;
using UnityEngine;
using DebugManager;

public class ObjectMoving : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(RunEveryInterval(4f));
    }

    IEnumerator RunEveryInterval(float _time)
    {
        Console.Log("RunEveryInterval");
        yield return new WaitForSeconds(_time);
    }
}