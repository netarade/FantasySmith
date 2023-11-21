using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemData;

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
*/

/// <summary>
/// �κ��丮 ���Ϳ�Ƽ�� ������ ����մϴ�. ������Ʈ�� �ٿ��� �ϸ�, ���ٸ� �ν��Ͻ� ���� ������ �ʿ����� �ʽ��ϴ�.<br/>
/// �� ��ư Ŭ�� �� ������ ���ǵǾ� �ֽ��ϴ�<br/>
/// ���� �ܺ� ������ �ؾ� �ϹǷ� �ı��Ұ� �Ӽ�������Ʈ ���ٴ� ���Ӱ� ������ �� �ִ� ������Ʈ�� �����Ϸ� �մϴ�.<br/>
/// </summary>
public class InventoryManagement : MonoBehaviour
{
    [SerializeField] private Button btnTapAll;               // ��ư ���� ������ �� �� �ǿ� �´� �������� ǥ�õǵ��� �ϱ� ���� ����
    [SerializeField] private Button btnTapWeap;
    [SerializeField] private Button btnTapMisc;

    [SerializeField] private Transform slotListTr;                   // �κ��丮�� ���� ������Ʈ�� Ʈ������ ����
    [SerializeField] private List<GameObject> weapList;              // ���� �������� �־ �����ϴ� �κ��丮
    [SerializeField] private List<GameObject> miscList;              // ��ȭ �������� �־ �����ϴ� �κ��丮

    public static GameObject statusWindow;                          // ����â�� ǥ���ϴ� ��ũ��Ʈ(ItemPointerStatusWindow)���� �����ϱ� ���� ����ִ� ����
    public GameObject InventoryPanel;                               // �κ��丮 �ǳ��� �����Ͽ� ���� �״� �ϱ� ����.
    
    private Image[] inventoryImgArr;                    // �κ��丮�� ��� �̹���
    private Text[] inventoryTextArr;                    // �κ��丮�� ��� �ؽ�Ʈ
    public static bool isInventoryOn;                   // On���� ����ϱ� ���� ����
    



    void Start()
    {
        statusWindow = GameObject.Find( "Panel-ItemStatus" );   // ����â ����
        slotListTr = GameObject.Find("SlotList").transform;     // ���Ը���Ʈ Ʈ������ ����        
        InventoryPanel = GameObject.Find("Inventory");          // �κ��丮 �ǳ� ����
       




        // �� ��ư ����
        btnTapAll = InventoryPanel.transform.GetChild(1).GetComponent<Button>();
        btnTapWeap = InventoryPanel.transform.GetChild(2).GetComponent<Button>();
        btnTapMisc = InventoryPanel.transform.GetChild(3).GetComponent<Button>();
        btnTapMisc.Select();    // ù ���� �� Select ǥ��
        
        // ��ư �̺�Ʈ ��� - int �� ���ڸ� �ٸ��� �༭ ����մϴ�.
        btnTapAll.onClick.AddListener( () => BtnTapClick(0) );
        btnTapWeap.onClick.AddListener( () => BtnTapClick(1) );
        btnTapMisc.onClick.AddListener( () => BtnTapClick(2) );


        // �κ��丮�� ��� �̹����� �ؽ�Ʈ ����
        inventoryImgArr = InventoryPanel.GetComponentsInChildren<Image>();  
        inventoryTextArr = InventoryPanel.GetComponentsInChildren<Text>();


        // ���� ���� �� �κ��丮 �ǳڰ� ����â �ǳ��� ���д�.
        statusWindow.SetActive(false);  
        InventoryOnOffSwitch(false);
        isInventoryOn = false;          //�ʱ⿡ �κ��丮�� ���� ����


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
        ItemInfo[] itemInfos = slotListTr.GetComponentsInChildren<ItemInfo>();
        foreach(ItemInfo info in itemInfos)
        {
            info.gameObject.GetComponent<Image>().enabled = onOffState;

            if(info.item.Type==ItemType.Misc)   //��ȭ �������� ��� ��ø �ؽ�Ʈ�� �ִ�.
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
            if( slotListTr.parent.GetChild(4).childCount==0 ) //������Ʈ emptyList�� �ƹ��͵� ��� ���� �ʴٸ�, �������� �ʴ´�.
                return;

            for(int i=0; i<weapList.Count; i++) //���� ����Ʈ�� �ϳ��� ��ġ
            {
                for(int j=0; j<slotListTr.childCount; j++) // ������ ���ʷ� ��ȸ
                {
                    if(slotListTr.GetChild(j).childCount==0)
                    {
                        weapList[i].transform.SetParent( slotListTr.GetChild(j), false );     // �� ������ ��ġ
                        weapList[i].transform.localPosition = Vector3.zero;
                        break;
                    }                
                } 
            }
            for(int i=0; i<miscList.Count; i++) //��Ÿ ����Ʈ�� �ϳ��� ��ġ
            {
                for(int j=0; j<slotListTr.childCount; j++) // ������ ���ʷ� ��ȸ
                {
                    if(slotListTr.GetChild(j).childCount==0)
                    {
                        miscList[i].transform.SetParent( slotListTr.GetChild(j), false );     // �� ������ ��ġ
                        miscList[i].transform.localPosition = Vector3.zero;
                        break;
                    }                
                } 
            } 
              
        }
        else if(btnIdx==1)  // Sword ��
        {
            for(int i=0; i<slotListTr.childCount; i++) // ������ ������ŭ
            {
                if( slotListTr.GetChild(i).childCount!=0 ) // ���Կ� ����ִ� ������Ʈ�� ��� ��� ������Ʈ emptyList�� �̵��Ѵ�.
                {
                    slotListTr.GetChild(i).GetChild(0).localPosition = Vector3.zero;
                    slotListTr.GetChild(i).GetChild(0).SetParent(slotListTr.parent.GetChild(4), false);
                }
            }

            for( int i = 0; i<weapList.Count; i++ ) // ���� ����Ʈ�� ��� ������ ������ �о���δ�.
            {
                int targetIdx = weapList[i].GetComponent<ItemInfo>().item.SlotIndex;
                weapList[i].transform.SetParent( slotListTr.GetChild(targetIdx), false );     // �ش� ������ �°� ��ġ�Ѵ�.
                weapList[i].transform.localPosition = Vector3.zero;                                                                                              
            }
        }
        else if(btnIdx==2)  // Misc ��
        {
            for(int i=0; i<slotListTr.childCount; i++) // ������ ���� ��ŭ
            {
                if( slotListTr.GetChild(i).childCount!=0 ) // ���Կ� ����ִ� ������Ʈ�� ��� ��� ������Ʈ emptyList�� �̵��Ѵ�.
                {
                    slotListTr.GetChild(i).GetChild(0).localPosition = Vector3.zero;
                    slotListTr.GetChild(i).GetChild(0).SetParent(slotListTr.parent.GetChild(4), false);
                }
            }

            for( int i = 0; i<miscList.Count; i++ ) // ��Ÿ ����Ʈ�� ��� ������ ������ �о���δ�. �ش� ������ �°� ��ġ�Ѵ�.
            {
                int targetIdx = miscList[i].GetComponent<ItemInfo>().item.SlotIndex;
                miscList[i].transform.SetParent( slotListTr.GetChild(targetIdx), false );     
                miscList[i].transform.localPosition = Vector3.zero;                                                                                              
            }
        }

    }


    
}
