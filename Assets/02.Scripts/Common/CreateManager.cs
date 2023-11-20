using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using CraftData;
using System;
using UnityEngine.UI;
using DataManagement;
using UnityEngine.SceneManagement;

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
*/


/// <summary>
/// 아이템 생성에 관한 로직을 담당하는 클래스이며,<br/>
/// 아이템 생성을 원하는 시점에 싱글톤 인스턴스를 참조하여 관련 메서드를 호출하면 됩니다.
/// </summary>
public class CreateManager : MonoBehaviour
{
    public static CreateManager instance;       // 매니저 싱글톤 인스턴스 생성
       
    
    public Dictionary<string, Item> miscDic;                // 게임 시작 시 넣어 둘 전체 잡화아이템 사전 
    public Dictionary<string, Item> weaponDic;              // 게임 시작 시 넣어 둘 전체 무기아이템 사전

    [SerializeField] Transform slotListTr;                  // 인벤토리의 슬롯 오브젝트의 트랜스폼 참조
    [SerializeField] GameObject itemPrefab;                 // 리소스 폴더 또는 직접 드래그해서 복제할 오브젝트를 등록해야 한다.        
    

    public void Awake()
    {        
        if( instance==null )
        {
            instance=this;
            DontDestroyOnLoad( instance );
        }
        else if( instance!=null )
            Destroy( this.gameObject );           // 싱글톤 이외 인스턴스는 삭제

        itemPrefab=Resources.Load<GameObject>( "ItemOrigin" );  // 리소스 폴더에서 원본 프리팹 가져오기
        slotListTr=GameObject.Find( "SlotList" ).transform;     // 슬롯리스트를 인식 시켜 남아있는 슬롯을 확인할 것이다.


        // 모든 월드 아이템 등록
        CreateAllItemDictionary();

        SceneManager.sceneLoaded += OnSceneLoaded; // 씬이로드될때 새롭게 참조를 잡아준다.
                
    }


