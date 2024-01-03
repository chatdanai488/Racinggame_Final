using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Used to populate the rankings window and makes it possible to sort all players in order, showing where the current player stands.
/// Note that this class is only here as a loose example of how a ranking window can be set up in a game.
/// For the purpose of the example, it uses live data from the game (Starlink).
/// </summary>

[RequireComponent(typeof(UIGrid))]
public class UIRankings : MonoBehaviour
{
	// Prefab for what a single row looks like.
	public GameObject prefab;

	// Label used to show information such as "Loading" or "No data found"
	public UILabel info;

	// Columns used for sorting
	public UIButton columnA;
	public UIButton columnB;
	public UIButton columnC;

	// Column backgrounds -- used to highlight on hover
	public UISprite backgroundA;
	public UISprite backgroundB;
	public UISprite backgroundC;

	// Color of the normal row's background
	public Color normalColor = Color.white;

	// Color of the highlighted row's background (used to highlight the player)
	public Color highlightColor = Color.white;

	// Columns are created by adding a thin border sliced sprite that should only be shown while there is data to show.
	public UIWidget[] ornaments;

	// All entries loaded from the server get stored in this format for easy sorting
	class Entry
	{
		public string name;
		public int exp;
		public int valueA;
		public int valueB;
		public int valueC;
	}

	// Raw data that came from the server
	string mData = null;

	// Grid used for easy arranging of columns
	UIGrid mGrid;

	// Full list of players coming from the server
	BetterList<Entry> mFullList = new BetterList<Entry>();

	// Cleaned up list showing sorted entries near the player
	BetterList<Entry> mCleanList = new BetterList<Entry>();

	// Column by which the current sorting is done
	GameObject mSortColumn = null;

	// Instantiated rows
	UIScoreRow[] mChildren = null;

	// Whether we're in the process of downloading data from the server
	bool mDownloading = false;

	/// <summary>
	/// Register the button event handlers.
	/// </summary>

	void Awake ()
	{
		mGrid = GetComponent<UIGrid>();
		
		UIEventListener.Get(columnA.gameObject).onClick = SortNow;
		UIEventListener.Get(columnB.gameObject).onClick = SortNow;
		UIEventListener.Get(columnC.gameObject).onClick = SortNow;
		
		columnA.isEnabled = false;
		columnB.isEnabled = false;
		columnC.isEnabled = false;
	}

	/// <summary>
	/// Start the download process.
	/// </summary>

	void OnEnable ()
	{
		bool hasData = (mFullList.size > 0);

		columnA.isEnabled = hasData;
		columnB.isEnabled = hasData;
		columnC.isEnabled = hasData;

		backgroundA.enabled	= hasData && (mSortColumn == columnA.gameObject);
		backgroundB.enabled	= hasData && (mSortColumn == columnB.gameObject);
		backgroundC.enabled	= hasData && (mSortColumn == columnC.gameObject);

		if (string.IsNullOrEmpty(mData))
		{
			if (PlayerProfile.allowedToAccessInternet)
			{
				if (!mDownloading)
				{
					mDownloading = true;
					info.text = Localization.Get("Loading");
					info.enabled = true;
					for (int i = 0; i < ornaments.Length; ++i)
						ornaments[i].enabled = false;
					StartCoroutine(DownloadData());
				}
			}
			else
			{
				info.text = Localization.Get("Wifi Required");
				info.enabled = true;
				for (int i = 0; i < ornaments.Length; ++i)
					ornaments[i].enabled = false;
			}
		}
		else Show();
	}

	/// <summary>
	/// Download the data from the internet.
	/// </summary>

	IEnumerator DownloadData ()
	{
		WWW w = null;

		try
		{
			// Using live data here... real ranked data from the game (Starlink)
			w = new WWW("http://starlink.tasharen.com/ranked.txt");
		}
		catch (System.Exception ex)
		{
			info.text = ex.Message;
		}

		if (w != null)
		{
			yield return w;

			if (string.IsNullOrEmpty(w.error))
			{
				ParseData(w.text);
				w.Dispose();
				w = null;
				SortNow(columnA.gameObject);
			}
			else
			{
				info.text = w.error;
				info.enabled = true;
				for (int i = 0; i < ornaments.Length; ++i)
					ornaments[i].enabled = false;
				w.Dispose();
				w = null;
			}
		}
		mDownloading = false;
	}

	/// <summary>
	/// Parse the downloaded data and turn it into usable information.
	/// </summary>

