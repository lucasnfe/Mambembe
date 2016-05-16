using UnityEngine;
using System.Collections;

public class MambStoryEvent : MambEventMarker {

	public TextAsset m_text;

	public override void TriggerEnterEvent() {

		HUD.Instance.storyDisplay.SetText (m_text.text);
	}

	public override void TriggerExitEvent() {

		GameWorld.Instance.Player.ConsumeEnergy (5);
	}
}
