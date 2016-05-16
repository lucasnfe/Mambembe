using UnityEngine;
using System.Collections;

public abstract class MambItem : MonoBehaviour {

	public abstract bool CheckAvailability(ref string message);

	public abstract void ActivateItem();

	public abstract void DeactivateItem();
}
