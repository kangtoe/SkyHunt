using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDamageable : Damageable
{
    override public void GetDamaged(float damage, int hitObjOwner)
    {
        base.GetDamaged(damage, hitObjOwner);

        float healthRatio = currnetHealth / maxHealth;
        if (photonView.IsMine)
        {            
            UiManager.Instance.UpdateMyHpGage(healthRatio);
        }
        else
        {
            UiManager.Instance.UpdateOtherHpGage(healthRatio);
        }
    }

    private void Update()
    {
        // 데미지 디버깅 코드
        //if (photonView.IsMine)
        //{
        //    if (Input.GetKeyDown(KeyCode.Tab))
        //    {
        //        GetDamaged(10, PhotonNetwork.LocalPlayer.ActorNumber);
        //    }
        //}
    }

    override protected void Die()
    {
        //GameManager.instance.GameOver();
        UiManager.Instance.ActiveOverPanel();
        photonView.RPC(nameof(OtherDie), RpcTarget.Others);
        base.Die();
    }

    [PunRPC]
    void OtherDie()
    {
        UiManager.Instance.ActiveOtherDeathUi();
    }
}
