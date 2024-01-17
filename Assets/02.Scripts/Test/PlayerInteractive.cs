using CreateManagement;
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
 */




public class PlayerInteractive : MonoBehaviour
{


    InventoryInfo playerInventory;
    QuickSlot quickSlot;

    Animator animator;
    bool bMouseCoolTime = true;
    
    void Start()
    {
        int childIdx = transform.childCount-1;
        playerInventory = transform.GetChild(childIdx).GetChild(0).GetComponent<InventoryInfo>();
        quickSlot = transform.GetChild(childIdx).GetChild(1).GetComponent<QuickSlot>();
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


        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("Ű�е�1");
            quickSlot.ItemEquipSwitch(1);

        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            print("Ű�е�2");
            quickSlot.ItemEquipSwitch(2);
        }




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
