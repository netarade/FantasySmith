using System;
using UnityEngine;
using ItemData;
using InventoryManagement;
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
* <v2.0 - 2024_0115_�ֿ���>
* 1- ǥ���ǿɼ��� InteractiveŬ�������� �Űܿ�.
* 2- �����ɼ� ���� (�ݵ�� �����ɼ��� �����ϹǷ�)
* 
*/
public class InventoryInitializer : MonoBehaviour
{
    [Header("�κ��丮 ��� ���� ����")]
    public bool isReset;

    [Header("��ųʸ��� ���� ���Ѽ��� ����(���� ��ųʸ� �Ұ�)")]
    public DicType[] dicTypes;
    
    [Header("Ȱ��ȭ �� ��Ƽ������ ������ ����")]
    [Header("(���̰� 0�̸� ��ǥ�� ���� - ��ü������ �۵�)")]
    [Header("(ǥ���� ���� All-��ü, Quest-����Ʈ, Misc-��ȭ, Equip-���)")]
    public TabType[] showTabType;


    [Header("----- �Ʒ����� �̿ϼ� -----")]
    [Header("�������� �̸��� ���� ������ ���� ����")]
    public bool isCustomFileSetting;
    public string fileName;
    public int saveSlotNo;

}


/// <summary>
/// ��ųʸ��� ������ ���� ���Ѽ��� ��Ÿ���� ����ü�Դϴ�.<br/>
/// Initializer���� Inventory�� �����Ͽ� �ʱ�ȭ ���� �ֱ� ���ѿ뵵�� ���˴ϴ�.<br/>
/// </summary>
[Serializable]
public struct DicType
{
    public ItemType itemType;
    public int slotLimit;
}