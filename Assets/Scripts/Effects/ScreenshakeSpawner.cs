using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshakeSpawner : MonoBehaviour {
	
	[SerializeField] Screenshake _screenshake;
	[Space]
	[SerializeField] GameEvent[] _gameEvents;
	[SerializeField] float _strength = 0.1f;

	private void OnEnable() {

		foreach (GameEvent gameEvent in _gameEvents) {
			gameEvent.Subscribe(HandleEvent);
		}
	}

	private void OnDisable() {

		foreach (GameEvent gameEvent in _gameEvents) {
			gameEvent.Unsubscribe(HandleEvent);
		}
	}

	private void HandleEvent(object obj, GameEvent gameEvent) {

		_screenshake.AddScreenshake(_strength);
	}
}