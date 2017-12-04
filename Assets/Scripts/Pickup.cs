using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

	[EventSender] [SerializeField] PositionGameEvent _wasPickedUpEvent;
	[SerializeField] Collider2D _collider;
	[SerializeField] SpriteRenderer _spriteRenderer;
	[SerializeField] Animator _animator;

	private void OnEnable() {
		
		_collider.enabled = true;
		_spriteRenderer.enabled = true;
		_animator.SetBool("Active", true);
	}

    private void OnTriggerEnter2D(Collider2D other) {

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
