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
    [Header("임시 비활성화 컴포넌트들")]    
    public Renderer[] renderers;
    //public ParticleSystem[] pss;
    bool isDestroyed = false;

    [Space]
    public GameObject hitEffect;    
    public LayerMask targetLayer; // 해당 오브젝트와 충돌을 검사할 레이어        
    public int damage;
    public int impact;
    public float movePower;

    public float liveTime = 10;
    float spwanedTime = 0;
     
    protected Rigidbody2D rbody;
    protected SpriteRenderer sprite;
    TrailRenderer trail;

    public int ownerActor;

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
        ownerActor = photonView.OwnerActorNr;
    }

    private void Update()
    {
        if (isDestroyed) return;
        spwanedTime += Time.deltaTime;
        if (photonView.IsMine)
        {
            if (liveTime < spwanedTime) DestroyGolbal();
        }
    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if (isDestroyed) return;

        // targetLayer 검사
        if (1 << other.gameObject.layer == targetLayer.value)
        {
            //if (PhotonNetwork.IsMasterClient)
            //{
            //    int id = other.gameObject.GetPhotonView().ViewID;
            //    photonView.RPC(nameof(Impact), RpcTarget.AllBuffered, id);
            //}            

            if (photonView.IsMine)
            {
                int id = other.gameObject.GetPhotonView().ViewID;
                photonView.RPC(nameof(Impact), RpcTarget.AllBuffered, id, photonView.OwnerActorNr);

                DestroyGolbal();
            }
            else
            {
                DestroyLocal();
            }           
        }
    }

    private void OnValidate()
    {
        renderers = GetComponentsInChildren<Renderer>();
        //pss = GetComponentsInChildren<ParticleSystem>();
    }

    [PunRPC]
    protected void Impact(int coll_Id, int ownerActorNr)
    {
        if (isDestroyed) return;

        PhotonView pv = PhotonView.Find(coll_Id);
        Collider2D coll = pv.gameObject.GetComponent<Collider2D>();

        // 피해주기
        Damageable damageable = coll.transform.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.GetDamaged(damage, ownerActorNr);
        }
        // 힘 가하기
        Rigidbody2D rbody = coll.transform.GetComponent<Rigidbody2D>();
        if (rbody)
        {
            Vector2 dir = coll.transform.position - transform.position;
            rbody.AddForce(dir * impact, ForceMode2D.Impulse);
        }        
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

    // photonView.IsMine 아닌 경우 호출
    protected void DestroyLocal()
    {
        //Debug.Log("DestroyLocal : " + name);

        isDestroyed = true;

        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
        //foreach (ParticleSystem ps in pss)
        //{
        //    ParticleSystem.EmissionModule emission = ps.emission;
        //    emission.rateOverTime = 0;
        //}        
    }

    // photonView.IsMine 경우 호출
    protected void DestroyGolbal()
    {
        //Debug.Log("DestroyGolbal : " + name);

        isDestroyed = true;        

        if (trail)
        {
            //Debug.Log("trail disttach");
            trail.transform.parent = null;
            trail.autodestruct = true;                                 
        }

        if (hitEffect)
        {
            //Debug.Log("Instantiate hitEffect");
            string str = "Projectiles/" + hitEffect.name;
            GameObject go = PhotonNetwork.Instantiate(str, transform.position, transform.rotation);
        }

        PhotonNetwork.Destroy(gameObject);        
    }
}
