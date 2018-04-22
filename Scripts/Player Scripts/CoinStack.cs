using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinStack : MonoBehaviour {

	public static CoinStack instance;
	public float magnitude;

	float posY = 0;

	public GameObject gold;
	public GameObject silver;

	private ArrayList coins = new ArrayList();

	PlayerMovement player;

	bool callOnce = true;

	void Start () {
		instance = this;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
	}

	void Update () {
		if (GameManager.level >= 7 && callOnce) {
			callOnce = false;
			transform.Translate(1f, 0, 0);
		}
	}

	public void addGold () {
		if (coins.Count < 80) {
			GameObject clone = Instantiate (gold, new Vector3 (transform.position.x, transform.position.y + posY, 0), Quaternion.identity, transform);
			coins.Add(clone);
			posY += 0.125f;
		}
	}

	public void addSilver () {
		if (coins.Count < 80) {
			GameObject clone = Instantiate (silver, new Vector3 (transform.position.x, transform.position.y + posY, 0), Quaternion.identity, transform);
			coins.Add(clone);
			posY += 0.125f;
		}
	}

	public void LoseCoins () {
		int loop = 0;
		int coinsN = coins.Count;
		if (coinsN > 40) {
			loop = coinsN - 40;
		} else if (coinsN > 0) {
			loop = coinsN;
		} else if (coinsN <= 0) {
			player.Died();
			return;
		}

		posY -= loop * 0.125f;

		for (int i = coinsN - 1; i > coinsN - 1 - loop; i--) {
			RemoveCoin((GameObject)coins[i]);
			coins.Remove(coins[i]);
		}

	}

	public void LoseAllCoins () {
		for (int i = coins.Count - 1; i >= 0; i--) {
			RemoveCoin((GameObject)coins[i]);
		}
	}

	void RemoveCoin (GameObject coin) {
		Destroy(coin, 5f);
		coin.GetComponent<Rigidbody2D>().simulated = true;
		coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f) * magnitude, Random.Range(0, 1f) * magnitude + 400f));
	}
}
