using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스폰할 전체 유닛 종류에서 일정 점수로 스폰할 수 있는 적 종류와 수량을 구한다.
public class SpwanInfo : MonoBehaviour
{
    public static SpwanInfo instance;

    [Header("Spwan Info")]
    public List<GameObject> spwanUnits; // 스폰할 전체 유닛 리스트    

    private void Awake()
    {
        instance = this;
    }

    // 출현할 적의 종류와 수량 구하기
    public Dictionary<GameObject, int> GetSpwanableEnemys(int wavePoint, int currentWave)
    {
        // 실제 스폰할 적 종류와 수량
        Dictionary<GameObject, int> spwanEnemys = new Dictionary<GameObject, int>();
        // 스폰할 적 종류
        List<GameObject> enemySorts = GetSpwanableEnemySorts(wavePoint, currentWave);
        int remainPoint = wavePoint;

        // 최소 비용
        int minPoint = GetMinPoint(enemySorts);
        while (remainPoint >= minPoint)
        {
            // 무작위 적 하나 뽑기
            int ran = Random.Range(0, enemySorts.Count);
            EnemyInfo info = enemySorts[ran].GetComponent<EnemyInfo>();
            
            // 남은 가용 포인트가 충분치 않음
            if (info.point > remainPoint) continue;

            // 적 수량 추가
            if (spwanEnemys.ContainsKey(enemySorts[ran])) spwanEnemys[enemySorts[ran]]++;
            else spwanEnemys.Add(enemySorts[ran], 1);

            remainPoint -= info.point;
        }

        return spwanEnemys;
    }

    // 현재 웨이브에서 출현 가능한 적 종류 구하기    
    List<GameObject> GetSpwanableEnemySorts(int wavePoint, int currentWave)
    {
        List<GameObject> spwanableEnemys = new List<GameObject>();
        int maxSort = 3; // 한번에 뽑아서 반환할 수 있는 종류 갯수

        // 적 모든 종류 리스트 복제 -> 검사를 완료한 요소는 삭제
        List<GameObject> spwanUnitsToInspect = spwanUnits.GetRange(0, spwanUnits.Count);        

        while (maxSort > 0)
        {
            // 전체 적 종류 중 하나 뽑기
            int index = Random.Range(0, spwanUnitsToInspect.Count);                        

            EnemyInfo info = spwanUnitsToInspect[index].GetComponent<EnemyInfo>();

            // 스폰 비용 전체 가용 포인트의 절반 이하인가 -> 한 웨이브는 적어도 2기 이상의 적으로 구성하도록
            if (info.point <= wavePoint / 2)
            {
                // 최소 등장 웨이브 조건을 만족하는가
                if (info.spwanMinWave <= currentWave)
                {
                    // 기존 리스트에 존재하지 않는 경우인가 -> 출현 가능한 적 종류
                    if (!IsContainInList(spwanableEnemys, spwanUnitsToInspect[index]))
                    {
                        spwanableEnemys.Add(spwanUnitsToInspect[index]);
                        maxSort--;
                    }
                }                                
            }

            // 검사한 요소 삭제 ->  while 무한루프 방지
            spwanUnitsToInspect.RemoveAt(index);
            // 모든 요소 검사 후 삭제됨 -> 나가
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
