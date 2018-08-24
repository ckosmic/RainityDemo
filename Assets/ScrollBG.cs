using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBG : MonoBehaviour {

	public ContextMenu contextMenu;
	public static bool mouseOverIcon = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(-Vector3.right * Time.deltaTime * 0.1f, Space.World);
		if (transform.position.x <= -transform.localScale.x * 10)
			transform.position = new Vector3(0, 0, 20);
	}

	void OnMouseUpAsButton() {
		contextMenu.HideMenu();
	}

	void OnMouseOver() {
		if (Input.GetMouseButtonDown(1) && !mouseOverIcon) {
			contextMenu.activeMenu = 1;
			contextMenu.GoToMousePosition();
		}
	}
}
