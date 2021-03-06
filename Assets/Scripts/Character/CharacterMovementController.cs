﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CharacterMovementController : MonoBehaviour {

	[EventSender] [SerializeField] PositionDirectionGameEvent _didBecomeGrounded;
	[EventSender] [SerializeField] PositionDirectionGameEvent _characterStartedJumpEvent;
	[SerializeField] Vector3Variable _position;
	[Space]
	[SerializeField] float _movementSpeed = 1.0f;
	[SerializeField] float _groundFriction = 0.8f;
	[SerializeField] float _airFriction = 0.8f;
 	[SerializeField] float _maxVerticalSpeed = 1.0f;
	[SerializeField] float _jumpStartSpeed = 10.0f;
	[SerializeField] float _jumpStartEnergy = 0.1f;
	[SerializeField] float _jumpEnergyDepletionMul = 0.9f;
	[SerializeField] float _groundCheckDistance = 0.1f;
	[SerializeField] float _minTimeBetweenGrounded = 0.2f;
	[SerializeField] Vector2 _groundCheckSize = new Vector2(1.0f, 1.0f);
	[SerializeField] Vector2 _groundCheckOffset = new Vector2(0.0f, 0.0f);
	[SerializeField] LayerMask _groundCheckLayerMask;
	
	public enum MovingDirection {
		Left,
		Right,
	}

	public MovingDirection movingDirection {
		get {
			return _movingDirection;
		}
	}

	public bool grounded {
		get {
			return _grounded;
		}
	}

	public Vector3 velocity {
		get {
			return _rigidbody.velocity;
		}
	}

	public Vector3 position {
		get {
			return _rigidbody.position;
		}
	}

	public bool isTouchingGround {
		get {
			return _wasTouchingGround;
		}
	}

	private Rigidbody2D _rigidbody;
	private RaycastHit2D[] _raycastResults;
	private bool _grounded;
	private bool _wasTouchingGround;
	private float _canBeGroundedTimer;
	private bool _jumpKeyWasUp;
	private float _jumpEnergy;
	private MovingDirection _movingDirection;
	private Vector3 _pushAccumulator;

	private void Awake() {

		_rigidbody = GetComponent<Rigidbody2D>();
		Assert.IsNotNull(_rigidbody);

		_raycastResults = new RaycastHit2D[4];
	}

	private void FixedUpdate() {		

		Vector3 velocity = _rigidbody.velocity;
		Vector3 position = _rigidbody.position;		
		
		_position.value = position;

		if (_canBeGroundedTimer > 0.0f) {
			_canBeGroundedTimer -= Time.fixedDeltaTime;
		}

		var isTouchingGround = Physics2D.BoxCastNonAlloc((Vector2)position + _groundCheckOffset, _groundCheckSize, 0.0f, Vector2.down, _raycastResults, _groundCheckDistance, _groundCheckLayerMask) > 0;

		// Ground Check
		if (_canBeGroundedTimer <= 0.0f && isTouchingGround) {
			_grounded = true;
			if (!_wasTouchingGround) {
				_didBecomeGrounded.position = transform.position;
				_didBecomeGrounded.Raise(this, _didBecomeGrounded);
			}			
		}
		_wasTouchingGround = isTouchingGround;
		// else {
		// 	_grounded = false;			 
		// }		

		bool directionKeyIsActive = false;

		bool leftKeyActive = Input.GetKey(KeyCode.LeftArrow);
		bool rightKeyActive = Input.GetKey(KeyCode.RightArrow);
		bool jumpKeyActive = Input.GetKey(KeyCode.Z);

		// Controls
		if (leftKeyActive ^ rightKeyActive) {
			if (leftKeyActive) {
				_movingDirection = MovingDirection.Left;
				directionKeyIsActive = true;
				if (velocity.x > 0.0f) {
					velocity.x *= _groundFriction;	
				}			
				velocity.x -= Time.fixedDeltaTime * _movementSpeed;
			}			
			if (rightKeyActive) {
				_movingDirection = MovingDirection.Right;
				directionKeyIsActive = true;
				if (velocity.x < 0.0f) {
					velocity.x *= _groundFriction;	
				}			
				velocity.x += Time.fixedDeltaTime * _movementSpeed;
			}
		}

		if (!directionKeyIsActive) {
			velocity.x *= _grounded ? _groundFriction : _airFriction;
			velocity += _pushAccumulator;
			_pushAccumulator = Vector3.zero;
		}

		// Max velocity
		if (velocity.x > _maxVerticalSpeed) {
			velocity.x = _maxVerticalSpeed;
		}
		else if (velocity.x < -_maxVerticalSpeed) {
			velocity.x = -_maxVerticalSpeed;
		}		

		// Jump
		if (jumpKeyActive) {			
			if (_grounded && _jumpKeyWasUp) {			
				velocity.y = _jumpStartSpeed;
				_canBeGroundedTimer = _minTimeBetweenGrounded;
				_jumpEnergy = _jumpStartEnergy;
				_grounded = false;
				_jumpKeyWasUp = false;
				_characterStartedJumpEvent.position = transform.position;
				_characterStartedJumpEvent.Raise(this, _characterStartedJumpEvent);
			} else {
				velocity.y += _jumpEnergy * Time.fixedDeltaTime;
			}			
		}
		else {
			_jumpKeyWasUp = true;
		}

		_jumpEnergy *= _jumpEnergyDepletionMul;

		_rigidbody.velocity = velocity;
	}

	public void PushBack(float strength) {

		if (_movingDirection == MovingDirection.Left) {
			_pushAccumulator += new Vector3(strength, 0.0f, 0.0f);
		}
		else {
			_pushAccumulator += new Vector3(-strength, 0.0f, 0.0f);
		}
	}

	public void TeleportTo(Vector3 position) {

		_rigidbody.velocity = Vector3.zero;
		_rigidbody.position = position;
	}

    private void OnDrawGizmosSelected() {
		
		if (!_rigidbody) {
			_rigidbody = GetComponent<Rigidbody2D>();
		}
        Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(_rigidbody.position + _groundCheckOffset, _groundCheckSize);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(_rigidbody.position  + _groundCheckOffset + Vector2.down * _groundCheckDistance, _groundCheckSize);        
    }
}
