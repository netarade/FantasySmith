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
* 
*/
public class InventoryInitializer : MonoBehaviour
{

    [Header("인벤토리 모든 설정 리셋")]
    public bool isReset = false;
    
    [Header("인벤토리 식별 번호")]
    public int inventoryId = -1;

    [Header("아이템을 3d상태로 보관하는 인벤토리 여부")]
    public bool isItem3dStore = false;

    [Header("게임 중에 생성되는 인벤토리 여부")]
    public bool isInstantiated = false;


    [Header("딕셔너리와 슬롯 제한수를 지정(같은 딕셔너리 불가)")]
    public DicType[] dicTypes;
    
    [Header("활성화 할 액티브탭의 종류를 지정")]
    [Header("(길이가 0이면 탭표시 안함 - 전체탭으로 작동)")]
    [Header("(표시탭 종류 All-전체, Quest-퀘스트, Misc-잡화, Weapon-장비)")]
    public TabType[] showTabType;


    public InventoryInfo inventoryInfo;

    private void Awake()
    {
        inventoryInfo = GetComponent<InventoryInfo>();
    }

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