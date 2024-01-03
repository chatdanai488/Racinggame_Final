using UnityEngine;

/// <summary>
/// Attach this script to a button that you want to act as a pause button in the game.
/// </summary>

public class UIPauseButton : MonoBehaviour
{
	public bool animateWhenPaused = true;

	Transform mTrans;
	bool mPaused = false;
	Vector3 mFrom = new Vector3(1f, 1f, 1f);
	Vector3 mTo = new Vector3(1.25f, 1.25f, 1.25f);

	void Awake () { mTrans = transform; }

	void Start ()
	{
		if (GameManager.gameType == GameManager.GameType.Multiplayer)
			NGUITools.SetActiveChildren(gameObject, false);
	}

	void OnClick ()
	{
		if (mPaused)
		{
			mPaused = false;
			GameManager.Unpause();
		}
		else if (GameManager.gameType != GameManager.GameType.Multiplayer)
		{
			mPaused = true;
			GameManager.Pause();
		}
	}

	void Update ()
	{
		if (animateWhenPaused)
		{
			if (mPaused || Time.timeScale < 0.1f)
			{
				float factor = Mathf.PingPong(Time.realtimeSinceStartup, 0.5f);
				mTrans.localScale = Vector3.Lerp(mTrans.localScale, Vector3.Lerp(mFrom, mTo, factor), 1f - Time.timeScale);
			}
			else mTrans.localScale = mFrom;
		}
	}
}
