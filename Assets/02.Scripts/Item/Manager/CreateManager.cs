using System.Collections.Generic;
using UnityEngine;
using ItemData;
using System;
using WorldItemData;
using DataManagement;

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
* <v11.0 - 2023_1228_최원준>
* 1- CreateManager클래스의 miscDic, WeaponDic의 변수명을 worldMiscDic worldWeapDic으로 변경
*  
* 2- Item프리팹의 계층구조 변경(3D오브젝트 하위에 2D오브젝트를 둠)으로 인해 ItemInfo를 GetComponent로 참조하던 것에서 GetComponentInChildren으로 변경
* 
* 3- Item프리팹 참조를 리소스폴더의 ItemOrigin에서 Item3D로 변경
* 
* 4- slotListTr을 Start문에서 참조하는것이 아니라 inventory를 인자로 받을때 생성할 위치정보도 같이 받아야할 필요성(추후구현예정)  
* 
* <v11.1 - 2023_1228_최원준>
* 1- Item프리팹 참조를 리소스폴더의 ItemOrigin에서 Item2D로 변경
* 
* <v11.2 - 2023_1230_최원준>
* 1- CreateItemToNearstSlot메서드의 이름변경 - CreateItemToInventorySlot
* 
* 2- CreateItemToInventorySlot메서드의 count인자를 overlapCount로 이름변경(오브젝트의 수량이 아닌 잡화아이템의 중첩수량 옵션이므로)
* 
* 3- CreateItemToInventorySlot메서드의 반환값을 int형의 갯수가 아니라 bool값 true, false로 변경
* 
* 4- CreateItemToInventorySlot메서드 인자를 InventoryInfo로 받을 수 있도록 변경
* 
* 
* 5- CreateItemToWorld 메서드 추가
* 인자로 월드의 Transform 정보를 전달받거나 Vector3값을 전달받도록 하였음.
* 
* 6- FindNearstRemainSlotIdx메서드 삭제.
* CreateManager클래스에서 매번 다른 인벤토리 정보를 받아야 하므로 InventoryInfo클래스에서 가지고 있도록 변경
* 
* <v11.3 - 2023_1231_최원준>
* 1- 메서드 CreateItemToInventorySlot를 CreateItemToInventory로 이름변경
* 
* <v12.0 - 2024_0103_최원준>
* 1- CreateManager 싱글턴 삭제, OnSceneLoad, Destroy문 삭제, CreateItemToWorld 메서드 삭제
* Awake에 모든 참조 설정으로 변경
* 
* <v12.1 - 2024_0105_최원준>
* 1- 월드 딕셔너리 정보를 빠르게 접근가능하게 해주고, 
* CreateManager에서는 아이템을 만들어서 ItemInfo의 참조값만 반환하는 형태로 구현
* 
* 2- 메서드들이 이름이 제대로 입력되지 않았을 때, null값 반환이 아니라 예외처리하도록 변경
* 3- dicType을 초기화 안하던점 수정
* 
* 4- CreateItem메서드에서 클론생성시 슬롯인덱스를 -1로 초기화하는 구문추가
* 인벤토리쪽에서 아이템을 집어넣고 슬롯인덱스를 기존의 아이템을 읽어들여서 인덱스를 보게되는데
* 큰값으로 초기화되거나 하면 안되므로 지정값 초기화
* 
* <v12.2 - 2024_0108_최원준>
* 1- CreateWorldItem메서드 내부에서 아이템 생성 시 이미지와 수량정보를 반영안해주고 있었던 점 수정
* OnItemCreatedInWorld 메서드를 호출해는 것으로 코드 변경
* 
* 2- 네임스페이스 VisualManger와 함께 CreateManagement에 속하도록 변경.
* 
* 3- itemPrefab 변수명을 itemPrefab2D로 변경
* 
* 4- 아이템 생성시 VisualManger에서 3D 오브젝트 참조값을 받와와서 2D 오브젝트에 합쳐주는 오브젝트를 반환하도록 구현
* 
* 5- GetWorldItem 메서드명을 GetWorldItemClone으로 변경
* 
* <v12.3 - 2024_0109_최원준>
* 1- itemObj변수명 itemObj2D로 변경
* 
* 
* <v12.4 - 2024_0111_최원준>
* 1- 아이템 3D 오브젝트에 생성 시 태그 추가
* 
* <v12.5 - 2024_0118_최원준>
* 1- 아이템 3D 오브젝트에 콜라이더 컴포넌트가 없다면 박스 콜라이더를 임시로 붙여주는 코드 작성
* 
* <v12.6 - 2024_0118_최원준>
* 1- 3D아이템 생성 시 태그가 없다면 추가하도록 변경
* 빌딩아이템의 경우 따로 태그가 존재해야하기 때문
* 
* 2- 3D아이템 생성 시 콜라이더를 붙여주기 위한 조건 검사를 GetComponentInChildren으로 변경
* (자식포함 모든 계층에 콜라이더가 없어야 붙여주기 위함. 빌딩오브젝트의 경우 자식에만 콜라이더가 있는 경우가 있음)
* 
* <v12.7 - 2024_0122_최원준>
* 1- 읽기전용 아이템 정보를 반환하는 메서드 GetReadableItem메서드를 추가
* 아이템을 생성하지 않고 ItemInfo 클래스의 Name, Desc 값 등의 프로퍼티를 호출할 수 있으며, 스프라이트를 참조할 수 있어야 할 필요
* (아이템 이동이 없으며 정보만 참조가능한 용도로 크래프팅UI등에 사용하기 위함)
* 
* 2- CreateWorldItem의 itemName과 count를 인자로 받는 메서드 내부 코드를 
* Item을 인자로 받는 오버로딩 메서드를 재사용하여 호출하는 것으로 변경
* (이유는 로직이 바뀔 때마다 두 번 적어야 하기 때문.)
* 
* 3- 아이템 오브젝트 생성 시 콜라이더를 자동으로 붙여주는 코드 삭제
* 오브젝트마다 콜라이더가 다르기 때문에 프리팹에서 수동으로 맞춰주기로 팀끼리 합의.
* 
* 4- 빌딩 아이템 생성시 isDecortation속성에 따라 태그를 붙여줘야할 경우와 붙여주지 말아야할 경우를 구분하는 식으로 코드 변경
* 
* <v12.8 - 2024_0124_최원준>
* 1- 인벤토리에 고유 식별번호를 부여하기 위하여
* 저장용 클래스 IdData를 데이터매니저를 통해 세이브 로드하는 코드를 작성하였고,
* GetId메서드를 작성
* 
* 2- CreateWorldItem의 빌딩아이템에 태그를 붙여주는 경우의 코드를 
* itemInfo.Item을 ItemBuilding으로 변환시켜서 isDecoration속성을 확인하던 부분을
* BuildingType프로퍼티로 바로 확인하여 붙여주도록 변경
* 
* <v12.9 - 2024_0125_최원준>
* 1- RegisterId, UnregisterId의 매개변수 keyPrefabName을 keyRootName으로 변경
* 
* 2- 건설아이템의 ItemMisc에서 Item으로 상속관계 변경으로 인해
* CreateWorldItem메서드 내부 빌딩아이템에 태그를 붙여주는 경우의 코드를 ItemType검사로 변경
* 
* <v13.0 - 2024_0130_최원준>
* 1- GetNewId메서드 idData의 null검사문 추가
* 
* <v13.1 - 2024_0131_최원준>
* 1- Item인스턴스를 인자로 전달받아 호출하는 CreateWorldItem메서드의 마지막 부분에
* 아이이템형 인벤토리의 ownerID를 업데이트하는 UpdateItemInventoryInfo메서드를 호출해주도록 하였음.
* 이유는 InventoryInfo의 경우 저장파일을 따로 두지 않기 때문에 ownerID를 게임 시작할 때 수동으로 잡아줘야 해당 ID를 바탕으로 파일로드가 가능해짐.
* 
* 기존의 CreateWorldItem의 ItemInfo를 기반으로 호출하는 메서드에서는 새롭게 생성하기 때문에 ID를 할당후 수동 업데이트 해줬지만
* Item을 기반으로 호출하는 메서드의 경우 이미 존재하는 아이템을 새롭게 생성하는 것이므로 해당 Item인스턴스에 저장된 ID를 수동으로 업데이트 해줄 필요가 있음.
* 
* <v13.2 - 2024_0213_최원준>
* 1- IdData의 세이브 로드 코드를 메서드화 하여, 슬롯을 인자로 받아 세이브,로드하도록 변경
* (기존 OnApplicationQuit에서 세이브하던 코드를 삭제 및 UI이벤트핸들러 연결로 변경, OnDestroy에서 세이브코드 해제, PlayerPrefs에서 로드슬롯번호를 받아오도록 변경)
* 
*/

