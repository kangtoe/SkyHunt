using Photon.Pun;
using UnityEngine;

public class Ball : MonoBehaviourPun
{
    // 네트워크 호스트 && 해당 오브젝트는 이 로컬에서 생성됨
    public bool IsMasterClientLocal => PhotonNetwork.IsMasterClient && photonView.IsMine;

    private Vector2 direction = Vector2.right;
    private readonly float speed = 10f;
    private readonly float randomRefectionIntensity = 0.1f; // 입사/반사각 무작위화
    
    private void FixedUpdate()
    {
        // 호스트의 오브젝트가 아닌 경우
        if (!IsMasterClientLocal || PhotonNetwork.PlayerList.Length < 2) return;

        // 볼 움직임 제어
        {
            var distance = speed * Time.deltaTime;
            var hit = Physics2D.Raycast(transform.position, direction, distance);
           
            if (hit.collider != null) // 충돌시
            {
                // 표면 반사 처리
                direction = Vector2.Reflect(direction, hit.normal); 
                direction += Random.insideUnitCircle * randomRefectionIntensity; // 방향 오차 추가

                // 골대 충돌 시 점수 추가
                var goalpost = hit.collider.GetComponent<Goalpost>();
                if (goalpost != null)
                {
                    //GameManager.Instance.AddScore(goalpost.playerNumber, 1);
                }
            }

            transform.position = (Vector2)transform.position + direction * distance;
        }
    }
}