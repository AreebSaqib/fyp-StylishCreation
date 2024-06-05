using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum PanelType
{
    removeAds, unlockPlayers, unlockLevels, unlockEverything, firstCoinPack, secondCoinPack, thirdCoinPack
}
[System.Serializable]
public class IAP_Panels
{

    public PanelType panelType;
    public GameObject _Panel;
}

public class IAPPanelsManager : MonoBehaviour
{
    public bool disableAfterPurchase;
    public AudioSource purchaseSFX;

    public enum ActivationType
    {
        simple, random
    }
    public ActivationType activationType;
    [ListDrawerSettings(ShowIndexLabels = true)]
    public IAP_Panels[] IAP_Panels;
    private int unlockIndex = -1, activationProbability, totalPanels, unlockCount;
    private bool isRemoveAds, isPlayerUnlocked, isLevelUnlocked;
    private bool removeAdAvailable, unlockPlayersAvailable, unlockLevelsAvailable;
    [HideInInspector]
    public bool isFound = false;
    private List<int> excludedNumbers = new List<int>();
    private void OnEnable()
    {
        unlockIndex = -1;
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
            SamanGame_SaveLoad.LoadProgress();
        }
        if (excludedNumbers.Count > 0)
        {
            excludedNumbers.Clear();
        }
        for(int i=0;i< IAP_Panels.Length; i++)
        {
            if (IAP_Panels[i]._Panel)
            {
                totalPanels++;
                excludedNumbers.Add(i);
                if(IAP_Panels[i].panelType == PanelType.removeAds)
                {
                    removeAdAvailable = true;
                }
                else if (IAP_Panels[i].panelType == PanelType.unlockPlayers)
                {
                    unlockPlayersAvailable = true;
                }
                if (IAP_Panels[i].panelType == PanelType.unlockLevels)
                {
                    unlockLevelsAvailable = true;
                }
                IAP_Panels[i]._Panel.SetActive(false);
            }
        }
        if (activationType == ActivationType.simple)
        {
            if (GameManager.Instance.selectedLevel == 0 || GameManager.Instance.selectedLevel % 3 == 0)
            {
                CheckUnlockedStatus();
            }
        }
        else if (activationType == ActivationType.random)
        {
            activationProbability = Random.Range(0, 6);
            if (activationProbability == 1)
            {
                CheckUnlockedStatus();
            }
        }
        //if (SaveData.Instance.Players.Count > 0)
        //{
     
