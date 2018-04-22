using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DificultyManager : MonoBehaviour {

	public Transform rightPilar;
	public Transform leftPilar;

	public Transform laserCanon;

	[SerializeField]
	private float dificultyTimerMin, dificultyTimerMax;

	void Start () {
		StartCoroutine ( DificultyTimer() );
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator DificultyTimer () {
		float time = Random.Range(dificultyTimerMin, dificultyTimerMax);
		float chance = Random.Range(0f, 1f);
		yield return new WaitForSeconds ( time );

		if (GameManager.level > 1 && chance >= 0.6f) {
			BlockSpawn.instance.spiked = 2;
		}

		if (GameManager.level > 2 && chance < 0.4f) {
			
			Transform clone = Instantiate( laserCanon, Vector2.zero, Quaternion.identity, Random.Range(0, 1f) < 0.5 ? rightPilar : leftPilar );
			clone.localPosition = new Vector2( 0, - 7f );
		}

		StartCoroutine ( DificultyTimer() );

	}
}
