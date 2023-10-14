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

        // targetLayer �˻�
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

        // �����ֱ�
        Damageable damageable = coll.transform.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.GetDamaged(damage);
        }
        // �� ���ϱ�
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

    // źȯ ��ġ�� ��ݴ� �������ʿ� �ֳ�? -> TODO : ��ġ����ø�  static�� ���� ����?
    [PunRPC]
    // shooter���� ���� �� ȣ�� -> �ʱ�ȭ
    // hitEffect�� GameObject ����ȭ�� �Ұ����� ����� �߻�ü �����鿡�� ������ �� 
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
