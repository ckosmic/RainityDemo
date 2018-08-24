using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchPlane : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		double height = Camera.main.orthographicSize * 1.0;

		double width = height * Screen.width / Screen.height;

		transform.localScale = new Vector3((float)width, 1, (float)height);
		transform.localScale /= 5;
	}
}
