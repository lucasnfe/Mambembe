using UnityEngine;
using System.Collections;

public class MambPlayerEvent : MambStoryEvent {

	public override void TriggerExitEvent() {

		GameWorld.Instance.Player.ConsumeEnergy (50);
		Invoke ("ReloadScene", 1f);
	}

	private void ReloadScene() {

		GameData.PlayerFood = 100;
		GameData.PlayerEnergy = MambConstants.PLAYER_MAX_ENERGY;
			
		HUD.Instance.ReloadScene ();
	}
}
