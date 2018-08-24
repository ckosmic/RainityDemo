using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	public Text lockButtonText;
	public static bool iconsLocked = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ToggleIconLock() {
		iconsLocked = !iconsLocked;
		lockButtonText.text = (iconsLocked ? "Unlock icons" : "Lock icons");
	}
}
