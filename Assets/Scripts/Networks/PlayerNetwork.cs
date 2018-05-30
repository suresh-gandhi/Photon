using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork Instance;

    public string PlayerName { get; private set; }

    private PhotonView photonView;

    private int PlayersInGame = 0;

	// Use this for initialization
	private void Awake () {
        Instance = this; 

        photonView = GetComponent<PhotonView>();

        PlayerName = "Summi#" + Random.Range(1000, 9999);             // This is used to identify ourselves once we hook up the lobby code.

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;             // Creates a delegate basically. Whenever the scene loading has occured then it will call the method given below basically.
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {          // The information about the scene that loaded in and the information about how it got loaded in.
        if (scene.name == "Game") {
            if (PhotonNetwork.isMasterClient)
            {
                MasterLoadedGame();
            }
            else {
                NonMasterLoadedGame();
            }
        }               
    }

    private void MasterLoadedGame() {
        photonView.RPC("RPC_loadedGameScene", PhotonTargets.MasterClient);          // If another player doesn't join this would cure the bug which the previous code engenders 
        photonView.RPC("RPC_loadGameOthers", PhotonTargets.Others);                 // RPC is the method of the photon view component and what is does is that it lets us broadcast the message to various targets(others, master client or all)
    }

    private void NonMasterLoadedGame()
    {
        photonView.RPC("RPC_loadedGameScene", PhotonTargets.MasterClient);          // We only want to call this method on the master "client"
    }
        
    [PunRPC]                                                                    // In order to have a method be capable of contacting other players or other clients it has to have this at the top. If we do not have that it wont work. Putting RPC in front of the method is not mandatory although it is in some type of networking.
    private void RPC_loadGameOthers() {
        PhotonNetwork.LoadLevel(1);                                             // So basically we are telling all the other clients to load level no 1 whenever the master loads into the game level.
    }

    [PunRPC]
    private void RPC_loadedGameScene() {                        // This method tells the master only that how many players are in the game and how many should be in the game and when they are in the right amount it will start the game.
        PlayersInGame++;
        if (PlayersInGame == PhotonNetwork.playerList.Length) {         // If the number of players in the game are equal to the length of the player list
            print("All players are in the game scene.");
            photonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }        
    }

    [PunRPC]
    private void RPC_CreatePlayer(){
        float randomValue = Random.Range(0f, 5f);
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NewPlayer"), Vector3.up * randomValue, Quaternion.identity, 0);
    }

}
