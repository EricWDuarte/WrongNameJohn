using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoedaScript : MonoBehaviour {

	private Rigidbody2D rb;
	public int coinValue;
	private PlayerHP hp;
	public bool golden;

	public LayerMask GroundLayer;

	public Transform UpSensor;

	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D>();
		hp = GameObject.FindWithTag("Player").GetComponent<PlayerHP>();
	}

	public void GetCoin () {
		if (golden) {
			CoinStack.instance.addGold();
		} else {
			CoinStack.instance.addSilver();
		}
		
	}

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
