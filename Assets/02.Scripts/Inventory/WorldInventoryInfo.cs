using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInventoryInfo : InventoryInfo
{

    protected override void Start()
    {        
        /** 호출 순서 고정: 로드->인터렉티브스크립트 초기화 및 슬롯생성요청->아이템표현 ***/
        this.LoadInventoryData();         // 저장된 플레이어 데이터를 불러옵니다. 
        interactive.Initialize(this);     // 인터렉티브 스크립트 초기화를 진행합니다. 
        LoadAllItemTransformInfo();       // 아이템의 3D 정보를 업데이트 합니다.

        StartCoroutine( SaveAllInfoTimeCheck(10.0f) );
    }


    IEnumerator SaveAllInfoTimeCheck(float checkDuration)
    {
        Debug.Log("월드아이템 저장을 진행 중입니다.");
        SaveAllItemTransformInfo();
        yield return new WaitForSeconds(checkDuration);
    }




    /// <summary>
    /// 인벤토리가 보유하고 있는 모든 사전의 아이템 3D 오브젝트의 Transform 정보를 최신화합니다.<br/>
    /// 3D 아이템을 보관하는 인벤토리에서 로드 이후 호출되어져야 하는 메서드입니다.<br/>
    /// </summary>
    protected void LoadAllItemTransformInfo()
    {
        Dictionary<string, List<ItemInfo>> itemDic;                     // 참조할 아이템 사전을 선언합니다.
            
        for(int i=0; i<inventory.dicLen; i++)                           // 인벤토리 사전의 갯수만큼 반복합니다.
        {
            itemDic =inventory.GetItemDic( inventory.dicType[i] );      // 아이템 종류에 따른 인벤토리의 사전을 할당받습니다.
                          
            // 아이템 사전이 없거나 리스트가 존재하지 않는다면 다음 사전을 참조합니다.
            if(itemDic==null || itemDic.Count==0)   
                continue;

            // 인벤토리 사전에서 ItemInfo를 하나씩 꺼내어 가져옵니다.
            foreach( List<ItemInfo> itemInfoList in itemDic.Values )    
            {
                foreach( ItemInfo itemInfo in itemInfoList )
                {
                    // 아이템의 STransform 정보를 불러와서 Transform에 동기화시켜줍니다.
                    itemInfo.SerializedTr.Deserialize( itemInfo.ItemTr );
                }
            }
        }
    }

    /// <summary>
    /// 인벤토리가 보유하고 있는 모든 사전의 아이템 3D 오브젝트의 Transform 정보를 최신화합니다.<br/>
    /// 3D 아이템을 보관하는 인벤토리에서 로드 이후 호출되어져야 하는 메서드입니다.<br/>
    /// </summary>
    protected void SaveAllItemTransformInfo()
    {
        Dictionary<string, List<ItemInfo>> itemDic;                     // 참조할 아이템 사전을 선언합니다.
            
        for(int i=0; i<inventory.dicLen; i++)                           // 인벤토리 사전의 갯수만큼 반복합니다.
        {
            itemDic =inventory.GetItemDic( inventory.dicType[i] );      // 아이템 종류에 따른 인벤토리의 사전을 할당받습니다.
                          
            // 아이템 사전이 없거나 리스트가 존재하지 않는다면 다음 사전을 참조합니다.
            if(itemDic==null || itemDic.Count==0)   
                continue;

            // 인벤토리 사전에서 ItemInfo를 하나씩 꺼내어 가져옵니다.
            foreach( List<ItemInfo> itemInfoList in itemDic.Values )    
            {
                foreach( ItemInfo itemInfo in itemInfoList )
                {
                    // 아이템의 STransform 정보를 불러와서 Transform을 반영하여 저장합니다.
                    itemInfo.SerializedTr.Serialize( itemInfo.ItemTr );
                }
            }
        }
    }






}
