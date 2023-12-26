using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using CraftData;
using System;
using UnityEngine.SceneManagement;
using WorldItemData;


/*
 * 
* [작업 사항]
* 
* <v1.0 - 2023_1101_최원준>
* 1- 테스트용 클래스 작성
* 
* <v1.1 - 2023_1101_최원준>
* 1- 아이템 클래스 기반으로 아이템 초기화, 구매, 제작, 숙련도 변화 예시를 만들어봄.
* 2- 기타 주석 처리
* 
* <v2.0 - 2023_1103_최원준>
* 1- 중복, 복잡한 로직 메서드화하여 간소화 및 재사용 가능하게 변경
* 
* 2- playerInventory가 Item을 가지는 것이 아니라 GameObject를 가지도록 수정.
* playerInventory는 게임 시작 시 플레이어가 가지게 될 리스트 변수이며, 게임 종료와 시작 시 저장 및 로드해야 할 변수이다.
* 이 리스트에는 개념아이템이 아니라 아이템오브젝트를 보관해야 한다. 아이템 오브젝트에는 개념아이템 정보를 포함한다.
* 
* <v3.0 - 2023_1104_최원준>
* 1- 플레이어 인벤토리를 GameObject를 보관하는 weapList, miscList로 분리하여 관리하고 구조체로 통합
* 
* <v3.1 - 2023_1105_최원준>
* 1- ItemInfo의 OnItemChanged 메서드의 호출 방식 내부자동 호출 변경으로 인해서 
* 해당 메서드 사용구문을 삭제
* 2- CreateManager의 접근 편리성과 딕셔너리 및 메서드 항시 상주화 필요성으로 인해 싱글톤으로 수정. 
* 
* <v3.2 - 2023_1106_최원준>
* 1- 무기 아이템 사전 아이템 클래스 추가 설계로 인한 주석처리
* 2- 잡화 및 무기아이템 사전 입력 진행 중(미완료)
* 
* <v3.3 - 2023_1106_최원준>
* 1- 무기, 잡화 아이템 사전 입력완료
* 2- 탭 클릭 이벤트 추가
* 각 탭을 클릭할 때 해당 오브젝트만 보여지도록 하였음.
* 
* <v4.0 - 2023_1106_최원준>
* 1- Start로직 Awake로 통합.
* 씬이 넘어가더라도 한번만 가지고 있으면 되며, 다른 클래스의 Start 로직에서 활용해야 하기 때문
* 2- 버튼 탭 클릭이벤트를 InventoryManager 스크립트로 파일 분리
* 3- 필요없는 변수 삭제, 메서드 간소화 및 이름 정리
* 4- 외부에서 호출이 불필요한 메서드 private처리 
* 5- 제작목록 클래스 구조 변경으로 인한 CreateNewPlayerCraftableList 메서드 수정
* 6- 주석 보완
* 
* <v4.1- 2023_1106_최원준>
* 1- DataManager사용 구문 Initialize()메서드 추가
* 2- DataManager Json 방식으로 교체
* 3- CreateNewPlayerCraftableList에서 bowList를 중복하여 2번사용하여 1개를 weapList로 수정
* 
* <v4.2 - 2023_1106_최원준>
* 1- 플레이어 관련 초기화 구문을 Start 위치로 옮기고 메서드 정리
* 2- 아이템의 중첩횟수를 오브젝트가 반영하도록 수정
* 
* <v4.3 - 2023_1108_최원준>
* 1- Inventory 클래스 세이브 문제로 ItemInfo를 인자로 주면 새로운 프리팹을 만들어주는 메서드와
* ItemInfo를 바탕으로 오브젝트의 정보를 최신화 해주는 메서드를 구현하려고하였다가 
* Item 클래스 자체의 설계 문제가 있음을 알고 중단
* 2- CreateNewPlayerCraftableList메서드 내부에서 리스트가 안채워져 있음에도 불구하고 return하는 현상 수정 
* 
* <v4.4 - 2023_1108_최원준>
* 1- 인벤토리의 저장 및 로드가 제대로 되지 않아 
* 플레이어 관련 변수는 계속 초기화가 이루어지도록 하였으며, 그에 따라 내부에 if와 return문을 삭제.
* 2- 잡화 아이템 생성 시 중첩카운트에 포함되지 않고 새로운 오브젝트를 생성하던 점 수정.
* 3- 아이템 생성 시 현재 인벤토리의 상태에 따라 이미지를 끄게 수정
* 4- 골드와 실버 플레이어 변수 추가
* 
* <v4.5 - 2023_1109_최원준>
* 1- 아이템을 생성했을 때 이미지가 꺼져있던 점을 수정. - 
* 인벤토리가 꺼졌을 때는 하위의 이미지를 끄겠다고 생각했는데 문제는 다시 인벤토리가 켜질 때 하위의 이미지를 끈채로 시작하게 되었기 때문. 
* (상위 부모가 꺼졌을 때는 자식 컴포넌트를 켜도 보이지 않는다. 부모가 꺼졌다고 자식 컴포넌트를 꺼버리면 자식 컴포넌트를 끈채로 시작하게 되버린다.)
* 
* 2- 잡화아이템 강화석 추가, MiscType을 Attribute에서 Enhancement로 변경 통일 
* 
* 3- 각인의 월드 사전 생성이 MiscType.Basic이었던 점을 Enhancement로 수정
* 
* <v5.0 - 2023_1112_최원준>
* 1- CreateItemToNearstSlot메서드에 클론한 아이템을 등록할 때 (오브젝트의 중첩텍스트를 켜는) 불필요한 중복코드를 집어넣었던 점을 수정
* 
* <v6.0 - 2023_1115_최원준>
* 1- 인벤토리 클래스 딕셔너리 전환으로 인한 Create메서드 대폭 수정
* 2- ItemInfo 클래스에서 item 값 입력 시 자동 메서드 호출로 인한 중복코드 수정
* 3- Create메서드 내부 지역 변수명이 itemInfo로 되어있던 점을 itemMisc으로 수정
* 
* <v7.0 - 2023_1116_최원준>
* 1- CreateManager클래스 내부의 ItemImageCollection 변수 및 참조 선언을 삭제하고 ItemInfo로 옮김.
* 2- OnSceneLoaded메서드에서 ItemImageCollection 참조를 다시 잡아주던 부분을 삭제
* 3- 월드 아이템 딕셔너리 생성 시 ItemImageCollection 참조 변수를 활용하던 부분을 ImageReferenceIndex 변수 생성 방식으로 변경
* (아이템 생성시 외부 이미지 참조를 직접 저장하던 방식에서 외부 이미지 변수에 접근하여 참조 할 고유 인덱스를 저장하는 방식으로 변경)
* 
* <v8.0 - 2023_1119_최원준>
* 1- 플레이어 초기화 관련 메서드들을 모두 삭제하였음.(CreateFirstPlayerData와 내부 호출 메서드들)
* 기존에 제작 목록, 숙련 목록을 따로 초기화 해주는 메서드가 CreateManager 클래스에 있었는데 CraftData에서 하나의 클래스로 통합하였으며,
* 제작 목록을 인스턴스 생성 시에 CreateManager를 참고하여 알아서 생성해주도록 하는 것이 클래스 의존성을 감소시킨다고 판단.
* 
* 2- InitEvent 삭제
* 마찬가지로 CraftManager에서 처음 시작 시 직접 인벤토리 클래스 생성 및 제작목록 클래스 생성을 담당하면 CreateManager의 Init을 기다릴 필요가 없기 때문
* 
* 3- 아이템 복제 및 상점 구매, 숙련도 조정 예시 등 주석처리 되어있던 것을 삭제하였음.
* 
* <v9.0 -2023_1216_최원준>
* 1-속성석을 수금지화풍 이름으로 나눈 후 MiscType.Attribute로 변경
* 
* <v10.0 - 2023_1220_최원준>
* 1- CreateItemToNearstSlot 메서드를 수정된 인벤토리 클래스에 맞게 변경을 진행하는 중
* 
* <v10.1 - 2023_1221_최원준>
* 1- CreateItemToNearstSlot 메서드를 수정 완료하였습니다
* 내부적으로 AddCloneItemToInventory 메서드와 CreateMiscItemRepeatly 를 호출하고 있습니다
* 
* 2- CreateAllItemDictionary메서드 내부의 월드아이템 데이터를 
* MonoBehaviour를 상속하지 않는 WorldItemData_Misc과 Weapon 스크립트로 분리시켜 이동하였습니다.
* 이유는 MonoBehaviour를 상속하지 않는 CraftData에서 CreateManager의 싱글톤에 접근하지 못하기 때문입니다.
* 
* <v10.2 - 2023-1224_최원준>
* 1- CreateAllItemDic 메서드를 LoadAllItemDic으로 변경
* 
* <v10.3 - 2023_1224_최원준>
* 1- CreateItemByInfo메서드에서 두번째 인자에서 게임오브젝트 활성화여부를 묻던점을 삭제하였음.
* (비활성화해서 반환하기 위한 목적이었으나 다른 형태로 해결하기 위해)
* 
* <v10.4 - 2023_1226_최원준>
* 1- AddCloneItemToInventory메서드에서 OnItemChanged를 메서드를 호출해주는 것으로 변경
* 
* 
*/


