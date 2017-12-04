using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	[SerializeField] GameObject _scoreBar;
	[SerializeField] Text _scoreText;
	[SerializeField] Text _highscoreText;

	public static bool showScoreBar = false;

	private void Start() {
		
		_scoreBar.SetActive(showScoreBar || ScoreManager.highscore > 0);
		_scoreText.text = "SCORE: " + ScoreManager.lastScore.ToString();
		_highscoreText.text = "BEST: " + ScoreManager.highscore.ToString();
	}

	private void Update() {

		if (Input.anyKeyDown) {
			SceneManager.LoadScene("Main");
		}
	}
}
