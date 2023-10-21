using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 폭발적인 힘을 한번에 가함 -> 감속하여 속력이 0에 가까워 지면 다시 폭발적으로 이동
public class MoveImpulse : MonoBehaviour
{
    public float movePower = 1f;
    public float minVelocity = 0.1f; // 이 이하로 속력이 떨어지면 급가속
    
    float minInterval = 1f;
    float lastImpulsedTime = 0;
    Rigidbody2D rbody;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        //AdjustStat();

        rbody.AddForce(transform.up * movePower, ForceMode2D.Impulse);
    }

    // 움직임에 개체 별 약간의 차이를 둔다.
    void AdjustStat()
    {
        float minMult = 0.9f;
        float maxMult = 1.1f;
        float randomMultiplier = Random.Range(minMult, maxMult);
        movePower *= randomMultiplier;
    }

    private void FixedUpdate()
    {
        // 이전 힘을 가한 이래로 일정시간 경과
        if (Time.time < lastImpulsedTime + minInterval) return;

        if (rbody.velocity.magnitude < minVelocity)
        {
            rbody.AddForce(transform.up * movePower * rbody.mass, ForceMode2D.Impulse);
            lastImpulsedTime = Time.time;
        }        
    }
}
