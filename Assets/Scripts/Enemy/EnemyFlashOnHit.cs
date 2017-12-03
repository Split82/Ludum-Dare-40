using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlashOnHit : MonoBehaviour {

	[SerializeField] SpriteFlashEffect _spriteFlashEffect;
	[SerializeField] Enemy _enemy;

	private void OnEnable() {

		_enemy.wasHitEvent += HandleEnemyWasHitEvent;
	}

	private void OnDisable() {
		
		_enemy.wasHitEvent -= HandleEnemyWasHitEvent;
	}

	private void HandleEnemyWasHitEvent() {

		_spriteFlashEffect.Flash(0.032f);
	}
}
