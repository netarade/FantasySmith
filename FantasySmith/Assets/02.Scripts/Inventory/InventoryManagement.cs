using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * [작업 사항]  
 * <v1.0 - 2023_1106_최원준>
 * 1- 초기 클래스 정의
 * 
 * 2- 인벤토리 인터엑티브 로직을 담당
 * 각 탭을 클릭했을 때의 메서드가 정의되어 있음.
 * 
 * 3- 현재 전체 탭 클릭에 대한 로직이 불완전하게 구현되어있음.
 * 
 * <v1.1 - 2023_1106_최원준>
 * 1-ItemPointerStatusWindow에서 상태창 참조가 동적할당으로는 제대로 잡히지 않아 
 * 게임 시작 시 참조하는 변수를 미리 잡아놓도록 static 참조 변수를 선언.
 * 
 */

/// <summary>
/// 인벤토리 인터엑티브 로직을 담당합니다. 컴포넌트로 붙여야 하며, 별다른 인스턴스 선언 접근이 필요하지 않습니다.<br/>
/// 탭 버튼 클릭 시 동작이 정의되어 있습니다<br/>
/// </summary>
public class InventoryManagement : MonoBehaviour
{
    [SerializeField] private Button btnTapAll;               // 버튼 탭을 눌렀을 때 각 탭에 맞는 아이템이 표시되도록 하기 위한 참조
    [SerializeField] private Button btnTapWeap;
    [SerializeField] private Button btnTapMisc;

    [SerializeField] private Transform slotListTr;                   // 인벤토리의 슬롯 오브젝트의 트랜스폼 참조
    [SerializeField] private List<GameObject> weapList;              // 무기 아이템을 넣어서 관리하는 인벤토리
    [SerializeField] private List<GameObject> miscList;              // 잡화 아이템을 넣어서 관리하는 인벤토리

    public static GameObject statusWindow;                          // 상태창을 표시하는 스크립트(ItemPointerStatusWindow)에서 참조하기 위해 잡아주는 변수

    // Start is called before the first frame update
    void Start()
    {
        statusWindow = GameObject.Find( "Panel-ItemStatus" );   // 상태창 참조
        slotListTr = GameObject.Find("SlotList").transform;     // 슬롯리스트 참조
        weapList = CraftManager.instance.weapList;
        miscList = CraftManager.instance.miscList;

        // 탭 버튼 참조
        btnTapAll = GameObject.Find("Inventory").transform.GetChild(1).GetComponent<Button>();
        btnTapWeap = GameObject.Find("Inventory").transform.GetChild(2).GetComponent<Button>();
        btnTapMisc = GameObject.Find("Inventory").transform.GetChild(3).GetComponent<Button>();
        btnTapMisc.Select();    // 첫 시작 시 Select 표시
        
        // 버튼 이벤트 등록 - int 값 인자를 다르게 줘서 등록합니다.
        btnTapAll.onClick.AddListener( () => BtnTapClick(0) );
        btnTapWeap.onClick.AddListener( () => BtnTapClick(1) );
        btnTapMisc.onClick.AddListener( () => BtnTapClick(2) );
        
    }

    public void BtnItemCreateToInventory()
    {
        CreateManager.instance.CreateItemToNearstSlot(weapList, "철 검");
        CreateManager.instance.CreateItemToNearstSlot(weapList, "미스릴 검");
        CreateManager.instance.CreateItemToNearstSlot(miscList, "철");
        CreateManager.instance.CreateItemToNearstSlot(miscList, "미스릴");
    }


    
    /// <summary>
    /// 버튼 클릭 시 각탭에 해당하는 아이템만 보여주기 위한 메서드입니다. 각 탭의 버튼 클릭 이벤트에 등록해야 합니다.
    /// </summary>
    /// <param name="btnIdx"></param>
    public void BtnTapClick( int btnIdx )
    {
        
        
        if(btnIdx==0)       // All 탭
        {   
            if( slotListTr.parent.GetChild(4).childCount==0 ) //오브젝트 emptyList에 아무것도 담겨 있지 않다면, 실행하지 않는다.
                return;
            Debug.Log("버튼 0");

            for(int i=0; i<weapList.Count; i++) //무기 리스트를 하나씩 배치
            {
                for(int j=0; j<slotListTr.childCount; j++) // 슬롯을 차례로 순회
                {
                    if(slotListTr.GetChild(j).childCount==0)
                    {
                        weapList[i].transform.SetParent( slotListTr.GetChild(j), false );     // 빈 공간에 배치
                        weapList[i].transform.localPosition = Vector3.zero;
                        break;
                    }                
                } 
            }
            for(int i=0; i<miscList.Count; i++) //기타 리스트를 하나씩 배치
            {
                for(int j=0; j<slotListTr.childCount; j++) // 슬롯을 차례로 순회
                {
                    if(slotListTr.GetChild(j).childCount==0)
                    {
                        miscList[i].transform.SetParent( slotListTr.GetChild(j), false );     // 빈 공간에 배치
                        miscList[i].transform.localPosition = Vector3.zero;
                        break;
                    }                
                } 
            } 
              
        }
        else if(btnIdx==1)  // Sword 탭
        {
            for(int i=0; i<slotListTr.childCount; i++) // 슬롯의 갯수만큼
            {
                if( slotListTr.GetChild(i).childCount!=0 ) // 슬롯에 들어있는 오브젝트를 모두 잠시 오브젝트 emptyList로 이동한다.
                {
                    slotListTr.GetChild(i).GetChild(0).localPosition = Vector3.zero;
                    slotListTr.GetChild(i).GetChild(0).SetParent(slotListTr.parent.GetChild(4), false);
                }
            }

            for( int i = 0; i<weapList.Count; i++ ) // 무기 리스트의 모든 아이템 정보를 읽어들인다.
            {
                int targetIdx = weapList[i].GetComponent<ItemInfo>().item.SlotIndex;
                weapList[i].transform.SetParent( slotListTr.GetChild(targetIdx), false );     // 해당 정보에 맞게 배치한다.
                weapList[i].transform.localPosition = Vector3.zero;                                                                                              
            }
        }
        else if(btnIdx==2)  // Misc 탭
        {
            for(int i=0; i<slotListTr.childCount; i++) // 슬롯의 갯수 만큼
            {
                if( slotListTr.GetChild(i).childCount!=0 ) // 슬롯에 들어있는 오브젝트를 모두 잠시 오브젝트 emptyList로 이동한다.
                {
                    slotListTr.GetChild(i).GetChild(0).localPosition = Vector3.zero;
                    slotListTr.GetChild(i).GetChild(0).SetParent(slotListTr.parent.GetChild(4), false);
                }
            }

            for( int i = 0; i<miscList.Count; i++ ) // 기타 리스트의 모든 아이템 정보를 읽어들인다. 해당 정보에 맞게 배치한다.
            {
                int targetIdx = miscList[i].GetComponent<ItemInfo>().item.SlotIndex;
                miscList[i].transform.SetParent( slotListTr.GetChild(targetIdx), false );     
                miscList[i].transform.localPosition = Vector3.zero;                                                                                              
            }
        }

    }


    
}
