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
            itemInfo.OnItemWorldGain(inventoryInfo);
        }
    }

}
