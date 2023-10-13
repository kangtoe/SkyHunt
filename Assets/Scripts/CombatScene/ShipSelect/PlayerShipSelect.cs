using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerShipSelect : MonoBehaviourPunCallbacks
{
    [SerializeField]
    List<GameObject> SelectableShips;
    
    [Space]
    [SerializeField]
    Text Player1Text;
    [SerializeField]
    Text Player2Text;

    [Space]
    [SerializeField]
    List<Button> Player1BtnList;
    [SerializeField]
    List<Button> Player2BtnList;

    [Space]
    [SerializeField]
    Color nomalColor;
    [SerializeField]
    Color selectedColor;

    int player1SelectIdx = -1;
    int player2SelectIdx = -1;

    // Start is called before the first frame update
    void Start()
    {
        // 텍스트 초기화
        {
            string myTxt = "My Ship";
            string otherTxt = "Other Ship";

            if (PhotonNetwork.IsMasterClient)
            {
                Player1Text.text = myTxt;
                Player2Text.text = otherTxt;
            }
            else
            {
                Player1Text.text = otherTxt;
                Player2Text.text = myTxt;
            }
        }

        // 버튼 스프라이트 초기화
        {
            for (int i = 0; i < Player1BtnList.Count; i++)
            {
                Player1BtnList[i].transform.GetChild(0).GetComponent<Image>().sprite
                = SelectableShips[i].GetComponent<SpriteRenderer>().sprite;
            }
            for (int i = 0; i < Player2BtnList.Count; i++)
            {
                Player2BtnList[i].transform.GetChild(0).GetComponent<Image>().sprite
                = SelectableShips[i].GetComponent<SpriteRenderer>().sprite;
            }
        }

        // 버튼 이벤트 초기화
        {            
            if (PhotonNetwork.IsMasterClient) // player1의 경우
            {
                // Player1 버튼 초기화
                for (int i = 0; i < Player1BtnList.Count; i++)
                {
                    int tmp = i;
                    Player1BtnList[i].onClick.AddListener(delegate
                    {
                        photonView.RPC(nameof(SelectPlayer), RpcTarget.AllBuffered, tmp);
                    });
                }

                // Player2 버튼 초기화
                for (int i = 0; i < Player2BtnList.Count; i++)
                {
                    Player2BtnList[i].onClick.AddListener(delegate
                    {
                        photonView.RPC(nameof(SelectPlayer), RpcTarget.AllBuffered, -1);
                    });
                }
            }
            else // player2의 경우
            {
                // Player1 버튼 초기화
                for (int i = 0; i < Player1BtnList.Count; i++)
                {
                    Player1BtnList[i].onClick.AddListener(delegate
                    {
                        photonView.RPC(nameof(SelectPlayer), RpcTarget.AllBuffered, -1);
                    });
                }

                // Player2 버튼 초기화
                for (int i = 0; i < Player2BtnList.Count; i++)
                {
                    int tmp = i;
                    Player2BtnList[i].onClick.AddListener(delegate
                    {
                        photonView.RPC(nameof(SelectPlayer), RpcTarget.AllBuffered, tmp);                        
                    });
                }
            }
        }

        // 선택 표시 초기화
        {
            BtnOutlineActive(1, -1);
            BtnColorSelect(1, -1);
            BtnOutlineActive(2, -1);
            BtnColorSelect(2, -1);
        }

        // 호스트 정보 받아오기
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(Init), RpcTarget.AllBuffered);                
            }
        }
    }

    [PunRPC]
    void Init()
    {
        Debug.Log("Init");

        // 모든 플레이어에게 -> 선택 버튼 동기화
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(ApplySelect), RpcTarget.AllBuffered, 1, player1SelectIdx);
        }
    }

    // 
    [PunRPC]
    void ApplySelect(int playerNum, int SelectIdx)
    {
        Debug.Log("ApplySelect ||" + "playerNum : " + playerNum + "||" + "SelectIdx : " + SelectIdx);

        if (SelectIdx == 1)
        {
            player1SelectIdx = SelectIdx;
        }
        else if (SelectIdx == 2)
        {
            player2SelectIdx = SelectIdx;
        }
        else
        {
            Debug.Log("error : " + playerNum);
        }

        BtnOutlineActive(1, player1SelectIdx);
        BtnOutlineActive(2, player2SelectIdx);
        
        BtnColorSelect(1, player1SelectIdx, player2SelectIdx);
        BtnColorSelect(2, player1SelectIdx, player2SelectIdx);
    }

    // 플레이어 쉽 선택 버튼 클릭 처리
    [PunRPC]
    public void SelectPlayer(int shipIdx)
    {
        Debug.Log("SelectPlayer || isMas : " + PhotonNetwork.IsMasterClient + " || shipIdx : " + shipIdx);

        // 선택 불가
        if (shipIdx == -1) return;

        // 중복 검사
        if (shipIdx == player1SelectIdx || shipIdx == player2SelectIdx) return;

        // 선택 처리
        int playerNum;        
        if (PhotonNetwork.IsMasterClient)
        {
            player1SelectIdx = shipIdx;
            playerNum = 1;
        }        
        else
        {
            player2SelectIdx = shipIdx;
            playerNum = 2;
        }

        photonView.RPC(nameof(ApplySelect), RpcTarget.AllBuffered, playerNum, player1SelectIdx);
    }

    // playerNum에 해당하는 버튼 리스트 구하기
    List<Button> GetBtnList(int playerNum)
    {
        if (playerNum == 1) return Player1BtnList;
        else if (playerNum == 2) return Player2BtnList;
        else
        {
            Debug.Log("playerNum error");
            return null;
        }
    }

    // 선택된 버튼 아웃라인 활성화
    void BtnOutlineActive(int playerNum, int btnIdx)
    {
        Button btn;
        bool active;
        List<Button> btnList = GetBtnList(playerNum);        

        for (int i = 0; i < btnList.Count; i++)
        {                        
            if (btnIdx == i) active = true;
            else active = false;

            btn = btnList[i];
            btn.GetComponent<Outline>().enabled = active;
        }        
    }

    // 선택된 버튼 색 변경
    void BtnColorSelect(int playerNum, int btnIdx)
    {
        Button btn;
        Color color;
        List<Button> btnList = GetBtnList(playerNum);        

        for (int i = 0; i < btnList.Count; i++)
        {
            if (btnIdx == i) color = selectedColor;
            else color = nomalColor;

            btn = btnList[i];
            btn.GetComponent<Image>().color = color;
        }
    }
    void BtnColorSelect(int playerNum, int btnIdx1, int btnIdx2)
    {
        Button btn;
        Color color;
        List<Button> btnList = GetBtnList(playerNum);

        for (int i = 0; i < btnList.Count; i++)
        {
            if (btnIdx1 == i) color = selectedColor;
            else if (btnIdx2 == i) color = selectedColor;
            else color = nomalColor;

            btn = btnList[i];
            btn.GetComponent<Image>().color = color;
        }
    }
}