/// <summary>
/// 아이템 생성에 관한 로직을 담당하는 클래스이며,<br/>
/// 아이템 생성을 원하는 시점에 싱글톤 인스턴스를 참조하여 관련 메서드를 호출하면 됩니다.
/// </summary>
public class CreateManager : MonoBehaviour
{
    public static CreateManager instance;       // 매니저 싱글톤 인스턴스 생성
           
    public Dictionary<string, Item> miscDic;    // 게임 시작 시 넣어 둘 전체 잡화아이템 사전 
    public Dictionary<string, Item> weaponDic;  // 게임 시작 시 넣어 둘 전체 무기아이템 사전

    [SerializeField] Transform slotListTr;      // 인벤토리의 슬롯 오브젝트의 트랜스폼 참조
    [SerializeField] GameObject itemPrefab;     // 리소스 폴더 또는 직접 드래그해서 복제할 오브젝트를 등록해야 한다.        
    

    public void Awake()
    {        
        if( instance==null )
        {
            instance=this;
            DontDestroyOnLoad( instance );
        }
        else if( instance!=null )
            Destroy( this.gameObject );           // 싱글톤 이외 인스턴스는 삭제

        // 리소스 폴더에서 원본 프리팹 가져오기
        itemPrefab=Resources.Load<GameObject>( "ItemOrigin" );  

        // 슬롯리스트를 인식 시켜 남아있는 슬롯을 확인하기 위해
        Transform canvasTr = GameObject.FindWithTag("CANVAS_CHARACTER").transform;
        slotListTr=canvasTr.GetChild(0).GetChild(0).GetChild(0).GetChild(0);       
        
        // 모든 월드 아이템 등록
        LoadAllItemDictionary();    

        SceneManager.sceneLoaded += OnSceneLoaded; // 씬이로드될때 새롭게 참조를 잡아준다.
                
    }


