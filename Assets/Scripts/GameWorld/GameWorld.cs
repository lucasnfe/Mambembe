using UnityEngine;
using System.Collections;

public class GameWorld : MambSingleton<GameWorld> {

	public MambPlayer Player { get; set; }
	public MambCamera Camera { get; set; }

	// Use this for initialization
	void Awake() {
	
		Player = FindObjectOfType<MambPlayer> ();
		Camera = FindObjectOfType<MambCamera> ();
	}
}
