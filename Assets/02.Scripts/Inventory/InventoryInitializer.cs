using System;
using UnityEngine;
using ItemData;

public class InventoryInitializer : MonoBehaviour
{
    [Header("���� ��ư")]
    public bool isReset;

    [Header("��ųʸ��� ���Ѽ��� ����(���� ��ųʸ� �Ұ�)")]
    public DicType[] dicTypes;   
}

[Serializable]
public struct DicType
{
    public ItemType itemType;
    public int slotLimit;
}