namespace CreateManagement
{

    /// <summary>
    /// 아이템 생성에 관한 로직을 담당하는 클래스이며,<br/>
    /// 아이템 생성을 원하는 시점에 싱글톤 인스턴스를 참조하여 관련 메서드를 호출하면 됩니다.
    /// </summary>
    public class CreateManager : MonoBehaviour
    {
        public WorldItem worldItemData;         // 게임 시작 시 넣어 둘 월드 사전의 참조값입니다.
        GameObject itemPrefab2D;                // 리소스 폴더 또는 에디터에서 복제할 2D 프리팹을 참조합니다    

        Dictionary<string, Item>[] worldDic;    // 사전 배열 참조변수
        ItemType[] dicType;                     // 각 사전 별 보관하는 아이템 종류
        int dicLen;                             // 사전 배열의 길이

        VisualManager visualManager;
        readonly string itemTag = "Item";       // 아이템 3D 오브젝트에 적용시킬 태그
        
        DataManager dataManager;                // 데이터 저장용 매니저 참조
        IdData idData;                          // 고유 식별번호를 부여하기 위한 저장용 인스턴스

        InventoryInfo worldInventoryInfo;       // 아이템을 3D 상태로 보관하는 월드 인벤토리

        readonly string saveFileName = "WorldInventory_IdData_"; // 세이브 파일명


