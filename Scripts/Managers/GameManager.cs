using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public static int level = 0;

	private int score = 0;
	private int scoreSize = 14;

	private int scoreToEnlarge = 0;
	public int enlargeScoreRequired;

	private bool playerIsDead = false;

	public Text scoreText;
	public GameObject restartText;

	private bool wasPaused = false;
	private bool isPaused = false;

	public GameObject pausedScreen;


	void Awake () {
		instance = this;
		restartText.SetActive(false);

		level = 0;

		BlockBehavior.blockLists.Clear();
		BlockBehavior.deadLists.Clear();
		BlockBehavior.crazyInProgress = false;

	}

	void Update () {
		if (playerIsDead) {
			if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Space)) {
				SceneManager.LoadScene("Main Menu");
			}	
		}

		if (isPaused && !wasPaused) {
			wasPaused = true;
			pausedScreen.SetActive(true);
			Time.timeScale = 0;
		}

		if (isPaused && Input.GetKey(KeyCode.Space)) {
			isPaused = false;
			wasPaused = false;
			pausedScreen.SetActive(false);
			Time.timeScale = 1f;
		}

//		if (Input.GetMouseButtonDown(0))
//			AddScore(enlargeScoreRequired);
	}

	public void AddScore (int scoreToAdd) {
		score += scoreToAdd;

		string scoreTemp = score.ToString();
		string scoreString = "";

		for (int i = 0; i < scoreSize - scoreTemp.Length; i++) {
			scoreString += "0";
		}

		scoreString += scoreTemp;

		scoreText.text = scoreString;

		scoreToEnlarge += scoreToAdd;

		if (level < 7 && scoreToEnlarge >= enlargeScoreRequired) {
			scoreToEnlarge = 0;
			enlargeScoreRequired *= 4;
			level += 1;
			BlockSpawn.instance.EnlargeArena();
			BackgroundManager.instance.Enlarge();
		}
	}

	public void PlayerDied () {
		CoinStack.instance.LoseAllCoins();
		restartText.SetActive(true);
		playerIsDead = true;

	}

	void OnApplicationFocus (bool focus) {
		isPaused = focus;
	}
}