    /// <summary>
    /// 씬이 전환되었을 때 이벤트 호출로 다시 참조를 잡기 위한 로직
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 리소스 폴더에서 원본 프리팹 가져오기
        itemPrefab=Resources.Load<GameObject>( "ItemOrigin" );  

        // 슬롯리스트를 인식 시켜 남아있는 슬롯을 확인하기 위해
        Transform canvasTr = GameObject.FindWithTag("CANVAS_CHARACTER").transform;
        slotListTr=canvasTr.GetChild(0).GetChild(0).GetChild(0).GetChild(0);       
        
        // 모든 월드 아이템 등록
        LoadAllItemDictionary();     
    }

    /// <summary>
    /// 오브젝트 삭제 시 이벤트 연결 해제
    /// </summary>
    public void OnDestroy()
    {        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }





    /// <summary>
    /// 가장 가까운 슬롯에 원하는 아이템을 생성합니다.<br/>
    /// 아이템 생성에 성공했다면 0을 반환합니다.<br/>
    /// 잡화 아이템의 경우 넣은 수량만큼 아이템을 중첩하여 새롭게 생성하여 주지만,<br/>
    /// 추가 오브젝트로 인하여 새롭게 아이템을 생성하지 못하는 경우 나머지 수량을 반환합니다.<br/>
    /// * 이름이 일치하지 않을 경우 예외를 발생시킵니다. *
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inventory">플레이어의 인벤토리 변수가 필요합니다. 해당 인벤토리에 아이템 오브젝트를 추가하여 줍니다.</param>
    /// <param name="itemName">아이템 테이블을 참고하여 한글명을 기입해주세요. 띄어쓰기에 유의해야 합니다. ex)철, 철 검, 미스릴, 미스릴 검</param>
    /// <param name="count">생성을 원하는 갯수입니다. 하나의 슬롯에 중첩하여 생성될 것입니다. 기본 값은 1이며, 장비류는 반드시 1개만 생성됩니다.</param>
    /// <returns>반환값은 아이템을 생성하고 더 이상 생성하지 못하는 나머지 수량입니다. 모든 생성이 완료되었다면 0을 반환합니다.</returns>
    public int CreateItemToNearstSlot(Inventory inventory, string itemName, int count = 1)
    {
        if( count <=0 ) //수량이 0이하로 들어왔다면 무조건 성공했다고 판단합니다.
            return 0;

        int findSlotIdx = FindNearstRemainSlotIdx(); //가장 가까운 슬롯인덱스를 반환받습니다

        // 인자로 들어온 이름이 장비류인지, 잡화류인지를 설정합니다
        bool isWeapName = weaponDic.ContainsKey( itemName );
        bool isMiscName = miscDic.ContainsKey( itemName );

        // 인벤토리의 남아있는 칸이 없다면 생성하지 못합니다 (잡화라면 인자로 들어온 count를 반환하고, 장비류라면 1을 반환합니다)
        if( findSlotIdx == -1 && isMiscName ) 
            return count;
        else if(findSlotIdx == -1)
            return 1;
                
        
        // 사전의 개념 아이템을 클론하여 (아이템 원형을 복제해서) 또 다른 개념 아이템을 생성하기위한 변수입니다
        Item itemClone = null;

        // 개념아이템을 실제 오브젝트로 만들어 넣어야하는, 유저의 인벤토리 딕셔너리에 존재하는 아이템 오브젝트 리스트입니다
        List<GameObject> itemObjList;  
               

        if( isWeapName )         // 이름이 무기 종류라면
        {
            // 인벤토리의 무기목록에 해당 아이템 리스트가 없다면 리스트를 새롭게 생성합니다
            if( !inventory.weapDic.ContainsKey( itemName ) )  
                inventory.weapDic[itemName] = new List<GameObject>();
            
            // 인벤토리의 해당 딕셔너리에 접근하여 아이템 오브젝트 리스트 참조를 설정합니다
            itemObjList = inventory.weapDic[itemName];  

            // 무기라면 무조건 복제합니다 (중첩될 일이 없으므로)
            itemClone=(ItemWeapon)weaponDic[itemName].Clone();   
                        
            // 인벤토리 리스트에 개념아이템을 장착한 오브젝트를 추가합니다
            AddCloneItemToInventory( itemObjList, itemClone, findSlotIdx );

        }
        else if( isMiscName )     // 이름이 잡화 종류라면 
        {
            // 인벤토리의 잡화목록에 해당 아이템 리스트가 없다면 리스트를 새롭게 생성합니다
            if( !inventory.miscDic.ContainsKey( itemName ) )
                inventory.miscDic[itemName]=new List<GameObject>(); 
            
            // 인벤토리의 해당 딕셔너리에 접근하여 아이템 오브젝트 리스트 참조를 설정합니다
            itemObjList = inventory.miscDic[itemName];

            if(itemObjList.Count > 0 )      // 들어가있는 오브젝트가 하나라도 있다면 (중복수량을 체크해야 합니다)
            {
                int remainCount = count;    // 채워야할 나머지 수량을 설정합니다


                // 리스트에 들어있는 잡화 게임오브젝트를 읽어들여서 남은수량이 0이될 때까지 최대 수량으로 채워줍니다.
                for(int i=0; i<itemObjList.Count; i++) 
                {
                    //Debug.Log("------기존 잡화아이템이 있는경우------");

                    // 해당 오브젝트의 개념아이템 정보를 뽑아옵니다
                    ItemMisc itemMisc = (ItemMisc) itemObjList[i].GetComponent<ItemInfo>().Item; 

                    // SetOVerlapCount메서드를호출하여 최대수량까지 채우고 나머지를 새롭게 반환받습니다.
                    remainCount = itemMisc.SetOverlapCount(remainCount);    
                    
                    // 최대수량이 변동되었으므로 이를 오브젝트 상에 반영합니다.
                    itemObjList[i].GetComponent<ItemInfo>().UpdateCountTxt();


                    //반환값이 더이상 없다면 for문을 빠져나갑니다. (i순서로 오브젝트의 수량을 채웁니다.)
                    if( remainCount==0 )
                        break;
                } // End of For Statement


                if( remainCount>0 )   // 반환값이 남아있다면,
                {            
                    //Debug.Log("------기존 잡화아이템에다가 들어온 수량을 다 못채운 경우------");
                    // 남은 수량을 인자로 넣어서 다시 아이템을 생성하는 재귀호출에 들어갑니다
                    return CreateMiscItemRepeatly(inventory, itemName, remainCount);
                }


            }
            else if(itemObjList.Count == 0)     //들어가있는 오브젝트가 하나도 없다면 (바로위의 for문이 한번도 돌아가지 않았다면)
            {                
                //Debug.Log("------새로운 잡화아이템에다가 수량을 채워야하는 경우------");
                //Debug.Log("인자로 들어온 count : "+ count);
                itemClone=(ItemMisc)miscDic[itemName].Clone();    // 개념아이템을 클론하고,
                ( (ItemMisc)itemClone ).SetOverlapCount( count );   // 클론의 중첩 갯수를 받은 인자로 지정합니다.

                //Debug.Log("Item에 등록된 Count : " + ((ItemMisc)itemClone).OverlapCount);

                // 인벤토리 리스트에 개념아이템을 장착한 오브젝트를 추가합니다
                AddCloneItemToInventory( itemObjList, itemClone, findSlotIdx );
            }

        }
        else // 어떤 이름도 일치하지 않는다면 예외를 발생 시킵니다
        {
            throw new Exception("생성 할 아이템 명이 정확하게 일치하지 않습니다. 확인하여 주세요.");
        }

        
        // 아이템 생성이 성공한 경우 0을 반환합니다
        return 0;
    }




    /// <summary>
    /// 잡화 아이템의 수량을 인자로 넣어서 계속해서 인벤토리에 생성해주는 메서드입니다.<br/>
    /// 남은 수량이 0이될 때까지 재귀호출을 반복합니다.
    /// </summary>
    /// <param name="inventory">생성할 인벤토리</param>
    /// <param name="itemName">잡화아이템 이름</param>
    /// <param name="remainCount">남은 수량</param>
    /// <returns></returns>
    private int CreateMiscItemRepeatly(Inventory inventory, string itemName, int remainCount)
    {        
        if(remainCount==0)  // 인자로 호출 된 수량이 0이라면 더이상 재귀호출을 진행하지 않고 0을 반환합니다. 
            return 0;

        int findSlotIdx = FindNearstRemainSlotIdx();
        int remCount;


        // 새롭게 개념아이템을 클론합니다.
        ItemMisc itemClone=(ItemMisc)miscDic[itemName].Clone();

        // 오브젝트리스트 설정
        List<GameObject> itemObjList = inventory.miscDic[itemName];


        // SetOVerlapCount메서드를호출하여 최대수량까지 채우고 나머지를 새롭게 반환받습니다.
        remCount = ( (ItemMisc)itemClone ).SetOverlapCount( remainCount );                                               
                                                                       
        // 인벤토리 리스트에 개념아이템을 장착한 오브젝트를 추가합니다
        AddCloneItemToInventory( itemObjList, itemClone, findSlotIdx );
        

        // 남은 수량을 새로운 인자로 넣어서 재귀호출을 진행합니다.
        return CreateMiscItemRepeatly(inventory, itemName, remCount);
    }



    /// <summary>
    /// 인자로 전달한 게임오브젝트 리스트에 게임오브젝트를 생성하여 아이템 정보를 추가한 상태로 넣어줍니다.
    /// </summary>
    /// <param name="itemObjList">인벤토리의 게임오브젝트가 담긴 리스트</param>
    /// <param name="itemClone">추가 할 아이템 인스턴스</param>
    /// <param name="findSlotIdx">추가 할 슬롯 인덱스</param>
    private void AddCloneItemToInventory( List<GameObject> itemObjList, Item itemClone, int findSlotIdx )
    {
        // 개념 아이템의 슬롯의 인덱스 정보를 찾은 위치로 수정한다.
        itemClone.SlotIndex=findSlotIdx;

        // 슬롯리스트의 해당 슬롯에 게임 상에서 보여 질 오브젝트를 생성하여 배치한다.
        GameObject itemObject = Instantiate( itemPrefab, slotListTr.GetChild( findSlotIdx ) );

        // 스크립트 상의 item에 사전에서 클론한 아이템을 참조하게 한다. (내부적으로 OnItemChanged()메서드가 호출되어 자동으로 오브젝트에 정보를 동기화)       
        itemObject.GetComponent<ItemInfo>().Item=itemClone;

        itemObjList.Add( itemObject );    // 인벤토리는 GameObject 인스턴스를 보관함으로서 transform정보와 개념 아이템을 정보를 포함하게 된다.            
        
        itemObject.GetComponent<ItemInfo>().OnItemChanged();    //아이템 수정사항을 반영해줍니다.
    }


    
    /// <summary>
    /// 개념 아이템 정보가 이미 있다면 이를 가지고 그에 맞는 아이템 오브젝트를 만들어 줍니다.
    /// </summary>
    /// <param name="item"></param>
    public GameObject CreateItemByInfo(Item item)
    {
        GameObject itemObject = Instantiate(itemPrefab);    // 프리팹하나를 복제해 옵니다.

        itemObject.GetComponent<ItemInfo>().Item = item;    // 아이템 정보 할당이 이루어질 때 자동으로 이미지,수량,위치가 동기화 됩니다.

        return itemObject;
    }    



    /// <summary>
    /// 남아있는 슬롯 중에서 가장 작은 인덱스를 반환합니다. 슬롯이 꽉 찬경우 -1을 반환합니다.
    /// </summary>
    public int FindNearstRemainSlotIdx() 
    {
        int findIdx = -1;

        for( int i = 0; i<slotListTr.childCount; i++ )
        {
            if( slotListTr.GetChild(i).childCount!=0 )  // 해당 슬롯리스트에 자식이 있다면 다음 슬롯리스트로 넘어간다.
                continue;

            findIdx = i;
            break;
        }

        return findIdx;     // findIdx가 수정되지 않았다면 -1을 반환한다. 수정되었다면 0이상의 인덱스값을 반환한다.
    }







    /// <summary>
    /// 월드의 모든 개념 아이템을 담아두는 딕셔너리를 초기화합니다.
    /// </summary>
    private void LoadAllItemDictionary()
    {
        // 플레이어와 상관없이 게임 시스템 자체가 들고 있어야할 집합이며,
        // 플레이어는 아이템이 생성될 때 이 집합에서 복제해서 들고있게 될 것이다.

        WorldItem worldItemData = new WorldItem();
        miscDic = worldItemData.miscDic;
        weaponDic = worldItemData.weaponDic;
    }








    

    

}
