using UnityEngine;
using System;
using System.Net;

/// <summary>
/// Globally accessible variables and functions that don't really belong anywhere else.
/// </summary>

public class GameManager : MonoBehaviour
{
	static GameManager mInstance;
	
	public enum GameType
	{
		None,
		SinglePlayer,
		Multiplayer,
	}

	/// <summary>
	/// Random number generator to be used throughout the code.
	/// </summary>

	static public RandomGenerator random = new RandomGenerator();

	/// <summary>
	/// Type of the game being played.
	/// </summary>

	static public GameType gameType = GameType.None;

	/// <summary>
	/// Whether tooltips are going to be shown.
	/// </summary>

	static public bool enableTooltips = true;

	/// <summary>
	/// 15 minute limit.
	/// </summary>

	static public float timeLimit = 900f;

	/// <summary>
	/// Current elapsed game time. This value is synchronized with all connected players.
	/// </summary>

	static public float gameTime = 0f;

	// Number of times the game has been paused
	static int mPause = 0;
	static float mTargetTimeScale = 1f;

	/// <summary>
	/// PlayerPrefs-saved time limit.
	/// </summary>

	static float savedTimeLimit
	{
		get
		{
			string s = PlayerPrefs.GetString("Time Limit", "15");
			float val = 15f;
			float.TryParse(s, out val);
			return val * 60f;
		}
	}

	/// <summary>
	/// Pause the game.
	/// </summary>

	static public void Pause ()
	{
		++mPause;
		mTargetTimeScale = 0f;
	}

	/// <summary>
	/// Unpause the game.
	/// </summary>

	static public void Unpause ()
	{
		if (--mPause < 1)
		{
			mTargetTimeScale = 1f;
			mPause = 0;
		}
	}

	/// <summary>
	/// Start a new single player game.
	/// </summary>

	static public void StartSingleGame ()
	{
		if (mInstance != null)
		{
			gameType = GameType.SinglePlayer;
			timeLimit = savedTimeLimit;
			gameTime = 0f;
#if UNITY_4_7
			Application.LoadLevel("Game Scene");
#else
			UnityEngine.SceneManagement.SceneManager.LoadScene("Game Scene");
#endif		
		}
	}

	/// <summary>
	/// End the game in progress.
	/// </summary>

	static public void EndGame ()
	{
		if (gameType != GameType.None)
		{
			gameType = GameType.None;
			Time.timeScale = 0f;
			mTargetTimeScale = 0f;
			mPause = 0;

			// This would be a good place to show a match summary screen... but since I don't have one, just load the menu instead.
			LoadMenu();
		}
	}

	/// <summary>
	/// Forfeit the current game.
	/// </summary>

	static public void Forfeit ()
	{
		if (gameType != GameType.None) EndGame();
		else LoadMenu();
	}

	/// <summary>
	/// Load the main menu, ending the game in progress.
	/// </summary>

	static void LoadMenu ()
	{
		gameType = GameType.None;
		Time.timeScale = 1f;
		mTargetTimeScale = 1f;
		mPause = 0;

#if UNITY_4_7
		if (Application.loadedLevelName != "Menu Scene")
		{
			if (mInstance != null)
			{
				Destroy(mInstance);
				mInstance = null;
			}
			Application.LoadLevel("Menu Scene");
		}
#else
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Menu Scene")
		{
			if (mInstance != null)
			{
				Destroy(mInstance);
				mInstance = null;
			}
			UnityEngine.SceneManagement.SceneManager.LoadScene("Menu Scene");
		}
#endif
	}

	/// <summary>
	/// Set the instance reference.
	/// </summary>

	void Awake ()
	{
		if (mInstance == null)
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			Application.targetFrameRate = PlayerProfile.powerSavingMode ? 30 : 60;
			gameTime = 0f;
			mInstance = this;
		}
		else Destroy(this);
	}

	/// <summary>
	/// Clear the instance reference.
	/// </summary>

	void OnDestroy () { if (mInstance == this) mInstance = null; }

	/// <summary>
	/// Keep track of game time.
	/// </summary>

	void Update ()
	{
		Time.timeScale = Mathf.Lerp(Time.timeScale, mTargetTimeScale, 8f * RealTime.deltaTime);
		gameTime += Time.deltaTime;

		// Once the timer limit has been reached, end the game
		if (timeLimit > 0f && gameTime > timeLimit) EndGame();
	}
}
