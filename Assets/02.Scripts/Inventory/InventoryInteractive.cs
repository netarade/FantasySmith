using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InventoryManagement;
using ItemData;
using System.Runtime.CompilerServices;

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
* 
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
    
    Transform inventoryTr;                      // �κ��丮 �ǳ��� �����Ͽ� ���� �״� �ϱ� ����.   
    public Transform slotListTr;                // �κ��丮 ��ü�� ���Ը���Ʈ�� Ʈ������ ����
    public Transform emptyListTr;               // �������� ������ �ʴ� ���� ��� �Űܵ� ������Ʈ(����Ʈ�� 1��° �ڽ�)

    InventoryInfo inventoryInfo;                // �÷��̾��� �κ��丮 ����
    Inventory inventory;                        // �κ��丮 ��ũ��Ʈ ���� �κ��丮 ������

    Button[] btnTap;                            // ��ư ���� ������ �� �� �ǿ� �´� �������� ǥ�õǵ��� �ϱ� ���� ����

    bool isInventoryOn;                         // On���� ����ϱ� ���� ����
    CanvasGroup inventoryCG;                    // �κ��丮�� ĵ���� �׷�

    public enum eTapType { All, Weap, Misc }    // � ������ �������� �����ִ� ������
    ItemType curActiveTab;                      // ���� Ȱ��ȭ ���� ���� ���� (��ư Ŭ�� �� �ߺ� ������ �����ϱ� ����)
    bool isActiveTabAll;                        // ���� Ȱ��ȭ ���� ���� ��ü ��������, ���� �������� ���θ� ��ȯ

    



    /// <summary>
    /// ���� Ȱ��ȭ���� ��ü ��������, ���� �������� ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsActiveTabAll { get{return isActiveTabAll; } }


    /// <summary>
    /// ���� �κ��丮 ������Ʈ�� ���� �ִ��� ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsInventoryOn { get { return isInventoryOn;} } 



    void Awake()
    {
        inventoryTr = transform;                                       // �κ��丮 �ǳ� ����
        slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);  // ����Ʈ-����Ʈ-��ü ���Ը���Ʈ
        emptyListTr = inventoryTr.GetChild(0).GetChild(1);             // ����Ʈ-EmptyList
                                                                       
        inventoryInfo = inventoryTr.GetComponent<InventoryInfo>();     // �κ��丮 ������ �����մϴ�
        inventory = inventoryInfo.inventory;            


        btnTap = new Button[3]; //��ư �迭�� ���� ����

        // �� ��ư ������Ʈ ���� �� ��ư�� �̺�Ʈ�� ����մϴ�.(int�� ���ڸ� �ٸ��� �༭ ����մϴ�)
        for( int i = 0; i<3; i++ )
        {
            btnTap[i]=inventoryTr.GetChild( 1 ).GetChild( i ).GetComponent<Button>(); //�κ��丮-Buttons������ ����
            btnTap[i].onClick.AddListener( () => BtnTapClick( i ) );
        }

        btnTap[0].Select();            // ù ���� �� �׻� ��ü���� Select�� ǥ�����ݴϴ�.
        curActiveTab = ItemType.None;  // Ȱ��ȭ���� ���� ��ü������ ����
        isActiveTabAll = true;         // ��ü�� ���� ���º��� ����
        
        // �κ��丮�� ��� �̹����� �ؽ�Ʈ ����, ĵ�����׷� ����
        inventoryCG = inventoryTr.GetComponent<CanvasGroup>();

        // ���� ���� �� �κ��丮 �ǳ��� ���д�.
        SwitchInventoryAppear(false);
        isInventoryOn = false;          //�ʱ⿡ �κ��丮�� ���� ����
    }


    /// <summary>
    /// �κ��丮 ������ Ŭ�� �� OnOff ����ġ
    /// </summary>
    public void InventoryQuit()
    {
        SwitchInventoryAppear( !isInventoryOn );    // ȣ�� �� ���� �ݴ� ���·� �־��ݴϴ�
        isInventoryOn = !isInventoryOn;             // ���� ��ȭ�� �ݴ�� ����մϴ�
    }


    /// <summary>
    /// �κ��丮 Ű ���� IŰ�� ������ �κ��丮�� �����մϴ� (���߿� InputSystem������� �����ؾ� �մϴ�)
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            SwitchInventoryAppear( !isInventoryOn );    // ȣ�� �� ���� �ݴ� ���·� �־��ݴϴ�
            isInventoryOn = !isInventoryOn;             // ���� ��ȭ�� �ݴ�� ����մϴ�
        }        
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
            if(curActiveTab==ItemType.None)     // ���� Ȱ��ȭ ���� ���� ��ü ���̶�� ��������� �ʴ´�.
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
            itemDic=inventory.GetItemDicIgnoreExsists( (ItemType)i );   // CurDicLen ������ŭ ��ųʸ��� �ϳ��� �����մϴ�.

            foreach( List<GameObject> itemObjList in itemDic.Values )   // �ش� ��ųʸ��� ������Ʈ����Ʈ�� �����ɴϴ�.
            {
                foreach( GameObject itemObj in itemObjList )            // ������Ʈ����Ʈ���� ������Ʈ�� �ϳ��� �����ɴϴ�.
                {
                    ItemInfo itemInfo = itemObj.GetComponent<ItemInfo>();  // ������ ������ �о���Դϴ�.

                    itemInfo.UpdateActiveTabInfo();                     // Ȱ��ȭ �� ������ ���� Ȱ��ȭ�� �������� �����մϴ�.
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
            for(int i=0; i<inventory.CurDicLen; i++)
                inventoryInfo.UpdateDicItemPosition( (ItemType)i );  
        }
        // ���� Ȱ��ȭ ���� ���� ���� ��� ���� ��ųʸ��� ������� ȣ��
        else
            inventoryInfo.UpdateDicItemPosition(curActiveTab);
    }

    








}
