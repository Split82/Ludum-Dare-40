using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEffect : MonoBehaviour {

	[SerializeField] FloatVariable _fadeOutValue;
	[SerializeField] Transform _topElement;
	[SerializeField] Transform _bottomElement;

	private float _prevValue = -1.0f;

	private void Update() {

		var newValue = _fadeOutValue.value;
		if (_prevValue != newValue) {

			var scale = new Vector3(1.0f, 1.0f - newValue, 1.0f);
			_topElement.localScale = scale;
			_bottomElement.localScale = scale;
			_prevValue = newValue;
		}
	}
}
