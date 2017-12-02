using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour {

	[SerializeField] float _rotationSpeed = 8.0f;
	[SerializeField] float _fade = 0.9f;
	[SerializeField] float _maxValue = 1.0f;	

	private float _screenshakeValue;
	private Vector3 _startPos;

	private void Start() {

		_startPos = transform.localPosition;
	}

	private void Update() {

		if (_screenshakeValue > _maxValue) {
			_screenshakeValue = _maxValue;
		}

		var time = Time.timeSinceLevelLoad;
		var frac = time - Mathf.Floor(time);
		var fi = frac * Mathf.PI * _rotationSpeed;

		Vector2 offset = new Vector2(Mathf.Sin(fi), Mathf.Cos(fi));
		offset *= _screenshakeValue;

		transform.localPosition = _startPos + (Vector3)offset;

		_screenshakeValue *= _fade;
	}

	public void AddScreenshake(float value) {
		
		_screenshakeValue += value;
	}
}