        //}
    }
    public void RemoveAds()
    {
        Helper.RemoveAds();
        if (purchaseSFX)
        {
            purchaseSFX.Play();
        }
        if (disableAfterPurchase)
        {

            StartCoroutine(DeactivatingPanels());
        }

    }
    public void UnlockAllPlayers()
    {
        Helper.UnlockAllPlayers();
        if (FindObjectOfType<GSF_PlayerSelection>())
        {
            FindObjectOfType<GSF_PlayerSelection>().UpdateStatus();
        }
        if (purchaseSFX)
        {
            purchaseSFX.Play();
        }
        if (disableAfterPurchase)
        {
            StartCoroutine(DeactivatingPanels());
        }


    }
    public void UnlockAllLevels()
    {
        Helper.UnlockAllLevels(20);
        if (purchaseSFX)
        {
            purchaseSFX.Play();
        }
        if (disableAfterPurchase)
        {
            StartCoroutine(DeactivatingPanels());
        }

    }

    public void UnlockEverything()
    {


        Helper.UnlockEverything();
        if (purchaseSFX)
        {
            purchaseSFX.Play();
        }

        if (disableAfterPurchase)
        {
            StartCoroutine(DeactivatingPanels());
        }

    }
    public void PurchaseFirstCoinsPack()
    {
        SaveData.Instance.Coins += 2000;
        SamanGame_SaveLoad.SaveProgress();
        UpdateCoins();
        if (disableAfterPurchase)
        {
            StartCoroutine(DeactivatingPanels());
        }
    }
    public void PurchaseSecondCoinsPack()
    {
        SaveData.Instance.Coins += 4000;
        SamanGame_SaveLoad.SaveProgress();
        UpdateCoins();
        if (disableAfterPurchase)
        {
            StartCoroutine(DeactivatingPanels());
        }
    }
    public void PurchaseThirdCoinsPack()
    {
        SaveData.Instance.Coins += 6000;
        SamanGame_SaveLoad.SaveProgress();
        UpdateCoins();
        if (disableAfterPurchase)
        {
            StartCoroutine(DeactivatingPanels());
        }
    }
    public void PurchaseFourthCoinsPack()
    {
        SaveData.Instance.Coins += 10000;
        SamanGame_SaveLoad.SaveProgress();
        UpdateCoins();

        if (disableAfterPurchase)
        {
            StartCoroutine(DeactivatingPanels());
        }
    }
    private void UpdateCoins()
    {
        if (purchaseSFX)
        {
            purchaseSFX.Play();
        }
    }

    IEnumerator DeactivatingPanels()
    {
        yield return new WaitForSecondsRealtime(1f);
        for(int i=0;i< IAP_Panels.Length; i++)
        {
            if (IAP_Panels[i]._Panel)
            {
                IAP_Panels[i]._Panel.SetActive(false);
            }
        }
    }

    private void CheckUnlockedStatus()
    {
        if (IAP_Panels.Length > 0)
        {
            if (SaveData.Instance.RemoveAds)
            {
                isRemoveAds = true;
            }
            for (int i = 0; i < SaveData.Instance.Players.Count; i++)
            {
                if (!SaveData.Instance.Players[i].isLocked)
                {
                    unlockCount++;
                }
            }
            if (unlockCount == SaveData.Instance.Players.Count)
            {
                isPlayerUnlocked = true;
            }
            //Remember put correct numbers of levels here
            if (SaveData.Instance.LevelsUnlocked == 20)
            {
                isLevelUnlocked = true;
            }
            while (excludedNumbers.Count > 0 && !isFound)
            {
                unlockIndex = GetRandomNumber();
                if (IAP_Panels[unlockIndex].panelType == PanelType.firstCoinPack)
                {
                    isFound = true;
                }
                else if (IAP_Panels[unlockIndex].panelType == PanelType.secondCoinPack)
                {
                    isFound = true;
                }
                else if (IAP_Panels[unlockIndex].panelType == PanelType.thirdCoinPack)
                {
                    isFound = true;
                }
                else if (!isRemoveAds && IAP_Panels[unlockIndex].panelType == PanelType.removeAds)
                {
                    isFound = true;
                }
                else if (isRemoveAds && IAP_Panels[unlockIndex].panelType == PanelType.removeAds)
                {
                    ExcludeIndex(unlockIndex);
                }
                else if (!isPlayerUnlocked && IAP_Panels[unlockIndex].panelType == PanelType.unlockPlayers)
                {
                    isFound = true;
                }
                else if (isPlayerUnlocked && IAP_Panels[unlockIndex].panelType == PanelType.unlockPlayers)
                {
                    ExcludeIndex(unlockIndex);
                }
                else if (!isLevelUnlocked && IAP_Panels[unlockIndex].panelType == PanelType.unlockLevels)
                {
                    isFound = true;
                }
                else if (isLevelUnlocked && IAP_Panels[unlockIndex].panelType == PanelType.unlockLevels)
                {
                    ExcludeIndex(unlockIndex);
                }
                else if ((!isRemoveAds && removeAdAvailable && IAP_Panels[unlockIndex].panelType == PanelType.unlockEverything) || (!isPlayerUnlocked && unlockPlayersAvailable && IAP_Panels[unlockIndex].panelType == PanelType.unlockEverything) || (!isLevelUnlocked && unlockLevelsAvailable && IAP_Panels[unlockIndex].panelType == PanelType.unlockEverything))
                {
                    isFound = true;
                }
                else
                {
                    ExcludeIndex(unlockIndex);
                }
            }
            if (isFound)
            {
                IAP_Panels[unlockIndex]._Panel.SetActive(true);
            }
        }
    }
    private void ExcludeIndex(int _Index)
    {
        for(int i = 0; i < excludedNumbers.Count; i++)
        {
            if(excludedNumbers[i] == _Index)
            {
                excludedNumbers.RemoveAt(i);
            }
        }
    }
    private int GetRandomNumber()
    {
        return Random.Range(0, IAP_Panels.Length);
    }
}
