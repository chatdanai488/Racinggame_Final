using UnityEngine;

[RequireComponent(typeof(UIButton))]
public class UIProfileButton : MonoBehaviour
{
	public UIToggle checkbox;

	UIButton mBtn = null;

	void Awake ()
	{
		mBtn = GetComponent<UIButton>();
		mBtn.isEnabled = (checkbox != null && checkbox.value);
	}

	void OnEnable ()
	{ 
		if (checkbox != null)
			EventDelegate.Add(checkbox.onChange, OnCheckboxState);
	}

	void OnDisable ()
	{
		if (checkbox != null)
			EventDelegate.Remove(checkbox.onChange, OnCheckboxState);
	}

	void OnCheckboxState ()
	{
		mBtn.isEnabled = UIToggle.current.value;
	}
}
