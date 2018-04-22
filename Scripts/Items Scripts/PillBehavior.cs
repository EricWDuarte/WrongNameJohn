using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillBehavior : MonoBehaviour {

	private Rigidbody2D rb;
	public Transform UpSensor;
	public LayerMask GroundLayer;

	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -2f) {
			transform.Translate(0, 13f, 0);
			rb.velocity = Vector2.zero;
		}

		if (Physics2D.OverlapCircle(UpSensor.position, 0.05f, GroundLayer)) {
			transform.Translate(0, 1f, 0);
		}
	}
}
