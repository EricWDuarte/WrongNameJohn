using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	public bool menu = false;

	public IEnumerator Shake (float duration, float magnitude, Vector2 dir) {

		if (!menu) {
			Vector3 initialPos = transform.localPosition;
			Vector3 nextPos = transform.localPosition;

			float timer = 0;

			while (timer < duration) {

				nextPos.x = (Random.Range(-0.8f, 0.8f) + dir.x * timer/duration) * magnitude;
				nextPos.y = (Random.Range(-0.8f, 0.8f) + dir.y * timer/duration) * magnitude;

				transform.localPosition = Vector2.Lerp(transform.localPosition, nextPos, 0.1f);

				timer += Time.deltaTime;

				yield return null;
			}

			transform.localPosition = initialPos;
		}
	}
}
