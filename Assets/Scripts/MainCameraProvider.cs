using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraProvider : ScriptableObject {

	private Camera _camera;

	public Camera camera {
		get {
			return _camera;
		}
	}

	public void Init(Camera camera) {
		
		_camera = camera;
	}
}
