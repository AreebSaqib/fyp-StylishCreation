using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindOpponent : MonoBehaviour
{
    public Image targetImage;
    public Sprite[] avatarImg;
    private List<Sprite> avatarList = new List<Sprite>();
    private int startIndex;
    private float findDelay;
    private float nextDealy;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < avatarImg.Length; i++)
        {
            if (avatarImg[i])
                avatarList.Add(avatarImg[i]);
        }
        startIndex = Random.Range(1, avatarList.Count);
        StartCoroutine(FindingOpponent());
    }

    IEnumerator FindingOpponent()
    {
        while(findDelay < 4)
        {
            if (targetImage && nextDealy < findDelay)
            {
                targetImage.sprite = avatarList[startIndex];
                startIndex++;
                if (startIndex >= avatarList.Count)
                    startIndex = 0;
                nextDealy += 0.2f;
            }
            yield return null;
            findDelay += Time.deltaTime;
        }
        yield return null;
    }
}
