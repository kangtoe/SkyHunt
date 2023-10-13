using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// collider - collider 충돌 시, 부딪힌 대상에게 피해와 힘을 가한다.
public class Impactable : MonoBehaviour
{
    public float impactDamage = 10;
    public float impactPower = 10;

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log(name + " : OnCollisionEnter2D to = " + other.gameObject.name);

        // 피해주기
        Damageable damageable = coll.transform.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.GetDamaged(impactDamage);

            // 충돌 효과 프리팹 불러오기
            GameObject bumpEffect = damageable.bumpEffect;            
            if (bumpEffect)
            {
                Instantiate(bumpEffect, coll.GetContact(0).point, bumpEffect.transform.rotation);
            }
        }
        // 힘 가하기
        Rigidbody2D rbody = coll.transform.GetComponent<Rigidbody2D>();
        if (rbody)
        {
            Vector2 dir = coll.transform.position - transform.position;
            rbody.AddForce(dir * impactPower, ForceMode2D.Impulse);
        }
        
    }
}
