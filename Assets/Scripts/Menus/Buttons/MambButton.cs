using UnityEngine;
using System.Collections;

public class MambButton : MonoBehaviour {

	// Use this for initialization
	public void LoadScene (string sceneName) {

		MambSceneManager.Instance.LoadScene (sceneName, false, ActionBeween, ActionAfter);
	}

	protected virtual void ActionBeween () {
		
	}

	protected virtual void ActionAfter () {

	}
}