using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Photon.MonoBehaviour {

    private PhotonView PhotonView;

    private Vector3 targetPosition;

    private Quaternion targetRotation;

    public float Health;            // It is public because 1. I can assign it from anywhere 2. We can see it in the inspector

    private void Awake() {
        PhotonView = GetComponent<PhotonView>();
    }

	// Update is called once per frame
	void Update () {
        if (PhotonView.isMine) {                    // If the person trying to control it does indeed own the game object
            CheckInput();
        }
        else
        {
            SmoothMove();
        }
	}

    // build in method of the photon. This is called everytime when we receive a packet for the object whether it is our object or someone else's object. One thing -> it doesn't get called when we are lone on the server. Also it will only be called when we are observing the player movement script. So in order for this method to fire inside the script we must be observing the script.
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting)
        {             // If we are the one who is writing the data
            stream.SendNext(transform.position);                    // Keep care of order here Ok!
            stream.SendNext(transform.rotation);
            // We can do it for the health as well if we follow the approach other way round.
        }
        else {        
            targetPosition = (Vector3)stream.ReceiveNext();         // This will pull the first entry out from the stream
            targetRotation = (Quaternion)stream.ReceiveNext(); 
        }
    }

    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.25f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500 * Time.deltaTime);
    }

    private void CheckInput() {
        float moveSpeed = 100.0f;
        float rotateSpeed = 500.0f;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        transform.position += transform.forward * (vertical * moveSpeed * Time.deltaTime);
        transform.Rotate(new Vector3(0, horizontal * rotateSpeed * Time.deltaTime, 0));
    }
}
