using UnityEngine;

/// <summary>
/// Offline chat window for the game. It simply adds the typed text to the chat window.
/// </summary>

public class UIGameChat : UIChat
{
	/// <summary>
	/// Sound to play when a new message arrives.
	/// </summary>

	public AudioClip notificationSound;

	/// <summary>
	/// Add the player's message to the chat window.
	/// </summary>

	protected override void OnSubmit (string text)
	{
		text = string.Format("[{0}]: {1}", PlayerProfile.playerName, text);
		Add(text, Color.white);
		NGUITools.PlaySound(notificationSound);
		UIInput.current.isSelected = false;
	}
}
