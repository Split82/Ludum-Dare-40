using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour {

	[SerializeField] Text _text;

	private IEnumerator Start() {

		for(;;) {
			_text.enabled = !_text.enabled;
			yield return new WaitForSeconds(0.3f);
		}
	}	
}
