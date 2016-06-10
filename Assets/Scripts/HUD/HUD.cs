using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class HUD : MambSingleton<HUD> {

	private bool isFading;
	public bool IsFading () {
		return isFading;
	}

	private float camRayLength;

	private Color cursorColor;
	private MambFadeScene sceneFader;
	private Animator foodBarAnimation;
	private Animator energyBarAnimation;
	private AudioSource audioSource;

	public Vector3 CursorDelta   { get; set; }
	public Vector3 NextCursorPos { get; set; }

	public Texture2D    defaultCursor;
	public Text         consoleMessage;

	// Item Buttons
	public ActivateItemButton QButton; 
	public ActivateItemButton AButton; 
	public ActivateItemButton WButton; 

	// Instrument Buttons
	public InstrumentKeyButton RKey; 
	public InstrumentKeyButton TKey; 
	public InstrumentKeyButton YKey; 
	public InstrumentKeyButton UKey; 

	// Food and enery bars
	public Slider foodBar;
	public Slider energyBar;

	// Gold text
	public Text gold;

	// Story display
	public StoryDisplay storyDisplay;

	void Awake() {

		sceneFader = GetComponent<MambFadeScene> ();
		foodBarAnimation = foodBar.GetComponentInChildren<Animator> ();
		energyBarAnimation = energyBar.GetComponentInChildren<Animator> ();
		audioSource = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {

		// Camera ray size
		camRayLength = 100f;

		Cursor.SetCursor (defaultCursor, Vector2.zero, CursorMode.Auto);

		// Make sure log string is disabled 
		RemoveLog ();

		isFading = false;
	}
		
	// Update is called once per frame
	void Update () {

		// Handle keyboard events and shortcuts
		KeyboardActions ();

		// Handle mouse events
		MouseActions();
	}

	void KeyboardActions() {

		if (Input.GetKeyDown (KeyCode.Q)) {

			QButton.Click ();
		} 
		else if (Input.GetKeyDown (KeyCode.A)) {

			AButton.Click ();
		}
		else if (Input.GetKeyDown (KeyCode.W)) {

			WButton.Click ();
		}

		if (GameWorld.Instance.Player.playerState == PlayerState.Playing) {

			if (Input.GetKeyDown (KeyCode.U))
				RKey.PlayKey ();

			if (Input.GetKeyDown (KeyCode.H))
				TKey.PlayKey ();

			if (Input.GetKeyDown (KeyCode.D))
				YKey.PlayKey ();

			if (Input.GetKeyDown (KeyCode.C))
				UKey.PlayKey ();
		}
	}

	void MouseActions() {

		if (GameWorld.Instance.Camera.IsFocusing ()) {

			CursorDelta = Vector3.zero;
			return;
		}

		if (GameWorld.Instance.Player.playerState == PlayerState.Playing) {
			return;
		}
			
		if (Input.GetMouseButtonUp (0))
			CursorDelta = Vector3.zero;
		
		if (Input.GetMouseButton (0)) {

			if (EventSystem.current.IsPointerOverGameObject ())
				return;

			if(MambInnBallon.IsMouseOverBallon(Input.mousePosition))
				return;

			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

			RaycastHit floorHit;

			// Perform the raycast and if it hits something on the floor layer...
			RaycastHit[] hits = Physics.RaycastAll (camRay);

			foreach (RaycastHit h in hits) {

				if (h.collider.gameObject.layer == LayerMask.NameToLayer ("Interactble")) {

					CursorDelta = Vector3.zero;
					return;
				}
			}
		
			foreach (RaycastHit h in hits) {
				
				if (h.collider.gameObject.layer == LayerMask.NameToLayer("Floor")) {

					Vector3 distance = h.point - GameWorld.Instance.Player.transform.position;
					distance.y = 0f;

					if (distance.magnitude < 0.35f || h.point.y > 0f)
						return;

					// Normalise the movement vector and make it proportional to the speed per second.
					NextCursorPos = h.point;
					CursorDelta = distance.normalized;
				}
			}
		}
	}

	public void SetFood(float food) {

		foodBar.value = food;

		uint maxFood = MambConstants.PLAYER_MAX_FOOD;
		float barLowLimit = (float)MambConstants.PLAYER_LOW_FOOD / (float)maxFood;

		if (foodBar.value <= barLowLimit)
			foodBarAnimation.SetTrigger ("BarBlink");
	}

	public void SetEnergy(float energy) {

		energyBar.value = energy;

		uint maxEnergy = MambConstants.PLAYER_MAX_ENERGY;
		float barLowLimit = (float)MambConstants.PLAYER_LOW_ENERGY / (float)maxEnergy;

		if (energyBar.value <= barLowLimit)
			energyBarAnimation.SetTrigger ("BarBlink");
	}

	public void ReloadScene() {

		isFading = true;
		sceneFader.StartFadeOut ("StoryMenu");
	}

	public void PlayMouseOverSFX() {

		audioSource.Play ();
	}

	public void Log(string message) {

		consoleMessage.text = message;
		consoleMessage.enabled = true;

		CancelInvoke ();
		Invoke ("RemoveLog", 3f);
	}

	private void RemoveLog() {

		consoleMessage.text = "";
		consoleMessage.enabled = false;
	}
}
