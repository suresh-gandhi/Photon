using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour {

    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    private RoomLayoutGroup RoomLayoutGroup {
        get { return _roomLayoutGroup; }
    }

    public void OnClickJoinRoom(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName))
        {                 // It just tells the photon network that we want to join that room


        }
        else {
            print("Join room failed.");                 // It may fail maybe if we are not connected to the network or maybe if the room is full. There are a variety of reasons. We don't want to know them basically
        }     

    }
}
