using UnityEngine;

/// <summary>
/// Checkbox controller for the "power saving" state in options.
/// </summary>

[RequireComponent(typeof(UIToggle))]
public class UIOptionPower : MonoBehaviour
{
	public UILabel info;
	void OnClick () { info.text = Localization.Get("Power Saving Info"); }

	UIToggle mCheck;

	void Awake ()
	{
		mCheck = GetComponent<UIToggle>();
		EventDelegate.Add(mCheck.onChange, SaveState);
	}

	void OnEnable () { mCheck.value = PlayerProfile.powerSavingMode; }
	void SaveState () { PlayerProfile.powerSavingMode = UIToggle.current.value; }
}
