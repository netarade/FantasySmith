using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemGain : MonoBehaviour
{
    InventoryInfo inventoryInfo;
    InventoryInteractive inventoryInteractive;

    Animator animator;
    bool bMouseCoolTime = true;

    void Start()
    {
        inventoryInfo = GetComponentInChildren<InventoryInfo>();
        inventoryInteractive = GetComponentInChildren<InventoryInteractive>();

        animator = GetComponent<Animator>();
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
            itemInfo.OnItemWorldGain(inventoryInfo);
        }
    }

}
