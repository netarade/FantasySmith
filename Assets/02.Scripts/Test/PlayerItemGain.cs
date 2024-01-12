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
    
    GraphicRaycaster gRaycaster;                // �κ��丮 ĵ������ �׷��ȷ���ĳ����
    PointerEventData pEventData;                // �׷��� ����ĳ���� �� ���� �� ������ �̺�Ʈ
    List<RaycastResult> raycastResults;         // �׷��� ����ĳ���� ����� ���� ����Ʈ

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
            inventoryInteractive.InventoryOpenSwitch();
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
                    InventoryInfo chestInven = hitInfo.collider.GetComponentInChildren<InventoryInfo>();
                    chestInven.InventoryOpenSwitch();
                    otherRaycaster = hitInfo.collider.GetComponentInChildren<GraphicRaycaster>();
                }
            }
        }


        // ���� �߿� ���콺 ��ư�� Ŭ���� ��� - �ݵ�� ���� ����
        if( Input.GetMouseButton( 2 ) )
        {
            raycastResults.Clear();

            // �̺�Ʈ �ý����� �� �����մϴ�.
            pEventData = new PointerEventData(EventSystem.current);
            
            // �̺�Ʈ�� �Ͼ �������� ���콺�� �ٽ� Ŭ������ ���� �������� �����մϴ�.
            pEventData.position=Input.mousePosition;

            // �׷��� ����ĳ��Ʈ�� ���� ����� �޽��ϴ�.
            gRaycaster.Raycast( pEventData, raycastResults );


            otherRaycastResults.Clear();

            //print("����ĳ������ �����մϴ�.");
            otherRaycaster.Raycast( pEventData, otherRaycastResults);

            // ����ĳ���ÿ� ������ ���(������ ������Ʈ�� �ִ� ���)
            if( raycastResults.Count>0 )
            {

                string objNames ="";

                for(int i=0; i<raycastResults.Count; i++ )
                    objNames += raycastResults[i].gameObject.name + " ";

                print( "[����Ǿ����ϴ�!]" + objNames);

            }
            else
            {
                print( "[������� �ʾҽ��ϴ�]");
            }


            if( otherRaycastResults.Count>0 )
            {

                string objNames ="";

                for(int i=0; i<otherRaycastResults.Count; i++ )
                    objNames += otherRaycastResults[i].gameObject.name + " ";

                print( "[�ݴ��ʿ� ����Ǿ����ϴ�!]" + objNames);

            }
            else
            {
                print( "[�ݴ��ʿ��� ������� �ʾҽ��ϴ�]");
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
            itemInfo.OnItemWorldGain(playerInven);
        }
    }

}
