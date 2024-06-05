using UnityEngine;
using System.Collections;

public class GSF_AdsManager
{

    public static void ShowInterstitial(int sequenceID = 0, string SceneName = null)
    {
        //if (MyAdsManager.Instance != null)
        //{
        //    if (SaveData.Instance != null)
        //    {
        //        if (!SaveData.Instance.RemoveAds)
        //        {
        //            MyAdsManager.Instance.ShowInterstitialAds(sequenceID);
        //        }
        //    }
        //}
        //else
        //{
        //    Debug.LogWarning("MyAdsManager Instance Not Found !");
        //}
    }

    public static void ShowRewardedVideo(int sequenceID, string SceneName)
    {
        //if (MyAdsManager.Instance != null)
        //{
        //    MyAdsManager.Instance.ShowRewardedVideos();
        //}
        //else
        //{
        //    Debug.LogWarning("MyAdsManager Instance Not Found !");
        //}
    }

    public static void ShowBanner(int sequenceID, string SceneName)
    {
        //if (MyAdsManager.Instance != null)
        //{
        //    if (SaveData.Instance != null)
        //    {
        //        if (!SaveData.Instance.RemoveAds)
        //        {
        //            MyAdsManager.Instance.ShowAdmobBanner();
        //        }
        //    }
        //}
        //else
        //{
        //    Debug.LogWarning("MyAdsManager Instance Not Found !");
        //}
    }

    public static void HideBanner()
    {
        //if (MyAdsManager.Instance != null)
        //{
        //    if(GameManager.Instance.bannerCalled)
        //        MyAdsManager.Instance.HideAdmobBanner();
        //}
        //else
        //{
        //    Debug.LogWarning("MyAdsManager Instance Not Found !");
        //}
    }

    public static void RemoveAdvertisements()
    {
        //if (MyAdsManager.Instance != null)
        //{
        //    if (GameManager.Instance.bannerCalled)
        //        MyAdsManager.Instance.HideAdmobBanner();
        //}
        //else
        //{
        //    Debug.LogWarning("MyAdsManager Instance Not Found !");
        //}
    }
}
