using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            int coins = PlayerPrefs.GetInt("Coins");
            coins++;
            PlayerPrefs.SetInt("Coins", coins);
            Destroy(gameObject);
        }
    }
}
