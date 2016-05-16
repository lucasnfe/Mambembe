using UnityEngine;
using System.Collections;

public abstract class MambEventMarker : MonoBehaviour {


	void OnTriggerEnter(Collider other) {

		TriggerEnterEvent ();
		HUD.Instance.storyDisplay.Click ();
	}

	void OnTriggerExit(Collider other) {

		TriggerExitEvent ();
		HUD.Instance.storyDisplay.Click ();
	}

	public abstract void TriggerEnterEvent();

	public abstract void TriggerExitEvent();
}