using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnEvent : MonoBehaviour {

	[SerializeField] GameEvent _gameEvent;
	[SerializeField] AudioSource _audioSource;
	[SerializeField] AudioClip[] _audioClips;
	[SerializeField] float _volume = 1.0f;
	private RandomObjectPicker<AudioClip> _randomObjectPicker;

	private void Awake() {

		_randomObjectPicker = new RandomObjectPicker<AudioClip>(_audioClips, 0.1f);		
	}

	private void OnEnable() {
		_gameEvent.Subscribe(HandleGameEvent);
	}

	private void OnDisable() {
		_gameEvent.Unsubscribe(HandleGameEvent);
	}

	private void HandleGameEvent(object obj, GameEvent gameEvent) {		
		
		var audioClip = _randomObjectPicker.PickRandomObject();
		
		if (audioClip) {
			_audioSource.pitch = Random.Range(0.9f, 1.1f);
			_audioSource.PlayOneShot(audioClip, _volume);
		}
	}
}
