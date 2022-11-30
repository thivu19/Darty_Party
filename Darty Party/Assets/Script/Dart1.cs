using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Dart1 : MonoBehaviour
{
    //[SerializeField] private Transform groundCheckTransform = null;
    [SerializeField] private LayerMask playerMask;
    private bool jumpKeyWasPressed;
    private bool suspendDart = false;
    private float horizontalInput;
    public float downTime, upTime, dartPower, missTime;
    private Rigidbody rigidbodyComponent;
    //private bool paused = false;
    private bool throwingStage = true;
    private bool ready = false;
    private bool timeChecker = false;
    private Vector3 rotationAngle = new Vector3(0, 0, 0);
    public float totalPoints = 300;
    [SerializeField] private HurtBox boardPart;

    // Triggers on connection with dart board
    private void OnTriggerEnter(Collider other)
    {
        suspendDart = true;
    }

    // Update the score depending on the board part
    public void pointChanger(float point)
    {
        float temp = totalPoints - point;
        // Check if the score equal to 0 then finish
        if (temp == 0)
        {
            totalPoints = temp;
            Debug.Log("Dart POINTS updated");
            SceneManager.LoadScene("Winning");
            
        } 
        else if (temp < 0) // Check if the score is less than 0 then bust
        {
            Debug.Log("Dart POINTS not updated");
        }
        else // Else update the score
        {
            totalPoints = temp;
            Debug.Log("Dart POINTS updated");
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        // Checkings for all keys and implements their functions
        if (Input.GetKeyDown(KeyCode.Space) && !ready && throwingStage)
        {
            downTime = Time.time;
            ready = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) && ready)
        {
            upTime = Time.time;
            dartPower = (upTime - downTime);
            if (dartPower > 2.5f)
            {
                dartPower = 2.5f;
            }
            dartPower = dartPower * 400;
            jumpKeyWasPressed = true;
            ready = false;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && throwingStage)
        {
            if (rotationAngle.y  < 50f)
            {
                rotationAngle += new Vector3(0, 5, 0);
                transform.eulerAngles = rotationAngle;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && throwingStage)
        {
            if (rotationAngle.y > -50f)
            {
                rotationAngle += new Vector3(0, -5, 0);
                transform.eulerAngles = rotationAngle;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && throwingStage)
        {
            if (rotationAngle.x < 30f)
            {
                rotationAngle += new Vector3(5, 0, 0);
                transform.eulerAngles = rotationAngle;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && throwingStage)
        {
            if (rotationAngle.x > -30f)
            {
                rotationAngle += new Vector3(-5, 0, 0);
                transform.eulerAngles = rotationAngle;
            }
        }
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    { 
        // Throwing dart
        if (jumpKeyWasPressed)
        {
            rigidbodyComponent.useGravity = true;
            rigidbodyComponent.AddRelativeForce(Vector3.forward * dartPower, ForceMode.Force);
            missTime = Time.time;
            jumpKeyWasPressed = false;
            throwingStage = false;
            timeChecker = true;
        }

        // Checks to see how long it has been since dart thrown, resets if missed.
        if(timeChecker)
        { 
            if (((Time.time) - missTime) > 4)
            {
                suspendDart = true;
                timeChecker = false;
            }
        }

        // Makes dart stick, also resets dart.
        if (suspendDart)
        {
            rigidbodyComponent.useGravity = false;
            jumpKeyWasPressed = false;
            rigidbodyComponent.isKinematic = true;
            StartCoroutine(Waiting());
            suspendDart = false;
            rigidbodyComponent.isKinematic = false;
        }
    }

    // Waits for a second to show user where dart hit, then resets position.
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1);
        //Debug.Log("Dart reset rotation");
        rotationAngle = new Vector3(0, 0, 0);
        //Debug.Log("Dart reset position");
        rigidbodyComponent.position = new Vector3(0, 0, 0);
        throwingStage = true;
    }
}
