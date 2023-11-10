using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDamageable : Damageable
{
    protected void Start()
    {
        // 사망 시 이벤트 추가
        {
            onDeadLocal.AddListener(delegate {
                //Debug.Log("onDeadLocal");       
            });

            onDeadGlobal.AddListener(delegate {
                Debug.Log("onDeadGlobal");

                if (photonView.IsMine) GameManager.Instance.GameOver();
                photonView.RPC(nameof(OtherDie), RpcTarget.OthersBuffered);
            });

        }

        base.Start();
    }

    override public void GetDamaged(float damage, int hitObjOwner)
    {
        //Debug.Log("player GetDamaged :" + damage);        
        lastHitObjOwner = hitObjOwner;        

        // 체력 감소 처리
        currnetHealth -= damage;
        if (currnetHealth < 0) currnetHealth = 0;        
        float healthRatio = currnetHealth / maxHealth;

        if (photonView.IsMine)
        {            
            // UI 연동
            UiManager.Instance.UpdateMyHpGage(healthRatio);
            photonView.RPC(nameof(UpdateOtherHpGageRPC), RpcTarget.OthersBuffered, healthRatio);

            // 사망 처리
            if (currnetHealth == 0) Die();
            else SoundManager.Instance.PlaySound("OnDamage"); // 사망 시에는 사망 효과음 재생
        }                        
    }

    [PunRPC]
    void UpdateOtherHpGageRPC(float ratio)
    {        
        UiManager.Instance.UpdateOtherHpGage(ratio);
    }

    // 다른 플레이어 사망
    [PunRPC]
    void OtherDie()
    {
        //Debug.Log("OtherDie");
        UiManager.Instance.ActiveOtherDeathUi();
    }
}
