using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefabOnEvent : MonoBehaviour {

	[SerializeField] PositionGameEvent _gameEvent;
	[SerializeField] GameObject _prefab;

	private void OnEnable() {

		_gameEvent.Subscribe(HandleEvent);
	}

	private void OnDisable() {

		_gameEvent.Unsubscribe(HandleEvent);
	}

	private void HandleEvent(object sender, PositionGameEvent gameEvent) {

		_prefab.Spawn(gameEvent.position);
	}
}
