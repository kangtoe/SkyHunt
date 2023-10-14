using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    WaveManager instace;

    public float waveInterval = 3;
    //public float sortInterval = 1;
    
    bool spwaning = false;

    SpwanInfo  SpwanInfo => SpwanInfo.instance;
    EnemySpwaner Spwaner => EnemySpwaner.instance;

    // ���� wave�� ���� ����Ʈ -> currentWave * pointMult
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
        // ��� �� ���� �Ϸ�, �����ִ� �� ���� -> ��� �� ����
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
        // ���� �ð� �� ���� ����
        yield return new WaitForSeconds(waveInterval);        
        currentWave++;

        //Debug.Log("Spwan start");
        //UiManager.instance.ShowWaveUi("WAVE 0" + currentWave.ToString());

        // ������ �� ������ ����
        Dictionary<GameObject, int> enemys = SpwanInfo.GetSpwanableEnemys(CurrentWavePoint, currentWave);

        // ������ �� ����Ʈ
        List<GameObject> spwaned = new List<GameObject>();

        foreach (KeyValuePair<GameObject, int> pair in enemys)
        {
            // ���� ������ ���� ���� �ð��� �ΰ� ����
            for (int i = 0; i < pair.Value; i++)
            {
                // ������ ��
                GameObject enemy = pair.Key;

                // ���� �������� �ð��� �α�
                float interval = GetUnitInterval(enemy.GetComponent<EnemyInfo>());
                yield return new WaitForSeconds(interval);

                // null�� ��� list���� ����
                spwaned.RemoveAll(e => e == null);

                GameObject go = Spwaner.SpwanEnemyRandomPos(enemy, spwaned);
                spwaned.Add(go);
            }

            // �ٸ� ������ ���� ������ �� �߰����� ��� ������ ��
            //yield return new WaitForSeconds(sortInterval);
        }

        spwaning = false;
    }

    // ���� ������ ���� ���� ���� ������ ��ȯ 
    float GetUnitInterval(EnemyInfo enemy)
    {
        // ���� ���� / PointPerSec -> ���� ����
        float PointPerSec = 100; 
        return enemy.point / PointPerSec;
    }
}
