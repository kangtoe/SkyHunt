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
        //rbody = GetComponent<Rigidbody2D>();

        onDeadLocal.AddListener(delegate {
            //Debug.Log("onDeadLocal");

            isDestoryedLocal = true;
            gameObject.SetActive(false);
            SoundManager.Instance.PlaySound("Explosion");            

            if (diePrefab)
            {
                string str = "Projectiles/" + diePrefab.name;
                PhotonNetwork.Instantiate(str, transform.position, diePrefab.transform.rotation);
            }
        });

        onDeadGlobal.AddListener(delegate {
            //Debug.Log("onDeadLocal");

            PhotonNetwork.Destroy(photonView);            
        });
    }

    virtual public void GetDamaged(float damage, int hitObjOwner)
    {
        if (!photonView.IsMine) return;

        lastHitObjOwner = hitObjOwner;
        //Debug.Log(name + " : GetDamaged = " + damage + "( by : player " + hitObjOwner + ")");         

        currnetHealth -= damage;
        if (currnetHealth < 0) currnetHealth = 0;
        if (currnetHealth == 0) Die();
    }

    virtual protected void Die()
    {
        Debug.Log("die");

        //if (!PhotonNetwork.IsMasterClient) return;      

        if (!isDestoryedLocal) onDeadLocal.Invoke();          
        if (photonView.IsMine) onDeadGlobal.Invoke();
    }
}
