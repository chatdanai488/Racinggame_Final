using UnityEngine;

/// <summary>
/// This script, when attached to a label, will download news as soon as it's enabled,
/// and will display the news by changing the label's text.
/// </summary>

[RequireComponent(typeof(UILabel))]
public class UINews : MonoBehaviour
{
	public string url = "http://misc.tasharen.com/news.txt";

	UILabel mLabel;
	string mData = null;

	void Awake ()
	{
		mLabel = GetComponent<UILabel>();
		mLabel.text = Localization.Get("Loading News");
	}

	void OnEnable ()
	{
		if (string.IsNullOrEmpty(mData))
		{
			if (PlayerProfile.allowedToAccessInternet)
			{
				GameWebRequest.Create(url, OnFinished);
			}
			else
			{
				mLabel.text = Localization.Get("Wifi Required");
			}
		}
	}

	void OnFinished (bool success, object obj, string text)
	{
		if (success)
		{
			mData = text;
			mLabel.text = text;
		}
		else
		{
			mLabel.text = Localization.Get("News Failed");
		}
		Destroy(this);
	}
}
