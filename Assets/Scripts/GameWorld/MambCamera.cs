using UnityEngine;
using System.Collections;

public class MambCamera : MonoBehaviour
{
	public delegate void FocusCallbak();

    public Transform target;
	public Vector3 focusOffset;
    public float smoothing = 5f;

    private Vector3 offset;             
	private Vector3 originalPos;
	private CameraState cameraState;

	private FocusCallbak m_focusCallback;
    void Start ()
    {
		transform.LookAt (target.position);

        // Calculate the initial offset.
        offset = transform.position - target.position;
    }

    void FixedUpdate ()
    {
		if (cameraState == CameraState.MovingIn) {

			Vector3 dest = target.position + target.forward * 10f + focusOffset;

			transform.position = Vector3.Lerp (transform.position, dest, smoothing * Time.deltaTime);
			transform.LookAt (target.position);

			if (Vector3.Distance (transform.position, dest) <= 0.05f) {

				cameraState = CameraState.OnFocus;
				if (m_focusCallback != null)
					m_focusCallback ();
			}
		} 
		else if (cameraState == CameraState.MovingOut) {

			transform.position = Vector3.Lerp (transform.position, originalPos, smoothing * Time.deltaTime);
			transform.LookAt (target.position);

			if (Vector3.Distance (transform.position, originalPos) <= 0.05f) {

				cameraState = CameraState.Default;
			}
		} 
		else if (cameraState == CameraState.Default) {

			// Create a postion the camera is aiming for based on the offset from the target.
			Vector3 targetCamPos = target.position + offset;

			// Smoothly interpolate between the camera's current position and it's target position.
			transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);

			transform.LookAt (target.position);
		}
    }

	public void FocusOnTarget(FocusCallbak onFocusCallback = null) {

		cameraState = CameraState.MovingIn;
		originalPos = transform.position;

		m_focusCallback = onFocusCallback;
	}

	public void LooseFocus(FocusCallbak offFocusCallback = null) {

		cameraState = CameraState.MovingOut;

		m_focusCallback = offFocusCallback;
		if (m_focusCallback != null)
			m_focusCallback ();
	}

	public bool IsFocusing() {

		return (cameraState == CameraState.MovingIn || cameraState == CameraState.MovingOut );
	}
}