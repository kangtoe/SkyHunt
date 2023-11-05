using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 오브젝트가 화면 밖으로 벗어났을 때,
// 반대편 (오른쪽 <-> 왼쪽 / 위쪽 <-> 아래쪽) 가장자리 이동
public class JumpShip : MonoBehaviourPun
{
    // player ship 사이즈를 알아오기 위해 사용
    Collider2D ShipCollider;
    TrailEffect effect;
    
    Vector2 cameraSize;
    Vector2 shipSize;

    float lastJumpTime = 0;
    float jumpMinInterval = 0.5f; // 한번 가장자리 이동이 실행되면, 직후 이 기간동안은 다시 가장자리 이동체크를 중단

    // Start is called before the first frame update
    void Start()
    {       
        ShipCollider = GetComponent<Collider2D>();
        effect = GetComponentInChildren<TrailEffect>();

        cameraSize = GetCameraSize();
        shipSize = GetPlayerShipBoundSize();

        float interval = 0.1f;
        InvokeRepeating(nameof(JumpToOppsiteCheck), 0, interval);
    }

    Vector2 GetPlayerShipBoundSize()
    {
        return ShipCollider.bounds.size;        
    }

    Vector2 GetCameraSize()
    {
        float cameraSizeX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x * 2;
        float cameraSizeY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y * 2;
        return new Vector2(cameraSizeX, cameraSizeY);
    }



    // 화면에서 벗어난 가장자리 방향에 따라 반대편 가장자리로 이동 
    void JumpToOppsiteCheck()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (Time.time < lastJumpTime + jumpMinInterval) return;

        float moveX = cameraSize.x / 2 + shipSize.x;
        float moveY = cameraSize.y / 2 + shipSize.y;
        Vector3 pos = transform.position;

        // x축
        if (transform.position.x < -moveX)
        {
            if (effect) effect.TrailDistachRPC();

            photonView.RPC(nameof(SetPos), RpcTarget.AllBuffered, moveX, pos.y);
            //transform.position = new Vector3(moveX, pos.y, pos.z);
            
            if (effect) effect.TrailAttachRPC();
            AddForceToOppsite(Edge.Right);
            lastJumpTime = Time.time;

            //Debug.Log("move left -> right");
        }
        else if (transform.position.x > moveX)
        {
            if (effect) effect.TrailDistachRPC();

            photonView.RPC(nameof(SetPos), RpcTarget.AllBuffered, -moveX, pos.y);
            //transform.position = new Vector3(-moveX, pos.y, pos.z);

            if (effect) effect.TrailAttachRPC();
            AddForceToOppsite(Edge.Left);
            lastJumpTime = Time.time;

            //Debug.Log("move right -> left");
        }

        // y축
        if (transform.position.y < -moveY)
        {
            if (effect) effect.TrailDistachRPC();

            photonView.RPC(nameof(SetPos), RpcTarget.AllBuffered, pos.x, moveY);
            //transform.position = new Vector3(pos.x, moveY, pos.z);
            
            if (effect) effect.TrailAttachRPC();
            AddForceToOppsite(Edge.Up);
            lastJumpTime = Time.time;

            //Debug.Log("move down -> up");
        }
        else if (transform.position.y > moveY)
        {
            if (effect) effect.TrailDistachRPC();

            photonView.RPC(nameof(SetPos), RpcTarget.AllBuffered, pos.x, -moveY);
            //transform.position = new Vector3(pos.x, -moveY, pos.z);

            if (effect) effect.TrailAttachRPC();
            AddForceToOppsite(Edge.Down);
            lastJumpTime = Time.time;

            //Debug.Log("move up -> down");
        }
    }

    // 가장 자리 이동 후 반대편으로 약간 밀어준다
    // -> 점프 직후 속력이 충분하지 못해 다시 점프가 일어나는 현상을 줄여준다
    void AddForceToOppsite(Edge jumpedEdge)
    {
        //Debug.Log("AddForceToCenter");

        Vector2 dir = Vector2.zero;
        if (jumpedEdge == Edge.Down) dir = Vector2.up;
        if (jumpedEdge == Edge.Up) dir = Vector2.down;
        if (jumpedEdge == Edge.Right) dir = Vector2.left;
        if (jumpedEdge == Edge.Left) dir = Vector2.right;

        float movePower = 3f;
        GetComponent<Rigidbody2D>().AddForce(dir * movePower, ForceMode2D.Impulse);
    }

    [PunRPC]
    void SetPos(float x, float y)
    {
        transform.position = new Vector2(x, y);
    }
}
