using UnityEngine;
using System.Collections;

public class MambNPC : MonoBehaviour {

	private static int hangingAmount = 0;

	private Vector3 originalPos;

	private AudioSource audioSource;
	private NavMeshAgent agent;
	private Animator  playerAnimator;

	private Vector3 m_startDirection;

	private bool  m_tipped = false;
	private int   m_timesTriedTip = 0;
	private float m_timerToConsiderTip = 0f;
	private float m_timerToTip = 0f;
	private float m_timerToConsiderHang = 0f;

	readonly float DIST_TO_TIP    = 1.5f;
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

		Animating ();

		if (playerState == NPCState.GoingHome) {

			transform.LookAt (m_startDirection);

			if (MoveToPoint () == true)
				playerState = NPCState.Idle;
		}

		if (playerState == NPCState.ApprochingPlayer) {

			if (MoveToPoint () == true) {
				playerState = NPCState.Hanging;
				hangingAmount++;
			}
		}

		if (playerState == NPCState.Hanging) {

			//			Debug.Log ("Approaching player.");
			agent.speed = 0f;
			agent.velocity = Vector3.zero;

			MambPlayer player = GameWorld.Instance.Player;
			MambMusicalInstrument instument = (MambMusicalInstrument)player.playerItem;

			if (instument != null) {

				m_timerToConsiderTip += Time.deltaTime;
				if (m_timerToConsiderTip >= 5f) {

					m_timerToConsiderTip = 0f;

					if (Random.value * (float)hangingAmount >= (1f / (1f + instument.Score)) * 5f) {

						playerState = NPCState.Tiping;
						hangingAmount--;
						m_timesTriedTip = 0;
					} 
					else {

						m_timesTriedTip++;
					}

					if (m_timesTriedTip >= 3) {

						agent.speed = 1f;
						playerState = NPCState.GoingHome;
						m_timesTriedTip = 0;
					}
				}
			}
		}

		if (playerState == NPCState.Tiping) {

			agent.speed = 0f;
			agent.velocity = Vector3.zero;

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

			agent.speed = 0f;
			agent.velocity = Vector3.zero;

			if (m_tipped)
				return;

			MambPlayer player = GameWorld.Instance.Player;
			if (player == null)
				return;

//			if (Vector3.Distance (transform.position, player.transform.position) > DIST_TO_LISTEN)
//				return;

			if (player.playerState == PlayerState.Playing) {

				MambMusicalInstrument instument = (MambMusicalInstrument)player.playerItem;
				if (instument.Score < 0.3f)
					return;

				m_timerToConsiderHang += Time.deltaTime;
				if (m_timerToConsiderHang >= m_timeToTip) {

					if (Random.value*2f <= instument.Score) {

						agent.destination = GameWorld.Instance.Player.transform.position;
						agent.stoppingDistance = DIST_TO_TIP;

						playerState = NPCState.ApprochingPlayer;
					}
						
					m_timerToConsiderHang = 0f;
//					Debug.Log ("Considered tipping.");
				}
			}
		}
	}

	void TipPlayer() {

		uint tip = (uint)Random.Range (m_minTipValue, m_maxTipValue);

		GameWorld.Instance.Player.AddGold (tip);
		audioSource.Play ();

		m_tipped = true;
	}

	bool MoveToPoint() {

		agent.speed = 1f;

		if (Vector3.Distance (transform.position, agent.destination) <= agent.stoppingDistance) {

			agent.speed = 0f;
			agent.velocity = Vector3.zero;
			return true;
		}

		return false;
	}

	void Animating ()
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = (agent.velocity.magnitude >= 0.01f);

		// Tell the animator whether or not the player is walking.
		playerAnimator.SetBool ("IsWalking", walking);
	}
}
