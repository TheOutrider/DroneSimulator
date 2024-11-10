using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private float health = 100f, amplitude = 0.5f, frequency = 1f, sightRange = 100f, bulletFireRate = 1, fireRate = 1, fireRateTimer;
    private Vector3 startPosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject[] turrets;
    [SerializeField] private ParticleSystem damageBlast;

    private bool playerInAttackRange, isDead = false;

    public LayerMask whatIsPlayer;

    private GameObject playerDrone;

    void Start()
    {
        startPosition = transform.position;
        playerDrone = GameObject.FindWithTag("Player");
    }

    bool IsObjectInRange()
    {
        if (playerDrone == null) return false; // Ensure targetObject is assigned
        float distance = Vector3.Distance(transform.position, playerDrone.transform.position);
        return distance <= sightRange;
    }

    void Update()
    {

        playerInAttackRange = IsObjectInRange();

        if (playerInAttackRange)
        {
            // StartCoroutine(FireBullets());
            if (ShouldFire()) Fire();
        }

        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        transform.LookAt(playerDrone.transform);
        foreach (GameObject turret in turrets)
        {
            turret.transform.LookAt(playerDrone.transform);
        }

        HandleHealth();
    }

    public bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate)
            return false;
        else
            return true;
    }

    public void Fire()
    {
        fireRateTimer = 0;
        foreach (var turret in turrets)
        {
            if (!isDead)
            {
                GameObject bullet = Instantiate(bulletPrefab, turret.transform.position, turret.transform.rotation);
            }

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("ENEMY BULLET HIT : " + other.gameObject.tag);
        health -= 10;
    }

    private void HandleHealth()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
            damageBlast.gameObject.SetActive(true);
            damageBlast.Play();
            // MusicManager.Instance.PlayBlast();
            StartCoroutine(StopBlast());
        }
    }

    IEnumerator StopBlast()
    {
        yield return new WaitForSeconds(1.5f);
        damageBlast.gameObject.SetActive(false);
        damageBlast.Stop();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