        private void Awake()
        {
            // 리소스 폴더에서 원본 프리팹 가져오기
            itemPrefab2D=Resources.Load<GameObject>( "Item2D" );

            // 월드 아이템 데이터의 인스턴스를 하나 만듭니다.
            worldItemData=new WorldItem();

            // 사전배열의 참조변수 설정
            worldDic=worldItemData.worldDic;
            dicType=worldItemData.dicType;
            dicLen=worldDic.Length;

            visualManager = GetComponent<VisualManager>();
            dataManager = GetComponent<DataManager>();
            worldInventoryInfo = GetComponentInChildren<InventoryInfo>();   // 게임매니저 하위에 존재
                  
            

            // 로드할 슬롯번호 - SlotNo 키값이 존재하지 않는다면(새 게임이라면) 0을, 존재한다면(기존 게임이라면) 해당 키값을 받아서 설정합니다.
            int loadSlotNo = 0;    
            if(PlayerPrefs.HasKey("SlotNo"))
                loadSlotNo = PlayerPrefs.GetInt("SlotNo");

            LoadFile(loadSlotNo);                      // 로드할 슬롯번호를 인자로 전달하여 파일을 로드합니다,.

            SaveLoadManager.OnSaveData += SaveFile;    // 세이브파일 메서드를 UI 이벤트 핸들러와 연결합니다.

        }
        
        
        /// <summary>
        /// 파일로 저장된 IdData 인스턴스를 로드하여 게임 중에 사용 할 IdData 인스턴스에 입력합니다.
        /// </summary>
        public void LoadFile(int slotNo)
        {
            dataManager.FileSettings(saveFileName, slotNo);     // 저장 파일 이름 설정
            idData = dataManager.LoadData<IdData>();            // 파일을 로드하여 인스턴스 참조
        }

        /// <summary>
        /// 게임 중에 사용한 IdData 인스턴스를 세이브합니다.
        /// </summary>
        public void SaveFile(int slotNo)
        {
            dataManager.FileSettings(saveFileName, slotNo);     // 저장 파일 이름 설정
            dataManager.SaveData<IdData>( idData );             // 사용 중인 IdData 인스턴스를 파일로 세이브
        }


        private void OnDestroy()
        {
            SaveLoadManager.OnSaveData -= SaveFile;    // 씬 전환 시 이벤트 핸들러에 연결된 세이브 메서드를 제거합니다.          
        }




