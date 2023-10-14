using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 좌우로 방향을 바꾸어가며 회전
// 전방으로 나아가기 위해서는 시작 시 회전
public class BulletWave : BulletBase
{
    [Header("Wave Info")]
    float ellapsedTime = 0f; //  삼각 함수 파형을 그리기 위해 사용되는 경과 시간
    float moveMult = 0.02f; // 삼각 함수 파형을 그리는 속도
    public float width = 5; // 진폭
    public float height = 5; // 파장
    public bool waveInverse; // x좌표 반전 여부

    override protected void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        MoveWave();
    }
     
    void MoveWave()
    {
        ellapsedTime += moveMult * movePower;

        float x1 = ellapsedTime;
        float y1 = Mathf.Sin(x1);

        float x2 = ellapsedTime += moveMult * movePower;
        float y2 = Mathf.Sin(x2);

        int dirX;
        if (waveInverse) dirX = -1;
        else dirX = 1;

        Vector2 delta = new Vector2(dirX * (y2 - y1) * width, (x2 - x1) * height);
        
        rbody.velocity = transform.rotation * delta;
    }
}
