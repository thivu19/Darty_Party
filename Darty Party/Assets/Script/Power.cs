using UnityEngine;
using TMPro;

public class Power : MonoBehaviour
{
    [SerializeField] private Dart1 dart;
    [SerializeField] private Dart1 dart2;
    public TMP_Text powerText;

    // Update is called once per frame
    void Update()
    {
        if(dart.isTurn()) {
            powerText.text = dart.displayedPower.ToString();
        }
        else if(dart2.isTurn()) {
            powerText.text = dart2.displayedPower.ToString();
        }
    }
}
