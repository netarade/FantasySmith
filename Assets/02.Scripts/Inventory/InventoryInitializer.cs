using System;
using UnityEngine;
using ItemData;
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
 */
public class InventoryInitializer : MonoBehaviour
{
    [Header("인벤토리 모든 설정 리셋")]
    public bool isReset;

    [Header("딕셔너리와 슬롯 제한수를 지정(같은 딕셔너리 불가)")]
    public DicType[] dicTypes;
    
    [Header("----- 아래부터 미완성 -----")]
    [Header("저장파일 이름과 저장 슬롯을 직접 설정")]
    public bool isCustomFileSetting;
    public string fileName;
    public int saveSlotNo;

    [Header ("슬롯칸 공유 옵션")]
    [Header("(개별탭 생성이 불가능하며, 전체탭으로만 볼 수 있습니다.)")]
    public bool isShare;

    [Header ("슬롯칸 공유 시 제한 칸수 지정")]
    public int shareSlotCountLimit;

    [Header ("슬롯 공유 시 제외 타입(전체탭 선택 금지)")]
    public DicType[] shareExceptTabTypes;
}

[Serializable]
public struct DicType
{
    public ItemType itemType;
    public int slotLimit;
}