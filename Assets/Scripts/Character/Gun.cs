using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	[EventSender] [SerializeField] PositionDirectionGameEvent _didFireEvent;
	[SerializeField] CharacterMovementController _characterMovementController;
	[SerializeField] Bullet _bulletPrefab;
	[SerializeField] Transform _firePoint;
	[SerializeField] float _interval = 0.1f;
	[SerializeField] float _kickBack = 10.0f;
	[SerializeField] bool _useKeyDown = false;
		
	private float _timeAccumulator;

	private void Start() {

		ObjectPool.CreatePool(_bulletPrefab, 100, null);
	}

	private void Update() {

		_timeAccumulator += Time.deltaTime;

		var fireButtonActive = false;
		if (_useKeyDown) {
			fireButtonActive = Input.GetKeyDown(KeyCode.X);
		}
		else {
			fireButtonActive = Input.GetKey(KeyCode.X);			
		}

		if (fireButtonActive && _timeAccumulator >= _interval) {	
			var firePointPos = _firePoint.transform.position;
			float flipDirection = _characterMovementController.movingDirection == CharacterMovementController.MovingDirection.Left ? 1.0f : -1.0f;
			var bullet = _bulletPrefab.Spawn(firePointPos);
			var direction = -_firePoint.transform.right * flipDirection;
			bullet.Init(direction);
			_timeAccumulator -= _interval;
			_characterMovementController.PushBack(_kickBack);
			_didFireEvent.position = firePointPos;
			_didFireEvent.direction = direction;
			_didFireEvent.Raise(this, _didFireEvent);
		}

		if (_timeAccumulator > _interval) {
			_timeAccumulator = _interval;
		}
	}
}
