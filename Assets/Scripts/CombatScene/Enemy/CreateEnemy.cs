using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CreateEnemy : MonoBehaviourPun
{
    public GameObject enemyPrefab;
    //public GameObject createEffect;

    public float createStartDelay = 3f; // 생성 시작 대기
    public float createDelay = 2f; // 생성 간 딜레이
    float lastCreate = 0;

    public float createAngle = 90;
    public float createPower = 1;

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        createStartDelay -= Time.deltaTime;
        if (createStartDelay > 0) return;

        CreateCheck();
    }

    protected virtual void CreateCheck()
    {
        // 마지막 발사로부터 충분한 시간 간격이 있었는가
        if (Time.time >= lastCreate + createDelay)
        {
            // 마지막 발사시점 갱신
            lastCreate = Time.time;

            //Debug.Log("fire!");
            Create();
        }
    }

    public void Create()
    {
        // 생성
        string str = "Enemys/" + enemyPrefab.name;        
        GameObject go = PhotonNetwork.Instantiate(str, transform.position, transform.rotation);

        // 약간의 랜덤한 힘 가하기        
        float randomAngle = Random.Range(-createAngle, createAngle); // 무작위 각도
        Vector2 dir = Quaternion.Euler(0, 0, randomAngle) * transform.up; // 현재 오브젝트 각도 + 무작위 각도만큼 회전        
        go.GetComponent<Rigidbody2D>().AddForce(dir.normalized * createPower, ForceMode2D.Impulse);

        // 시각 효과
        //if (createEffect) Instantiate(createEffect, transform.position, transform.rotation);
    }
}
