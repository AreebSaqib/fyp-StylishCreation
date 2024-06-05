using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using JetBrains.Annotations;

[System.Serializable]
public class PlayerProps
{
    public string playerName;
    public int playerHealth;
    public int playerDamage;
    public int playerRange;
    public bool isLocked = true;
}

[System.Serializable]
public class Mode2Items
{
    public List<bool> HairLock = new List<bool>();
    public List<bool> DressLock = new List<bool>();
    public List<bool> EyesLock = new List<bool>();
    public List<bool> LipsLock = new List<bool>();
    public List<bool> NecklaceLock = new List<bool>();
    public List<bool> BeachthingLock = new List<bool>();
    public List<bool> BagLock = new List<bool>();
    public List<bool> GlassesLock = new List<bool>();
    public List<bool> BlushLock = new List<bool>();
    public List<bool> CapLock = new List<bool>();

}
[System.Serializable]
public class Mode1Items
{
    public List<bool> HairLock = new List<bool>();
    public List<bool> DressLock = new List<bool>();
    public List<bool> NecklaceLock = new List<bool>();
    public List<bool> ShoesLock = new List<bool>();
    public List<bool> EarringsLock = new List<bool>();
    public List<bool> InstrumentLock = new List<bool>();



}
[System.Serializable]
public class Mode3Items
{
    public List<bool> HairLock = new List<bool>();
    public List<bool> DressLock = new List<bool>();
    public List<bool> CrownLock = new List<bool>();
    public List<bool> NecklaceLock = new List<bool>();
    public List<bool> BagLock = new List<bool>();
    public List<bool> EarringsLock = new List<bool>();


}
[System.Serializable]
public class Mode4Items
{
    public List<bool> HairLock = new List<bool>();
    public List<bool> DressLock = new List<bool>();
    public List<bool> ShoesLock = new List<bool>();

    public List<bool> NecklaceLock = new List<bool>();

    public List<bool> BagLock = new List<bool>();
    public List<bool> EarringsLock = new List<bool>();



}
[System.Serializable]
public class Mode5Items
{
    public List<bool> HairLock = new List<bool>();
    public List<bool> DressLock = new List<bool>();


    public List<bool> NecklaceLock = new List<bool>();
    public List<bool> BindiLock = new List<bool>();
    public List<bool> BagLock = new List<bool>();
    public List<bool> EarringsLock = new List<bool>();



}

[System.Serializable]
public class SaveData
{

    public static SaveData instance;
    public static SaveData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveData();
            }
            return instance;
        }
    }
    public bool RemoveAds = false;
    public bool PurchashedAllItems = false;
    public int LevelsUnlocked = 1;
    public int EventsUnlocked = 0;
    public int SelectedAvatar = 0;
    public string ProfileName;
    public Image PlayerImage;
    public Image OpponentImage;
    public bool ProfileCreated = false;
    public bool isSound = true, isMusic = true, isVibration = true, isRightControls = true;
    public int Coins = 100000;
    public List<PlayerProps> Players = new List<PlayerProps>();
    public Mode2Items Mode2Items = new Mode2Items();
    public Mode1Items Mode1Items = new Mode1Items();
    public Mode3Items Mode3Items = new Mode3Items();
    public Mode4Items Mode4Items = new Mode4Items();
    public Mode5Items Mode5Items = new Mode5Items();
    public string hashOfSaveData;
    //Constructor to save actual GameData
    public SaveData() { }

    //Constructor to check any tampering with the SaveData
    public SaveData(bool ads, bool purchasedAllItems, int levelsUnlocked, int coins,
        List<PlayerProps> _players, Mode2Items _mode2Items, Mode1Items _mode1Items, Mode3Items _mode3Items, Mode4Items _mode4Items, Mode5Items _mode5Items)
    {
        RemoveAds = ads;
        PurchashedAllItems = purchasedAllItems;
        LevelsUnlocked = levelsUnlocked;
        Coins = coins;
        Players = _players;
        Mode2Items = _mode2Items;
        Mode1Items = _mode1Items;
        Mode3Items = _mode3Items;
        Mode4Items = _mode4Items;
        Mode5Items = _mode5Items;

    }
}