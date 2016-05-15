using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MambSingleton<HUD> {

	private int   floorMask;
	private float camRayLength;

	private Color cursorColor;

	public Vector3 CursorDelta   { get; set; }
	public Vector3 NextCursorPos { get; set; }

	public Texture2D    defaultCursor;

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

	// Use this for initialization
	void Start () {

		// Camera ray size
		camRayLength = 100f;

		// Create a layer ask for the floor layer.
		floorMask = LayerMask.GetMask ("Floor");

		Cursor.SetCursor (defaultCursor, Vector2.zero, CursorMode.Auto);
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

			if (Input.GetKeyDown (KeyCode.R))
				RKey.PlayKey ();

			if (Input.GetKeyDown (KeyCode.T))
				TKey.PlayKey ();

			if (Input.GetKeyDown (KeyCode.Y))
				YKey.PlayKey ();

			if (Input.GetKeyDown (KeyCode.U))
				UKey.PlayKey ();
		}

		if (Input.GetKeyDown (KeyCode.G)) {

			storyDisplay.Click ();
		}
	}

	void MouseActions() {

		if (GameWorld.Instance.Camera.IsFocusing ()) {

			CursorDelta = Vector3.zero;
			return;
		}

		if (Input.GetMouseButtonUp (0))
			CursorDelta = Vector3.zero;

		if (Input.GetMouseButton (0)) {

			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject (-1))
				return;

			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

			RaycastHit floorHit;

			// Perform the raycast and if it hits something on the floor layer...
			if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {

				Vector3 distance = floorHit.point - GameWorld.Instance.Player.transform.position;
				distance.y = 0f;

				if (distance.magnitude < 0.35f || floorHit.point.y > 0f)
					return;

				// Normalise the movement vector and make it proportional to the speed per second.
				NextCursorPos = floorHit.point;
				CursorDelta = distance.normalized;
			}
		}
	}
}
