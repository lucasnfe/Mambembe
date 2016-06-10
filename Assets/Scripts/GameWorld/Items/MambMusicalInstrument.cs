using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SynchronizerData;

[RequireComponent (typeof (AudioSource))]
public class MambMusicalInstrument : MambItem {

	public float m_scorePerNote = 0.01f;

	public GameObject m_sheetPanel;
	public GameObject m_noteIcon;

	private MambObjectPool m_notes;

	public float Score { get; set; }

	private List<GameObject>  m_playedNotes;
	private GameObject[]      m_notesToDelete;
	private GameObject[] 	  m_nextFourNotes;

	private int   m_del;
	private float m_noteStart;
	private float m_sheetPos;
	private float m_lastNode;

	private BeatCounter      _beatCounter;
	private BeatObserver     _beatObserver;
	private BeatSynchronizer _beatSync;

	private readonly float []m_barPos = {20f, 40f, 60f, 80f};

	void Awake() {

		_beatSync = GetComponent<BeatSynchronizer>();
		_beatCounter = GetComponent<BeatCounter>();
		_beatObserver = GetComponent<BeatObserver>();
	}

	// Use this for initialization
	void Start () {

		m_sheetPos  = Screen.width / 2f - m_sheetPanel.GetComponent<RectTransform> ().rect.width / 3f;
		m_noteStart = Screen.width / 2f + m_sheetPanel.GetComponent<RectTransform> ().rect.width / 3f;

		m_notes = new MambObjectPool (m_noteIcon, 50, m_sheetPanel.transform);

		m_playedNotes   = new List<GameObject>();
		m_notesToDelete = new GameObject[m_barPos.Length];
		m_nextFourNotes = new GameObject[m_barPos.Length];

		for (int i = 0; i < m_barPos.Length; i++)
			m_barPos [i] += ((Screen.height / 2f) - m_sheetPanel.GetComponent<RectTransform> ().rect.height);
	}
		
	void Update() {

		if (GameWorld.Instance.Player.playerState != PlayerState.Playing)
			return;

		if ((_beatObserver.beatMask & BeatType.OnBeat) == BeatType.OnBeat) {

			ScoreToBeatValue ();

			GameObject no = m_notes.GetFreeObject ();

			float randomBarPos = m_barPos [Random.Range (0, ScoreToBars())];
			if (randomBarPos == m_lastNode) {

				for (int i = 0; i < ScoreToBars(); i++) {

					if(m_barPos[i] != m_lastNode) {
						randomBarPos = m_barPos [i];
						break;
					}
				}
			}

			no.transform.position = new Vector3 (m_noteStart, randomBarPos, 0f);
			m_playedNotes.Add (no);

			m_lastNode = randomBarPos;
		}

		foreach (GameObject note in m_playedNotes) {

			note.transform.Translate (Vector3.left * 88f * Time.deltaTime);

			if (note.transform.position.x <= m_sheetPos + 50f) {

				PlayedWrongNote ();

				m_notesToDelete[m_del] = note;
				m_del++;
			}
		}

		int deletedNotes = 0;

		for (int i = 0; i < m_del; i++) {

			m_notes.SetFreeObject (m_notesToDelete[i]);
			m_playedNotes.Remove (m_notesToDelete[i]);
			m_notesToDelete [i] = null;
			deletedNotes++;
		}

		m_del -= deletedNotes;
	}

	override public bool CheckAvailability(ref string message) {

		if (GameWorld.Instance.Player.energy <= MambConstants.PLAYER_LOW_ENERGY) {

			message = "Jack is too tired to play now.";
			HUD.Instance.Log (message);
			return false;
		}

		return true;
	}

	override public void ActivateItem() {

		GameWorld.Instance.ForegroundSource.Stop ();
		GameWorld.Instance.Camera.FocusOnTarget (ActivateMusicSheet);
	}

	override public void DeactivateItem() {

		GameWorld.Instance.ForegroundSource.Play ();
		GameWorld.Instance.Camera.LooseFocus (DeactivateMusicSheet);
	}

	public void ActivateMusicSheet() {

		Score = 0f;
		m_sheetPanel.SetActive (true);

		GameWorld.Instance.Player.playerItem = this;
		GameWorld.Instance.Player.playerState = PlayerState.Playing;
	}

	public void DeactivateMusicSheet() {

		m_notes.FreeAllObjects ();
		m_playedNotes.Clear ();

		m_sheetPanel.SetActive (false);

		GameWorld.Instance.Player.playerItem = null;
		GameWorld.Instance.Player.playerState = PlayerState.Idle;
	}

	public GameObject[] GetPlayedNotes() {

		for (int i = 0; i < m_barPos.Length; i++) {

			if(i <= m_playedNotes.Count - 1)
				m_nextFourNotes [i] = m_playedNotes [i];
		}

		return m_nextFourNotes;
	}

	public void DeleteNote(GameObject note) {

		Score += m_scorePerNote;

		if (Score >= 1f)
			Score = 1f;

		m_notesToDelete[m_del] = note;
		m_del++;
	}

	public void PlayedWrongNote() {

		Score -= m_scorePerNote;

		if (Score <= 0f)
			Score = 0f;
	}

	public int ScoreToBars() {

		if (Score >= 0f && Score < 0.4f)
			return 3;

		else if (Score >= 0.4f && Score < 0.6f)
			return 3;

		else if (Score >= 0.6f && Score < 0.7f)
			return 3;

		return 4;
	}

	public void ScoreToBeatValue() {

		if (Score >= 0f && Score < 0.8f)
			_beatCounter.beatValue = BeatValue.WholeBeat;

		else if (Score >= 0.8f && Score <= 1f)
			_beatCounter.beatValue = BeatValue.HalfBeat;

		Debug.Log ("Score = " + Score);

		_beatCounter.CalculateSamples ();
		
	}
}
