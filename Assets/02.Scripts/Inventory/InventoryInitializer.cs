using System;
using UnityEngine;
using ItemData;
/*
 * [�۾� ����]  
 * <v1.0 - 2023_1106_�ֿ���>
 * 1 - �κ��丮�� ���ϴ� ������ ���ϴ� ���� ũ�⸸ŭ �����ϱ� ���� �ɼ� �߰�
 * 
 * 
 */
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