using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlashEffect : MonoBehaviour {

	[SerializeField] SpriteRenderer[] _spriteRenderers;
	[SerializeField] Material _flashMaterial;
	private Material[] _originalMaterials;
	
	private void Start() {
		
		_originalMaterials = new Material[_spriteRenderers.Length];

		for (int i = 0; i < _spriteRenderers.Length; i++) {
			_originalMaterials[i] = _spriteRenderers[i].material;
		}			
	}
	
	public void Flash(float duration) {
		
		StartCoroutine(FlashCoroutine(duration));
	}

	private IEnumerator FlashCoroutine(float duration) {

		for (int i = 0; i < _spriteRenderers.Length; i++) {
			_spriteRenderers[i].material = _flashMaterial;
		}
		
		yield return new WaitForSeconds(duration);

		for (int i = 0; i < _spriteRenderers.Length; i++) {
			_spriteRenderers[i].material = _originalMaterials[i];
		}
	}
}
