using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshakeSpawner : MonoBehaviour {
	
	[SerializeField] Screenshake _screenshake;
	[Space]
	[SerializeField] EventStrengthCouple[] _eventStrengthCouples;

	[System.Serializable]
	public class EventStrengthCouple {
		public GameEvent gameEvent;
		public float strength;
	}

	private Dictionary<GameEvent, float> _eventStrengthDict;

	private void Awake() {

		_eventStrengthDict = new Dictionary<GameEvent, float>();
		foreach (EventStrengthCouple eventStrengthCouple in _eventStrengthCouples) {
			_eventStrengthDict[eventStrengthCouple.gameEvent] = eventStrengthCouple.strength;
		}
	}

	private void OnEnable() {

		foreach (EventStrengthCouple eventStrengthCouple in _eventStrengthCouples) {
			eventStrengthCouple.gameEvent.Subscribe(HandleEvent);
		}
	}

	private void OnDisable() {

		foreach (EventStrengthCouple eventStrengthCouple in _eventStrengthCouples) {
			eventStrengthCouple.gameEvent.Unsubscribe(HandleEvent);
		}
	}

	private void HandleEvent(object obj, GameEvent gameEvent) {

		float strength = _eventStrengthDict[gameEvent];
		_screenshake.AddScreenshake(strength);
	}
}