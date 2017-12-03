using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealPixelPerfectCamera : MonoBehaviour {

	[SerializeField] int _heightFraction = 4;

	private Camera _thisCamera;
	private Camera _pixelCamera;
	private RenderTexture _renderTexture;
	
	private void Start() {

		_thisCamera = GetComponent<Camera>();
		
		var pixelCameraGO = new GameObject("PixelCamera");
		pixelCameraGO.transform.parent = transform;
		_pixelCamera = pixelCameraGO.AddComponent<Camera>();
		_pixelCamera.CopyFrom(_thisCamera);
		_pixelCamera.enabled = false;

		var pixelsHeight = _thisCamera.pixelHeight / _heightFraction;
		var _renderTexture = new RenderTexture(Mathf.RoundToInt(_thisCamera.aspect * pixelsHeight), pixelsHeight, 24, RenderTextureFormat.ARGB32);
		_renderTexture.filterMode = FilterMode.Point;
		_pixelCamera.targetTexture = _renderTexture;		

		// This camera doesn't render anything.
		_thisCamera.cullingMask = 0;
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dst) {

		_pixelCamera.Render();
		Graphics.Blit(_pixelCamera.targetTexture, dst);
	}
}
