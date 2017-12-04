using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class Snap : MonoBehaviour {
	
	[SerializeField] Vector3 snap = new Vector3(1.0f, 1.0f, 1.0f);
	[SerializeField] Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);
	[SerializeField] bool _localPosition = false;
	
	public void SnapPosition () {

		if (Application.isPlaying) {
			return;
		}

		if (snap.x == 0 || snap.y == 0 || snap.z == 0) {
			return;
		}

		Vector3 pos = _localPosition ? transform.localPosition : transform.position;
		pos.x = Mathf.Round(pos.x / snap.x) * snap.x;
		pos.y = Mathf.Round(pos.y / snap.y) * snap.y;
		pos.z = Mathf.Round(pos.z / snap.z) * snap.z;
		if (_localPosition) {
			transform.localPosition = pos + offset;	
		} 
		else {
			transform.position = pos + offset;				
		}
	}
}
