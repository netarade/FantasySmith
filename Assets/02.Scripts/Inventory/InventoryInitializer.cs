using System;
using UnityEngine;
using ItemData;
using InventoryManagement;
/*
* [작업 사항]  
* <v1.0 - 2024_0101_최원준>
* 1 - 인벤토리를 원하는 사전과 원하는 제한 크기만큼 생성하기 위한 옵션 추가
* 
* <v1.1 - 2024_0102_최원준>
* 1- 슬롯 칸공유 옵션을 추가
* 이유는 전체 슬롯으로 모든것을 추가하기 위함
* 
* 2- 저장파일 이름 직접 설정 옵션을 추가 
* 
* <v2.0 - 2024_0115_최원준>
* 1- 표시탭옵션을 Interactive클래스에서 옮겨옴.
* 2- 공유옵션 삭제 (반드시 공유옵션이 들어가야하므로)
* 
* <v2.1 - 2024_0125_최원준>
* 1- 파일설정을 삭제하고 인벤토리 식별번호를 타나내는 inventoryId를 추가
* 
* 2- 인벤토리 종류를 구분하기 위해 isItem3DStore와 isInstantiated 속성을 추가하였음
* (각각 3d 아이템 보관 인벤토리와 게임 진행 중에 생성되는 인벤토리인지를 알려주는 속성)
* 
* <v2.2 - 2024_0125_최원준>
* 1- 인벤토리 식별번호 inventoryId를 ownerId로 변경
* 인벤토리 식별번호가 필요한 것이 아니라 인벤토리 소유자 Id의 세팅이 필요한것 이므로
* 
* <v3.0 - 2024_0126_최원준>
* 1- 이니셜라이져 이전상태(v2.0)으로 롤백
* 
* 2- 인벤토리 소유자 식별 번호를 삭제
* 이유는 플레이어에 UserInfo스크립트를 부착하여 메서드 전달수단으로 사용하기로 하였기 때문.
* 
* 3- 인벤토리 종류를 구분하는 isItem3dStore, isInstantiated속성을 삭제
* 아이템을 3D로 보관하는 인벤토리 속성의 경우 - 월드 인벤토리 전용 클래스인 WorldInventoryInfo에서 오버라이딩을 통해 조절할 수 있으며,
* 동적할당 인벤토리의 경우(즉, 아이템형 인벤토리의 경우) 계층 최상위 부모에 UserInfo가 존재하는지 여부에 따라 구분할 수 있기 때문
* 
* 
*/
public class InventoryInitializer : MonoBehaviour
{

    [Header("인벤토리 모든 설정 리셋")]
    public bool isReset = false;
    
    [Header("중심 인벤토리 속성 - 동일 계층 다중 인벤토리가 있는 경우 하나만 체크")]
    public bool isServer = false;

    [Header("전체탭 공유 옵션 - 퀘스트를 제외한 모든 종류의 슬롯 제한수가 공유됩니다.")]
    public bool isShareAll = false;

    [Header("딕셔너리와 슬롯 제한수를 지정(같은 딕셔너리 불가)")]
    public DicType[] dicTypes;
    
    [Header("활성화 할 액티브탭의 종류를 지정")]
    [Header("(길이가 0이면 탭표시 안함 - 전체탭으로 작동)")]
    [Header("(표시탭 종류 All-전체, Quest-퀘스트, Misc-잡화, Weapon-장비)")]
    public TabType[] showTabType;


}


/// <summary>
/// 딕셔너리의 종류와 슬롯 제한수를 나타내는 구조체입니다.<br/>
/// Initializer에서 Inventory로 전달하여 초기화 값을 주기 위한용도로 사용됩니다.<br/>
/// </summary>
[Serializable]
public struct DicType
{
    public ItemType itemType;
    public int slotLimit;
}