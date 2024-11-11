using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private bool addBulletSpread = true;
    [SerializeField] private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField] private float shootForce = 1000f, life = 1.5f;
    private Rigidbody rb;
    public string simpleBulletId;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = GetDirection() * shootForce;
        Destroy(gameObject, life);
    }


    private void OnTriggerEnter(Collider other)
    {
        DeactivateObjects();
    }

    private void DeactivateObjects()
    {
        rb.velocity = Vector3.zero;
        Destroy(gameObject);
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;

        if (addBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
                Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
                Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
            );

            direction.Normalize();
        }
        return direction;
    }
}
