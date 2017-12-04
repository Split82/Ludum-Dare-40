using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRunningPS : MonoBehaviour {

	[SerializeField] ParticleSystem _particleSystem;
	[SerializeField] CharacterMovementController _movementController;
	[SerializeField] float _thresholdVelocity = 3.0f;

	private ParticleSystem.EmissionModule _emissionModule;
	
	private bool isEmitting;

	private void Start() {
		
		_emissionModule = _particleSystem.emission;
	}
		
	private void Update() {
		
		var shouldEmit = Mathf.Abs(_movementController.velocity.x) > _thresholdVelocity && _movementController.isTouchingGround;
		if (isEmitting != shouldEmit) {
			_emissionModule.enabled = shouldEmit;
			isEmitting = shouldEmit;
		}
	}
}
