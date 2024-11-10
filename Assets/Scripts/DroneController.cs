using System.Collections;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Stats")]
    [SerializeField] private float responsiveness = 500f, throttleAmount = 25f, maxThrottle = 100f, bulletFireRate = 0.075f;
    public float throttle, roll, pitch, yaw;
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

    private void HandleTurret()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
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
        else if (Input.GetKeyUp(KeyCode.Mouse0))
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

        if (Input.GetKey(KeyCode.Space))
        {
            isAccelerating = true;
            throttle += Time.deltaTime * throttleAmount;
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
            throttle -= Time.deltaTime * throttleAmount;
        }

        throttle = Mathf.Clamp(throttle, 0, maxThrottle);
    }

    IEnumerator Deccelerate()
    {
        yield return new WaitForSeconds(2);
        if (!isAccelerating)
        {
            Debug.Log("SHOULD DCELRATOR NOW");
            throttle -= Time.deltaTime * throttleAmount * 0.5f;
        }

    }

}
