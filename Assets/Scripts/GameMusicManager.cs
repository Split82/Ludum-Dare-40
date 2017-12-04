using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicManager : MonoBehaviour {

	[SerializeField] FloatVariable _fadeOutValue;
	[SerializeField] AudioSource _audioSource;
	[SerializeField] float _volume = 1.0f;
	private float _prevValue = -1.0f;

	private void Update() {

		var newValue = _fadeOutValue.value;
		if (_prevValue != newValue) {

			_audioSource.volume = newValue * _volume;
		}
	}
}
