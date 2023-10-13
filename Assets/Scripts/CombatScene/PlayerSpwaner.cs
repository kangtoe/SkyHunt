using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpwaner : MonoBehaviourPunCallbacks
{
    public static PlayerSpwaner Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<PlayerSpwaner>();

            return instance;
        }
    }
    private static PlayerSpwaner instance;

    public Transform spawnPosition1;
    public Transform spawnPosition2;

    public GameObject player1ShipPrefab;
    public GameObject player2ShipPrefab;

    [Header("디버그 : 현재 스폰된 플레이어 기체들")]
    [SerializeField]
    List<GameObject> spwanedPlayerShips = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 쉽 스폰
        {
            string _name = "PlayerShips/";
            GameObject spwanedShip;

            // 스폰
            if (PhotonNetwork.IsMasterClient)
            {
                _name += player1ShipPrefab.name;
                spwanedShip = PhotonNetwork.Instantiate(_name, spawnPosition1.position, Quaternion.identity);
            }
            else
            {
                _name += player2ShipPrefab.name;
                spwanedShip = PhotonNetwork.Instantiate(_name, spawnPosition2.position, Quaternion.identity);                
            }
             
            // 리스트 추가
            int id = spwanedShip.GetPhotonView().ViewID;
            photonView.RPC(nameof(AddPlayer), RpcTarget.AllBuffered, id);
        }        
    }

    // 리스트에 추가
    [PunRPC]
    void AddPlayer(int _viewId)
    {
        PhotonView pv = PhotonView.Find(_viewId);
        GameObject newObject = pv.gameObject;
        spwanedPlayerShips.Add(newObject);
    }

    public GameObject GetClosestPlayer(Vector3 pos)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (GameObject ship in spwanedPlayerShips)
        {
            if (ship == null) continue;

            // 플레이어 또는 기준 지점과 각 오브젝트 간의 거리를 계산
            float distance = Vector3.Distance(ship.transform.position, pos);

            // 현재까지 가장 가까운 오브젝트보다 더 가까운 오브젝트를 찾으면 업데이트
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = ship;
            }
        }

        return closestObject;
    }
}

