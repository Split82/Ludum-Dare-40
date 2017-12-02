using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	
	[SerializeField] float _speed;		

	private Transform _transform;
	private Vector3 _direction;

	private void Awake() {

		_transform = transform;
	}	

	public void Init(Vector3 direction) {
		
		_direction = direction;		
		 var angle = Mathf.Atan2(-_direction.y, -_direction.x) * Mathf.Rad2Deg;
 		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	private void FixedUpdate() {

		_transform.position += Time.fixedDeltaTime * _speed * _direction;	
	}
}
