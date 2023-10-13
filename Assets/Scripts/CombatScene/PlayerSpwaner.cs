using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpwaner : MonoBehaviourPunCallbacks
{
    public Transform spawnPosition1;
    public Transform spawnPosition2;

    public GameObject player1ShipPrefab;
    public GameObject player2ShipPrefab;

    // Start is called before the first frame update
    void Start()
    {        
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(player1ShipPrefab.name, spawnPosition1.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(player2ShipPrefab.name, spawnPosition2.position, Quaternion.identity);
        }        
    }
}

