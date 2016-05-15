using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MambFadeScene: MonoBehaviour
{
	private string sceneToLoad;

	public Image FadeImg;
	public float fadeSpeed = 1.5f;

	public bool    sceneStarting = false;
	public bool    sceneEnding   = false;

	void Awake()
	{
		FadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
	}

	void Update()
	{
		// If the scene is starting...
		if (sceneStarting)
			StartScene();

		if (sceneEnding)
			EndScene ();
	}


	void FadeToClear()
	{
		// Lerp the colour of the image between itself and transparent.
		FadeImg.color = Color.Lerp(FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
	}


	void FadeToBlack()
	{
		// Lerp the colour of the image between itself and black.
		FadeImg.color = Color.Lerp(FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
	}

	void StartScene()
	{
		// Fade the texture to clear.
		FadeToClear();

		// If the texture is almost clear...
		if (FadeImg.color.a <= 0.05f)
		{
			// ... set the colour to clear and disable the RawImage.
			FadeImg.color = Color.clear;
			FadeImg.enabled = false;

			// The scene is no longer starting.
			sceneStarting = false;
		}
	}

	void EndScene()
	{
		// Make sure the RawImage is enabled.
		FadeImg.enabled = true;

		// Start fading towards black.
		FadeToBlack();

		// If the screen is almost black...
		if (FadeImg.color.a >= 0.995f) {

			sceneEnding = false;
			MambSceneManager.Instance.LoadScene (sceneToLoad, false);
		}
	}

	public void StartFadeOut(string scene) {

		FadeImg.color = Color.clear;

		sceneEnding = true;
		sceneToLoad = scene;
	}
}   