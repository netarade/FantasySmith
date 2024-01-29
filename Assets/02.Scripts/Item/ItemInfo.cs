using UnityEngine;
using UnityEngine.UI;
using ItemData;
using System;
using CreateManagement;
using InventoryManagement;
using DataManagement;

/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1102_최원준>
 * 1- 최초작성 및 주석처리
 * 
 * <v2.0 - 2023_1103_최원준>
 * 1- 주석 수정
 * 2- 이미지 컴포넌트 잡는 구문을 Start메서드에서 OnEnable로 변경
 * 인스턴스가 생성되어 이미지 컴포넌트를 잡기 시작하면 OnItemAdded와 호출 시점이 동시성을 가져서 스프라이트 이미지가 변경되지 않는다.
 * 
 * <v3.0 - 2023-1105_최원준>
 * 1- 개념아이템 변수인 item을 프로퍼티화 시켜서 set이 호출되었을 때 OnItemChanged()가 호출되도록 변경 
 * OnItemAdded는 private처리 및 내부 예외처리 구문 삭제
 *
 *<v4.0 - 2023_1108_최원준>
 *1- 아이템이 파괴될 때 정보를 저장하도록 구현하였으나, 모든아이템의 기록이 저장되지 않는 문제발생
 *=> 아이템쪽에서 파괴될떄마다 딕셔너리를 생성해서 CraftManager쪽에서 마지막에 한번 미리 생성해주도록 변경
 *
 *2- OnItemChanged 메서드 주석추가
 *
 *3- UpdateCountTxt 메서드 추가
 * 아이템 수량이 변경 될때 동적으로 텍스트를 수정해주도록 하였음.
 * item쪽에서 메서드를 가지고 있음으로 해서 편리한 접근성 확보.
 *
 *<v5.0 - 2023_1112_최원준>
 *1- OnItemAdded메서드 추가수정. (CreateManager쪽에서 중복코드 사용하고 있던 점 수정 및 통합, 주석 추가) 
 *
 *<v6.0 - 2023_1114_최원준>
 *1- OnItemAdded메서드를 OnItemChanged로 이름변경
 *2- ItemInfo 클래스 설명 주석 추가
 *3- private 메서드 public 메서드로 변경
 *4- 멤버 변수 item이 public되어있던 점을 private 처리. 반드시 프로퍼티를 통한 초기화를 위해
 *
 *<v7.0 - 2023_1116_최원준>
 *1- ItemInfo 클래스가 ItemImageCollection 멤버변수를 포함하여 외부이미지를 참조하도록 설정하였습니다. 
 *(CreateManager에 있던 참조 변수를 옮겨옴.)
 *
 *2- UpdateImage메서드를 수정하였습니다.
 *기존의 아이템 클래스가 ImageCollection 구조체 변수를 멤버로 포함하고 있던 점에서 ImageReferenceIndex 구조체 변수를 멤버로 포함하도록 바꾸었기 때문에
 *item의 ImageReferenceIndex 멤버변수로 부터 인덱스값을 받아와서 ImageCollection 변수에 접근하여 오브젝트에 이미지를 넣어주도록 수정.
 *
 *<v7.1 - 2023_1119_최원준>
 *1- OnDestroy()메서드 주석처리. 
 *Inventory클래스를 직렬화 가능하게 변경할 예정이므로
 *
 *2- RemoveItemObject 미구현 메서드 제거 - inventory클래스에서 구현
 *ItemInfo 클래스는 오브젝트의 정보만 최신화 시켜주는 역할을 하게 해야하기 때문이며, 
 *ItemInfo에서 item의 내부정보를 수정하는 메서드를 추가하기 시작하면, Inventory클래스에서의 기능을 중복 구현할 가능성이 커짐.
 *
 *<v8.0 - 2023_1216_최원준>
 *1- 아이템의 상태창 이미지 변수 statusImage 추가 및 UpdateImage메서드 내부 수정
 *
 *2- 아이템 파괴시 전달 로직 주석처리 되어있던 부분 제거
 *
 *3- slotList 변수명 slotListTr로 변경
 *
 *4- Transform imageCollectionsTr 임시변수 선언후 
 * GameObject.Find( "ImageCollections" ) 중복 호출 로직 수정
 *
 *<v8.1 - 2023_1217_최원준>
 *1- ItemImageCollection 변수들을 하나씩 참조하던 것을 배열로 만들어서 참조
 *
 *<v8.2 - 2023_1221_최원준>
 *1- GameObject.Find()메서드로 오브젝트를 검색하던 것을 빠른참조로 변경
 *
 *<v9.0 - 2023_1222_최원준>
 *1- 태그참조 철자오류 수정 (CANVAS_CHRACTER -> CANVAS_CHARACTER)
 *
 *2- ItemImageCollection[]의 배열을 참조만하고 생성을 안해서 뜨는 배열의 bounds오류 수정
 *
 *3- 아이템의 생성시점에 UpdateImage나 UpdatePosition을 호출하면 참조가 잡히지 않기 때문에 bounds오류가 뜨는데
 * OnItemChanged메서드를 아이템의 생성시점 호출이 아니라, 아이템의 등장 시점에 호출하도록 수정하였음.
 *
 *4 - SlotListTr이 뷰포트로 잡혀있던 점을 수정
 *
 *<v9.1 - 2023_1224_최원준>
 *1- Item프로퍼티의 주석일부 삭제
 *2- 컴포넌트 참조 구문 Start에서 OnEanble로 이동 및 정리
 *3- UpdataImage메서드 아이템 종류에 따른 중복로직 제거 후 간략화
 *
 *<v9.2 - 2023_1226_최원준>
 *1- 일부 디버그 출력메서드 정리
 *2- UpdatePosition에 slotListTr의 childCount 검사구문 추가
 *3- Item 프로퍼티 다시 주석해제 
 *
 *<v9.3 - 2023_1228_최원준>
 *1- 아이템 프리팹 계층 구조 변경으로 인해 (3D오브젝트 하위에 2D 오브젝트를 두는 구조)
 *각각 Transform, RectTransform itemTr와 itemRectTr 변수를 선언하여 자기트랜스폼 캐싱처리
 *
 *2- UpdatePosition 2D오브젝트의 부모를 변경하던 점을 최상위 3D오브젝트를 변경하도록 수정
 *
 *3- UpdatePosition 변수명을 UpdateInventoryPosition으로 변경
 *
 *<v9.4 - 2023_1228_최원준
 *1- 아이템 프리팹 계층구조 재변경으로 인해 (3D오브젝트, 2D오브젝트 전환방식)
 *코드를 2D기준으로 임시 변경 (itemTr->itemRectTr)
 *
 *
 *<v10.0 - 2023-1229_최원준>
 *1- UpdateSlotPosition에서 선택인자를 추가하여 슬롯리스트 정보를 주는 경우에는 해당 슬롯리스트의 인덱스로 포지션업데이트를 하며,
 *슬롯리스트 정보를 주지 않은 경우에는 아이템이 현재 담겨있는 슬롯을 기준으로 슬롯리스트를 참조해서 인덱스에 따른 포지션 업데이트를 하도록 변경
 *
 *<v10.1 - 2023_1230_최원준>
 *1- UpdateSlotPoisition메서드명을 UpdatePositionInSlotList로 변경 하고
 *슬롯리스트를 인자로 받도록 설정. 선택인자로 인자가 전달되지 않았다면 계층구조를 참조하여 자동계산하도록 변경
 *
 *2- OnItemChanged메서드를 외부에서 호출할 때 slotList를 인자로 받아서 호출가능하도록 변경,
 *UpdatePositionInSlotList에 slotList인자를 전달하여 호출. 마찬가지로 선택인자로 호출가능
 *
 *3- 계층구조 전환코드를 간단하게 수정
 *미리 ItemTr ItemRectTr을 잡아놓을 필요없이
 *전환이 이루어질 때의 메서드를 호출하면 2D가 메인이여야 할때는 하위 마지막자식 
 *
 *<v10.2 - 2023_1231_최원준>
 *1- Locate 2D, 3D 메서드
 *2- SetOverlapCount메서드
 *3- FindNearstRemainSlotIdx 메서드 inventoryInfo 매개변수 오버로딩
 *
 *<v10.3 - 2024_0101_최원준>
 *1- 아이템이 현재 활성화 기준의 탭을 확인하고 포지션업데이트를 해야 하므로, 인터렉티브 스크립트에서 활성화탭 기준 변수를 받아도록 하였음
 *2- prevDropEventCallerTr변수 설정 및 OnItemDrop메서드 추가
 *
 *3- FindNearstRemainSlotIdx 메서드 모두 삭제
 *InventoryInfo에 있어야 할 기능이며, 아이템이 인벤토리 정보를 참조하고 있기 때문에 인벤토리를 참조하여 호출하기만 하면 되기 때문
 *
 *4- OnEnable에서 슬롯리스트 설정 시 canvsTr을 find 메서드로 찾아서 처리하던 점 삭제하고,
 *UpdateInventoryInfo메서드 호출로 변경
 *
 * 
 *
 *
 *
 * [추후 수정해야할 점] 
 *  1- UpdateInventoryPosition이 현재 자신 인벤토리 기준으로 수정하고 있으나,
 *  나중에 UpdatePosition을 할 때 아이템이 보관함 슬롯의 인덱스 뿐만 아니라 어느 보관함에 담겨있는지도 정보가 있어야 한다.
 * 
 * 2- 슬롯드롭이벤트가 발생할 때 인벤토리 정보가 다르다면 이전 인벤토리에서 이 아이템 목록을 제거해야한다.
 *  계층변경이 일어날 때 인벤토리에서 이 아이템을 목록에 추가하거나 제거해야 한다.
 *  Drag에서 인벤토리 밖으로 빼냈을 때 인벤토리 목록에서 이 아이템을 제거해야 한다.
 *
 *
 *
 *
 * [이슈_0101] 
 * 1- 슬롯의 정보(인벤토리 정보) 업데이트 시점
 * a- Slot To Slot에서 SlotDrop이 일어날 때 Slot을 통해 받아야 한다.
 * b- ItemDrag해서 인벤토리 외부로 Drop할때 ItemDrag를 통해 받아야 한다.
 * c- ItemInfo에서 2D to World(월드정보인자), 3D to Slot(인벤토리정보인자) 할때 자체적으로 확인해야 한다.
 * => 외부에서 업데이트 메서드를 호출할 수 있게 해줘야한다.
 *
 * 2- 타 인벤토리로 업데이트가 이뤄질 때는 
 * slotIndexAll과 slotIndex 모두를 받야야 한다.
 * 이유는 한번 받은 상태에서 다른 탭변경을 시도하면 위치 정보가 안맞기 때문
 *
 *
 *
 *<v10.4 - 2024_0102_최원준>
 *1- OnItemDrop메서드 구현완료
 *어떤 아이템의 드롭 이벤트 발생시 외부 스크립트에서 호출하도록 설정
 *2- UpdateActiveTabInfo 메서드 정의하고 OnItemChanged 내부에 추가.
 *
 *
 * <v11.0 - 2024_0102_2_최원준>
 * 1- OnItemWorldDrop메서드의 매개변수를 dropEventCallerTr에서 worldPlaceTr로 변경
 * 
 * 2- Transfer2DToWorld메서드 수정
 * 
 * a. prevDropEventCallerTr참조가 최신화 안하고 null로 잡혀있던 점 수정
 * b. inventoryInfo.RemoveItem(this);가 null값 전달 시 없던 점 수정.
 * c. 예외처리 상황 isWorldPositioned기반으로 변경
 * 
 * 3- OnItemWorldDrop, Transfer2DToWorld메서드 Vector3, Quaternion인자 전달 기반 오버로딩, 반환값 void로 수정
 * 
 * 4- UpdateInventoryInfo메서드에서 prevDropEventCaller 정보를 최신화하는 코드 추가
 * 
 * 5- OnItemChanged메서드를 OnItemCreated로 변경 및 주석 수정, 인자로 InventoryInfo 스크립트를 받도록 설정.
 * 아이템이 인벤토리에 생성되는 상황에서 호출하는 것이 더욱 명확한 의미기이 때문
 * 
 * 6- OnEnable메서드 주석보완, canvasTr의 계층참조로 처리하던 코드 삭제
 * 
 * 7- UpdateActiveTabInfo 메서드 주석 보완 및
 * 인자를 제한적으로 받아 어떤 상황에서 호출해야 하는 지 명확하게 설정
 * 
 * 8- OnItemSlotDrop 주석 수정
 * 
 * 9- ChangeHierarchy메서드 선택인자 null값 추가
 * 3D->Slot에서 호출 시 인덱스를 입력하고 인덱스에 따른 Position Update를 따로 해주어야 하기 때문에 부모 설정이 필요하지 않음.
 * 
 * 10- Locate3DToSlot, SlotList, InventoryInfo등의 오버로딩 메서드 삭제 후 
 * Locate3DToInventory메서드 하나로 통일.
 * 
 * 11- 사용자가 호출 할 OnItemGain메서드 추가
 *
 *
 * <v11.1 - 2024_0102_최원준>
 * 1- ItemInfo 클래스 partial클래스로 정의
 * 2- 아이템 수량관련 기본 메서드를 ItemInfo_2.cs로 옮김
 * 3- emptyListTr 변수 추가
 * 아이템을 삭제하기 전 임시로 빈 공간으로 옮길 슬롯리스트
 * 
 * 
 *
 * [이슈_0102]
 * 1- UpdateImage에서 iicArr에 인덱스를 대입하여 참조값을 찾아가는 부분이 있는데, 
 * 모든 아이템이 이 메서드를 가지고 있을 필요가 없기때문에 특정 인스턴스에 요청해서 정보를 받아오면 된다고 생각되는데,
 * CreateManager에 요청하는 것이 적합하지만, 이미지 우선순위로 싱글톤에서 참조를 받아오면 시점이 늦기때문에 다른방안을 생각 중.
 * => 아이템 생성 전 미리 오브젝트를 만들어놓고 해당 메서드를 호출하도록 하는 방식으로 접근하면 어떨까 생각.
 * 
 * 2- OnEnable에서 OnItemCreate메서드를넣고 UpdateInventoryInfo를 따로 호출할지 생각 중.
 * => 이미지 우선순위로 OnItemCreate를 수동으로 호출로 결정, UpdateInventoryInfo메서드를 내부에 포함.
 * 
 * [나중에 수정할 것_0102]
 * 1- SetOverlap메서드 수정
 * 2- OnEnable에 있는 iic참조 관련 메서드 렌더링 관리 클래스쪽에서 호출 할 예정
 *
 * <v11.2 - 2024_0104_최원준>
 * 1- RemoveItem(this) 를 RemoveItem(item.Name)으로 변경
 * InventoryInfo클래스의 ItemInfo 오버로딩을 삭제하였기 때문
 *
 *2- UpdateActiveTabInfo의 인자 caller를 interactiveCaller로 이름변경
 *
 *3- UpdateInventoryInfo에서 내부적으로 UpdateActiveTabInfo 메서드까지 호출로 변경.
 *=> 인벤토리 정보를 업데이트하는 메서드를 하나로 통합하기 위함.
 *
 *4- UpdateActiveTabInfo 메서드를 다른 메서드에서 호출하는 것 삭제
 *=> interactive 클래스에서 탭변경으로 수동 탭변경할 때 이외에는 호출하지 않아야 함.
 *
 *5- UpdateInventoryInfo메서드의 매개변수를 Transform에서 InventoryInfo클래스로 변경
 *해당 메서드를 호출하는 모든 코드 수정
 *=> 이유는 inventoryInfo클래스에서 호출할 itemInfo를 받아서 자기참조 주소를 넣어서 호출하는 경우가 많고,
 *모든 호출의 단위를 통일하기 위해서
 *
 *<12.0-2024_0105_최원준>
 *1- UpdatePosition에서 슬롯리스트의 하위자식검사를 하던 부분 수정
 *2- IsSlotEnough에 ItemType을 기반으로 호출하던 부분을 ItemInfo를 인자로 넣어 호출하는 메서드로 변경
 *(이유는 잡화아이템의 경우 중첩해서 들어갈 경우 슬롯이 필요하지 않은 경우도 있기 때문.)
 *
 *3- OnItemWorldDrop 메서드를 삭제
 *아이템은 인벤토리에서 방출되어야 나갈 수 있기 때문에 인벤토리에서 제거하기 전까지는 허용하지 않음.
 *Remove메서드를 통해 나가야 함.
 *
 *4- Transfer2DtoWorld, TransferWorldTo2D 메서드를 모두 DimensionShift메서드로 통합하였습니다.
 *위치정보 인자를 주어 이동시키던 것을 일단인벤토리에서 제거하면, 사용자에게 맡기는 쪽으로 구현합니다.
 *(기본 드랍위치는 playerDropTr로 지정되어있습니다.)
 *
 *<v12.1 - 2024_0105_최원준>
 *1- 기존의 OnItemCreated를 OnItemCreatedInInventory로 변경, 신규메서드 OnItemCreateInWorld 추가
 *아이템이 인벤토리 정보를 바탕으로 인벤토리 내부에 생성되는 경우와, 인벤토리 외부에 생성되는 경우를 나누었음.
 *
 *2- UpdateInventoryInfo메서드 내부의 prevDropSlotTr을 itemRrectTr.parent에서 slotListTr.GetChild(item.SlotIndex)로 수정
 * (계층이 월드에 나온상태에서 잡아주고 있었기 때문에 null값 참조였음)
 *
 *<v12.2 - 2024_0106_최원준>
 *1- 상태창 스크립트를 아이템에서 인벤토리로 옮기면서 statusInteractive변수를 추가하여, 인벤토리가 바뀔때마다 최신 상태창 참조값을 반영하도록 구현
 *기존에는 아이템이 상태창을 처리하는 로직을 모두 들고 있었지만, 상태창이 로직을 가지게만들고, ItemInfo_2.cs에서 아이템에 포인터를 두면 해당 상태창 메서드를 호출시키도록 변경
 *
 *<v12.3 - 2024_0107_최원준>
 *1- UpdatePositionInInventory메서드에서 아이템의 크기를 슬롯의 크기에 맞게 수동으로 조절하도록 변경 
 *( 기존 Slot에 붙어있던 VerticalLayoutGroup은 아이템이 슬롯에 담길 때 자동으로 크기를 맞춰주지만, 인벤토리내 모든 오브젝트가 활성화,비활성화 될때마다
 *자식 아이템의 크기와 위치를 마음대로 되돌리는 기능을 가지고 있기 때문)
 *
 *<v12.4 - 2024_0108_최원준>
 *1- SetDropPosition메서드명을 OnItemWorldDrop으로 변경
 *RemoveItem후에 itemInfo의 포지션을 설정하기가 번거롭기 때문에 인자 전달로 전송시킬 필요가 있으며 이를 대체하기 위함.
 *(RemoveItem을 잊어먹고 호출하더라도 예외처리하지 않고 RemoveItem해주도록 변경)
 *
 *2- OnItemGain메서드명을 OnItemWorldGain으로 이름 변경
 *
 *3- playerDropTr을 플레이어의 맨마지막 자식 위치로 설정
 *
 *4- 월드 상태의 아이템을 먹었을 때 2d 회전값이 돌아가있는 점을 UpdatePosition에서 다시 0으로 맞추어주도록 추가
 *
 *<v12.5 - 2024_0108_최원준>
 *1- playerDropTr 변수명을 baseDropTr로 변경하고,
 *기본 드랍정보를 인벤토리가 변경될 때마다 해당 인벤토리로 부터 받아서 초기화하도록 UpdateInventoryInfo에서 참조 설정
 *
 *2- ItemInfo가 iicMisc을 직접 참조하던 방식을 삭제 및
 *visualManager 변수를 추가하여 해당 메서드를 통해 참조값을 얻어오는 방식으로 구현 
 *
 *innerSprite, statusSprite를 visualManager의 GetItemSprite메서드를 통한 호출로 구현
 *
 *
 *<v12.6 - 2024_0109_최원준>
 *1- 2D 오브젝트와 3D오브젝트를 분리해서 생성하면서 기존 ItemInfo에서 OnEnable에서 3D오브젝트의 ItemTr을 바로 잡을 수 없게 되면서
 *텍스트 정보가 보이지 않는 문제가 있음
 *=> 기존 OnEnable이 아니라 Update형식으로 OnItemCreated메서드에서 다시 잡도록 구현
 *
 *2- OnItemCreatedInInventory메서드를 OnItemAdded로 변경, OnItemCreatedInWorld메서드를 OnItemCreated로 변경 및 로직수정
 *이유는 메서드의 목적성이 OnItemCreated는 아이템이 항상 월드 상에서 생성한 이후 아이템 정보를 정의하기 위해서이며,
 *OnItemAdded는 아이템 월드에서 생성 이후 인벤토리에 넣었을 때의 정보참조를 위한 메서드이기 때문
 *
 *3- UpdateInventoryInfo에서 baseDropTr을 null로 잡아주고 있던점 수정
 *OnItemWorldDrop시 인벤토리의 RemoveItem을 먼저 해주게 되는데, 드랍위치를 null로 잡아버리기 때문에 드랍 불가능한 현상 발생하였기 때문
 *
 *<v12.7 - 2024_0110_최원준>
 *1- UpdateActiveTabInfo를 안전성을 위해 호출자를 매개변수로 넣어서 호출하는 방식으로 변경하여 외부에서만 호출가능하게 함.
 *UpdateInventoryInfo에서 액티브탭의 정보를 읽어올 때는 InventoryInteractive 클래스의 프로퍼티에 직접접근하여 가져오는 방식으로 변경
 *
 *<v12.8 - 2024_0111_최원준>
 *1- 퀘스트 아이템을 추가하면서 UpdatePositionInSlotList에서 전체탭인 경우에 로직이 작동하지 못하도록 변경
 *
 *<v12.9 - 2024_0112_최원준>
 *1- 퀘스트탭을 열어놓은 상태에서 아이템을 먹으면 퀘스트 슬롯에 업데이트 되는 문제가 있어서 UpdatePositionInSlotList에
 *퀘스트아이템이 아니고, 개별탭 기준이라면, 포지션 업데이트를 실행하지않도록 조건검사문을 추가
 *
 *<v13.0 - 2024_0116_최원준>
 *1- 현재 인터렉티브 스크립트의 탭정보를 저장하는 curActiveTab변수를 추가
 *탭변동이 일어날때, 아이템 Add가 일어날때 탭정보가 업데이트 되어야 하며,
 *UpdatePositionInSlot 메서드가 호출 될 때 현재 탭 상태에 따라서 위치가 업데이트 되어야 한다.
 *(즉, 현재 탭에 해당하는 아이템이 아닌 경우 빈리스트로 이동하는 것이 있어야 한다.)
 *
 *2- curActiveTab변수를 새롭게 선언하여 아이템이 현재 활성화 탭정보를 가지고 있도록 하였음. (포지션 업데이트 시 필요하므로)
 *
 *3- UpdateActiveTabInfo메서드에서 활성화탭 정보 업데이트 시 매개인자로 curActiveTab추가
 *
 *4- UpdateActiveTabInfo 매개변수 없는 오버로딩 메서드 추가하여
 *아이템 쪽에서 OnItemAdded에서 활성화탭정보를 인터렉티브 스크립트로부터 수동으로 받아 업데이트 할 수 있게 하였음.
 *
 *5- UpdatePositionInSlotList메서드명 변경 -> UpdatePositionInfo
 *
 *6- 신규 메서드 MoveToEmptyList를 만들고, UpdatePositionInfo메서드의 기존 일부 코드를 MoveToSlot으로 만든 후
 *curActiveTab에 따라서 조건 분기하여 포지션 업데이트를 실행하도록 구현
 *
 * <v13.1 - 2024_0116_최원준>
 * 1- 아이템이 현재 속해있는 탭을 반환하는 CurActiveTab 읽기전용 프로퍼티 추가
 * (다른 인벤토리에 슬롯 드랍시 현재 아이템이 어떤 탭에 속해있었는지 확인 한 다음, 
 * 전송할 인벤토리의 탭이 일치하지 않을때 실패처리하지 않으면 아이템이 사라진것 처럼 보이게 되기 때문)
 * 
 * <v13.2 - 2024_0117_최원준>
 * 1- UpdateCountTxt메서드를 UpdateTextInfo로 변경하였으며, 
 * 일부 코드를 InitCountTxt메서드로 분리하여 OnItemCreated메서드에서 한번만 호출하도록 변경
 * 
 * 수량 변동 뿐만 아니라 상태창의 spec 텍스트까지 변경사항이 있다면 반영해야 하므로,
 * ItemMisc뿐만 아니라 모든 아이템이 OnItemAdded될 때 호출하도록 변경
 * (관련 변수들 ItemInfo_4.cs에 선언 및 활용)
 * 
 * 2- SlotIndex 프로퍼티명 SlotIndexEach로 변경
 * 
 * <v13.3 - 2024_0123_최원준>
 * 1- 아이템의 콜라이더를 끄기 위한 itemCol변수 추가하여 OnItemCreated에서 생성될 때 itemTr을 통해 초기화하도록 설정
 * 
 * <v13.4 - 2024_0124_최원준>
 * 1- playerTr을 ownerTr로 변경
 * 
 * 2- 아이템 소유주 정보를 나타내는 OwnerId를 추가
 * 
 * <v13.5 - 2024_0124_최원준>
 * 1- OwnerId의 타입을 string에서 int형으로 변경 및 주석 수정
 * 이유는 순번대로 id를 증가시켜 부여하기 위함
 * 
 * 2- OwnerTr 및 ItemTr 읽기전용 프로퍼티 추가 
 *
 * <v13.6 - 2024_0125_최원준>
 * 1- 읽기전용 프로퍼티 OwnerName과 worldOnwnerName을 추가하였음.
 * 이유는 아이템이 저장될때 소유자명도 같이 저장해야 Id를 인식할 때 키값으로 접근이 가능하기 때문
 * 
 * 2- worldInventoryInfo를 추가하여, visualManager스크립트가 위치한 하위 컴포넌트를 참조하도록 하였음.
 * 이는 ItemInfo_3.cs의 RigesterToWorldInventory메서드에서 사용하기 위함
 * 
 * 3- SwitchAppearAs2D메서드를 OnItemCreated에서 호출해주도록 함.
 * 아이템을 생성하고 바로 사용하는 경우 DimensionShift를 사용하지 않았기 때문에 2D기능이 종료가 되지 않은 상태로 나가서 셀렉팅이 일어남.
 * 
 * <v13.7 - 2024_0125_최원준>
 * 1- 아이템이 3D상태일 때 캔버스그룹의 blockRaycasts를 코드로 false로 만들어도 적용되지 않는 문제가 있어서
 * (이는 3D상태일때도 캔버스그룹을 통해 레이캐스팅을 받거나 흘리는 기능이 있기 때문인것으로 보여지는데)
 * => 이미지 컴포넌트의 레이캐스트 블록을 해제하고, Interactable속성을 비활성화 시키는 것으로 변경
 * 
 * <v13.8 - 2024_0125_최원준>
 * 1- OwnerId이외의 아이템 자체의 Id 프로퍼티 추가하였음.
 * 이유는 월드 인벤토리에 아이템이 저장될 때 누구의 아이템인지 유저 정보도 있어야 하지만, 
 * 인벤토리의 경우 동일한 이름의 아이템이 만들어 질 때의 식별번호가 필요하기 때문(저장파일을 달리 하기 위해서)
 * 
 * <v14.0 - 2024_0126_최원준>
 * 1- Id변수명을 ItemId로 변경
 * 2- UpdateInventoryInfo에서 월드로 나갈때 ownerId를 0에서 -1로 초기화하는 것으로 변경
 * 
 * 
 * <v14.1 - 2024_0126_최원준>
 * 1- ItemTr 프로퍼티 명을 Item3dTr로 변경 
 * (사용할 때 혼동하지 않기 위함)
 * 
 * <v14.2 - 2024_0126_최원준>
 * 
 * 1- TrApplyRange 열거형 옵션을 추가하여 월드 아이템 드랍할 때 Transform 인자 전달 시 
 * 어떤 부분만 적용시킬 지 선택가능하게 하였음.
 * 
 * 2- OnItemWorldDrop메서드를 기본적으로 전달받은 Transform 인자의 위치값만 적용시키도록 하였으며,
 * TrApplyRange을 인자로 전달받아 회전과 스케일값을 선택적으로 적용시킬 수 있게 구현. 주석도 수정
 * 
 * 
 * 3- 아이템이 인벤토리창을 닫아도 키보드로 셀렉팅 일어나는 현상이 있어
 * SwitchAppearAs2D메서드를 public 으로 설정하여 인벤토리 창을 닫을 때 
 * 인벤토리가 호출하도록 하여 아이템 셀렉팅을 막음.
 * 
 * <v14.3 - 2024_0128_최원준>
 * 1- 읽기전용 프로퍼티 StatusWindowInteractive를 추가하여 아이템제거 시 해당 상태창을 볼 수 있게 하였음.
 * 이유는 소모 아이템 우클릭 시 제거되는 순간 PointerExit이벤트가 발생하지 않기 때문에 상태창이 남아있기 때문에 강제로 종료해줘야할 필요성이 있음  
 * 
 * <v14.4 - 2024_0128_최원준>
 * 1- UpdatePositionInfo메서드 내부 MoveToSlot메서드에서 itemRectTr의 rotation값을 초기화하고 있던 부분을 localRotation을 초기화하도록 수정
 * (아이템습득 시 2D 이미지가 회전해버리는 문제가 생겼음)
 * 
 * <v14.5 - 2024_0129_최원준>
 * 1- 아이템의 Rigidbody itemRb 변수 및 프로퍼티 속성을 추가 및 OnItemCreated에서 초기화
 * 이유는 장착 및 해제 시 중력을 OnOff할 필요성이 있으므로
 * 
 */

