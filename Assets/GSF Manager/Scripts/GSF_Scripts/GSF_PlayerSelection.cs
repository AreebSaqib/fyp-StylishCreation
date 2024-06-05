using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
//using UnityEngine.Purchasing;
using System;

#region SoundProps
[System.Serializable]
public class PlayerSelection_Sounds
{
    public AudioSource bgMusic;
    public AudioSource defaultBtnSFX;
    public AudioSource nextBtnSFX;
    public AudioSource backBtnSFX;
    public AudioSource selectionBtnSFX;
    public AudioSource purchasingSFX;
    public bool sameBGMInNextScene;
}
#endregion

#region UIElements
[System.Serializable]
public class PlayerSelectionUI
{
    public Button buyBtn, nextBtn;
    public Image profileImg, buyingCoinSlot, buyingPriceSlot, loadingFillBar;
    public Text profileName, requiredCoins, requiresPrice, loadProgressText, totalCoins, notEnoughCoins;
    public GameObject buyingPopup, rightBtn, leftBtn, unlockAllPlayersBtn, IAP_priceBtn, IAP_coinsBtn;
    public GameObject loadingPanel;
}
#endregion

#region PlayerStats
[System.Serializable]
public class PlayerStats
{
    public Text playerName;
    public Image healthFill;
    public Image damageFill;
    public Image rangeFill;
}
#endregion

#region PlayerProperties
[System.Serializable]
public class PlayerProperties
{
    public string playerName;
    [Range(0, 100)]
    public int playerHealth;
    [Range(0, 100)]
    public int playerDamage;
    [Range(0, 100)]
    public int playerRange;
    public GameObject playerObject;
    [OnValueChanged("IsLockedChanged")]
    public bool isLocked;

    public enum UnlockType
    {
        none,
        withPrice,
        withCoins,
        withBoth
    }
    [ShowIf("isLocked")]
    [OnValueChanged("UnlockTypeChanged")]
    public UnlockType unlockType;
    [ShowIf("isPriceBased")]
    [Range(0, 1000)]
    public float unlockPrice;
    [ShowIf("isCoinBased")]
    [Range(0, 100000)]
    public int unlockCoins;
    [ShowIf("isPriceBased")]
    public string IAP_Id;
    [HideInInspector]
    public bool isPriceBased, isCoinBased;
    public bool updateNow;
    private void UnlockTypeChanged()
    {
        if (unlockType == UnlockType.withCoins)
        {
            isPriceBased = false;
            isCoinBased = true;
        }
        else if (unlockType == UnlockType.withPrice)
        {
            isCoinBased = false;
            isPriceBased = true;
        }
        else if (unlockType == UnlockType.withBoth)
        {
            isCoinBased = true;
            isPriceBased = true;
        }
        else
        {
            isPriceBased = false;
            isCoinBased = false;
        }
    }
    private void IsLockedChanged()
    {
        if (!isLocked)
        {
            isPriceBased = false;
            isCoinBased = false;
        }
        else
        {
            unlockType = UnlockType.none;
        }
    }
}
#endregion


public class GSF_PlayerSelection : MonoBehaviour
{

    #region Variables
    [FoldoutGroup("PlayerSelection Sounds")]
    [HideLabel]
    public PlayerSelection_Sounds soundProps;
    [FoldoutGroup("PlayerSelection UI")]
    [HideLabel]
    public PlayerSelectionUI uIElements;
    [FoldoutGroup("Player Statistics")]
    [HideLabel]
    public PlayerStats playerStats;
    public Sprite[] playerAvatars;
    [ListDrawerSettings(ShowIndexLabels = true)]
    public PlayerProperties[] playerProps;
    [Header("Scene Selection")]
    public Scenes PreviousScene;
    public Scenes NextScene;
    [Header("Ad Sequence ID")]
    public int SequenceID = 1;
    public bool LoadingSequence;
    public int LoadingSequenceID = 4;
    private int playerCounter = 0;
    int unlockCount = 0;
    private string dollarSign = "$ ";
    private List<PlayerProps> tempProps = new List<PlayerProps>();
    private GameObject[] bgMusicReferences;
    #endregion

