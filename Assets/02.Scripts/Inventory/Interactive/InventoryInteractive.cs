using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InventoryManagement;
using ItemData;
using System;

/*
* [�۾� ����]  
* <v1.0 - 2023_1106_�ֿ���>
* 1- �ʱ� Ŭ���� ����
* 
* 2- �κ��丮 ���Ϳ�Ƽ�� ������ ���
* �� ���� Ŭ������ ���� �޼��尡 ���ǵǾ� ����.
* 
* 3- ���� ��ü �� Ŭ���� ���� ������ �ҿ����ϰ� �����Ǿ�����.
* 
* <v1.1 - 2023_1106_�ֿ���>
* 1-ItemPointerStatusWindow���� ����â ������ �����Ҵ����δ� ����� ������ �ʾ� 
* ���� ���� �� �����ϴ� ������ �̸� ��Ƴ����� static ���� ������ ����.
* 
* <v1.2 - 2023_1108_�ֿ���>
* 1- �κ��丮 iŰ�� ���� �ݴ� ��� ����
* 2- ��ũ��Ʈ ������ġ�� Inventory �ǳڿ��� GameController ������Ʈ�� �ű�
* 
* <v1.3 - 2023_1109_�ֿ���>
* 1- InventroyOnOffSwitch ö�� ����
* 
* <v2.0 - 2023_1120_�ֿ���> 
* 1- InventoryManagement�� ���� ����
* 
* a. ���� ���� �� �κ��丮�� ����(������Ʈ)�� �����ϰ�, ���� ����Ʈ(������Ʈ) �ּҸ� �����ϴ� ����. 
* (���� �κ��丮�� ���� �� ������Ʈ�� �����ϴ� ����). ���� ���� �޼��带 ���� (�κ��丮�� �� �� �� ���� ��ŭ ��������� �Ѵ�.)
* 
* b. �÷��̾��� ���� �κ��丮�� �����ϴ� ����.(������ PlayerInven ��ũ��Ʈ�� ������ �����ϰ� CraftManagerŬ������ ����) 
* ���� ���� �� �÷��̾� �κ��丮�� ���̺� �ε��Ͽ� �����ϰ� �ִ�. �� ��ȯ �ÿ��� ���� ����. 
* 
* c. �κ��丮 �� ����â�� ���� �巯���� ����. OnOff �޼��带 ����.
* 
* d. �ǹ�ư Ŭ������ ó���� ���� �޼��� ����
* 
* <v2.1 - 2023_1121_�ֿ���>
* 1- BtnItemCreateToInventory(������ ���� �׽�Ʈ ��ư) �޼��� ����
* Ŭ������ ������ ���� ����̹Ƿ� 
* 
* 2- list, list �ʱ�ȭ ���� ���� 
* �κ��丮 Ŭ���� ������ �ٲ�����Ƿ�
* 
* <v2.2 - 2023_1122_�ֿ���>
* 1- �� ���Ը���Ʈ ���� �� ����, ���Ը���Ʈ�� ���� ������ �����Ҵ� ���� ��  
* 
* <v3.0 - 2023_1215_�ֿ���>
* 1- �κ��丮�� �����ϴ� ĵ���� �������� ���� �κ��丮 �� ����â ��ġ, ��ư���� �������� ���� �� ���� ���� (�±׷� ĵ���� ����)
* 2- �ǹ�ư ��� ���� ���� ��
* 
* <v3.1 - 2023_1216_�ֿ���>
* 1- ���Ը���Ʈ�� ��ü��, ������ ���� �ִ� ���� ��ü�Ǹ� �����ϵ��� ����Ͽ���. (SlotListAllTr���� SlotListTr�� ����)
* 2- ���� �� Ŭ�� �� �󸮽�Ʈ�� �ű��, ���� ���� ������ŭ�� ������ ���̵��� �ϸ�, �ش� ���� �ε����� �����Ͽ� ��ġ�� �ű⵵�� ��� ���� �Ϸ�
* 3- ���� Ȱ��ȭ ���� ���� enum�� Ȱ���Ͽ� ����ϰ� �� ��ư Ŭ�� �� �ߺ� ������� �ʵ��� ����
* 
* <v4.0 - 2023-1217_�ֿ���>
* 1- statusWindow�� ����- ������ ����â�� �κ��丮�� ���������� �۵� �����ϱ� ������ 
* ����â ������ ������ �ִ� ItemPointerSatusWindow.cs ��ũ��Ʈ�� �ִ� ���� ���ٰ� �Ǵ�
* 
* 2- �κ��丮 ������Ʈ�� ĵ���� �׷� ������Ʈ�� ��� �� InventoryOnOffSwitch �޼��忡�� �̸� �����ϴ� ������� ����
* (������ �κ��丮 �̹����� �ؽ�Ʈ�� �Ź� ���� ã�Ƽ� �����ϴ� ��Ŀ��� ����)
* 
* <4.1 -2023_1221_�ֿ���>
* 1- ��ũ��Ʈ ������ġ GameController������Ʈ���� Canvas-Chracter�� ����
* 
* <v4.2 - 2023_1221_�ֿ���>
* 1- inventory�� �߸��� ���� ����
* PlayerInven������ invenInfo.GetComponent<Inventory>()�� �����ϰ� �־�����, invenInfo.inventory�� ����
* 
* <v4.3 - 2023_1226_�ֿ���>
* 1- Start���� Awake������ ����
* ������ �κ��丮�� ������ ���� �����Ǿ�� ������ ������Ʈ�� ���Կ� ���� �� �ֱ� ����
* (�κ��丮�� �ε��ϸ鼭 UpdateAllItemInfo()�޼��带 ȣ���ϱ� ����)
* 
* 2- InventoryOnOffSwitch private�޼���� ���� �� ������ �ּ�����
* 
* <v5.0 - 2023_1229_�ֿ���>
* 1- Ŭ������ ���ϸ� InventoryManagement���� InventoryInteractive�� ����
* 2- ��ũ��Ʈ ������ġ Canvas-Character���� Inventory������Ʈ�� ���濡 ���� inventory���� ����
* 3- ������ inventoryPanelTr�� inventoryTr�� ����
* 
* <v5.1 - 2023_1230_�ֿ���>
* 1- �����ϴ� ���� �������� ����. �κ��丮 ���ο� ������ �ϳ� �־�ΰ� �� ������Ʈ�� �����Ͽ� �����ϵ��� ����
* 
* 2- �κ��丮 ������ ������ŭ ���� �����ϴ� �ڵ带 InventoryInfo Ŭ������ �ű�.
* ���� Ŭ���������� �κ��丮�� ���ͷ�Ƽ�� ��ɸ� ����ϵ��� �ϱ� ���Ͽ�
* 
* 3- activeTab -> curActiveTab ������ ����
* 
* <v5.2 - 2023_1231_�ֿ���>
* 1- eTapType�� activeTab�� public ���� ��, activeTab ������ InventoryInfoŬ�������� �� �� �ֵ��� ����
*
*
* <v5.3 - 2024_0101_�ֿ���>
* 1- ���κ��� isActiveTabAll ������ �ΰ� IsActiveTabAll ������Ƽ�� �����ؼ� ��ü�� ��������, ������ �������� 
* ������ �ʿ��� Update Position�� �ε����� �� �� �ֵ��� �Ͽ���.
* 
* <v6.0 -2024_0102_�ֿ���
* 1- curActiveTab private������ �����Ͽ� ������ ����.
* 2- InventoryOnOffSwitch ������ -> SwitchInventoryAppear�� ����
* 3- InventoryCvGroup ������ -> InventoryCG�� ����
* 4- isInventoryOn staticŰ���� ����
* 5- eTapType ������ ���� -> ���� ������ ItemType���� ����, ��ü���� ItemType.None���� ����
* inventory�� �޼��� ȣ�⿡ ���ڷ� �ֱ� ���ϰ� �ϱ� ���Ͽ�.
* 6- AdjustSlotListNum �޼���� AdjustSlotCount�� ����
* 7- LocateDicItemToSlotList �޼���� MoveActiveItemToSlotList�� ����
* 8- MoveActiveItemToSlot�޼��� �߰�
* 9- ��� �޼��� Ȱ��ȭ�� �������� ����ȭ, BtnTapClick ������� ����ȭ �ۼ�
* 
* [��������]
* 1- LocateDicItemToSlotList�� �ʿ伺
* �������� inventory���� ������ �о���µ� �׷��ʿ���� ���� ���Կ� ������ �������� ������ �о���̸� ��.
* 
* 2- AdjustSlotListNum���� ���� ĭ�� ���Ѱ����� �о���µ� ������ ���꿡 �ɸ��� �ð��� �����Ƿ�
* �ʱ⿡ �����ؼ� ����ϰ� ������ ���� �� �̺�Ʈ�� ���� �� �ֵ��� �ؾ� �Ѵ�.
* 
* [��������_0102]
* 1- ActiveTab���� ������ ���Կ� �����ϴ�(x)->�κ��丮�� �����ϴ� ��� ���� �����ۿ��� �����ϰ� ��ġ������ ������Ʈ ���Ѿ���.
* (OnItemChanged�޼��� ȣ��)
* 
* 
* <v6.1 - 2024_0104_�ֿ���>
* 1- UpdateAllActiveTabInfo �޼��� ������ SetActiveTabInfo�ڵ带 UpdaeteActiveTabInfo�� ����
* ȣ�����ڸ� ���� �ִ� ���� �ƴ϶� ȣ���ڸ� ���� �������� ���� interactive�� ������Ŵ���μ� �ܺ�ȣ���� ���輺�� ����.
* 
* <v7.0 - 2024_0110_�ֿ���>
* 1- �޼��� InventoryQuit�� InventoryOpenSwitch�� ���� ��
* InventoryInfo ��ũ��Ʈ�� UpdateOpenState�� ȣ���Ͽ� ����â�� ���¸� �ݿ��ϵ��� ����
* ( �κ��丮 â ����ݴ� �޼���� InputSystem�� ������XŬ�� ���ʿ��� Ȱ�� ����)
* 
* 2- itemInfo�� UpdateActiveTabInfo�� �������� ���� ȣ���ڸ� �Ű������� �־ ȣ���ϴ� ������� ����.
* 
* 3- public ���������� ����, enum eTabType ����
* 4- ������ ���� ������ InfoŬ�������� �Űܿ� �� �޼���� �����Ͽ���, �� �޼��带 InfoŬ�������� ȣ���ϴ� �������� ����
* 
* 5- (InteractiveŬ������ ���� ����)
* �ʱ�ȭ�ϴµ� InventroyInfo Ŭ������ ���� �������� �߻��ϸ�, 
* ��� ����ϴ� ���� �ƴ϶� Ư�� �׼� �̺�Ʈ�� �߻� �� ����ϸ� �ǹǷ� InventoryInfo Ŭ������ �ʱ�ȭ�� �ñ�� ������ �����Ͽ���.
* (Awake������ ���� �ʱ�ȭ�� �����ϸ� ȣ������� ��Ȯ���� �ʱ� ������ 
* CreateInventorySlot�� ȣ���� �߻��� �� infoŬ���� ���� �������� �������� �ʴ� ������ �߻��Ͽ� �������� �� ���� ��������.)
* 
* 6- Awkae�� ���� �� �ڵ带 Initialize�޼��带 ����� �̵�. InfoŬ�������� ȣ�������� ����
* 
* 7- ���̺�Ʈ ��� �� ���ٽ� ���ο� i�� ���������� ���� ������ ��� ��ư�̺�Ʈ�� ������ ���������� ȣ��Ǵ� ���� �߰�
* (���ٽ� Ŭ���� Ư��) �����ϱ� ���� ���� for������ ���Ӱ� int������ �Ҵ��ؼ� i���� �޾Ƽ� �ִ� ���·� ����
* 
* <v7.1 - 2024_0111_�ֿ���>
* 1- �����̹� �帣�� �°� ������ Ŭ������ �����ϸ鼭 �ǰ��� ������ ��ü,����Ʈ������ �ǿ� �°� ����
* 
* <v8.0 - 2024_0112_�ֿ���>
* 1- �κ��丮 Ŭ���� ���� �������� ����
* MoveActiveItemToSlot���� dicLen�� dicLen-1������ �ϴ����� �����ϰ�, if������ dicType ����Ʈ ������ �˻繮�� �������.
* 
* 2- ���� ����ڰ� ���� ������ �� �ֵ��� ���ȭ 
* TabType�� �����ϰ�, �ش� �迭 ������ �ν����ͺ� �󿡼� ���� ������ �� �ֵ��� �Ͽ���.
* 
* 3- �ǹ�ư�� ����ڰ� ������ showTabType ���� ��ŭ �����Ҵ��ϰ�, �ش� ������ �̸��� �����ְ�, �̺�Ʈ�� ������ֵ��� ����
* 
* 4- ������ ItemType�� �°� ���� �Ǿ��ִ� �޼������ TabType�� �°� ����
* 
* 5- TabType�� ItemType���� ��ȯ �޼��带 �߰�, ���� ������ �߰�
* 
* 
* <v8.1 - 2024_0113_�ֿ���>
* 1- �κ��丮�� ���� ���¸� InventoryInfoŬ�������� ������ �ʿ伺�� ���� ���� �޼���� ������ �����Ͽ���.
* 
* <v8.2 - 2024_0114_�ֿ���>
* 1- �κ��丮 �ݱ� ��ư�� X��ư�� ������ ���� BtnInventoryClose�޼��带 ����
* �ٸ� �κ��丮�� ���� ���� ���� ���� ���¸� �����ϰ�, ���� ���°� �ƴҶ��� �ڽ��� â�� �ݴ� ������ ����
* 
* <V9.0 - 2024_0115_�ֿ���>
* 1- TapType ���� ������ �޼��带 InventoryŬ������ �����Ͽ�����, �޼���� static���� �ξ� ���ټ��� ������Ŵ.
* ������ InventoryŬ���� ���������ε� � ���� �������� Add����, SlotIndex�� ������� ���ΰ� �ʿ��ϱ� ����
* 
* 2- showTabType�� InteractiveŬ�������� �Է¹޾� �ʱ�ȭ �ϴ� ���� InitializerŬ������ �Űܼ� �ʱ�ȭ�ϵ��� ����
* ������ InventoryŬ�������� ������ � ���� �������� Add���� �˾ƾ� �ϱ� ������ �ʱ�ȭ���� �Ѱܹ��� �ʿ䰡 �ֱ� ����
* 
* 3- showTabType�� CreateActiveTabBtn�޼��忡�� Initilizer�κ��� ���� �о���̵��� ���� 
* 
* 4- ������ tabKindItemTypeList�� tabKindList�� ����
* 
* 5- ������ btnTabIdx�� btnTabType���� ����
* 
* 6- MoveActiveItemToSlot�޼����� �ڵ带 ��ü�ǰ� ���������� �����ϴ� ���� �ϳ��� �������� �ۼ�
* ConvertTabTypeToItemTypeList�޼��忡�� curActiveTab�� ���� ItemType�� List�� ����ֱ� ����
* 
* <v9.1 - 2024_0116_�ֿ���>
* 1- AdjustSlotCount�޼��忡�� �������� ���� ���� ��ųʸ� ���Ѽ��� �������� ���ϴ� �Ϳ���
* �ٷ� �ε����� ���� ���ϵ��� ����
* 
* 2- AdjustSlotCount�޼��� ���� ��ü ���� ���� ���� ���� �� 
* GetSlotCountLimitTab�޼��带 ����ϴ� ���� slotCoutLimitTab������ �ٷ� ����ϵ��� ����
* 
* 3- UpdateAllItemActiveTabInfo�޼��� ������ GetItemDicIgnoreExsistsȣ�� �Ű����� ����
* (ItemType)i ���� ���޿��� dicType[i]�� �����ϵ��� ����
* 
* 4- MoveSlotItemToEmptyList�޼���� MoveCurSlotItemToEmptyList�� �����ϰ�,
* ��ü ������ �������� �ݺ����� ���� �������� �̵���Ű�� �κ��� ���� ��Ƽ�� ���� ���� ���� ������ ����.
* ���� �޼��� ȣ�� ��ġ�� curActiveTab ������ �ٲ�� ������ �ٷ� ȣ�����ִ� ������ ����
* 
* 5- MoveActiveItemToSlot�޼���� MoveCurActiveItemToSlot���� ����
* (���� �������� ��ġ�ϴ� ��� �������� �̵���Ų�ٴ� �ǹ�)
* 
* 6- UpdateAllItemActiveTabInfo�޼��� ���� UpdateActiveTabInfo�� ȣ�����ڷ� curActiveTab �߰�
* 
* 7- BtnTapClick�޼��� isActiveTabAll ���ϴ� �κ��� if������ ���׿����ڷ� ����
* 
* 8- CurActiveTab������Ƽ�� �����Ͽ�, ItemInfo���� AddItem�� �̷���� �� �ֽ��������� ���������� �����ϵ��� �Ͽ���.
* 
* <v9.2- 2024_0116_�ֿ���>
* 1- UpdateAllItemActiveTabInfo�޼��� ���� GetItemDic���� null�˻繮 �߰�
* 
* <v9.3 - 2024_0124_�ֿ���>
* 1- InventoryŬ������ ��ųʸ� ���������� GameObject��ݿ��� ItemInfo�� �����ϸ鼭 ���� �޼��� ����
* (UpdateAllItemActiveTabInfo)
* 
*/






