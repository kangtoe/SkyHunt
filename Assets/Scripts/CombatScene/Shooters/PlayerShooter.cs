using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : ShooterBase
{
    //public int shooterLevel = 1; // ���� ������ ���ʹ� �� ���� ������ ��
    //protected int maxShooterLevel = 5; // ���� ������ �ְ� ����

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
