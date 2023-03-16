using UnityEngine;

public class EnemyArcher : MonoBehaviour
{
    private float fireRate = 5f;
    public GameObject arrowPrefab;
    public Transform firePoint;
    private Transform player;

    private float nextFireTime = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
        arrowRb.velocity = (player.position - firePoint.position).normalized * 25;
    }
}