/// <summary>
/// �κ��丮 ���Ϳ�Ƽ�� ������ ����մϴ�. ������Ʈ�� �ٿ��� �ϸ�, ���ٸ� �ν��Ͻ� ���� ������ �ʿ����� �ʽ��ϴ�.<br/>
/// �κ��丮 �ݱ�, �� ���� �� ��ư Ŭ�� �� ������ ���ǵǾ� �ֽ��ϴ�<br/>
/// �� ��ũ��Ʈ�� �ܺο��� �پ��� ������ �� �� �ֽ��ϴ�.<br/>
/// ���� Canvas-Character�� �����Ǿ� �ֽ��ϴ�.<br/>
/// </summary>
public class InventoryInteractive : MonoBehaviour
{   
    Transform inventoryTr;          // �κ��丮 �ǳ��� �����Ͽ� ���� �״� �ϱ� ����.   
    InventoryInfo inventoryInfo;    // �÷��̾��� �κ��丮 ����
    Inventory inventory;            // �κ��丮 ��ũ��Ʈ ���� �κ��丮 ������

    Transform slotListTr;           // �κ��丮 ��ü�� ���Ը���Ʈ�� Ʈ������ ����
    Transform emptyListTr;          // �������� ������ �ʴ� ���� ��� �Űܵ� ������Ʈ(����Ʈ�� 1��° �ڽ�)