/// <summary>
/// 아이템 관련 전송 메서드에서 Transform 인자를 전달할 때 어떤 부분을 적용 시킬 지 결정할 수 있습니다.<br/>
/// 위치값만 적용 시킬지, 위치값과 회전값을 동시에 적용시킬 지, 위치, 회전, 크기 모든 값을 적용시킬 지 결정할 수 있습니다.
/// </summary>
public enum TrApplyRange { Pos, Pos_Rot, Pos_Rot_Scale }

/// <summary>
/// 게임 상의 아이템 오브젝트는 이 클래스를 컴포넌트로 가져야합니다.<br/><br/>
/// 
/// ItemInfo 스크립트가 컴포넌트로 붙은 아이템 오브젝트의 자체적인 기능은 다음과 같습니다.<br/>
/// (ItemInfo의 개념 아이템 인스턴스인 item이 할당될 때 자동으로 이루어집니다.)<br/><br/>
/// 
/// 1.오브젝트의 이미지를 개념 아이템의 정보와 대조하여 채웁니다.<br/>
/// 2.잡화아이템의 경우 중첩횟수를 아이템정보와 비교하여 표시하여 줍니다. 비잡화 아이템의 경우 텍스트를 끕니다.<br/>
/// 3.인벤토리 슬롯 상의 포지션을 아이템 정보와 대조하여 해당 슬롯에 위치시킵니다.<br/><br/>
/// 
/// 주의) 아이템의 내부 정보가 바뀔 때 마다 최신 정보를 오브젝트에 반영해야 합니다.<br/>
/// 1,2,3의 경우 각 메서드를 따로 호출 할 수 있으며 모든 것을 한번에 호출하는  OnItemChanged메서드가 있습니다.<br/>
/// </summary>
public partial class ItemInfo : MonoBehaviour
{
    /**** 아이템 고유 정보 ****/
    Item item;              // 아이템의 실제 정보가 담긴 변수
    
