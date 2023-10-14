using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// źȯ�� ���߸� ����
public class BulletRemain : BulletBase
{
    public float remainVelocity = 0.2f;

    private void LateUpdate()
    {
        //Debug.Log("velocity : " + rbody.velocity.magnitude);

        if (rbody.velocity.magnitude <= remainVelocity)
        {
            DestroySelf();
        }
    }
}
