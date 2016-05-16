using UnityEngine;
using System.Collections.Generic;

public class MambPlayer : MonoBehaviour
{	
	public float speed = 6f;

	public AudioClip step;

	public uint gold   { get; set; }
	public uint food   { get; set; }
	public uint energy { get; set; }
	private float stepTimer = 0f;

	private Vector3     playerMove;
	private Vector3     playerCollisions;
	private Animator    playerAnimator;
	private Rigidbody   playerRigidbody;
	private AudioSource playerAudioSource;

	public PlayerState playerState { get; set; }
	public MambItem    playerItem  { get; set; }

	void Awake ()
	{
		// Set up references.
		playerAnimator = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
		playerAudioSource = GetComponent <AudioSource> ();
	}

	void Start() {

		playerState = PlayerState.Idle;

		SetFood(GameData.PlayerFood);

		SetEnergy(GameData.PlayerEnergy);

		SetGold(GameData.PlayerGold);
	}

	void FixedUpdate ()
	{
		if (playerAnimator.GetBool ("isDying") == true)
			return;

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
		if (playerState == PlayerState.Playing) {

			playerMove = Vector3.zero;
			return;
		}

		if (HUD.Instance.CursorDelta != Vector3.zero)
			playerMove = HUD.Instance.CursorDelta;

		playerRigidbody.velocity = playerMove * speed * Time.deltaTime; 

		if (Vector3.Distance (transform.position, HUD.Instance.NextCursorPos) <= 0.05f)
			playerRigidbody.velocity = Vector3.zero;

		if (playerRigidbody.velocity.magnitude >= 1f) {

			stepTimer += Time.fixedDeltaTime;
			if (stepTimer >= 0.4f) {

				playerAudioSource.PlayOneShot (step);
				playerAudioSource.pitch = Random.Range (0.8f, 1f);
				stepTimer = 0f;
			}
		}
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

	public void SetFood(uint value) {

		if (value >= 0 && value <= MambConstants.PLAYER_MAX_FOOD) {
			
			food = value;
			HUD.Instance.SetFood((float)food/(float)MambConstants.PLAYER_MAX_FOOD);
		}
	}

	public void ConsumeEnergy(uint value) {

		SetEnergy (energy - value);
	}

	public void SetEnergy(uint value) {

		if (value >= 0 && value <= MambConstants.PLAYER_MAX_ENERGY) {

			energy = value;
			HUD.Instance.SetEnergy((float)energy/(float)MambConstants.PLAYER_MAX_ENERGY);

			if (energy <= 0) {

				playerAnimator.SetBool ("isDying", true);

				playerRigidbody.velocity = Vector3.zero; 
				playerMove = Vector3.zero; 
			}
		}
	}

	public void AddGold(uint value) {

		if (gold + value >= 0 && gold + value <= MambConstants.PLAYER_MAX_GOLD)
			SetGold (gold + value);
	}

	public void ConsumeGold(uint value) {

		SetGold (gold - value);
	}

	public void SetGold(uint value) {

		if (value >= 0 && value <= MambConstants.PLAYER_MAX_GOLD)
			gold = value;

		HUD.Instance.gold.text = value.ToString ();
	}
}