using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private Dart1 dart;
    public TMP_Text scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = dart.totalPoints.ToString();
    }
}
