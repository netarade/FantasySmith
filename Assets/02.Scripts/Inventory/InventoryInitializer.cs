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
* <v2.1 - 2024_0125_�ֿ���>
* 1- ���ϼ����� �����ϰ� �κ��丮 �ĺ���ȣ�� Ÿ������ inventoryId�� �߰�
* 
* 2- �κ��丮 ������ �����ϱ� ���� isItem3DStore�� isInstantiated �Ӽ��� �߰��Ͽ���
* (���� 3d ������ ���� �κ��丮�� ���� ���� �߿� �����Ǵ� �κ��丮������ �˷��ִ� �Ӽ�)
* 
* 
*/
public class InventoryInitializer : MonoBehaviour
{

    [Header("�κ��丮 ��� ���� ����")]
    public bool isReset = false;
    
    [Header("�κ��丮 �ĺ� ��ȣ")]
    public int inventoryId = -1;

    [Header("�������� 3d���·� �����ϴ� �κ��丮 ����")]
    public bool isItem3dStore = false;

    [Header("���� �߿� �����Ǵ� �κ��丮 ����")]
    public bool isInstantiated = false;


    [Header("��ųʸ��� ���� ���Ѽ��� ����(���� ��ųʸ� �Ұ�)")]
    public DicType[] dicTypes;
    
    [Header("Ȱ��ȭ �� ��Ƽ������ ������ ����")]
    [Header("(���̰� 0�̸� ��ǥ�� ���� - ��ü������ �۵�)")]
    [Header("(ǥ���� ���� All-��ü, Quest-����Ʈ, Misc-��ȭ, Weapon-���)")]
    public TabType[] showTabType;


    public InventoryInfo inventoryInfo;

    private void Awake()
    {
        inventoryInfo = GetComponent<InventoryInfo>();
    }

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