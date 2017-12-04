using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	[EventSender] [SerializeField] PositionGameEvent _characterDidDieEvent;
	[Space]
	[SerializeField] Gun _gun;
	[SerializeField] CharacterMovementController _movementController;
	[SerializeField] CharacterVSEnemyHitDetector _hitDetector;
	[SerializeField] AnimationCurve _deathJumpCurve;
	[SerializeField] CharacterAnimationController _characterAnimationController;

	private bool _isDying;

	private void OnEnable() {

		_hitDetector.wasHitByEnemyEvent += HandleCharacterWasHit;
	}

	private void OnDisable() {
		
		_hitDetector.wasHitByEnemyEvent -= HandleCharacterWasHit;
	}

	private void HandleCharacterWasHit() {

		if (_isDying) {
			return;
		}

		_characterDidDieEvent.Raise(this, _characterDidDieEvent);

		_isDying = true;
		_gun.enabled = false;
		_movementController.enabled = false;		
		_characterAnimationController.enabled = false;		

		StartCoroutine(DyingCoroutine(Vector2.right));
	}
	
	private IEnumerator DyingCoroutine(Vector2 direction) {

		Time.timeScale = 0.5f;

		Vector3 startPos = transform.position;
		var yieldInstruction = new WaitForEndOfFrame();

		float directionMul = direction.x > 0 ? 1.0f : -1.0f;
		float jumpLength = 3.0f;
		float duration = 0.7f;
		float elapsedTime = 0.0f;
		float rotation = 320.0f;
		while (elapsedTime < duration) {

			float t = elapsedTime / duration;
			transform.position = startPos + new Vector3(directionMul * t * jumpLength, jumpLength * _deathJumpCurve.Evaluate(t), 0.0f);
			transform.eulerAngles = new Vector3(0.0f, 0.0f, rotation * t);
			elapsedTime += Time.deltaTime;
			yield return yieldInstruction;
		}
		transform.eulerAngles = Vector3.zero;

		Time.timeScale = 1.0f;
		this.Recycle();		
	}
}