    Button[] btnTap;                // ��ư ���� ������ �� �� �ǿ� �´� �������� ǥ�õǵ��� �ϱ� ���� ����        
    GameObject slotPrefab;          // ������ �������� �����ϱ� ���� ������ ����
        
          
    TabType[] showTabType;    // Initializer�� �ν����ͺ信�� ������ Ȱ��ȭ ��ų ���� ����      
    TabType curActiveTab;       // ���� Ȱ��ȭ ���� ���� ���� (��ư Ŭ�� �� �ߺ� ������ �����ϱ� ����)
    bool isActiveTabAll;        // ���� Ȱ��ȭ ���� ���� ��ü ��������, ���� �������� ���θ� ��ȯ
    
    // �ǿ� �ش��ϴ� ������ Ÿ���� ���� �ӽ� ����Ʈ
    List<ItemType> tabKindList = new List<ItemType>();     



    /// <summary>
    /// �������� ���������� ���θ� ��ȯ�ϰų� �����մϴ�.<br/>
    /// �������� 1���� �������̶�� �ٸ� �������� ������ �� �����ϴ�.<br/>
    /// ItemDrag���� ������ �ް� �����մϴ�.
    /// </summary>
    public bool IsItemSelecting {get; set;} = false;


    /// <summary>
    /// ���� Ȱ��ȭ ���� ������ ��ȯ�մϴ�.
    /// </summary>
    public TabType CurActiveTab { get {  return curActiveTab; } }   

