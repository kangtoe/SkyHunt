using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� ���� �ѹ��� ���� -> �����Ͽ� �ӷ��� 0�� ����� ���� �ٽ� ���������� �̵�
public class MoveImpulse : MonoBehaviour
{
    public float movePower = 1f;
    public float minVelocity = 0.1f; // �� ���Ϸ� �ӷ��� �������� �ް���
    
    float minInterval = 1f;
    float lastImpulsedTime = 0;
    Rigidbody2D rbody;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        AdjustStat();

        rbody.AddForce(transform.up * movePower, ForceMode2D.Impulse);
    }

    // �����ӿ� ��ü �� �ణ�� ���̸� �д�.
    void AdjustStat()
    {
        float minMult = 0.9f;
        float maxMult = 1.1f;

        float randomMultiplier = Random.Range(minMult, maxMult);
        movePower *= randomMultiplier;
    }

    private void FixedUpdate()
    {
        // ���� ���� ���� �̷��� �����ð� ���
        if (Time.time < lastImpulsedTime + minInterval) return;

        if (rbody.velocity.magnitude < minVelocity)
        {
            rbody.AddForce(transform.up * movePower * rbody.mass, ForceMode2D.Impulse);
            lastImpulsedTime = Time.time;
        }        
    }
}
