using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAmmoScript : MonoBehaviour
{
    [SerializeField]
    private float amplitude = 0.5f, frequency = 1f, rotateSpeed = 10f;

    private Vector3 originalPosition = new Vector3(), tempPosition = new Vector3();

    private bool ammoAcquired;

    private AudioSource audioSource;

    [SerializeField]
    private GameObject body;

    void Start()
    {
        originalPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        tempPosition = originalPosition;
        tempPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPosition;

        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(body);
        if (!ammoAcquired)
        {
            ammoAcquired = true;
            audioSource.Play();
            Destroy(gameObject, 1f);
        }

    }
}
