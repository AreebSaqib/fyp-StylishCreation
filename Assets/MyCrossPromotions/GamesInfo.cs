using System.Collections.Generic;
using UnityEngine;

public class GamesInfo
{
    public static GamesInfo instance;
    public static GamesInfo Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GamesInfo();
            }
            return instance;
        }
    }
    public List<GamesData> gamesData = new List<GamesData>();
    public AdsData adsData = new AdsData();
    public GamesInfo() { }
    public GamesInfo(List<GamesData> _gamesData, AdsData _adsData)
    {
        gamesData = _gamesData;
        adsData = _adsData;
    }
}

[System.Serializable]
public class GamesData
{
    public string appVersion;
    public string appName;
    public string appLink;
    public Sprite appIcon;
}
[System.Serializable]
public class AdsData
{
    public bool canShowUnityBanner = false;
    public string bannerAdSequence = "Admob";
    public string interstitialAdSequence = "Admob";
    public string rewardedAdSequence = "Admob";
}
