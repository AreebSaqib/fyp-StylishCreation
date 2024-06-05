using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class IconAd : MonoBehaviour
{
    public GameObject adIcon;
    public Image iconImage;
    public Button iconBtn;
    public int iconIndex = 0;
    private string appLink = "";

    private void OnEnable()
    {
        HandleIcon(false);
        if (GamesInfo.Instance.gamesData.Count > iconIndex)
        {
            ManageIconAd();
        }
    }

    public void OpenLink()
    {
        Application.OpenURL(appLink);
    }

    private void ManageIconAd()
    {
        if (!string.IsNullOrEmpty(GamesInfo.Instance.gamesData[iconIndex].appLink) && GamesInfo.Instance.gamesData[iconIndex].appIcon)
        {
            appLink = GamesInfo.Instance.gamesData[iconIndex].appLink;
            iconImage.sprite = GamesInfo.Instance.gamesData[iconIndex].appIcon;
            HandleIcon(true);
        }
        else
        {
            HandleIcon(false);
        }
    }

    private void HandleIcon(bool isTrue)
    {
        if (iconBtn)
            iconBtn.interactable = isTrue;
        if (iconImage)
            iconImage.enabled = isTrue;
        if (adIcon)
            adIcon.SetActive(isTrue);
    }
}
