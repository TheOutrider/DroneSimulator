// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// [RequireComponent(typeof(PlayerInput))]
// public class DoneSim : MonoBehaviour
// {

//     private Rigidbody rb;
//     protected float startDrag, startAngularDrag;
//     [SerializeField]
//     private float weightsInLbs = 1f, minMaxPitch = 30f, minMaxRoll = 40f, yawPower = 40f;

//     const float lbsToKg = 0.454f;

//     private Vector2 cyclic;
//     private float pedals, throttle;

//     public Vector2 Cyclic { get => cyclic; }
//     public float Pedals { get => pedals; }
//     public float Throttle { get => throttle; }

//     private void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         if (rb)
//         {
//             rb.mass = weightsInLbs * lbsToKg;
//             startDrag = rb.drag;
//             startAngularDrag = rb.angularDrag;
//         }
//     }

//     private void OnCyclic(InputValue value)
//     {
//         cyclic = value.Get<Vector2>();
//     }

//     private void OnPedals(InputValue value)
//     {
//         pedals = value.Get<float>();
//     }

//     private void OnThrottle(InputValue value)
//     {
//         throttle = value.Get<float>();
//     }

//     private void Update()
//     {

//     }

//     private void FixedUpdate()
//     {
//         if (!rb)
//         {
//             return;
//         }
//         HandlePhysics();
//     }

//     private void HandlePhysics()
//     {
//         HandleEngines();
//         HandleControls();
//     }

//     private void HandleControls()
//     {

//     }

//     private void HandleEngines()
//     {
//         rb.AddForce(Vector3.up * (rb.mass * Physics.gravity.magnitude));

//     }
// }




/*

using System.Collections;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Stats")]
    [SerializeField] private float responsiveness = 500f, liftAmount = 25f, maxLift = 100f, bulletFireRate = 0.075f, forwardThrottleIncrement = 0.1f, maxThrust = 200f;
    // , upForce;

    public float lift, roll, pitch, yaw, forwardThrottle;
    private float lastLifteValue = 0;
    private bool isFiringBullet = false, cameraSwap = false, isAccelerating = false;

    [Header("Prefabs & Particles")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject[] turrets;
    [SerializeField] private ParticleSystem[] thrusters, muzzleFlash;

    [Header("Drone Audios")]
    [SerializeField] private AudioSource droneAudioSource;
    [SerializeField] private AudioClip turret;

    [Header("Camera POVs")]
    [SerializeField]
    private GameObject FPC, TPC;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        foreach (var flash in muzzleFlash)
        {
            flash.Stop();
        }
    }

    private void Update()
    {
        HandleInputs();
        // HandleController();
        HandleTurret();
        HandleCameraPOV();
    }

    private void HandleCameraPOV()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameraSwap = !cameraSwap;
            FPC.SetActive(cameraSwap);
            TPC.SetActive(!cameraSwap);
        }
    }

    // private void HandleController()
    // {
    //     if (Input.GetKey(KeyCode.I))
    //     {
    //         upForce = 450;
    //     }
    //     else if (Input.GetKey(KeyCode.K))
    //     {
    //         upForce = -350;
    //     }
    //     else if (!Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.K))
    //     {
    //         upForce = 98.1f;
    //     }
    // }

    private void HandleTurret()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFiringBullet = true;
            droneAudioSource.clip = turret;
            droneAudioSource.Play();
            foreach (var flash in muzzleFlash)
            {
                flash.Play();
            }
            StartCoroutine(FireBullets());

        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            isFiringBullet = false;
            foreach (var flash in muzzleFlash)
            {
                flash.Stop();
            }
            droneAudioSource.Stop();
        }
    }

    private IEnumerator FireBullets()
    {
        while (isFiringBullet)
        {
            foreach (var turret in turrets)
            {
                GameObject bullet = Instantiate(bulletPrefab, turret.transform.position, turret.transform.rotation);
            }
            yield return new WaitForSeconds(bulletFireRate);
        }
    }

    private void FixedUpdate()
    {
        if (!isAccelerating)
        {
            rb.AddForce(Vector3.up * (rb.mass * Physics.gravity.magnitude));
        }

        rb.AddForce(maxThrust * forwardThrottle * transform.forward);

        // rb.AddRelativeForce(Vector3.up * upForce);
        rb.AddForce(transform.up * lastLifteValue, ForceMode.Impulse);
        rb.AddTorque(transform.right * pitch * responsiveness);
        rb.AddTorque(transform.forward * -roll * responsiveness);
        rb.AddTorque(transform.up * yaw * responsiveness);
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space))
        {
            // lift = lastLifteValue;
            isAccelerating = true;
            lift += Time.deltaTime * liftAmount;
            lastLifteValue = lift;
            Debug.Log("ACC ++++");
            StartCoroutine(Deccelerate());
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("ACCELRATOR RELEASED");
            isAccelerating = false;
        }

        else if (Input.GetKey(KeyCode.LeftShift))
        {
            // lift = lastLifteValue;
            lift -= Time.deltaTime * liftAmount;
            lastLifteValue = lift;
            isAccelerating = true;
            StartCoroutine(Deccelerate());
        }

        if (Input.GetKey(KeyCode.Mouse0)) forwardThrottle += forwardThrottleIncrement;
        else if (Input.GetKey(KeyCode.Mouse1)) forwardThrottle -= forwardThrottleIncrement;

        lift = Mathf.Clamp(lift, 0, maxLift);
        forwardThrottle = Mathf.Clamp(forwardThrottle, 0, maxThrust);

    }

    IEnumerator Deccelerate()
    {
        yield return new WaitForSeconds(2);
        if (!isAccelerating)
        {
            lastLifteValue = 0;
            Debug.Log("SHOULD DCELRATOR NOW");
            lift -= Time.deltaTime * liftAmount * 0.5f;
        }

    }

}

*/