    VisualManager visualManager;    // 아이템의 Sprite이미지를 전달받기 위한 아이템 비쥬얼 관리 클래스 참조
    Image itemImage;                // 아이템이 인벤토리에서 2D상에서 보여질 이미지 컴포넌트 참조  
    public Sprite innerSprite;      // 아이템이 인벤토리에서 보여질 이미지 스프라이트
    public Sprite statusSprite;     // 아이템이 상태창에서 보여질 이미지 스프라이트 (상태창 스크립트에서 참조를 하게 됩니다.)
    Text countTxt;                  // 잡화 아이템의 수량을 반영할 텍스트
            
    RectTransform itemRectTr;       // 자기자신 2D 트랜스폼 참조(초기 계층 - 상위 부모)
    Transform itemTr;               // 자기자신 3D 트랜스폼 참조(초기 계층 - 하위 마지막 자식)
    CanvasGroup itemCG;             // 아이템의 캔버스 그룹 컴포넌트 (아이템이 월드로 나갔을 때 2D이벤트를 막기위한 용도) 
    Collider itemCol;               // 아이템의 3D오브젝트가 가지고 있는 기본 콜라이더
    Rigidbody itemRb;               // 아이템의 3D오브젝트가 가지고 있는 리지드바디

    InventoryInfo worldInventoryInfo;   // 아이템을 3D상태로 보관할 수 있는 월드 인벤토리 정보

