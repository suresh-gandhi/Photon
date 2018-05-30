using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicArea : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        // print("OnTriggerEnter first line");
        if (!PhotonNetwork.isMasterClient)              // This would be commented if we do it otherwise
        {
            // print("Returned successfully");
            return;                 
        }

        // print("This function is being called inside the master client");

        PhotonView photonView = other.GetComponent<PhotonView>();       // If it needs to sync up with the server it should have a photon view component.
        if(photonView != null /*&& photonView.isMine*/) {                                        // If there is an object not synced up basically then photon view would be null and we have to prevent that then.
            PlayerManagement.Instance.ModifyHealth(photonView.owner, -10);   
        }
    }

}
