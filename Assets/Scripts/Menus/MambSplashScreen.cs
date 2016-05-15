using UnityEngine;
using System.Collections;

public class MambSplashScreen : MonoBehaviour {

	public Texture2D  defaultCursor;
	private MambFadeScene sceneFader;

	void Awake() {

		sceneFader = GetComponent<MambFadeScene> ();
	}

	// Use this for initialization
	void Start () {

		Invoke ("LoadMainMenu", 1f);
	}

	void LoadMainMenu() {

		Cursor.SetCursor (defaultCursor, Vector2.zero, CursorMode.Auto);
		sceneFader.StartFadeOut ("MainMenu");
	}
}
