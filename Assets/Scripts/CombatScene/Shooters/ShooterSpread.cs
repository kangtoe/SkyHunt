using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShooterSpread : ShooterStandard
{
    public int shotCount = 5; // 한번에 발사하는 탄환 수
    public float shotAngle = 30;

    override protected void Fire()
    {
        if (!photonView.IsMine) return;

        foreach (Transform firePoint in firePoints)
        {
            for (int i = 0; i < shotCount; i++)
            {
                //Debug.Log("i : " + i);

                // 중앙으로부터 회전할 정도
                // 중앙으로부터 회전이 클수록 탄속도 느려진다.
                float f = -1 + 2f * i / (shotCount - 1);
                //float f = Random.Range(-1f, 1f);                 
                //Debug.Log("i : " + i + "||" + "f : " + f);

                Vector3 pos = firePoint.position;

                // firePoint에서 target 방향
                float angle = f * shotAngle;
                Quaternion rot = firePoint.rotation * Quaternion.Euler(0, 0, angle);

                // 발사체 생성
                GameObject go = CreateBullet(pos, rot);
                
                float speed = 0.5f * projectileMovePower + 0.5f * projectileMovePower * (1 - Mathf.Abs(f));
                go.GetComponent<BulletBase>().Init_RPC(targetLayer, damage, impactPower, speed, projectileLiveTime, color.r, color.g, color.b);
            }
        }
    }
}
