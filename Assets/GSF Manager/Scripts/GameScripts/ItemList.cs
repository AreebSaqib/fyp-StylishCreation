using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class ItemValues
{
    public ItemInfo itemInfo;
    public bool isLocked;
    [ShowIf("isLocked")]
    public bool videoUnlock;
    [ShowIf("isLocked")]
    public bool coinsUnlock;
    [Range(0, 50000)]
    [ShowIf("coinsUnlock")]
    public int requiredCoins;
}

public class ItemList : MonoBehaviour
{
    public ItemValues[] itemValues;
}