	void ParseData (string data)
	{
		mData = data;
		mFullList.Clear();

		string[] lines = mData.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0, imax = lines.Length; i < imax; ++i)
		{
			string line = lines[i];

			string[] parts = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length == 6)
			{
				Entry ent = new Entry();
				ent.name = parts[1];

				int.TryParse(parts[2], out ent.exp);
				int.TryParse(parts[3], out ent.valueA);
				int.TryParse(parts[4], out ent.valueB);
				int.TryParse(parts[5], out ent.valueC);

				mFullList.Add(ent);
			}
		}
	}

	/// <summary>
	/// Sort the list in order.
	/// </summary>

	void SortNow (GameObject go)
	{
		if (mFullList.size == 0)
		{
			info.text = Localization.Get("No Data");
			info.enabled = true;
			for (int i = 0; i < ornaments.Length; ++i)
				ornaments[i].enabled = false;
			return;
		}

		if (mSortColumn != go)
		{
			mSortColumn = go;
			mCleanList.Clear();

			if (go == columnC.gameObject)
			{
				for (int i = 0; i < mFullList.size; ++i)
				{
					Entry ent = mFullList[i];
					if (ent.valueC != 0) mCleanList.Add(ent);
				}
			}
			else if (go == columnB.gameObject)
			{
				for (int i = 0; i < mFullList.size; ++i)
				{
					Entry ent = mFullList[i];
					if (ent.valueB != 0) mCleanList.Add(ent);
				}
			}
			else
			{
				for (int i = 0; i < mFullList.size; ++i)
				{
					Entry ent = mFullList[i];
					if (ent.valueA != 0) mCleanList.Add(ent);
				}
			}

			// Columns are actually checkboxes -- only one can be active at a time
			UIToggle chk = go.GetComponent<UIToggle>();
			if (chk != null) chk.value = true;

			columnA.isEnabled = true;
			columnB.isEnabled = true;
			columnC.isEnabled = true;

			if (go == columnA.gameObject)
			{
				mCleanList.Sort(delegate(Entry a, Entry b) { return b.valueA.CompareTo(a.valueA); });
			}
			else if (go == columnB.gameObject)
			{
				mCleanList.Sort(delegate(Entry a, Entry b) { return b.valueB.CompareTo(a.valueB); });
			}
			else if (go == columnC.gameObject)
			{
				mCleanList.Sort(delegate(Entry a, Entry b) { return b.valueC.CompareTo(a.valueC); });
			}

			Show();
		}
	}

	/// <summary>
	/// Show the list.
	/// </summary>

	void Show ()
	{
		backgroundA.enabled	= (mSortColumn == columnA.gameObject);
		backgroundB.enabled	= (mSortColumn == columnB.gameObject);
		backgroundC.enabled	= (mSortColumn == columnC.gameObject);

		if (mChildren == null)
		{
			mChildren = new UIScoreRow[7];

			for (int i = 0; i < 7; ++i)
			{
				GameObject child = NGUITools.AddChild(gameObject, prefab);
				mChildren[i] = child.GetComponent<UIScoreRow>();
				mChildren[i].name = (i + 1).ToString();
			}
			mGrid.Reposition();
		}

		// Default to the end of the list so that if the player is not ranked, they will show up at the end
		int index = mCleanList.size == 0 ? 0 : (mCleanList.size - 1);

		for (int i = 0; i < mCleanList.size; ++i)
		{
			Entry ent = mCleanList[i];

			// Starlink actually matches profile IDs instead of names...
			// However since this project doesn't have IDs, we simply use player names.
			if (ent.name == PlayerProfile.playerName)
			{
				index = i;
				break;
			}
		}

		int min = index - 3;
		int max = index + 4;

		while (max > mCleanList.size)
		{
			--max;
			--min;
		}

		while (min < 0)
		{
			++min;
			++max;
		}

		for (int i = min; i < max; ++i)
		{
			UIScoreRow row = mChildren[i - min];

			bool isValid = i < mCleanList.size;

			if (isValid)
			{
				Entry ent = mCleanList[i];
				row.nameLabel.text = ent.name;
				row.rankLabel.text = (i + 1).ToString();
				row.titleLabel.text = PlayerProfile.GetTitleByExp(ent.exp);
				row.labelA.text = ent.valueA.ToString();
				row.labelB.text = ent.valueB.ToString();
				row.labelC.text = ent.valueC.ToString();
				row.background.color = (ent.name == PlayerProfile.playerName) ? highlightColor : normalColor;
			}
			else row.background.color = normalColor;

			row.nameLabel.enabled = isValid;
			row.rankLabel.enabled = isValid;
			row.titleLabel.enabled = isValid;
			row.labelA.enabled = isValid;
			row.labelB.enabled = isValid;
			row.labelC.enabled = isValid;
		}
		info.enabled = false;
		for (int i = 0; i < ornaments.Length; ++i)
			ornaments[i].enabled = true;
	}
}
