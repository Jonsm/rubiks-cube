using UnityEngine;
using System.Collections;

public class CubeMouseDetection : MonoBehaviour {
	public GameObject detectorPrefab;
	private GameObject detector;

	public static Vector3 clickedFaceNormal;

	void OnMouseDown() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			float absoluteX = Mathf.Abs (hit.normal.x);
			float absoluteY = Mathf.Abs (hit.normal.y);
			float absoluteZ = Mathf.Abs (hit.normal.z);
			clickedFaceNormal = new Vector3(absoluteX,absoluteY,absoluteZ);
		}
		detector = Instantiate (detectorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		SphereAngleDetection.starterCubePosition = transform.position;
		//Debug.Log ("down");
	}

	void OnMouseUp() {
		//Debug.Log ("up");
		Destroy (detector);
		//controllerScript.StartCoroutine ("Snap");
	}
}
