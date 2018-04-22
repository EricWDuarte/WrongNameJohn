using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

	public GameObject bulletParent;
	public GameObject Bullet;
	public Transform Gun;

	public float fireRate = 4.0f;

	private bool canShoot = true;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire")) {
			if (canShoot) {
				ShootBullet ();
				canShoot = false;
				StartCoroutine("timer");
			}
		}
	}

	void ShootBullet () {
		GameObject temp = Instantiate(Bullet, Gun.position, Quaternion.identity, bulletParent.transform);
		temp.transform.localScale = transform.localScale;

		SfxManager.instance.Shoot();
	}

	IEnumerator timer () {
		yield return new WaitForSeconds(1.0f/fireRate);
		canShoot = true;
	}
}
