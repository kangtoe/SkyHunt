using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDamageable : Damageable
{
    protected void Start()
    {
        base.Start();

        onDeadLocal.AddListener(delegate {
            //Debug.Log("onDeadLocal");
       
        });

        onDeadGlobal.AddListener(delegate {

            if (photonView.IsMine) GameManager.Instance.GameOver();
            photonView.RPC(nameof(OtherDie), RpcTarget.Others);
        });
    }

    override public void GetDamaged(float damage, int hitObjOwner)
    {
        Debug.Log("player GetDamaged :" + damage);

        base.GetDamaged(damage, hitObjOwner);

        float healthRatio = currnetHealth / maxHealth;
        if (photonView.IsMine)
        {            
            UiManager.Instance.UpdateMyHpGage(healthRatio);
            photonView.RPC(nameof(UpdateOtherHpGageRPC), RpcTarget.OthersBuffered, healthRatio);
        }     

        SoundManager.Instance.PlaySound("OnDamage");
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

    [PunRPC]
    void UpdateOtherHpGageRPC(float ratio)
    {        
        UiManager.Instance.UpdateOtherHpGage(ratio);
    }

    [PunRPC]
    void OtherDie()
    {
        UiManager.Instance.ActiveOtherDeathUi();
    }
}
