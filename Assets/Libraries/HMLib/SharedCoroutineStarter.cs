using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SharedCoroutineStarter : MonoBehaviour {

	public static SharedCoroutineStarter instance {
		get {
			if (_instance == null) {
				var go = new GameObject("SharedCoroutineStarter");
				_instance = go.AddComponent<SharedCoroutineStarter>();
			}
			return _instance;
		}
	}

	private static SharedCoroutineStarter _instance;
}
