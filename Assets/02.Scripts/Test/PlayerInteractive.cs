using CreateManagement;
using ItemData;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * [�۾� ����]
 * 
 * <v1.0>
 * 1- �÷��̾� �׽�Ʈ�� ��ũ��Ʈ �ۼ�
 * 
 * <v2.0>
 * 1- EŰ�� ����Ͽ� �κ��丮�� ���� �ڵ� �׽�Ʈ
 * 
 * <v3.0 - 2024_0117_�ֿ���>
 * 1- ����Ű�е带 ����Ͽ� �������� ���� �ڵ� �׽�Ʈ
 * 2- ��ũ��Ʈ�� PlayerInteractive�� ����
 * 
 * <v3.1 - 2024_0118_�ֿ���>
 * 1- �����԰� �÷��̾��κ��丮 �������� (�������� ����ã�� �� �������� ���� ã�� ���·�) ����
 *
 */




public class PlayerInteractive : MonoBehaviour
{


    InventoryInfo playerInventory;
    QuickSlot quickSlot;

    Animator animator;
    bool bMouseCoolTime = true;
    
    float playerHp = 100f;
    float playerHunger = 10f;
    float playerThirsty = 10f;
        
    void Start()
    {
        quickSlot = GetComponentInChildren<QuickSlot>();
        playerInventory = quickSlot.transform.parent.GetChild(0).GetComponent<InventoryInfo>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // �÷��̾ ���ݹ�ư�� ������ ��Ȳ
        if(Input.GetMouseButtonDown(1) && bMouseCoolTime)
        {
            bMouseCoolTime = false;
            animator.SetTrigger("Attack");
            StartCoroutine( MouseDelayTime(1f) );
        }

        // �÷��̾ �κ��丮 â�� ���� ��Ȳ
        if(Input.GetKeyDown(KeyCode.I))
        {
            playerInventory.InventoryOpenSwitch();
        }
        

        // �÷��̾ �κ��丮�� ���� ��Ȳ
        Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(clickRay.origin, clickRay.direction*100f, Color.cyan);

        if(Input.GetKeyDown(KeyCode.E))
        {            
            RaycastHit hitInfo;

            if( Physics.Raycast(clickRay, out hitInfo, Mathf.Infinity) )
            {
                if( hitInfo.collider.CompareTag("Inventory") )
                {
                    InventoryInfo chestInventory = hitInfo.collider.GetComponentInChildren<InventoryInfo>();
                    playerInventory.ConnectInventory(chestInventory);
                }
            }
        }


        // ������ ��ư�� ������ ��Ȳ
        if(Input.GetKeyDown(KeyCode.Alpha1))
            quickSlot.ItemEquipSwitch(1);

        if(Input.GetKeyDown(KeyCode.Alpha2))
            quickSlot.ItemEquipSwitch(2);

        if(Input.GetKeyDown(KeyCode.Alpha3))
            quickSlot.ItemEquipSwitch(3);
        
        if(Input.GetKeyDown(KeyCode.Alpha4))
            quickSlot.ItemEquipSwitch(4);

        if(Input.GetKeyDown(KeyCode.Alpha5))
            quickSlot.ItemEquipSwitch(5);

        if(Input.GetKeyDown(KeyCode.Keypad0))
            quickSlot.ItemEquipSwitch(5);

        // ���콺 ��Ŭ���� ������ ��Ȳ
        // if(Input.GetMouseButtonDown(1))
        //    playerInventory.

    }


    /// <summary>
    /// itemStatus�� ������ ��ġ��ŭ �÷��̾��� �������ͽ� ��ġ�� ������Ű�� �޼����Դϴ�.<br/>
    /// ������ �ʿ��� ȣ��Ǵ� �뵵�� ���
    /// </summary>
    public void VaryPlayerStatus(ItemStatus status)
    {
        playerHp += status.hp;
        playerHunger += status.hunger;
        playerThirsty += status.thirsty;
    }

    public void PrintDebugInfo()
    {
        string debugStr = string.Format(
            $"ü�� : {playerHp}" +
            $"��� : {playerHunger}" +
            $"���� : {playerThirsty}"
            );

        Debug.Log( debugStr );
    }




    /// <summary>
    /// ���� ���콺 ��ư�� ��Ÿ�� ����
    /// </summary>
    IEnumerator MouseDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        bMouseCoolTime = true;
    }

    // �ִϸ��̼� �̺�Ʈ �߻� �� ȣ��
    public void Hit()
    {
        
    }

    /// <summary>
    /// �÷��̾ �������� �����ϴ� ��Ȳ
    /// </summary>
    private void OnTriggerEnter( Collider other )
    {
        ItemInfo itemInfo = other.GetComponentInChildren<ItemInfo>();
        
        if( itemInfo != null)
        {
            itemInfo.OnItemWorldGain(playerInventory);
        }
    }

}
