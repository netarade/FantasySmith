using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemData;

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
* <v1.2 - 2023_1108_최원준>
* 1- 인벤토리 i키를 통해 닫는 기능 구현
* 2- 스크립트 부착위치를 Inventory 판넬에서 GameController 오브젝트로 옮김
* 
* <v1.3 - 2023_1109_최원준>
* 1- InventroyOnOffSwitch 철자 수정
* 
* <v2.0 - 2023_1120_최원준> 
* 1- InventoryManagement의 역할 설정
* 
* a. 게임 시작 시 인벤토리의 슬롯(오브젝트)을 생성하고, 슬롯 리스트(오브젝트) 주소를 관리하는 역할. 
* (개념 인벤토리의 게임 상 오브젝트를 관리하는 역할). 슬롯 생성 메서드를 보유 (인벤토리의 각 탭 별 제한 만큼 생성해줘야 한다.)
* 
* b. 플레이어의 개념 인벤토리를 참조하는 역할.(기존의 PlayerInven 스크립트의 역할을 통합하고 CraftManager클래스는 제거) 
* 게임 시작 시 플레이어 인벤토리를 세이브 로드하여 참조하고 있다. 씬 전환 시에도 참조 유지. 
* 
* c. 인벤토리 및 상태창의 숨김 드러남을 관리. OnOff 메서드를 보유.
* 
* d. 탭버튼 클릭시의 처리에 관한 메서드 보유
* 
* <v2.1 - 2023_1121_최원준>
* 1- BtnItemCreateToInventory(아이템 생성 테스트 버튼) 메서드 삭제
* 클래스와 연관성 없는 기능이므로 
* 
* 2- weapList, miscList 초기화 구문 삭제 
* 인벤토리 클래스 구성이 바뀌었으므로
*/

/// <summary>
/// 인벤토리 인터엑티브 로직을 담당합니다. 컴포넌트로 붙여야 하며, 별다른 인스턴스 선언 접근이 필요하지 않습니다.<br/>
/// 탭 버튼 클릭 시 동작이 정의되어 있습니다<br/>
/// 많은 외부 참조를 해야 하므로 파괴불가 속성오브젝트 보다는 새롭게 참조할 수 있는 오브젝트에 부착하록 합니다.<br/>
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
    public GameObject InventoryPanel;                               // 인벤토리 판넬을 참조하여 껐다 켰다 하기 위함.
    
    private Image[] inventoryImgArr;                    // 인벤토리의 모든 이미지
    private Text[] inventoryTextArr;                    // 인벤토리의 모든 텍스트
    public static bool isInventoryOn;                   // On상태 기록하기 위한 변수
    



    void Start()
    {
        statusWindow = GameObject.Find( "Panel-ItemStatus" );   // 상태창 참조
        slotListTr = GameObject.Find("SlotList").transform;     // 슬롯리스트 트랜스폼 참조        
        InventoryPanel = GameObject.Find("Inventory");          // 인벤토리 판넬 참조
       




        // 탭 버튼 참조
        btnTapAll = InventoryPanel.transform.GetChild(1).GetComponent<Button>();
        btnTapWeap = InventoryPanel.transform.GetChild(2).GetComponent<Button>();
        btnTapMisc = InventoryPanel.transform.GetChild(3).GetComponent<Button>();
        btnTapMisc.Select();    // 첫 시작 시 Select 표시
        
        // 버튼 이벤트 등록 - int 값 인자를 다르게 줘서 등록합니다.
        btnTapAll.onClick.AddListener( () => BtnTapClick(0) );
        btnTapWeap.onClick.AddListener( () => BtnTapClick(1) );
        btnTapMisc.onClick.AddListener( () => BtnTapClick(2) );


        // 인벤토리의 모든 이미지와 텍스트 참조
        inventoryImgArr = InventoryPanel.GetComponentsInChildren<Image>();  
        inventoryTextArr = InventoryPanel.GetComponentsInChildren<Text>();


        // 게임 시작 시 인벤토리 판넬과 상태창 판넬을 꺼둔다.
        statusWindow.SetActive(false);  
        InventoryOnOffSwitch(false);
        isInventoryOn = false;          //초기에 인벤토리는 꺼진 상태


    }






    /// <summary>
    /// 인벤토리 아이콘 클릭 시 OnOff 스위치
    /// </summary>
    public void InventoryQuit()
    {
        InventoryOnOffSwitch( !isInventoryOn );     // 반대 상태로 넣어준다.
        isInventoryOn = !isInventoryOn;             // 상태 변화를 기록한다.
    }






    /// <summary>
    /// 인벤토리 키 맵핑 I키를 누르면 인벤토리를 종료한다.
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            InventoryOnOffSwitch( !isInventoryOn );     // 반대 상태로 넣어준다.
            isInventoryOn = !isInventoryOn;             // 상태 변화를 기록한다.
        }
        
    }

    /// <summary>
    /// 인벤토리의 모든 이미지와 텍스트를 꺼줍니다.
    /// </summary>
    public void InventoryOnOffSwitch(bool onOffState )
    {
        foreach(Image image in inventoryImgArr)
            image.enabled = onOffState;
        foreach(Text text in inventoryTextArr)
            text.enabled = onOffState;

        // 인벤토리의 이미지를 키고 끌때마다 현재 슬롯에 있는 아이템의 이미지와 텍스트도 동일한 상태로 동기화 해줍니다.        
        ItemInfo[] itemInfos = slotListTr.GetComponentsInChildren<ItemInfo>();
        foreach(ItemInfo info in itemInfos)
        {
            info.gameObject.GetComponent<Image>().enabled = onOffState;

            if(info.item.Type==ItemType.Misc)   //잡화 아이템의 경우 중첩 텍스트가 있다.
            info.gameObject.GetComponentInChildren<Text>().enabled = onOffState;
        }
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
