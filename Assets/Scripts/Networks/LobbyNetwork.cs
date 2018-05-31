using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviour
{
    private void Start()
    {
        if (!PhotonNetwork.connected) {
            print("Connecting to server..");
            PhotonNetwork.ConnectUsingSettings("0.0.0");
        }
    }

    // Called by photon
    private void OnConnectedToMaster() {
        print("Connected to the master");
        PhotonNetwork.automaticallySyncScene = false;               // If this is true whenever we join a room it will automatically sync us to whichever scene that the master client is currently on.
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    // Called by photon
    private void OnJoinedLobby(){
        print("Joined lobby.");
        if (!PhotonNetwork.inRoom) {                                // This would prevent to having weird issues basically.
            MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();                        // Whenever we are going to join the lobby it is going to show the lobby panel over the top.
        }

    }

}
