using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
//using UnityEngine.Purchasing;


#region SoundProps
[System.Serializable]
public class ModeSelection_Sounds
{
    public AudioSource defaultBtnSFX;
    public AudioSource selectionBtnSFX;
    public AudioSource purchaseSFX;
}
#endregion

#region UIElements
[System.Serializable]
public class ModeSelectionUI
{
    public Text totalCoins;
    public Image fillBar;
    public GameObject modePanel;
    public GameObject noVideoPanel;
    public GameObject notEnoughPanel;
    public GameObject unlockEverythingPanel;
    public GameObject loadingPanel;
}
#endregion


public class ModeSelection : MonoBehaviour
{
    #region Variables
    [FoldoutGroup("ModeSelection Sounds")]
    [HideLabel]
    public ModeSelection_Sounds soundProps;
    [FoldoutGroup("ModeSelectionUI")]
    [HideLabel]
    public ModeSelectionUI uIElements;
    public string[] productIds;
    [Header("Scene Selection")]
    public float loadTime = 4f;
    private bool loadNow;
    private int selectedMode;
    private List<PlayerProps> tempProps = new List<PlayerProps>();
    private ItemList modeList;
    private RewardType rewardType = RewardType.none;
    private int itemToUnlock;
    #endregion

    void OnEnable()
    {
        //if (MyAdsManager.Instance != null)
        //{
        //    MyAdsManager.Instance.onRewardedVideoAdCompletedEvent += OnRewardedVideoComplete;
        //}
    }

    void OnDisable()
    {
        //if (MyAdsManager.Instance != null)
        //{
        //    MyAdsManager.Instance.onRewardedVideoAdCompletedEvent -= OnRewardedVideoComplete;
        //}
    }

