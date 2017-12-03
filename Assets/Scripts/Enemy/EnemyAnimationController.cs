using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour {

	[SerializeField] EnemyMovementAI _enemyMovementAI;

	private void Update() {
		
		transform.localScale = new Vector3(_enemyMovementAI.movingDirection == EnemyMovementAI.MovingDirection.Left ? 1.0f : -1.0f, 1.0f, 1.0f);
	}
}
