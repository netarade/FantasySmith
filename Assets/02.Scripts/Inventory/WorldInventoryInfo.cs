using DataManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * [작업사항]
 * <v1.0 - 2024_0125_최원준>
 * 1- 3D 상태로 아이템을 보관하는 인벤토리 클래스를 작성
 * 
 * 2- LoadAllItemTransformInfo를 기존 InventoryInfo에서 옮겨왔으며,
 * 추가로 SaveAllItemTransformInfo메서드 작성
 * 
 * <v1.1 - 2024_0125_최원준>
 * 1- SaveAllInfoTimeCheck메서드명 SaveAllItemTransformInfoInterval로 변경
 * 매개변수명을 timeDuration에서 interval로 수정
 * 
 * <v2.0 - 2024_0210_최원준>
 * 1- SaveAllItemTransformInfoInterval 주기저장 메서드 삭제하고 관련 코드를 삭제 
 * (Start에서 코루틴 호출하는 것을 삭제, saveInterval 변수 삭제)
 * 
 * 이유는 아이템 위치 정보를 주기 저장할 것이 아니라 인벤토리가 저장될 때 같이 저장되는 형태가 되어야 하기 때문
 * (즉, 인벤토리 정보가 주기저장 혹은 수동 저장되기 직전에 한 번만 호출되면 되기 때문)
 * 
 * 2- SaveInventoryData를 오버라이딩 하여 
 * 인벤토리 정보 저장 직전 SaveAllItemTransformInfo메서드를 호출하여 아이템 STransform정보를 저장하도록 하였음
 * 
 * 3- InitOpenState메서드의 호출을 인벤토리 로드 이후로 설정 (기존 부모스크립트의 Awake에서 Start로 변경)
 * 이유는 내부적으로 할당된 변수를 사용하여 null레퍼런스가 잡히기 때문인데 이는 인벤토리 내부 모든 아이템의 2D 상태를 비활성화하기 때문
 * 
 * 4- loadSlotNo를 UIManager에서 설정한 PlayerPrefs의 SlotNo 키값을 받아와서 설정하도록 변경
 * 
 */


public class WorldInventoryInfo : InventoryInfo
{
    protected override void Start()
    {        
        /** 호출 순서 고정: 로드->아이템표현 ***/

        // 로드할 슬롯번호 - SlotNo 키값이 존재하지 않는다면(새 게임이라면) 0을, 존재한다면(기존 게임이라면) 해당 키값을 받아서 설정합니다.
        int loadSlotNo = 0;    
        if(PlayerPrefs.HasKey("SlotNo"))
            loadSlotNo = PlayerPrefs.GetInt("SlotNo");        
        
        LoadInventoryData(loadSlotNo);          // 저장된 플레이어 데이터를 불러옵니다. 
        LoadAllItemTransformInfo();             // 월드아이템의 Transform정보를 업데이트 합니다.
        InitOpenState(false);                   // 인벤토리의 오픈상태를 꺼짐으로 만듭니다
    }

    
    /// <summary>
    /// 아이템의 STransform 정보를 저장함과 동시에 인벤토리 관련 데이터를 저장합니다.
    /// </summary>
    protected override void SaveInventoryData( int slotNo )
    {
        SaveAllItemTransformInfo();         // 인벤토리 세이브 전 내부 아이템의 STransform 정보를 업데이트 합니다.
        base.SaveInventoryData(slotNo);     
    }












    /// <summary>
    /// 인벤토리가 보유하고 있는 모든 사전의 아이템 3D 오브젝트의 Transform 정보를 최신화합니다.<br/>
    /// 3D 아이템을 보관하는 인벤토리에서 로드 이후 호출되어져야 하는 메서드입니다.<br/>
    /// </summary>
    protected void LoadAllItemTransformInfo()
    {
        Dictionary<string, List<ItemInfo>> itemDic;                     // 참조할 아이템 사전을 선언합니다.
            
        if(inventory==null)
            Debug.Log("월드 인벤토리가 존재하지 않습니다.");
        else if(inventory.dicLen == 0)
            Debug.Log("월드 인벤토리의 사전이 존재하지 않습니다.");

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
                    itemInfo.SerializedTr.Deserialize( itemInfo.Item3dTr );
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
            itemDic = inventory.GetItemDic( inventory.dicType[i] );      // 아이템 종류에 따른 인벤토리의 사전을 할당받습니다.
                          
            // 아이템 사전이 없거나 리스트가 존재하지 않는다면 다음 사전을 참조합니다.
            if(itemDic==null || itemDic.Count==0)   
                continue;
            
            // 인벤토리 사전에서 ItemInfo를 하나씩 꺼내어 가져옵니다.
            foreach( List<ItemInfo> itemInfoList in itemDic.Values )    
            {
                foreach( ItemInfo itemInfo in itemInfoList )
                {        
                    // 아이템의 STransform 정보를 불러와서 Transform을 반영하여 저장합니다.
                    itemInfo.SerializedTr.Serialize( itemInfo.Item3dTr );  
                }
            }
        }
    }







}
