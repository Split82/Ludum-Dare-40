using UnityEngine;
using System.Collections;

public class ScreenshotRecorder : MonoBehaviour {

    public enum RecordingType {
        Sequence,
        F10ForScreenshot,
        Interval,
        ScreenshotOnPause,
    }

	[SerializeField] string _folder = "ScreenshotFolder";
	[SerializeField] int _frameRate = 60;
    [SerializeField] int _interval = 20;
    [SerializeField] RecordingType _recordingType = RecordingType.F10ForScreenshot;
	[SerializeField] int _superSize = 1;
    [SerializeField] bool _pauseWithPButton = true;

    private int _counter;
    private float _originalTimeScale;
    private bool _paused;

	private void Start() {
        
        if (_recordingType == RecordingType.Sequence) {
			Time.captureFramerate = _frameRate;
		}
		System.IO.Directory.CreateDirectory(_folder);

        _counter = _interval;
	}

	private void Update() {
        
        if (_recordingType == RecordingType.Sequence) {
            SaveScreenshot();
		}
        else if (_recordingType == RecordingType.Interval && _counter == 0) {
            SaveScreenshot();
            _counter = _interval;
        }
		else if (_recordingType == RecordingType.F10ForScreenshot && Input.GetKeyDown(KeyCode.F10)) {
            SaveScreenshot();
		}

        if (_counter > 0) {
            _counter--;
        }

        if (_pauseWithPButton && Input.GetKeyDown(KeyCode.P)) {
            _paused = !_paused;

            if (_paused) {
                _originalTimeScale = Time.timeScale;
                Time.timeScale = 0.0f;
            }
            else {
                Time.timeScale = _originalTimeScale;
            }
        }
	}

    private void OnApplicationFocus(bool hasFocus) {
        
        if (_recordingType == RecordingType.ScreenshotOnPause && hasFocus) {
            SaveScreenshot();
        }
    }

    private void SaveScreenshot() {		

        string name = string.Format("{0}/{1:D04} shot.png", _folder, Time.frameCount);
        ScreenCapture.CaptureScreenshot(name, _superSize);        
		Debug.Log("Screenshot saved to \"" + name + "\""); 
    }
}