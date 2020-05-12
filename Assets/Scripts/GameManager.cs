using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public delegate void GameDelegate();
	public static event GameDelegate OnGameStarted;
	public static event GameDelegate OnGameOverConfirmed;

	public static GameManager Instance;

	public GameObject startPage;
	public GameObject gameOverPage;
	public GameObject countdownPage;
	public Text scoreText;

	enum PageState{
		none,
		start,
		gameOver,
		countdown
	}

	int score = 0;
	bool gameOver = true;

	public bool GameOver { get {return gameOver; } }
	public int Score { get { return score; } }

	void Awake(){
		Instance = this;
	}

	void OnEnable(){
		Countdowntext.OnCountdownFinished += OnCountdownFinished;
		TapController.OnPlayerDied += OnPlayerDied;
		TapController.OnPlayerScored += OnPlayerScored;
	}

	void OnDisable(){
		Countdowntext.OnCountdownFinished -= OnCountdownFinished;
		TapController.OnPlayerDied -= OnPlayerDied;
		TapController.OnPlayerScored -= OnPlayerScored;
	}

	void OnCountdownFinished(){
		SetPageState (PageState.none);
		OnGameStarted ();
		score = 0;
		gameOver = false;
	}

	void OnPlayerDied(){
		gameOver = true;
		int savedScore = PlayerPrefs.GetInt ("HighScore");
		if (score > savedScore) {
			PlayerPrefs.SetInt ("HighScore", score);
		}
		SetPageState (PageState.gameOver);
	}

	void OnPlayerScored(){
		score++;
		scoreText.text = score.ToString();
	}

	void SetPageState(PageState state){
		switch (state) {
		case PageState.none:
			startPage.SetActive (false);
			gameOverPage.SetActive (false);
			countdownPage.SetActive (false);
			break;
		case PageState.start:
			startPage.SetActive (true);
			gameOverPage.SetActive (false);
			countdownPage.SetActive (false);
			break;
		case PageState.gameOver:
			startPage.SetActive (false);
			gameOverPage.SetActive (true);
			countdownPage.SetActive (false);
			break;
		case PageState.countdown:
			startPage.SetActive (false);
			gameOverPage.SetActive (false);
			countdownPage.SetActive (true);
			break;	
		}
	}

	public void ConfirmGameOver(){
		OnGameOverConfirmed ();
		scoreText.text = "0";
		SetPageState (PageState.start);
	}

	public void StartGame(){
		SetPageState (PageState.countdown);
	}


}
