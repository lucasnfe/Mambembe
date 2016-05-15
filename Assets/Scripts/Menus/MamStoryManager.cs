using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MamStoryManager : MonoBehaviour {

	private static int m_pageNumber = 0;

	private MambFadeScene sceneFader;

	public Text        m_text;
	public TextAsset []m_pages;

	void Awake() {

		sceneFader = GetComponent<MambFadeScene> ();
	}

	// Use this for initialization
	void Start () {

		m_text.text = "";

		if (m_pages.Length > 0)
			m_text.text = m_pages [m_pageNumber].text;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButtonDown (0)) {

			FlipPage ();
		}
	}

	void FlipPage() {

		if (m_pageNumber + 1 < m_pages.Length) {

			m_pageNumber++;
			sceneFader.StartFadeOut ("StoryMenu");
		} 
		else {

			sceneFader.StartFadeOut ("DemoScene");
		}
	}
}
