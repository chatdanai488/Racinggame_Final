using UnityEngine;

/// <summary>
/// This script is attached to the "Upgrade" button.
/// </summary>

[RequireComponent(typeof(UIButton))]
public class UIUpgradeButton : MonoBehaviour
{
	UIButton mButton;

	void OnEnable () { OnAccessLevelChanged(); }
	void OnAccessLevelChanged () { GetComponent<UIButton>().isEnabled = !PlayerProfile.fullAccess; }

	void OnClick ()
	{
		// It's up to you to determine what happens when the upgrade button gets clicked on.
		// In Starlink's case, I would perform an in-app transaction or open a specific URL.
		PlayerProfile.fullAccess = true;
		UIMessageBox.Show(Localization.Get("Upgraded"), Localization.Get("Upgraded Info"));
	}
}
