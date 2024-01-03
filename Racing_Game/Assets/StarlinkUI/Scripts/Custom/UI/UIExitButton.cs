using UnityEngine;

/// <summary>
/// Basic exit button functionality. In the game it will return to the menu, and in the menu it will exit to desktop.
/// </summary>

[RequireComponent(typeof(UIButton))]
public class UIExitButton : MonoBehaviour
{
#if UNITY_WEBPLAYER
	void Start ()
	{
		UIButton btn = GetComponent<UIButton>();
		if (btn != null && Application.loadedLevelName == "Menu Scene") btn.isEnabled = false;
	}
#endif

	void OnClick ()
	{
#if UNITY_4_7
		if (Application.loadedLevelName == "Menu Scene")
#else
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Menu Scene")
#endif
		{
			Application.Quit();
		}
		else
		{
			GameManager.Forfeit();
		}
	}
}
