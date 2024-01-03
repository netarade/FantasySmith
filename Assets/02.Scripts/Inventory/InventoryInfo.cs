using UnityEngine;
using InventoryManagement;
using DataManagement;
using ItemData;
using System;
using System.Collections.Generic;

/*
 * [작업 사항]  
 * <v1.0 - 2023_1106_최원준>
 * 1- 초기 클래스 정의
 * 제작관련 변수들을 보유하도록 설계, 싱글톤으로 접근성 보유
 * <v1.1 - 2023_1106_최원준>
 * 1- 제작목록을 List에서 클래스로 구조 변경으로 인한 수정
 * 2- 주석보완
 * 
 * <v1.2 - 2023_1106_최원준>
 * 1- Start의 로직을 OnEnable로 수정. 
 * 다른 스크립트에서 Start구문에서 instance에 접근하여 정보를 받아가고 있기 때문.
 * 2- 직렬화 안되는 문제가 발생하여 다시 OnEnable에서 Start로 수정.
 * 
 * <v2.0 - 2023_1107_최원준>
 * 1- 빠른 초기화가 이루어져야 스크립트에서 정보를 받아가므로 Awake문으로 옮김.
 * 대리자를 이용하여 연관성을 최소화하여 순서대로 초기화가 이루어지도록 함. 
 * 
 * <v2.1 - 2023_1108_최원준>
 * 1- 아이템이 씬이 넘어갈때 파괴되기 때문에 스크립트를 저장하려고 하였으나 스크립트 또한 파괴되는 문제 발생
 * => 딕셔너리를 이용해서 이름과 수량만 저장하고 새롭게 생성하는 대체 방식으로 수정. 
 * 
 * 2- 씬 로드 시 무조건 하나의 재료만 들어있는 문제 발생
 * => 아이템쪽에서 파괴될떄마다 딕셔너리를 생성했었기 때문에 CraftManager쪽에서 마지막에 한번 미리 생성해주도록 변경
 * 
 * <v3.0 - 2023_1120_최원준>
 * 1- CraftManager 클래스 및 파일명 변경 PlayerInven, 파일위치 이동 Common폴더 -> Player폴더
 * 
 * 2- PlayerInven 클래스 역할 설정
 * a. 플레이어의 인벤토리 데이터를 보관하는 역할
 * b. 게임시작 및 게임종료 시 플레이어 인벤토리를 로드 및 세이브해주며, 씬 전환시 인벤토리를 유지하게 해줘야 한다.
 * c. 실제 플레이어 오브젝트에 스크립트가 붙어있어야 한다. (플레이어가 들고있는 데이터모음이 될 것이다.)
 * d. 오브젝트가 파괴될 때(게임 종료 시)
 * 
 * 3- weapList, miscList를 제거 (inventory쪽에 기능을 추가하여 접근성을 높일 계획이므로)
 * 4- 기타 로직들 제거 및 주석처리 (미완료)
 * 5- UpdateInventoryText 메서드 주석처리 - inventory쪽에 해당 기능을 추가 할 예정이므로
 * 
 * <v3.1 - 2023_1122_최원준>
 * 1- 인벤토리 및 숙련사전 주석 수정 보완
 * 
 * <v4.0 - 2023_1216_최원준>
 * 1- slotListTr항목을 추가 - CreateManager에 PlayerInven 스크립트를 전달하여 참조시킬 필요성 때문
 * 2- 싱글톤 제거 - 다른 스크립트의 start 구문 등에서 참조를 할 때 싱글톤이 그보다 더 빠르게 초기화되지 않으면 안되는데
 * OnSceneLoad로 초기화를 하다보면 느리기 때문
 * 
 * <v4.1 - 2023_1217_최원준>
 * 1- 스크립트 간소화, LoadPlayerData, SavePlayerData 메서드 추가하여 
 * Awake,OnDestroy,OnApplicationQuit에서 호출되도록 수정
 * 
 * 2- 게임매니저 싱글톤에서 isNewGame상태를 읽어와서 새로운 인벤토리를 생성하는 구문 추가
 * 
 * <v4.2 - 2023_1221_최원준>
 * 1- OnApplicationQuit 메서드내부에 에디터와 프로그램 종료 로직을 넣었던 점 삭제 
 * 2- 게임매니저의 인스턴스로 isNewGame을 판별하던 구문을 PlayerPrefs의 키값 참조로 변경
 * 이유는 PlayerInven 스크립트가 Awake로 빠른 초기화를 해야하는데 싱글톤으로의 접근시점이 같아서 불러오기 어렵기 때문 
 * 3- 테스트용 키삭제 구문 PlayerPrefs.DeleteAll 추가
 * 
 * <v5.0 - 2023_1222_최원준>
 * 1- 클래스와 파일명을 PlayerInven에서 InventoryInfo로 수정 
 * (ItemInfo와 이름의 일관성을 맞추기 위함)
 * 
 * <v5.1 - 2023_1222_최원준>
 * 1- Awkae문에서 PlayerPrefs의 키값을 통해 처음시작과 이어하기를 구분해서 메서드를 호출하던 구문을 삭제하고
 * LoadData하나만 호출해도 처음 데이터가 만들어지도록 변경하였습니다.
 * 
 * <v5.2 - 2023_1224_최원준>
 * 1- InitPlayerData() 메서드 삭제
 * 2- LoadPlayerData() 메서드를 Start문으로 옮김
 * 이유는 인벤토리가 역직렬화 될 때 내부적으로 CreateManager의 싱글톤의 메서드를 불러오기 때문에 호출시점을 늦출 필요가 있음
 * 
 * <V5.3 - 2023_1226_최원준>
 * 1- 인벤토리 로드시 LoadAllItem메서드에서 UpdateItemInfo메서드 호출로 변경
 * <v5.4 - 2023_1226_최원준>
 * 1- 인벤토리 세이브 로드메서드를 일반화 메서드 호출로 변경
 * 
 * <v6.0 - 2023_1229_최원준>
 * 1- craftDic, gold, silver등 연관없는 변수 삭제 및 저장 로드도 inventory만 불러오도록 변경
 * 
 * <v6.1 - 2023_1230_최원준>
 * 1- slotListTr변수 추가 - 인벤토리가 자신의 슬롯목록 주소를 관리하도록 하였음
 * 
 * 2- InventoryInteractive에서 슬롯 프리팹을 생성하던 코드를 옮겨옴
 * 이유는 슬롯을 늘리거나 줄이거나 하는 메서드를 만들고, 정보를 반영하기 위해
 * 
 * 
 * <v6.2 - 2024_0101_최원준>
 * 1-Inventory의 AddItem, RemoveItem메서드를 추가 (내용 정의는 추가예정)
 * 내부 인벤토리를 숨김처리하고, 사용의 편리함을 주기위해
 * 
 * 2-Inventory의 FindNearstSlotIdx메서드 추가
 * (ItemInfo에서 자신이 속한 인벤토리의 가장 가까운 슬롯 인덱스를 반환받기 위해 필요)
 * 
 * 3- isSlotEnough메서드 추가
 * ItemInfo에서 인벤토리 정보를 업데이트 하기 이전에 먼저 들어갈 공간이 있는지를 확인하고 변경하는 과정이 필요
 * 
 * 
 * (수정 예정)
 * 1- 인벤토리의 정보가 수정되었음을 반영하는 이벤트 생성 및 호출
 * 인벤토리 정보를 참조하는 Interactive스크립트에서 사용할 수 있도록 하기 위하여
 * 
 * 
 * 
 * 
 * <v7.0 - 2024_0103_최원준>
 * 1- AddItem, RemoveItem, CreateItem메서드 Info2클래스로 분할처리
 * 
 * 2- 
 * 
 * 
 * 
 */



