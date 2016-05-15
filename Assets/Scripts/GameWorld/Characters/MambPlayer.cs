using UnityEngine;
using System.Collections.Generic;

public class MambPlayer : MonoBehaviour
{	
	public int   maxGold   = 100;
	public float speed     = 6f;
	public float maxFood   = 100f;
	public float maxEnergy = 100f;

	private int   gold = 3;
	private float food = 1f;
	private float energy = 100f;

	private Vector3   playerMove;
	private Vector3   playerCollisions;
	private Animator  playerAnimator;
	private Rigidbody playerRigidbody;

	public PlayerState playerState { get; set; }
	public MambItem    playerItem  { get; set; }

	void Awake ()
	{
		// Set up references.
		playerAnimator = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
	}

	void Start() {

		playerState = PlayerState.Idle;

		SetFood (0.1f);

		SetEnergy (0.1f);

		SetGold (gold);
	}

	void FixedUpdate ()
	{
		// Move the player around the scene.
		Move ();

		// Turn player when there is a click
		Turning ();

		// Animate the player.
		Animating ();
	}

	void OnCollisionEnter(Collision collision) {

		foreach (ContactPoint con in collision.contacts) {

			if (con.normal != Vector3.up) {
				playerMove = Vector3.zero;
			}
		}
	}

	void OnCollisionStay(Collision collision) {

		foreach (ContactPoint con in collision.contacts) {

			if (con.normal != Vector3.up) {
				playerMove = Vector3.zero;
			}
		}
	}

	void Move ()
	{

		if (HUD.Instance.CursorDelta != Vector3.zero)
			playerMove = HUD.Instance.CursorDelta;

		playerRigidbody.velocity = playerMove * speed * Time.deltaTime; 

		if (Vector3.Distance (transform.position, HUD.Instance.NextCursorPos) <= 0.05f)
			playerRigidbody.velocity = Vector3.zero;
	}

	void Turning ()
	{
		if (HUD.Instance.CursorDelta == Vector3.zero)
			return;

		// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
		Quaternion newRotation = Quaternion.LookRotation (playerMove);

		// Set the player's rotation to this new rotation.
		playerRigidbody.MoveRotation (newRotation);
	}

	void Animating ()
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = (playerRigidbody.velocity.magnitude > 0f);

		// Tell the animator whether or not the player is walking.
		playerAnimator.SetBool ("IsWalking", walking);
	}

	public void SetFood(float value) {

		if (value >= 0f && value <= maxFood) {
			
			food = maxFood * value;
			HUD.Instance.foodBar.value = value;
		}
	}

	public void SetEnergy(float value) {

		if (value >= 0f && value <= maxEnergy) {

			energy = maxEnergy * value;
			HUD.Instance.energyBar.value = value;
		}
	}

	public void AddGold(int value) {

		if (gold + value >= 0 && gold + value <= maxGold)
			SetGold (gold + value);
	}

	public void SetGold(int value) {

		if (value >= 0 && value <= maxGold)
			gold = value;

		HUD.Instance.gold.text = value.ToString ();
	}
}