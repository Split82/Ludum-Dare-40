using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	[SerializeField] MainCameraProvider _mainCameraProvider;
	[Space]
	[SerializeField] Camera _camera;

	public void Awake() {

		_mainCameraProvider.Init(_camera);
	}
}
