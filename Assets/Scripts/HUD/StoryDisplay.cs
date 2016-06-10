using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryDisplay : MonoBehaviour {

	private bool m_isOpen;
	public bool IsOpen {
		get { return m_isOpen; }
	}

	private Animation m_animation;

	private bool wasStoryEvent;
	public bool WasStoryEvent() {
		return wasStoryEvent;
	}

	public Text m_text;

	void Awake() {

		m_animation = GetComponent<Animation> ();
	}

	public void Click() {

		if (!m_isOpen)

			OpenDisplay ();
		else
			CloseDisplay ();
	}

	private void OpenDisplay() {

		if (!m_isOpen) {

			m_animation.Play ("StoryPanel");
			m_isOpen = true;
		}
	}

	private void CloseDisplay() {

		if (m_isOpen) {

			m_animation.Play ("StoryPanelClose");
			m_isOpen = false;
		}
	}

	public void SetText(string text, bool storyEvent = false) {

		wasStoryEvent = storyEvent;
		m_text.text = text;
	}
}
