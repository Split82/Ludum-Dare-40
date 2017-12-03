using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PixelPerfectCamera : MonoBehaviour {
	
	[SerializeField] Camera[] _cameras;
	[SerializeField] int _referenceHeight = 800;
	[SerializeField] int _scale = 4;
	[SerializeField] int pixelsPerUnit = 48;

	private float _offset;
	private Transform _parentTransform;

	private void Awake() {

		_parentTransform = transform.parent;
		RefreshSizesAndAlign();
	}

	private void LateUpdate() {

		RefreshSizesAndAlign();
	}

	private void RefreshSizesAndAlign() {

		Vector3 parentPos = _parentTransform.position;
		parentPos.x = (Mathf.Round(parentPos.x * pixelsPerUnit * _scale) + 0.5f) / (pixelsPerUnit * _scale);
		parentPos.y = (Mathf.Round(parentPos.y * pixelsPerUnit * _scale) + 0.5f) / (pixelsPerUnit * _scale);

		float newScale = Mathf.Max(Mathf.RoundToInt(Screen.height * _scale / (float)_referenceHeight), 1);

		for (int i = 0; i < _cameras.Length; i++) {
			_cameras[i].orthographicSize = (Screen.height / (float)pixelsPerUnit) / (float)newScale * 0.5f;
			_cameras[i].transform.position = parentPos;
		}
	}
}
