using UnityEngine;

/// <summary>
/// Checkbox controller for the "caps" state in options.
/// </summary>

[RequireComponent(typeof(UIToggle))]
public class UIOptionCaps : MonoBehaviour
{
	public UILabel info;
	void OnClick () { info.text = Localization.Get("Caps Info"); }

	UIToggle mCheck;

	void Awake () { mCheck = GetComponent<UIToggle>(); }

	void OnEnable ()
	{
		EventDelegate.Add(mCheck.onChange, SaveState);
		mCheck.value = (Localization.language == "Caps");
	}

	void OnDestroy ()
	{
		EventDelegate.Remove(mCheck.onChange, SaveState);
	}
	
	void SaveState ()
	{
		Localization.language = UIToggle.current.value ? "Caps" : "English";
	}
}
