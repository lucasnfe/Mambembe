using UnityEngine;
using System.Collections;

public class InstrumentKeyButton : MonoBehaviour {

	private AudioSource m_audioSource;
	private AudioClip m_originalClip;

	public AudioClip m_wrongNote;

	void Awake() {

		m_audioSource = GetComponent<AudioSource> ();
	}	

	void Start() {

		m_originalClip = m_audioSource.clip;
	}

	public void PlayKey() {

		if (GameWorld.Instance.Player.playerItem == null)
			return;

		if (GameWorld.Instance.Player.energy <= MambConstants.PLAYER_LOW_ENERGY) {

			GameWorld.Instance.Player.playerItem.DeactivateItem ();
			return;
		}

		if (GameWorld.Instance.Player.playerItem.GetType() == typeof(MambMusicalInstrument)) {

			MambMusicalInstrument instument = (MambMusicalInstrument)GameWorld.Instance.Player.playerItem;

			foreach (GameObject note in instument.GetPlayedNotes()) {

				if (note == null)
					break;
					
				RectTransform noteRect = note.GetComponent<RectTransform> ();
				RectTransform keyRect = this.GetComponent<RectTransform> ();

				if (RectTransformUtility.RectangleContainsScreenPoint (keyRect, noteRect.anchoredPosition)) {

					m_audioSource.clip = m_originalClip;
					m_audioSource.Play ();

					instument.DeleteNote (note);
					break;
				} 
				else { 
					
					m_audioSource.clip = m_wrongNote;
					m_audioSource.Play ();

					instument.PlayedWrongNote ();
				}

				GameWorld.Instance.Player.ConsumeEnergy (1);
			}
		}
	}
}
