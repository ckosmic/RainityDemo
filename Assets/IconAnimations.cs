using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconAnimations : MonoBehaviour {

	public IconScript iconScript;

	float rotSpeed = 1000;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update() {
		if (iconScript.mouseOver == false) {
			rotSpeed = 1000;
			transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, Vector3.zero, Time.deltaTime * 10);
		} else {
			rotSpeed = Mathf.Lerp(rotSpeed, 100, Time.deltaTime * 3);
			transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
		}
	}
}
