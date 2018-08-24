using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

	public LayerMask interaction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, interaction)) {
			//transform.position = Vector3.Lerp(transform.position, hit.point, Time.deltaTime * 2);
			transform.position = hit.point;
		}
	}
}
