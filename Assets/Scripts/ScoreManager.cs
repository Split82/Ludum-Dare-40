using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager {

	private const string kHighscoreKey = "Highscore";
	private const string kLastScoreKey = "LastScore";

	public static int highscore {
		get {
			return PlayerPrefs.GetInt(kHighscoreKey, 0);
		}
	}

	public static int lastScore {
		get {
			return PlayerPrefs.GetInt(kLastScoreKey, 0);
		}
	}

	public static void PushScore(int score) {

		PlayerPrefs.SetInt(kLastScoreKey, score);

		if (score > highscore) {
			PlayerPrefs.SetInt(kHighscoreKey, score);
		}
	}
}
