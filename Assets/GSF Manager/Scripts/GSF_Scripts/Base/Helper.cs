using System;
using UnityEngine;

public class Helper
{

    #region SetTextures
    public static void SetTextures(GameObject refObject, Texture refTextures)
    {
        var skinMeshes = refObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (skinMeshes.Length > 0)
        {
            for (int i = 0; i < skinMeshes.Length; i++)
            {
                for (int j = 0; j < skinMeshes[i].materials.Length; j++)
                {
                    if (skinMeshes[i].materials[j].name.Contains("_Body"))
                    {
                        skinMeshes[i].materials[j].SetTexture("_MainTex", refTextures);
                    }
                }
            }
        }
        var rendererMeshes = refObject.GetComponentsInChildren<MeshRenderer>();
        if (rendererMeshes.Length > 0)
        {
            for (int i = 0; i < rendererMeshes.Length; i++)
            {
                for (int j = 0; j < rendererMeshes[i].materials.Length; j++)
                {
                    if (rendererMeshes[i].materials[j].name.Contains("_Body"))
                    {
                        rendererMeshes[i].materials[j].SetTexture("_MainTex", refTextures);
                    }
                }
            }
        }
    }
    #endregion


    #region RemoveAds
    public static void RemoveAds()
    {
        SaveData.Instance.RemoveAds = true;
        GSF_AdsManager.RemoveAdvertisements();
        SamanGame_SaveLoad.SaveProgress();
    }
    #endregion

    #region UnlockAllLevels
    public static void UnlockAllLevels(int _levels)
    {
        SaveData.Instance.LevelsUnlocked = _levels;
        SamanGame_SaveLoad.SaveProgress();
    }
    #endregion

    #region UnlockAllPlayers
    public static void UnlockAllPlayers()
    {
        for(int x = 0; x < SaveData.Instance.Players.Count; x++)
        {
            SaveData.Instance.Players[x].isLocked = false;
        }
        SamanGame_SaveLoad.SaveProgress();
    }
    #endregion

    #region UnlockEverything
    public static void UnlockEverything()
    {
        RemoveAds();
        UnlockAllPlayers();
        UnlockAllLevels(25);
    }
    #endregion
}