    #region Start
    void Start()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
            SamanGame_SaveLoad.LoadProgress();
        }
        uIElements.leftBtn.gameObject.SetActive(false);
        SetInitialValues();
        PlayerSelectionSettings();
        GetPlayerInfo();
        CheckUnlockStatus();
        uIElements.profileName.text = SaveData.Instance.ProfileName;
        uIElements.profileImg.sprite = playerAvatars[SaveData.Instance.SelectedAvatar];
        ShowAds(SequenceID, "Player Selection");
        LogEvents.Instance.Event("player_selection_started", "player_selection_started", "started");
    }
    #endregion

    #region SetInitialValues
    private void SetInitialValues()
    {
        // Cash saved data frem "SaveData" if SaveData is not empty
        for (int i = 0; i < SaveData.Instance.Players.Count; i++)
        {
            tempProps.Add(SaveData.Instance.Players[i]);
        }
        // Check if there is any new data available or data that needs to be updated
        if (SaveData.Instance.Players.Count < playerProps.Length)
        {
            for (int i = 0; i < playerProps.Length; i++)
            {
                if (playerProps[i].updateNow && SaveData.Instance.Players.Count > i)
                {
                    // Check if there is a data saved in SaveData file and it needs to be updated in case of new data to add
                    UpdateData(i);
                }
                else if (SaveData.Instance.Players.Count <= i)
                {
                    // Add new data to SaveData file in case the file is empty or new data is available
                    AddNewData(i);
                }
            }
        }
        else
        {
            // Check if there is a data saved in SaveData file and it needs to be updated in case of no new data to add
            for (int i = 0; i < playerProps.Length; i++)
            {
                if (playerProps[i].updateNow && SaveData.Instance.Players.Count > 0)
                {
                    UpdateData(i);
                }
            }
        }
        /* Restore SaveData isLocked state from cashed tempProps in case you accidentally changed that value in inspector if you 
        actually wanna change the lock satate you can comment this loop statement*/
        for (int i = 0; i < tempProps.Count; i++)
        {
            SaveData.Instance.Players[i].isLocked = tempProps[i].isLocked;
        }
        // Setting up Player Properties to actual Properties from SaveData file  
        for (int i = 0; i < playerProps.Length; i++)
        {

            playerProps[i].isLocked = SaveData.Instance.Players[i].isLocked;
        }
        SamanGame_SaveLoad.SaveProgress();
    }
    private void UpdateData(int arrayIndex)
    {

        //SaveData.Instance.Players[arrayIndex].isLocked = playerProps[arrayIndex].isLocked;
    }
    private void AddNewData(int arrayIndex)
    {
        PlayerProps _playerProps = new PlayerProps();
        _playerProps.isLocked = playerProps[arrayIndex].isLocked;
        SaveData.Instance.Players.Add(_playerProps);
    }
    #endregion

    #region LevelSelectionSettings
    private void PlayerSelectionSettings()
    {
        if (!SaveData.Instance.isMusic)
        {
            soundProps.bgMusic.enabled = false;
        }
        if (!SaveData.Instance.isSound)
        {
            soundProps.defaultBtnSFX.enabled = false;
            soundProps.nextBtnSFX.enabled = false;
            soundProps.backBtnSFX.enabled = false;
            soundProps.selectionBtnSFX.enabled = false;
            soundProps.purchasingSFX.enabled = false;
        }
    }
    #endregion

    #region GetPlayerInfo
    private void GetPlayerInfo()
    {
        if (SaveData.Instance.Coins <= 0)
        {
            uIElements.totalCoins.text = "000";
        }
        else
        {
            uIElements.totalCoins.text = SaveData.Instance.Coins.ToString();
        }
        for (int i = 0; i < playerProps.Length; i++)
        {
            if (i == playerCounter)
            {
                playerStats.healthFill.fillAmount = playerProps[i].playerHealth / 100f;
                playerStats.damageFill.fillAmount = playerProps[i].playerDamage / 100f;
                playerStats.rangeFill.fillAmount = playerProps[i].playerRange / 100f;
                if (playerProps[i].playerObject)
                {
                    playerProps[i].playerObject.SetActive(true);
                }

                if (playerProps[i].isLocked)
                {
                    if (playerProps[i].unlockType == PlayerProperties.UnlockType.withCoins)
                    {
                        uIElements.buyingCoinSlot.gameObject.SetActive(true);
                        uIElements.buyingPriceSlot.gameObject.SetActive(false);
                        // if you are using Ui Animation script then button will not disable because script enables it back
                        uIElements.IAP_priceBtn.SetActive(false);
                    }
                    else if (playerProps[i].unlockType == PlayerProperties.UnlockType.withPrice)
                    {
                        uIElements.buyingCoinSlot.gameObject.SetActive(false);
                        uIElements.buyingPriceSlot.gameObject.SetActive(true);
                        // if you are using Ui Animation script then button will not disable because script enables it back
                        uIElements.IAP_coinsBtn.SetActive(false);
                    }
                    else if (playerProps[i].unlockType == PlayerProperties.UnlockType.withBoth)
                    {
                        uIElements.IAP_priceBtn.SetActive(true);
                        uIElements.IAP_coinsBtn.SetActive(true);
                        uIElements.buyingCoinSlot.gameObject.SetActive(true);
                        uIElements.buyingPriceSlot.gameObject.SetActive(true);
                    }
                    uIElements.buyingCoinSlot.transform.GetChild(0).GetComponent<Text>().text = playerProps[i].unlockCoins.ToString();
                    uIElements.buyingPriceSlot.transform.GetChild(0).GetComponent<Text>().text = playerProps[i].unlockPrice.ToString();
                    uIElements.buyBtn.gameObject.SetActive(true);
                    uIElements.nextBtn.gameObject.SetActive(false);

                }
                else
                {
                    uIElements.buyingCoinSlot.gameObject.SetActive(false);
                    uIElements.buyingPriceSlot.gameObject.SetActive(false);
                    uIElements.buyBtn.gameObject.SetActive(false);
                    uIElements.nextBtn.gameObject.SetActive(true);
                }
            }
            else
            {
                if (playerProps[i].playerObject)
                {
                    playerProps[i].playerObject.SetActive(false);
                }
            }
        }
    }
    #endregion

    #region ScrollSelect
    public void ScrollSelect(int selectIndex)
    {
        GameManager.Instance.selectedPlayer = selectIndex;
        soundProps.selectionBtnSFX.Play();
        playerCounter = selectIndex;
        GetPlayerInfo();
    }
    #endregion

    #region RightSeletionBtn
    public void RightSeletionBtn()
    {
        soundProps.selectionBtnSFX.Play();
        if (playerCounter < playerProps.Length - 1)
        {
            uIElements.leftBtn.SetActive(true);
            playerCounter++;
            GetPlayerInfo();
        }
        if(playerCounter == playerProps.Length - 1)
        {
            uIElements.rightBtn.SetActive(false);
        }

        GameManager.Instance.selectedPlayer = playerCounter;
    }
    #endregion

    #region LeftSelectionBtn
    public void LeftSelectionBtn()
    {
        soundProps.selectionBtnSFX.Play();
        if (playerCounter >= 1)
        {
            uIElements.rightBtn.SetActive(true);
            playerCounter--;
            GetPlayerInfo();
        }
        if (playerCounter == 0)
        {
            uIElements.leftBtn.SetActive(false);
        }
        GameManager.Instance.selectedPlayer = playerCounter;
    }
    #endregion

    #region IAP_Purchaing
    public void BuyBtn()
    {
        soundProps.defaultBtnSFX.Play();
        uIElements.requiredCoins.text = playerProps[playerCounter].unlockCoins.ToString();
        uIElements.requiresPrice.text =  playerProps[playerCounter].unlockPrice.ToString();
        //uIElements.IAP_priceBtn.GetComponent<IAPButton>().productId = playerProps[playerCounter].IAP_Id;
        uIElements.buyingPopup.SetActive(true);
    }
    public void BuyWithCoins()
    {
        soundProps.defaultBtnSFX.Play();
        if (playerProps[playerCounter].unlockCoins <= SaveData.Instance.Coins)
        {
            soundProps.purchasingSFX.Play();
            uIElements.buyingPopup.SetActive(false);
            SaveData.Instance.Coins -= playerProps[playerCounter].unlockCoins;
            SaveData.instance.Players[playerCounter].isLocked = false;
            playerProps[playerCounter].isLocked = false;
            SamanGame_SaveLoad.SaveProgress();
            GetPlayerInfo();
            uIElements.totalCoins.text = SaveData.Instance.Coins.ToString();
            CheckUnlockStatus();
        }
        else
        {
            uIElements.notEnoughCoins.enabled = true;
        }
    }
    public void BuyWithPrice()
    {
        soundProps.purchasingSFX.Play();
        uIElements.notEnoughCoins.enabled = false;
        uIElements.buyBtn.gameObject.SetActive(false);
        SaveData.Instance.Players[playerCounter].isLocked = false;
        playerProps[playerCounter].isLocked = false;
        SamanGame_SaveLoad.SaveProgress();
        GetPlayerInfo();
        StartCoroutine(DisableObject(uIElements.buyingPopup));
        CheckUnlockStatus();
    }
    public void UnlockAllPlayers()
    {
        soundProps.purchasingSFX.Play();
        Helper.UnlockAllPlayers();
        for (int i = 0; i < playerProps.Length; i++)
        {
            playerProps[i].isLocked = false;
        }
        GetPlayerInfo();
        StartCoroutine(DisableObject(uIElements.unlockAllPlayersBtn));
    }
    #endregion

    #region PlayLevel
    public void PlayLevel()
    {
        GameManager.Instance.SessionStatus = 1;
        GameManager.Instance.selectedPlayer = playerCounter;
        GameManager.Instance.selectedTexture = playerCounter;
        uIElements.loadingPanel.SetActive(true);
        StartCoroutine(LoadScene());
        LogEvents.Instance.Event("player_selection_to_level_selection", "player_selection_to_level_selection", "completed");
    }
    IEnumerator LoadScene()
    {
        soundProps.nextBtnSFX.Play();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(NextScene.ToString());
        while (!asyncOperation.isDone)
        {
            uIElements.loadProgressText.text = Mathf.Floor(asyncOperation.progress * 100) + "%";
            uIElements.loadingFillBar.fillAmount = (asyncOperation.progress * 100);
            if (asyncOperation.progress >= 0.9f)
            {
                uIElements.loadProgressText.text = "100%";
                uIElements.loadingFillBar.fillAmount = 1f;
            }
            yield return null;
        }
    }
    #endregion

    #region BackBtn
    public void BackBtn()
    {
        soundProps.backBtnSFX.Play();
        SceneManager.LoadScene(PreviousScene.ToString());
        LogEvents.Instance.Event("player_selection_to_main_menu", "player_selection_to_main_menu", "completed");
    }
    #endregion

    #region CancelPopup
    public void CancelPopup()
    {
        soundProps.defaultBtnSFX.Play();
        uIElements.notEnoughCoins.enabled = false;
        uIElements.buyingPopup.SetActive(false);
    }
    #endregion

    #region ShowAds
    void ShowAds(int id, string SceneName)
    {
        GSF_AdsManager.ShowInterstitial(id, SceneName);
    }
    #endregion

    #region CheckUnlockStatus
    private void CheckUnlockStatus()
    {
        unlockCount = 0;
        for (int i = 0; i < playerProps.Length; i++)
        {
            if (!playerProps[i].isLocked)
            {
                unlockCount++;
            }
        }
        if (unlockCount == playerProps.Length)
        {
            StartCoroutine(DisableObject(uIElements.unlockAllPlayersBtn));
        }
    }
    IEnumerator DisableObject(GameObject refObject)
    {
        yield return new WaitForSeconds(1f);
        refObject.SetActive(false);
    }
    #endregion

    public void UpdateStatus()
    {

    }
}
