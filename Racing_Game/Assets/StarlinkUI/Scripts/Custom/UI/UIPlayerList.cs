using UnityEngine;

/// <summary>
/// This script creates a visible list of players that are currently present in the game.
/// In this package, the "list" is just a single player since the game is not networked.
/// In the TNet version of the package, this class is a fair bit more advanced.
/// </summary>

public class UIPlayerList : MonoBehaviour
{
	// Tween triggered on click
	public TweenPosition tween;

	// Player entry prefab
	public GameObject prefab;

	// Instantiated prefab
	UIPlayerName mPlayer = null;
	bool mShown = false;

	/// <summary>
	/// Add the player to the list of players on the left.
	/// The TNet version of this package does a lot more here.
	/// </summary>

	void Start ()
	{
		GameObject parent = tween.gameObject;

		// Add the player. The TNet version of the package does a lot more here,
		// but since this version doesn't use networking, there is only one player to work with.
		GameObject go = NGUITools.AddChild(tween.gameObject, prefab);
		mPlayer = go.GetComponent<UIPlayerName>();
		mPlayer.playerName = PlayerProfile.playerName;
		mPlayer.UpdateInfo(mShown);

		// Make sure that the tweened object has a collider
		NGUITools.AddWidgetCollider(parent);
		UIEventListener.Get(parent).onClick = ToggleList;

		// Refresh everything immediately so that there is no visible delay
		parent.BroadcastMessage("CreatePanel", SendMessageOptions.DontRequireReceiver);
		UIPanel pnl = NGUITools.FindInParents<UIPanel>(gameObject);
		if (pnl != null) pnl.Refresh();
	}

	/// <summary>
	/// Show / hide the list of players.
	/// </summary>

	void ToggleList (GameObject go)
	{
		mShown = !mShown;
		tween.Toggle();
		mPlayer.UpdateInfo(mShown);
	}
}
