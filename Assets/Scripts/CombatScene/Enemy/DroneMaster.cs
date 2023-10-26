using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DroneMaster : MonoBehaviourPun
{
    public GameObject dronePrefab;
    public int createDroneCount = 5;
    public int droneMoveSpeed = 1;
    public float halfRadius = 1; // 드론 생성시 부모 오브젝트와 간격

    // 드론 별 이루는 각도 저장
    Dictionary<GameObject, float> droneInfos = new Dictionary<GameObject, float>();

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        // 사망 시, 제어 드론 파괴
        Damageable damageable = GetComponent<Damageable>();
        damageable.onDeadGlobal.AddListener(delegate
        {
            DestoryDrones();
        });

        CreateDornes();
    }
 
    void Update()
    {
        MoveDrones(droneMoveSpeed * Time.deltaTime);
    }

    // n개의 드론을 동일한 각도 간격으로 배치
    void CreateDornes()
    {        
        float halfRadius = 1;
        Vector2 createPoint;

        for (int i = 0; i < createDroneCount; i++)
        {
            Debug.Log("create drone");

            // 생성 위치 알아내기
            int deg = 360 / createDroneCount * i; // 드론이 부모 오브젝트와 이루는 각
            float rad = deg * Mathf.Deg2Rad;
            createPoint = new Vector2(halfRadius * Mathf.Sin(rad), halfRadius * Mathf.Cos(rad));

            // 생성
            Vector3 pos = transform.position + (Vector3)createPoint;
            string str = "Enemys/" + dronePrefab.name;
            GameObject go = PhotonNetwork.Instantiate(str, pos, transform.rotation);          

            // dictionary에 정보 추가
            //droneInfos.Add(go, deg);            
            PhotonView pv = go.GetComponent<PhotonView>();
            photonView.RPC(nameof(AddDroneList), RpcTarget.AllBuffered, pv.ViewID, deg);
        }
    }

    [PunRPC]
    void AddDroneList(int viewID, int deg)
    {
        Debug.Log("AddDroneList || viewID : " + viewID + " || deg : " + deg);

        PhotonView pv = PhotonView.Find(viewID);
        droneInfos.Add(pv.gameObject, deg);
    }

    void MoveDrones(float angle)
    {
        Vector2 movePoint;

        List<GameObject> keys = new List<GameObject>(droneInfos.Keys);
        
        foreach (GameObject key in keys)
        {
            if (key == null)
            {
                droneInfos.Remove(key);
                continue;
            }

            //Debug.Log("MoveDrones");

            // 이동 위치 알아내기
            float deg = droneInfos[key] + angle;
            float rad = deg * Mathf.Deg2Rad;
            movePoint = new Vector2(halfRadius * Mathf.Sin(rad), halfRadius * Mathf.Cos(rad));
            
            // 오브젝트 이동, 이동한 각도 dictionary에 반영
            key.transform.position = (Vector2)transform.position + movePoint;
            droneInfos[key] = deg;
        }
    }

    // 모든 드론 파괴
    public void DestoryDrones()
    {        
        // 순차적으로 약간의 지연시간 후 파괴
        float delay = 0.15f;
        float currentDelay = 0;
        foreach (GameObject drone in droneInfos.Keys)
        {
            currentDelay += delay;
            drone.GetComponent<DroneSevant>().DelayDestory_RPC(currentDelay);
        }
    }

    private void OnDrawGizmos()
    {
        // 드론 궤적 그리기
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, halfRadius);
    }
}
