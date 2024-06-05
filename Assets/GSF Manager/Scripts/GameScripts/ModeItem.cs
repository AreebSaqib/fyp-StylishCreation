using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;

public class ModeItem : MonoBehaviour
{
    public Button itemBtn;
    public GameObject lockIcon;
    public GameObject coinSlot;
    public Text unlockCoins;
    public bool isLocked;
    [ShowIf("isLocked")]
    public bool coinsUnlock;
    [Range(0, 50000)]
    [ShowIf("coinsUnlock")]
    public int requiredCoins;
    public int itemRank;
}
