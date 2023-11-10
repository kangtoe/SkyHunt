using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class Damageable : MonoBehaviourPun
{    
    public GameObject diePrefab;
    public GameObject bumpEffect; // 충돌 시 Impactable에서 호출    
    public float maxHealth = 100;    

    [SerializeField]
    protected float currnetHealth;
    //Rigidbody2D rbody;        

    [HideInInspector]
    public UnityEvent onDeadGlobal;
    [HideInInspector]
    public UnityEvent onDeadLocal;

    [HideInInspector]
    public int lastHitObjOwner;
    
    bool isDestoryedLocal = false;

    // Start is called before the first frame update
    protected void Start()
    {
        currnetHealth = maxHealth;
        //rbody = GetComponent<Rigidbody2D>()

        // 사망 시 이벤트 체인 등록
        {
            onDeadLocal.AddListener(delegate {
                //Debug.Log("onDeadLocal");

                // 사망 효과
                SoundManager.Instance.PlaySound("Explosion");
                if (diePrefab)
                {
                    string str = "Projectiles/" + diePrefab.name;
                    PhotonNetwork.Instantiate(str, transform.position, diePrefab.transform.rotation);
                }

                // 임시 사망 처리
                isDestoryedLocal = true;
                //gameObject.SetActive(false); // 해당 코드 이후 등록된 AddListener 코드들이 호출되지 않으므로 주석처리
                
                // 실제 사망
                photonView.RPC(nameof(GlobalDestroy), RpcTarget.AllBuffered);
            });

            onDeadGlobal.AddListener(delegate {
                //Debug.Log("onDeadLocal");

                
            });
        }        
    }

    virtual public void GetDamaged(float damage, int hitObjOwner)
    {
        //if (!photonView.IsMine) return;

        lastHitObjOwner = hitObjOwner;
        //Debug.Log(name + " : GetDamaged = " + damage + "( by : player " + hitObjOwner + ")");         

        // 피격 및 사망 처리
        currnetHealth -= damage;
        if (currnetHealth < 0) currnetHealth = 0;
        if (currnetHealth == 0) Die();         
    }

    virtual protected void Die()
    {
        Debug.Log("die");        

        if (!isDestoryedLocal) onDeadLocal.Invoke();
        //if (photonView.IsMine) onDeadGlobal.Invoke();
        onDeadGlobal.Invoke();
    }

    [PunRPC]
    void GlobalDestroy()
    {
        Debug.Log("lastHitObjOwner : " + lastHitObjOwner + " || LocalPlayer: " + PhotonNetwork.LocalPlayer.ActorNumber);

        // 격추한 플레이어인가?
        if (lastHitObjOwner != PhotonNetwork.LocalPlayer.ActorNumber) return;
        
        // 점수 증가
        int i = GetComponent<EnemyInfo>().point;
        ScoreManager.Instance.AddScore(i);

        // 오브젝트 삭제
        photonView.RPC(nameof(NetDestory), RpcTarget.AllBuffered);
    }

    [PunRPC]
    void NetDestory()
    { 
        if(photonView.IsMine) PhotonNetwork.Destroy(photonView);
    }
}
