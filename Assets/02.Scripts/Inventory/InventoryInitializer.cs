using System;
using UnityEngine;
using ItemData;
/*
 * [작업 사항]  
 * <v1.0 - 2023_1106_최원준>
 * 1 - 인벤토리를 원하는 사전과 원하는 제한 크기만큼 생성하기 위한 옵션 추가
 * 
 * 
 */
public class InventoryInitializer : MonoBehaviour
{
    [Header("리셋 버튼")]
    public bool isReset;

    [Header("딕셔너리와 제한수를 지정(같은 딕셔너리 불가)")]
    public DicType[] dicTypes;   
}

[Serializable]
public struct DicType
{
    public ItemType itemType;
    public int slotLimit;
}