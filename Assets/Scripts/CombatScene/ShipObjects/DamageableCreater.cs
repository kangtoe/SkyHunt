using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사망 시 적 생성
public class DamageableCreater : Damageable
{
    public int createCount = 3;

    override protected void Die()
    {
        CreateOnDie();

        base.Die();
    }

    void CreateOnDie()
    {
        CreateEnemy createEnemy = GetComponent<CreateEnemy>();
        createEnemy.createAngle = 90f;

        while (createCount > 0)
        {
            createCount--;
            createEnemy.Create();
        }
    }
}
