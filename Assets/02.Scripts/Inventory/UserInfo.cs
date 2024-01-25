using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * [작업사항]
 * <v1.0 - 2024_0106_최원준>
 * 1- 유저의 고유 식별번호(ID)를 할당받아 보관하고 있는 스크립트 작성
 * (계층 최상위에 부착해야 하는 스크립트)
 * 
 * 
 */

public class UserInfo : MonoBehaviour
{
    [SerializeField] int userId;    // 유저 고유 식별번호 (원래는 로비, 로그인화면에서 지정되어야 하지만, 현재는 인스펙터뷰로 지정)

    /// <summary>
    /// 유저 프리팹의 오브젝트 명을 반환합니다.
    /// </summary>
    [HideInInspector]
    public string UserPrefabName {get {return gameObject.name;} } 
                
    /// <summary>
    /// 유저 고유의 식별번호를 반환합니다.
    /// </summary>
    [HideInInspector]
    public int UserId { get {return userId;} }

    /// <summary>
    /// 유저가 가지고 있는 인벤토리 정보를 반환합니다.
    /// </summary>
    [HideInInspector]
    public InventoryInfo inventoryInfo;

    /// <summary>
    /// 유저가 가지고 있는 퀵슬롯 정보를 반환합니다.
    /// </summary>
    [HideInInspector]
    public QuickSlot quickSlot;

    private void Awake()
    {
        inventoryInfo = GetComponentInChildren<InventoryInfo>();
        quickSlot = GetComponentInChildren<QuickSlot>();
    }
}
