using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class FadeInOnStart : MonoBehaviour {

	[SerializeField] Ease01 _ease01;	

	private void Start() {
		
		_ease01.FadeIn();
	}
}