/// <summary>
/// 게임 실행 중 제작 관련 실시간 플레이어 정보 들을 보유하고 있는 전용 정보 클래스입니다.<br/>
/// 인스턴스를 생성하여 정보를 확인하시기 바랍니다.
/// </summary>
public partial class InventoryInfo : MonoBehaviour
{
    /// <summary>
    /// 플레이어가 보유하고 있는 아이템을 보관하는 인벤토리와 관련된 정보를 가지고 있는 클래스입니다.<br/>
    /// 인벤토리에 아이템을 생성하고 제거하거나 현재 아이템의 검색 기능 등을 가지고 있습니다.<br/>
    /// 딕셔너리 내부에 게임 오브젝트를 보유하고 있으므로 씬 전환이나 세이브 로드 시에 반드시 Item 형식의 List로의 Convert가 필요합니다.
    /// </summary>
    public Inventory inventory;

    /// <summary>
    /// 현재 인벤토리가 관리하는 슬롯 리스트의 Transform 정보입니다.
    /// </summary>
    public Transform slotListTr;
        
    private GameObject slotPrefab;              // 슬롯을 동적으로 생성하기 위한 프리팹 참조
    private InventoryInteractive interactive;   // 자신의 인터렉티브 스크립트를 참조하여 활성화 탭정보를 받아오기 위한 변수 선언

