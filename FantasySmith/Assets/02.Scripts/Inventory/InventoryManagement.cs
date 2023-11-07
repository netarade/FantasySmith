using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
 */

/// <summary>
/// �κ��丮 ���Ϳ�Ƽ�� ������ ����մϴ�. ������Ʈ�� �ٿ��� �ϸ�, ���ٸ� �ν��Ͻ� ���� ������ �ʿ����� �ʽ��ϴ�.<br/>
/// �� ��ư Ŭ�� �� ������ ���ǵǾ� �ֽ��ϴ�<br/>
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

    // Start is called before the first frame update
    void Start()
    {
        statusWindow = GameObject.Find( "Panel-ItemStatus" );   // ����â ����
        slotListTr = GameObject.Find("SlotList").transform;     // ���Ը���Ʈ ����
        weapList = CraftManager.instance.weapList;
        miscList = CraftManager.instance.miscList;

        // �� ��ư ����
        btnTapAll = GameObject.Find("Inventory").transform.GetChild(1).GetComponent<Button>();
        btnTapWeap = GameObject.Find("Inventory").transform.GetChild(2).GetComponent<Button>();
        btnTapMisc = GameObject.Find("Inventory").transform.GetChild(3).GetComponent<Button>();
        btnTapMisc.Select();    // ù ���� �� Select ǥ��
        
        // ��ư �̺�Ʈ ��� - int �� ���ڸ� �ٸ��� �༭ ����մϴ�.
        btnTapAll.onClick.AddListener( () => BtnTapClick(0) );
        btnTapWeap.onClick.AddListener( () => BtnTapClick(1) );
        btnTapMisc.onClick.AddListener( () => BtnTapClick(2) );
        
    }

    public void BtnItemCreateToInventory()
    {
        CreateManager.instance.CreateItemToNearstSlot(weapList, "ö ��");
        CreateManager.instance.CreateItemToNearstSlot(weapList, "�̽��� ��");
        CreateManager.instance.CreateItemToNearstSlot(miscList, "ö");
        CreateManager.instance.CreateItemToNearstSlot(miscList, "�̽���");
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
            Debug.Log("��ư 0");

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
