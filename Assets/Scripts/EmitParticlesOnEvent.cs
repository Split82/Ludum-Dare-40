using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitParticlesOnEvent : MonoBehaviour {

	[SerializeField] PositionDirectionGameEvent _gameEvent;
	[SerializeField] ParticleSystem _particleSystem;
	[SerializeField] bool _ignoreDirection = false;

	private ParticleSystem.EmitParams _emitParams;

	private void Awake() {

		_emitParams = new ParticleSystem.EmitParams();
		_emitParams.applyShapeToPosition = true;
	}

	private void OnEnable() {

		_gameEvent.Subscribe(HandleEvent);
	}

	private void OnDisable() {

		_gameEvent.Unsubscribe(HandleEvent);
	}

	private void HandleEvent(object sender, PositionDirectionGameEvent gameEvent) {

		_emitParams.position = gameEvent.position;		
		if (_ignoreDirection == false) {
			_emitParams.rotation = Mathf.Atan2(-gameEvent.direction.y, -gameEvent.direction.x) * Mathf.Rad2Deg;
		}
		_particleSystem.Emit(_emitParams, 1);
	}
}
