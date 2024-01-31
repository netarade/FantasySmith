using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * [�۾�����]
 * <v1.0 - 2024_0106_�ֿ���>
 * 1- ������ ���� �ĺ���ȣ(ID)�� �Ҵ�޾� �����ϰ� �ִ� ��ũ��Ʈ �ۼ�
 * (���� �ֻ����� �����ؾ� �ϴ� ��ũ��Ʈ)
 * 
 * 
 */

public class UserInfo : MonoBehaviour
{
    [SerializeField] int userId;    // ���� ���� �ĺ���ȣ (������ �κ�, �α���ȭ�鿡�� �����Ǿ�� ������, ����� �ν����ͺ�� ����)

    /// <summary>
    /// ���� �������� ������Ʈ ���� ��ȯ�մϴ�.
    /// </summary>
    [HideInInspector]
    public string UserPrefabName {get {return gameObject.name;} } 
                
    /// <summary>
    /// ���� ������ �ĺ���ȣ�� ��ȯ�մϴ�.
    /// </summary>
    [HideInInspector]
    public int UserId { get {return userId;} }

    /// <summary>
    /// ������ ������ �ִ� �κ��丮 ������ ��ȯ�մϴ�.
    /// </summary>
    [HideInInspector]
    public InventoryInfo inventoryInfo;

    /// <summary>
    /// ������ ������ �ִ� ������ ������ ��ȯ�մϴ�.
    /// </summary>
    [HideInInspector]
    public QuickSlot quickSlot;

    private void Awake()
    {
        inventoryInfo = GetComponentInChildren<InventoryInfo>();
        quickSlot = GetComponentInChildren<QuickSlot>();
    }
}
