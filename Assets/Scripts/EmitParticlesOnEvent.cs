using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitParticlesOnEvent : MonoBehaviour {

	[SerializeField] PositionDirectionGameEvent _gameEvent;
	[SerializeField] ParticleSystem _particleSystem;
	[SerializeField] bool _useDirectionForRotationStart = false;
	[SerializeField] bool _useDirectionForEmmisionShapeAngle = false;
	[SerializeField] float _directionShapeRotationOffset = 0.0f;
	[SerializeField] bool _flipEmissionShapeAngleBasedOnDirection = false;
	[SerializeField] int _minParticlesCount = 1;
	[SerializeField] int _maxParticlesCount = 1;

	private ParticleSystem.EmitParams _emitParams;
	private ParticleSystem.ShapeModule _shapeModule;

	private void Awake() {

		_emitParams = new ParticleSystem.EmitParams();
		_emitParams.applyShapeToPosition = true;

		_shapeModule = _particleSystem.shape;
	}

	private void OnEnable() {

		_gameEvent.Subscribe(HandleEvent);
	}

	private void OnDisable() {

		_gameEvent.Unsubscribe(HandleEvent);
	}

	private void HandleEvent(object sender, PositionDirectionGameEvent gameEvent) {

		_emitParams.position = gameEvent.position;		
		var angle = Mathf.Atan2(-gameEvent.direction.y, -gameEvent.direction.x) * Mathf.Rad2Deg;
		if (_useDirectionForRotationStart) {
			_emitParams.rotation = angle;
		}
		if (_useDirectionForEmmisionShapeAngle) {
			if (_flipEmissionShapeAngleBasedOnDirection && gameEvent.direction.x > 0.0f) {
				_shapeModule.rotation = new Vector3(0.0f, 0.0f, angle - _directionShapeRotationOffset - _shapeModule.arc);			
			}
			else {
				_shapeModule.rotation = new Vector3(0.0f, 0.0f, angle + _directionShapeRotationOffset);
			}
		}
		_particleSystem.Emit(_emitParams, Random.Range(_minParticlesCount, _maxParticlesCount + 1));
	}
}
