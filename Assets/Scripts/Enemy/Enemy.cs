using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField] int _startEnergy = 1;
	[SerializeField] HittableByBullet _hittableByBullet;	
	[SerializeField] EnemyMovementAI _enemyMovementAI;
	[SerializeField] Collider2D[] _colliders;

	public event System.Action wasHitEvent;

	private int _energy;
	private bool _isDying;

	public void Init(int downDirection, EnemyMovementAI.MovingDirection movingDirection, int startEnergy) {
		
		_energy = startEnergy;
		_enemyMovementAI.Init(downDirection, movingDirection);
		_enemyMovementAI.enabled = true;
		_isDying = false;

		foreach (var collider in _colliders) {
			collider.enabled = true;
		}
	}

	private void Awake() {

		_energy = _startEnergy;
	}

	private void OnEnable() {

		_hittableByBullet.bulletHitDetectedEvent += HandleBulletHitDetected;
	}

	private void OnDisable() {

		_hittableByBullet.bulletHitDetectedEvent -= HandleBulletHitDetected;
	}

	private void HandleBulletHitDetected() {

		_energy--;
		
		if (!_isDying) {
			if (wasHitEvent != null) {
				wasHitEvent();
			}
		}

		if (_energy <= 0 && !_isDying) {
			_isDying = true;
			StartCoroutine(DyingCoroutine());
		}
	}

	private IEnumerator DyingCoroutine() {

		foreach (var collider in _colliders) {
			collider.enabled = false;
		}
		_enemyMovementAI.enabled = false;
		yield return new WaitForSeconds(5.5f);
		this.Recycle();
	}
}
