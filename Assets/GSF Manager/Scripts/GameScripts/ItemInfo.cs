using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;


public class ItemInfo : MonoBehaviour
{
    public Button itemBtn;
    public Image itemIcon;
    public GameObject videoBtn;
    public GameObject coinSlot;
    public Text unlockCoins;
    public bool isLocked;
    [ShowIf("isLocked")]
    public bool videoUnlock;
    [ShowIf("isLocked")]
    public bool coinsUnlock;
    [Range(0, 50000)]
    [ShowIf("coinsUnlock")]
    public int requiredCoins;
}
