using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPlace : MonoBehaviour {

	[SerializeField] Enemy _enemyPrefab;
	[SerializeField] int _downDirection;
	[SerializeField] EnemyMovementAI.MovingDirection _movingDirection;

	public void SpawnEnemy(int energy) {

		var enemy = _enemyPrefab.Spawn(transform.position);
		enemy.Init(_downDirection, _movingDirection, energy);
	}
}