        /// <summary>
        /// 아이템 이름을 기반으로 해당 아이템의 ItemType을 반환합니다<br/><br/>
        /// ** 해당 이름의 아이템이 존재하지 않는 경우 예외 발생 **
        /// </summary>
        /// <returns>해당 이름 아이템의 ItemType을 반환</returns>
        public ItemType GetWorldItemType( string itemName )
        {
            for( int i = 0; i<dicLen; i++ )
                if( worldDic[i].ContainsKey( itemName ) )     // 해당 이름의 키가 사전에 존재하는지 찾습니다.
                    return dicType[i];                      // 존재하는 경우 해당 사전의 아이템 타입을 반환합니다.

            // 해당 이름이 존재하지 않는 경우 예외처리
            throw new Exception( "해당 아이템이 존재하지 않습니다." );
        }

        /// <summary>
        /// 아이템 이름을 기반으로 해당 아이템의 월드 딕셔너리 참조값을 반환합니다.<br/><br/>
        /// ** 해당 이름의 아이템이 존재하지 않는 경우 예외 발생 **
        /// </summary>
        /// <returns>해당 아이템의 월드 사전 참조 값 반환</returns>
        public Dictionary<string, Item> GetWorldDic( string itemName )
        {
            for( int i = 0; i<dicLen; i++ )
                if( worldDic[i].ContainsKey( itemName ) )
                    return worldDic[i];

            // 해당 이름이 존재하지 않는 경우 예외처리
            throw new Exception( "해당 아이템이 존재하지 않습니다." );
        }



        /// <summary>
        /// 아이템 종류를 기반으로 해당 아이템의 월드 딕셔너리 참조값을 반환합니다.<br/><br/>
        /// ** 해당 종류의 사전이 존재하지 않는 경우 예외 발생 **
        /// </summary>
        /// <returns>해당 아이템의 월드 사전 참조 값 반환</returns>
        public Dictionary<string, Item> GetWorldDic( ItemType itemType )
        {
            if( itemType==ItemType.None )
                throw new Exception( "정확한 종류의 사전을 선택해주세요." );

            return worldItemData.worldDic[(int)itemType];
        }




        /// <summary>
        /// 이름을 검색하여 클론할 월드 아이템 하나를 가져옵니다.<br/><br/>
        /// ** 해당 이름의 아이템이 존재하지 않는 경우 예외 발생 **
        /// </summary>
        /// <returns>클론할 아이템의 참조 값을 반환</returns>
        private Item GetWorldItemClone( string itemName )
        {
            for( int i = 0; i<dicLen; i++ )
                if( worldDic[i].ContainsKey( itemName ) )
                    return (Item)worldDic[i][itemName].Clone();

            // 해당 이름이 존재하지 않는 경우 예외처리
            throw new Exception( "해당 아이템이 존재하지 않습니다." );
        }


        /// <summary>
        /// 해당 아이템이름을 기반으로 아이템이 월드 딕셔너리 목록에서 존재하는지 여부를 반환합니다<br/><br/>
        /// </summary>
        /// <returns>해당 이름의 아이템이 존재하지 않는 경우 false, 존재하는 경우 true를 반환</returns>
        public bool IsContainsWorldItemName( string itemName )
        {
            for( int i = 0; i<dicLen; i++ )
                if( worldDic[i].ContainsKey( itemName ) )
                    return true;

            return false;
        }


