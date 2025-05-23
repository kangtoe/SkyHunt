// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmoothSyncMovement.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Utilities, 
// </copyright>
// <summary>
//  Smoothed out movement for network gameobjects
// </summary>                                                                                             
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace Photon.Pun.UtilityScripts
{
    /// <summary>
    /// Smoothed out movement for network gameobjects
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class SmoothSyncMovement : MonoBehaviourPun, IPunObservable
    {
        public float SmoothingDelay = 5;

        public bool canTeleport = true;
        public float teleportPos = 3;

        Rigidbody2D rb;

        public void Awake()
        {
            bool observed = false;
            foreach (Component observedComponent in this.photonView.ObservedComponents)
            {
                if (observedComponent == this)
                {
                    observed = true;
                    break;
                }
            }
            if (!observed)
            {
                Debug.LogWarning(this + " is not observed by this object's photonView! OnPhotonSerializeView() in this class won't be used."); 
            }

            rb = GetComponent<Rigidbody2D>();            
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //We own this player: send the others our data
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(rb.velocity);
            }
            else
            {
                //Network player, receive data
                correctPlayerPos = (Vector3)stream.ReceiveNext();
                correctPlayerRot = (Quaternion)stream.ReceiveNext();
                correctPlayerVel = (Vector2)stream.ReceiveNext();
            }
        }

        private Vector3 correctPlayerPos = Vector3.positiveInfinity; //We lerp towards this        
        private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this
        private Vector2 correctPlayerVel = Vector3.zero; //We lerp towards this        

        public void Update()
        {
            if (!photonView.IsMine)
            {                
                // OnPhotonSerializeView에서 초기화된 적 없는 경우 스킵
                if (float.IsInfinity(correctPlayerPos.x)) return;

                //Update remote player (smooth this, this looks good, at the cost of some accuracy)

                if (canTeleport)
                {
                    // 너무 먼 거리는 보간 없이 즉시 이동
                    float dist = Vector3.Distance(transform.position, correctPlayerPos);
                    if (teleportPos < dist) transform.position = correctPlayerPos;
                    else transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
                }
                else transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * this.SmoothingDelay);

                transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * this.SmoothingDelay);

                rb.velocity = Vector2.Lerp(rb.velocity, correctPlayerVel, Time.deltaTime * this.SmoothingDelay);
            }
        }

    }
}