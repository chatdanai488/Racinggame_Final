using UnityEngine;

/// <summary>
/// This script makes it possible to choose a game type as well as to start the actual game.
/// </summary>

[RequireComponent(typeof(UIButton))]
public class UIPlayButton : MonoBehaviour
{
	public enum Type
	{
		None,
		Single,
		Multi,
	}

	static public Type choice = Type.None;

	/// <summary>
	/// Game type.
	/// </summary>

	public Type type = Type.Single;

	/// <summary>
	/// Whether to start the game on click, or just set the game type.
	/// </summary>

	public bool startOnClick = false;

	/// <summary>
	/// Whether the button will be disabled in the web player (hosting is not allowed in the web browser).
	/// </summary>

	public bool disableInWebPlayer = false;

	/// <summary>
	/// Whether Wifi must be enabled (for multiplayer games)
	/// </summary>

	public bool requireWifi = false;

	/// <summary>
	/// If the button's type is "none" it simply means "use previously chosen type".
	/// Final Play button is set up like that so that it simply starts the game using a previously chosen selection.
	/// </summary>

	public Type chosenType { get { return (type != Type.None) ? type : choice; } }

	UIButton mButton;
	bool mForceDisable = false;

	void Awake ()
	{
		mButton = GetComponent<UIButton>();
#if UNITY_WEBPLAYER || UNITY_FLASH
		if (disableInWebPlayer)
		{
			mButton.isEnabled = false;
			enabled = false;
		}
#endif
	}

	void OnEnable ()
	{
		mForceDisable = (requireWifi && !PlayerProfile.allowedToAccessInternet);
	}

	void Update ()
	{
		if (mForceDisable) mButton.isEnabled = false;
		else mButton.isEnabled = (!startOnClick || ((int)chosenType < (int)Type.Multi));
	}

	void OnClick ()
	{
		if (startOnClick)
		{
			switch (chosenType)
			{
				case Type.Single:
				GameManager.StartSingleGame();
				break;

				case Type.Multi:
				Debug.LogError("You need the TNet version of the package to play multiplayer.");
				break;
#if UNITY_EDITOR
				default:
				Debug.Log("No game type selected");
				break;
#endif
			}
		}
		else choice = type;
	}
}
