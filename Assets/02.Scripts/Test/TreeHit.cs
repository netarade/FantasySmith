using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreeHit : MonoBehaviour
{
    Transform dropTr;
    CreateManager createManager;
    bool isHitCoolTime = false;


    int hp = 10;
    void Start()
    {
        dropTr = transform.GetChild(0);
        createManager = GameObject.FindWithTag("GameController").GetComponent<CreateManager>();

    }

    private void OnTriggerEnter( Collider other )
    {
        if( other.CompareTag("ITEM") && !isHitCoolTime )
        {
            isHitCoolTime = true;
            StartCoroutine( ActivateHitCoolTime(1.5f) );

            hp-=1;

            if(hp==7)
                createManager.CreateWorldItem("단단한 나뭇가지", 2).OnItemWorldDrop(dropTr);


            InventoryInfo inventoryInfo = null;
            Transform parentTr = other.transform;
            Transform nextParentTr = parentTr.parent;
            
            while (nextParentTr != null)
            {      
                parentTr = nextParentTr;        // 이전 부모 기록
                nextParentTr = parentTr.parent; 
            }

            inventoryInfo = parentTr.GetComponentInChildren<InventoryInfo>();


            if(hp==5)
                createManager.CreateWorldItem("사냥꾼의 활").OnItemWorldGain(inventoryInfo);

        }      
    }

    IEnumerator ActivateHitCoolTime(float time)
    {
        yield return new WaitForSeconds(time);
        isHitCoolTime = false;
    }
}
