using UnityEngine;
using System.Collections;

public class MambTavernBallon : MambInteractBallon {

	public override void DrawInteractionOptions(float startX, float startY) {

		//Render the text
		if (GUI.Button (new Rect (startX, startY + 20, 60, 15), "Drink 5", guiSkin.button)) {

			Debug.Log ("Drink!");

			if (!GameData.PayedInn) {
				HUD.Instance.Log ("To drink here Jack must pay the Inn.");
				return;
			}

			if (GameWorld.Instance.Player.gold >= 5) {

				GameWorld.Instance.Player.ConsumeGold (5);
				GameData.SaveGameState ();

				HUD.Instance.ReloadScene ();
			}
			else
				HUD.Instance.Log ("Jack needs 5 gold coins to drink in the Tabern.");
		}
	}
}
