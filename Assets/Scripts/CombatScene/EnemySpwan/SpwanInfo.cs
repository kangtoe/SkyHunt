using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ��ü ���� �������� ���� ������ ������ �� �ִ� �� ������ ������ ���Ѵ�.
public class SpwanInfo : MonoBehaviour
{
    public static SpwanInfo instance;

    [Header("Spwan Info")]
    public List<GameObject> spwanUnits; // ������ ��ü ���� ����Ʈ    

    private void Awake()
    {
        instance = this;
    }

    // ������ ���� ������ ���� ���ϱ�
    public Dictionary<GameObject, int> GetSpwanableEnemys(int wavePoint, int currentWave)
    {
        // ���� ������ �� ������ ����
        Dictionary<GameObject, int> spwanEnemys = new Dictionary<GameObject, int>();
        // ������ �� ����
        List<GameObject> enemySorts = GetSpwanableEnemySorts(wavePoint, currentWave);
        int remainPoint = wavePoint;

        // �ּ� ���
        int minPoint = GetMinPoint(enemySorts);
        while (remainPoint >= minPoint)
        {
            // ������ �� �ϳ� �̱�
            int ran = Random.Range(0, enemySorts.Count);
            EnemyInfo info = enemySorts[ran].GetComponent<EnemyInfo>();
            
            // ���� ���� ����Ʈ�� ���ġ ����
            if (info.point > remainPoint) continue;

            // �� ���� �߰�
            if (spwanEnemys.ContainsKey(enemySorts[ran])) spwanEnemys[enemySorts[ran]]++;
            else spwanEnemys.Add(enemySorts[ran], 1);

            remainPoint -= info.point;
        }

        return spwanEnemys;
    }

    // ���� ���̺꿡�� ���� ������ �� ���� ���ϱ�    
    List<GameObject> GetSpwanableEnemySorts(int wavePoint, int currentWave)
    {
        List<GameObject> spwanableEnemys = new List<GameObject>();
        int maxSort = 3; // �ѹ��� �̾Ƽ� ��ȯ�� �� �ִ� ���� ����

        // �� ��� ���� ����Ʈ ���� -> �˻縦 �Ϸ��� ��Ҵ� ����
        List<GameObject> spwanUnitsToInspect = spwanUnits.GetRange(0, spwanUnits.Count);        

        while (maxSort > 0)
        {
            // ��ü �� ���� �� �ϳ� �̱�
            int index = Random.Range(0, spwanUnitsToInspect.Count);                        

            EnemyInfo info = spwanUnitsToInspect[index].GetComponent<EnemyInfo>();

            // ���� ��� ��ü ���� ����Ʈ�� ���� �����ΰ� -> �� ���̺�� ��� 2�� �̻��� ������ �����ϵ���
            if (info.point <= wavePoint / 2)
            {
                // �ּ� ���� ���̺� ������ �����ϴ°�
                if (info.spwanMinWave <= currentWave)
                {
                    // ���� ����Ʈ�� �������� �ʴ� ����ΰ� -> ���� ������ �� ����
                    if (!IsContainInList(spwanableEnemys, spwanUnitsToInspect[index]))
                    {
                        spwanableEnemys.Add(spwanUnitsToInspect[index]);
                        maxSort--;
                    }
                }                                
            }

            // �˻��� ��� ���� ->  while ���ѷ��� ����
            spwanUnitsToInspect.RemoveAt(index);
            // ��� ��� �˻� �� ������ -> ����
            if (spwanUnitsToInspect.Count == 0) break;
        }

        if (spwanableEnemys.Count == 0) Debug.Log("no spwanable Enemys!");
        return spwanableEnemys;
    }

    int GetMinPoint(List<GameObject> list)
    {
        int min = int.MaxValue;

        foreach (GameObject go in list)
        {
            EnemyInfo info = go.GetComponent<EnemyInfo>();
            if (info.point < min) min = info.point;
        }

        return min;
    }

    bool IsContainInList(List<GameObject> objects, GameObject go)
    {        
        foreach (GameObject m_object in objects)
        {
            if (m_object.Equals(go)) return true;
        }
        return false;
    }

    void LogList(List<GameObject> list)
    {
        foreach (GameObject element in list)
        {
            Debug.Log("element : " + element);
        }
    }
}
