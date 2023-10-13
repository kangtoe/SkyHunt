using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShooterStandard : ShooterBase
{
    [Header("Fire Info")]
    // 등장 후 첫 사격까지 대기시간
    public float shootStartDelay = 3f;

    // Update is called once per frame
    void Update()
    {
        shootStartDelay -= Time.deltaTime;
        if (shootStartDelay > 0) return;

        TryFire();
    }

    protected override void Fire()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        base.Fire();
    }
}
