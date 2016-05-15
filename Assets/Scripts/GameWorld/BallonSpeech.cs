using UnityEngine;
using System.Collections;

public class BallonSpeech : MonoBehaviour
{
	//this game object's transform
	private Transform goTransform;
	//the game object's position on the screen, in pixels
	private Vector3 goScreenPos;
	//the game objects position on the screen
	private Vector3 goViewportPos;

	//color before the event
	private Color 	 m_startcolor;
	//object renderer
	private Renderer m_renderer;

	//the width of the speech bubble
	public int bubbleWidth = 200;
	//the height of the speech bubble
	public int bubbleHeight = 100;

	public string description = "";

	//a material to render the triangular part of the speech balloon
	public Material mat;

	//a guiSkin, to render the round part of the speech balloon
	public GUISkin guiSkin;

	//an offset, to better position the bubble
	public float offsetX = 0;
	public float offsetY = 150;

	//an offset to center the bubble
	private int centerOffsetX;
	private int centerOffsetY;

	private bool isMouseOver;

	//use this for early initialization
	void Awake ()
	{
		//get this game object's transform
		goTransform = this.GetComponent<Transform>();
		m_renderer = GetComponent<Renderer> ();
	}

	//use this for initialization
	void Start()
	{
		//if the material hasn't been found
		if (!mat)
		{
			Debug.LogError("Please assign a material on the Inspector.");
			return;
		}

		//if the guiSkin hasn't been found
		if (!guiSkin)
		{
			Debug.LogError("Please assign a GUI Skin on the Inspector.");
			return;
		}

		//Calculate the X and Y offsets to center the speech balloon exactly on the center of the game object
		centerOffsetX = bubbleWidth/2;
		centerOffsetY = bubbleHeight/2;

		m_startcolor = m_renderer.material.color;
	}

	void Update() {

		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		bool wasHit = false;

		RaycastHit[] hits = Physics.RaycastAll (camRay);
		foreach (RaycastHit h in hits) {

			if (h.collider.gameObject == gameObject) {

				SelectObject ();
				wasHit = true;
			}
		}

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
		if (!isMouseOver)
			return;

		//Begin the GUI group centering the speech bubble at the same position of this game object. After that, apply the offset
		GUI.BeginGroup(new Rect(goScreenPos.x-centerOffsetX-offsetX,
			Screen.height-goScreenPos.y-centerOffsetY-offsetY, bubbleWidth, bubbleHeight));


		//Render the round part of the bubble
		GUI.Box(new Rect(0,0,bubbleWidth,bubbleHeight), "", guiSkin.box);

		//Render the text
//		GUI.Label(new Rect(10,0,190,50), gameObject.name, guiSkin.label);
		GUI.Label(new Rect(10,0,190,50), description, guiSkin.label);

		//If the button is pressed, go back to 41 Post
//		if(GUI.Button(new Rect(10,60,100,30),"Test"))
//		{
//			Application.OpenURL("http://www.41post.com/?p=4123");
//		}

		GUI.EndGroup();
	}

	void SelectObject()
	{
		m_renderer.material.color = Color.yellow;
		isMouseOver = true;
	}

	void DeselectObject()
	{
		m_renderer.material.color = m_startcolor;
		isMouseOver = false;
	}
}