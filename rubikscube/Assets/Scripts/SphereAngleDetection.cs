using UnityEngine;
using System.Collections;

public class SphereAngleDetection : MonoBehaviour {
	private GameObject controller;
	private GameController controllerScript;
	
	private Vector3 oldAxis = Vector3.zero;
	private Vector3 snappedStart;
	private Vector3 snapped;

	public static float angle;
	public static int pos;
	public static Vector3 axis;
	public static Vector3 starterCubePosition;

	public float speed;

	void Start() {
		angle = 0;
		pos = 0;
		oldAxis = Vector3.down;
		controller = GameObject.Find ("Controller");
		controllerScript = controller.GetComponent<GameController>() as GameController;
		controllerScript.StartCoroutine ("RotatePlane");
	}

	void Update() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			Vector3 normal = hit.normal;
			PlaneFinder (normal);
		}
	}

	void PlaneFinder(Vector3 normal) {
		Vector3 added = normal + 2 * CubeMouseDetection.clickedFaceNormal;
		float min = Mathf.Min (Mathf.Abs (added.x),Mathf.Abs (added.y),Mathf.Abs (added.z));

		if (Mathf.Abs(min-Mathf.Abs (added.x)) < .1) {
			pos = (int)starterCubePosition.x;
			axis = Vector3.right;
		} else if (Mathf.Abs(min-Mathf.Abs (added.y)) < .1) {
			pos = (int)starterCubePosition.y;
			axis = Vector3.up;
		} else {
			pos = (int)starterCubePosition.z;
			axis = Vector3.forward;
		}
		snapped = normal - min * axis;
		//Debug.DrawLine(Vector3.zero, snapped, Color.red, 10, false);
		Debug.Log ("snapped: " + snapped.ToString() + ", axis: " + axis.ToString() 
		           + ", normal: " +normal.ToString () + ", clicked: " + CubeMouseDetection.clickedFaceNormal.ToString());

		if (axis != oldAxis) {
			//Debug.Log ("changing");
			oldAxis = axis;
			snappedStart = snapped;
			controllerScript.changingAxis = true;
		}

		float sign = Mathf.Sign (Vector3.Dot (axis,Vector3.Cross (snappedStart,snapped)));
		angle = Vector3.Angle (snappedStart, snapped) * sign * speed;
		//Debug.Log (ax + " " + angle);
	}
}
