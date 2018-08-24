using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour {

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.AddForce(new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), ForceMode.Impulse);
		rb.AddTorque(new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)));
	}

	void FixedUpdate() {
		
	}
}
