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

    SpriteRenderer sprite;
    TrailRenderer trail;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        //if (!photonView.IsMine) return;

        //sprite = GetComponent<SpriteRenderer>();
        //trail = GetComponentInChildren<TrailRenderer>();
        
        rbody = GetComponent<Rigidbody2D>();
        rbody.velocity = transform.up * movePower;
        //Debug.Log("velocity : " + rbody.velocity);

        if (!photonView.IsMine) return;
        Invoke(nameof(DestroySelf), liveTime);
    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        //Debug.Log("other:" + other.name);

        // targetLayer 검사
        if (1 << other.gameObject.layer == targetLayer.value)
        {
            int id = other.gameObject.GetPhotonView().ViewID;
            photonView.RPC(nameof(Impact), RpcTarget.AllBuffered, id);
        }
    }

    [PunRPC]
    void Impact(int coll_Id)
    {
        PhotonView pv = PhotonView.Find(coll_Id);
        Collider2D coll = pv.gameObject.GetComponent<Collider2D>();

        // 피해주기
        Damageable damageable = coll.transform.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.GetDamaged(damage);
        }
        // 힘 가하기
        Rigidbody2D rbody = coll.transform.GetComponent<Rigidbody2D>();
        if (rbody)
        {
            Vector2 dir = coll.transform.position - transform.position;
            rbody.AddForce(dir * impact, ForceMode2D.Impulse);
        }

        DestroySelf();
    }

    public void Init_RPC(int targetLayer, int damage, int impact, float movePower, float liveTime, float colorR, float colorG, float colorB)
    {
        //Debug.Log("targetLayer :  " + targetLayer);

        photonView.RPC(nameof(Init), RpcTarget.All, targetLayer, damage, impact, movePower, liveTime, colorR, colorG, colorB);
    }

    // 탄환 수치를 사격당 설정할필요 있나? -> TODO : 수치변경시만  static한 값을 수정?
    [PunRPC]
    // shooter에서 생성 시 호출 -> 초기화
    // hitEffect는 GameObject 직렬화가 불가능한 관계로 발사체 프리펩에서 지정할 것 
    public void Init(int targetLayer, int damage, int impact, float movePower, float liveTime, float colorR, float colorG, float colorB)
    {
        //Debug.Log("init");        
        this.targetLayer = targetLayer;
        this.damage = damage;
        this.impact = impact;
        this.movePower = movePower;
        this.liveTime = liveTime;

        sprite = GetComponent<SpriteRenderer>();        
        trail = GetComponentInChildren<TrailRenderer>();

        ColorCtrl colorCtrl = GetComponent<ColorCtrl>();
        Color color = new Color(colorR, colorG, colorB);
        colorCtrl.SetColor(color);
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

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }            
    }
}