    /*** 아이템 변동 정보 ***/

    /*** Locate2DToWorld 또는 Locate3DToWorld 메서드 호출 시 변동***/
    bool isWorldPositioned;         // 아이템이 월드에 나와있는지 여부

    /**** InventoryInfoChange 메서드 호출 시 변동 ****/
    Transform inventoryTr;              // 현재 아이템이 들어있는 인벤토리의 계층정보를 참조합니다.
    Transform slotListTr;               // 현재 아이템이 담겨있는 슬롯리스트 트랜스폼 정보
    Transform emptyListTr;              // 아이템을 임시로 이동 시킬 빈공간 리스트

    InventoryInfo inventoryInfo;                // 현재 아이템이 참조 할 인벤토리정보 스크립트
    InventoryInteractive inventoryInteractive;  // 현재 아이템이 참조 할 인벤토리 인터렉티브 스크립트
    StatusWindowInteractive statusInteractive;  // 현재 아이템이 참조 할 상태창 인터렉티브 스크립트

    Transform ownerTr;                  // 현재 아이템을 소유하고 있는 인벤토리 소유자 정보 참조

    Transform baseDropTr;               // 아이템이 떨어질 기본 드랍 위치
    bool isBaseDropSetParent;           // 기본 드랍위치에 부모설정 옵션이 걸려있는지 여부


