using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWave : MonoBehaviour
{
    float ellapsedTime = 0f; //  삼각 함수 파형을 그리기 위해 사용되는 경과 시간    
    public float movePower = 0.1f; // 삼각 함수 파형을 그리는 속도
    public float width = 20; // 진폭
    public float height = 20; // 파장
    public bool waveInverse; // x좌표 반전 여부

    Rigidbody2D rbody;

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        ellapsedTime += movePower;

        float x1 = ellapsedTime;
        float y1 = Mathf.Sin(x1);

        float x2 = ellapsedTime += movePower;
        float y2 = Mathf.Sin(x2);

        int dirX;
        if (waveInverse) dirX = -1;
        else dirX = 1;

        Vector2 delta = new Vector2(dirX * (y2 - y1) * width, (x2 - x1) * height);

        rbody.AddForce(transform.rotation * delta * rbody.mass);
    }
}
