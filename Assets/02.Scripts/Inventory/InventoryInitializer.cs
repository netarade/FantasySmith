using System;
using UnityEngine;
using ItemData;

public class InventoryInitializer : MonoBehaviour
{
    [Header("리셋 버튼")]
    public bool isReset;

    [Header("딕셔너리와 제한수를 지정(같은 딕셔너리 불가)")]
    public DicType[] dicTypes;   
}

[Serializable]
public struct DicType
{
    public ItemType itemType;
    public int slotLimit;
}