        /// <summary>
        /// 월드 인벤토리 생성 시 사용해야 할 메서드입니다.
        /// </summary>
        /// <returns></returns>
        public ItemInfo CreateWorldInventoryItem(UserInfo itemOwnerInfo, string itemName)
        {
            InventoryInfo inventoryInfo = itemOwnerInfo.inventoryInfo;
            
            if(inventoryInfo==null)
                throw new Exception("전달한 Transform이 인벤토리를 소유하고 있지 않습니다.");
            
            Item itemClone = GetWorldItemClone( itemName );         // 새로운 아이템 클론을 할당받습니다.
            ItemType itemType = GetWorldItemType( itemName );       // 이름에 해당하는 ItemType을 확인합니다.

            if( itemType != ItemType.Building)
                throw new Exception("ItemBuilding 타입이 아닙니다. 확인하여 주세요.");

            // 아이템을 생성하고 ItemInfo 참조값을 반환받습니다.
            ItemInfo itemInfo = CreateWorldItem(itemClone).RegisterWorldInvetory(itemOwnerInfo);
                  

            // 새롭게 생성하는 아이템이 건설 아이템 중에서 인벤토리 종류인 경우에는 추가 정보를 입력해야 합니다.
            if(itemInfo.BuildingType == BuildingType.Inventory)
            {
                // 새로운 ID를 부여합니다.
                int itemId = GetNewId(IdType.Inventory, itemInfo.Item3dTr.name);

                // ID를 등록합니다.
                itemInfo.SetPrivateId(itemId);

                // 아이템화 인벤토리의 인벤토리 정보를 참조합니다.
                InventoryInfo itemInventoryInfo = itemInfo.Item3dTr.GetComponentInChildren<InventoryInfo>();
                                
                // itemInfo를 전달하여 소유자 정보를 업데이트 해줍니다.
                itemInventoryInfo.UpdateItemInventoryInfo(itemInfo);
            }
            
            
            return itemInfo;    // 생성한 itemInfo를 반환합니다.
        }







        /// <summary>
        /// 3D 월드 상에 아이템 오브젝트 1개를 생성하여 ItemInfo 참조값을 반환합니다.<br/>
        /// 잡화아이템의 경우 중첩수량을 인자로 전달할 수 있습니다.(비잡화 아이템의 경우 무시합니다.)<br/>
        /// 중첩 최대 수량을 넘어가는 경우 최대 수량까지만 생성하여 줍니다.(오브젝트 1개만 생성합니다.)<br/><br/>
        /// ** 해당 이름의 아이템이 존재하지 않는 경우와 수량이 0이하인 경우 예외 발생 **
        /// </summary>
        /// <returns>해당 아이템 오브젝트의 ItemInfo 컴포넌트 참조 값을 반환합니다.</returns>
        public ItemInfo CreateWorldItem( string itemName, int overlapCount = 1 )
        {
            Item itemClone = GetWorldItemClone( itemName );     // 새로운 아이템 클론을 할당받습니다.
            ItemType itemType = GetWorldItemType( itemName );   // 이름에 해당하는 ItemType을 확인합니다.

            if( overlapCount<=0 )
                throw new Exception( "수량이 0이하 입니다. 정확하게 입력해주세요." );

            // 잡화 아이템인 경우 수량 정보 입력
            if( itemType==ItemType.Misc )
                ( (ItemMisc)itemClone ).AccumulateOverlapCount( overlapCount );
                     
            // 건설 아이템인 경우 예외 처리
            else if(itemType==ItemType.Building)
                throw new Exception("건설 아이템의 생성은 CreateWorldInventoryItem 메서드를 이용해야 합니다.");

            // Item 인스턴스를 전달하여 오브젝트를 생성하여 반환합니다.
            return CreateWorldItem(itemClone);
        }



