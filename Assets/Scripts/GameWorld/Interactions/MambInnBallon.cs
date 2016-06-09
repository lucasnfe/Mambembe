using UnityEngine;
using System.Collections;

public class MambInnBallon : MambInteractBallon {

	public override void DrawInteractionOptions(float startX, float startY) {

		//Render the text
		if(!GameData.PayedInn) {
			
			if (GUI.Button (new Rect (startX, startY + 20, 60, 15), "Pay 10", guiSkin.button)) {

				Debug.Log ("Pay!");

				if (GameWorld.Instance.Player.gold >= 10) {

					GameData.PayedInn = true;
					GameWorld.Instance.Player.ConsumeGold (10);
					GameData.SaveGameState ();

					HUD.Instance.ReloadScene ();
				}
				else
					HUD.Instance.Log ("Jack needs 10 gold coins to pay its stay.");
			}
		}
	}
}