    /// <summary>
    /// 씬이 전환되었을 때 이벤트 호출로 다시 참조를 잡기 위한 로직
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        itemPrefab=Resources.Load<GameObject>( "ItemOrigin" );  // 리소스 폴더에서 원본 프리팹 가져오기
        slotListTr=GameObject.Find( "SlotList" ).transform;     // 슬롯리스트를 인식 시켜 남아있는 슬롯을 확인할 것이다.
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
    /// 현재 인벤토리 슬롯에 빈자리가 없다면 false를 반환합니다.<br/>
    /// * 이름이 일치하지 않을 경우 예외를 발생시킵니다. *
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inventoryList">플레이어의 인벤토리 변수가 필요합니다. 해당 리스트에 아이템 오브젝트를 추가하여 줍니다.</param>
    /// <param name="itemName">아이템 테이블을 참고하여 한글명을 기입해주세요. 띄어쓰기에 유의해야 합니다. ex)철, 철 검, 미스릴, 미스릴 검</param>
    /// <param name="count">생성을 원하는 갯수입니다. 하나의 슬롯에 중첩하여 생성될 것입니다. 기본 값은 1이며, 장비류는 반드시 1개만 생성됩니다.</param>
    /// <returns></returns>
    public bool CreateItemToNearstSlot(List<GameObject> inventoryList, string itemName, int count = 1)
    {
        int findSlotIdx = FindNearstRemainSlotIdx();

        if( findSlotIdx == -1 ) // 인벤토리의 남아있는 칸이 없다면 생성하지 못한다.
            return false;
                
        
        // 사전의 개념 아이템을 클론하여 (아이템 원형을 복제해서) 또 다른 개념 아이템을 생성한다.
        Item itemClone = null;

        if( weaponDic.ContainsKey( itemName ) )                     // 무기라면 무조건 복제한다. (중첩될 일이 없으므로)
        {
            itemClone=(ItemWeapon)weaponDic[itemName].Clone();
        }
        else if( miscDic.ContainsKey( itemName ) )                  // 잡화라면 
        {  
            for( int i = 0; i<inventoryList.Count; i++ )           // 아이템이 생성될 때마다 플레이어의 잡화 목록을 검사한다.
            {
                ItemMisc itemMisc = (ItemMisc)inventoryList[i].GetComponent<ItemInfo>().item;  // 해당 오브젝트의 개념아이템 정보를 뽑아옵니다


                if( itemMisc.Name==itemName )                  // 잡화 아이템의 이름이 일치한다면,
                {
                    itemMisc.InventoryCount += count;          // 중첩 횟수를 받은 인자 만큼 늘리고
                    inventoryList[i].GetComponent<ItemInfo>().UpdateCountTxt(); 
                    // 아이템 오브젝트에도 중첩 수를 반영합니다.
                    // (이 경우는 클론을 따로 만들지 않기 때문에 직접 오브젝트에 접근하여 업데이트 해주어야 합니다.)
                    break;                                     // 한번 걸리면 반복문을 탈출합니다.
                }
                else if( i==inventoryList.Count-1 )                        // 마지막 잡화목록에서도 일치하는 이름이 없다면,
                {
                    itemClone = (ItemMisc)miscDic[itemName].Clone();        // 개념아이템을 클론하고,
                    ( (ItemMisc)itemClone ).InventoryCount=count;           // 클론의 중첩 갯수를 받은 인자로 지정합니다.                    
                }
            }
            
            if(inventoryList.Count==0) // 잡화 인벤토리가 비었다면 (위의 for문이 돌아가지 않았다면)
            {
                itemClone = (ItemMisc)miscDic[itemName].Clone();        // 개념아이템을 클론하고,
                ( (ItemMisc)itemClone ).InventoryCount=count;           // 클론의 중첩 갯수를 받은 인자로 지정합니다.
            }
        }
        else // 어떤 이름도 일치하지 않는다면 예외를 발생 시킨다.
        {
            throw new Exception("생성 할 아이템 명이 정확하게 일치하지 않습니다. 확인하여 주세요.");
        }

        if( itemClone!=null )   // 클론이 성공적으로 복제되었다면, (잡화의 경우 기존의 아이템이 있다면 복제되지 않는다.)
        {
            // 개념 아이템의 슬롯의 인덱스 정보를 찾은 위치로 수정한다.
            itemClone.SlotIndex=findSlotIdx;

            // 슬롯리스트의 해당 슬롯에 게임 상에서 보여 질 오브젝트를 생성하여 배치한다.
            GameObject itemObject = Instantiate( itemPrefab, slotListTr.GetChild( findSlotIdx ) );

            // 스크립트 상의 item에 사전에서 클론한 아이템을 참조하게 한다. (내부적으로 OnItemChanged()메서드가 호출되어 자동으로 오브젝트에 정보를 동기화)       
            itemObject.GetComponent<ItemInfo>().Item=itemClone;
                              
            inventoryList.Add( itemObject );    // 인벤토리는 GameObject 인스턴스를 보관함으로서 transform정보와 개념 아이템을 정보를 포함하게 된다.
            
            print("메서드에서의 리스트 갯수: " + inventoryList.Count);
            print("매니저에서의 리스트 갯수: " + PlayerInven.instance.inventory.miscList.Count);
        }

        return true;
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
    private void CreateAllItemDictionary()
    {
        // 플레이어와 상관없이 게임 시스템 자체가 들고 있어야할 집합이며,
        // 플레이어는 아이템이 생성될 때 이 집합에서 복제해서 들고있게 될 것이다.

        miscDic=new Dictionary<string, Item>()
        {           //0x1000
            { "철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000000", "철", 3.0f, new ImageReferenceIndex(0) ) },
            { "강철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000001", "강철", 5.0f, new ImageReferenceIndex(1) ) },
            { "흑철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000002", "흑철", 7.0f, new ImageReferenceIndex(2) ) },
            { "미스릴", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000003", "미스릴", 20.0f, new ImageReferenceIndex(3) ) }, 
            { "코발트", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000004", "코발트", 16.0f, new ImageReferenceIndex(4) ) },
            { "티타늄", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000005", "티타늄", 25.0f, new ImageReferenceIndex(5) ) },
            { "오리하르콘", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000006", "오리하르콘", 60.0f, new ImageReferenceIndex(6) ) },
            { "단단한 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000007", "단단한 나뭇가지", 2.0f, new ImageReferenceIndex(7) ) },
            { "튼튼한 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000008", "튼튼한 나뭇가지", 3.0f, new ImageReferenceIndex(8) ) },
            { "가벼운 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000009", "가벼운 나뭇가지", 4.0f, new ImageReferenceIndex(9) ) },
            { "부드러운 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000010", "부드러운 나뭇가지", 5.0f, new ImageReferenceIndex(10) ) },
            { "엘프의 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000011", "엘프의 나뭇가지", 20.0f, new ImageReferenceIndex(11) ) },
            { "축복받은 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000012", "축복받은 나뭇가지", 40.0f, new ImageReferenceIndex(12) ) },

            { "하늘의 룬", new ItemMisc( ItemType.Misc, MiscType.Additive, "0000300", "하늘의 룬", 100.0f, new ImageReferenceIndex(0) ) },
            { "불타는 심장", new ItemMisc( ItemType.Misc, MiscType.Additive, "0000301", "불타는 심장", 100.0f, new ImageReferenceIndex(1) ) },
            { "영롱한 구슬", new ItemMisc( ItemType.Misc, MiscType.Additive, "0000302", "영롱한 구슬", 100.0f, new ImageReferenceIndex(2) ) },
            { "요정의 목화", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000303", "요정의 목화", 100.0f, new ImageReferenceIndex(3) ) }, 
            { "달의 조각", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000304", "달의 조각", 30.0f, new ImageReferenceIndex(4) ) },
            { "신비한 조각", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000305", "신비한 조각", 30.0f, new ImageReferenceIndex(5) ) },
            { "크롬 결정", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000306", "크롬 결정", 25.0f, new ImageReferenceIndex(6) ) },
            { "수은 결정", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000307", "수은 결정", 20.0f, new ImageReferenceIndex(7) ) },
            { "얼음 결정", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000308", "얼음 결정", 15.0f, new ImageReferenceIndex(8) ) },
            { "백금", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000309", "백금", 60.0f, new ImageReferenceIndex(9) ) },
            { "흑요석", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000310", "흑요석", 40.0f, new ImageReferenceIndex(10) ) },
            { "금", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000311", "금", 30.0f, new ImageReferenceIndex(11) ) },
            { "은", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000312", "은", 20.0f, new ImageReferenceIndex(12) ) },
            { "점토", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000313", "점토", 2.0f, new ImageReferenceIndex(13) ) },
            { "솜뭉치", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000314", "솜뭉치", 1.0f, new ImageReferenceIndex(14) ) },
            { "물소의 뿔", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000315", "물소의 뿔", 15.0f, new ImageReferenceIndex(15) ) },
            { "짐승 가죽", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000316", "짐승 가죽", 6.0f, new ImageReferenceIndex(16) ) },
            
            { "초급 물리의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000700", "초급 물리의 각인", 50.0f, new ImageReferenceIndex(0) ) },
            { "초급 공속의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000701", "초급 공속의 각인", 50.0f, new ImageReferenceIndex(1) ) },
            { "초급 흡혈의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000702", "초급 흡혈의 각인", 50.0f, new ImageReferenceIndex(2) ) },
            { "초급 사격의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000703", "초급 사격의 각인", 50.0f, new ImageReferenceIndex(3) ) },
            { "초급 피해의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000704", "초급 피해의 각인", 50.0f, new ImageReferenceIndex(4) ) },
            { "중급 물리의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000705", "중급 물리의 각인", 125.0f, new ImageReferenceIndex(0) ) },
            { "중급 공속의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000706", "중급 공속의 각인", 125.0f, new ImageReferenceIndex(1) ) },
            { "중급 흡혈의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000707", "중급 흡혈의 각인", 125.0f, new ImageReferenceIndex(2) ) },
            { "중급 사격의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000708", "중급 사격의 각인", 125.0f, new ImageReferenceIndex(3) ) },
            { "중급 피해의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000709", "중급 피해의 각인", 125.0f, new ImageReferenceIndex(4) ) },
            { "고급 물리의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000710", "고급 물리의 각인", 200.0f, new ImageReferenceIndex(0) ) },
            { "고급 공속의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000711", "고급 공속의 각인", 200.0f, new ImageReferenceIndex(1) ) },
            { "고급 흡혈의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000712", "고급 흡혈의 각인", 200.0f, new ImageReferenceIndex(2) ) },
            { "고급 사격의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000713", "고급 사격의 각인", 200.0f, new ImageReferenceIndex(3) ) },
            { "고급 피해의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000714", "고급 피해의 각인", 200.0f, new ImageReferenceIndex(4) ) },
            
            { "나무", new ItemMisc( ItemType.Misc, MiscType.Fire, "0000600", "나무", 1.0f, new ImageReferenceIndex(5) ) },
            { "석탄", new ItemMisc( ItemType.Misc, MiscType.Fire, "0000601", "석탄", 6.0f, new ImageReferenceIndex(6) ) },
            { "석유", new ItemMisc( ItemType.Misc, MiscType.Fire, "0000602", "석유", 15.0f, new ImageReferenceIndex(7) ) },
            { "속성석", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000610", "속성석", 500.0f, new ImageReferenceIndex(8) ) }, 
            { "강화석", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000611", "강화석", 50.0f, new ImageReferenceIndex(9) ) }, 
        };

        weaponDic=new Dictionary<string, Item>()
        {
            /*** 검 ***/
            { "철 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001000", "철 검", 10.0f, new ImageReferenceIndex(0)
                , ItemGrade.Low, 10, 100, 1.0f, 10, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("철",2)} 
                , new CraftMaterial[]{new CraftMaterial("점토",1)} 
                , Recipie.Eu) 
            },
            { "강철 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001001", "강철 검", 20.0f, new ImageReferenceIndex(1)
                , ItemGrade.Low, 12, 100, 1.0f, 18, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("강철",2), new CraftMaterial("철",1)}
                , new CraftMaterial[]{new CraftMaterial("점토",2)}
                , Recipie.Na)
            },
            { "미스릴 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001002", "미스릴 검", 40.0f, new ImageReferenceIndex(2) 
                , ItemGrade.Low, 18, 100, 1.0f, 10, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",2), new CraftMaterial("강철",1),new CraftMaterial("철",1)}
                , new CraftMaterial[]{new CraftMaterial("은",2)}
                , Recipie.Ma)
            
            },
            { "흑철 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001003", "흑철 검", 60.0f, new ImageReferenceIndex(3) 
                , ItemGrade.Low, 23, 100, 1.0f, 40, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("흑철",2), new CraftMaterial("강철",3)}
                , new CraftMaterial[]{new CraftMaterial("점토",3)}
                , Recipie.Ga)
            },
            { "프레첼", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001004", "프레첼", 80.0f, new ImageReferenceIndex(4) 
                , ItemGrade.Low, 25, 100, 1.0f, 20, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",2), new CraftMaterial("강철",2),new CraftMaterial("흑철",1)}
                , new CraftMaterial[]{new CraftMaterial("점토",2),new CraftMaterial("흑요석",1)}
                , Recipie.Da)
            },


            { "얼음칼날", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001100", "얼음칼날", 120.0f, new ImageReferenceIndex(5)
                , ItemGrade.Medium, 36, 150, 1.0f, 25, EnumAttribute.Water
                , new CraftMaterial[]{new CraftMaterial("미스릴",7)}
                , new CraftMaterial[]{new CraftMaterial("얼음 결정",5)}
                , Recipie.Da)
            },
            { "백은의 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001101", "백은의 검", 180.0f, new ImageReferenceIndex(6)
                , ItemGrade.Medium, 38, 150, 1.0f, 30, EnumAttribute.Wind
                , new CraftMaterial[]{new CraftMaterial("티타늄",2),new CraftMaterial("미스릴",4)}
                , new CraftMaterial[]{new CraftMaterial("백금",5)}
                , Recipie.Ga)
            },
            { "기사단의 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001102", "기사단의 검", 150.0f, new ImageReferenceIndex(7)
                , ItemGrade.Medium, 48, 150, 1.0f, 30, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("티타늄",1),new CraftMaterial("미스릴",3),new CraftMaterial("강철",2)}
                , new CraftMaterial[]{new CraftMaterial("금",3),new CraftMaterial("은",2)}
                , Recipie.Na)
            },
            { "아밍 소드", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001103", "아밍 소드", 110.0f, new ImageReferenceIndex(8) 
                , ItemGrade.Medium, 32, 150, 1.0f, 12, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("티타늄",1),new CraftMaterial("미스릴",3)}
                , new CraftMaterial[]{new CraftMaterial("금",2),new CraftMaterial("흑요석",2)}
                , Recipie.Ma)
            },
            { "하플랑", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001104", "하플랑", 100.0f, new ImageReferenceIndex(9) 
                , ItemGrade.Medium, 38, 150, 1.0f, 20, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",4),new CraftMaterial("강철",2)}
                , new CraftMaterial[]{new CraftMaterial("은",3),new CraftMaterial("크롬 결정",1)}
                , Recipie.Ee)
            },

            { "천공의 지배", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001200", "천공의 지배", 680.0f, new ImageReferenceIndex(10)
                , ItemGrade.High, 80, 200, 1.10f, 10, EnumAttribute.Wind
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",3),new CraftMaterial("티타늄",5),new CraftMaterial("미스릴",10)}
                , new CraftMaterial[]{new CraftMaterial("하늘의 룬",2), new CraftMaterial("수은 결정",5)}
                , Recipie.Da)
            },
            { "파멸의 불꽃", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001201", "파멸의 불꽃", 640.0f, new ImageReferenceIndex(11)
                , ItemGrade.High, 105, 200, 1.10f, 30, EnumAttribute.Fire
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",5),new CraftMaterial("티타늄",10),new CraftMaterial("미스릴",3)}
                , new CraftMaterial[]{new CraftMaterial("불타는 심장",1), new CraftMaterial("신비한 조각",5)}
                , Recipie.Ra)
            },
            { "듀란달", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001202", "듀란달", 480.0f, new ImageReferenceIndex(12) 
                , ItemGrade.High, 120, 200, 1.10f, 50, EnumAttribute.Wind
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",3),new CraftMaterial("티타늄",5),new CraftMaterial("흑철",8)}
                , new CraftMaterial[]{new CraftMaterial("금",2),new CraftMaterial("달의 조각",5)}
                , Recipie.Ma)
            },
            { "미스틸테인", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001203", "미스틸테인", 700.0f, new ImageReferenceIndex(13) 
                , ItemGrade.High, 95, 200, 1.10f, 25, EnumAttribute.Gold
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",5),new CraftMaterial("미스릴",5),new CraftMaterial("코발트",12)}
                , new CraftMaterial[]{new CraftMaterial("신비한 조각",5),new CraftMaterial("영롱한 구슬",2)}
                , Recipie.Ra)
            },
            { "크시포스", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001204", "크시포스", 350.0f, new ImageReferenceIndex(14) 
                , ItemGrade.High, 80, 200, 1.10f, 30, EnumAttribute.Earth
                , new CraftMaterial[]{new CraftMaterial("미스릴",13),new CraftMaterial("강철",6)}
                , new CraftMaterial[]{new CraftMaterial("은",2),new CraftMaterial("백금",2), new CraftMaterial("크롬 결정",3)}
                , Recipie.Ga)
            },

            /**** 활 ****/
            { "사냥꾼의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008000", "사냥꾼의 활", 10.0f, new ImageReferenceIndex(0)
                , ItemGrade.Low, 10, 100, 0.8f, 10, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("철",3), new CraftMaterial("단단한 나뭇가지",3)} 
                , null 
                , Recipie.Eu) 
            },
            { "롱 보우", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008001", "롱 보우", 20.0f, new ImageReferenceIndex(1)
                , ItemGrade.Low, 12, 100, 0.8f, 18, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("철",5), new CraftMaterial("부드러운 나뭇가지",2)}
                , new CraftMaterial[]{new CraftMaterial("물소의 뿔",2)}
                , Recipie.Na)
            },
            { "컴뱃 보우", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008002", "컴뱃 보우", 40.0f, new ImageReferenceIndex(2) 
                , ItemGrade.Low, 18, 100, 0.8f, 10, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("강철",5), new CraftMaterial("단단한 나뭇가지",2)}
                , null
                , Recipie.Ma)
            
            },
            { "양치기의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008003", "양치기의 활", 60.0f, new ImageReferenceIndex(3) 
                , ItemGrade.Low, 23, 100, 0.8f, 40, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("철",3), new CraftMaterial("가벼운 나뭇가지",2)}
                , new CraftMaterial[]{new CraftMaterial("솜뭉치",4)}
                , Recipie.Ga)
            },
            { "도망자의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008004", "도망자의 활", 80.0f, new ImageReferenceIndex(4) 
                , ItemGrade.Low, 25, 100, 0.8f, 20, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("철",4), new CraftMaterial("가벼운 나뭇가지",3)}
                , new CraftMaterial[]{new CraftMaterial("짐승가죽",2)}
                , Recipie.Da)
            },


            { "샤이닝 보우", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008100", "샤이닝 보우", 120.0f, new ImageReferenceIndex(5)
                , ItemGrade.Medium, 36, 150, 0.8f, 25, EnumAttribute.Water
                , new CraftMaterial[]{new CraftMaterial("티타늄",5),new CraftMaterial("단단한 나뭇가지",5)}
                , new CraftMaterial[]{new CraftMaterial("백금",3)}
                , Recipie.Da)
            },
            { "이글 보우", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008101", "이글 보우", 180.0f, new ImageReferenceIndex(6)
                , ItemGrade.Medium, 38, 150, 0.8f, 30, EnumAttribute.Wind
                , new CraftMaterial[]{new CraftMaterial("코발트",6),new CraftMaterial("튼튼한 나뭇가지",2)}
                , new CraftMaterial[]{new CraftMaterial("수은 결정",4)}
                , Recipie.Ra)
            },
            { "탄궁", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008102", "탄궁", 150.0f, new ImageReferenceIndex(7) 
                , ItemGrade.Medium, 48, 150, 0.8f, 30, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",5),new CraftMaterial("부드러운 나뭇가지",4)}
                , new CraftMaterial[]{new CraftMaterial("물소의 뿔",3)}
                , Recipie.Ma)
            },
            { "각궁", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008103", "각궁", 110.0f, new ImageReferenceIndex(8) 
                , ItemGrade.Medium, 32, 150, 0.8f, 12, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",4),new CraftMaterial("가벼운 나뭇가지",3)}
                , new CraftMaterial[]{new CraftMaterial("크롬 결정",5)}
                , Recipie.Ee)
            },
            { "추격자의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008104", "추격자의 활", 100.0f, new ImageReferenceIndex(9) 
                , ItemGrade.Medium, 38, 150, 0.8f, 20, EnumAttribute.None
                , new CraftMaterial[]{new CraftMaterial("흑철",3),new CraftMaterial("가벼운 나뭇가지",5)}
                , new CraftMaterial[]{new CraftMaterial("짐승 가죽",4)}
                , Recipie.Ga)
            },


            { "취풍 파르티아", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008200", "취풍 파르티아", 450.0f, new ImageReferenceIndex(10)
                , ItemGrade.High, 80, 200, 0.88f, 16, EnumAttribute.Wind
                , new CraftMaterial[]{new CraftMaterial("티타늄",7),new CraftMaterial("미스릴",13),new CraftMaterial("엘프의 나뭇가지",3)}
                , new CraftMaterial[]{new CraftMaterial("하늘의 룬",4), new CraftMaterial("크롬 결정",3)}
                , Recipie.Da)
            },
            { "피닉스의 속삭임", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008201", "피닉스의 속삭임", 640.0f, new ImageReferenceIndex(11)
                , ItemGrade.High, 105, 200, 0.88f, 28, EnumAttribute.Fire
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",3),new CraftMaterial("티타늄",14),new CraftMaterial("축복받은 나뭇가지",4)}
                , new CraftMaterial[]{new CraftMaterial("불타는 심장",1), new CraftMaterial("영롱한 구슬",3)}
                , Recipie.Ga)
            },
            { "태양의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008202", "태양의 활", 530.0f, new ImageReferenceIndex(12) 
                , ItemGrade.High, 120, 200, 0.88f, 50, EnumAttribute.Fire
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",7),new CraftMaterial("흑철",18),new CraftMaterial("엘프의 나뭇가지",5)}
                , new CraftMaterial[]{new CraftMaterial("불타는 심장",3),new CraftMaterial("하늘의 룬",2)}
                , Recipie.Ra)
            },
            { "장미의 신궁", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008203", "장미의 신궁", 480.0f, new ImageReferenceIndex(13) 
                , ItemGrade.High, 95, 200, 0.88f, 32, EnumAttribute.Earth
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",5),new CraftMaterial("코발트",14),new CraftMaterial("축복받은 나뭇가지",3)}
                , new CraftMaterial[]{new CraftMaterial("요정의 목화",7)}
                , Recipie.Na)
            },
            { "파마의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008204", "파마의 활", 420.0f, new ImageReferenceIndex(14) 
                , ItemGrade.High, 80, 200, 0.88f, 36, EnumAttribute.Gold
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",5),new CraftMaterial("미스릴",11),new CraftMaterial("축복받은 나뭇가지",4)}
                , new CraftMaterial[]{new CraftMaterial("신비한 조각",4),new CraftMaterial("영롱한 구슬",1)}
                , Recipie.Ma)
            }

        };    

    }








    

    

}
