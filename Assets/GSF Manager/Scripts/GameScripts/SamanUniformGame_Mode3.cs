using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using DG.Tweening;

#region SamanUniformGame_Model_UI
[System.Serializable]
public class SamanUniformGame_Mode3_UI
{
    public GameObject PlayerCharacter;
    public GameObject OpponentCharacter;
    public Text totalCoins;
    public Text rewardCoins;
    [Header("Scrollers")]
    public GameObject DressScroller;
    public GameObject CrownScroller,NecklaceScroller, HairScroller, EarringScroller, BagScroller, AllScrollers;
    [Header("PLayer Items")]
    public Image PlayerDress;
    public Image PlayerCrown,PlayerNecklace, PlayerHair,PlayerEarrings, PlayerBag;
    public Image playerAvatar_1, playerAvatar_2;
    [Header("Opponent Items")]
    public Image OpponentDress;
    public Image OpponentCrown,OpponentNecklace, OpponentHair,  OpponentEarrings, OpponentBag;
    public Image opponentAvatar_1, opponentAvatar_2;
    [Header("Fill Bar")]
    public Image fillbar;
    [Header("Panels")]
    public GameObject uiPanel;
    public GameObject AD_Panel, SS_Panel, avatarPanel, StartPanel,VSPanel,previewPanel, judgesPanel, notReadyPanel, levelComplete, notEnoughPanel, notAvailablePanel, loadingPanel;
    [Header("Score Items")]
    public Text playerPoints;
    public Text playerScore;
    public Text opponentPoints;
    public Text opponentScore;
    public GameObject[] playerStars;
    public GameObject[] opponentStars;
    [Header("Other Elements")]
    public GameObject characterParent;
    public GameObject trophyPanel, winHeader, loseHeader;
    public GameObject previewBtn;
    public GameObject nextbtn;
    public GameObject videoSlot, coinSlot;
    public Text playerID, opponentID;
    public Image screenShotImg;
    public Image BGimage;
    public Text Timetxt;
    public Image WaitImage;
    public Image Connecting;
}
#endregion


public class SamanUniformGame_Mode3 : MonoBehaviour
{
    #region Variables
    [System.Serializable]
    public enum SelectedItem
    {
        None, Dress,Bag, Hair, Crown, Earrings, Necklace
    }
    [FoldoutGroup("Saman Uniform Game Mode3 UI")]
    [HideLabel]
    public SamanUniformGame_Mode3_UI uIElements;
    private SelectedItem selectedItem;
    public Sprite[] PlayerAvatars;
    public Sprite[] opponentAvatars;
    public Sprite[] HairSprites;
    public Sprite[] DressSprites;
    public Sprite[] EarringSprites;
    public Sprite[] CrownSprites;
    public Sprite[] NecklaceSprites;
    public Sprite[] BagSprites;
    public Sprite[] BackGround;
    public Sprite Connected;
    public GameObject finalParticles;
    public AudioSource categorySelectSfx;
    public AudioSource itemSelectSfx;
    public AudioSource purchaseSfx;
    public AudioSource opponentSfx;
    private ItemInfo tempItem;
    private List<ItemInfo> hairList = new List<ItemInfo>();
    private List<ItemInfo> dressList = new List<ItemInfo>();
    private List<ItemInfo> earringsList = new List<ItemInfo>();
    private List<ItemInfo> CrownList = new List<ItemInfo>();
    private List<ItemInfo> necklaceList = new List<ItemInfo>();
    private List<ItemInfo> BagList = new List<ItemInfo>();
    private List<Sprite> avatarList = new List<Sprite>();
    private int startIndex;
    private float findDelay;
    private float nextDealy;
    private bool canShowInterstitial;
    private int selectedIndex;
    private int readyCount;
    private int dressRank, hairsRank, BagRank;
    private int eyesRank, CrownRank;
    private int necklaceRank, earringsRank;
    private int OP_DressingPoints, OP_MakeupPoints, OP_JewellertPoints;
    private int playerScore, opponentScore;
    private int count = 0;
    private int playerStarCount = 0;
    private int opponentStarCount = 0;
    private bool IsTimeRunning = false;
    public float timeRemaning = 3f;
    private Texture2D _Taxture;

    private enum RewardType
    {
        None, SelectionItem, Coins, DoubleReward
    }
    private RewardType rewardType;
    #endregion

    void OnEnable()
    {
       
    }
    void OnDisable()
    {
       
    }

