using UnityEngine;
using System.Collections;

public class MambNPC : MonoBehaviour {

	private Vector3 originalPos;

	private AudioSource audioSource;
	private NavMeshAgent agent;

	private float m_timerToConsiderTip = 0f;
	private float m_timerToTip = 0f;

	readonly float DIST_TO_TIP    = 1.00f;
	readonly float DIST_TO_HOME   = 0.05f;
	readonly float DIST_TO_LISTEN = 5.00f;

	public float  m_timeToTip = 1f;

	public int    m_minTipValue = 1;
	public int    m_maxTipValue = 2;

	public NPCState playerState { get; set; }

	void Awake() {

		agent = GetComponent<NavMeshAgent> ();
		audioSource = GetComponent<AudioSource> ();

		GetComponent<Renderer> ().material.color = Random.ColorHSV ();
	}

	// Use this for initialization
	void Start () {

		playerState = NPCState.Idle;
		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (playerState == NPCState.GoingHome) {

			Debug.Log ("Goind home.");

			if (MoveToPoint() == true)
				playerState = NPCState.Idle;
		}

		if (playerState == NPCState.ApprochingPlayer) {

			Debug.Log ("Approaching player.");

			if (MoveToPoint() == true)
				playerState = NPCState.Tiping;
		}

		if (playerState == NPCState.Tiping) {

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
					Debug.Log ("Considered tipping.");
				}
			}
		}
	}

	void TipPlayer() {

		int tip = Random.Range (m_minTipValue, m_maxTipValue);

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
}
