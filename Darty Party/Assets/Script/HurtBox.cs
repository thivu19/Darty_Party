using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField] private Dart1 dart; 
    [SerializeField] private Dart1 dart2; 
    
    public int point;
    public bool triggered = false;
    public bool ready = true;
    
    // Once the dart enter the hurt box it will update score depending on area
    private void OnTriggerEnter(Collider other)
    {
        triggered = true;
        Debug.Log("TRIGGERED");
        if (ready)
        {
            if (triggered == true)
            {
                ready = false;
                StartCoroutine(Waiting());
            }
        }
    }

    IEnumerator Waiting()
    {
        if(dart.isTurn()) {
            dart.pointChanger(point);
        }
        else if(dart2.isTurn()) {
            dart2.pointChanger(point);
        }
        yield return new WaitForSeconds(1.1f);
        triggered = false;
        ready = true;
    }
}