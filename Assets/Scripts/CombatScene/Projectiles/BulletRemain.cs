using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 탄환이 멈추면 삭제
public class BulletRemain : BulletBase
{
    public float remainVelocity = 0.2f;

    private void LateUpdate()
    {
        //Debug.Log("velocity : " + rbody.velocity.magnitude);

        if (!photonView.IsMine) return;

        if (rbody.velocity.magnitude <= remainVelocity)
        {
            DestroyGolbal();
        }
    }
}
