using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveCurrentMatch : MonoBehaviour {

    public void OnClick_LeaveMatch() {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);             // We are essentially telling the photon to load the lobby here basically.
    }

}