    /**** InventoryInfoChange 메서드 호출시 변동 ****/
    /**** inveractive에서 변동일어 날 때마다 변동*****/
    bool isActiveTabAll;                // 현재 아이템이 담겨있는 인벤토리의 활성화 탭의 기준이 전체인지, 개별인지 여부
    TabType curActiveTab;               // 현재 아이템이 담겨있는 인벤토리의 활성화 탭 정보
    

    
    /**** InventoryInfoChange 메서드 호출시 변동 ****/
    /**** OnItemSlotDrop 이벤트 호출 시 변동 ****/
    Transform prevDropSlotTr;    // 드랍이벤트가 발생할 때 이전의 드랍이벤트 호출자를 기억하기 위한 참조 변수 



    
    /*** 내부에서만 수정가능한 읽기전용 프로퍼티***/

    /// <summary>
    /// 아이템이 월드에 나와있는지 (3D 오브젝트 인지) 여부를 반환합니다.
    /// </summary>
    public bool IsWorldPositioned { get {return isWorldPositioned;} }
    
    /// <summary>
    /// 현재 아이템이 담긴 인벤토리의 정보입니다.
    /// </summary>
    public InventoryInfo InventoryInfo { get {return inventoryInfo;} }


    /// <summary>
    /// 현재 아이템이 담긴 인벤토리 인터렉티브 스크립트 참조값을 반환합니다.
    /// </summary>
    public InventoryInteractive InventoryInteractive { get { return inventoryInteractive;} }


    /// <summary>
    /// 현재 아이템이 담긴 인벤토리의 상태창 인터렉티브 스크립트 참조값을 반환합니다.
    /// </summary>
    public StatusWindowInteractive StatusWindowInteractive { get { return statusInteractive;} }


    /// <summary>
    /// 현재 아이템이 담겨있는 슬롯리스트의 Transform을 반환합니다.
    /// </summary>
    public Transform SlotListTr { get {return slotListTr;} }

    /// <summary>
    /// 현재 아이템이 속해있는 슬롯의 탭을 반환합니다.
    /// </summary>
    public TabType CurActiveTab { get { return curActiveTab;} }

    /// <summary>
    /// 현재 아이템 3D 오브젝트의 최상위에 부착되어 있는 콜라이더입니다.
    /// </summary>
    public Collider ItemCol { get { return itemCol;} }

    /// <summary>
    /// 현재 아이템 3D 오브젝트의 최상위에 부착되어있는 리지드바디입니다.
    /// </summary>
    public Rigidbody ItemRb { get { return itemRb;} }



