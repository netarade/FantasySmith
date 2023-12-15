using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemData;
using CraftData;

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
* 2- weapList, miscList �ʱ�ȭ ���� ���� 
* �κ��丮 Ŭ���� ������ �ٲ�����Ƿ�
* 
* <v2.2 - 2023_1122_�ֿ���>
* 1- �� ���Ը���Ʈ ���� �� ����, ���Ը���Ʈ�� ���� ������ �����Ҵ� ���� ��  
* 
* <v3.0 - 2023_1215_�ֿ���>
* 1- �κ��丮�� �����ϴ� ĵ���� �������� ���� �κ��丮 �� ����â ��ġ, ��ư���� �������� ���� �� ���� ���� (�±׷� ĵ���� ����)
* 2- �ǹ�ư ��� ���� ���� ��
*/

/// <summary>
/// �κ��丮 ���Ϳ�Ƽ�� ������ ����մϴ�. ������Ʈ�� �ٿ��� �ϸ�, ���ٸ� �ν��Ͻ� ���� ������ �ʿ����� �ʽ��ϴ�.<br/>
/// �κ��丮 �ݱ�, �� ���� �� ��ư Ŭ�� �� ������ ���ǵǾ� �ֽ��ϴ�<br/>
/// ���� �ܺ� ������ �ؾ� �ϹǷ� �ı��Ұ� �Ӽ�������Ʈ ���ٴ� ���Ӱ� ������ �� �ִ� ������Ʈ�� �����Ϸ� �մϴ�.<br/>
/// </summary>
public class InventoryManagement : MonoBehaviour
{
    [SerializeField] private Button btnTapAll;                      // ��ư ���� ������ �� �� �ǿ� �´� �������� ǥ�õǵ��� �ϱ� ���� ����
    [SerializeField] private Button btnTapWeap;
    [SerializeField] private Button btnTapMisc;

    public static Transform statusWindowTr;                         // ����â�� ǥ���ϴ� ��ũ��Ʈ(ItemPointerStatusWindow)���� �����ϱ� ���� ����ִ� ����
    public Transform InventoryPanelTr;                              // �κ��丮 �ǳ��� �����Ͽ� ���� �״� �ϱ� ����.
        
    [SerializeField] private Transform slotListAllTr;               // �κ��丮 ��ü�� ���Ը���Ʈ�� Ʈ������ ����
    [SerializeField] private Transform slotListWeapTr;              // �κ��丮 ������ ���Ը���Ʈ�� Ʈ������ ����
    [SerializeField] private Transform slotListMiscTr;              // �κ��丮 ��Ÿ�� ���Ը���Ʈ�� Ʈ������ ����
    public GameObject slotPrefab;                                   // ������ �������� �����ϱ� ���� ������ ����


    private Image[] inventoryImgArr;                                // �κ��丮�� ��� �̹���
    private Text[] inventoryTextArr;                                // �κ��丮�� ��� �ؽ�Ʈ
    public static bool isInventoryOn;                               // On���� ����ϱ� ���� ����
    
    Inventory inventory;                                            // �÷��̾��� �κ��丮 ����
    Transform emptyListTr;                                          // �������� ������ �ʴ� ���� ��� �Űܵ� ������Ʈ(����Ʈ�� 1��° �ڽ�)


