using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ������Ʈ�� ȭ�� ������ ����� ��,
// �ݴ��� (������ <-> ���� / ���� <-> �Ʒ���) �����ڸ� �̵�
public class JumpShip : MonoBehaviourPun
{
    // player ship ����� �˾ƿ��� ���� ���
    Collider2D ShipCollider;
    TrailEffect effect;
    
    Vector2 cameraSize;
    Vector2 shipSize;

    float lastJumpTime = 0;
    float jumpMinInterval = 0.5f; // �ѹ� �����ڸ� �̵��� ����Ǹ�, ���� �� �Ⱓ������ �ٽ� �����ڸ� �̵�üũ�� �ߴ�

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;

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



    // ȭ�鿡�� ��� �����ڸ� ���⿡ ���� �ݴ��� �����ڸ��� �̵� 
    void JumpToOppsiteCheck()
    {
        if (Time.time < lastJumpTime + jumpMinInterval) return;

        float moveX = cameraSize.x / 2 + shipSize.x;
        float moveY = cameraSize.y / 2 + shipSize.y;
        Vector3 pos = transform.position;

        // x��
        if (transform.position.x < -moveX)
        {
            if (effect) effect.TrailDistach();

            photonView.RPC(nameof(SetPos), RpcTarget.AllBuffered, moveX, pos.y);
            //transform.position = new Vector3(moveX, pos.y, pos.z);
            
            if (effect) effect.TrailAttach();
            AddForceToOppsite(Edge.Right);
            lastJumpTime = Time.time;

            //Debug.Log("move left -> right");
        }
        else if (transform.position.x > moveX)
        {
            if (effect) effect.TrailDistach();

            photonView.RPC(nameof(SetPos), RpcTarget.AllBuffered, -moveX, pos.y);
            //transform.position = new Vector3(-moveX, pos.y, pos.z);

            if (effect) effect.TrailAttach();
            AddForceToOppsite(Edge.Left);
            lastJumpTime = Time.time;

            //Debug.Log("move right -> left");
        }

        // y��
        if (transform.position.y < -moveY)
        {
            if (effect) effect.TrailDistach();

            photonView.RPC(nameof(SetPos), RpcTarget.AllBuffered, pos.x, moveY);
            //transform.position = new Vector3(pos.x, moveY, pos.z);
            
            if (effect) effect.TrailAttach();
            AddForceToOppsite(Edge.Up);
            lastJumpTime = Time.time;

            //Debug.Log("move down -> up");
        }
        else if (transform.position.y > moveY)
        {
            if (effect) effect.TrailDistach();

            photonView.RPC(nameof(SetPos), RpcTarget.AllBuffered, pos.x, -moveY);
            //transform.position = new Vector3(pos.x, -moveY, pos.z);

            if (effect) effect.TrailAttach();
            AddForceToOppsite(Edge.Down);
            lastJumpTime = Time.time;

            //Debug.Log("move up -> down");
        }
    }

    // ���� �ڸ� �̵� �� �ݴ������� �ణ �о��ش�
    // -> ���� ���� �ӷ��� ������� ���� �ٽ� ������ �Ͼ�� ������ �ٿ��ش�
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
