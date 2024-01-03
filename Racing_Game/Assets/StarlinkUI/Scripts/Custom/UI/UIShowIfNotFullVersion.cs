using UnityEngine;

public class UIShowIfNotFullVersion : MonoBehaviour
{
	void OnEnable ()
	{
		OnAccessLevelChanged();
	}

	void OnAccessLevelChanged ()
	{
		NGUITools.SetActiveChildren(gameObject, !PlayerProfile.fullAccess);
	}
}
