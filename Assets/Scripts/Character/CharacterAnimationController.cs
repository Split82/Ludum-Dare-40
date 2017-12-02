using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour {

	[SerializeField] BasicGameEvent _charactedStartedJumpEvent;
	[SerializeField] CharacterMovementController _characterMovementController;
	[SerializeField] Animator _animator;
	[SerializeField] Transform _graphicsTransform;
	[Space]
	[SerializeField] float _runningSpeedThreshold = 0.1f;
	[SerializeField] float _jumpRotationSpeed = 180.0f;
	[SerializeField] AnimationCurve _jumpRotationCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	
	private int _runningID;	
	
	private void Start() {

		_runningID = Animator.StringToHash("Running");		
	}

	private void OnEnable() {

		_charactedStartedJumpEvent.Subscribe(HandleCharactedStartedJumpEvent);
	}

	private void OnDisable() {
		_charactedStartedJumpEvent.Unsubscribe(HandleCharactedStartedJumpEvent);
	}
		
	private void Update() {
 			
		var isRunning = _characterMovementController.grounded && Mathf.Abs(_characterMovementController.velocity.x) > _runningSpeedThreshold;
		_animator.SetBool(_runningID, isRunning);	

		_graphicsTransform.localScale = new Vector3(_characterMovementController.movingDirection == CharacterMovementController.MovingDirection.Left ? 1.0f : -1.0f, 1.0f, 1.0f);
	}

	private void HandleCharactedStartedJumpEvent(object sender, BasicGameEvent gameEvent) {

		StopAllCoroutines();
		StartCoroutine(JumpRotationCoroutine());
	}

	private IEnumerator JumpRotationCoroutine() {

		float progress = 0.0f;		
		var yieldInstruction = new WaitForEndOfFrame();
		while (progress < 1) {
			float flipRotation = _characterMovementController.movingDirection == CharacterMovementController.MovingDirection.Left ? 1.0f : -1.0f;
			_graphicsTransform.localEulerAngles = new Vector3(0.0f, 0.0f, _jumpRotationCurve.Evaluate(progress) * flipRotation * 360.0f);
			progress += Time.deltaTime * _jumpRotationSpeed;
			yield return yieldInstruction;
		}
		
		_graphicsTransform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);	
	}
}
