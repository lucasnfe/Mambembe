using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

	public static uint PlayerEnergy = MambConstants.PLAYER_LOW_ENERGY;
	public static uint PlayerFood   = MambConstants.PLAYER_LOW_ENERGY * 3;
	public static uint PlayerGold   = 0;

	public static bool PayedInn = false;

	public static void SaveGameState() {

		PlayerEnergy = GameWorld.Instance.Player.energy;
		PlayerFood = GameWorld.Instance.Player.food;
		PlayerGold = GameWorld.Instance.Player.gold;
	}
}
