using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// collider - collider �浹 ��, �ε��� ��󿡰� ���ؿ� ���� ���Ѵ�.
public class Impactable : MonoBehaviour
{
    public float impactDamage = 10;
    public float impactPower = 10;

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log(name + " : OnCollisionEnter2D to = " + other.gameObject.name);

        // �����ֱ�
        Damageable damageable = coll.transform.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.GetDamaged(impactDamage);

            // �浹 ȿ�� ������ �ҷ�����
            GameObject bumpEffect = damageable.bumpEffect;            
            if (bumpEffect)
            {
                Instantiate(bumpEffect, coll.GetContact(0).point, bumpEffect.transform.rotation);
            }
        }
        // �� ���ϱ�
        Rigidbody2D rbody = coll.transform.GetComponent<Rigidbody2D>();
        if (rbody)
        {
            Vector2 dir = coll.transform.position - transform.position;
            rbody.AddForce(dir * impactPower, ForceMode2D.Impulse);
        }
        
    }
}
