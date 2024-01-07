using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemGain : MonoBehaviour
{
    InventoryInfo inventoryInfo;
    Animator animator;
    bool bMouseCoolTime = true;

    void Start()
    {
        inventoryInfo = GetComponentInChildren<InventoryInfo>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1) && bMouseCoolTime)
        {
            bMouseCoolTime = false;
            animator.SetTrigger("Attack");
            StartCoroutine( MouseDelayTime(1f) );
        }
    }

    IEnumerator MouseDelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        bMouseCoolTime = true;
    }

    public void Hit()
    {
        
    }

    private void OnTriggerEnter( Collider other )
    {
        ItemInfo itemInfo = other.GetComponentInChildren<ItemInfo>();
        
        if( itemInfo != null)
        {
            itemInfo.OnItemWorldGain(inventoryInfo);
        }
    }

}
