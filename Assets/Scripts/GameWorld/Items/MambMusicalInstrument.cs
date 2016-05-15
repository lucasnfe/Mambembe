using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent (typeof (AudioSource))]
public class MambMusicalInstrument : MambItem {

	public float m_scorePerNote = 0.1f;

	public Slider m_score;
	public GameObject m_sheetPanel;
	public GameObject m_noteIcon;

	private MambObjectPool m_notes;

	private List<GameObject>  m_playedNotes;
	private GameObject[]      m_notesToDelete;
	private GameObject[] 	  m_nextFourNotes;

	private int   m_del;
	private float m_noteStart;
	private float m_sheetPos;
	private float m_noteTimer;

	private float []m_barPos = {40f, 60f, 80f, 100f};

	// Use this for initialization
	void Start () {

		m_noteTimer = 1.38f;

		m_sheetPos  = Screen.width / 2f - m_sheetPanel.GetComponent<RectTransform> ().rect.width / 3f;
		m_noteStart = Screen.width / 2f + m_sheetPanel.GetComponent<RectTransform> ().rect.width / 3f;

		m_notes = new MambObjectPool (m_noteIcon, 50, m_sheetPanel.transform);

		m_playedNotes   = new List<GameObject>();
		m_notesToDelete = new GameObject[m_barPos.Length];
		m_nextFourNotes = new GameObject[m_barPos.Length];
	}

	void FixedUpdate() {

		if (GameWorld.Instance.Player.playerState != PlayerState.Playing)
			return;

		foreach (GameObject note in m_playedNotes) {

			note.transform.Translate (Vector3.left * 60f * Time.fixedDeltaTime);

			if (note.transform.position.x <= m_sheetPos + 50f) {

				m_score.value = 0f;

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
		m_noteTimer += Time.fixedDeltaTime;

		if (m_noteTimer >= 1.38f) {

			GameObject note = m_notes.GetFreeObject ();
			float randomBarPos = m_barPos [Random.Range (0, m_barPos.Length)];
			note.transform.position = new Vector3 (m_noteStart, randomBarPos, 0f);
			m_playedNotes.Add (note);

			m_noteTimer = 0f;
		}
	}

	override public void ActivateItem() {

		GameWorld.Instance.Camera.FocusOnTarget (ActivateMusicSheet);
	}

	override public void DeactivateItem() {

		GameWorld.Instance.Camera.LooseFocus (DeactivateMusicSheet);
	}

	public void ActivateMusicSheet() {

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

		m_score.value += m_scorePerNote;

		m_notesToDelete[m_del] = note;
		m_del++;
	}

	public void PlayedWrongNote(GameObject note) {

		m_score.value = 0;
	}
}
