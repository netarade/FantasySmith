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
    
    GraphicRaycaster gRaycaster;                // 인벤토리 캔버스의 그래픽레이캐스터
    PointerEventData pEventData;                // 그래픽 레이캐스팅 시 전달 할 포인터 이벤트
    List<RaycastResult> raycastResults;         // 그래픽 레이캐스팅 결과를 받을 리스트

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
        // 플레이어가 공격버튼을 누르는 상황
        if(Input.GetMouseButtonDown(1) && bMouseCoolTime)
        {
            bMouseCoolTime = false;
            animator.SetTrigger("Attack");
            StartCoroutine( MouseDelayTime(1f) );
        }

        // 플레이어가 인벤토리 창을 여는 상황
        if(Input.GetKeyDown(KeyCode.I))
        {
            playerInventory.InventoryOpenSwitch();
        }
        

        // 플레이어가 인벤토리를 여는 상황
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
    /// 공격 마우스 버튼의 쿨타임 간격
    /// </summary>
    IEnumerator MouseDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        bMouseCoolTime = true;
    }

    // 애니메이션 이벤트 발생 시 호출
    public void Hit()
    {
        
    }

    /// <summary>
    /// 플레이어가 아이템을 습득하는 상황
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
