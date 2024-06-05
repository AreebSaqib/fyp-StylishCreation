using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VirtualItem {
    public string Name;
    public string ItemKey;
    public int Price;
    public int AmountToGive;
    public Sprite Icon;
    public Button ItemButton;
}
public enum _PurchaseType
{
    NoAds,
    Coins,
    AllLevels,
    AllPlayers,
    Everything
}
[System.Serializable]
public class CurrencyItem {
    public string Name;
    public string Price;
    public Sprite Icon;
    public Button ItemButton;
    [Header("IAP")]
    public string IAPKey;
    public _PurchaseType _type;
    public int Amount;
}

public class GSF_StoreManager : MonoBehaviour {

    public Text Coins;

    [Header ("Virtual Items")]
    public VirtualItem[] VirtualItems;

    [Header ("Currency Items")]
    public CurrencyItem[] CurrencyItems;

    bool _isConnected;
    private int btnIndex;
    void Start ()
    {
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
            SamanGame_SaveLoad.LoadProgress();
        }
        if (SaveData.Instance.Coins <= 0)
        {
            Coins.text = "000";
        }
        else
        {
            Coins.text = SaveData.Instance.Coins.ToString();
        }
        InitializeStore ();
    }

    private void OnEnable()
    {
        LogEvents.Instance.Event("store_opened", "store_opened", "started");
    }

    private void OnDisable()
    {
        LogEvents.Instance.Event("store_closed", "store_closed", "completed");
    }

    void InitializeStore () {
        for (int i = 0; i < VirtualItems.Length; i++) {
            VirtualItems[i].ItemButton.transform.Find ("Name").GetComponent<Text> ().text = VirtualItems[i].Name;
            VirtualItems[i].ItemButton.transform.Find ("Price").GetComponent<Text> ().text = VirtualItems[i].Price.ToString ();
            VirtualItems[i].ItemButton.transform.Find ("Icon").GetComponent<Image> ().sprite = VirtualItems[i].Icon;
            int item_id = i;
            VirtualItems[i].ItemButton.onClick.AddListener(() => PurchaseVirtualItem(item_id));
        }

        for (int i = 0; i < CurrencyItems.Length; i++) {
            CurrencyItems[i].ItemButton.transform.Find ("Name").GetComponent<Text> ().text = CurrencyItems[i].Name;
            CurrencyItems[i].ItemButton.transform.Find ("Price").GetComponent<Text> ().text = CurrencyItems[i].Price;
            CurrencyItems[i].ItemButton.transform.Find ("Icon").GetComponent<Image> ().sprite = CurrencyItems[i].Icon;
            int item_id = i;
            CurrencyItems[i].ItemButton.onClick.AddListener(() =>
            {
                btnIndex = item_id;
            });
        }
    }

    public void PurchaseVirtualItem (int id) {
  
    }

    public void PurchaseInAppItem () {
        switch (CurrencyItems[btnIndex]._type)
        {
            case _PurchaseType.NoAds:
                LogEvents.Instance.Event("remove_ads_from_store_success", "remove_ads_from_store_success", "started");
                Helper.RemoveAds();
                break;
            case _PurchaseType.AllLevels:
                LogEvents.Instance.Event("unlock_all_levels_from_store_success", "unlock_all_levels_from_store_success", "started");
                Helper.UnlockAllLevels(CurrencyItems[btnIndex].Amount);
                break;
            case _PurchaseType.AllPlayers:
                LogEvents.Instance.Event("unlock_all_players_from_store_success", "unlock_all_players_from_store_success", "started");
                Helper.UnlockAllPlayers();
                break;
            case _PurchaseType.Everything:
                LogEvents.Instance.Event("unlock_everything_from_store_success", "unlock_everything_from_store_success", "started");
                Helper.UnlockEverything();
                break;
            case _PurchaseType.Coins:
                LogEvents.Instance.Event("virtual_currency_purchase_success_" + CurrencyItems[btnIndex].IAPKey, "virtual_currency_purchase_success_" + CurrencyItems[btnIndex].IAPKey, "started");
                SaveData.Instance.Coins = CurrencyItems[btnIndex].Amount;
                SamanGame_SaveLoad.SaveProgress();
                break;
        }
    }

    public void CloseStore ()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        gameObject.SetActive (false);
    }
}