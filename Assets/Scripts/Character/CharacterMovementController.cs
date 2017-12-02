﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CharacterMovementController : MonoBehaviour {

	[SerializeField] float _movementSpeed = 1.0f;
	[SerializeField] float _groundFriction = 0.8f;
 	[SerializeField] float _maxVerticalSpeed = 1.0f;
	[SerializeField] float _jumpStartSpeed = 10.0f;
	[SerializeField] float _groundCheckDistance = 0.1f;
	[SerializeField] float _minTimeBetweenGrounded = 0.2f;
	[SerializeField] Vector2 _groundCheckSize = new Vector2(1.0f, 1.0f);
	[SerializeField] Vector2 _groundCheckOffset = new Vector2(0.0f, 0.0f);
	[SerializeField] LayerMask _groundCheckLayerMask;

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
		if (_canBeGroundedTimer <= 0.0f && Physics2D.BoxCastNonAlloc((Vector2)position + _groundCheckOffset, _groundCheckSize, 0.0f, Vector2.down, _raycastResults, _groundCheckDistance, _groundCheckLayerMask) > 0) {
			_grounded = true;
		}
		else {
			_grounded = false;			 
		}

		bool directionKeyIsActive = false;

		bool leftKeyActive = Input.GetKey(KeyCode.A);
		bool rightKeyActive = Input.GetKey(KeyCode.D);
		bool jumpKeyActive = Input.GetKey(KeyCode.W);

		// Controls
		if (leftKeyActive ^ rightKeyActive) {
			if (leftKeyActive) {
				directionKeyIsActive = true;
				if (velocity.x > 0.0f) {
					velocity.x *= _groundFriction;	
				}			
				velocity.x -= Time.fixedDeltaTime * _movementSpeed;
			}			
			if (rightKeyActive) {
				directionKeyIsActive = true;
				if (velocity.x < 0.0f) {
					velocity.x *= _groundFriction;	
				}			
				velocity.x += Time.fixedDeltaTime * _movementSpeed;
			}
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
		if (jumpKeyActive) {			
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
