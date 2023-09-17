using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerShip : MonoBehaviourPun
{
    public float MovePower = 10f;

    Rigidbody2D rbody;
    TrailEffect trailEffect;
    //FlameEffect flameEffect;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        //trailEffect = GetComponentInChildren<TrailEffect>();

        //flameEffect = GetComponentInChildren<FlameEffect>();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (Input.GetMouseButton(1))
        {
            rbody.AddForce(transform.up * MovePower * rbody.mass);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 속도에 따른 Trail 효과 제어
        //float TrailVelocity = 0.5f; // 트레일 효과를 주기 위한 최소 속도
        //if (rbody.velocity.magnitude <= TrailVelocity) trailEffect.TrailDistach();
        //else trailEffect.TrailAttach();

    }
}