    /// <summary>
    /// ���� Ȱ��ȭ���� ��ü ��������, ���� �������� ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsActiveTabAll { get{return isActiveTabAll; } }






    /// <summary>
    /// �� ��ũ��Ʈ�� �����ϱ� ���ؼ� InventoryInfoŬ�������� ȣ������� �� �޼����Դϴ�.<br/>
    /// �ش� InventoryInfo���� Load�� �̷���� ���Ŀ� ���� ������ �������� �ʱ�ȭ�� �����մϴ�.<br/><br/>
    /// *** ȣ�� InventoryInfo�� InventoryInteractive�� ������ �ٸ��ٸ� ���ܰ� �߻��մϴ�. ***
    /// </summary>
    public void Initialize(InventoryInfo caller)
    {
        inventoryTr = transform;                                       // �κ��丮 �ǳ� ����  

        if(caller.transform != inventoryTr)
            throw new Exception("�ʱ�ȭ�� ������ �� ���� ȣ�����Դϴ�. Ȯ���Ͽ� �ּ���.");

        inventoryInfo = caller;                                        // Info Ŭ���� ���� ���
        inventory = inventoryInfo.inventory;                           // ���� �κ��丮 ���� ���� ���
        
        slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);  // ����Ʈ-����Ʈ-��ü ���Ը���Ʈ
        slotPrefab = slotListTr.GetChild(0).gameObject;                // ���� ����Ʈ ������ �̸� 1���� �߰��Ǿ� ����
        emptyListTr = inventoryTr.GetChild(0).GetChild(1);             // ����Ʈ-EmptyList
                                 
