using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    override public void GetDamaged(float damage, int hitObjOwner)
    {
        base.GetDamaged(damage, hitObjOwner);
        float healthRatio = currnetHealth / maxHealth;
        //UiManager.instance.SetHealthBar(healthRatio);
    }

    override protected void Die()
    {
        //GameManager.instance.GameOver();
        base.Die();
    }
}
