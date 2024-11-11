using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DroneController : MonoBehaviour
{
    private Rigidbody rb;

    // [Header("Stats")]
    [SerializeField]
    private float responsiveness = 500f,
    liftAmount = 25f,
    maxLift = 100f,
    maxThrottle = 35f,
    bulletFireRate = 0.075f, throttle = 0.1f, maxThrust = 200f, maxReachableHeight = 65;


    public float lift, roll, pitch, yaw, lastLiftValue, throttleIncrement = 0.1f, health = 200f;
    private bool isFiringBullet = false, isAccelerating = false, isDead = false;

    [Header("Prefabs & Particles")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject[] turrets;
    [SerializeField] private ParticleSystem[] thrusters, muzzleFlash;
    [SerializeField] private ParticleSystem damageBlast;

    [Header("Drone Audios")]
    [SerializeField] private AudioSource droneAudioSource;
    [SerializeField] private AudioClip turret, blast;



    [Header("Canvas")]
    [SerializeField] private Slider slider;

    public Transform respawnLocation;


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
        HandleHealth();
    }

    private void HandleTurret()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
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
        else if (Input.GetKeyUp(KeyCode.Mouse2))
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
        if (rb.position.y > maxReachableHeight && rb.velocity.y > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        rb.AddForce(maxThrust * throttle * transform.forward);
        rb.AddForce(transform.up * lift, ForceMode.Impulse);
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
            lift = lastLiftValue;
            isAccelerating = true;
            lift += Time.deltaTime * liftAmount;
            lastLiftValue = lift;
            StartCoroutine(Deccelerate());
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isAccelerating = false;
        }

        else if (Input.GetKey(KeyCode.LeftShift))
        {
            isAccelerating = true;
            lift -= Time.deltaTime * liftAmount;
            StartCoroutine(Deccelerate());
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isAccelerating = false;
        }

        if (Input.GetKey(KeyCode.Mouse0)) throttle += throttleIncrement;
        else if (Input.GetKey(KeyCode.Mouse1)) throttle -= throttleIncrement * 2;

        lift = Mathf.Clamp(lift, 0, maxLift);
        throttle = Mathf.Clamp(throttle, 0, maxThrottle);
    }

    IEnumerator Deccelerate()
    {
        yield return new WaitForSeconds(1);
        if (!isAccelerating)
        {
            lift = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            health -= 10;
        }
    }

    private void HandleHealth()
    {
        if (health <= 0 && !isDead)
        {
            droneAudioSource.PlayOneShot(blast);
            isDead = true;
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
            damageBlast.gameObject.SetActive(true);
            damageBlast.Play();
            StartCoroutine(StopBlast());
        }
    }

    IEnumerator StopBlast()
    {
        yield return new WaitForSeconds(1.5f);
        damageBlast.gameObject.SetActive(false);
        damageBlast.Stop();
        yield return new WaitForSeconds(1);
        RespawnDrone();
    }

    private void RespawnDrone()
    {
        lift = 0; roll = 0; pitch = 0; yaw = 0;
        transform.position = respawnLocation.transform.position;
        transform.rotation = respawnLocation.rotation;

        isDead = false;
        health = 200;
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HealthBar")
        {
            health += 75;
            if (health > 200f)
            {
                health = 200f;
            }
        }
    }
}
