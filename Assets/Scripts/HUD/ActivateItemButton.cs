using UnityEngine;
using System.Collections;

public class ActivateItemButton : MonoBehaviour {

	private bool isItemActive;
		
	public MambItem item;
	
	// Update is called once per frame
	public void Click () {

		if (item == null)
			return;

		if (GameWorld.Instance.Camera.IsFocusing ())
			return;
	
		if (!isItemActive) {

			isItemActive = true;

			item.gameObject.SetActive (true);
			item.ActivateItem ();
		} 
		else {

			isItemActive = false;

			item.gameObject.SetActive (false);
			item.DeactivateItem ();
		}
	}
}
