using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InventoryManagement;
using ItemData;
using System.Runtime.CompilerServices;
using System;
using Unity.VisualScripting;
using UnityEngine.Events;

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
* 
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

    bool isInventoryOn;             // On���� ����ϱ� ���� ����
    CanvasGroup inventoryCG;        // �κ��丮�� ĵ���� �׷�

    ItemType curActiveTab;          // ���� Ȱ��ȭ ���� ���� ���� (��ư Ŭ�� �� �ߺ� ������ �����ϱ� ����)
    bool isActiveTabAll;            // ���� Ȱ��ȭ ���� ���� ��ü ��������, ���� �������� ���θ� ��ȯ


    
    GameObject slotPrefab;          // ������ �������� �����ϱ� ���� ������ ����







    /// <summary>
    /// �������� ���������� ���θ� ��ȯ�ϰų� �����մϴ�.<br/>
    /// �������� 1���� �������̶�� �ٸ� �������� ������ �� �����ϴ�.<br/>
    /// ItemDrag���� ������ �ް� �����մϴ�.
    /// </summary>
    public bool IsItemSelecting {get; set;} = false;

    /// <summary>
    /// ���� Ȱ��ȭ���� ��ü ��������, ���� �������� ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsActiveTabAll { get{return isActiveTabAll; } }


    /// <summary>
    /// ���� �κ��丮 ������Ʈ�� ���� �ִ��� ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsInventoryOn { get { return isInventoryOn;} } 



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
        
        inventoryCG = inventoryTr.GetComponent<CanvasGroup>();         // �κ��丮�� ĵ�����׷� ����
        slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);  // ����Ʈ-����Ʈ-��ü ���Ը���Ʈ
        slotPrefab = slotListTr.GetChild(0).gameObject;                // ���� ����Ʈ ������ �̸� 1���� �߰��Ǿ� ����
        emptyListTr = inventoryTr.GetChild(0).GetChild(1);             // ����Ʈ-EmptyList
                                                                       
        CreateActiveTabBtn();   // ��Ƽ���� ��ư ����
        InitOpenState();        // �κ��丮 ���� ���� �ʱ�ȭ
    }


    /// <summary>
    /// ��ư ��Ƽ�� ���� �������� �����ϰ� �̺�Ʈ�� ����մϴ�.
    /// </summary>
    private void CreateActiveTabBtn()
    {        
        // ��Ƽ���� ���� - ���� inventory�� dicNum+1�� ��������
        int activeTabNum = 3;

        // ��ư �迭�� ������ ������ �����մϴ�.
        btnTap = new Button[activeTabNum]; 

        // �̸� �����Ǿ��ִ� ��ü���� �������� ���� �� �� �̺�Ʈ�� �����մϴ�.
        btnTap[0] = inventoryTr.GetChild(1).GetChild(0).GetComponent<Button>();
        btnTap[0].onClick.AddListener( () => BtnTapClick( 0 ) );
        

        // ������ ���� ��ü���� �������� �����Ҵ��Ͽ� �̺�Ʈ�� �����մϴ�.
        for(int i=1; i<activeTabNum; i++)
        {
            int idx = i;
            btnTap[i] = Instantiate(btnTap[0].gameObject, btnTap[0].transform.parent).GetComponent<Button>();
            btnTap[i].onClick.AddListener( () => BtnTapClick( idx ) );
        }
        
        // ��ư �̸��� �ؽ�Ʈ�� �����մϴ�. (���� �κ��丮 Ŭ���� ���� ���� �� ��������) 
        btnTap[1].gameObject.name = "ActiveTab-Weap";
        btnTap[1].GetComponentInChildren<Text>().text = "����";
        
        btnTap[2].gameObject.name = "ActiveTab-Misc";
        btnTap[2].GetComponentInChildren<Text>().text = "��ȭ";
                
        


        btnTap[0].Select();            // ù ���� �� �׻� ��ü���� Select�� ǥ�����ݴϴ�.
        curActiveTab = ItemType.None;  // Ȱ��ȭ���� ���� ��ü������ ����
        isActiveTabAll = true;         // ��ü�� ���� ���º��� ����
    }


    /// <summary>
    /// �κ��丮 â�� ���� �ʱ�ȭ�� �����մϴ�.
    /// </summary>
    private void InitOpenState()
    {
        // ���� ���� �� �κ��丮 �ǳ��� ���д�.
        SwitchInventoryAppear(false);
        isInventoryOn = false;          //�ʱ⿡ �κ��丮�� ���� ����        
    }







    /// <summary>
    /// InventoryInfo���� slotCoutLimit������ ���� �޾Ƽ� ������ �����ϴ� �޼����Դϴ�.<br/>
    /// slotCountLimit������ �ֽ�ȭ�� ���¿��� ȣ���Ͽ��� �մϴ�.<br/>
    /// *** ���� �κ��丮�� InventoryInfo�� �ƴϸ� ȣ���� �� �����ϴ�. ***
    /// </summary>
    public void CreateInventorySlot(InventoryInfo caller)
    {                          
        if( caller != inventoryInfo )
            throw new Exception("ȣ���ڸ� Ȯ���Ͽ� �ּ���. ���� �κ��丮������ ȣ�� �� �� �ֽ��ϴ�.");
                      

        // �÷��̾� �κ��丮 ����(��ü �� ���� ĭ��)�� �����Ͽ� ������ �������� ����
        // ���� �κ��丮�� ������ �� �� ��������Ƿ� �ϳ��� ���ϰ� ����
        for( int i = 0; i<caller.inventory.SlotCountLimitAll-1; i++ )
            Instantiate( slotPrefab, slotListTr );
    }










    /// <summary>
    /// �κ��丮 â�� ����ݴ� �޼����Դϴ�.<br/>
    /// �÷��̾� InputSystem������ IŰ�� �����ų� �κ��丮 ������ x������ Ŭ�� �� ȣ��˴ϴ�.<br/>
    /// </summary>
    public void InventoryOpenSwitch()
    {
        SwitchInventoryAppear( !isInventoryOn );    // ȣ�� �� ���� �ݴ� ���·� �־��ݴϴ�
        isInventoryOn = !isInventoryOn;             // ���� ��ȭ�� �ݴ�� ����մϴ�

        inventoryInfo.UpdateOpenState(this, isInventoryOn);     // ���¸� InfoŬ������ �ݿ��մϴ�.
    }



    /// <summary>
    /// �κ��丮�� ��� �̹����� �ؽ�Ʈ�� ���ݴϴ�.
    /// </summary>
    private void SwitchInventoryAppear(bool onOffState )
    {
        inventoryCG.blocksRaycasts = onOffState;   // �׷��� ��� ����ĳ��Ʈ�� �������ݴϴ�
        inventoryCG.alpha = onOffState ? 1f : 0f;  // �׷��� ������ �������ݴϴ�
    }



    /// <summary>
    /// ��ư Ŭ�� �� ���ǿ� �ش��ϴ� �����۸� �����ֱ� ���� �޼����Դϴ�. �� ���� ��ư Ŭ�� �̺�Ʈ�� ����ؾ� �մϴ�.
    /// </summary>
    /// <param name="btnIdx"></param>
    public void BtnTapClick( int btnIdx )
    {
        if(btnIdx==0)       // All ��
        {
            if( curActiveTab==ItemType.None )     // ���� Ȱ��ȭ ���� ���� ��ü ���̶�� ��������� �ʴ´�.
                return;
            
            curActiveTab = ItemType.None;       // Ȱ��ȭ ���� �� ���� 
            isActiveTabAll = true;              // Ȱ��ȭ ���� ��ü�� ����
        }
        else if(btnIdx==1)  // ���� �ǹ�ư Ŭ��
        {
            if(curActiveTab==ItemType.Weapon)   // ���� Ȱ��ȭ ���� ���� ���� ���̶�� ��������� �ʴ´�.
                return;

            curActiveTab = ItemType.Weapon;     // Ȱ��ȭ ���� �� ����
            isActiveTabAll = false;             // Ȱ��ȭ ���� ��ü�� ����
        }
        else if(btnIdx==2)  // ��ȭ �ǹ�ư Ŭ��
        {
            if(curActiveTab==ItemType.Misc)     // ���� Ȱ��ȭ ���� ���� ��ȭ ���̶�� ��������� �ʴ´�.
                return;

            curActiveTab = ItemType.Misc;       // Ȱ��ȭ ���� �� ����
            isActiveTabAll = false;             // Ȱ��ȭ ���� ��ü�� ����
        }

        UpdateAllItemActiveTabInfo();           // �κ��丮�� ��� �����ۿ� Ȱ��ȭ �� ������ �ݿ��մϴ�
        MoveSlotItemToEmptyList();              // ���� ���Կ� �ִ� ��� �������� �󸮽�Ʈ�� �̵��մϴ�
        AdjustSlotCount();                      // Ȱ��ȭ ���� ������ŭ ������ �ø��ų� ���Դϴ�
        MoveActiveItemToSlot();                 // Ȱ��ȭ �� ������ ������ ��ųʸ��� ��� �������� ���Կ� ��ġ�մϴ�       
    }



    /// <summary>
    /// ��� �����ۿ� Ȱ��ȭ�� ������ �ݿ��մϴ�.<br/>
    /// </summary>
    private void UpdateAllItemActiveTabInfo()
    {
        //// �ϳ��� �����ͼ� ���� �� ��ųʸ� ���� ����
        Dictionary<string, List<GameObject>> itemDic;

        for( int i = 0; i<inventory.CurDicLen; i++ )
        {
            itemDic=inventory.GetItemDicIgnoreExsists( (ItemType)i );      // CurDicLen ������ŭ ��ųʸ��� �ϳ��� �����մϴ�.

            foreach( List<GameObject> itemObjList in itemDic.Values )      // �ش� ��ųʸ��� ������Ʈ����Ʈ�� �����ɴϴ�.
            {
                foreach( GameObject itemObj in itemObjList )               // ������Ʈ����Ʈ���� ������Ʈ�� �ϳ��� �����ɴϴ�.
                {
                    ItemInfo itemInfo = itemObj.GetComponent<ItemInfo>();  // ������ ������ �о���Դϴ�.

                    itemInfo.UpdateActiveTabInfo(this, isActiveTabAll);    // Ȱ��ȭ �� ������ ���� Ȱ��ȭ�� �������� �����մϴ�.
                }

            }
        }
    }

    /// <summary>
    /// �� ���� Ŭ������ �� �ش� ���� ĭ ����ŭ ������ ������ ���̰ų� �÷��ݴϴ�
    /// </summary>
    private void AdjustSlotCount()
    {
        int curSlotCount = inventory.GetItemSlotCountLimit(curActiveTab);
        int allSlotCount = inventory.GetItemSlotCountLimit(ItemType.None);
                
        for(int i=0; i<curSlotCount; i++)                   
            slotListTr.GetChild(i).gameObject.SetActive(true);   // ���� ���� ������ŭ ���ݴϴ�

        for(int i=curSlotCount; i<allSlotCount; i++)
            slotListTr.GetChild(i).gameObject.SetActive(false);  // �������� ���ݴϴ� 
    }


    /// <summary>
    /// ���� ���� ����Ʈ�� �ִ� ������Ʈ�� �Ͻ������� �� ����Ʈ�� �Ű��ִ� �޼����Դϴ�
    /// </summary>
    private void MoveSlotItemToEmptyList()
    {
        for( int i = 0; i<inventory.SlotCountLimitAll; i++ )    // ��ü ���Ը���Ʈ�� ��� ���� ��ȸ
        {
            if( slotListTr.GetChild(i).childCount!=0 )          // i��° ���Կ� �������� ����ִٸ�
            {
                // ���Կ� ����ִ� ������ ������Ʈ�� ��� ��� ������Ʈ emptyList�� �̵��ϰ�, ��ġ�� �����մϴ�
                slotListTr.GetChild(i).GetChild(0).SetParent( emptyListTr, false );
            }
        }
    }


    /// <summary>
    /// ���� Ȱ��ȭ ���� �� ������ ������ ��ųʸ��� ��� �������� ���Կ� ��ġ�մϴ�<br/>
    /// ���������� InventoryInfo Ŭ������ ���� ��ųʸ��� �����ϴ� ��� �������� ��ġ ������ ������Ʈ�ϴ� �޼��带 Ȱ���մϴ�
    /// </summary>
    private void MoveActiveItemToSlot()
    {
        // ���� Ȱ��ȭ ���� ��ü ���� ��� ��� ��ųʸ��� ������� ȣ��
        if( isActiveTabAll )
        {
            print(inventory.CurDicLen);
            for( int i = 0; i<inventory.CurDicLen; i++ )
                inventoryInfo.UpdateDicItemPosition( (ItemType)i );
        }
        // ���� Ȱ��ȭ ���� ���� ���� ��� ���� ��ųʸ��� ������� ȣ��
        else
            inventoryInfo.UpdateDicItemPosition( curActiveTab );
    }

    








}
