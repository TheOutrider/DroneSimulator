using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private float amplitude = 0.5f, frequency = 1f, sightRange = 30f, bulletFireRate = 1f;
    private Vector3 startPosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject[] turrets;

    private bool playerInAttackRange;

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

        if (playerInAttackRange) AttackPlayer();

        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        transform.LookAt(playerDrone.transform);
    }

    private void AttackPlayer()
    {
        StartCoroutine(FireBullets());
    }

    private IEnumerator FireBullets()
    {
        while (playerInAttackRange)
        {
            foreach (var turret in turrets)
            {
                GameObject bullet = Instantiate(bulletPrefab, turret.transform.position, turret.transform.rotation);
            }
            yield return new WaitForSeconds(bulletFireRate);
        }
    }
}
