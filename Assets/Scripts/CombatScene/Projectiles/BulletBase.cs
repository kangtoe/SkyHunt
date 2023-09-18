using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// źȯ�� ������� ���� 3����
// 1. ���� �� ���� �ð��� ���.
// 2. ȭ�� ������ ����
// 3. ��ü �浹
public class BulletBase : MonoBehaviourPun
{
    public GameObject hitEffect;
    public LayerMask targetLayer; // �ش� ������Ʈ�� �浹�� �˻��� ���̾�        
    public int damage;
    public int impact;
    public float movePower;
    public float liveTime;

    protected Rigidbody2D rbody;
    TrailRenderer trail;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        //if (!photonView.IsMine) return;

        rbody = GetComponent<Rigidbody2D>();
        trail = GetComponentInChildren<TrailRenderer>();
        Invoke(nameof(DestroySelf), liveTime);
        rbody.velocity = transform.up * movePower;                
        //Debug.Log("velocity : " + rbody.velocity);
    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine) return;
        //Debug.Log("other:" + other.name);

        // targetLayer �˻�
        if (1 << other.gameObject.layer == targetLayer.value)
        {
            //Debug.Log("name:" + name + ", hit damege:" + damage);

            // �����ְ�
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable)
            {
                damageable.GetDamaged(damage);
            }

            Rigidbody2D rbody = other.GetComponent<Rigidbody2D>();
            if (rbody)
            {
                rbody.AddForce(transform.up * impact, ForceMode2D.Impulse);
            }

            DestroySelf();
        }
    }

    // shooter���� ���� �� ȣ�� -> �ʱ�ȭ
    public void Init(GameObject hitEffect, LayerMask targetLayer, int damage, int impact, float movePower, float liveTime)
    {
        this.hitEffect = hitEffect;
        this.targetLayer = targetLayer;
        this.damage = damage;
        this.impact = impact;
        this.movePower = movePower;
        this.liveTime = liveTime;
    }

    bool destoryProcessing = false;
    protected void DestroySelf()
    {
        if (destoryProcessing) return;
        destoryProcessing = true;

        if (trail)
        {
            //Debug.Log("trail disttach");
            trail.transform.parent = null;
            trail.autodestruct = true;                                 
        }

        //Debug.Log("Instantiate hitEffect");
        if(hitEffect) Instantiate(hitEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
