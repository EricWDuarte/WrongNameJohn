using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCanon : MonoBehaviour {

	public Transform player;
	public Transform GunPoint;

	private bool up = false;
	private bool aiming = true;
	private bool down = true;

	[SerializeField]
	private float upSpeed, maxUp, rotateSpeed;

	[SerializeField]
	private float timeToShoot, shootDelay, laserDespawnTimer, despawnTimer;

	[SerializeField]
	private GameObject laser;

	[SerializeField]
	private Transform canon;

	private bool canonShrink = false; 
	private Vector2 startingSize;
	private Vector2 startingPos;


	void Start () {
		startingSize = canon.localScale;
		startingPos = canon.localPosition;

		player = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<Transform>();
		StartCoroutine(LaserTimer());
	}

	void Update () {

		if (aiming)
			Aim();

		if (canonShrink)
			Shrink();

		if (up == false)
			GoUp();

		if (down == false && up == true)
			GoDown();
	}

	void GoUp () {
		
		transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, maxUp, upSpeed * Time.deltaTime), 0);

		if (transform.position.y >= maxUp -0.2f)
			up = true;
	}

	void Aim() {
		transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, Mathf.Atan2(player.position.y - transform.position.y, player.position.x - transform.position.x) * Mathf.Rad2Deg, rotateSpeed * Time.deltaTime));
	}

	void Shrink () {
		float mult = 1f - (0.9f * Time.deltaTime);
		canon.localScale = new Vector2(mult * canon.localScale.x, canon.localScale.y);
		canon.localPosition = new Vector2(mult * canon.localPosition.x, canon.localPosition.y);
	}

	IEnumerator LaserTimer () {
		yield return new WaitForSeconds( timeToShoot );
		canonShrink = true;
		aiming = false;
		Vector3 lingPlayer = player.position;

		yield return new WaitForSeconds( shootDelay );
		canonShrink = false;
		canon.localScale = startingSize;
		canon.localPosition = startingPos;

		RaycastHit2D rayHit;
		Ray2D ray = new Ray2D(GunPoint.position, lingPlayer - GunPoint.position);
		rayHit = Physics2D.Raycast(ray.origin, ray.direction);

		if (rayHit.collider != null) {
			if (rayHit.collider.tag == "Player")
				rayHit.collider.SendMessage( "Died" );

			if (rayHit.collider.tag == "Ground")
				rayHit.collider.SendMessage( "SafeExplode" );
		}


		GameObject clone = Instantiate ( laser, GunPoint.position, transform.localRotation, transform );
		clone.GetComponent<SpriteRenderer>().size = new Vector2(rayHit.distance, clone.GetComponent<SpriteRenderer>().size.y);
		Destroy(clone, laserDespawnTimer);

		yield return new WaitForSeconds( despawnTimer );
		down = false;

	}

	void GoDown () {
		transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, -3f, upSpeed * Time.deltaTime), 0);
		if (transform.position.y <= -3 + 0.2f)
			Destroy(gameObject);
	}

}
