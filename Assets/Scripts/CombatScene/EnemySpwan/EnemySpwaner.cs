using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum Edge
{ 
    Up = 0,
    Down,
    Right,
    Left
}

// 주어진 가장자리 면에서, 적 스폰 지점과 회전을 정해서 스폰
public class EnemySpwaner : MonoBehaviourPun
{
    public static EnemySpwaner instance;

    //public GameObject enemy;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //SpwanEnemy(enemy, 12, Edge.Up);
    }

    // 주어진 가장자리면에서, count만큼 같은 간격으로 생성
    public void SpwanEnemy(GameObject enemyPrefab, int count, Edge spwanSide)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("!PhotonNetwork.IsMasterClient");
            return;
        }

        for (int i = 1; i <= count; i++)
        {
            Vector2 pos = GetSpwanPoint(spwanSide, (float)i / (count + 1));
            Quaternion rot = GetSpwanRot(spwanSide);

            PhotonNetwork.Instantiate(enemyPrefab.name, pos, rot);            

            //lastSpwanTime = Time.time;
        }
    }

    // 무작위 지점에서 생성
    public GameObject SpwanEnemyRandomPos(GameObject enemyPrefab)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("!PhotonNetwork.IsMasterClient");
            return null;
        }

        // 생성할 면을 무작위로 고른다.
        Edge randomEdge = GetRandomEdge();

        // 스폰 정보
        string str = "Enemys/" + enemyPrefab.name;
        Vector2 pos = GetSpwanPoint(randomEdge);
        Quaternion rot = GetSpwanRot(randomEdge);
        
        GameObject go = PhotonNetwork.Instantiate(str, pos, rot);
        return go;
    }

    // 무작위 지점에서 생성 + 리스트의 다른 오브젝트와 너무 가깝다면 재배치
    public GameObject SpwanEnemyRandomPos(GameObject enemyPrefab, List<GameObject> others)
    {
        GameObject go = SpwanEnemyRandomPos(enemyPrefab);

        float closeMin = 2; // 너무 가까운지 체크할 거리

        int tryCount = 100;
        // 너무 가까우면 재배치
        while (IsCloseToOthers(go, others, closeMin))
        {
            if (tryCount <= 0) break;
            tryCount--;

            // 생성할 면을 무작위로 고른다.
            Edge randomEdge = GetRandomEdge();
            go.transform.position = GetSpwanPoint(randomEdge);
            go.transform.rotation = GetSpwanRot(randomEdge);
        }        

        return go;
    }

    // 한 게임 오브젝트가 다른 리스트의 게임 오브젝트들과 dist 거리 안에 있는가?
    bool IsCloseToOthers(GameObject go, List<GameObject> others, float dist)
    {
        foreach (GameObject other in others)
        {
            if ((other.transform.position - go.transform.position).magnitude < dist) return true;
        }

        return false;
    }

    // Edge enum 중 하나를 무작위로 구한다.
    Edge GetRandomEdge()
    {
        int i = System.Enum.GetNames(typeof(Edge)).Length;
        Edge edge = (Edge)Random.Range(0, i);
        return edge;
    }

    // 가장자리 면에서 viewPoint 좌표계에서 between에 해당하는 월드 좌표 반환
    // between == null이면, 가장자리 면 위 무작위 한 점을 반환
    Vector2 GetSpwanPoint(Edge spwanSide, float? between = null)
    {
        if(between == null) between = Random.Range(0f, 1f);
        Vector3 viewPos = Vector3.zero;

        switch (spwanSide)
        {
            // 상부 가장자리
            case Edge.Up:
                viewPos = new Vector3(between.Value, 1f, 1f);
                break;
            // 하부 가장자리
            case Edge.Down:
                viewPos = new Vector3(between.Value, 0, 1f);
                break;
            // 오른쪽 가장자리
            case Edge.Right:
                viewPos = new Vector3(1, between.Value, 1f);
                break;
            // 왼쪽 가장자리
            case Edge.Left:
                viewPos = new Vector3(0, between.Value, 1f);
                break;
        }

        return Camera.main.ViewportToWorldPoint(viewPos);
    }

    Quaternion GetSpwanRot(Edge spwanSide)
    {
        float angle = 0;

        switch (spwanSide)
        {
            // 상부 가장자리
            case Edge.Up:
                angle = 180;
                break;
            // 하부 가장자리
            case Edge.Down:
                angle = 0;
                break;
            // 오른쪽 가장자리
            case Edge.Right:
                angle = 90;
                break;
            // 왼쪽 가장자리
            case Edge.Left:
                angle = 270;
                break;
        }

        return Quaternion.Euler(0f, 0f, angle);
    }
}
