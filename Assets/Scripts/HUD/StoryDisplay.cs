using UnityEngine;
using System.Collections;

public class StoryDisplay : MonoBehaviour {

	private bool m_isOpen;

	private Animation m_animation;

	public AnimationClip m_openAnimation;
	public AnimationClip m_closeAnimation;

	void Awake() {

		m_animation = GetComponent<Animation> ();
	}

	void Start() {

		m_animation.AddClip (m_openAnimation, "open");
		m_animation.AddClip (m_closeAnimation, "close");
	}

	public void Click() {

		if (!m_isOpen)

			OpenDisplay ();
		else
			CloseDisplay ();
	}

	private void OpenDisplay() {

		if (!m_isOpen) {

			Debug.Log ("asda");
			m_animation.Play ("open");

			m_isOpen = true;
		}
	}

	private void CloseDisplay() {

		if (m_isOpen) {

			m_animation.Play ("close");

			m_isOpen = false;
		}
	}
}
