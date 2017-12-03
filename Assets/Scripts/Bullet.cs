using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	
	[EventSender] [SerializeField] PositionDirectionGameEvent _bulletDidHitObstacleEvent;

	[SerializeField] Rigidbody2D _rigidbody;
	[SerializeField] float _speed;		

	private Vector2 _direction;

	private void Start() {

		_rigidbody.bodyType = RigidbodyType2D.Kinematic;
	}	

	public void Init(Vector2 direction) {
		
		_direction = direction;		
		var angle = Mathf.Atan2(-_direction.y, -_direction.x) * Mathf.Rad2Deg;
 		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	private void FixedUpdate() {

		_rigidbody.MovePosition(_rigidbody.position + Time.fixedDeltaTime * _speed * _direction);
	}

    void OnTriggerEnter2D(Collider2D other) {

		_bulletDidHitObstacleEvent.position = _rigidbody.position;
		_bulletDidHitObstacleEvent.direction = _direction;
		_bulletDidHitObstacleEvent.Raise(this, _bulletDidHitObstacleEvent);
		this.Recycle();
    }
}
