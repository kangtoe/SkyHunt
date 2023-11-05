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
    protected float currnetHealth;
    //Rigidbody2D rbody;        

    [HideInInspector]
    public UnityEvent onDeadGlobal;
    [HideInInspector]
    public UnityEvent onDeadLocal;

    [HideInInspector]
    public int lastHitObjOwner;

    // Start is called before the first frame update
    void Start()
    {
        currnetHealth = maxHealth;
        //rbody = GetComponent<Rigidbody2D>();

        onDeadLocal.AddListener(delegate { Debug.Log("onDeadLocal"); });        
    }

    virtual public void GetDamaged(float damage, int hitObjOwner)
    {
        lastHitObjOwner = hitObjOwner;
        //Debug.Log(name + " : GetDamaged = " + damage + "( by : player " + hitObjOwner + ")");

        if (!PhotonNetwork.IsMasterClient)
        {
            // TODO :  마스터 클라이언트와 체력 동기화
            //return;
        }                

        currnetHealth -= damage;
        if (currnetHealth < 0) currnetHealth = 0;
        if (currnetHealth == 0) Die();
    }

    virtual protected void Die()
    {
        onDeadLocal.Invoke();
        if (!PhotonNetwork.IsMasterClient) return;
        //if (!photonView.IsMine) return;

        if (diePrefab)
        {
            string str = "Projectiles/" + diePrefab.name;
            PhotonNetwork.Instantiate(str, transform.position, diePrefab.transform.rotation);
        }

        SoundManager.Instance.PlaySound("Explosion");
        onDeadGlobal.Invoke();
        PhotonNetwork.Destroy(photonView);        
    }
}
