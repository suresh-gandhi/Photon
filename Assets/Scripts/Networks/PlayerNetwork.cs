using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork Instance;

    public string PlayerName { get; private set; }

	// Use this for initialization
	private void Awake () {
        Instance = this;
        PlayerName = "Summi#" + Random.Range(1000, 9999);             // This is used to identify ourselves once we hook up the lobby code.
	}
	
}
