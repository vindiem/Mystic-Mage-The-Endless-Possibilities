using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage = 10;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        transform.LookAt(player.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") == true)
        {
            collision.gameObject.GetComponent<Skills>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
