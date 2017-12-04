using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HittableByBullet : MonoBehaviour {

	public event System.Action<Vector2, Vector2> bulletHitDetectedEvent;

	private void Awake() {

		Assert.IsNotNull(GetComponent<Collider2D>());
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (bulletHitDetectedEvent != null) {
			var bullet = other.gameObject.GetComponent<Bullet>();			
			bulletHitDetectedEvent(bullet.transform.position, bullet.direction);
		}
	}
}
