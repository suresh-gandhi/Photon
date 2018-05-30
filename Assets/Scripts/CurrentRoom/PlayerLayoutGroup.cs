using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayoutGroup : MonoBehaviour {

    [SerializeField]
    private GameObject _playerListingPrefab;
    private GameObject PlayerListingPrefab {
        get { return _playerListingPrefab; }
    }

    private List<PlayerListing> _playerListings = new List<PlayerListing>();
    private List<PlayerListing> PlayerListings {
        get { return _playerListings; }
    }

    // Called by photon whenever the master leaves and this is called on all players
    private void OnMasterClientSwitched(PhotonPlayer newMasterClient) {
        PhotonNetwork.LeaveRoom();
    }

    // Called by photon whenever you join a room
    private void OnJoinedRoom() {

        foreach (Transform child in transform)              // This is done to prevent the duplicates.
        {
            Destroy(child.gameObject);
        }

        MainCanvasManager.Instance.CurrentRoomCanvas.transform.SetAsLastSibling();

        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;     // This will get all the list of the current players in the room you resided.
        for (int i = 0; i < photonPlayers.Length; i++) {
            PlayerJoinedRoom(photonPlayers[i]);                      // By default we will be in the existing list when we join it, so it is going to add the all existing players and ourself as well.
        }
    }

    // Called by photon when a player joins the room
    private void OnPhotonPlayerConnected(PhotonPlayer photonPlayer) {
        PlayerJoinedRoom(photonPlayer);
    }

    // Called by photon when a player leaves the room
    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer) {
        PlayerLeftRoom(photonPlayer);
    }

    private void PlayerJoinedRoom(PhotonPlayer photonPlayer) {
        if(photonPlayer == null)                                     // I don't think this would ever happen but better be safe and secured :)
        {
            return;
        }
         
        PlayerLeftRoom(photonPlayer);

        GameObject playerListingObj = Instantiate(PlayerListingPrefab);
        playerListingObj.transform.SetParent(transform, false);

        PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing>();
        playerListing.ApplyPhotonPlayer(photonPlayer);                  // This will set the text on the prefab as well.

        PlayerListings.Add(playerListing);
    }

    private void PlayerLeftRoom(PhotonPlayer photonPlayer) {
        int index = PlayerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if (index != -1) {                                          // If they already exist in our list we are going to destroy the object.
            Destroy(PlayerListings[index].gameObject);
            PlayerListings.RemoveAt(index);
        }
    }

    public void OnClickRoomState() {
            
        if (!PhotonNetwork.isMasterClient) {
            return;
        }

        PhotonNetwork.room.IsOpen = !PhotonNetwork.room.IsOpen;
        PhotonNetwork.room.IsVisible = PhotonNetwork.room.IsOpen;
    }

    public void OnClickLeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }


}
