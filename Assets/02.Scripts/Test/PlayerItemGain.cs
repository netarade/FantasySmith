using CreateManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerItemGain : MonoBehaviour
{






    InventoryInfo playerInventory;
    InventoryInteractive inventoryInteractive;

    Animator animator;
    bool bMouseCoolTime = true;
    
    GraphicRaycaster gRaycaster;                // �κ��丮 ĵ������ �׷��ȷ���ĳ����
    PointerEventData pEventData;                // �׷��� ����ĳ���� �� ���� �� ������ �̺�Ʈ
    List<RaycastResult> raycastResults;         // �׷��� ����ĳ���� ����� ���� ����Ʈ

    GraphicRaycaster otherRaycaster;
    List<RaycastResult> otherRaycastResults;
    void Start()
    {

        playerInventory = GetComponentInChildren<InventoryInfo>();
        inventoryInteractive = GetComponentInChildren<InventoryInteractive>();

        animator = GetComponent<Animator>();

        gRaycaster = transform.GetChild(4).GetChild(0).GetComponentInChildren<GraphicRaycaster>();
        raycastResults = new List<RaycastResult>();
        otherRaycastResults = new List<RaycastResult>();
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
