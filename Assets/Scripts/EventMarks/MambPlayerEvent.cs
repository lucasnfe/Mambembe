using UnityEngine;
using System.Collections;

public class MambPlayerEvent : MambStoryEvent {

	public override void TriggerExitEvent() {

		GameWorld.Instance.Player.ConsumeEnergy (5);
		Invoke ("ReloadScene", 1f);
	}

	private void ReloadScene() {

		GameData.PlayerFood = 100;
		GameData.PlayerEnergy = 100;
			
		HUD.Instance.ReloadScene ();
	}
}
