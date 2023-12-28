using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InventoryManagement;

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
* [���� ��������]
* 1- �κ��丮 ���� ���� ������ InventoryInfo Ŭ���������ϵ��� �Űܾ���.
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
    [SerializeField] Button[] btnTap;           // ��ư ���� ������ �� �� �ǿ� �´� �������� ǥ�õǵ��� �ϱ� ���� ����

    public Transform inventoryTr;               // �κ��丮 �ǳ��� �����Ͽ� ���� �״� �ϱ� ����.
   
    [SerializeField] GameObject slotPrefab;     // ������ �������� �����ϱ� ���� ������ ����
    public Transform slotListTr;                // �κ��丮 ��ü�� ���Ը���Ʈ�� Ʈ������ ����

    public static bool isInventoryOn;           // On���� ����ϱ� ���� ����
    CanvasGroup inventoryCvGroup;               // �κ��丮�� ĵ���� �׷�

    Inventory inventory;                        // �÷��̾��� �κ��丮 ����
    public Transform emptyListTr;               // �������� ������ �ʴ� ���� ��� �Űܵ� ������Ʈ(����Ʈ�� 1��° �ڽ�)

    enum eTapType { All, Weap, Misc }           // � ������ �������� �����ִ� ������
    eTapType activeTab;                         // ���� Ȱ��ȭ ���� ���� ���� (��ư Ŭ�� �� �ߺ� ������ �����ϱ� ����)



    void Awake()
    {
        inventoryTr = transform;                                       // �κ��丮 �ǳ� ����
        slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);  // ����Ʈ-����Ʈ-��ü ���Ը���Ʈ
        emptyListTr = inventoryTr.GetChild(0).GetChild(1);             // ����Ʈ-EmptyList

        btnTap = new Button[3]; //��ư �迭�� ���� ����

        // �� ��ư ������Ʈ ���� �� ��ư�� �̺�Ʈ�� ����մϴ�.(int�� ���ڸ� �ٸ��� �༭ ����մϴ�)
        for( int i = 0; i<3; i++ )
        {
            btnTap[i]=inventoryTr.GetChild( 1 ).GetChild( i ).GetComponent<Button>(); //�κ��丮-Buttons������ ����
            btnTap[i].onClick.AddListener( () => BtnTapClick( i ) );
        }

        btnTap[0].Select();         // ù ���� �� �׻� ��ü���� Select�� ǥ�����ݴϴ�.
        activeTab = eTapType.All;   // Ȱ��ȭ���� ���� ��ü������ ����
        
        // �κ��丮�� ��� �̹����� �ؽ�Ʈ ����, ĵ�����׷� ����
        inventoryCvGroup = inventoryTr.GetComponent<CanvasGroup>();

        // ���� ���� �� �κ��丮 �ǳ��� ���д�.
        InventoryOnOffSwitch(false);
        isInventoryOn = false;          //�ʱ⿡ �κ��丮�� ���� ����


        // ����ũ��Ʈ�� �����Ǿ����ִ� �κ��丮 ������Ʈ�� �����Ͽ� �κ��丮 ������ �޾ƿɴϴ�
        InventoryInfo invenInfo = this.gameObject.GetComponent<InventoryInfo>();
        inventory = invenInfo.inventory;


        // �÷��̾� �κ��丮 ����(��ü �� ���� ĭ��)�� �����Ͽ� ������ �������� ����
        for( int i = 0; i<invenInfo.inventory.AllCountLimit; i++ )
            Instantiate( slotPrefab, slotListTr );
    }



    /// <summary>
    /// �κ��丮 ������ Ŭ�� �� OnOff ����ġ
    /// </summary>
    public void InventoryQuit()
    {
        InventoryOnOffSwitch( !isInventoryOn );     // �ݴ� ���·� �־��ش�.
        isInventoryOn = !isInventoryOn;             // ���� ��ȭ�� ����Ѵ�.
    }


    /// <summary>
    /// �κ��丮 Ű ���� IŰ�� ������ �κ��丮�� �����մϴ� (���߿� InputSystem������� �����ؾ� �մϴ�)
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            InventoryOnOffSwitch( !isInventoryOn );     // �ݴ� ���·� �־��ش�.
            isInventoryOn = !isInventoryOn;             // ���� ��ȭ�� ����Ѵ�.
        }        
    }


    /// <summary>
    /// �κ��丮�� ��� �̹����� �ؽ�Ʈ�� ���ݴϴ�.
    /// </summary>
    private void InventoryOnOffSwitch(bool onOffState )
    {
        inventoryCvGroup.blocksRaycasts = onOffState;   // �׷��� ��� ����ĳ��Ʈ�� �������ݴϴ�
        inventoryCvGroup.alpha = onOffState ? 1f : 0f;  // �׷��� ������ �������ݴϴ�
    }



    /// <summary>
    /// ��ư Ŭ�� �� ���ǿ� �ش��ϴ� �����۸� �����ֱ� ���� �޼����Դϴ�. �� ���� ��ư Ŭ�� �̺�Ʈ�� ����ؾ� �մϴ�.
    /// </summary>
    /// <param name="btnIdx"></param>
    public void BtnTapClick( int btnIdx )
    {
        
        if(btnIdx==0)       // All ��
        {   
            if(activeTab==eTapType.All)                         // ���� Ȱ��ȭ ���� ���� ��ü ���̶�� ��������� �ʴ´�.
                return;
            
            activeTab = eTapType.All;                           // Ȱ��ȭ ���� �� ���� 
            MoveSlotItemToEmptyList();                          // �󸮽�Ʈ�� ��� ������ �̵�
            AdjustSlotListNum(eTapType.All);                    // ��ü ���� ������ŭ ������ �ø��ų� ����
            LocateDicItemToSlotList(inventory.weapDic, true);   // ��ü�� �ε����� �������� ��� ������ ��ġ
            LocateDicItemToSlotList(inventory.miscDic, true);   
        }
        else if(btnIdx==1)  // ���� �ǹ�ư Ŭ��
        {
            if(activeTab==eTapType.Weap)                        // ���� Ȱ��ȭ ���� ���� ���� ���̶�� ��������� �ʴ´�.
                return;
            
            activeTab = eTapType.Weap;                          // Ȱ��ȭ ���� �� ����
            MoveSlotItemToEmptyList();                          // �󸮽�Ʈ�� ��� ������ �̵�
            AdjustSlotListNum(eTapType.Weap);                   // ���� ���� ������ŭ ������ �ø��ų� ����
            LocateDicItemToSlotList(inventory.weapDic, false);  // ���� �� �ε����� �������� ��� ������ ��ġ
        }
        else if(btnIdx==2)  // ��ȭ �ǹ�ư Ŭ��
        {
            if(activeTab==eTapType.Misc)                         // ���� Ȱ��ȭ ���� ���� ��ȭ ���̶�� ��������� �ʴ´�.
                return;
            
            activeTab = eTapType.Misc;                          // Ȱ��ȭ ���� �� ����
            MoveSlotItemToEmptyList();                          // �󸮽�Ʈ�� ��� ������ �̵�
            AdjustSlotListNum(eTapType.Misc);                   // ��ȭ ���� ������ŭ ������ �ø��ų� ����
            LocateDicItemToSlotList(inventory.miscDic, false);  // ������ �ε����� �������� ��� ������ ��ġ
        }

    }


    /// <summary>
    /// �� ���� Ŭ������ �� �ش� ���� ĭ ����ŭ ������ ������ ���̰ų� �÷��ݴϴ�
    /// </summary>
    private void AdjustSlotListNum( eTapType tapType )
    {
        switch(tapType)
        {
            case eTapType.All:
                for(int i=0; i<inventory.AllCountLimit; i++)
                    slotListTr.GetChild(i).gameObject.SetActive(true);   // ��ü ���� ������ŭ ���ݴϴ�
                break;

            case eTapType.Weap:
                for(int i=0; i<inventory.WeapCountLimit; i++)                   
                    slotListTr.GetChild(i).gameObject.SetActive(true);   // ���� ���� ������ŭ ���ݴϴ�

                for(int i=inventory.WeapCountLimit; i<inventory.AllCountLimit; i++)
                    slotListTr.GetChild(i).gameObject.SetActive(false);  // �������� ���ݴϴ�    
                break;

            case eTapType.Misc:
                for(int i=0; i<inventory.MiscCountLimit; i++)
                    slotListTr.GetChild(i).gameObject.SetActive(true);   // ��ȭ ���� ������ŭ ���ݴϴ�       
                
                for(int i=inventory.MiscCountLimit; i<inventory.AllCountLimit; i++)
                    slotListTr.GetChild(i).gameObject.SetActive(false);  // �������� ���ݴϴ�
                break;
        }        
    }


    /// <summary>
    /// ���� ���� ����Ʈ�� �ִ� ������Ʈ�� �Ͻ������� �� ����Ʈ�� �Ű��ִ� �޼����Դϴ�
    /// </summary>
    private void MoveSlotItemToEmptyList()
    {
        for( int i = 0; i<inventory.AllCountLimit; i++ )        // ��ü ���Ը���Ʈ�� ��� ���� ��ȸ
        {
            if( slotListTr.GetChild(i).childCount!=0 )       // i��° ���Կ� �������� ����ִٸ�
            {
                // ���Կ� ����ִ� ������ ������Ʈ�� ��� ��� ������Ʈ emptyList�� �̵��ϰ�, ��ġ�� ����
                slotListTr.GetChild(i).GetChild(0).SetParent( emptyListTr, false );
                slotListTr.GetChild(i).GetChild(0).localPosition = Vector3.zero;
            }
        }
    }

    /// <summary>
    /// ��ųʸ��� ��� �������� �ε��� ������ �������� ���Ը���Ʈ�� ��ġ���ִ� �޼����Դϴ�.
    /// �������ڷ� �ش� ������ ������ ��ųʸ���, ��ü �ε��� �������� ���� �ε��� ���������� �����Ͽ��� �մϴ�.
    /// </summary>
    private void LocateDicItemToSlotList(Dictionary<string, List<GameObject>> itemDic, bool isIndexAll)
    {        
        int targetIdx;  // Ÿ�� �ε��� ����

        foreach( List<GameObject> list in itemDic.Values )  // ��ųʸ����� ���ӿ�����Ʈ ����Ʈ�� �ϳ��� ������
        {
            for( int i = 0; i<list.Count; i++ )             // ����Ʈ�� ����ִ� �ߺ� ������Ʈ�� ����ŭ �ϳ��� i�� ����
            {                
                if(isIndexAll)                                                      // ��ü�� �����̶��
                    targetIdx = list[i].GetComponent<ItemInfo>().Item.SlotIndexAll; // ��ü �ε��� ������ �о����
                else                                                                
                    targetIdx = list[i].GetComponent<ItemInfo>().Item.SlotIndex;    // ���� �� �ε��� ������ �о����             
                                
                list[i].transform.SetParent( slotListTr.GetChild( targetIdx ), false ); // �ش� �ε��� ������ �°� ��ġ�Ѵ�.
                list[i].transform.localPosition = Vector3.zero;                            // ������ġ�� ĭ�� �°� ����
            }
        }
    }




}
