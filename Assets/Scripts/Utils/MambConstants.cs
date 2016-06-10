using UnityEngine;
using System.Collections;

public enum PlayerState
{
	Idle,
	Playing,
}

public enum NPCState 
{
	Idle,
	ApprochingPlayer,
	Hanging,
	GoingHome,
	Tiping
}

public enum CameraState
{
	Default,
	MovingIn,
	MovingOut,
	OnFocus
}

public enum Item
{
	Ocarina
}

delegate void ActivateItem();

public class MambConstants {

	public static readonly uint PLAYER_LOW_ENERGY = 150;
	public static readonly uint PLAYER_LOW_FOOD = 150;

	public static readonly uint PLAYER_MAX_GOLD   = 100;
	public static readonly uint PLAYER_MAX_FOOD   = 1000;
	public static readonly uint PLAYER_MAX_ENERGY = 1000;

}