using System;
using System.Collections;
using UnityEngine;

public class Dart1 : MonoBehaviour
{
    [SerializeField] private HurtBox boardPart;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Dart1 enemyPlayer;

    private Vector3 rotationAngle = new Vector3(0, 0, 0);
    private Rigidbody rigidbodyComponent;
    private float horizontalInput;
    private bool jumpKeyWasPressed;
    private bool suspendDart = false;
    private bool throwingStage = true;
    private bool ready = false;
    private bool timeChecker = false;

    public float downTime, upTime, dartPower, missTime;
    public float totalPoints = 300;
    public int displayedPower = 0;
    public bool turn = true;


    // Triggers on connection with dart board
    private void OnTriggerEnter(Collider other)
    {
        suspendDart = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(turn) {
        // Checkings for all keys and implements their functions
        if (Input.GetKeyDown(KeyCode.Space) && !ready && throwingStage)
        {
            downTime = Time.time;
            ready = true;
        }
        if (Input.GetKey(KeyCode.Space) && Time.time - downTime < 2.5f && ready && throwingStage) 
        {
            Debug.Log((Time.time - downTime).ToString("00:00.00"));
            displayedPower = (int)((Time.time - downTime)*40);
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
                rotationAngle += new Vector3(0, 2.5f, 0);
                transform.eulerAngles = rotationAngle;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && throwingStage)
        {
            if (rotationAngle.y > -50f)
            {
                rotationAngle += new Vector3(0, -2.5f, 0);
                transform.eulerAngles = rotationAngle;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && throwingStage)
        {
            if (rotationAngle.x < 30f)
            {
                rotationAngle += new Vector3(2.5f, 0, 0);
                transform.eulerAngles = rotationAngle;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && throwingStage)
        {
            if (rotationAngle.x > -30f)
            {
                rotationAngle += new Vector3(-2.5f, 0, 0);
                transform.eulerAngles = rotationAngle;
            }
        }
        horizontalInput = Input.GetAxis("Horizontal");
        }
    }

    private void FixedUpdate()
    { 
        if(isTurn()){ 

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
        else
        {
            StartCoroutine(TurnWaiting());
        }
    }

    // Public class for checking which instance's turn it is
    public bool isTurn() {
        return turn;
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

    // Waits for a second to show user where dart hit, then resets position.
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1);
        rotationAngle = new Vector3(0, 0, 0);
        transform.eulerAngles = rotationAngle;
        rigidbodyComponent.position = new Vector3(0, 0, 0);
        displayedPower = 0;
        throwingStage = true;
        turn = false;
    }

    IEnumerator TurnWaiting()
    {
        rigidbodyComponent.position = new Vector3(110, 110, 110);
        while(enemyPlayer.isTurn()) {
            yield return new WaitForSeconds(0);
        }
        rotationAngle = new Vector3(0, 0, 0);
        rigidbodyComponent.position = new Vector3(0, 0, 0);
        //missTime = Time.time;
        timeChecker = false;
        suspendDart = false;
        turn = true;
    }
}
