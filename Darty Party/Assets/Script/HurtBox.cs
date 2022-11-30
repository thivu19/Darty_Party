using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public int point;
    public bool triggered = false;
    public bool ready = true;
    [SerializeField] private Dart1 dart; 

    private void OnTriggerEnter(Collider other)
    {
        triggered = true;
        Debug.Log("TRIGGERED");
        if (ready)
        {
            if (triggered == true)
            {
                StartCoroutine(Waiting());
                ready = false;
            }
        }
    }

    private void triggeredAction()
    {
        dart.pointChanger(point);
        StartCoroutine(Waiting());
    }

    IEnumerator Waiting()
    {
        dart.pointChanger(point);
        yield return new WaitForSeconds(1.1f);
        triggered = false;
        ready = true;
    }
}