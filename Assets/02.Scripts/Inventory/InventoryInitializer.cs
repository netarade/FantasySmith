using System;
using UnityEngine;
using ItemData;
/*
 * [�۾� ����]  
 * <v1.0 - 2024_0101_�ֿ���>
 * 1 - �κ��丮�� ���ϴ� ������ ���ϴ� ���� ũ�⸸ŭ �����ϱ� ���� �ɼ� �߰�
 * 
 * <v1.1 - 2024_0102_�ֿ���>
 * 1- ���� ĭ���� �ɼ��� �߰�
 * ������ ��ü �������� ������ �߰��ϱ� ����
 * 
 * 2- �������� �̸� ���� ���� �ɼ��� �߰� 
 * 
 */
public class InventoryInitializer : MonoBehaviour
{
    [Header("�κ��丮 ��� ���� ����")]
    public bool isReset;

    [Header("��ųʸ��� ���� ���Ѽ��� ����(���� ��ųʸ� �Ұ�)")]
    public DicType[] dicTypes;
    
    [Header("----- �Ʒ����� �̿ϼ� -----")]
    [Header("�������� �̸��� ���� ������ ���� ����")]
    public bool isCustomFileSetting;
    public string fileName;
    public int saveSlotNo;

    [Header ("����ĭ ���� �ɼ�")]
    [Header("(������ ������ �Ұ����ϸ�, ��ü�����θ� �� �� �ֽ��ϴ�.)")]
    public bool isShare;

    [Header ("����ĭ ���� �� ���� ĭ�� ����")]
    public int shareSlotCountLimit;

    [Header ("���� ���� �� ���� Ÿ��(��ü�� ���� ����)")]
    public DicType[] shareExceptTabTypes;
}

[Serializable]
public struct DicType
{
    public ItemType itemType;
    public int slotLimit;
}