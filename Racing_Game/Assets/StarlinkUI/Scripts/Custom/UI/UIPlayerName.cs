using UnityEngine;

/// <summary>
/// Script controlling player name entries that show up on the left-hand side in the game.
/// </summary>

public class UIPlayerName : MonoBehaviour
{
	public string playerName = "Guest";
	public UISprite background;
	public UISprite icon;
	public UILabel label;

	bool mIsVisible = false;

	public void UpdateInfo (bool isVisible)
	{
		icon.spriteName = "Circle - Human";
		label.text = playerName;

		Color c = Color.white;
		icon.color = c;

		c.a = label.alpha;
		label.color = c;

		if (mIsVisible != isVisible)
		{
			mIsVisible = isVisible;
			TweenAlpha.Begin(icon.gameObject, 0.25f, isVisible ? 0f : 1f).method = UITweener.Method.EaseInOut;
		}
	}
}
