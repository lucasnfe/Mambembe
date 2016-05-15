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