using UnityEngine;
using System.Collections;

public class GameManager
{

	private static GameManager instance;

	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameManager ();
			}
			return instance;
		}
	}

	public bool Initialized = false;
	public bool bannerCalled = false;
	public string GameStatus;
	public int objectivesAchieved;
	public int SessionStatus = 0;
	public bool SessionAd = false;
    public int selectedPlayer;
    public int selectedLevel;
    public int totalKills;
    public int selectedTexture;
    public bool stopPlaying;
    public bool showStats;

}