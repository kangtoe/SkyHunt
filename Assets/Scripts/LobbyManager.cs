using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks // PunCallback (포톤 pun 콜백) 감지
{
    private readonly string gameVersion = "0.01";

    public Text connectionInfoText;
    public Button joinButton;
    
    private void Start()
    {
        joinButton.interactable = false;
        connectionInfoText.text = "마스터 서버에 접속중...";

        // 접속에 필요한 정보(게임 버전) 설정
        PhotonNetwork.GameVersion = gameVersion;

        // 설정한 정보를 가지고 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();                
    }
    
    // 마스터 서버 접속 시
    public override void OnConnectedToMaster()
    {
        SetReadyUi(true);
    }
    
    // 서버 접속 해제됨 -> 재접속 시도
    public override void OnDisconnected(DisconnectCause cause)
    {
        SetReadyUi(false);

        // 재접속 시도
        PhotonNetwork.ConnectUsingSettings();                                
    }

    // 연결 되어 있는 지 ui 표시
    void SetReadyUi(bool ready)
    {
        joinButton.interactable = ready;
        if(ready) connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
        else connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";
    }
    
    // join 버튼 클릭시 접속 시도-> 버튼 이벤트로 호출
    public void Connect()
    {
        // 중복 접속 시도 방지
        joinButton.interactable = false;
        
        if (PhotonNetwork.IsConnected) // 마스터 서버에 접속중
        {
            connectionInfoText.text = "룸에 접속...";

            // 룸 접속 실행
            PhotonNetwork.JoinRandomRoom();                        
        }
        else // 마스터 서버에 접속중이 아니라면
        {
            SetReadyUi(false);

            // 마스터 서버에 접속 시도
            PhotonNetwork.ConnectUsingSettings();                             
        }
    }

    // 포톤 이벤트 메소드 -> 랜덤 룸 참가에 실패한 경우 (빈 방 없음)
    public override void OnJoinRandomFailed(short returnCode, string message)
    {        
        connectionInfoText.text = "빈 방이 없음, 새로운 방 생성...";
        // 최대 2명을 수용 가능한 빈방을 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }
    
    public override void OnJoinedRoom()
    {        
        connectionInfoText.text = "방 참가 성공";

        // 모든 룸 참가자 Main 씬 로드
        PhotonNetwork.LoadLevel("Combat");
    }
}