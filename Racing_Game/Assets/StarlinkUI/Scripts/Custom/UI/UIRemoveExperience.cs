using UnityEngine;

public class UIRemoveExperience : MonoBehaviour
{
	void OnClick ()
	{
		PlayerProfile.experience = 0;
	}
}
