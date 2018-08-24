using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithMouse : MonoBehaviour {

	Vector3 origPos;

	// Use this for initialization
	void Start () {
		origPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newLoc = new Vector3((Input.mousePosition.y - Screen.height / 2) / 60, (-Input.mousePosition.x + Screen.width / 2) / 60, 0);
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newLoc), Time.deltaTime * 3);
		transform.Translate(new Vector3(Mathf.PerlinNoise(Time.time,0)/50, Mathf.PerlinNoise(Time.time, 1)/50, Mathf.PerlinNoise(Time.time, 2)/50));
		transform.position = Vector3.Lerp(transform.position, origPos, Time.deltaTime * 7);
	}
}