    /// <summary>
    /// 아이템이 3D로 전환되어 월드에 놓여진 상태가 될 때, 해당 아이템의 소유자명을 말합니다.
    /// </summary>
    public string worldOwnerName { get; } = "World";
        
    /// <summary>
    /// 아이템의 소유자 명을 반환합니다.<br/>
    /// 아이템이 인벤토리에 보관된다면 해당 인벤토리의 계층 최상위 오브젝트 명이 소유자명이 됩니다.
    /// </summary>
    public string OwnerName { get { return item.OwnerName; } }

    /// <summary>
    /// 아이템의 소유자를 식별할 수 있는 고유 숫자를 반환합니다.<br/>
    /// 이는 해당 인벤토리를 소유하고 있는 계층 최상위 부모 오브젝트의 고유 식별 번호입니다.
    /// </summary>
    public int OwnerId { get { return item.OwnerId; } }
    

    /// <summary>
    /// 아이템 소유자의 Transform 참조값을 반환합니다.<br/>
    /// 이는 해당 인벤토리를 소유하고 있는 계층 최상위 부모 오브젝트를 말합니다.
    /// </summary>
    public Transform OwnerTr { get { return ownerTr; } }


    /// <summary>
    /// 아이템의 3D 오브젝트가 가지고 있는 Transform 컴포넌트 참조값을 반환합니다.
    /// </summary>
    public Transform Item3dTr { get { return itemTr; } }

    /// <summary>
    /// 아이템 고유의 식별번호입니다.<br/>
    /// 동일한 이름의 아이템에 식별번호가 필요한 경우 부여될 수 있습니다. (ex. 인벤토리 아이템)<br/>
    /// 식별번호가 부여되지 않은 아이템의 Id 기본값은 -1입니다.
    /// </summary>
    public int ItemId {get {return item.Id;} }



    /*** 내부 아이템의 속성을 변경해주는 프로퍼티 ***/

    /// <summary>
    /// 아이템 오브젝트가 해당하는 탭에서 몇번 째 슬롯에 들어있는 지 인덱스를 반환하거나 설정합니다.
    /// </summary>
    public int SlotIndexEach { get{return item.SlotIndexEach;} set{ item.SlotIndexEach=value;} }

    
    /// <summary>
    /// 아이템 오브젝트가 전체 탭에서 몇번 째 슬롯에 들어있는 지 인덱스를 반환하거나 설정합니다.
    /// </summary>
    public int SlotIndexAll { get{return item.SlotIndexAll;} set{item.SlotIndexAll=value;} }
      
    /// <summary>
    /// 아이템이 담겨있는 실제 정보를 직접 저장하거나 반환받습니다.<br/>
    /// 클론 한 Item 인스턴스를 저장하고, 저장 되어있는 인스턴스를 불러올 수 있습니다.<br/>
    /// </summary>
    public Item Item { set{ item=value; } get { return item; } }
    

    



    /// <summary>
    /// 이미지 컴포넌트를 잡는 우선순위를 높이기 위해 OnEnable 사용<br/>
    /// 자기자신의 컴포넌트 참조와 변동이 절대로 없는 외부 컴포넌트의 참조를 잡는데 사용합니다.<br/>
    /// </summary>
    private void OnEnable()
    {
        // 자기자신 2d 트랜스폼 참조(최초 생성 시 - 자신 컴포넌트)
        itemRectTr = transform.GetComponent<RectTransform>();   

        itemImage = GetComponent<Image>();
        countTxt = GetComponentInChildren<Text>();
        
        itemCG = GetComponent<CanvasGroup>();
        visualManager = GameObject.FindWithTag("GameController").GetComponent<VisualManager>();
        worldInventoryInfo = visualManager.GetComponentInChildren<InventoryInfo>();
    }




    /// <summary>
    /// 아이템이 인벤토리 내부에 추가되는 경우에 호출 할 메서드입니다.<br/>
    /// 아이템의 기존 정보를 바탕으로 인벤토리에 추가되고 표현되기 위한 작업을 진행합니다.<br/><br/>
    /// 현재 InventoryInfo의 AddItem메서드와 Load메서드에서 사용되고있습니다.<br/>
    /// </summary>
    public void OnItemAdded(InventoryInfo inventoryInfo)
    {
        /*** 인자 미 전달 시 예외처리 ***/
        if(inventoryInfo==null)
            throw new Exception("이 메서드는 인벤토리 정보가 반드시 필요합니다. 확인하여 주세요");
        
        // 월드 상에서 추가되었다면 아이템 구조를 변경합니다
        if(isWorldPositioned)
            DimensionShift(false);
        
        // 텍스트 정보를 최신화 합니다.     
        UpdateTextInfo();              

        // 인벤토리 정보를 최신화합니다
        UpdateInventoryInfo(inventoryInfo); 

        // 슬롯 위치 정보를 최신화합니다
        UpdatePositionInfo();
    }
        


    /// <summary>
    /// 아이템이 월드에 새롭게 생성되는경우에 호출해야 할 메서드입니다.<br/>
    /// 아이템 정보를 바탕으로 오브젝트를 구성합니다.<br/><br/>
    /// 현재 CreateManager의 CreateItem메서드에서 사용되고 있습니다.<br/>
    /// </summary>
    public void OnItemCreated()
    {   
        // 아이템의 월드 상태를 활성화합니다.
        isWorldPositioned = true;   

        // 2D오브젝트와 분리되어서 생성된 3D 오브젝트의 계층 정보를 참조합니다
        // (2D오브젝트 상위에 Canvas를 둘 때 한 번더 수정될 가능성 있음!)
        itemTr = transform.parent;

        // 3D오브젝트의 콜라이더 정보를 참조합니다.
        itemCol = itemTr.GetComponent<Collider>();      

        // 3D 오브젝트의 리지드바디 정보를 참조합니다.
        itemRb = itemTr.GetComponent<Rigidbody>();
        
        // 아이템의 중첩수량 텍스트를 초기화합니다.
        InitCountTxt();

        // 아이템 오브젝트의 고유 정보를 읽어 들여 2D 오브젝트에 반영합니다
        UpdateImage();          

        // 텍스트 정보를 최신화 합니다.
        UpdateTextInfo();                   
                
        // 인벤토리 정보를 초기화합니다
        UpdateInventoryInfo(null);
        
        // 2D 기능을 중단한 상태로 생성합니다.
        SwitchAppearAs2D(true);
    }


    





    /// <summary>
    /// 아이템의 이미지 정보를 받아와서 오브젝트에 반영합니다.<br/>
    /// Item 클래스는 정의될 때 외부에서 참조할 이미지 인덱스를 저장하고 있습니다.<br/>
    /// 해당 인덱스를 참고하여 인스펙터뷰에 등록된 이미지를 참조합니다.
    /// </summary>
    public void UpdateImage()
    {        
        // 참조할 스프라이트 이미지를 visual 관리 메서드를 통해 자신의 정보를 전달하여 참조값을 받아 저장합니다.
        innerSprite = visualManager.GetItemSprite(this, SpriteType.innerSprite);
        statusSprite = visualManager.GetItemSprite(this, SpriteType.statusSprite);
        
        // 참조한 스프라이트 이미지를 기반으로 아이템이 보여질 2D이미지를 장착합니다.
        itemImage.sprite = innerSprite;
    }


