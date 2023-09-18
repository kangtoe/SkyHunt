using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
