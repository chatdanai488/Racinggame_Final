using UnityEngine;

/// <summary>
/// Checkbox controller for the "hints" state in options.
/// </summary>

[RequireComponent(typeof(UIToggle))]
public class UIOptionHints : MonoBehaviour
{
	public UILabel info;
	void OnClick () { info.text = Localization.Get("Hints Info"); }

	UIToggle mCheck;

	void Awake ()
	{
		mCheck = GetComponent<UIToggle>();
		EventDelegate.Add(mCheck.onChange, SaveState);
	}

	void OnEnable () { mCheck.value = PlayerProfile.hints; }
	void SaveState () { PlayerProfile.hints = UIToggle.current.value; }
}