        CreateInventorySlot(caller);    // �κ��丮 ���� ����
        CreateActiveTabBtn();           // ��Ƽ���� ��ư ����
    }


    
    /// <summary>
    /// InventoryInfo���� slotCoutLimit������ ���� �޾Ƽ� ������ �����ϴ� �޼����Դϴ�.<br/>
    /// slotCountLimit������ �ֽ�ȭ�� ���¿��� ȣ���Ͽ��� �մϴ�.<br/>
    /// *** ���� �κ��丮�� InventoryInfo�� �ƴϸ� ȣ���� �� �����ϴ�. ***
    /// </summary>
    private void CreateInventorySlot(InventoryInfo caller)
    {                          
        if( caller != inventoryInfo )
            throw new Exception("ȣ���ڸ� Ȯ���Ͽ� �ּ���. ���� �κ��丮������ ȣ�� �� �� �ֽ��ϴ�.");
                      
        // ���� �κ��丮���� ������ ���� �����ϱ� ���� ��ü���� �ε����� ����մϴ�.
        int allTabIdx = inventory.GetTabIndex(ItemType.None);

        // �÷��̾� �κ��丮 ����(��ü �� ���� ĭ��)�� �����Ͽ� ������ �������� ����
        // ���� �κ��丮�� ������ �� �� ��������Ƿ� �ϳ��� ���ϰ� ����
        for( int i = 0; i<caller.inventory.slotCountLimitTab[allTabIdx]-1; i++ )
            Instantiate( slotPrefab, slotListTr );
    }







    /// <summary>
    /// ��ư ��Ƽ�� ���� �������� �����ϰ� �̺�Ʈ�� ����մϴ�.
    /// </summary>
    private void CreateActiveTabBtn()
    {        
        // ������ ���� ��Ÿ���� �̴ϼȶ����� Ŭ�����κ��� �о���Դϴ�.
        showTabType = inventoryTr.GetComponent<InventoryInitializer>().showTabType;

        // ����ڰ� ������ ��Ƽ���� ����
        int activeTabNum = showTabType.Length;

        // �̸� �����Ǿ��ִ� �ǹ�ư 1���� �����մϴ�.
        Button tabBtnPrefab = inventoryTr.GetChild(1).GetChild(0).GetComponent<Button>();
               
        // ��ư �迭�� ������ ������ �����մϴ�.
        btnTap = new Button[activeTabNum];         
               
        for(int i=0; i<activeTabNum; i++)
        {
            //���ٽ� Ŭ���� Ư������ ���� �״�ξ��� �ʰ� ���Ӱ� �Ҵ��մϴ�.
            TabType btnTabType = showTabType[i];   
                        
            // ��ư�� ���Ӱ� �����Ͽ� ���̷�Ű�� ��ġ�ϰ�, �̸� ���� �� �̺�Ʈ�� �����մϴ�.
            btnTap[i] = Instantiate(tabBtnPrefab.gameObject, tabBtnPrefab.transform.parent).GetComponent<Button>();
            btnTap[i].onClick.AddListener( () => BtnTapClick( btnTabType ) );
            btnTap[i].gameObject.name = GetTabObjectName(btnTabType);
            btnTap[i].GetComponentInChildren<Text>().text = GetTabTextName(btnTabType);
        }
        
        // ��� �ǹ�ư�� ������ �Ϸ��Ͽ��ٸ� ���� �� ��ư�� ��Ȱ��ȭ�մϴ�.
        tabBtnPrefab.gameObject.SetActive(false);
        
        
        // ���� �������� �ʾҴٸ� �׻� ��ü���� �������� ǥ�����ְ�,
        if( activeTabNum==0 )
        {
            isActiveTabAll = true;
            curActiveTab = TabType.All;
        }
        // ���� �����Ͽ��ٸ�, ù ���� �� �׻� ù��° ���� �������� Select�� ǥ�����ݴϴ�.
        else
        {            
            isActiveTabAll = (showTabType[0]==TabType.All) ? true : false;
            curActiveTab = showTabType[0];
            btnTap[0].Select();
        }

    }


    


    /// <summary>
    /// ���ڷ� ���� �� ������ ���� �� ������ �ؽ�Ʈ���� ����ϴ�.
    /// </summary>
    /// <returns>�ش� ���� �ؽ�Ʈ ��</returns>
    public string GetTabTextName(TabType tabType)
    {
        string name = "";

        if(tabType==TabType.All)
            name = "��ü";
        else if(tabType==TabType.Quest)
            name = "����Ʈ";
        else if(tabType == TabType.Misc)   
            name = "��ȭ";
        else if(tabType==TabType.Equip)
            name = "���";
        else
            throw new Exception("�� �̸��� �������� �ʾҽ��ϴ�.");

        return name;
    }

    /// <summary>
    /// ���ڷ� ���� �� ������ ������Ʈ�� �̸��� ����ϴ�.
    /// </summary>
    /// <returns>�ش� ���� ������Ʈ ��</returns>
    public string GetTabObjectName(TabType tabType)
    {
        string name = "";

        if(tabType==TabType.All)
            name = "ActiveTab-All";
        else if(tabType==TabType.Quest)
            name = "ActiveTab-Quest";
        else if(tabType==TabType.Misc)   
            name = "ActiveTab-Misc";
        else if(tabType==TabType.Equip)
            name = "ActiveTab-Weapon";
        else
            throw new Exception("�� ������Ʈ���� �������� �ʾҽ��ϴ�.");

        return name;
    }







    





















    /// <summary>
    /// X��ư�� ���� �κ��丮�� ���� �� ȣ�����ִ� �޼����Դϴ�.<br/>
    /// �ٸ� �κ��丮�� ���� ���¶�� ������ �����ϰ� ��� �κ��丮 â�� ������,<br/>
    /// ���� ���°� �ƴ϶�� �ڽ��� �κ��丮 â�� �ݽ��ϴ�.
    /// </summary>
    public void BtnInventoryClose()
    {
        if(inventoryInfo.IsConnect)
            inventoryInfo.DisconnectInventory();
        else
            inventoryInfo.InitOpenState(false);
    }












    /// <summary>
    /// ��ư Ŭ�� �� ���ǿ� �ش��ϴ� �����۸� �����ֱ� ���� �޼����Դϴ�. �� ���� ��ư Ŭ�� �̺�Ʈ�� ����ؾ� �մϴ�.
    /// </summary>
    public void BtnTapClick( TabType btnTabType )
    {        
        // ���� �̺�Ʈ�� �Ͼ ���ε����� ���� Ȱ��ȭ���� �ǰ� �����ϴٸ� ��������� �ʽ��ϴ�.
        if( curActiveTab==btnTabType )      
            return;
                
        MoveCurSlotItemToEmptyList();           // (�������� �ٲ������) ���� ���� ���Կ� �ִ� ��� �������� �󸮽�Ʈ�� �̵��մϴ�.

        // ���ڷ� ���� �ֽ� �������� ���� Ȱ��ȭ ������ �����մϴ�.
        curActiveTab = btnTabType;

        // �Ǽ��ÿ� ���� �� Ȱ��ȭ ������ ��ü������, ���������� �����մϴ�.
        isActiveTabAll = (btnTabType==TabType.All)? true : false;

        UpdateAllItemActiveTabInfo();           // �κ��丮�� ��� �����ۿ� Ȱ��ȭ �� ������ �ݿ��մϴ�
        AdjustSlotCount();                      // Ȱ��ȭ ���� ������ŭ ������ �ø��ų� ���Դϴ�
        MoveCurActiveItemToSlot();              // Ȱ��ȭ �� ������ ������ ��ųʸ��� ��� �������� ���Կ� ��ġ�մϴ�       
    }



    /// <summary>
    /// �κ��丮�� ���� �����ϰ� �ִ� ��� ������ �����ۿ� Ȱ��ȭ�� ������ �ݿ��մϴ�.<br/>
    /// </summary>
    private void UpdateAllItemActiveTabInfo()
    {
        // �ϳ��� �����ͼ� ���� �� ��ųʸ� ���� ����
        Dictionary<string, List<ItemInfo>> itemDic;

        for( int i = 0; i<inventory.dicLen; i++ )
        {
            itemDic=inventory.GetItemDic( inventory.dicType[i] );          // CurDicLen ������ŭ ��ųʸ��� �ϳ��� �����մϴ�.

            // �����ۻ����� �������� �ʰų� ���� ����Ʈ�� �������� �ʴ´ٸ� ���� ������ ã���ϴ�
            if(itemDic == null || itemDic.Count==0)
                continue;

            foreach( List<ItemInfo> itemInfoList in itemDic.Values )      // �ش� ��ųʸ��� ItemInfo����Ʈ�� �����ɴϴ�.
            {
                foreach( ItemInfo itemInfo in itemInfoList )               // ItemInfo����Ʈ���� ItemInfo�� �ϳ��� �����ɴϴ�.
                    itemInfo.UpdateActiveTabInfo(this, curActiveTab, isActiveTabAll);   // �������� Ȱ��ȭ �� ������ �ֽ�ȭ�մϴ�.
            }
        }
    }

    /// <summary>
    /// �� ���� Ŭ������ �� �ش� ���� ĭ ����ŭ ������ ������ ���̰ų� �÷��ݴϴ�
    /// </summary>
    private void AdjustSlotCount()
    {
        // ���� �ǿ� �ش��ϴ� ���� ����
        int curTabSlotCount = inventory.slotCountLimitTab[(int)curActiveTab];

        // ��ü �ǿ� �ش��ϴ� ���� ����
        int allTabSlotCount = inventory.slotCountLimitTab[(int)TabType.All];

        // ���� ���� ������ŭ ���ݴϴ�
        for(int i=0; i<curTabSlotCount; i++)                   
            slotListTr.GetChild(i).gameObject.SetActive(true);   

        // �������� ���ݴϴ� 
        for(int i=curTabSlotCount; i<allTabSlotCount; i++)
            slotListTr.GetChild(i).gameObject.SetActive(false);  
    }




    /// <summary>
    /// ���� ���� ����Ʈ�� �ִ� ������Ʈ�� �Ͻ������� �� ����Ʈ�� �Ű��ִ� �޼����Դϴ�
    /// </summary>
    private void MoveCurSlotItemToEmptyList()
    {
        
        for( int i = 0; i<inventory.slotCountLimitTab[(int)curActiveTab]; i++ )     // ���� ���Ը���Ʈ�� ��� ���� ��ȸ
        {
            if( slotListTr.GetChild(i).childCount!=0 )          // i��° ���Կ� �������� ����ִٸ�
            {
                // ���Կ� ����ִ� ������ ������Ʈ�� ��� ��� ������Ʈ emptyList�� �̵��ϰ�, ��ġ�� �����մϴ�
                slotListTr.GetChild(i).GetChild(0).SetParent(emptyListTr, false);
            }
        }
    }


    /// <summary>
    /// ���� Ȱ��ȭ ���� �� ������ ������ ��ųʸ��� ��� �������� ���Կ� ��ġ�մϴ�<br/>
    /// ���������� InventoryInfo Ŭ������ ���� ��ųʸ��� �����ϴ� ��� �������� ��ġ ������ ������Ʈ�ϴ� �޼��带 Ȱ���մϴ�
    /// </summary>
    private void MoveCurActiveItemToSlot()
    {
        // �������� �ش��ϴ� ������ ������ ���̸� ���մϴ�.
        int tabKindLen = Inventory.ConvertTabTypeToItemTypeList(ref tabKindList, curActiveTab);

        // �������� �ش��ϴ� ��� �������� �������� ������Ʈ �մϴ�.
        for(int i=0; i<tabKindLen; i++)
            inventoryInfo.UpdateDicItemPosition( tabKindList[i] );  
    }

    





}
