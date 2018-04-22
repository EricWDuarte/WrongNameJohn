using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDescend : MonoBehaviour {

	public float fallSpeed = 1.5f;
	public float gravity;

	public bool onGround = false;
	public bool callOnce = true;

	public LayerMask GroundLayer;
	public LayerMask SpikeLayer;


	public Transform SensorDown;

	private CameraShake cameraShake;

	void Start () {
		cameraShake = Camera.main.GetComponent<CameraShake>();
	}

	void Update () {
		
		onGround = Physics2D.OverlapCircleAll(SensorDown.position, 0.05f, GroundLayer).Length +
			Physics2D.OverlapCircleAll(SensorDown.position, 0.05f, SpikeLayer).Length > 0;

		if (!onGround) {

			fallSpeed += Mathf.Pow(fallSpeed,1.1f) * gravity  * Time.deltaTime;
			transform.Translate(new Vector3(0, -fallSpeed * Time.deltaTime, 0));

		} 
		else {

			fallSpeed = 5f;
			Vector3 temp = new Vector3 (transform.position.x, Mathf.Round(transform.position.y), 0);
			transform.position = temp;

			if (callOnce) {
				callOnce = false;
				StartCoroutine(cameraShake.Shake(0.05f, 1f, Vector2.up));
				if (SfxManager.instance != null)
				SfxManager.instance.BlockImpact();
			}

		}
	} //  Update
}
