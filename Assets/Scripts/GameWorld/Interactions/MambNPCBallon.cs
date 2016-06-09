using UnityEngine;
using System.Collections;

public class MambNPCBallon : MambInteractBallon {

	public string talkText;

	public override void DrawInteractionOptions(float startX, float startY) {

		if (GUI.Button (new Rect (startX, startY + 20, 60, 15), "Talk", guiSkin.button)) {

			HUD.Instance.storyDisplay.SetText (talkText);
			HUD.Instance.storyDisplay.Click ();

			isInteractionWindowOpen = false;
		}
	}
}