        /// <summary>
        /// Item 인스턴스가 이미 존재하는 경우에 
        /// 해당 아이템의 정보로 월드 상에 아이템 오브젝트를 생성하여 참조값을 반환하여 줍니다.
        /// </summary>
        /// <returns>해당 아이템 오브젝트의 ItemInfo 컴포넌트 참조 값을 반환합니다.</returns>
        public ItemInfo CreateWorldItem( Item item )
        {
            // 오브젝트 월드에 생성
            GameObject itemObj2D = Instantiate( itemPrefab2D );

            // 아이템 정보를 등록
            ItemInfo itemInfo = itemObj2D.GetComponent<ItemInfo>();
            itemInfo.Item=item;

            // 아이템 정보를 전달하여 3D 오브젝트를 복제 생성한다음, itemObj에 부착합니다.
            GameObject itemObj3D = Instantiate( visualManager.GetItemPrefab3D(itemInfo));
            
            // 건설 아이템이 아닌 경우에만 태그를 붙여줍니다.
            if( item.Type != ItemType.Building)
                itemObj3D.tag = itemTag;

            // 퀘스트 아이템의 경우 아이템 셀렉팅을 막습니다.
            if(item.Type == ItemType.Quest)
                itemInfo.ItemSelect.enabled = false;
                     
            // 프리팹의 모든 계층에 콜라이더가 달려있지 않아야 콜라이더를 임시로 붙여줍니다. 
            if(itemObj3D.GetComponentInChildren<Collider>() == null )
                itemObj3D.AddComponent<BoxCollider>().isTrigger=false;


            // 2D오브젝트를 3D오브젝트 하위에 부착합니다.
            itemObj2D.transform.SetParent( itemObj3D.transform );

            // 아이템의 초기화를 진행합니다.
            itemInfo.OnItemCreated();
            



            if(itemInfo.BuildingType == BuildingType.Inventory)
            {
                // 고유 ID가 할당되어 있는 경우
                if( itemInfo.ItemId>=0 )
                {
                    // 아이템화 인벤토리의 인벤토리 정보를 참조합니다.
                    InventoryInfo itemInventoryInfo = itemInfo.Item3dTr.GetComponentInChildren<InventoryInfo>();

                    // itemInfo를 전달하여 소유자 정보를 업데이트 해줍니다.
                    if( itemInventoryInfo!=null )
                        itemInventoryInfo.UpdateItemInventoryInfo( itemInfo );
                }
            }




            // 컴포넌트 참조값 반환
            return itemInfo;
        }



        /// <summary>
        /// 읽기 전용 ItemInfo를 반환합니다.<br/>
        /// 월드 아이템 속성 값과 스프라이트 정보를 가지는 2D 정보입니다.<br/>
        /// 3D 오브젝트(실체)가 없기 때문에 인벤토리의 아이템으로서 사용이 불가능하며,<br/>
        /// 고유 정보를 읽기만 가능하고 쓰기는 불가능합니다.
        /// </summary>
        /// <returns></returns>
        public ItemInfo GetReadableItem(string itemName)
        {
            ItemInfo itemInfo = itemPrefab2D.GetComponent<ItemInfo>();
            itemInfo.Item = GetWorldDic(itemName)[itemName];

            itemInfo.innerSprite = visualManager.GetItemSprite(itemInfo, SpriteType.innerSprite);
            itemInfo.statusSprite = visualManager.GetItemSprite(itemInfo, SpriteType.statusSprite);

            return itemInfo;
        }


        /// <summary>
        /// 동일한 프리팹에 다른 고유의 식별번호를 부여하고 Id사전에 저장해줍니다.<br/>
        /// 전달 인자로 어떤 Id 딕셔너리를 참조할 것인지와 프리팹명을 전달해야 합니다.<br/> 
        /// 동적으로 Id를 할당해야 할 경우에 사용합니다.<br/>
        /// </summary>
        /// <returns>새롭게 부여된 식별번호를 반환</returns>
        public int GetNewId(IdType idDicType, string keyRootName)
        {
            if(idData==null)
                throw new Exception("idData 세이브파일 인스턴스가 생성되지 않았습니다. (저장파일 오류이므로 삭제후 사용합니다.)");

            return idData.GetNewId(idDicType, keyRootName);
        }


        
        /// <summary>
        /// Id사전에 원하는 숫자로 고유 식별 번호를 등록합니다.<br/>
        /// 참조할 Id종류, 키로 접근할 최상위 프리팹 명, 등록할 식별 번호를 전달해야 합니다.<br/>
        /// 동일한 id가 존재한다면 실패를 반환받습니다.
        /// </summary>
        /// <returns>id 등록 성공 시 true를, 실패 시 false를 반환</returns>
        public bool RegisterId(IdType idDicType, string keyRootName, int id)
        {
            return idData.RegisterId(idDicType, keyRootName, id);
        }


        /// <summary>
        /// Id사전에 등록되어있는 고유 식별 번호의 등록을 해제합니다.<br/>
        /// 참조할 Id종류, 키로 접근할 최상위 프리팹 명, 해제할 식별 번호를 전달해야 합니다.
        /// </summary>
        public void UnregisterId( IdType idDicType, string keyRootName, int id )
        {
            idData.UnregisterId(idDicType, keyRootName, id);
        }




    }
}