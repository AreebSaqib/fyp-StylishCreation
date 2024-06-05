using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Scenes
{
    MySplashScreen,
    MyMainMenu,
    ModeSelection,
    MyPlayerSelection,
    MyLevelSelection,
    MyGamePlay,
    FashionStyle,
    IndianStyle,
    RedCarpetStyle
}

[System.Serializable]
public class CommonItemUI
{
    [Header("Selection Scrollers")]
    public GameObject crownScroller;
    public GameObject shadeScroller;
    public GameObject glassesScroller;
    public GameObject purseScroller;
    public GameObject shoesScroller;
    public GameObject petScroller;
    [Header("Player Selection Eelements")]
    public Image girlCrown;
    public Image eyeShade;
    public Image eyeGlasses;
    public Image girlPurse;
    public Image girlShoes;
    public Image cutePet;
    [Header("Player Selection Eelements")]
    public Image opponentGirlCrown;
    public Image opponentEyeShade;
    public Image opponentEyeGlasses;
    public Image opponentGirlPurse;
    public Image opponentGirlShoes;
    public Image opponentCutePet;
}

public enum RewardType
{
    none, SelectionItem, Coins, DoubleReward
}

public class Tags
{
    public const string playerTag = "Player";

}

public class Strings
{

}





