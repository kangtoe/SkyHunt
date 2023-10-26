using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DroneSevant : MonoBehaviourPun
{    
    public void DelayDestory_RPC(float f)
    {
        Vector2 force = Random.insideUnitCircle; // 임의의 힘
        photonView.RPC(nameof(DelayDestory), RpcTarget.AllBuffered, f, force);
    }

    [PunRPC]
    void DelayDestory(float f, Vector2 force)
    {
        StartCoroutine(DelayDestoryCr(f, force));
    }

    IEnumerator DelayDestoryCr(float f, Vector2 force)
    {
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(f);

        // 즉사에 해당하는 피해
        GetComponent<Damageable>().GetDamaged(10000, photonView.OwnerActorNr);
    }
}
