using UnityEngine;
using System.Collections;

public class MambNPC : MonoBehaviour {

	private Vector3 originalPos;

	private AudioSource audioSource;
	private NavMeshAgent agent;
	private Animator  playerAnimator;

	private Vector3 m_startDirection;

	private float m_timerToConsiderTip = 0f;
	private float m_timerToTip = 0f;

	readonly float DIST_TO_TIP    = 1.15f;
	readonly float DIST_TO_HOME   = 0.05f;
	readonly float DIST_TO_LISTEN = 5.00f;

	public float m_timeToTip = 1f;

	public uint m_minTipValue = 1;
	public uint m_maxTipValue = 2;

	public NPCState playerState { get; set; }

	void Awake() {

		agent = GetComponent<NavMeshAgent> ();
		audioSource = GetComponent<AudioSource> ();
		playerAnimator = GetComponent <Animator> ();

		GetComponent<Renderer> ().material.color = Random.ColorHSV ();
	}

	// Use this for initialization
	void Start () {

		playerState = NPCState.Idle;
		originalPos = transform.position;
		m_startDirection = transform.position + transform.forward;
	}
	
	// Update is called once per frame
	void Update () {

		if (playerState == NPCState.GoingHome) {

			Debug.Log ("Goind home.");

			transform.LookAt (m_startDirection);

			if (MoveToPoint () == true)
				playerState = NPCState.Idle;
		}

		if (playerState == NPCState.ApprochingPlayer) {

			Debug.Log ("Approaching player.");

			if (MoveToPoint() == true)
				playerState = NPCState.Tiping;
		}

		Animating ();

		if (playerState == NPCState.Tiping) {

			agent.speed = 0f;

			m_timerToTip += Time.deltaTime;
			if (m_timerToTip >= 1f) {

				TipPlayer ();

				agent.destination = originalPos;
				agent.stoppingDistance = DIST_TO_HOME;

				playerState = NPCState.GoingHome;
				m_timerToTip = 0f;
			}
		}

		if (playerState == NPCState.Idle) {

			MambPlayer player = GameWorld.Instance.Player;

			if (player == null)
				return;

			if (Vector3.Distance (transform.position, player.transform.position) > DIST_TO_LISTEN)
				return;

			if (player.playerState == PlayerState.Playing) {

				m_timerToConsiderTip += Time.deltaTime;
				if (m_timerToConsiderTip >= m_timeToTip) {

					MambMusicalInstrument instument = (MambMusicalInstrument)player.playerItem;

					if (Random.value <= instument.m_score.value) {

						agent.destination = GameWorld.Instance.Player.transform.position;
						agent.stoppingDistance = DIST_TO_TIP;

						playerState = NPCState.ApprochingPlayer;
					}
						
					m_timerToConsiderTip = 0f;
//					Debug.Log ("Considered tipping.");
				}
			}
		}
	}

	void TipPlayer() {

		uint tip = (uint)Random.Range (m_minTipValue, m_maxTipValue);

		GameWorld.Instance.Player.AddGold (tip);
		audioSource.Play ();
	}

	bool MoveToPoint() {

		agent.speed = 1f;

		if (Vector3.Distance (transform.position, agent.destination) <= agent.stoppingDistance) {

			agent.speed = 0f;
			return true;
		}

		return false;
	}

	void Animating ()
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = (agent.velocity != Vector3.zero);

		// Tell the animator whether or not the player is walking.
		playerAnimator.SetBool ("IsWalking", walking);
	}
}
