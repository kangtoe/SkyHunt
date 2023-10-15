using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWave : MonoBehaviour
{
    float ellapsedTime = 0f; //  �ﰢ �Լ� ������ �׸��� ���� ���Ǵ� ��� �ð�    
    public float movePower = 0.1f; // �ﰢ �Լ� ������ �׸��� �ӵ�
    public float width = 20; // ����
    public float height = 20; // ����
    public bool waveInverse; // x��ǥ ���� ����

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
