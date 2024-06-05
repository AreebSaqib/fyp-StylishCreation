using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class SamanGame_SaveLoad
{
    private static string filePath = Path.Combine(Application.persistentDataPath, "SavedGame.json");
    public static void SaveProgress()
    {
        SaveData CheckSave = new SaveData(SaveData.Instance.RemoveAds, SaveData.Instance.PurchashedAllItems, SaveData.Instance.LevelsUnlocked, SaveData.Instance.Coins, SaveData.Instance.Players,
            SaveData.instance.Mode2Items,SaveData.instance.Mode1Items,SaveData.instance.Mode3Items, SaveData.instance.Mode4Items, SaveData.instance.Mode5Items);
        string saveDataString = JsonUtility.ToJson(CheckSave, true);
        SaveData.Instance.hashOfSaveData = HashGenerator(saveDataString);
        string saveDataHashed = JsonUtility.ToJson(SaveData.Instance, true);
        File.WriteAllText(filePath, saveDataHashed);
    }

    public static void LoadProgress()
    {
        if (File.Exists(filePath))
        {
            string fileContent = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(fileContent, SaveData.Instance);
            Debug.Log(filePath);
        }
        else
        {
            SaveProgress();
        }
    }

    public static string HashGenerator(string saveContent)
    {
        SHA256Managed crypt = new SHA256Managed();
        string hash = string.Empty;
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(saveContent), 0, Encoding.UTF8.GetByteCount(saveContent));
        foreach (byte bit in crypto)
        {
            hash += bit.ToString("x2");
        }
        return hash;
    }

    public static void DeleteProgress()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
