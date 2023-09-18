using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : ShooterBase
{
    //public int shooterLevel = 1; // 높은 레벨의 슈터는 더 강한 공격을 함
    //protected int maxShooterLevel = 5; // 슈터 레벨의 최고 상한

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetMouseButton(0))
        {
            TryFire();
        }        
    }
}
