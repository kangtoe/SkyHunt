using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// rotate to player와 함께 사용 중
// 타겟과 이루는 각도에 따라 가하는 힘을 조절
public class BulletVariable : BulletBase
{
    public Transform target;

    override protected void Start()
    {
        base.Start();
        //rbody.velocity = Vector2.zero;
        //target = GameManager.instance.GetCurrentPlayer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!target)
        {
            rbody.AddForce(transform.up * movePower);
        }
        else
        {
            float powerMult = 1 - (GetAngleToTarget() / 180);
            //rbody.velocity = (transform.up * movePower * powerMult);
            rbody.AddForce(transform.up * movePower * powerMult);
        }                
    }

    // 타겟과 오브젝트 위쪽 방향 백터가 이루는 각도의 절댓값 (0~180도 사이)
    float GetAngleToTarget()
    {
        Vector2 vec = target.position - transform.position;
        float currentUpAngle = transform.rotation.eulerAngles.z + 90; // 위쪽 방향 백터
        float angle = Mathf.Abs(currentUpAngle - Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg);
        if(angle > 180) angle = 360 - angle;

        return angle;
    }
}
