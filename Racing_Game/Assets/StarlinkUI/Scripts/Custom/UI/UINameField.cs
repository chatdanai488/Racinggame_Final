using UnityEngine;

/// <summary>
/// Attach this script to an input if you want it to represent the player's name.
/// </summary>

[RequireComponent(typeof(UIInput))]
public class UINameField : MonoBehaviour
{
	UIInput mInput;

	void Awake ()
	{
		mInput = GetComponent<UIInput>();
		EventDelegate.Set(mInput.onSubmit, OnSubmit);
	}

	void OnEnable ()
	{
		mInput.value = PlayerProfile.playerName;
	}

	void OnSubmit ()
	{
		string text = UIInput.current.value;
		PlayerProfile.playerName = text;
		if (text != PlayerProfile.playerName)
			mInput.value = PlayerProfile.playerName;
	}
}
