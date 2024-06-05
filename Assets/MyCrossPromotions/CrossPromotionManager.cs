using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CrossPromotionManager : MonoBehaviour
{
    public int maxLoads = 2;
    private string iconName = "";
    private string serverPath = "";

    private void Start()
    {
        if (GamesInfo.Instance != null)
        {
            StartCoroutine(GetText());
        }
    }

    IEnumerator GetTexture(int listIndex)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(serverPath);
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            if (request.isNetworkError || request.isHttpError)
                Debug.Log(request.error);
            else
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(request);
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                GamesInfo.Instance.gamesData[listIndex].appIcon = sprite;
            }
        }
    }

    IEnumerator GetText()
    {
        //You Have To Change the Server link 
        UnityWebRequest request = UnityWebRequest.Get("https://raw.githubusercontent.com/Addi111/CrossPromotions/main/OverloadedWings/Links/LinksForCustomAdSequence.json");
        yield return request.SendWebRequest();
        if (request.isDone)
        {
            if (request.isNetworkError || request.isHttpError)
                Debug.Log(request.error);
            else
            {
                string fileContent = request.downloadHandler.text.ToString();
                JsonUtility.FromJsonOverwrite(fileContent, GamesInfo.Instance);
                for (int i = 0; i < GamesInfo.Instance.gamesData.Count; i++)
                {
                    if (i < maxLoads)
                    {
                        iconName = GamesInfo.Instance.gamesData[i].appName;
                        serverPath = "https://raw.githubusercontent.com/Addi111/CrossPromotions/main/Icons/" + iconName + ".png";
                        StartCoroutine(GetTexture(i));
                    }
                }
            }
        }
    }
}
