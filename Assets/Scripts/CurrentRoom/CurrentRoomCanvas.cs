using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour {

    public void OnClickStartSync() {
        // If it is not the master client then simply return as we don't have the authority now.
        if (!PhotonNetwork.isMasterClient) {
            return;
        }

        PhotonNetwork.LoadLevel(1);
    }

    public void OnClickStartDelayed() {
        // If it is not the master client then simply return as we don't have the authority now.
        if (!PhotonNetwork.isMasterClient) {
            return;
        }

        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;
        PhotonNetwork.LoadLevel(1);
    }

}