    #region Start
    void Start()
    {
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
            SamanGame_SaveLoad.LoadProgress();
        }
        uIElements.totalCoins.text = SaveData.Instance.Coins.ToString();
        SetInitialValues();
        if (SaveData.Instance.PurchashedAllItems)
        {
            OverrideToLocal();
        }
        GetPlayerInfo();
        LogEvents.Instance.EventStarted("ModeSelection:Started", "ModeSelection:Started", "Started");
    }
    #endregion

    #region SetInitialValues
    private void SetInitialValues()
    {
        if (uIElements.modePanel)
        {
            if (uIElements.modePanel.GetComponent<ItemList>())
            {
                modeList = uIElements.modePanel.GetComponent<ItemList>();
            }
        }
        // Cash saved data frem "SaveData" if SaveData is not empty
        for (int i = 0; i < SaveData.Instance.Players.Count; i++)
        {
            tempProps.Add(SaveData.Instance.Players[i]);
        }
        if (modeList)
        {
            // Check if there is any new data available or data that needs to be updated
            if (SaveData.Instance.Players.Count < modeList.itemValues.Length)
            {
                for (int i = 0; i < modeList.itemValues.Length; i++)
                {
                    if (SaveData.Instance.Players.Count <= i)
                    {
                        // Add new data to SaveData file in case the file is empty or new data is available
                        PlayerProps _playerProps = new PlayerProps();
                        _playerProps.isLocked = modeList.itemValues[i].isLocked;
                        SaveData.Instance.Players.Add(_playerProps);
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
            for (int i = 0; i < modeList.itemValues.Length; i++)
            {
                modeList.itemValues[i].isLocked = SaveData.Instance.Players[i].isLocked;
            }
            //Adding Click listeners to btns 
            for (int i = 0; i < modeList.itemValues.Length; i++)
            {
                int Index = i;
                if (modeList.itemValues[i].itemInfo.itemBtn)
                {
                    modeList.itemValues[i].itemInfo.itemBtn.onClick.AddListener(() =>
                    {
                        selectedMode = Index;
                        SelectItem(Index);
                    });
                }
            }
        }
        SamanGame_SaveLoad.SaveProgress();
    }
    #endregion

    #region OverrideToLocal
    private void OverrideToLocal()
    {
        for (int i = 0; i < modeList.itemValues.Length; i++)
        {
            modeList.itemValues[i].isLocked = false;
        }
    }
    #endregion

    #region SelectItem
    private void SelectItem(int selectedIndex)
    {
        itemToUnlock = selectedIndex;
        if (loadNow)
            return;
        if (soundProps.selectionBtnSFX) soundProps.selectionBtnSFX.Play();
        rewardType = RewardType.SelectionItem;
        if (modeList)
        {
            if (modeList.itemValues.Length > selectedIndex)
            {
                if (modeList.itemValues[selectedIndex].isLocked)
                {
                    if (modeList.itemValues[selectedIndex].videoUnlock)
                    {
                        CheckVideoStatus();
                    }
                    else if (modeList.itemValues[selectedIndex].coinsUnlock)
                    {
                        if (SaveData.Instance.Coins >= modeList.itemValues[selectedIndex].requiredCoins)
                        {
                            modeList.itemValues[selectedIndex].isLocked = false;
                            SaveData.Instance.Players[selectedIndex].isLocked = false;
                            SaveData.Instance.Coins -= modeList.itemValues[selectedIndex].requiredCoins;
                            SamanGame_SaveLoad.SaveProgress();
                            GetPlayerInfo();
                            if (soundProps.purchaseSFX) soundProps.purchaseSFX.Play();
                        }
                        else
                        {
                            if (uIElements.notEnoughPanel) uIElements.notEnoughPanel.SetActive(true);

                        }
                    }
                }
                else
                {
                    GameManager.Instance.selectedPlayer = selectedIndex;
                    uIElements.fillBar.fillAmount = 0;
                    uIElements.loadingPanel.SetActive(true);
                    uIElements.modePanel.SetActive(false);
                    loadNow = true;
                    GSF_AdsManager.ShowInterstitial(1, "Mode Selection");
                }
            }
        }
        if (uIElements.totalCoins) uIElements.totalCoins.text = SaveData.Instance.Coins.ToString();
    }
    #endregion

    #region GetPlayerInfo
    private void GetPlayerInfo()
    {
        #region Get Characters Info
        if (modeList)
        {
            for (int i = 0; i < modeList.itemValues.Length; i++)
            {
                if (modeList.itemValues[i].isLocked)
                {
                    if (modeList.itemValues[i].videoUnlock)
                    {
                        if (modeList.itemValues[i].itemInfo.videoBtn)
                        {
                            modeList.itemValues[i].itemInfo.videoBtn.SetActive(true);
                        }
                        if (modeList.itemValues[i].itemInfo.coinSlot)
                        {
                            modeList.itemValues[i].itemInfo.coinSlot.SetActive(false);
                        }
                    }
                    else if (modeList.itemValues[i].coinsUnlock)
                    {
                        if (modeList.itemValues[i].itemInfo.videoBtn)
                        {
                            modeList.itemValues[i].itemInfo.videoBtn.SetActive(false);
                        }
                        if (modeList.itemValues[i].itemInfo.coinSlot)
                        {
                            modeList.itemValues[i].itemInfo.coinSlot.SetActive(true);
                            if (modeList.itemValues[i].itemInfo.unlockCoins)
                            {
                                modeList.itemValues[i].itemInfo.unlockCoins.text = modeList.itemValues[i].requiredCoins.ToString();
                            }
                        }
                    }
                }
                else
                {
                    if (modeList.itemValues[i].itemInfo.videoBtn)
                    {
                        modeList.itemValues[i].itemInfo.videoBtn.SetActive(false);
                    }
                    if (modeList.itemValues[i].itemInfo.coinSlot)
                    {
                        modeList.itemValues[i].itemInfo.coinSlot.SetActive(false);
                    }
                }
            }
        }
        #endregion
    }
    #endregion

    #region CheckVideoStatus
    private void CheckVideoStatus()
    {
        //if (MyAdsManager.Instance != null)
        //{
        //    if (MyAdsManager.Instance.IsAdmobRewardedAvailable())
        //    {
        //        MyAdsManager.Instance.ShowRewardedVideos();
        //    }
        //    else
        //    {
        //        uIElements.noVideoPanel.SetActive(true);
        //    }
        //}
        //else
        //{
        //    uIElements.noVideoPanel.SetActive(true);
        //}
    }
    #endregion

    #region RewardedVideoCompleted
    public void OnRewardedVideoComplete()
    {
        if (rewardType == RewardType.SelectionItem)
        {
            if (modeList.itemValues.Length > selectedMode)
            {
                if (modeList.itemValues[selectedMode].isLocked)
                {
                    modeList.itemValues[selectedMode].isLocked = false;
                }
            }
            SaveData.Instance.Players[selectedMode].isLocked = false;
        }
        else if (rewardType == RewardType.Coins)
        {
            SaveData.Instance.Coins += 3000;
            uIElements.totalCoins.text = SaveData.Instance.Coins.ToString();
        }
        SamanGame_SaveLoad.SaveProgress();
        rewardType = RewardType.none;
        if (soundProps.purchaseSFX) soundProps.purchaseSFX.Play();
        SamanGame_SaveLoad.SaveProgress();
        GetPlayerInfo();
    }
    #endregion

    #region GetRewardedCoins
    public void GetRewardedCoins()
    {
        rewardType = RewardType.Coins;
        CheckVideoStatus();
    }
    #endregion

    #region UnlockMode
    public void UnlockMode()
    {
        modeList.itemValues[itemToUnlock].isLocked = false;
        SaveData.Instance.Players[itemToUnlock].isLocked = false;
        GetPlayerInfo();
        SamanGame_SaveLoad.SaveProgress();
        if (soundProps.purchaseSFX) soundProps.purchaseSFX.Play();
        if(itemToUnlock == 1)
        {
            LogEvents.Instance.EventStarted("IndianMode:Purchased", "IndianMode:Purchased", "Purchased");
        }
        else if (itemToUnlock == 2)
        {
            LogEvents.Instance.EventStarted("RedCarpetMode:Purchased", "RedCarpetMode:Purchased", "Purchased");
        }
    }
    #endregion

    #region UnlockEverything
    public void UnlockEverything()
    {
        SaveData.Instance.PurchashedAllItems = true;
        Helper.UnlockAllPlayers();
        Helper.RemoveAds();
        OverrideToLocal();
        GetPlayerInfo();
        SamanGame_SaveLoad.SaveProgress();
        if (soundProps.purchaseSFX) soundProps.purchaseSFX.Play();
        LogEvents.Instance.EventStarted("UnlockEverything:Purchased", "UnlockEverything:Purchased", "Purchased");
    }
    #endregion

    #region Update
    private void Update()
    {
        if (loadNow)
        {
            if (uIElements.fillBar)
            {
                uIElements.fillBar.fillAmount += Time.deltaTime / loadTime;
                if (uIElements.fillBar.fillAmount >= 1)
                {
                    loadNow = false;
                    if (selectedMode == 0)
                    {
                        SceneManager.LoadSceneAsync(Scenes.FashionStyle.ToString());
                    }
                    else if (selectedMode == 1)
                    {
                        SceneManager.LoadSceneAsync(Scenes.IndianStyle.ToString());
                    }
                    else if (selectedMode == 2)
                    {
                        SceneManager.LoadScene(Scenes.RedCarpetStyle.ToString());
                    }
                }
            }
        }
    }
    #endregion
}
