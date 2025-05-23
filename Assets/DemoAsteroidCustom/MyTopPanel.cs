using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MyTopPanel : MonoBehaviour
{
    private readonly string connectionStatusMessage = "Status: ";

    [Header("UI References")]
    public Text ConnectionStatusText;

    #region UNITY

    public void Update()
    {
        ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
    }

    #endregion

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}