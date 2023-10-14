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

// �־��� �����ڸ� �鿡��, �� ���� ������ ȸ���� ���ؼ� ����
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

    // �־��� �����ڸ��鿡��, count��ŭ ���� �������� ����
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

    // ������ �������� ����
    public GameObject SpwanEnemyRandomPos(GameObject enemyPrefab)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("!PhotonNetwork.IsMasterClient");
            return null;
        }

        // ������ ���� �������� ����.
        Edge randomEdge = GetRandomEdge();

        // ���� ����
        string str = "Enemys/" + enemyPrefab.name;
        Vector2 pos = GetSpwanPoint(randomEdge);
        Quaternion rot = GetSpwanRot(randomEdge);
        
        GameObject go = PhotonNetwork.Instantiate(str, pos, rot);
        return go;
    }

    // ������ �������� ���� + ����Ʈ�� �ٸ� ������Ʈ�� �ʹ� �����ٸ� ���ġ
    public GameObject SpwanEnemyRandomPos(GameObject enemyPrefab, List<GameObject> others)
    {
        GameObject go = SpwanEnemyRandomPos(enemyPrefab);

        float closeMin = 2; // �ʹ� ������� üũ�� �Ÿ�

        int tryCount = 100;
        // �ʹ� ������ ���ġ
        while (IsCloseToOthers(go, others, closeMin))
        {
            if (tryCount <= 0) break;
            tryCount--;

            // ������ ���� �������� ����.
            Edge randomEdge = GetRandomEdge();
            go.transform.position = GetSpwanPoint(randomEdge);
            go.transform.rotation = GetSpwanRot(randomEdge);
        }        

        return go;
    }

    // �� ���� ������Ʈ�� �ٸ� ����Ʈ�� ���� ������Ʈ��� dist �Ÿ� �ȿ� �ִ°�?
    bool IsCloseToOthers(GameObject go, List<GameObject> others, float dist)
    {
        foreach (GameObject other in others)
        {
            if ((other.transform.position - go.transform.position).magnitude < dist) return true;
        }

        return false;
    }

    // Edge enum �� �ϳ��� �������� ���Ѵ�.
    Edge GetRandomEdge()
    {
        int i = System.Enum.GetNames(typeof(Edge)).Length;
        Edge edge = (Edge)Random.Range(0, i);
        return edge;
    }

    // �����ڸ� �鿡�� viewPoint ��ǥ�迡�� between�� �ش��ϴ� ���� ��ǥ ��ȯ
    // between == null�̸�, �����ڸ� �� �� ������ �� ���� ��ȯ
    Vector2 GetSpwanPoint(Edge spwanSide, float? between = null)
    {
        if(between == null) between = Random.Range(0f, 1f);
        Vector3 viewPos = Vector3.zero;

        switch (spwanSide)
        {
            // ��� �����ڸ�
            case Edge.Up:
                viewPos = new Vector3(between.Value, 1f, 1f);
                break;
            // �Ϻ� �����ڸ�
            case Edge.Down:
                viewPos = new Vector3(between.Value, 0, 1f);
                break;
            // ������ �����ڸ�
            case Edge.Right:
                viewPos = new Vector3(1, between.Value, 1f);
                break;
            // ���� �����ڸ�
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
            // ��� �����ڸ�
            case Edge.Up:
                angle = 180;
                break;
            // �Ϻ� �����ڸ�
            case Edge.Down:
                angle = 0;
                break;
            // ������ �����ڸ�
            case Edge.Right:
                angle = 90;
                break;
            // ���� �����ڸ�
            case Edge.Left:
                angle = 270;
                break;
        }

        return Quaternion.Euler(0f, 0f, angle);
    }
}
