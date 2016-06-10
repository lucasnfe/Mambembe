using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MambInteractBallon : MonoBehaviour
{
	private Transform goTransform;
	private Vector3 goScreenPos;
	private Vector3 goViewportPos;

	//color before the interaction
	private Color m_startColor;

	//object renderer
	private Renderer m_renderer;

	//an offset to center the bubble
	private int centerOffsetX;
	private int centerOffsetY;

	static private Dictionary <GameObject, Rect> ballons;

	private bool isMouseOver;
	protected bool isInteractionWindowOpen;

	public int    bubbleWidth  = 200;
	public int    bubbleHeight = 100;
	public float  offsetX      = 0f;
	public float  offsetY      = 150f;
	public string description;
	public bool   showInteractionOptions = true;

	public GUISkin  guiSkin;
	public Material highlighMaterial;


	//use this for early initialization
	void Awake ()
	{
		//get this game object's transform
		goTransform = GetComponent<Transform>();
		m_renderer = GetComponent<Renderer> ();
	}

	//use this for initialization
	void Start()
	{
		//if the guiSkin hasn't been found
		if (!guiSkin)
		{
			Debug.LogError("Please assign a GUI Skin on the Inspector.");
			return;
		}

		//Calculate the X and Y offsets to center the speech balloon exactly on the center of the game object
		centerOffsetX = bubbleWidth/2;
		centerOffsetY = bubbleHeight/2;

		m_startColor = m_renderer.material.color;

		ballons = new Dictionary <GameObject, Rect> ();
	}

	void Update() {

		if (GameWorld.Instance.Player.playerState == PlayerState.Playing)
			return;

		if (GameWorld.Instance.Camera.IsFocusing ()) {

			DeselectObject ();
			return;
		}

		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		bool wasHit = false;

		if (EventSystem.current.IsPointerOverGameObject ())
			return;

		RaycastHit[] hits = Physics.RaycastAll (camRay);
		foreach (RaycastHit h in hits) {

			if (h.collider.gameObject == gameObject) {

				SelectObject ();
				wasHit = true;

				if (Input.GetMouseButton (0) && showInteractionOptions)
					isInteractionWindowOpen = true;
			} 
		}
			
		if (Input.GetMouseButton (0) && isInteractionWindowOpen && !wasHit && !IsMouseOverBallon (Input.mousePosition))
			isInteractionWindowOpen = false;

		if(Input.GetMouseButton (0) && HUD.Instance.storyDisplay.IsOpen && !HUD.Instance.storyDisplay.WasStoryEvent())
			HUD.Instance.storyDisplay.Click ();

		if(isMouseOver && !wasHit)
			DeselectObject ();
	}

	//Called once per frame, after the update
	void LateUpdate()
	{
		//find out the position on the screen of this game object
		goScreenPos = Camera.main.WorldToScreenPoint(goTransform.position);	

		//Could have used the following line, instead of lines 70 and 71
		//goViewportPos = Camera.main.WorldToViewportPoint(goTransform.position);
		goViewportPos.x = goScreenPos.x/(float)Screen.width;
		goViewportPos.y = goScreenPos.y/(float)Screen.height;
	}

	//Draw GUIs
	void OnGUI()
	{
		if (HUD.Instance.IsFading ())
			return;

		if (isInteractionWindowOpen) {

			// Begin the GUI group centering the speech bubble at the same position of this game object. 
			// After that, apply the offset
			Rect groupRect = new Rect (goScreenPos.x - centerOffsetX, 
				Screen.height - goScreenPos.y - centerOffsetY - offsetY - 20, bubbleWidth, bubbleHeight);

			ballons[gameObject] = groupRect;

			GUI.BeginGroup(groupRect);

			//Render the round part of the bubble
			GUI.Box(new Rect(0,0,bubbleWidth,bubbleHeight), "", guiSkin.box);

			//Render the text
			GUI.Label(new Rect(10,0,190,50), description, guiSkin.label);

			DrawInteractionOptions (10, 0);

			if (GUI.Button (new Rect (10, groupRect.height - 25, 60, 15), "Cancel", guiSkin.button)) {
				isInteractionWindowOpen = false;
			}

			GUI.EndGroup();

			return;
		}

		if (!isMouseOver)
			return;

		// Begin the GUI group centering the speech bubble at the same position of this game object. 
		// After that, apply the offset
		GUI.BeginGroup(new Rect(goScreenPos.x-centerOffsetX-offsetX,
			Screen.height-goScreenPos.y-centerOffsetY-offsetY, 80, 20));

		//Render the round part of the bubble
		GUI.Box(new Rect(0,0,bubbleWidth,bubbleHeight), "", guiSkin.box);

		//Render the text
		GUI.Label(new Rect(10,0,190,50), description, guiSkin.label);

		GUI.EndGroup();
	}

	public virtual void DrawInteractionOptions(float startX, float startY) {

	}

	void SelectObject()
	{
		if(!isMouseOver)
			HUD.Instance.PlayMouseOverSFX ();

		m_renderer.material.color = Color.yellow;
		isMouseOver = true;
	}

	void DeselectObject()
	{

		if(!isInteractionWindowOpen)
			m_renderer.material.color = m_startColor;
		
		isMouseOver = false;
	}

	public static bool IsMouseOverBallon(Vector2 mousePosition) {

		Vector2 adjustPos = new Vector2 (mousePosition.x, Screen.height - mousePosition.y);

		foreach (Rect rect in ballons.Values) {
			bool contains = rect.Contains (adjustPos);

			if (contains)
				return true;
		}

		return false;
	}
}