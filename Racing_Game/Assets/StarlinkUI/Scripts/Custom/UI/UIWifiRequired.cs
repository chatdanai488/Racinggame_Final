using UnityEngine;

[RequireComponent(typeof(UIButton))]
public class UIWifiRequired : MonoBehaviour
{
	UIButton mButton;

	void Awake ()
	{
		mButton = GetComponent<UIButton>();
	}

	void OnEnable ()
	{
		mButton.isEnabled = PlayerProfile.allowedToAccessInternet;
	}
}