using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FindTarget : MonoBehaviourPun
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        StartCoroutine(UpdateTargetCr());
    }

    IEnumerator UpdateTargetCr()
    {
        while (true)
        {
            // 타겟 갱신
            Transform tf = PlayerSpwaner.Instance.GetClosestPlayer(transform.position);
            if (tf != null)
            {
                int id = tf.GetComponent<PhotonView>().ViewID;
                UpdateTarget(id);
                photonView.RPC(nameof(UpdateTarget), RpcTarget.OthersBuffered, id);
            }            

            // 대기
            yield return new WaitForSeconds(1);
        }
        
    }

    [PunRPC]
    // Update is called once per frame
    void UpdateTarget(int id)
    {
        //Debug.Log("UpdateTarget");
        target = PhotonView.Find(id).transform;
    }
}
