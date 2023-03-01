using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayersSpawn : MonoBehaviour
{
    public GameObject player;
    public float minZ, maxZ, minX, maxX;

    private void Start()
    {
        float RandomX = Random.Range(maxX, maxX);
        float RandomZ = Random.Range(maxZ, maxZ);
        Vector3 randomPosition = new Vector3(RandomX, 2, RandomZ);

        PhotonNetwork.Instantiate(player.name, randomPosition, Quaternion.identity);

    }

}
