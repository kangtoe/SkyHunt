using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShooterStandard : ShooterBase
{
    [Header("Fire Info")]
    // ���� �� ù ��ݱ��� ���ð�
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
