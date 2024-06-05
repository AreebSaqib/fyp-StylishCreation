using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[System.Serializable]
public class Rewarded_Videos_Properties
{
    public enum RewardType
    {
        coinsBoost, revivePlayer, skipLevel
    }
    public RewardType rewardType;
    public GameObject rewardPanel;
    public Button AdVideoBtn;
}


public class GSF_OfferWall : MonoBehaviour
{
    private enum RewardType
    {
        coinsBoost, revivePlayer, skipLevel
    }
    private RewardType rewardType;

    [ListDrawerSettings(ShowIndexLabels = true)]
    public Rewarded_Videos_Properties[] videosProperties; 

	[Header ("Ad Sequence ID")]
	public int SequenceID = 5;
    private int rewardIndex;
	void OnEnable ()
    {
        //if (ConsoliAds.Instance != null)
        //{
        //    ConsoliAds.onRewardedVideoAdCompletedEvent += RewardedVideoCompleted;
        //}
    }

	void OnDisable ()
    {
        //if (ConsoliAds.Instance != null)
        //{
        //    ConsoliAds.onRewardedVideoAdCompletedEvent -= RewardedVideoCompleted;
        //}
    }

    #region Start
    void Start ()
    {
        SetupOfferWalls();
        DisableOfferWalls();
        StartCoroutine(CheckRewardedVideo());
	}
    #endregion

    #region ShowRewardedVideo
    private void ShowRewardedVideo (int _Index)
    {
        rewardIndex = _Index;
        GSF_AdsManager.ShowRewardedVideo (SequenceID, "Rewarded Video");
    }
    #endregion

    #region SetupOfferWalls
    private void SetupOfferWalls()
    {
        for(int i=0;i< videosProperties.Length; i++)
        {
            if (videosProperties[i].AdVideoBtn)
            {
                int videoIndex = i;
                videosProperties[i].AdVideoBtn.onClick.AddListener(() => {
                    ShowRewardedVideo(videoIndex);
                });
            }
        }
    }
    #endregion

    #region RewardedVideoCompleted
    public void RewardedVideoCompleted ()
    {
        if (videosProperties[rewardIndex].rewardType == Rewarded_Videos_Properties.RewardType.coinsBoost)
        {
            if (FindObjectOfType<GSF_GameController>())
            {
                FindObjectOfType<GSF_GameController>().DoubleYourReward();
            }
            // You ca call your function here to give coins
        }
        else if (videosProperties[rewardIndex].rewardType == Rewarded_Videos_Properties.RewardType.revivePlayer)
        {
            Debug.Log("Revive Player");
            // You ca call your function here to revive player

        }
        else if (videosProperties[rewardIndex].rewardType == Rewarded_Videos_Properties.RewardType.skipLevel)
        {
            Debug.Log("Skip Level");
            // You ca call your function here to skip level
        }
        DisableOfferWalls();
        //if (ConsoliAds.Instance != null)
        //{
        //    ConsoliAds.Instance.LoadRewarded(SequenceID);
        //}
    }
    #endregion

    #region DisableOfferWalls
    private void DisableOfferWalls()
    {
        for (int i = 0; i < videosProperties.Length; i++)
        {
            if (videosProperties[i].rewardPanel)
            {
                videosProperties[i].rewardPanel.SetActive(false);
            }
            else if(videosProperties[i].AdVideoBtn)
            {
                videosProperties[i].AdVideoBtn.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region EnableOfferWalls
    private void EnableOfferWalls()
    {
        for (int i = 0; i < videosProperties.Length; i++)
        {
            if (videosProperties[i].rewardPanel)
            {
                videosProperties[i].rewardPanel.SetActive(true);
            }
            else if (videosProperties[i].AdVideoBtn)
            {
                videosProperties[i].AdVideoBtn.gameObject.SetActive(true);
            }
        }
    }
    #endregion

    #region CheckRewardedVideo
    IEnumerator CheckRewardedVideo ()
    {
        //if(ConsoliAds.Instance != null)
        //{
        //    ConsoliAds.Instance.LoadRewarded(SequenceID);
        //}
		yield return new WaitForSecondsRealtime (2f);
        //if (ConsoliAds.Instance != null)
        //{
        //    if (ConsoliAds.Instance.IsRewardedVideoAvailable(SequenceID))
        //    {
        //        EnableOfferWalls();
        //    }
        //    else
        //    {
        //        Debug.Log("Rewarded Video Not Available !");
        //    }
        //}
    }
    #endregion

}
