using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDamageable : Damageable
{
    protected void Start()
    {        
        onDeadLocal.AddListener(delegate {
            //Debug.Log("onDeadLocal");       
        });

        onDeadGlobal.AddListener(delegate {
            Debug.Log("onDeadGlobal");

            if (photonView.IsMine) GameManager.Instance.GameOver();
            photonView.RPC(nameof(OtherDie), RpcTarget.OthersBuffered);
        });

        base.Start();
    }

    override public void GetDamaged(float damage, int hitObjOwner)
    {
        //Debug.Log("player GetDamaged :" + damage);        

        lastHitObjOwner = hitObjOwner;        

        currnetHealth -= damage;
        if (currnetHealth < 0) currnetHealth = 0;        
        float healthRatio = currnetHealth / maxHealth;

        if (photonView.IsMine)
        {            
            UiManager.Instance.UpdateMyHpGage(healthRatio);
            photonView.RPC(nameof(UpdateOtherHpGageRPC), RpcTarget.OthersBuffered, healthRatio);
        }
        
        if (currnetHealth == 0) Die();
        else SoundManager.Instance.PlaySound("OnDamage"); // 사망 시에는 사망 효과음 재생
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
        Debug.Log("OtherDie");
        UiManager.Instance.ActiveOtherDeathUi();
    }
}
