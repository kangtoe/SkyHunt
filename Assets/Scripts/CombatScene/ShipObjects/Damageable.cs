using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Damageable : MonoBehaviourPun
{
    public GameObject diePrefab;
    public GameObject bumpEffect; // 충돌 시 Impactable에서 호출    
    public float maxHealth = 100;    
    protected float currnetHealth;
    //Rigidbody2D rbody;

    // Start is called before the first frame update
    void Start()
    {
        currnetHealth = maxHealth;
        //rbody = GetComponent<Rigidbody2D>();
    }

    virtual public void GetDamaged(float damage)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Debug.Log(name + " : GetDamaged = " + damage);

        currnetHealth -= damage;
        if (currnetHealth < 0) currnetHealth = 0;
        if (currnetHealth == 0) Die();
    }

    virtual protected void Die()
    {
        if (!PhotonNetwork.IsMasterClient) return;
       
        if (diePrefab)
        {
            string str = "Projectiles/" + diePrefab.name;
            PhotonNetwork.Instantiate(str, transform.position, diePrefab.transform.rotation);
        }
        
        PhotonNetwork.Destroy(photonView);        
    }
}
