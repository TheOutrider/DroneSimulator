using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private float health = 100f, amplitude = 0.5f, frequency = 1f, sightRange = 100f, bulletFireRate = 1, fireRate = 1, fireRateTimer;
    private Vector3 startPosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject[] turrets, bodyParts;
    [SerializeField] private ParticleSystem damageBlast;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bulletSound, blastSound;

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
        if (playerDrone == null) return false;
        float distance = Vector3.Distance(transform.position, playerDrone.transform.position);
        return distance <= sightRange;
    }

    void Update()
    {
        playerInAttackRange = IsObjectInRange();

        if (playerInAttackRange)
        {
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
        if (!isDead)
        {
            fireRateTimer = 0;
            audioSource.PlayOneShot(bulletSound);
            foreach (var turret in turrets)
            {
                if (!isDead)
                {
                    GameObject bullet = Instantiate(bulletPrefab, turret.transform.position, turret.transform.rotation);
                }
            }
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            health -= 10;
        }
        else if (other.gameObject.tag == "Fireball")
        {
            health -= 50;
        }
    }

    private void HandleHealth()
    {
        if (health <= 0 && !isDead)
        {
            audioSource.PlayOneShot(blastSound);
            isDead = true;
            foreach (GameObject parts in bodyParts)
            {
                parts.gameObject.SetActive(false);
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
        gameObject.SetActive(false);
    }

    public void RestartEnemy()
    {
        health = 100f;
        isDead = false;
        foreach (GameObject parts in bodyParts)
        {
            parts.gameObject.SetActive(true);
        }
        damageBlast.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }
}