    /// <summary>
    /// 아이템의 텍스트 정보를 업데이트 합니다.<br/>
    /// 아이템의 내부 수치를 수정했을 때 자동으로 호출해주기 위한 메서드입니다.
    /// </summary>
    public void UpdateTextInfo()
    {        
        // 상태창에 표시될 스펙정보를 최신화 합니다.
        strSpec = GetItemSpec();

        // 잡화 아이템이라면 중첩 수량정보를 최신화합니다.
        ItemMisc itemMisc = item as ItemMisc;
        if(itemMisc!=null)
            countTxt.text = itemMisc.OverlapCount.ToString();
    }


    /// <summary>
    /// 아이템의 중첩 수량표시를 초기화합니다.<br/>
    /// 잡화 아이템 종류는 수량 표시를 활성화하고, 비잡화 아이템의 경우 수량표시를 비활성화합니다.
    /// </summary>
    private void InitCountTxt()
    {
        if(item.Type is ItemType.Misc)
            countTxt.enabled = true;
        else
            countTxt.enabled = false;
    }








        
    /// <summary>
    /// 현재 아이템이 슬롯 리스트에 속해있다면 위치를 슬롯 인덱스에 맞게 최신화시켜줍니다.<br/>
    /// 슬롯 인덱스가 잘못되어 있다면 다른 위치로 이동 할 수 있습니다.<br/><br/>
    /// ** 아이템이 월드 상에 있거나 슬롯 참조가 안 잡힌 경우 예외를 던집니다. **<br/>
    /// </summary>
    public void UpdatePositionInfo()
    {
        // 슬롯 리스트에 슬롯이 생성되어있지 않다면 하위로직을 실행하지 않습니다.
        if( slotListTr.childCount==0 )
        {
            Debug.Log( "현재 슬롯이 생성되지 않은 상태입니다." );
            return;
        }

        // 아이템의 타입을 받아옵니다.
        ItemType itemType = item.Type;


        /**** 아이템을 탭에 표시 하지 않을 조건 ****/
        // 1. 전체탭의 경우 퀘스트 아이템을 제외하고 표시합니다.
        // 2. 전체탭이 아니라면 현재활성화 탭과 아이템이 속한 탭이 일치해야 표시합니다.

        // 현재 활성화탭이 전체 탭인경우
        if(curActiveTab == TabType.All)
        {
            // 퀘스트 아이템이라면, EmptyList로 이동합니다.
            if( itemType==ItemType.Quest )
            {
                MoveToEmptyList(); // 이 아이템을 빈 리스트로 이동시킵니다.
                return;
            }
        }
        // 아이템이 전체탭이 아닌 경우, 아이템의 탭타입이 현재활성화 탭이 아니라면
        else if( Inventory.ConvertItemTypeToTabType( item.Type )!=curActiveTab )
        {
            MoveToEmptyList();      // 이 아이템을 빈 리스트로 이동시킵니다.
            return;
        }


        MoveToSlot();   // 아이템을 슬롯으로 이동시킵니다.
    }


    
    /// <summary>
    /// 아이템을 일시적으로 빈리스트로 이동시키는 메서드입니다.<br/>
    /// 아이템 생성 시 내부적으로 사용됩니다.
    /// </summary>
    /// <exception cref="Exception"></exception>
    private void MoveToEmptyList()
    {
        if(isWorldPositioned)
            throw new Exception("월드 상태일 때는 슬롯으로 이동할 수 없습니다.");
        if(emptyListTr==null)
            throw new Exception("슬롯 참조가 잡혀있지 않습니다. 확인하여 주세요.");

        itemRectTr.SetParent(emptyListTr, false);
    }

    /// <summary>
    /// 아이템의 인덱스 정보를 참조하여 해당되는 슬롯으로 이동시켜주는 메서드입니다.<br/>
    /// 현재 활성화 중인 탭 정보에 따라 이동시켜야 하므로 유의해서 사용해야 합니다.<br/>
    /// </summary>
    private void MoveToSlot()
    {
        if(isWorldPositioned)
            throw new Exception("월드 상태일 때는 슬롯으로 이동할 수 없습니다.");
        if(slotListTr==null)
            throw new Exception("슬롯 참조가 잡혀있지 않습니다. 확인하여 주세요.");

        

        // 현재 활성화 중인 탭을 기반으로 어떤 인덱스를 참조할지 설정합니다.
        int activeIndex = isActiveTabAll? item.SlotIndexAll : item.SlotIndexEach;
        
        // 아이템의 크기를 슬롯리스트의 cell크기와 동일하게 맞춥니다.(슬롯의 크기와 동일하게 맞춥니다.)
        itemRectTr.sizeDelta = slotListTr.GetComponent<GridLayoutGroup>().cellSize;

        // 아이템의 부모를 해당 슬롯으로 설정합니다.
        itemRectTr.SetParent( slotListTr.GetChild(activeIndex) );  

        // 위치와 회전값을 수정합니다.
        itemRectTr.localPosition = Vector3.zero;
        itemRectTr.localRotation = Quaternion.identity;
    }









    /// <summary>
    /// 아이템의 인벤토리 정보가 변경될 때 인자로 받은 InventoryInfo를 기준으로 다시 설정해주는 메서드입니다.<br/>
    /// 새로운 인벤토리와 관련된 참조 목록을 전달인자를 바탕으로 초기화 합니다.<br/><br/>
    /// 아이템이 기존 인벤토리에서 움직이고 있는 경우는 호출될 필요가 없습니다.<br/>
    /// 아이템이 새로운 인벤토리로 들어오거나, 기존 인벤토리에서 나갈 때 호출해줘야 합니다.<br/>
    /// </summary>
    public void UpdateInventoryInfo(InventoryInfo newInventoryInfo)
    {        
        // null값이 전달된 경우는 월드로 나갔다고 판단합니다.
        if(newInventoryInfo == null)  
        {
            inventoryTr = null;
            inventoryInfo = null;
            inventoryInteractive = null;
            statusInteractive = null;

            slotListTr = null;
            emptyListTr = null;
            prevDropSlotTr = null;

            ownerTr = null;                    // 아이템 소유자 초기화
            item.OwnerId = -1;                 // 아이템 소유자 식별 번호를 초기화합니다.
            item.OwnerName = worldOwnerName;   // 아이템 소유자 명을 월드로 변경합니다.
        }
        else // 다른 인벤토리로 전달된 경우
        {
            // 인벤토리 참조 정보를 업데이트 합니다.
            inventoryInfo = newInventoryInfo;
            inventoryTr = inventoryInfo.transform;
            inventoryInteractive = inventoryTr.GetComponent<InventoryInteractive>();
            statusInteractive = inventoryTr.GetComponent<StatusWindowInteractive>();

            if(inventoryInfo == null || inventoryInteractive == null )
                throw new Exception("인벤토리 정보 참조가 잘못되었습니다. 확인하여 주세요.");
                        
            slotListTr = inventoryTr.GetChild(0).GetChild(0).GetChild(0);
            emptyListTr = inventoryTr.GetChild(0).GetChild(1);
                                         
            baseDropTr = inventoryInfo.baseDropTr;                      // 기본 드랍위치와 부모설정을 인벤토리로부터 참조
            isBaseDropSetParent = inventoryInfo.isBaseDropSetParent;      
            prevDropSlotTr = slotListTr.GetChild(item.SlotIndexEach);   // 이전 드롭이벤트 호출자를 현재 들어있는 슬롯으로 최신화                                  

            UpdateActiveTabInfo();                                      // 액티브 탭 정보를 최신화합니다.   

            ownerTr = inventoryInfo.OwnerTr;                // 아이템 소유자를 인벤토리 소유자로 결정합니다.
            item.OwnerId = inventoryInfo.OwnerId;           // 아이템 소유자 식별번호를 내부 아이템에 저장합니다.
            item.OwnerName = inventoryInfo.OwnerName;       // 아이템 소유자명을 인벤토리 소유자명으로 변경합니다.
        }      
    }

