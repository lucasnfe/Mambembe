using UnityEngine;
using System.Collections;

public class GameWorld : MambSingleton<GameWorld> {

	public MambPlayer Player { get; set; }
	public MambCamera Camera { get; set; }

	public AudioSource BackgroundSource { get; set; }
	public AudioSource ForegroundSource { get; set; }

	public AudioClip backgroundAudio; 
	public AudioClip foregroundAudio; 

	// Use this for initialization
	void Awake() {
	
		Player = FindObjectOfType<MambPlayer> ();
		Camera = FindObjectOfType<MambCamera> ();

		BackgroundSource = gameObject.AddComponent<AudioSource> ();
		ForegroundSource = gameObject.AddComponent<AudioSource> ();
	}

	void Start() {

		BackgroundSource.clip = backgroundAudio;
		BackgroundSource.loop = true;
		BackgroundSource.volume = 0.5f;
		BackgroundSource.Play ();

		ForegroundSource.clip = foregroundAudio;
		ForegroundSource.loop = true;
		ForegroundSource.volume = 0.5f;
		ForegroundSource.Play ();
	}
}
