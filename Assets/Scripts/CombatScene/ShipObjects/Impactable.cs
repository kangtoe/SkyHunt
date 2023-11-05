using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// collider - collider 충돌 시, 부딪힌 대상에게 피해와 힘을 가한다.
public class Impactable : MonoBehaviourPun
{
    public float impactDamage = 10;
    public float impactPower = 10;

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log(name + " : OnCollisionEnter2D to = " + other.gameObject.name);

        if (!PhotonNetwork.IsMasterClient) return;

        Vector2 point = coll.contacts[0].point;        
        int id = coll.gameObject.GetPhotonView().ViewID;
        photonView.RPC(nameof(Impact), RpcTarget.AllBuffered, id, point.x, point.y);
    }

    [PunRPC]
    void Impact(int coll_Id, float pointX, float pointY)
    {
        PhotonView pv = PhotonView.Find(coll_Id);
        if (!pv) return;

        Collider2D coll = pv.gameObject.GetComponent<Collider2D>();

        // 피해주기
        Damageable damageable = coll.transform.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.GetDamaged(impactDamage, photonView.OwnerActorNr);

            // 충돌 효과 프리팹 불러오기
            GameObject bumpEffect = damageable.bumpEffect;
            if (bumpEffect)
            {
                Vector2 vec = new Vector2(pointX, pointY);
                Instantiate(bumpEffect, vec, bumpEffect.transform.rotation);
            }
        }
        // 힘 가하기
        Rigidbody2D rbody = coll.transform.GetComponent<Rigidbody2D>();
        if (rbody)
        {
            Vector2 dir = coll.transform.position - transform.position;
            rbody.AddForce(dir * impactPower, ForceMode2D.Impulse);
        }
    }
}
