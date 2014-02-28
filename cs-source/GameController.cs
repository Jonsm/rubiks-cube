using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public GameObject[] cubes = new GameObject[26];
	private List<GameObject> toRotate = new List<GameObject> ();

	public float totalTime;
	private float turnTime;

	public float increment;
	public bool changingAxis = true;
	private float currentAngle;
	private Vector3 axis;
	private int pos;

	public float angle;
	public int posIn;
	public Vector3 axisIn;
	public Vector3 origin;

	void Start() {

	}

	void Update() {

	}

	public IEnumerator RotatePlane() {
		while (Input.GetMouseButton(0)) {
			if (changingAxis) {
				//Debug.Log (currentAngle + " c");
				StartCoroutine ("ResetRotations");
				yield return ResetRotations ();

				pos = SphereAngleDetection.pos;
				axis = SphereAngleDetection.axis;
				toRotate = new List<GameObject>();
				currentAngle = 0;

				foreach (GameObject cube in cubes) {
					if (CubeIsOnPlane (axis, pos, cube)) {
						toRotate.Add (cube);
					}
				}
				changingAxis = false;
			}

			yield return new WaitForSeconds(increment);
			//Debug.Log (currentAngle + " " + SphereAngleDetection.angle);
			foreach (GameObject cube in toRotate) {
				cube.transform.RotateAround (origin, axis, SphereAngleDetection.angle - currentAngle);
			}
			currentAngle = SphereAngleDetection.angle;
		}
		StartCoroutine ("Snap");
		yield return Snap ();
	}
	
	/*IEnumerator RotatePlane(Vector3 axis, int pos, string key) {
		List<GameObject> toRotate = new List<GameObject> ();
		foreach (GameObject cube in cubes) {
			if (CubeIsOnPlane (axis, pos, cube)) {
				toRotate.Add (cube);
			}
		}

		while (Input.GetKey (key)) {
			foreach (GameObject cube in toRotate) {
				cube.transform.RotateAround (origin, axis, angle * Mathf.Min (increment, totalTime - turnTime) / totalTime);
			}
			turnTime+=Mathf.Min (increment, totalTime - turnTime);
			yield return new WaitForSeconds(increment);
		}
		if (turnTime != 0) {
			if (turnTime > totalTime / 2) {
				foreach (GameObject cube in toRotate) {
					cube.transform.RotateAround (origin, axis, angle / totalTime * (totalTime - turnTime));
				}
			} else {
				foreach (GameObject cube in toRotate) {
					cube.transform.RotateAround (origin, axis, angle / totalTime * turnTime * -1);
				}
			}			
		}
		turnTime = 0;
	}*/

	IEnumerator ResetRotations() {
		foreach (GameObject cube in toRotate) {
			cube.transform.RotateAround (origin, axis, -1 * currentAngle);
		}
		yield return null;
	}

	IEnumerator Snap() {
		float roundedAngle = Mathf.Round (currentAngle / 90) * 90;
		foreach (GameObject cube in toRotate) {
			cube.transform.RotateAround (origin, axis, roundedAngle - currentAngle);
			//Debug.Log (roundedAngle - currentAngle);
		}

		foreach (GameObject cube in cubes) {
			float roundedX = Mathf.Round (cube.transform.localPosition.x);
			float roundedY = Mathf.Round (cube.transform.localPosition.y);
			float roundedZ = Mathf.Round (cube.transform.localPosition.z);
			cube.transform.localPosition = new Vector3(roundedX,roundedY,roundedZ);

			Vector3 euler = cube.transform.eulerAngles;
			euler.x = Mathf.Round(euler.x / 90) * 90;
			euler.y = Mathf.Round(euler.y / 90) * 90;
			euler.z = Mathf.Round(euler.z / 90) * 90;
			transform.eulerAngles = euler;
			//Debug.Log (euler.ToString ());
		}

		currentAngle = 0;
		yield return null;
	}

	bool CubeIsOnPlane(Vector3 axis, int pos, GameObject cube) {
		Vector3 subtracted = cube.transform.position - axis * pos;
		int rounded = Mathf.RoundToInt (Vector3.Dot (subtracted, axis));
		return (rounded == 0);
	}
}
