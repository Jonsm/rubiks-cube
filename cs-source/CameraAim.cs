using UnityEngine;
using System.Collections;

public class CameraAim : MonoBehaviour {
	private Vector3 originalMousePosition;
	private bool canMove;
	private float magnitude;
	public float speed;

	void Start () {
		transform.LookAt (Vector3.zero);
		magnitude = transform.position.magnitude;
	}

	void Update() {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (!Physics.Raycast (ray)) {
				canMove = true;
				//Debug.Log ("test");
				originalMousePosition = Input.mousePosition;
			} else {
				canMove = false;
			}
		}

		if (Input.GetMouseButton (0) && canMove) {
			float newPositionXIncrement = (Input.mousePosition.x - originalMousePosition.x) * speed;
			float newPositionYIncrement = (Input.mousePosition.y - originalMousePosition.y) * speed;
			if (Mathf.Abs (magnitude - transform.position.y) < .1) {
				newPositionYIncrement = Mathf.Clamp (newPositionYIncrement,-1,0);
			}	else if (Mathf.Abs (magnitude + transform.position.y) < .1) {
				newPositionYIncrement = Mathf.Clamp (newPositionYIncrement,0,1);
			}
			Vector3 newPositionIncrement = transform.TransformDirection(Vector3.right)*newPositionXIncrement + 
				transform.TransformDirection(Vector3.up)*newPositionYIncrement;
			transform.position = Vector3.Normalize (transform.position+newPositionIncrement) * magnitude;
			transform.LookAt (Vector3.zero);
			originalMousePosition = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp (0)) {
			canMove = true;
		}
	}
}
