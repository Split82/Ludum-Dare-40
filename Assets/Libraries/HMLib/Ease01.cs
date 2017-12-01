using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[CreateAssetMenu(fileName = "Ease01", menuName = "HMLib/Others/Ease01")]
public class Ease01 : ScriptableObject {

    [SerializeField] FloatVariable _variable;	
	[SerializeField] AnimationCurve _curve;
	[SerializeField] float _defaultFadeOutDuration = 1.5f;
	[SerializeField] float _defaultFadeInDuration = 0.7f;
	
	public void FadeIn() {
        
		FadeIn(_defaultFadeInDuration);
	}

	public void FadeOut() {
        
		FadeOut(_defaultFadeOutDuration);
	}
	
	public void FadeIn(float duration) {
        
		SharedCoroutineStarter.instance.StartCoroutine(Fade(0.0f, 1.0f, duration));		
	}

	public void FadeOut(float duration) {
        
		SharedCoroutineStarter.instance.StartCoroutine(Fade(_variable.value, 0.0f, duration));
	}

	private IEnumerator Fade(float fromValue, float toValue, float duration) {

		YieldInstruction waitForEndOfFrame = new WaitForEndOfFrame();

		float startTime = Time.timeSinceLevelLoad;
		while (Time.timeSinceLevelLoad - startTime < duration) {
			_variable.value = Mathf.Lerp(fromValue, toValue, _curve.Evaluate((Time.timeSinceLevelLoad - startTime) / duration));
			yield return waitForEndOfFrame;
		}

		_variable.value = toValue;
	}
}