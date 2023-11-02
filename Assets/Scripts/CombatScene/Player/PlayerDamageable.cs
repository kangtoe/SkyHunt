using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    override protected void Die()
    {
        //GameManager.instance.GameOver();
        UiManager.Instance.ActiveOverPanel();
        base.Die();
    }
}
