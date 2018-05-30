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

    private PlayerMovement CurrentPlayer;

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
        photonView.RPC("RPC_loadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);          // If another player doesn't join this would cure the bug which the previous code engenders 
        photonView.RPC("RPC_loadGameOthers", PhotonTargets.Others);                 // RPC is the method of the photon view component and what is does is that it lets us broadcast the message to various targets(others, master client or all)
    }

    private void NonMasterLoadedGame()
    {
        photonView.RPC("RPC_loadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);          // We only want to call this method on the master "client"
    }
        
    [PunRPC]                                                                    // In order to have a method be capable of contacting other players or other clients it has to have this at the top. If we do not have that it wont work. Putting RPC in front of the method is not mandatory although it is in some type of networking.
    private void RPC_loadGameOthers() {
        PhotonNetwork.LoadLevel(1);                                             // So basically we are telling all the other clients to load level no 1 whenever the master loads into the game level.
    }

    [PunRPC]
    private void RPC_loadedGameScene(PhotonPlayer photonPlayer) {                        // This method tells the master only that how many players are in the game and how many should be in the game and when they are in the right amount it will start the game.
        PlayerManagement.Instance.AddPlayerStats(photonPlayer);

        PlayersInGame++;
        if (PlayersInGame == PhotonNetwork.playerList.Length) {         // If the number of players in the game are equal to the length of the player list
            print("All players are in the game scene.");
            photonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }        
    }

    public void NewHealth(PhotonPlayer photonPlayer, int health) {
        photonView.RPC("RPC_NewHealth", photonPlayer, health);
    }

    [PunRPC]
    private void RPC_NewHealth(int health) {
        if(CurrentPlayer == null) {                 // We only want to perform the destroy operation when there is an object to destroy
            return;
        }

        if (health <= 0)
        {
            PhotonNetwork.Destroy(CurrentPlayer.gameObject);         // So this would call a network destroy which means it will destroy that object and it will be seen as destroyed across all clients including master the owning client that anyone has connected.  
        }
        else {
            CurrentPlayer.Health = health;
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer(){
        float randomValue = Random.Range(0f, 5f);
        GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NewPlayer"), Vector3.up * randomValue, Quaternion.identity, 0);
        CurrentPlayer = obj.GetComponent<PlayerMovement>();                             // Whenever the player is spawned CurrentMovement should be populated with that script from that prefab.
    }

}
