using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float bulletSpeed = 15f;


	void Start () {
		StartCoroutine(timer ());
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(-bulletSpeed * transform.localScale.x * Time.deltaTime, 0f, 0f);
	}

	IEnumerator timer () {
		yield return new WaitForSeconds(3f);
		AutoDestroy();
	}

	void OnCollisionEnter2D (Collision2D collision) {

		if (collision.collider.tag == "Ground") {
			collision.collider.SendMessage("GotHit");
			//SfxManager.instance.ShootImpact();
			AutoDestroy ();
		}

		if (collision.collider.tag == "Pilar") {
			AutoDestroy ();
		}
	
	}

	void AutoDestroy () {
		Destroy(gameObject);
	}
}
