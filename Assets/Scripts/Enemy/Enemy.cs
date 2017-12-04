using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[EventSender] [SerializeField] PositionDirectionGameEvent _wasKilledEvent;
	[EventSender] [SerializeField] BasicGameEvent _didHitRecycleTile;
	[EventSender] [SerializeField] PositionDirectionGameEvent _wasHitEvent;
	[NullAllowed] [EventSender] [SerializeField] BasicGameEvent _wasSpawnedEvent;
	[EventSender] [SerializeField] PositionDirectionGameEvent _didFinishDeathJump;
	[Space]
	[NullAllowed] [SerializeField] IntVariable _energyVariable;
	[SerializeField] EnemyType _enemyType;
	[SerializeField] int _startEnergy = 1;
	[SerializeField] HittableByBullet _hittableByBullet;	
	[SerializeField] EnemyMovementAI _enemyMovementAI;
	[SerializeField] Collider2D[] _colliders;
	[SerializeField] AnimationCurve _deathJumpCurve;

	public EnemyType enemyType {
		get {
			return _enemyType;
		}
	}

	public event System.Action wasHitEvent;
	public event System.Action didDieEvent;

	private int _energy;
	private bool _isDying;

	public void Init(int downDirection, EnemyMovementAI.MovingDirection movingDirection, int startEnergy) {
		
		_energy = startEnergy;
		_enemyMovementAI.Init(downDirection, movingDirection);
		_enemyMovementAI.enabled = true;		
		_isDying = false;
		if (_energyVariable) {
			_energyVariable.value = _energy;
		}

		foreach (var collider in _colliders) {
			collider.enabled = true;
		}

		if (_wasSpawnedEvent != null) {
			_wasSpawnedEvent.Raise(this, _wasSpawnedEvent);
		}
	}

	private void Awake() {

		_energy = _startEnergy;
	}

	private void OnEnable() {

		_hittableByBullet.bulletHitDetectedEvent += HandleBulletHitDetected;
		_enemyMovementAI.didHidRecycleTileEvent += HandleDidHitRecycleTile;
	}

	private void OnDisable() {

		_hittableByBullet.bulletHitDetectedEvent -= HandleBulletHitDetected;
		_enemyMovementAI.didHidRecycleTileEvent -= HandleDidHitRecycleTile;
	}

	private void HandleBulletHitDetected(Vector2 position, Vector2 direction) {

		if (_isDying) {
			return;
		}

		_energy--;
		if (_energyVariable) {
			_energyVariable.value = _energy;
		}
		
		if (wasHitEvent != null) {
			wasHitEvent();
		}
		_wasHitEvent.position = position;
		_wasHitEvent.direction = direction;
		_wasHitEvent.Raise(this, _wasHitEvent);
	

		if (_energy <= 0) {
			_wasKilledEvent.position = transform.position;
			_wasKilledEvent.direction = direction;
			_wasKilledEvent.Raise(this, _wasKilledEvent);
			_isDying = true;
			if (didDieEvent != null) {
				didDieEvent();
			}
			StartCoroutine(DyingCoroutine(direction));
		}
	}

	private void HandleDidHitRecycleTile() {

		_didHitRecycleTile.Raise(this, _didHitRecycleTile);
		this.Recycle();
	}

	private IEnumerator DyingCoroutine(Vector2 direction) {

		foreach (var collider in _colliders) {
			collider.enabled = false;
		}
		_enemyMovementAI.enabled = false;

		Vector3 startPos = transform.position;
		var yieldInstruction = new WaitForEndOfFrame();

		float directionMul = direction.x > 0 ? 1.0f : -1.0f;
		float jumpLength = 3.0f;
		float duration = 0.6f;
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

		_didFinishDeathJump.position = transform.position;
		_didFinishDeathJump.Raise(this, _didFinishDeathJump);

		this.Recycle();
	}
}
