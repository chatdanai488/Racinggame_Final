using UnityEngine;

/// <summary>
/// Demo script used to increase the player's experience in the Game Scene just to show experience gain.
/// </summary>

public class UIAddExperience : MonoBehaviour
{
	void OnClick ()
	{
		PlayerProfile.experience = PlayerProfile.experience + Random.Range(30000, 70000);
	}
}
