using CreateManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerItemGain : MonoBehaviour
{






    InventoryInfo playerInven;
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

        playerInven = GetComponentInChildren<InventoryInfo>();
        inventoryInteractive = GetComponentInChildren<InventoryInteractive>();

        animator = GetComponent<Animator>();

        gRaycaster = GetComponentInChildren<GraphicRaycaster>();
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
            inventoryInteractive.InventoryOpenSwitch();
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
                    InventoryInfo chestInven = hitInfo.collider.GetComponentInChildren<InventoryInfo>();
                    chestInven.InventoryOpenSwitch();
                    otherRaycaster = hitInfo.collider.GetComponentInChildren<GraphicRaycaster>();
                }
            }
        }


        // 선택 중에 마우스 버튼을 클릭한 경우 - 반드시 선택 종료
        if( Input.GetMouseButton( 2 ) )
        {
            raycastResults.Clear();

            // 이벤트 시스템을 재 설정합니다.
            pEventData = new PointerEventData(EventSystem.current);
            
            // 이벤트가 일어날 포지션을 마우스를 다시 클릭했을 때의 지점으로 설정합니다.
            pEventData.position=Input.mousePosition;

            // 그래픽 레이캐스트를 통해 결과를 받습니다.
            gRaycaster.Raycast( pEventData, raycastResults );


            otherRaycastResults.Clear();

            //print("레이캐스팅을 시작합니다.");
            otherRaycaster.Raycast( pEventData, otherRaycastResults);

            // 레이캐스팅에 성공한 경우(검출한 오브젝트가 있는 경우)
            if( raycastResults.Count>0 )
            {

                string objNames ="";

                for(int i=0; i<raycastResults.Count; i++ )
                    objNames += raycastResults[i].gameObject.name + " ";

                print( "[검출되었습니다!]" + objNames);

            }
            else
            {
                print( "[검출되지 않았습니다]");
            }


            if( otherRaycastResults.Count>0 )
            {

                string objNames ="";

                for(int i=0; i<otherRaycastResults.Count; i++ )
                    objNames += otherRaycastResults[i].gameObject.name + " ";

                print( "[반대쪽에 검출되었습니다!]" + objNames);

            }
            else
            {
                print( "[반대쪽에는 검출되지 않았습니다]");
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
            itemInfo.OnItemWorldGain(playerInven);
        }
    }

}
