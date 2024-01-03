using UnityEngine;

/// <summary>
/// Attach this to a label to display current game time.
/// If you set the "show remaining" flag to 'true', the timer will count backwards instead,
/// and will appear once the "show threshold" has been reached. For example, treshold of
/// 120 means that the timer will appear once there are 2 minutes left until the game ends.
/// </summary>

public class UIGameTime : MonoBehaviour
{
	public UILabel label;
	public UISprite background;
	public bool showRemaining = false;
	public int showThreshold = 0;

	int mLast = -1;

	void OnEnable ()
	{
		if (label == null) label = GetComponent<UILabel>();
		Update();
	}

	void Update ()
	{
		int sec = Mathf.FloorToInt(showRemaining ? GameManager.timeLimit - GameManager.gameTime : GameManager.gameTime);
		if (sec < 0) sec = 0;
		int min = sec / 60;

		if (showThreshold != 0)
		{
			if (sec > showThreshold)
			{
				if (label.enabled)
				{
					label.enabled = false;
					if (background != null) background.enabled = false;
				}
				return;
			}
			else if (!label.enabled)
			{
				// Show the countdown timer
				label.enabled = true;
				if (background != null) background.enabled = true;
				var anim = GetComponent<Animation>();
				if (anim != null) anim.Play();
			}
		}

		if (mLast != sec)
		{
			mLast = sec;
			sec = sec - min * 60;
			label.text = min + ((sec < 10) ? ":0" : ":") + sec;
		}
	}
}
