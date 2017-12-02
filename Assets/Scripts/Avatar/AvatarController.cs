using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AvatarController : MonoBehaviour {

	[SerializeField] float _movementSpeed = 1.0f;
	[SerializeField] float _groundFriction = 0.8f;
 	[SerializeField] float _maxVerticalSpeed = 1.0f;
	[SerializeField] float _jumpStartSpeed = 10.0f;
	[SerializeField] float _groundCheckDistance = 0.1f;
	[SerializeField] float _minTimeBetweenGrounded = 0.2f;
	[SerializeField] Vector2 _avatarSize = new Vector2(1.0f, 1.0f);
	[SerializeField] LayerMask _groundCheckLayerMask;

	private Rigidbody2D _rigidbody;
	private RaycastHit2D[] _raycastResults;
	private bool _grounded;
	private float _canBeGroundedTimer;
	private bool _jumpKeyWasUp;

	private void Awake() {

		_rigidbody = GetComponent<Rigidbody2D>();
		Assert.IsNotNull(_rigidbody);

		_raycastResults = new RaycastHit2D[4];
	}

	private void FixedUpdate() {

		Vector3 velocity = _rigidbody.velocity;
		Vector3 position = _rigidbody.position;

		if (_canBeGroundedTimer > 0.0f) {
			_canBeGroundedTimer -= Time.fixedDeltaTime;
		}

		// Ground Check
		if (_canBeGroundedTimer <= 0.0f && Physics2D.BoxCastNonAlloc(position, _avatarSize, 0.0f, Vector2.down, _raycastResults, _groundCheckDistance, _groundCheckLayerMask) > 0) {
			_grounded = true;
		}

		bool directionKeyIsActive = false;

		// Controls
		if (Input.GetKey(KeyCode.LeftArrow)) {
			directionKeyIsActive = true;
			if (velocity.x > 0.0f) {
				velocity.x *= _groundFriction;	
			}			
			velocity.x -= Time.fixedDeltaTime * _movementSpeed;
		}
		
		if (Input.GetKey(KeyCode.RightArrow)) {
			directionKeyIsActive = true;
			if (velocity.x < 0.0f) {
				velocity.x *= _groundFriction;	
			}			
			velocity.x += Time.fixedDeltaTime * _movementSpeed;
		}

		if (!directionKeyIsActive && _grounded) {
			velocity.x *= _groundFriction;
		}

		// Max velocity
		if (velocity.x > _maxVerticalSpeed) {
			velocity.x = _maxVerticalSpeed;
		}
		else if (velocity.x < -_maxVerticalSpeed) {
			velocity.x = -_maxVerticalSpeed;
		}		

		// Jump
		if (Input.GetKey(KeyCode.UpArrow)) {			
			if (_grounded && _jumpKeyWasUp) {			
				velocity.y = _jumpStartSpeed;
				_canBeGroundedTimer = _minTimeBetweenGrounded;
				_grounded = false;
			}
			_jumpKeyWasUp = false;
		}
		else {
			_jumpKeyWasUp = true;
		}

		_rigidbody.velocity = velocity;
	}

    private void OnDrawGizmosSelected() {
		
		if (!_rigidbody) {
			_rigidbody = GetComponent<Rigidbody2D>();
		}
        Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(_rigidbody.position, _avatarSize);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(_rigidbody.position + Vector2.down * _groundCheckDistance, _avatarSize);        
    }
}
