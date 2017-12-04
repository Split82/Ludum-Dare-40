using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHighlight : MonoBehaviour {

	[SerializeField] Animator _animator;
	[SerializeField] Enemy _enemy;

	private void OnEnable() {

		_animator.SetBool("Active", true);
		_enemy.didDieEvent += HandleEnemyDidDieEvent;
	}

	private void OnDisable() {
		
		_enemy.didDieEvent -= HandleEnemyDidDieEvent;
	}

	private void HandleEnemyDidDieEvent() {

		_animator.SetBool("Active", false);
	}
}
