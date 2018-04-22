using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour {

	public float invulTime = 3f;
	float timer;

	SpriteRenderer sr;

	PlayerMovement player;

	void Start () {
		sr = gameObject.GetComponent<SpriteRenderer>();
		player = gameObject.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
	}
		
	public void LoseHp () {
		if (timer > invulTime) {
			timer = 0;

			CoinStack.instance.LoseCoins();
			StartCoroutine(Flash(false));
		}
	}

	IEnumerator Flash (bool on) {
		yield return new WaitForSeconds (0.1f);
		Color color;
		if (timer < invulTime) {

			if (on == true) {
				color = sr.color;
				color.a = 1f;
				sr.color = color;
			} else {
				color = sr.color;
				color.a = 0f;
				sr.color = color;
			}

			StartCoroutine(Flash(!on));

		} else {
			color = sr.color;
			color.a = 1f;
			sr.color = color;
		}

	}
}