    /// <summary>
    /// 아이템이 현재 속한 인벤토리의 활성화 탭 정보를 업데이트합니다.<br/>
    /// InventoryInteractive스크립트에서 활성화 탭의 변경이 시도되었을 때 인터렉티브 스크립트 쪽에서 현재 들어있는 모든 아이템의
    /// 활성화 탭 정보를 최신화 하기 위해 호출하는 메서드입니다.<br/><br/>
    /// 다른 인벤토리로의 정보의 변동이 있을때, 혹은 같은 인벤토리 내에서 탭정보의 변동이 있을 때 호출을 진행합니다.<br/><br/>
    /// ** 현재 아이템이 속한 인벤토리의 Interactive 스크립트가 아니라면 예외가 발생합니다. **<br/>
    /// </summary>
    public void UpdateActiveTabInfo(InventoryInteractive caller, TabType curActiveTab, bool isActiveTabAll)
    {
        if(caller != inventoryInteractive)
            throw new Exception("변경 불가능한 호출자입니다. 확인하여주세요.");
                
        this.curActiveTab = curActiveTab;
        this.isActiveTabAll = isActiveTabAll; 
    }

    /// <summary>
    /// 아이템이 현재 속한 인벤토리의 활성화 탭 정보를 업데이트합니다.<br/>
    /// 아이템이 새롭게 추가되었을 때 아이템쪽에서 수동으로 Interactive스크립트에게서 값을 할당받습니다.<br/>
    /// </summary>
    private void UpdateActiveTabInfo()
    {
        curActiveTab = inventoryInteractive.CurActiveTab;
        isActiveTabAll = inventoryInteractive.IsActiveTabAll;
    }

     






    /// <summary>
    /// 아이템의 2D 모습을 중단하거나 다시 활성화시키는 메서드입니다.<br/>
    /// isWorldPositioned를 기반으로 최신화합니다.<br/>
    /// DimensionShift에서 내부 메서드로 사용되고 있습니다.
    /// </summary>
    public void SwitchAppearAs2D(bool isWorldPositioned)
    {        
        itemCG.interactable = !isWorldPositioned;
        itemCG.alpha = isWorldPositioned ? 0f:1f;
        itemImage.raycastTarget = !isWorldPositioned;
    }
           
    /// <summary>
    /// 2D와 3D 오브젝트의 계층관계를 변경하는 메서드입니다.<br/>
    /// isWorldPositioned를 기반으로 자동으로 최신화합니다.<br/>
    /// DimensionShift에서 내부 메서드로 사용되고 있습니다.
    /// </summary>
    private void ChangeHierarchy(bool isWorldPositioned)
    {
        if(isWorldPositioned)
        {  
            itemTr.gameObject.SetActive(true);      // 3D오브젝트를 활성화   
            itemTr.SetParent( null );               // 3D오브젝트의 부모를 슬롯에서 null으로 변경            
            itemRectTr.SetParent(itemTr);           // 2D오브젝트의 부모를 3D오브젝트로 변경
        }
        else
        {            
            itemRectTr.SetParent( emptyListTr );    // 2D 오브젝트의 부모를 빈공간으로 설정            
            itemTr.SetParent( itemRectTr );         // 3D 오브젝트의 부모를 2D 오브젝트로 설정
            itemTr.gameObject.SetActive( false );   // 3D 오브젝트 비활성화
        }
    }
      


    /// <summary>
    /// 아이템 정보를 2D UI에서 3D 월드 또는 3D월드에서 2D UI로 전환해주는 메서드입니다. 
    /// 월드 상태를 활성화 하고 오브젝트 계층구조를 변경하고, 모습 변경을 해주는 메서드입니다.<br/>
    /// </summary>
    public void DimensionShift(bool isMoveToWorld)
    {   
        // 월드상태 변수 활성화
        isWorldPositioned = isMoveToWorld;  
                
        // 계층구조를 변경합니다.
        ChangeHierarchy(isMoveToWorld);

        // 아이템의 모습을 변경합니다
        SwitchAppearAs2D(isMoveToWorld);
    }
     




    /// <summary>
    /// 아이템이 인벤토리에서 빠져나와 월드 상태로 전환되었을 때 드롭 할 위치를 설정해주기 위한 메서드입니다.<br/>
    /// **첫 번째 전달인자 - Transform의 정보를 아이템 오브젝트에 입력합니다.<br/><br/>
    /// 
    /// 인자를 미전달하면 지정해둔 드랍위치(플레이어)로 전송됩니다.(기본값: ItemInfo클래스의 playerDropTr)<br/><br/>
    /// 
    /// **두 번째 전달인자 - TransferType은 전달받은 Transform을 어디까지 적용시킬지 선택하는 옵션입니다.<br/>
    /// (기본값: 위치 값만 적용, 옵션을 통해 회전 값, 크기 값을 적용시 킬 수 있습니다.)<br/><br/>
    /// 
    /// **세 번째 전달 인자 - isSetParent를 true로 만들면 Transform 하위에 자식으로서 속하게 됩니다. (기본값: false)<br/><br/>
    /// </summary>
    public void OnItemWorldDrop(Transform dropPosTr=null, TrApplyRange transferType = TrApplyRange.Pos, bool isSetParent=false)
    {
        if( !IsWorldPositioned )
            inventoryInfo.RemoveItem(this);
        
        // 드롭 포지션이 직접 전달되지 않았다면,
        if(dropPosTr==null)
            dropPosTr = baseDropTr;
        
        // 3D 오브젝트의 부모를 설정
        if(isSetParent)
            itemTr.SetParent(dropPosTr);   
        
        // 3D 오브젝트의 위치와 회전값 설정 (TransferType에 따라서 Transform을 적용시킬 범위가 증가하게 됩니다.)         
        itemTr.position = dropPosTr.position;
                
        if( transferType >= TrApplyRange.Pos_Rot )
            itemTr.rotation = dropPosTr.rotation;

        if( transferType >= TrApplyRange.Pos_Rot_Scale)
            itemTr.localScale = dropPosTr.localScale;
    }



    /// <summary>
    /// 월드의 아이템을 습득하는 경우에 아이템 쪽에서 특정 인벤토리로 아이템을 추가하기 위해 필요한 메서드입니다.<br/>
    /// 인벤토리에서 AddItem메서드를 호출하는 것과 동일하므로 둘 중 하나만 사용하십시오.<br/>
    /// <br/><br/>
    /// *** 인벤토리 정보를 전달하지 않으면 예외가 발생합니다. ***
    /// </summary>
    /// <param name="inventoryInfo"></param>
    /// <returns>해당 인벤토리의 슬롯에 빈 자리가 없다면 false를 반환, 성공 시 true를 반환</returns>
    public bool OnItemWorldGain(InventoryInfo inventoryInfo)
    {        
        // 인자 미전달 시 예외처리
        if(inventoryInfo == null)
            throw new Exception("인벤토리 정보가 전달되지 않았습니다. 확인하여 주세요.");
        // 슬롯에 빈자리가 없다면 실패처리
        else if( !inventoryInfo.IsSlotEnough(this) )           
            return false;

        // 아이템을 월드에서 2D상태로 전환합니다.
        DimensionShift(false);

        // 아이템을 인벤토리에 추가합니다.
        inventoryInfo.AddItem(this);

        // 성공을 반환합니다.
        return true;
    }










    


    










}