    void Awake()
    {        
        slotListTr = transform.GetChild(0).GetChild(0).GetChild(0);
        slotPrefab = slotListTr.GetChild(0).gameObject;
        
        // 플레이어 인벤토리 정보(전체 탭 슬롯 칸수)를 참조하여 슬롯을 동적으로 생성 (현재 인벤토리에 슬롯이 한 개 들어있으므로 하나를 감하고 생성)
        for( int i = 0; i<inventory.SlotCountLimitAll-1; i++ )
            Instantiate( slotPrefab, slotListTr );

        interactive = gameObject.GetComponent<InventoryInteractive>();  // 자신의 인터렉티브 스크립트를 참조합니다.
    }

    /// <summary>
    /// 인스턴스가 새롭게 생성될 때마다 저장된 파일을 불러옵니다.<br/>
    /// 인벤토리를 역직렬화 할 때 내부적으로 CreateManager의 싱글톤의 메서드를 호출하므로 Start문에서 플레이어 데이터를 불러옵니다.
    /// </summary>
    void Start()
    {        
        LoadPlayerData();       // 저장된 플레이어 데이터를 불러옵니다.        
    }
        

    /// <summary>
    /// 파괴 되기전에 파일에 저장합니다.
    /// </summary>
    private void OnDestroy()
    {
        SavePlayerData();   // 플레이어 데이터 저장
    }
       
    /// <summary>
    /// 플레이어가 게임을 종료할 때
    /// </summary>
    public void OnApplicationQuit()
    {
        SavePlayerData(); // 플레이어 데이터 저장
    }


    /// <summary>
    /// 인벤토리 관련 플레이어 데이터를 불러옵니다
    /// </summary>
    void LoadPlayerData()
    {
        DataManager dataManager = new DataManager("Inventory",0); //세이브 파일명, 슬롯번호
        InventorySaveData loadData = dataManager.LoadData<InventorySaveData>();
                
        inventory=loadData.savedInventory.Deserialize();
        inventory.UpdateAllItemInfo();
    }


    /// <summary>
    /// 인벤토리 관련 플레이어 데이터를 저장합니다 
    /// </summary>
    void SavePlayerData()
    {
        DataManager dataManager = new DataManager("Inventory",0); //세이브 파일명, 슬롯번호
        
        // 메서드 호출 시점에 다른 스크립트에서 save했을 수도 있으므로 새롭게 생성하지 않고 기존 데이터 최신화합니다
        InventorySaveData saveData = dataManager.LoadData<InventorySaveData>();

        saveData.savedInventory.Serialize(inventory);

        dataManager.SaveData<InventorySaveData>(saveData);
    }




    /// <summary>
    /// 인벤토리에 존재하는 해당 이름의 아이템 수량을 증가시키거나 감소시킵니다.<br/>
    /// 인자로 해당 아이템 이름과 수량을 지정해야 합니다.<br/><br/>
    /// 아이템 수량을 감소시키려면 수량 인자로 음수를 전달하여야 하며,<br/>
    /// 기존 수량이 감소로 인해 0이되면 아이템이 인벤토리 목록에서 제거되며, 파괴됩니다.<br/><br/>
    /// 아이템 수량을 증가시키려면 수량 인자로 양수를 전달하여야 하며,<br/>
    /// 아이템 최대 수량 제한으로 인해 더 이상 수량을 증가시키기 못하는 경우는 나머지 초과 수량을 반환합니다.<br/><br/>
    /// 최신 순, 오래된 순으로 감소여부를 결정할 수있습니다. (기본값: 최신순)<br/><br/>
    /// ** 아이템 이름이 해당 인벤토리에 존재하지 않거나, 잡화아이템이 아닌 경우 예외를 발생시킵니다. **
    /// </summary>
    /// <returns></returns>
    public int SetItemOverlapCount( string itemName, int inCount, bool isLatestReduce = true )
    {
        return inventory.SetOverlapCount(itemName, inCount, isLatestReduce);
    }