    void Start()
    {
        Transform canvas = GameObject.FindWithTag("CANVAS_CHARACTER").transform;        // ĳ���� ĵ���� ����
        statusWindowTr = canvas.GetChild(1);                                            // ����â ����    
        InventoryPanelTr = canvas.GetChild(0);                                          // �κ��丮 �ǳ� ����
        slotListAllTr = InventoryPanelTr.GetChild(0).GetChild(0).GetChild(0);           // ����Ʈ-����Ʈ-��ü ���Ը���Ʈ
        slotListWeapTr = InventoryPanelTr.GetChild(0).GetChild(0).GetChild(1);          // ����Ʈ-����Ʈ-���� ���Ը���Ʈ
        slotListMiscTr = InventoryPanelTr.GetChild(0).GetChild(0).GetChild(2);          // ����Ʈ-����Ʈ-��Ÿ ���Ը���Ʈ
        emptyListTr = InventoryPanelTr.GetChild(0).GetChild(1);                         // ����Ʈ-EmptyList
       
        // �� ��ư ����
        btnTapAll = InventoryPanelTr.GetChild(1).GetChild(0).GetComponent<Button>();
        btnTapWeap = InventoryPanelTr.GetChild(1).GetChild(1).GetComponent<Button>();
        btnTapMisc = InventoryPanelTr.GetChild(1).GetChild(2).GetComponent<Button>();
        btnTapMisc.Select();    // ù ���� �� Select ǥ��
        
        // ��ư �̺�Ʈ ��� - int �� ���ڸ� �ٸ��� �༭ ����մϴ�.
        btnTapAll.onClick.AddListener( () => BtnTapClick(0) );
        btnTapWeap.onClick.AddListener( () => BtnTapClick(1) );
        btnTapMisc.onClick.AddListener( () => BtnTapClick(2) );


        // �κ��丮�� ��� �̹����� �ؽ�Ʈ ����
        inventoryImgArr = InventoryPanelTr.GetComponentsInChildren<Image>();  
        inventoryTextArr = InventoryPanelTr.GetComponentsInChildren<Text>();


        // ���� ���� �� �κ��丮 �ǳڰ� ����â �ǳ��� ���д�.
        statusWindowTr.gameObject.SetActive(false);  
        InventoryOnOffSwitch(false);
        isInventoryOn = false;          //�ʱ⿡ �κ��丮�� ���� ����


        //�÷��̾� ������Ʈ�� �����Ͽ� �κ��丮 ������ �޾ƿɴϴ�
        PlayerInven invenInfo = GameObject.FindWithTag("Player").GetComponent<PlayerInven>();
        inventory = invenInfo.GetComponent<Inventory>();


        // �÷��̾� �κ��丮 ����(�� �Ǻ� �κ��丮 ĭ��)�� �����Ͽ� ������ �������� ����
        for(int i=0; i< invenInfo.inventory.AllCountLimit; i++)
            Instantiate(slotPrefab, slotListAllTr);
        for(int i=0; i< invenInfo.inventory.WeapCountLimit; i++)
            Instantiate(slotPrefab, slotListWeapTr);
        for(int i=0; i< invenInfo.inventory.MiscCountLimit; i++)
            Instantiate(slotPrefab, slotListMiscTr);


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
    /// �κ��丮 Ű ���� IŰ�� ������ �κ��丮�� �����Ѵ�.
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
    public void InventoryOnOffSwitch(bool onOffState )
    {
        foreach(Image image in inventoryImgArr)
            image.enabled = onOffState;
        foreach(Text text in inventoryTextArr)
            text.enabled = onOffState;

        // �κ��丮�� �̹����� Ű�� �������� ���� ���Կ� �ִ� �������� �̹����� �ؽ�Ʈ�� ������ ���·� ����ȭ ���ݴϴ�.        
        ItemInfo[] itemInfos = slotListAllTr.GetComponentsInChildren<ItemInfo>();
        foreach(ItemInfo info in itemInfos)
        {
            info.gameObject.GetComponent<Image>().enabled = onOffState;

            if(info.Item.Type==ItemType.Misc)   //��ȭ �������� ��� ��ø �ؽ�Ʈ�� �ִ�.
            info.gameObject.GetComponentInChildren<Text>().enabled = onOffState;
        }
    }




    /// <summary>
    /// ��ư Ŭ�� �� ���ǿ� �ش��ϴ� �����۸� �����ֱ� ���� �޼����Դϴ�. �� ���� ��ư Ŭ�� �̺�Ʈ�� ����ؾ� �մϴ�.
    /// </summary>
    /// <param name="btnIdx"></param>
    public void BtnTapClick( int btnIdx )
    {
        
        if(btnIdx==0)       // All ��
        {   
            //if( emptyListTr.childCount==0 ) //������Ʈ emptyList�� �ƹ��͵� ��� ���� �ʴٸ�, �������� �ʴ´�.
            //    return;

            for(int i=0; i<inventory.WeapCount; i++)          // (���Ⱑ ��� ��������) ���� ����Ʈ�� �ϳ��� ��ġ
            {
                for(int j=0; j<slotListAllTr.childCount; j++) // ������ ���ʷ� ��ȸ
                {
                    if(slotListAllTr.GetChild(j).childCount==0)
                    {
                        weapList[i].transform.SetParent( slotListAllTr.GetChild(j), false );     // �� ������ ��ġ
                        weapList[i].transform.localPosition = Vector3.zero;
                        break;
                    }                
                } 
            }
            for(int i=0; i<miscList.Count; i++) //��Ÿ ����Ʈ�� �ϳ��� ��ġ
            {
                for(int j=0; j<slotListAllTr.childCount; j++) // ������ ���ʷ� ��ȸ
                {
                    if(slotListAllTr.GetChild(j).childCount==0)
                    {
                        miscList[i].transform.SetParent( slotListAllTr.GetChild(j), false );     // �� ������ ��ġ
                        miscList[i].transform.localPosition = Vector3.zero;
                        break;
                    }                
                } 
            } 
              
        }
        else if(btnIdx==1)  // Sword ��
        {
            for(int i=0; i<inventory.WeapCountLimit; i++)          // ���� ���Ը���Ʈ�� ���� ������ŭ i�� �ϳ��� ������Ų��
            {
                if( slotListWeapTr.GetChild(i).childCount!=0 )      // i��° ���Կ� �������� ����ִٸ�
                {
                    // ���Կ� ����ִ� ������ ������Ʈ�� ��� ��� ������Ʈ emptyList�� �̵��ϰ�, ��ġ�� ����
                    slotListWeapTr.GetChild(i).GetChild(0).SetParent(emptyListTr, false);
                    slotListWeapTr.GetChild(i).GetChild(0).localPosition = Vector3.zero;
                }
            }

            // �κ��丮�� ����������� ���ӿ�����Ʈ�� ����ִ� ����Ʈ�� �ϳ��� �����´�
            foreach(List<GameObject> weapList in inventory.weapDic.Values)
            {
                for( int i = 0; i<weapList.Count; i++ ) // ����Ʈ�� ����ִ� �ߺ� ������Ʈ�� ����ŭ �ϳ��� i�� ����
                {
                    int targetIdx = weapList[i].GetComponent<ItemInfo>().Item.SlotIndex;             // �������� �� �ε��� ������ �о����
                    weapList[i].transform.SetParent( slotListWeapTr.GetChild( targetIdx ), false );  // �ش� �ε��� ������ �°� ��ġ
                    weapList[i].transform.localPosition=Vector3.zero;                                // ������ġ�� ĭ�� �°� ����
                }
            }                                                                                                             
            
        }
        else if(btnIdx==2)  // Misc �ǹ�ư Ŭ��
        {
            for(int i=0; i<inventory.MiscCountLimit; i++)          // ��ȭ ���Ը���Ʈ�� ���� ������ŭ i�� �ϳ��� ������Ų��
            {
                if( slotListMiscTr.GetChild(i).childCount!=0 )      // i��° ���Կ� �������� ����ִٸ�
                {
                    // ���Կ� ����ִ� ������ ������Ʈ�� ��� ��� ������Ʈ emptyList�� �̵��ϰ�, ��ġ�� ����
                    slotListMiscTr.GetChild(i).GetChild(0).SetParent(emptyListTr, false);
                    slotListMiscTr.GetChild(i).GetChild(0).localPosition = Vector3.zero;
                }
            }

            // �κ��丮�� ��ȭ�������� ���ӿ�����Ʈ�� ����ִ� ����Ʈ�� �ϳ��� �����´�
            foreach(List<GameObject> miscList in inventory.miscDic.Values)
            {
                for( int i = 0; i<miscList.Count; i++ ) // ����Ʈ�� ����ִ� �ߺ� ������Ʈ�� ����ŭ �ϳ��� i�� ����
                {
                    int targetIdx = miscList[i].GetComponent<ItemInfo>().Item.SlotIndex;            // �������� �� �ε��� ������ �о����
                    miscList[i].transform.SetParent( slotListMiscTr.GetChild( targetIdx ), false ); // �ش� �ε��� ������ �°� ��ġ�Ѵ�.
                    miscList[i].transform.localPosition=Vector3.zero;                               // ������ġ�� ĭ�� �°� ����
                }
            }
        }

    }


    
}
