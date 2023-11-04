using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaveManager : MonoBehaviour
{
    WaveManager instace;

    public float waveInterval = 3;
    //public float sortInterval = 1;
    
    bool spwaning = false;

    SpwanInfo  SpwanInfo => SpwanInfo.instance;
    EnemySpwaner Spwaner => EnemySpwaner.instance;

    // 현재 wave의 가용 포인트 -> currentWave * pointMult
    public int currentWave = 10;
    int pointMult = 500;

    int CurrentWavePoint => (currentWave + 1) * pointMult;
    int CurrentEnemyCount => transform.childCount;

    private void Awake()
    {
        instace = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        // 모든 적 스폰 완료, 남아있는 적 없음 -> 모든 적 격파
        if (!spwaning && CurrentEnemyCount == 0)
        {            
            StartNextWave();
        }
    }    

    void StartNextWave()
    {        
        StartCoroutine(SpwanCr());
    }

    IEnumerator SpwanCr()
    {
        spwaning = true;
        // 일정 시간 후 생성 시작
        yield return new WaitForSeconds(waveInterval);        
        currentWave++;

        //Debug.Log("Spwan start");
        //UiManager.instance.ShowWaveUi("WAVE 0" + currentWave.ToString());

        // 스폰할 적 종류와 수량
        Dictionary<GameObject, int> enemys = SpwanInfo.GetSpwanableEnemys(CurrentWavePoint, currentWave);

        // 스폰된 적 리스트
        List<GameObject> spwaned = new List<GameObject>();

        foreach (KeyValuePair<GameObject, int> pair in enemys)
        {
            // 같은 종류의 적을 일정 시간을 두고 생성
            for (int i = 0; i < pair.Value; i++)
            {
                // 생성할 적
                GameObject enemy = pair.Key;

                // 다음 생성까지 시간차 두기
                float interval = GetUnitInterval(enemy.GetComponent<EnemyInfo>());
                yield return new WaitForSeconds(interval);

                // null인 요소 list에서 삭제
                spwaned.RemoveAll(e => e == null);

                GameObject go = Spwaner.SpwanEnemyRandomPos(enemy, spwaned);
                spwaned.Add(go);
            }

            // 다른 종류의 적을 생성할 때 추가적인 사건 간격을 둠
            //yield return new WaitForSeconds(sortInterval);
        }

        spwaning = false;
    }

    // 적의 점수에 따라 생성 기준 간격을 반환 
    float GetUnitInterval(EnemyInfo enemy)
    {
        // 적기 점수 / PointPerSec -> 생성 간격
        float PointPerSec = 100; 
        return enemy.point / PointPerSec;
    }
}