    /// <summary>
    /// 인벤토리에 해당 이름의 아이템의 중첩수량이 충분한지를 확인하는 메서드입니다.<br/>
    /// 인자로 아이템 이름과 수량이 필요합니다. (수량 미전달 시 기본 수량은 1개입니다.)<br/><br/>
    /// 세번 째 인자로 수량 감소모드를 선택하면 아이템의 중첩수량이 충분하다면 인자로 들어온 수량만큼 감소시키며, <br/>
    /// 0에 도달한 경우 아이템을 인벤토리에서 제거하고 파괴 시킵니다.<br/>
    /// 최신 순, 오래된 순으로 감소여부를 결정할 수있습니다. (기본값: 최신순)<br/><br/>
    /// ** 해당 이름의 아이템이 존재하지 않거나, 잡화 아이템이 아니거나, 수량을 잘못 전달했다면 예외를 발생시킵니다. **
    /// </summary>
    /// <returns>아이템 중첩수량이 충분하면 true를, 충분하지 않으면 false를 반환</returns>
    public bool IsItemEnoughOverlapCount( string itemName, int overlapCount = 1, bool isReduce = false, bool isLatestReduce = true )
    {
        return inventory.IsEnoughOverlapCount(itemName, overlapCount, isReduce, isLatestReduce);
    }

    /// <summary>
    /// 아이템이 인벤토리에 존재하는지 여부를 반환합니다.<br/>
    /// 최신 순, 오래된 순으로 삭제여부를 결정할 수있습니다. (기본값: 삭제안함, 최신순)<br/><br/>
    /// *** 아이템의 종류나 수량과 상관없이 오브젝트 단위로 존재하는지 여부만 반환합니다. ***<br/>
    /// </summary>
    /// <returns>아이템이 존재하지 않는 경우 false를, 아이템이 존재하거나 삭제에 성공한 경우 true를 반환</returns>
    public bool IsItemExist( string itemName, bool isRemove = false, bool isLatestRemove = true )
    {
        return inventory.IsExist(itemName, isRemove, isLatestRemove);
    }


    /// <summary>
    /// 아이템의 종류와 상관없이 아이템이 인벤토리에 존재하는지, 잡화아이템이라면 수량까지도 충분한지 여부를 반환합니다.<br/>
    /// 또한 아이템이 존재하거나, 잡화아이템인 경우 수량까지 충분하다면 제거 또는 감소를 결정할 수 있습니다.<br/>
    /// (기본값: 감소모드 안함, 최신순 감소)<br/><br/>
    /// *** 비잡화 아이템의 경우 수량 값을 무시합니다. 잡화아이템의 경우 수량이 1이상이 아니면 예외를 발생시킵니다. ***
    /// </summary>
    /// <returns>전달 한 인자의 모든 조건을 충족하는 경우 true를, 조건을 충족하지 않는 경우 false를 반환, 조건이 충족한 경우 감소를 수행</returns>
    public bool IsItemEnough( ItemPair pair, bool isReduce = false, bool isLatestReduce = true )
    {
        return inventory.IsEnough(pair, isReduce, isLatestReduce);
    }

