using UnityEngine;

/// <summary>
/// Have you ever wanted to have a saved option drop-down list where some choices are not valid unless some condition has been met?
/// This class is a great starting point -- it will validate the selection, and will only allow choices from the list of valid ones.
/// If the selection is not valid, an upgrade window will be shown instead.
/// </summary>

[RequireComponent(typeof(UIPopupList))]
public class UILimitedSavedOption : MonoBehaviour
{
	/// <summary>
	/// Key in PlayerPrefs where the last selection will be saved so that it can be automatically restored the next time you run the game.
	/// Make it unique and something simple, such as "Difficulty".
	/// </summary>

	public string keyName = "Key Name";

	/// <summary>
	/// List of valid choices that will be valid regardless of whether the game is full or not.
	/// </summary>

	public string[] validChoices;

	UIPopupList mList;

	/// <summary>
	/// Check to see if the specified choice is present in the list of valid choices.
	/// </summary>

	bool IsValid (string choice)
	{
		for (int i = 0; i < validChoices.Length; ++i)
			if (validChoices[i] == choice)
				return true;
		return false;
	}

	/// <summary>
	/// Register the selection change listener.
	/// </summary>

	void Awake ()
	{
		mList = GetComponent<UIPopupList>();
		EventDelegate.Add(mList.onChange, OnSelection);
	}

	/// <summary>
	/// Load the last selection.
	/// </summary>

	void OnEnable ()
	{
		string s = PlayerPrefs.GetString(keyName);
		if (!string.IsNullOrEmpty(s)) mList.value = s;
	}

	/// <summary>
	/// Validate the selection.
	/// </summary>

	void OnSelection ()
	{
		if (PlayerProfile.fullAccess || IsValid(UIPopupList.current.value))
		{
			// The selection is valid -- save it
			PlayerPrefs.SetString(keyName, UIPopupList.current.value);
		}
		else
		{
			// The selection is not valid. Change the selection to a valid one and show the upgrade window.
			mList.value = validChoices[validChoices.Length - 1];
			UIUpgradeWindow.Show();
		}
	}
}
