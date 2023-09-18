using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 탄환이 사라지는 조건 3가지
// 1. 생성 후 일정 시간이 경과.
// 2. 화면 밖으로 나감
// 3. 물체 충돌
public class BulletBase : MonoBehaviourPun
{
    public GameObject hitEffect;
    public LayerMask targetLayer; // 해당 오브젝트와 충돌을 검사할 레이어        
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

        // targetLayer 검사
        if (1 << other.gameObject.layer == targetLayer.value)
        {
            //Debug.Log("name:" + name + ", hit damege:" + damage);

            // 피해주가
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

    // shooter에서 생성 시 호출 -> 초기화
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