    /// <summary>
    /// 아이템의 종류와 상관없이 아이템이 인벤토리에 존재하는지, 잡화아이템이라면 수량까지도 충분한지 여부를 반환합니다.<br/>
    /// 또한 아이템이 존재하거나, 잡화아이템인 경우 수량까지 충분하다면 제거 또는 감소를 결정할 수 있습니다.<br/>
    /// (기본값: 감소모드 안함, 최신순 감소)<br/><br/>
    /// *** 비잡화 아이템의 경우 수량 값을 무시합니다. 잡화아이템의 경우 수량이 1이상이 아니면 예외를 발생시킵니다. ***
    /// </summary>
    /// <returns>전달 한 인자의 모든 조건을 충족하는 경우 true를, 조건을 충족하지 않는 경우 false를 반환, 조건이 충족한 경우 감소를 수행</returns>
    public bool IsItemsEnough( ItemPair[] pairs, bool isReduce = false, bool isLatestReduce = true )
    {
        return inventory.IsEnough(pairs, isReduce, isLatestReduce);
    }



    







    /// <summary>
    /// 현재 활성화 중인 가장 가까이 있는 남은 슬롯의 인덱스를 반환합니다.<br/>
    /// 연산이 빠르며, 인자를 받지 않습니다.<br/><br/>
    /// *** 활성화 중인 탭의 인덱스 밖에 구할 수 없기 때문에 같은 슬롯간의 이동 시에 호출하는 용도로 사용합니다. ***<br/>
    /// </summary>
    /// <returns>활성화 중인 탭에서 비어있는 슬롯의 가장 작은 인덱스입니다. 남은 슬롯이 없다면 -1을 반환합니다.</returns>
    public int FindNearstRemainActiveSlotIdx()
    {
        int findIdx = -1;

        for( int i = 0; i<slotListTr.childCount; i++ )  // 슬롯의 인덱스 0번부터 봅니다
        {
            if( slotListTr.GetChild(i).childCount!=0 )  // 해당 슬롯리스트에 자식이 있다면 다음 슬롯리스트로 넘어갑니다.
                continue;

            findIdx = i;                                // 찾은 인덱스로 설정합니다.
            break;
        }

        // findIdx가 수정되지 않았다면 -1을 반환합니다. 수정되었다면 0이상의 인덱스값을 반환합니다.
        return findIdx;
    }

    /// <summary>
    /// 가장 가까운 슬롯의 인덱스를 구합니다. 어떤 종류의 아이템을 넣을 것인지 인자로 전달하여야 합니다.<br/>
    /// ItemType인자에 해당하는 개별탭 슬롯 인덱스를 반환합니다. ItemType.None을 전달할 경우 전체탭의 슬롯 인덱스를 반환합니다. (기본값: 전체탭)
    /// </summary>
    /// <returns>ItemType.None을 전달할 경우 전체탭 슬롯 인덱스를 반환하며, 이외의 인자는 개별탭 슬롯 인덱스를 반환합니다. 
    /// 슬롯에 자리가 없다면 -1을 반환합니다.</returns>
    public int FindNearstSlotIdx( ItemType itemType = ItemType.None )
    {
        return inventory.FindNearstSlotIdx(itemType);
    }
    
    /// <summary>
    /// 가장 가까운 슬롯의 인덱스를 구합니다. 어떤 이름의 아이템을 넣을 것인지와 <br/>
    /// 개별탭 혹은 전체탭의 인덱스를 반환받고자 하는지 여부를 전달하여야 합니다. (기본값: 전체탭)<br/>
    /// </summary>
    /// <returns>true을 전달할 경우 전체탭 슬롯 인덱스를 반환하며, false는 개별탭 슬롯 인덱스를 반환합니다. 
    /// 슬롯에 자리가 없다면 -1을 반환합니다.</returns>
    public int FindNearstSlotIdx( string itemName, bool isIndexAll=true)
    {
        return inventory.FindNearstSlotIdx(itemName, isIndexAll);
    }






    /// <summary>
    /// 이 인벤토리의 슬롯에 아이템이 들어갈 자리가 있는지 여부를 반환하는 메서드입니다.<br/>
    /// 인자로 아이템 종류를 전달하여야 합니다.
    /// </summary>
    /// <returns>슬롯이 자리가 남는다면 true를, 슬롯에 자리가 없다면 false를 반환합니다.</returns>
    public bool IsSlotEnough(ItemType itemType)
    {
        if(FindNearstSlotIdx(itemType) == -1)
            return false;
        else
            return true;        
    }

