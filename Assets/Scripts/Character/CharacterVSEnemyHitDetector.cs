using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CharacterVSEnemyHitDetector : MonoBehaviour {	

	[SerializeField] LayerMask _enemyLayerMask;

	public event System.Action wasHitByEnemyEvent;

	private void Awake() {

		Assert.IsNotNull(GetComponent<Collider2D>());
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		if (_enemyLayerMask.ContainsLayer(other.gameObject.layer)) {
	
			if (wasHitByEnemyEvent != null) {
				wasHitByEnemyEvent();
			}
		}
	}
}