    #region Start
    private void Start()
    {
        int playerindx = PlayerPrefs.GetInt("Player");
        int opponentindx = PlayerPrefs.GetInt("Opponent");
        uIElements.playerAvatar_2.sprite = PlayerAvatars[playerindx];
        uIElements.opponentAvatar_2.sprite = opponentAvatars[opponentindx];
        Time.timeScale = 1;
        AudioListener.pause = false;
        AudioListener.volume = 1;
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
            SamanGame_SaveLoad.LoadProgress();
        }
        uIElements.totalCoins.text = SaveData.Instance.Coins.ToString();
        eyesRank = dressRank = hairsRank = CrownRank  = 1;
        //uIElements.opponentID.text = Random.Range(1200000, 5000000).ToString();
        selectedItem = SelectedItem.Dress;
        StartCoroutine(SamanUniformGame_InitializeValues());
        Input.multiTouchEnabled = false;
    }
    #endregion

    #region FindingOpponent
    IEnumerator SamanUniformGame_FindingOpponent()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < opponentAvatars.Length; i++)
        {
            if (opponentAvatars[i])
                avatarList.Add(opponentAvatars[i]);
        }
        startIndex = Random.Range(1, avatarList.Count);
        if (opponentSfx) opponentSfx.Play();
        while (findDelay < 4)
        {
            if (uIElements.opponentAvatar_1 && uIElements.opponentAvatar_2 && nextDealy < findDelay)
            {
                uIElements.opponentAvatar_1.sprite = avatarList[startIndex];
                uIElements.opponentAvatar_2.sprite = avatarList[startIndex];
                startIndex++;
                if (startIndex >= avatarList.Count)
                    startIndex = 1;
                nextDealy += 0.2f;
            }
            yield return null;
            findDelay += Time.deltaTime;
        }
        if (opponentSfx) opponentSfx.Stop();
        yield return null;
    }
    #endregion

    #region InitializeValues
    IEnumerator SamanUniformGame_InitializeValues()
    {
        #region Initializing Dress
        if (uIElements.DressScroller)
        {
            var dressInfo = uIElements.DressScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < dressInfo.Length; i++)
            {
                dressList.Add(dressInfo[i]);
            }
        }
        SamanUniformGame_SetupItemData(SaveData.Instance.Mode3Items.DressLock, dressList);
        SamanUniformGame_SetItemIcon(dressList, DressSprites);
        #endregion

        yield return new WaitForSeconds(0.2f);


        #region Initializing Hair
        if (uIElements.HairScroller)
        {
            var hairInfo = uIElements.HairScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < hairInfo.Length; i++)
            {
                hairList.Add(hairInfo[i]);
            }
        }
        SamanUniformGame_SetupItemData(SaveData.Instance.Mode3Items.HairLock, hairList);
        SamanUniformGame_SetItemIcon(hairList, HairSprites);
        #endregion

        yield return new WaitForSeconds(0.2f);


        #region Initializing Necklace
        if (uIElements.NecklaceScroller)
        {
            var neckLaceInfo = uIElements.NecklaceScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < neckLaceInfo.Length; i++)
            {
                necklaceList.Add(neckLaceInfo[i]);
            }
        }
        SamanUniformGame_SetupItemData(SaveData.Instance.Mode3Items.NecklaceLock, necklaceList);
        SamanUniformGame_SetItemIcon(necklaceList, NecklaceSprites);
        #endregion

        yield return new WaitForSeconds(0.2f);


        #region Initializing Earrings
        if (uIElements.EarringScroller)
        {
            var noseRingsInfo = uIElements.EarringScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < noseRingsInfo.Length; i++)
            {
                earringsList.Add(noseRingsInfo[i]);
            }
        }
        SamanUniformGame_SetupItemData(SaveData.Instance.Mode3Items.EarringsLock, earringsList);
        SamanUniformGame_SetItemIcon(earringsList,EarringSprites);
        #endregion

        

        yield return new WaitForSeconds(0.2f);


        #region Initializing Crown
        if (uIElements.CrownScroller)
        {
            var CrownInfo = uIElements.CrownScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < CrownInfo.Length; i++)
            {
                CrownList.Add(CrownInfo[i]);
            }
        }
        SamanUniformGame_SetupItemData(SaveData.Instance.Mode3Items.CrownLock, CrownList);
        SamanUniformGame_SetItemIcon(CrownList, CrownSprites);
        #endregion

        yield return new WaitForSeconds(0.2f);

        #region Initializing Bag
        if (uIElements.BagScroller)
        {
            var BagInfo = uIElements.BagScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < BagInfo.Length; i++)
            {
                BagList.Add(BagInfo[i]);
            }
        }
        SamanUniformGame_SetupItemData(SaveData.Instance.Mode3Items.BagLock, BagList);
        SamanUniformGame_SetItemIcon(BagList, BagSprites);
        #endregion




  

        SamanGame_SaveLoad.SaveProgress();

        if (SaveData.Instance.PurchashedAllItems)
        {
            SamanUniformGame_UnlockAllItems();
        }
        else
        {
            SamanUniformGame_GetItemsInfo();
        }
    }
    #endregion

    #region SetupItemData
    private void SamanUniformGame_SetupItemData(List<bool> unlockItems, List<ItemInfo> _ItemsInfo)
    {
        if (_ItemsInfo.Count > 0)
        {
            if (unlockItems.Count < _ItemsInfo.Count)
            {
                for (int i = 0; i < _ItemsInfo.Count; i++)
                {
                    if (unlockItems.Count <= i)
                    {
                        // Add new data to SaveData file in case the file is empty or new data is available
                        unlockItems.Add(_ItemsInfo[i].isLocked);
                    }
                }
            }
            // Setting up Hairs Properties to actual Properties from SaveData file  
            for (int i = 0; i < _ItemsInfo.Count; i++)
            {
                _ItemsInfo[i].isLocked = unlockItems[i];
            }
            //Adding Click listeners to btns 
            for (int i = 0; i < _ItemsInfo.Count; i++)
            {
                int Index = i;
                if (_ItemsInfo[i].itemBtn)
                {
                    _ItemsInfo[i].itemBtn.onClick.AddListener(() =>
                    {
                        selectedIndex = Index;
                        SamanUniformGame_SelectItem(Index);
                        if (itemSelectSfx) itemSelectSfx.Play();
                    });
                }
            }
        }
    }
    #endregion

    #region SetItemIcon
    private void SamanUniformGame_SetItemIcon(List<ItemInfo> refList, Sprite[] btnIcons)
    {
        if (refList != null)
        {
            for (int i = 0; i < refList.Count; i++)
            {
                if (btnIcons.Length > i)
                {
                    if (btnIcons[i] && refList[i].itemIcon)
                    {
                        refList[i].itemIcon.sprite = btnIcons[i];
                    }
                }
            }
        }
    }
    #endregion

    #region GetItemsInfo
    private void SamanUniformGame_GetItemsInfo()
    {
        if (selectedItem == SelectedItem.Hair)
        {
            SamanUniformGame_SetItemsInfo(hairList);
        }
        else if (selectedItem == SelectedItem.Earrings)
        {
            SamanUniformGame_SetItemsInfo(earringsList);
        }
        else if (selectedItem == SelectedItem.Crown)
        {
            SamanUniformGame_SetItemsInfo(CrownList);
        }
        else if (selectedItem == SelectedItem.Dress)
        {
            SamanUniformGame_SetItemsInfo(dressList);
        }
        else if (selectedItem == SelectedItem.Necklace)
        {
            SamanUniformGame_SetItemsInfo(necklaceList);
        }
        else if (selectedItem == SelectedItem.Bag)
        {
            SamanUniformGame_SetItemsInfo(BagList);
        }

    }
    #endregion

    #region SetItemsInfo
    private void SamanUniformGame_SetItemsInfo(List<ItemInfo> _ItemInfo)
    {
        if (_ItemInfo == null) return;
        for (int i = 0; i < _ItemInfo.Count; i++)
        {
            if (_ItemInfo[i].isLocked)
            {
                if (_ItemInfo[i].videoUnlock)
                {
                    if (_ItemInfo[i].videoBtn)
                    {
                        _ItemInfo[i].videoBtn.SetActive(true);
                    }
                    if (_ItemInfo[i].coinSlot)
                    {
                        _ItemInfo[i].coinSlot.SetActive(false);
                    }
                }
                else if (_ItemInfo[i].coinsUnlock)
                {
                    if (_ItemInfo[i].videoBtn)
                    {
                        _ItemInfo[i].videoBtn.SetActive(false);
                    }
                    if (_ItemInfo[i].coinSlot)
                    {
                        _ItemInfo[i].coinSlot.SetActive(true);
                        if (_ItemInfo[i].unlockCoins)
                        {
                            _ItemInfo[i].unlockCoins.text = _ItemInfo[i].requiredCoins.ToString();
                        }
                    }
                }
            }
            else
            {
                if (_ItemInfo[i].videoBtn) _ItemInfo[i].videoBtn.SetActive(false);
                if (_ItemInfo[i].coinSlot) _ItemInfo[i].coinSlot.SetActive(false);
            }
        }
    }
    #endregion

    #region StartPlaying
    public void SamanUniformGame_StartPlaying()
    {
        uIElements.PlayerCharacter.transform.parent.gameObject.SetActive(true);
        uIElements.PlayerCharacter.transform.localPosition += Vector3.right * 1000;
        StartCoroutine(AdDelay(40));
        StartCoroutine(SamanUniformGame_ShowInitialValues());
    }
    #endregion

    #region ShowInitialValues
    IEnumerator SamanUniformGame_ShowInitialValues()
    {
        yield return new WaitForSeconds(1f);
        uIElements.PlayerCharacter.transform.DOLocalMove(new Vector3(-10f, -50f, 300f), 1);
        uIElements.coinSlot.SetActive(true);
        uIElements.videoSlot.SetActive(true);
        uIElements.AllScrollers.SetActive(true);
    }
    #endregion

    #region SelectItem
    public void SamanUniformGame_SelectItem(int index)
    {
        if (selectedItem == SelectedItem.Hair)
        {
            SamanUniformGame_CheckSelectedItem(hairList, HairSprites, uIElements.PlayerHair);
        }

        else if (selectedItem == SelectedItem.Earrings)
        {
            SamanUniformGame_CheckSelectedItem(earringsList, EarringSprites, uIElements.PlayerEarrings);
        }
        else if (selectedItem == SelectedItem.Crown)
        {
            SamanUniformGame_CheckSelectedItem(CrownList, CrownSprites, uIElements.PlayerCrown);
        }
        else if (selectedItem == SelectedItem.Dress)
        {
            SamanUniformGame_CheckSelectedItem(dressList, DressSprites, uIElements.PlayerDress);
        }     
        else if (selectedItem == SelectedItem.Necklace)
        {
            SamanUniformGame_CheckSelectedItem(necklaceList, NecklaceSprites, uIElements.PlayerNecklace);
        }
        else if (selectedItem == SelectedItem.Bag)
        {
            SamanUniformGame_CheckSelectedItem(BagList, BagSprites, uIElements.PlayerBag);
        }

    }
    #endregion

    #region CheckSelectedItem
    private void SamanUniformGame_CheckSelectedItem(List<ItemInfo> itemInfoList, Sprite[] itemSprites, Image itemImage)
    {
        rewardType = RewardType.SelectionItem;
        if (itemInfoList.Count > selectedIndex)
        {
            tempItem = itemInfoList[selectedIndex];
            if (itemInfoList[selectedIndex].isLocked)
            {
                if (itemInfoList[selectedIndex].videoUnlock)
                {
                    SamanUniformGame_CheckVideoStatus();
                }
                else if (itemInfoList[selectedIndex].coinsUnlock)
                {
                    if (SaveData.Instance.Coins >= itemInfoList[selectedIndex].requiredCoins)
                    {
                        itemInfoList[selectedIndex].isLocked = false;
                        SaveData.Instance.Coins -= itemInfoList[selectedIndex].requiredCoins;
                        SamanUniformGame_UnlockSingleItem();
                        if (purchaseSfx) purchaseSfx.Play();
                        SamanUniformGame_SelectItem(selectedIndex);
                    }
                    else
                    {
                        if (uIElements.notEnoughPanel)
                            uIElements.notEnoughPanel.SetActive(true);
                    }
                }
            }
            else
            {
                if (itemSprites.Length > selectedIndex)
                {
                    if (itemSprites[selectedIndex])
                    {
                        if (itemImage)
                        {
                            itemImage.gameObject.SetActive(false);
                            itemImage.gameObject.SetActive(true);
                            itemImage.sprite = itemSprites[selectedIndex];
                            if (selectedItem == SelectedItem.Bag)
                                BagRank = 1;
                            else if (selectedItem == SelectedItem.Necklace)
                                necklaceRank = 1;
                            else if (selectedItem == SelectedItem.Earrings)
                                earringsRank = 1;
                     
                            readyCount++;
                            if (itemImage.transform.childCount > 0)
                            {
                                itemImage.transform.GetChild(0).gameObject.SetActive(true);
                            }
                        }
                    }
                }
                uIElements.totalCoins.text = SaveData.Instance.Coins.ToString();
                SamanUniformGame_GetItemsInfo();
                SamanUniformGame_CheckInterstitialAD();
            }
        }
    }
    #endregion

    #region GetRank
    private int SamanUniformGame_GetRank(int selectedCard, int totalItems)
    {
        int rankDivider = 0;
        rankDivider = totalItems / 3;
        if (rankDivider == 0)
        {
            rankDivider += 1;
        }
        if (selectedCard / rankDivider < 3)
        {
            return (selectedCard / rankDivider) + 1;
        }
        else
        {
            return 3;
        }
    }
    #endregion

    #region GetRewardedCoins
    public void SamanUniformGame_GetRewardedCoins()
    {
        rewardType = RewardType.Coins;
        SamanUniformGame_CheckVideoStatus();
    }
    #endregion

    #region GetDoubleRewarded
    public void SamanUniformGame_GetDoubleRewarded()
    {
        rewardType = RewardType.DoubleReward;
        SamanUniformGame_CheckVideoStatus();
    }
    #endregion

    #region CheckVideoStatus
    public void SamanUniformGame_CheckVideoStatus()
    {
        
    }
    #endregion

    #region UnlockSingleItem
    public void SamanUniformGame_UnlockSingleItem()
    {
        if (selectedItem == SelectedItem.Hair)
        {
            SaveData.Instance.Mode3Items.HairLock[selectedIndex] = false;
        }
        else if (selectedItem == SelectedItem.Earrings)
        {
            SaveData.Instance.Mode3Items.EarringsLock[selectedIndex] = false;
        }
        else if (selectedItem == SelectedItem.Crown)
        {
            SaveData.Instance.Mode3Items.CrownLock[selectedIndex] = false;
        }
        else if (selectedItem == SelectedItem.Dress)
        {
            SaveData.Instance.Mode3Items.DressLock[selectedIndex] = false;
        }
        else if (selectedItem == SelectedItem.Necklace)
        {
            SaveData.Instance.Mode3Items.NecklaceLock[selectedIndex] = false;
        }
        else if (selectedItem == SelectedItem.Bag)
        {
            SaveData.Instance.Mode3Items.BagLock[selectedIndex] = false;
        }
        SamanGame_SaveLoad.SaveProgress();
    }
    #endregion

    #region SelectCategory
    public void SamanUniformGame_SelectCategory(int index)
    {
        SamanUniformGame_DisableScroller();
        if (index == 0)
        {
            uIElements.DressScroller.SetActive(true);
            selectedItem = SelectedItem.Dress;
            uIElements.PlayerCharacter.transform.DOLocalMove(new Vector3(-50f, -350f, 200f), 1);
        }
       
        else if (index == 1)
        {
            uIElements.HairScroller.SetActive(true);
            selectedItem = SelectedItem.Hair;
            uIElements.PlayerCharacter.transform.DOLocalMove(new Vector3(-5f, -500f, -200f), 1);
        }
        else if (index == 3)
        {
            uIElements.CrownScroller.SetActive(true);
            selectedItem = SelectedItem.Crown;
            uIElements.PlayerCharacter.transform.DOLocalMove(new Vector3(-20f, -460f, -100f), 1);
        }
        else if (index == 2)
        {
            uIElements.NecklaceScroller.SetActive(true);
            selectedItem = SelectedItem.Necklace;
            uIElements.PlayerCharacter.transform.DOLocalMove(new Vector3(0f, -530f, -200f), 1);
        }
        else if (index == 4)
        {
            uIElements.EarringScroller.SetActive(true);
            selectedItem = SelectedItem.Earrings;
            uIElements.PlayerCharacter.transform.DOLocalMove(new Vector3(-30f, -410f, -300f), 1);
        }
       
        else if (index == 5)
        {
            uIElements.BagScroller.SetActive(true);
            selectedItem = SelectedItem.Bag;
            uIElements.PlayerCharacter.transform.DOLocalMove(new Vector3(-70f, -200f, 250f), 1);
        }
      
        SamanUniformGame_GetItemsInfo();
        if (categorySelectSfx) categorySelectSfx.Play();
    }
    #endregion

    #region ManageStars
    private void ManageStars(int playerPoints, int opponentPoints)
    {
        if (uIElements.playerStars.Length > playerStarCount && playerPoints >= opponentPoints)
        {
            uIElements.playerStars[playerStarCount].SetActive(true);
            playerStarCount++;
        }
        if (uIElements.opponentStars.Length > opponentStarCount && playerPoints <= opponentPoints)
        {
            uIElements.opponentStars[opponentStarCount].SetActive(true);
            opponentStarCount++;
        }
    }
    #endregion

    #region DisableScroller
    public void SamanUniformGame_DisableScroller()
    {
        uIElements.DressScroller.SetActive(false);
        uIElements.HairScroller.SetActive(false);
       
        uIElements.EarringScroller.SetActive(false);
        uIElements.CrownScroller.SetActive(false);
        uIElements.NecklaceScroller.SetActive(false);
        uIElements.BagScroller.SetActive(false);
    }
    #endregion

    #region AvtarSelection
    public void SamanUniformGame_AvtarSelection(int i)
    {
        uIElements.playerAvatar_1.sprite = PlayerAvatars[i];
        uIElements.opponentAvatar_2.sprite = PlayerAvatars[i];
        
    }
    #endregion

    #region AvtarSelected
    public void SamanUniformGame_AvtarSelected()
    {
        uIElements.avatarPanel.GetComponent<Image>().DOFade(0, 0.5f).OnComplete(delegate
        {
            uIElements.avatarPanel.SetActive(false);
            uIElements.StartPanel.SetActive(true);
        });
        StartCoroutine(SamanUniformGame_FindingOpponent());

    }
    #endregion

    #region TakeScreenShot
    public void SamanUniformGame_TakeScreenShot()
    {
        uIElements.screenShotImg.transform.parent.localScale = Vector3.one;
        uIElements.previewPanel.SetActive(false);
        StartCoroutine(TakeScreenShotNow());
        DownloadImage();
    }
    IEnumerator TakeScreenShotNow()
    {
        yield return new WaitForEndOfFrame();
        Texture2D _Texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        _Texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        _Texture.Apply();
        _Texture.LoadImage(_Texture.EncodeToPNG());
        Sprite sprite = Sprite.Create(_Texture, new Rect(0, 0, _Texture.width, _Texture.height), new Vector2(_Texture.width / 2, _Texture.height / 2));
        if (uIElements.screenShotImg)
        {
            uIElements.screenShotImg.sprite = sprite;
            uIElements.SS_Panel.SetActive(true);
        }
        _Taxture = _Texture;
    }
    public void DownloadImage()
    {
        string picturName = "ScreenShot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        //NativeGallery.SaveImageToGallery(_Taxture, "My Pictures", picturName);
        Invoke("PictureSaved", 0.8f);
    }
    private void PictureSaved()
    {
        uIElements.SS_Panel.SetActive(false);
        uIElements.previewPanel.SetActive(true);
        Destroy(_Taxture);
    }
    #endregion

    #region Preview
    public void SamanUniformGame_Preview()
    {
        if (readyCount >= 10)
        {
            uIElements.coinSlot.SetActive(false);
            uIElements.videoSlot.SetActive(false);
            uIElements.AllScrollers.SetActive(false);
            uIElements.previewPanel.SetActive(true);
            uIElements.PlayerCharacter.transform.DOLocalMove(new Vector3(-50f, -170f, 400f), 1);
        }
        else
        {
            if (uIElements.notReadyPanel) uIElements.notReadyPanel.SetActive(true);
        }
    }
    #endregion

    #region Restyle
    public void SamanUniformGame_Restyle()
    {
        uIElements.coinSlot.SetActive(true);
        uIElements.videoSlot.SetActive(true);
        uIElements.AllScrollers.SetActive(true);
        uIElements.previewPanel.SetActive(false);
        uIElements.PlayerCharacter.transform.DOLocalMove(new Vector3(50f, -50f, 300f), 1);
    }
    #endregion

    #region Submits
    public void SamanUniformGame_Submit()
    {
        uIElements.coinSlot.SetActive(false);
        uIElements.videoSlot.SetActive(false);
        uIElements.AllScrollers.SetActive(false); uIElements.PlayerCharacter.transform.DOLocalMove(new Vector3(-230f, -240f, 300f), 0.5f).OnComplete(delegate
        {
            uIElements.WaitImage.gameObject.SetActive(true);
            IsTimeRunning = true;
        });
        if (_Taxture != null) Destroy(_Taxture);
         
    }
    #endregion

    #region Judgement
    IEnumerator SamanUniformGame_Judgement()
    {
        SamanUniformGame_DressupOpponent();
        yield return new WaitForSeconds(0.5f);
        uIElements.OpponentCharacter.transform.localPosition += Vector3.right * 1000;
        uIElements.OpponentCharacter.transform.parent.gameObject.SetActive(true);
        uIElements.OpponentCharacter.transform.DOLocalMove(new Vector3(450f, -240f, 300f), 0.5f);
        yield return new WaitForSeconds(0.5f);
        uIElements.judgesPanel.transform.parent.parent.gameObject.SetActive(true);
        uIElements.judgesPanel.transform.localPosition += Vector3.down * 500;
        uIElements.judgesPanel.transform.DOLocalMove(new Vector3(0f, 0f, 0f), 0.5f);
        yield return new WaitForSeconds(2f);
        uIElements.playerPoints.text = ((((dressRank + BagRank) * 100) / 20)).ToString();
        uIElements.opponentPoints.text = OP_DressingPoints.ToString();
        StartCoroutine(SamanUniformGame_AddPlayerScore((((dressRank + BagRank) * 100) / 20)));
        StartCoroutine(SamanUniformGame_AddOpponentScore(OP_DressingPoints));
        ManageStars((((dressRank + BagRank) * 100) / 20), OP_DressingPoints);
        yield return new WaitForSeconds(2.5f);
        uIElements.playerPoints.text = ((((CrownRank + hairsRank + eyesRank) * 100) / 30)).ToString();
        uIElements.opponentPoints.text = OP_MakeupPoints.ToString();
        StartCoroutine(SamanUniformGame_AddPlayerScore((((hairsRank + eyesRank) * 100) / 30)));
        StartCoroutine(SamanUniformGame_AddOpponentScore(OP_MakeupPoints));
        ManageStars((((CrownRank + hairsRank + eyesRank) * 100) / 30), OP_MakeupPoints);
        yield return new WaitForSeconds(2.5f);
        uIElements.playerPoints.text = ((((necklaceRank + earringsRank +hairsRank) * 100) / 30)).ToString();
        uIElements.opponentPoints.text = OP_JewellertPoints.ToString();
        StartCoroutine(SamanUniformGame_AddPlayerScore((((necklaceRank + earringsRank +hairsRank) * 100) / 30)));
        StartCoroutine(SamanUniformGame_AddOpponentScore(OP_JewellertPoints));
        ManageStars((((necklaceRank + earringsRank + hairsRank) * 100) / 30), OP_JewellertPoints);
        yield return new WaitForSeconds(2f);
        uIElements.judgesPanel.transform.parent.parent.gameObject.SetActive(false);
        if (playerScore >= opponentScore)
        {
            uIElements.PlayerCharacter.transform.parent = uIElements.characterParent.transform;
            uIElements.PlayerCharacter.transform.DOLocalMove(new Vector3(50f, -250f, 100f), 0.5f);
            uIElements.OpponentCharacter.gameObject.SetActive(false);
            //uIElements.winHeader.SetActive(true);
            //uIElements.trophyPanel.SetActive(true);
            //uIElements.loseHeader.SetActive(false);
        }
        else
        {
            uIElements.OpponentCharacter.transform.parent = uIElements.characterParent.transform;
            uIElements.OpponentCharacter.transform.DOLocalMove(new Vector3(50f, -250f, 100f), 0.5f);
            uIElements.PlayerCharacter.gameObject.SetActive(false);
            //uIElements.winHeader.SetActive(false);
            //uIElements.trophyPanel.SetActive(false);
            //uIElements.loseHeader.SetActive(true);
        }
        finalParticles.SetActive(true);
        PlayerPrefs.SetInt("Level", 2);
        uIElements.rewardCoins.text = (playerScore * 20).ToString();
        SaveData.Instance.Coins += playerScore * 20;
        SamanGame_SaveLoad.SaveProgress();
        if (playerScore >= opponentScore)
            yield return new WaitForSeconds(3f);
        else
            yield return new WaitForSeconds(1f);
        uIElements.levelComplete.SetActive(true);
        yield return null;
    }
    #endregion

    #region DressupOpponent
    private void SamanUniformGame_DressupOpponent()
    {
        int wearProbability = Random.Range(0, 3);
        if (wearProbability != 0)
        {
            OP_DressingPoints++;
            WearItem(uIElements.OpponentBag, BagSprites);
        }
        OP_DressingPoints++;
       // WearItem(uIElements.OpponentShoes, ShoesSprites);
        OP_DressingPoints = ((OP_DressingPoints * 100) / 20 );


        wearProbability = Random.Range(0, 3);
        if (wearProbability != 0)
        {
            OP_MakeupPoints++;
            WearItem(uIElements.OpponentEarrings, EarringSprites);
        }
        OP_MakeupPoints++;
        WearItem(uIElements.OpponentHair, HairSprites);
        OP_MakeupPoints++;
        WearItem(uIElements.OpponentBag, BagSprites);
        OP_MakeupPoints = ((OP_MakeupPoints * 100) / 30);

        wearProbability = Random.Range(0, 3);
        if (wearProbability != 0)
        {
            OP_JewellertPoints++;
            WearItem(uIElements.OpponentNecklace, NecklaceSprites);
        }
        OP_JewellertPoints++;
        WearItem(uIElements.OpponentCrown, CrownSprites);
        wearProbability = Random.Range(0, 3);
        if (wearProbability != 0)
        {
            OP_JewellertPoints++;
            WearItem(uIElements.OpponentEarrings, EarringSprites);
        }
        OP_JewellertPoints = ((OP_JewellertPoints * 100) / 30);
    }
    private void WearItem(Image targetImg, Sprite[] targetSprites)
    {
        if (targetImg && targetSprites.Length > 0)
        {
            targetImg.sprite = targetSprites[Random.Range(0, targetSprites.Length)];
            targetImg.gameObject.SetActive(true);
        }
    }
    #endregion

    #region AddOpponentScore
    IEnumerator SamanUniformGame_AddOpponentScore(int scoreToAdd)
    {
        int modValue = scoreToAdd % 10;
        int perValue = (scoreToAdd) / 10;
        int loopValue = 0;
        if (perValue == 0)
        {
            modValue = 0;
            perValue = 1;
            loopValue = scoreToAdd;
        }
        else
        {
            loopValue = 10;
        }
        for (int i = 0; i < loopValue; i++)
        {
            opponentScore += perValue;
            if (uIElements.opponentScore)
                uIElements.opponentScore.text = "Score : " + opponentScore;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        opponentScore += modValue;
        if (uIElements.opponentScore)
            uIElements.opponentScore.text = "Score : " + opponentScore;
    }
    #endregion

    #region AddPlayerScore
    IEnumerator SamanUniformGame_AddPlayerScore(int scoreToAdd)
    {
        int modValue = scoreToAdd % 10;
        int perValue = (scoreToAdd) / 10;
        int loopValue = 0;
        if (perValue == 0)
        {
            modValue = 0;
            perValue = 1;
            loopValue = scoreToAdd;
        }
        else
        {
            loopValue = 10;
        }
        for (int i = 0; i < loopValue; i++)
        {
            playerScore += perValue;
            if (uIElements.playerScore)
                uIElements.playerScore.text = "Score : " + playerScore;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        playerScore += modValue;
        if (uIElements.playerScore)
            uIElements.playerScore.text = "Score : " + playerScore;
    }
    #endregion

    #region RewardedVideoCompleted
    public void SamanUniformGame_OnRewardedVideoComplete()
    {
        if (canShowInterstitial)
        {
            canShowInterstitial = !canShowInterstitial;
            StartCoroutine(AdDelay(2));
        }
        if (rewardType == RewardType.SelectionItem)
        {
            if (tempItem != null) tempItem.isLocked = false;
            SamanUniformGame_UnlockSingleItem();
            SamanUniformGame_SelectItem(selectedIndex);
        }
        else if (rewardType == RewardType.Coins)
        {
            SaveData.Instance.Coins += 2000;
            uIElements.totalCoins.text = SaveData.Instance.Coins.ToString();
            SamanGame_SaveLoad.SaveProgress();
        }
        else if (rewardType == RewardType.DoubleReward)
        {
            uIElements.rewardCoins.text = ((playerScore * 20) * 2).ToString();
            SaveData.Instance.Coins += playerScore * 20;
            SamanGame_SaveLoad.SaveProgress();
        }
        SamanUniformGame_GetItemsInfo();
        rewardType = RewardType.None;
        if (purchaseSfx) purchaseSfx.Play();
    }
    #endregion

    #region ShowInterstitialAD
    private void SamanUniformGame_CheckInterstitialAD()
    {
       
    }
    IEnumerator ShowInterstitialAD()
    {
        if (uIElements.AD_Panel)
        {
            uIElements.AD_Panel.SetActive(true);
            yield return new WaitForSeconds(1f);
            uIElements.AD_Panel.SetActive(false);
        }
       
    }
    IEnumerator AdDelay(float _Delay)
    {
        yield return new WaitForSeconds(_Delay);
        canShowInterstitial = !canShowInterstitial;
    }
    #endregion

    #region Scene Loading
    public void SamanUniformGame_Continue()
    {
        //StartCoroutine(LoadScene(Scenes.MyLevelSelection.ToString(), 4f));
        //LogEvents.Instance.GA_Event(GA_EventType.Completed, "Mode_1");
    }
    IEnumerator SamanUniformGame_LoadScene(string str, float delay)
    {
        uIElements.uiPanel.SetActive(false);
        uIElements.fillbar.fillAmount = 0;
        uIElements.loadingPanel.SetActive(true);
        while (uIElements.fillbar.fillAmount < 1)
        {
            uIElements.fillbar.fillAmount += Time.deltaTime / delay;
            yield return null;
        }
        SceneManager.LoadScene(str);
        yield return null;
    }
    #endregion

    #region UnlockAllItems
    public void SamanUniformGame_UnlockAllItems()
    {
        SamanUniformGame_UnlockList(dressList);
        SamanUniformGame_UnlockList(hairList);
        SamanUniformGame_UnlockList(earringsList);
        SamanUniformGame_UnlockList(CrownList);
        SamanUniformGame_UnlockList(necklaceList);
        SamanUniformGame_UnlockList(BagList);
    


        SamanUniformGame_GetItemsInfo();
    }
    #endregion

    #region UnlockList
    private void SamanUniformGame_UnlockList(List<ItemInfo> itemList = null)
    {
        if (itemList != null)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].isLocked = false;
            }
        }
    }
    #endregion

    #region BackgroundFun
    public void SamanUniformGame_BackgroundFun()
    {
        if (count < BackGround.Length)
        {
            uIElements.BGimage.sprite = BackGround[count];
            count++;
            if (count >= BackGround.Length)
                count = 0;
        }
    }
    #endregion

    #region Back
    public void SamanUniformGame_Back()
    {
        uIElements.fillbar.fillAmount = 0;
        uIElements.loadingPanel.SetActive(true);
        uIElements.fillbar.DOFillAmount(1, 2).SetEase(Ease.Linear).OnComplete(delegate
        {
            SceneManager.LoadScene(Scenes.MyLevelSelection.ToString());
        });
    }
    #endregion

    #region Next
    public void SamanUniformGame_Next()
    {
        uIElements.fillbar.fillAmount = 0;
        uIElements.loadingPanel.SetActive(true);
        uIElements.fillbar.DOFillAmount(1, 2).SetEase(Ease.Linear).OnComplete(delegate
        {
            SceneManager.LoadScene(Scenes.MyLevelSelection.ToString());
        });
    }
    #endregion

    #region Update
    public void Update()
    {
        if(IsTimeRunning)
        {
            if(timeRemaning>=0)
            {
                timeRemaning -= Time.deltaTime;
                float second = Mathf.FloorToInt(timeRemaning % 60);
                uIElements.Timetxt.text = "00:0" + second.ToString();
            }
            else
            {
                IsTimeRunning = false;
                uIElements.WaitImage.gameObject.SetActive(false);
                print("Time Run Out");
                StartCoroutine(SamanUniformGame_Judgement());
                
            }
        }
    }
    #endregion

    #region Share
    public void SamanUniformGame_Share()
    {
        new NativeShare()
            .SetSubject("Game Link")
            .SetText("This is My Game")
            .Share();
    }
    #endregion

}
