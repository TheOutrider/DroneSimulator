using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{

    private Rigidbody rb;
    [SerializeField] private float responsiveness = 500f, throttleAmount = 25f, maxThrottle = 100f;

    public float throttle, roll, pitch, yaw;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleInputs();
    }

//https://github.com/alvgaona/unity-drone-controller/blob/main/drone_controller/Assets/F450_Controller/Code/Physics/BaseRigidBody.cs

    private void FixedUpdate()
    {
        rb.AddForce(transform.up * throttle, ForceMode.Impulse);
        rb.AddTorque(transform.right * pitch * responsiveness);
        rb.AddTorque(transform.forward * -roll * responsiveness);
        rb.AddTorque(transform.up * yaw * responsiveness);
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space) )
        {
            throttle += Time.deltaTime * throttleAmount;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            throttle -= Time.deltaTime * throttleAmount;
        }

        throttle = Mathf.Clamp(throttle, 0, maxThrottle)   ;
    }


    // public float moveSpeed = 5f;   // Speed of movement
    // public float rotateSpeed = 50f; // Speed of rotation
    // public float liftSpeed = 3f;    // Speed for up/down movement

    // private float upDownAxis, forwardBackwardAxis, leftRightAxis;
    // private float forwardBackwardAngle = 0, leftRightAngle = 0;

    // [SerializeField]
    // private float speed = 1.3f, angle = 25;

    // bool isGrounded = false;

    // private Rigidbody rb;

    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();
    // }

    // private void HandleControls()
    // {

    //     //FOR LIFT
    //     if (Input.GetKey(KeyCode.Space))
    //     {
    //         upDownAxis = 10 * speed;
    //         isGrounded = false;
    //     }
    //     else if (Input.GetKeyDown(KeyCode.LeftControl))
    //     {
    //         upDownAxis = 8;
    //     }
    //     else
    //     {
    //         upDownAxis = 9.8f;
    //     }

    //     //FOR FOWARD AND BACKWWARD MOVEMENT

    //     if (Input.GetKey(KeyCode.W))
    //     {
    //         forwardBackwardAngle = Mathf.Lerp(forwardBackwardAngle, angle, Time.deltaTime);
    //         forwardBackwardAxis = speed;
    //     }
    //     else if (Input.GetKey(KeyCode.S))
    //     {
    //         forwardBackwardAngle = Mathf.Lerp(forwardBackwardAngle, -angle, Time.deltaTime);
    //         forwardBackwardAxis = -speed;
    //     }
    //     else
    //     {
    //         forwardBackwardAngle = Mathf.Lerp(forwardBackwardAngle, 0, Time.deltaTime);
    //         forwardBackwardAxis = 0;
    //     }

    //     //FOR LEFT AND RIGHT MOVEMENT

    //     if (Input.GetKey(KeyCode.D))
    //     {
    //         leftRightAngle = Mathf.Lerp(leftRightAngle, angle, Time.deltaTime);
    //         leftRightAxis = speed;
    //     }
    //     else if (Input.GetKey(KeyCode.D))
    //     {
    //         leftRightAngle = Mathf.Lerp(leftRightAngle, -angle, Time.deltaTime);
    //         leftRightAxis = -speed;
    //     }
    //     else
    //     {
    //         leftRightAngle = Mathf.Lerp(leftRightAngle, 0, Time.deltaTime);
    //         leftRightAxis = 0;
    //     }

    //     //FOR DYNAMIC MOVEMENT
    //     if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
    //     {
    //         forwardBackwardAngle = Mathf.Lerp(forwardBackwardAngle, angle, Time.deltaTime);
    //         leftRightAngle = Mathf.Lerp(leftRightAngle, angle, Time.deltaTime);
    //         forwardBackwardAxis = 0.5f * speed;
    //         leftRightAxis = 0.5f * speed;
    //     }

    //     if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
    //     {
    //         forwardBackwardAngle = Mathf.Lerp(forwardBackwardAngle, angle, Time.deltaTime);
    //         leftRightAngle = Mathf.Lerp(leftRightAngle, -angle, Time.deltaTime);
    //         forwardBackwardAxis = 0.5f * speed;
    //         leftRightAxis = -0.5f * speed;
    //     }

    //     if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
    //     {
    //         forwardBackwardAngle = Mathf.Lerp(forwardBackwardAngle, -angle, Time.deltaTime);
    //         leftRightAngle = Mathf.Lerp(leftRightAngle, angle, Time.deltaTime);
    //         forwardBackwardAxis = -0.5f * speed;
    //         leftRightAxis = 0.5f * speed;
    //     }

    //     if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
    //     {
    //         forwardBackwardAngle = Mathf.Lerp(forwardBackwardAngle, -angle, Time.deltaTime);
    //         leftRightAngle = Mathf.Lerp(leftRightAngle, -angle, Time.deltaTime);
    //         forwardBackwardAxis = -0.5f * speed;
    //         leftRightAxis = -0.5f * speed;
    //     }

    // }

    // void Update()
    // {

    //     HandleControls();
    //     transform.localEulerAngles = Vector3.back * leftRightAngle + Vector3.right * forwardBackwardAngle;

    //     // // Move the drone forward/backward
    //     // float moveForwardBackward = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
    //     // // Move the drone left/right
    //     // float moveLeftRight = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
    //     // // Lift the drone up/down
    //     // float moveUpDown = 0f;
    //     // if (Input.GetKey(KeyCode.Space)) // Move up when Space key is held
    //     // {
    //     //     moveUpDown = liftSpeed * Time.deltaTime;
    //     // }
    //     // if (Input.GetKey(KeyCode.LeftControl)) // Move down when LeftControl key is held
    //     // {
    //     //     moveUpDown = -liftSpeed * Time.deltaTime;
    //     // }

    //     // // Rotate the drone
    //     // float yaw = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime; // Rotate based on mouse movement (for yaw)
    //     // float pitch = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime; // Rotate based on mouse movement (for pitch)

    //     // // Apply movement
    //     // rb.MovePosition(transform.position + transform.forward * moveForwardBackward + transform.right * moveLeftRight + transform.up * moveUpDown);

    //     // // Apply rotation
    //     // transform.Rotate(Vector3.up, yaw);
    //     // transform.Rotate(Vector3.left, pitch);
    // }

    // private void FixedUpdate()
    // {
    //     rb.AddRelativeForce(leftRightAxis, upDownAxis, forwardBackwardAxis);
    // }
}
