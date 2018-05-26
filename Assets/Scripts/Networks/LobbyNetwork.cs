using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviour
{
    private void Start()
    {
        print("Connecting to server..");
        PhotonNetwork.ConnectUsingSettings("0.0.0");
    }

    private void OnConnectedToMaster() {
        print("Connected to the master");
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby(){
        print("Joined lobby.");
        if (!PhotonNetwork.inRoom) {                                // This would prevent to having weird issues basically.
            MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();                        // Whenever we are going to join the lobby it is going to show the lobby panel over the top.
        }

    }

}
