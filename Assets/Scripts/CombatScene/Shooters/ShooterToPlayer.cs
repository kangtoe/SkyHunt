using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// player 방향으로 사격
public class ShooterToPlayer : ShooterStandard
{
    FindTarget Ft
    {
        get
        {
            if (!ft) ft = GetComponent<FindTarget>();
            return ft;
        }
    }
    FindTarget ft;
    Transform Target => Ft.target;

    // 실제 사격 -> shooter의 firePoint에서 target 방향대로 projectile을 생성
    override protected void Fire()
    {
        if (!photonView.IsMine) return;

        foreach (Transform firePoint in firePoints)
        {            
            Vector2 dir;
            if (!Target) dir = firePoint.position;
            else dir = Target.position - firePoint.position; // firePoint에서 target 방향
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            Quaternion rot = Quaternion.Euler(0, 0, angle - 90);
            Vector3 pos = firePoint.position;

            // 발사체 생성
            GameObject go = CreateBullet(pos, rot);
            go.GetComponent<BulletBase>().Init_RPC(
                targetLayer, damage, impactPower, projectileMovePower, projectileLiveTime, color.r, color.g, color.b);
        }
    }
}
