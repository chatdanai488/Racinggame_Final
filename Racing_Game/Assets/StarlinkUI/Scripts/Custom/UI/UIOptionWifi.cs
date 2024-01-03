using UnityEngine;

/// <summary>
/// Checkbox controller for the "wifi" state in options.
/// </summary>

[RequireComponent(typeof(UIToggle))]
public class UIOptionWifi : MonoBehaviour
{
	public UILabel info;
	void OnClick () { info.text = Localization.Get("Carrier Info"); }

#if UNITY_ANDROID || UNITY_IPHONE
	UIToggle mCheck;

	void Awake ()
	{
		mCheck = GetComponent<UIToggle>();
		EventDelegate.Add(mCheck.onChange, SaveState);
	}

	void OnEnable () { mCheck.value = PlayerProfile.allow3G; }
	void SaveState () { PlayerProfile.allow3G = UIToggle.current.value; }
#else
	void Awake () { NGUITools.SetActive(gameObject, false); }
#endif
}