    /// <summary>
    /// 이 인벤토리의 슬롯에 아이템이 들어갈 자리가 있는지 여부를 반환하는 메서드입니다.<br/>
    /// 인자로 아이템 이름을 전달하여야 합니다.
    /// </summary>
    /// <returns>슬롯이 자리가 남는다면 true를, 슬롯에 자리가 없다면 false를 반환합니다.</returns>
    public bool IsSlotEnough(string itemName)
    {
        if(FindNearstSlotIdx(itemName, false)==-1)
            return false;
        else
            return true;
    }

    /// <summary>
    /// 이 인벤토리의 슬롯에 아이템이 들어갈 자리가 있는지 여부를 반환하는 메서드입니다.<br/>
    /// 인자로 아이템정보 스크립트를 전달하여야 합니다.
    /// </summary>
    /// <returns>슬롯이 자리가 남는다면 true를, 슬롯에 자리가 없다면 false를 반환합니다.</returns>
    public bool IsSlotEnough(ItemInfo itemInfo)
    {
        if(FindNearstSlotIdx(itemInfo.Item.Type) == -1)
            return false;
        else
            return true;  
    }







    /// <summary>
    /// 아이템 Info 컴포넌트를 인자로 받아서 어떤 슬롯에 들어갈지 최신화 해주는 메서드입니다. <br/>
    /// 내부적으로 FindNearstSlotIdx메서드를 호출하여 슬롯 정보를 입력해줍니다.<br/>
    /// </summary>
    /// <returns>슬롯에 빈자리가 없다면 false를 반환합니다. 아이템이 들어갈 공간이 없다는 뜻입니다.</returns>
    public bool SetItemSlotIdxBothToNearstSlot( ItemInfo itemInfo )
    {        
        // 호출 예외 항목 처리
        if( itemInfo==null )
            throw new Exception("ItemInfo 스크립트가 존재하지 않는 아이템입니다.");

        // 아이템 정보와 종류를 저장합니다.
        Item item = itemInfo.Item;
        ItemType itemType = item.Type;

        // 종류를 인자로 넣어서 개별 슬롯 인덱스를 반환 받습니다.
        int slotIdxEach = FindNearstSlotIdx(itemType);
        
        // 개별 슬롯 인덱스가 -1이라면 빈자리가 없는 것으로 판단하여 실패를 반환합니다.
        if(slotIdxEach==-1)     
            return false;        
        
        // 전체 슬롯 인덱스를 구합니다.
        int slotIdxAll = FindNearstSlotIdx(ItemType.None);;

        // 구한 슬롯 인덱스를 아이템 정보에 입력합니다.
        item.SlotIndex = slotIdxEach;
        item.SlotIndexAll = slotIdxAll;

        // 성공을 반환합니다.
        return true;
    }

    /// <summary>
    /// 특정 종류의 딕셔너리에 존재하는 아이템의 슬롯 정보를 업데이트해주는 메서드입니다.<br/>
    /// 아이템의 종류에 따른 딕셔너리를 참조하여 
    /// 해당 딕셔너리 내부의 모든 아이템을 대상으로 UpdatePositionInSlotList메서드를 호출합니다.<br/><br/>
    /// 현재는 interactive클래스에서 이 메서드를 활용하고 있습니다.<br/>
    /// </summary>
    /// <param name="itemType"></param>
    public void UpdateDicItemPosition(ItemType itemType)
    {            
        //인벤토리의 현재 활성화 탭종류와 일치하는 딕셔너리를 참조합니다.
        Dictionary<string, List<GameObject>> itemDic = inventory.GetItemDicIgnoreExsists(itemType);

        foreach( List<GameObject> itemObjList in itemDic.Values )  // 해당 딕셔너리의 오브젝트리스트를 가져옵니다.
        {
            foreach( GameObject itemObj in itemObjList )           // 오브젝트리스트에서 오브젝트를 하나씩 가져옵니다.
            {
                ItemInfo itemInfo = itemObj.GetComponent<ItemInfo>();  // 아이템 정보를 읽어들입니다.
                itemInfo.UpdatePositionInSlotList();                   // 활성화 탭 기반으로 해당 종류의 위치정보를 업데이트합니다.
            }
        }
    }



}
