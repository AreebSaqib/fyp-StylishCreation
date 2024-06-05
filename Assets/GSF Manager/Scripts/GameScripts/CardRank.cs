using UnityEngine;

public class CardRank : MonoBehaviour
{
    public int cardRank;
    public GameObject[] cardStars;

    public void SetRank(int _Rank)
    {
        cardRank = _Rank;
        for(int i = 0; i < cardStars.Length; i++)
        {
            if (cardStars[i])
            {
                if(i < _Rank)
                {
                    cardStars[i].SetActive(true);
                }
                else
                {
                    cardStars[i].SetActive(false);
                }
            }
        }
    }
}
