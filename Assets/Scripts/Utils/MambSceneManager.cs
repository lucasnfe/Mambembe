using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MambSceneManager : MambSingleton<MambSceneManager> {

	public delegate void ActionBetweenScenes();

	public AudioClip _backgroundMusic;

	private int _lastScene;
	public int LastScene { get { return _lastScene; } }

	private int _currentScene;
	public int CurrentScene { get { return _currentScene; } }

	void Start() {

		_backgroundMusic = Resources.Load("Audio/TitleTheme") as AudioClip;
	}

	public void LoadScene (string sceneName, bool showLoadingScreen = true, ActionBetweenScenes actioneBetweenScenes = null, ActionBetweenScenes actioneAfterLoading = null) {

		StartCoroutine(SceneSwitchCoroutine(sceneName, showLoadingScreen, actioneBetweenScenes, actioneAfterLoading));
	}

	public void ReloadScene (bool showLoadingScreen = true, ActionBetweenScenes actioneBetweenScenes = null, ActionBetweenScenes actioneAfterLoading = null) {

		string currentScene = SceneManager.GetActiveScene ().name;
		StartCoroutine(SceneSwitchCoroutine(currentScene, showLoadingScreen, actioneBetweenScenes, actioneAfterLoading));
	}

	IEnumerator SceneSwitchCoroutine (string sceneName, bool showLoadingScreen, ActionBetweenScenes actioneBetweenScenes, ActionBetweenScenes actioneAfterLoading) {

		_lastScene = SceneManager.GetActiveScene ().buildIndex;

		GameObject _loadingBanner = null;
		if (showLoadingScreen)
			SceneManager.LoadScene ("LoadingScene");

		yield return new WaitForSeconds(0.1f);

		if (actioneBetweenScenes != null)
			actioneBetweenScenes();

		if (showLoadingScreen) {
			_loadingBanner = Camera.main.gameObject;
			DontDestroyOnLoad (_loadingBanner);
		}
			
		SceneManager.LoadScene(sceneName);
		yield return new WaitForSeconds(0.1f);

		if (actioneAfterLoading != null)
			actioneAfterLoading();

		_currentScene = SceneManager.GetActiveScene ().buildIndex;

		if (_loadingBanner)
			Destroy (_loadingBanner);

		if (sceneName.EndsWith("Menu")) {
			if(!MambAudioController.Instance.IsPlayingMusic(_backgroundMusic))
				MambAudioController.Instance.PlayMusic(_backgroundMusic);
		}
		else
			MambAudioController.Instance.StopMusic();
	}
}
