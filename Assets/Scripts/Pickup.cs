using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

	[EventSender] [SerializeField] PositionGameEvent _wasPickedUpEvent;
	[SerializeField] Collider2D _collider;
	[SerializeField] SpriteRenderer _spriteRenderer;
	[SerializeField] Animator _animator;

	private bool _wasPickedUp;

	private void OnEnable() {
		
		_collider.enabled = true;
		_spriteRenderer.enabled = true;
		_animator.SetBool("Active", true);
		_wasPickedUp = false;
	}

    private void OnTriggerEnter2D(Collider2D other) {

		if (_wasPickedUp) {
			return;
		}
		_wasPickedUp = true;

		_collider.enabled = false;
		_spriteRenderer.enabled = false;
		_wasPickedUpEvent.position = transform.position;
		_wasPickedUpEvent.Raise(this, _wasPickedUpEvent);
		StartCoroutine(HideCoroutine());
    }

	private IEnumerator HideCoroutine() {

		_animator.SetBool("Active", false);
		yield return new WaitForSeconds(0.5f);
		this.Recycle();
	}
}
