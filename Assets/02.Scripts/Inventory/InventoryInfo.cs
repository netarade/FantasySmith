using UnityEngine;
using InventoryManagement;
using DataManagement;
using ItemData;
using System;
using System.Collections.Generic;
using CreateManagement;

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
 * 2- DataManager 인스턴스 생성방식에서 컴포넌트 참조 방식으로 변경
 * (멀티에서 스크립트마다 인스턴스 생성을 방지)
 * 
 * 3- 오버로딩메서드 isLatestReduce 변수명을 isLatestModify로 변경 (Inventory클래스와 통일)
 * 
 * 4- UpdateAllItemInfo메서드 새롭게 구현
 * 로드 시 모든 아이템의 오브젝트 정보를 최신화하기 위해 호출
 * 
 * <7.1 - 2024_0104_최원준>
 * 1- IsItemEnough에서 ItemPair 구조체하나만 받는 오버로딩 메서드 삭제
 * (오브젝트 1개의 이름과 수량을 전달하는 메서드가 따로 있기 때문이고, 
 * AddItem메서드에서 ItemPiar배열과 ItemPair를 받는 구조체를 추가하다보면 코드가 길어지기 때문)
 * 
 * <v7.2 - 2024_0105_최원준>
 * 1- 아이템 하나의 정보를 업데이트하는 UpdateItemInfo메서드 삭제
 * 아이템 추가 및 삭제 등의 변동이 일어날 때 ItemInfo 참조를 통해 업데이트 메서드를 호출하면되며,
 * 슬롯간 드랍도 itemInfo 참조로 업데이트를 호출해줄 것이기 때문
 * 
 * 2- 스타트에서 인벤토리를 로드할 때 Deserialize메서드 호출 시 createManager참조값을 전달하도록 수정
 * 이는 인벤토리 클래스가 직접 createManager를 찾을 수 없기 때문
 * 
 * 3- UpdateAllItemInfo메서드에서
 * 초기 딕셔너리에 값이 없는 경우에도 Update메서드를 호출하던 점을 수정
 * 
 * 4- IsSlotEnough에서 ItemType만 받는 오버로딩 메서드를 IsSlotEnoughIgnoreOverlap로변경,
 * itemName과 overlapCount를 받는 메서드를 IsSlotEnough로 변경
 * ItemInfo를 직접받는 IsSlotEnough추가
 * 
 * 5- SetItemSlotIdxBothToNearstSlot내부에서 IsSlotEnough의 ItemType기반 호출에서 ItemInfo기반 호출로 변경
 * -> 잡화아이템의 경우 슬롯인덱스가 필요하지 않은 경우가 있다.
 * 
 * (이슈)
 * 현재 아이템을 넣고 인덱스 정보를 넣어주는 구조에서
 * 인덱스 정보를 먼저 구해서 넣어주고, 아이템을 넣어주는 구조로 변경하게 되었다.
 * 따라서 잡화아이템의 인덱스 정보를 구할 때는 빈슬롯 기준으로 구하게 되는데,
 * 이를 기존 슬롯에 완전히 들어가는지 여부를 확인하고 슬롯이 꽉찼을 때 -1을 반환하지 않도록 해야 한다.
 * => (정정) 잡화 아이템이 처음if문에서 SlotEnough문을 통과하여 인덱스 정보가 -1로 잡혀도
 * AddItem할 때 파괴되므로 상관없어보인다.
 * 
 * <v7.3 - 2024_0108_최원준>
 * 1- 인스펙터뷰 상에서 지정하는 옵션인 baseDropTr, isBaseDropSetParent 변수 추가
 * 사용자가 드롭위치를 드래그앤 드롭으로 지정하여 OnItemWorldDrop메서드를 인자 없이 호출 시 편리하게 드랍하도록 하기위함.
 * 
 * <v7.4 - 2024_0109_최원준>
 * 1- IsSlotEnough주석 수정
 * 
 * <v8.0 - 2024_0110_최원준>
 * 1- 세이브 파일 이름을 Awake문에서 한번 세팅하지만 실제 로드 시점에서는 다시 초기화되어있는 문제가 있어서
 * 세이브, 로드 메서드 호출 바로 전에 FileSettings를 호출해주는 것으로 변경
 * 
 * 2- 세이브 파일 이름을 인벤토리의 최상위 부모오브젝트명+Inventory로 변경 
 * 
 * 3- 인벤토리 창의 활성화 여부를 알려주는 변수및 프로퍼티 IsWindowOpen 선언 후 
 * UpdateOpenState를 interactive 클래스에서 내부적으로 호출하도록 변경
 * 
 * 4- LoadPlayerData메서드를 Start문이 아니라 Awake문 호출로 변경.
 * LoadData메서드 내부에서 호출되던 UpdateAllItemInfo를 Awake문 외부로 빼내었음.
 * 
 * 5- 변수 명 UpdateAllItemInfo를 UpdateAllItemVisualInfo로 수정하였음.
 * 
 * 
 * 5- 슬롯의 생성을 interactive 스크립트에 맞김으로서 메서드 호출관계를 재정립
 * info클래스 인벤토리 정보 로드 이후-> interactive 클래스에서 슬롯 생성-> 아이템의 모습을 슬롯에 표현
 * (즉, 로드 이후 슬롯을 생성해야 하며, 슬롯이 생성 된 후 아이템의 모습을 표현해야 하기 때문)
 * 
 * 6- interactive 스크립트의 호출을 반드시 Info스크립트에서 하도록 변경
 * (interactive 스크립트는 이벤트 방식으로 동작하기 때문에 Awake문 초기화가 필요없기 때문 + Info스크립트 의존성을 해결)
 * 
 * 7- Load메서드를 Awake에서 Start로 수정하였음.
 * 이유는 로드 하면서 다른 스크립트의 메서드를 호출하는데 초기화가 이루어지지 않아서 에러가 발생하기 때문
 * (DataManager의 Path, CreateManager의 itemPrefab3D, VisualManager 등..)
 * 
 * <v8.1 - 2024_0112_최원준>
 * 1- 변수및 프로퍼티명 isWindowOpen을 isOpen으로 변경
 * 
 * <v8.2 - 2024_0113_최원준>
 * 1- 관련성을 이유로 isOpen 변수를 InventoryInfo_3.cs로 이전
 * 
 * 2- UpdateOpenState메서드 삭제
 * Interactive클래스의 InventoryOpenSwitch메서드를 직접 Inventory_3.cs로 옮긴 관계로
 * 더 이상 interactive에서 isOpen변수를 업데이트 해줄 필요가 없게되었음.
 * 
 * <v9.0 -2024_0114_최원준>
 * 1- 자기 트랜스폼 캐싱 inventoryTr변수 추가 및 slotListTr public 삭제, emptyList추가
 * 
 * 2- Awake문에 Inventory_3.cs 관련 변수 초기화를 진행
 * 
 * 3- 상속스크립트 QuickSlot을 정의하고 관련속성을 상속받기위해 private 변수와 메서드를 protected처리
 * 
 * 4- OnDestroy에서 Save하던 코드를 삭제
 * 이유는 파괴시 아이템의 컴포넌트를 하나씩 가져와서 저장하려고 하면 이미 오브젝트가 파괴되었기 때문에 에러가 뜨기 때문
 * (씬 전환 버튼을 누르기 직전에 Save를 호출해줘야 함)
 * 
 * 5- 메서드명 SetItemSlotIdxBothNearst를 SetItemSlotIndexBothLatest로 변경 후 Inventory클래스로 옮김
 * 이유는 AddItem 할 때 한동작으로 이루어져야 하며, 내부 inventory에서 밖에 계산할 수 없기 때문. 
 * 
 * 6- IsSlotEnough에 지정 슬롯인덱스를 받아서 확인하는 오버로딩 메서드 추가
 * 
 * 7- IsSlotEnoughIgnoreOverlap메서드 삭제
 * 
 * <v9.1 -2024_0115_최원준>
 * 1- interactive 변수 protected에서 public으로 변경
 * 이유는 각 info클래스에서 빠르게 접근하여 actvieTab값을 얻어오기 위함
 * 
 * 2- FindNearstRemainSlotIdx메서드 삭제
 * 인덱스 정보는 내부 inventory에서 처리해야 정확하기 때문
 * 
 * 3- IsSlotEnough 지정슬롯 인덱스를 인자로 받는 오버로딩 메서드명을 IsSlotEnoughCertain로 수정
 * 
 * <v9.2 - 2024_0115_최원준>
 * 1- IsSlothEnoughCertain 메서드를 IsSlotEmpty로 변경, IsSlotEnough 주석 보완
 * 
 * <v9.3 - 2024_0116_최원준>
 * 1- UpdateAllItemVisualInfo메서드 내부 for문에서 ItemType 전체만큼 반복하며 i를 직접 집어넣어 사전을 반환받는 코드를
 * dicLen만큼 반복하여 dicType[i]를 통해 사전을 반환받도록 수정
 * (=> 기존에는 아이템 종류별 모든 사전을 보유하고 있었으나 설계 수정 후 필요한 사전만 보유하는 형태로 변경하였으므로)
 * 
 * <v9.4 - 2024_0116_최원준>
 * 1- UpdateDicItemPosition메서드에서 GetItemDic관련 null값 검사문 추가
 * 2- UpdateAllItemVisualInfo메서드에서 GetItemDic관련 null값 검사문 추가
 * 
 * <v10.0 - 2024_0124_최원준>
 * 1- Inventory클래스의 딕셔너리 저장형식을 GameObject기반에서 ItemInfo로 변경하면서 관련 메서드 수정
 * (UpdateAllItemVisualInfo, UpdateDicItemPosition)
 * 
 * 2- ownerTr변수와 OwnerId프로퍼티를 선언하여 인벤토리가 생성시 부모오브젝트명을 ID로 부여받을 수 있게 하였음.
 * (AddItem,RemoveItem등에서 아이템에 소유주를 결정하게 하는 용도로 사용)
 * 
 * 3- 메서드명 변경 SavePlayerData LoadPlayerData -> SaveOwnerData LoadOwnerData 
 * 
 * 
 * 4- Awake문에서 saveFileName을 초기화할 때 inventoryTr의 계층참조를 직접 접근하여 name을 받아서 초기화하던 부분을
 * OwnerId를 호출하여 초기화하는 것으로 수정
 * 
 * 5- InventoryInfo의 IsWorldPositioned속성을 추가
 * Initializer에서 IsWorldPositioned값을 받아서 InventoryInfo의 IsWorldPositioned속성이 결정되도록 하였음.
 * (월드상에 놓여지는 인벤토리인지 여부를 결정)
 * 
 * 6- UpdateViusalizationInfo메서드를 IsWorldPositioned 속성이 아닌 경우에만 호출하도록 변경
 * 이유는 월드 보관함의 경우 2D 이미지를 보여줄 필요가 없으며, 3D오브젝트를 생성해주는 메서드가 호출되어져야 하기 때문
 * 
 * <v10.1 - 2024_0124_최원준>
 * 1- 메서드명 변경 SaveOwnerData LoadOwnerData -> SaveInventoryData LoadInventoryData
 * 
 * 2- IsWorldPositioned 변수명을 isItem3dStore로 변경하였음.
 * 이유는 월드에 놓여지는 속성이 아니라 3D상태의 아이템을 저장하는 역할을 하는 인벤토리이기 때문
 * 
 * 3- OwnerId의 타입을 string에서 int형으로 변경 및 주석 수정
 * 이유는 순번대로 id를 증가시켜 부여하기 위함
 *
 * 4- UpdateAllItemTransformInfo메서드를 추가.
 * 3d상태의 아이템을 보관하는 인벤토리의 경우 로드되면 해당 위치정보로 아이템을 옮겨주어야 하기 때문.
 * 
 * 5- 동적으로 생성되는 인벤토리인지 판단하는 isInstantiated변수를 추가하고 initializer를 통한 초기화를 진행
 * 
 * 6- 저장파일이름을 나타내는 변수인 saveFileName의 초기화를 직접 최상위 오브젝트명과 "Inventory"를 붙여서 구분하던 것을
 * 최상위 오브젝트명과 initializer의 inventoryId를 전달받아 초기화하는 방식으로 변경.
 * (이유는 오브젝트 명이 동일할 수있기 때문)
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


    protected Transform inventoryTr;              // 자신의 트랜스폼 캐싱
    protected Transform slotListTr;               // 현재 인벤토리가 관리하는 슬롯 리스트의 Transform 정보입니다.
    protected Transform emptyListTr;              // 현재 인벤토리가 관리하는 빈 리스트의 Transform 정보입니다.

    protected InventoryInitializer initializer;   // 사용자가 정의한 방식으로 인벤토리의 초기화를 진행하기 위한 참조
    public InventoryInteractive interactive;      // 자신의 인터렉티브 스크립트를 참조하여 활성화 탭정보를 받아오기 위한 변수 선언
    protected DataManager dataManager;            // 저장과 로드 관련 메서드를 호출 할 스크립트 참조
    protected CreateManager createManager;        // 아이템 생성을 요청하고 반환받을 스크립트 참조
    
    [Header("이 인벤토리의 아이템 기본 드랍위치")]
    public Transform baseDropTr;                // 아이템을 기본적으로 떨어 트릴 위치를 인스펙터뷰에서 직접 지정
    
    [Header("기본 드랍위치 부모 지정 옵션")]
    public bool isBaseDropSetParent;            // 드롭장소에 부모 계층에 속할지 지정하는 옵션 (씬정리 용도 및 부모와 함께 움직이도록 하는 용도)
    
    protected string saveFileName;              // 저장파일 이름 설정


    protected bool isItem3dStore;               // 3d 상태의 아이템을 보관하는 인벤토리인지 여부
    protected bool isInstantiated;              // 게임 중에 동적으로 생성되는 인벤토리인지 여부
    
    protected Transform ownerTr;                // 인벤토리 소유자 정보
    protected int ownerId;                      // 인벤토리 소유자 고유 식별 번호
        

    /// <summary>
    /// 인벤토리의 소유자 정보를 반환합니다.<br/>
    /// 인벤토리의 계층 최상위 부모 오브젝트이며, 플레이어 인벤토리의 경우 해당 플레이어 오브젝트를 말합니다.
    /// </summary>
    public Transform OwnerTr { get { return ownerTr;}  }
        
    /// <summary>
    /// 인벤토리의 소유자를 식별할 수 있는 고유 식별 번호입니다.
    /// </summary>
    public int OwnerId { get { return ownerId; } }          


    protected virtual void Awake()
    {         
        inventoryTr = transform; 
        slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);
        emptyListTr = inventoryTr.GetChild(0).GetChild(1);
                          
        interactive = GetComponent<InventoryInteractive>(); 
        initializer = GetComponent<InventoryInitializer>();     // 자신 오브젝트의 스크립트 참조
                                                                                                               
        isItem3dStore = initializer.isItem3dStore;    // 월드 아이템 생성 인벤토리인지를 결정합니다.         
        isInstantiated = initializer.isInstantiated;  // 동적 생성되는 인벤토리인지를 결정합니다.  
        ownerTr = inventoryTr.parent.parent;          // 계층 최상위 부모가 소유자가 됩니다.
        
        Transform gameController = GameObject.FindWithTag("GameController").transform;
        dataManager = gameController.GetComponent<DataManager>();           // 게임컨트롤러 태그가 있는 오브젝트의 컴포넌트 참조
        createManager = gameController.GetComponent<CreateManager>();       // 데이터 매니저와 동일한 오브젝트의 컴포넌트 참조
               
        // 저장 파일이름을 이니셜라이저의 id와 일치 시킵니다
        saveFileName = ownerTr.name + initializer.inventoryId;  


        /*** Inventory_3.cs 관련 변수 ***/
        inventoryCG = GetComponent<CanvasGroup>();  // 인벤토리의 캔버스그룹을 참조합니다
        InitOpenState(false);                       // 인벤토리의 오픈상태를 꺼짐으로 만듭니다

        // 플레이어(서버)인벤토리가 체크되어있다면, 
        if( isServer )
        {
            clientInfo = new List<InventoryInfo>(); // 클라이언트 인벤토리를 담을 수 있는 리스트를 할당합니다.
            clientInfo.Add(this);                   // 연결 인벤토리 정보에 자신을 등록합니다.
            serverInfo = this;                      // 자기자신을 서버로 등록합니다.
        }
    }

    // dataManager와 createManager의 초기화가 이루어진 이후 로드해야함.
    protected virtual void Start()
    {        
        /** 호출 순서 고정: 로드->인터렉티브스크립트 초기화 및 슬롯생성요청->아이템표현 ***/
        this.LoadInventoryData();              // 저장된 플레이어 데이터를 불러옵니다. 
        interactive.Initialize(this);       // 인터렉티브 스크립트 초기화를 진행합니다.

        // 3D아이템을 보관하는 인벤토리라면 월드상에 생성된 아이템의 Transform 정보를 반영합니다.
        if(isItem3dStore)
            UpdateAllItemTransformInfo();   
        // 2D아이템을 보관하는 인벤토리라면 슬롯에 모든 아이템의 시각화를 진행합니다.
        else
            UpdateAllItemVisualInfo(); 
    }




    /// <summary>
    /// 게임 종료시 저장합니다.
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        SaveInvetoryData(); // 플레이어 데이터 저장
    }


    /// <summary>
    /// 인벤토리 관련 데이터를 불러옵니다
    /// </summary>
    protected void LoadInventoryData()
    {
        // 로드 할 파일명을 설정합니다
        dataManager.FileSettings(saveFileName); 

        // 파일에서 로드한 데이터한 변수에 저장합니다.
        InventorySaveData loadData = dataManager.LoadInventoryData(initializer);              
        
        // 역직렬화하여 게임 상의 인벤토리로 변환합니다.
        inventory=loadData.savedInventory.Deserialize(initializer, createManager);   
    }


    /// <summary>
    /// 인벤토리 관련 데이터를 저장합니다 
    /// </summary>
    protected void SaveInvetoryData()
    {        
        // 세이브 할 파일명을 설정합니다
        dataManager.FileSettings(saveFileName);   

        // 메서드 호출 시점에 다른 스크립트에서 save했을 수도 있으므로 새롭게 생성하지 않고 기존 데이터 최신화합니다
        InventorySaveData saveData = dataManager.LoadInventoryData(initializer);

        // 직렬화하여 저장 가능한 인벤토리로 변환합니다.
        saveData.savedInventory.Serialize(inventory);   
        
        // 파일을 저장합니다.
        dataManager.SaveData(saveData);
    }







    /// <summary>
    /// 인벤토리가 보유하고 있는 모든 사전의 아이템에 인벤토리 정보를 전달하여 이미지나 위치 정보 등을 최신화합니다.<br/>
    /// 2D 아이템을 보관하는 인벤토리에서 로드 이후 호출되어져야 하는 메서드입니다.<br/>
    /// </summary>
    protected void UpdateAllItemVisualInfo()
    {   
        Dictionary<string, List<ItemInfo>> itemDic;                     // 참조할 아이템 사전을 선언합니다.
            
        for(int i=0; i<inventory.dicLen; i++)                           // 인벤토리 사전의 갯수만큼 반복합니다.
        {
            itemDic =inventory.GetItemDic( inventory.dicType[i] );      // 아이템 종류에 따른 인벤토리의 사전을 할당받습니다.
                          
            // 아이템 사전이 없거나 리스트가 존재하지 않는다면 다음 사전을 참조합니다.
            if(itemDic==null || itemDic.Count==0)   
                continue;

            // 인벤토리 사전에서 ItemInfo를 하나씩 꺼내어 가져옵니다.
            foreach( List<ItemInfo> itemInfoList in itemDic.Values )    
            {
                foreach( ItemInfo itemInfo in itemInfoList )
                    itemInfo.OnItemAdded( this );    // 현재 인벤토리 참조값을 전달하여 OnItemAdded메서드를 호출합니다
            }
        }
    }



    /// <summary>
    /// 인벤토리가 보유하고 있는 모든 사전의 아이템 3D 오브젝트의 Transform 정보를 최신화합니다.<br/>
    /// 3D 아이템을 보관하는 인벤토리에서 로드 이후 호출되어져야 하는 메서드입니다.<br/>
    /// </summary>
    protected void UpdateAllItemTransformInfo()
    {
        Dictionary<string, List<ItemInfo>> itemDic;                     // 참조할 아이템 사전을 선언합니다.
            
        for(int i=0; i<inventory.dicLen; i++)                           // 인벤토리 사전의 갯수만큼 반복합니다.
        {
            itemDic =inventory.GetItemDic( inventory.dicType[i] );      // 아이템 종류에 따른 인벤토리의 사전을 할당받습니다.
                          
            // 아이템 사전이 없거나 리스트가 존재하지 않는다면 다음 사전을 참조합니다.
            if(itemDic==null || itemDic.Count==0)   
                continue;

            // 인벤토리 사전에서 ItemInfo를 하나씩 꺼내어 가져옵니다.
            foreach( List<ItemInfo> itemInfoList in itemDic.Values )    
            {
                foreach( ItemInfo itemInfo in itemInfoList )
                {
                    // 아이템의 STransform 정보를 불러와서 Transform에 동기화시켜줍니다.
                    itemInfo.SerializedTr.Deserialize( itemInfo.ItemTr );
                }
            }
        }
    }








    /// <summary>
    /// 인벤토리에 존재하는 해당 이름의 *기존* 잡화 아이템 수량을 증가시키거나 감소시킵니다.<br/>
    /// 인자로 해당 아이템 이름과 수량을 지정해야 합니다.<br/><br/>
    /// 아이템 수량을 감소시키려면 수량 인자로 음수를 전달하여야 하며,<br/>
    /// 기존 수량이 감소로 인해 0이되면 아이템이 인벤토리 목록에서 제거되며, 파괴됩니다.<br/><br/>
    /// 아이템 수량을 증가시키려면 수량 인자로 양수를 전달하여야 하며,<br/>
    /// 아이템 최대 수량 제한으로 인해 더 이상 수량을 증가시키기 못하는 경우는 나머지 초과 수량을 반환합니다.<br/><br/>
    /// 최신 순, 오래된 순으로 감소여부를 결정할 수있습니다. (기본값: 최신순)<br/><br/>
    /// ** 아이템 이름이 해당 인벤토리에 존재하지 않거나, 잡화아이템이 아닌 경우 예외를 발생시킵니다. **
    /// </summary>
    /// <returns></returns>
    public int SetItemOverlapCount( string itemName, int inCount, bool isLatestModify = true )
    {
        return inventory.SetOverlapCount(itemName, inCount, isLatestModify, null);
    }


    
    /// <summary>
    /// 아이템의 종류와 상관없이 아이템이 해당 수량 만큼 인벤토리에 존재하는지 여부를 반환합니다.<br/>
    /// 아이템 이름과 수량으로 이루어진 구조체 배열을 인자로 받습니다.<br/><br/>
    /// 일반 아이템은 오브젝트의 갯수를 의미하며, 잡화 아이템은 중첩수량을 의미합니다.<br/>   
    /// 해당 수량만큼 감소 및 파괴옵션을 지정할 수 있습니다. (기본값: 수량 1, 수량 감소 및 파괴 안함, 최신순 감소 및 파괴)<br/><br/>
    /// *** 수량 인자가 0이하라면 예외를 발생시킵니다. ***
    /// </summary>
    /// <returns>아이템이 존재하며 수량이 충분한 경우 true를, 존재하지 않거나 수량이 충분하지 않다면 false를 반환</returns>
    public bool IsItemEnough( ItemPair[] pairs, bool isReduceAndDestroy = false, bool isLatestModify = true )
    {
        return inventory.IsEnough(pairs, isReduceAndDestroy, isLatestModify);
    }



    
    /// <summary>
    /// 아이템의 종류와 상관없이 아이템이 해당 수량 만큼 인벤토리에 존재하는지 여부를 반환합니다.<br/>
    /// 아이템 이름과 수량을 인자로 받습니다.<br/><br/>
    /// 일반 아이템은 오브젝트의 갯수를 의미하며, 잡화 아이템은 중첩수량을 의미합니다.<br/>   
    /// 해당 수량만큼 감소 및 파괴옵션을 지정할 수 있습니다. (기본값: 수량 1, 수량 감소 및 파괴 안함, 최신순 감소 및 파괴)<br/><br/>
    /// *** 수량 인자가 0이하라면 예외를 발생시킵니다. ***
    /// </summary>
    /// <returns>아이템이 존재하며 수량이 충분한 경우 true를, 존재하지 않거나 수량이 충분하지 않다면 false를 반환</returns>
    public bool IsItemEnough( string itemName, int count=1, bool isReduceAndDestroy = false, bool isLatestModify=true )
    {
        return inventory.IsEnough(itemName, count, isReduceAndDestroy, isLatestModify);      
    }






    












    /// <summary>
    /// 이 인벤토리의 슬롯에 아이템을 원하는 수량만큼 생성했을 때 아무 슬롯에 들어갈 빈 자리가 있는지 여부를 반환합니다.<br/>
    /// 인자로 아이템 이름과 수량인자를 전달해야 합니다. (수량인자의 기본값은 1입니다.)<br/><br/>
    /// 
    /// 비잡화아이템의 경우 수량인자만큼 오브젝트를 갖습니다. (즉, 수량인자가 오브젝트의 개수를 말합니다.)<br/>
    /// 잡화 아이템의 경우 수량인자를 넣어도 최대 중첩수량에 도달하기 전까지는 오브젝트를 1개만 형성합니다. (즉, 수량인자는 중첩수량을 말합니다.)<br/><br/>
    /// </summary>
    /// <returns>아이템을 생성가능하다면 true를, 불가능하다면 false를 반환합니다.</returns>
    public bool IsSlotEnough(string itemName, int overlapCount=1)
    {
        ItemType itemType = createManager.GetWorldItemType(itemName);

        if( itemType==ItemType.Misc )
            return inventory.IsAbleToAddMisc( itemName, overlapCount ); 
        else
            return inventory.GetCurRemainSlotCount(itemType) >= overlapCount; // 남은 슬롯 칸 수가 overlapCount이상
    }

    /// <summary>
    /// 해당 아이템이 아무 슬롯에 들어갈 빈 자리가 있는지 여부를 반환합니다.<br/>
    /// 기본 아이템의 경우 슬롯여부에 따라 성공, 실패를 반환하며,<br/>
    /// 잡화 아이템의 경우 중첩되어서 슬롯이 필요없는 경우 슬롯에 빈자리가 없어도 성공을 반환할 수 있습니다.<br/>
    /// 기존 아이템 정보가 존재해야 합니다.
    /// </summary>
    /// <returns>아이템을 생성가능하다면 true를, 불가능하다면 false를 반환합니다.</returns>
    public bool IsSlotEnough(ItemInfo itemInfo)
    {
        ItemType itemType = itemInfo.Item.Type;

        if( itemType == ItemType.Misc )
        {
            ItemMisc itemMisc = (ItemMisc)itemInfo.Item;
            return inventory.IsAbleToAddMisc(itemMisc.Name, itemMisc.OverlapCount);
        }
        else
            return inventory.IsRemainSlotIndirect(itemType);  // 아무자리에 남은 슬롯 칸수가 있는지
    }




    /// <summary>
    /// 해당 아이템이 현재 탭의 특정 슬롯에 들어갈 수 있을 지 여부를 반환합니다.<br/>
    /// 아이템 정보와 슬롯의 지정 인덱스, 전체 탭 여부를 인자로 받습니다.
    /// </summary>
    /// <returns>슬롯에 빈자리가 없는 경우 false를, 빈자리가 있는 경우 true를 반환</returns>
    public bool IsSlotEmpty(ItemInfo itemInfo, int slotIndex, bool isActiveTabAll)
    {
        return inventory.IsRemainSlotDirect(itemInfo.Item.Type, slotIndex, isActiveTabAll);         
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
        // 인벤토리의 현재 활성화 탭종류와 일치하는 딕셔너리를 참조합니다.
        Dictionary<string, List<ItemInfo>> itemDic = inventory.GetItemDic(itemType);

        // 해당 종류의 사전이 존재하지 않거나 리스트가 존재하지 않는다면 바로 종료합니다. 
        if(itemDic==null || itemDic.Count==0)
            return;


        foreach( List<ItemInfo> itemInfoList in itemDic.Values )    // 해당 딕셔너리의 ItemInfo리스트를 가져옵니다.
        {
            foreach( ItemInfo itemInfo in itemInfoList )            // ItemInfo리스트에서 ItemInfo를 하나씩 가져옵니다.
                itemInfo.UpdatePositionInfo();                      // 활성화 탭 기반으로 해당 종류의 위치정보를 업데이트합니다.
        }
    }



}
