using DataManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * [�۾�����]
 * <v1.0 - 2024_0125_�ֿ���>
 * 1- 3D ���·� �������� �����ϴ� �κ��丮 Ŭ������ �ۼ�
 * 
 * 2- LoadAllItemTransformInfo�� ���� InventoryInfo���� �Űܿ�����,
 * �߰��� SaveAllItemTransformInfo�޼��� �ۼ�
 * 
 * <v1.1 - 2024_0125_�ֿ���>
 * 1- SaveAllInfoTimeCheck�޼���� SaveAllItemTransformInfoInterval�� ����
 * �Ű��������� timeDuration���� interval�� ����
 * 
 * <v2.0 - 2024_0210_�ֿ���>
 * 1- SaveAllItemTransformInfoInterval �ֱ����� �޼��� �����ϰ� ���� �ڵ带 ���� 
 * (Start���� �ڷ�ƾ ȣ���ϴ� ���� ����, saveInterval ���� ����)
 * 
 * ������ ������ ��ġ ������ �ֱ� ������ ���� �ƴ϶� �κ��丮�� ����� �� ���� ����Ǵ� ���°� �Ǿ�� �ϱ� ����
 * (��, �κ��丮 ������ �ֱ����� Ȥ�� ���� ����Ǳ� ������ �� ���� ȣ��Ǹ� �Ǳ� ����)
 * 
 * 2- SaveInventoryData�� �������̵� �Ͽ� 
 * �κ��丮 ���� ���� ���� SaveAllItemTransformInfo�޼��带 ȣ���Ͽ� ������ STransform������ �����ϵ��� �Ͽ���
 * 
 * 3- InitOpenState�޼����� ȣ���� �κ��丮 �ε� ���ķ� ���� (���� �θ�ũ��Ʈ�� Awake���� Start�� ����)
 * ������ ���������� �Ҵ�� ������ ����Ͽ� null���۷����� ������ �����ε� �̴� �κ��丮 ���� ��� �������� 2D ���¸� ��Ȱ��ȭ�ϱ� ����
 * 
 * 4- loadSlotNo�� UIManager���� ������ PlayerPrefs�� SlotNo Ű���� �޾ƿͼ� �����ϵ��� ����
 * 
 */


public class WorldInventoryInfo : InventoryInfo
{
    protected override void Start()
    {        
        /** ȣ�� ���� ����: �ε�->������ǥ�� ***/

        // �ε��� ���Թ�ȣ - SlotNo Ű���� �������� �ʴ´ٸ�(�� �����̶��) 0��, �����Ѵٸ�(���� �����̶��) �ش� Ű���� �޾Ƽ� �����մϴ�.
        int loadSlotNo = 0;    
        if(PlayerPrefs.HasKey("SlotNo"))
            loadSlotNo = PlayerPrefs.GetInt("SlotNo");        
        
        LoadInventoryData(loadSlotNo);          // ����� �÷��̾� �����͸� �ҷ��ɴϴ�. 
        LoadAllItemTransformInfo();             // ����������� Transform������ ������Ʈ �մϴ�.
        InitOpenState(false);                   // �κ��丮�� ���»��¸� �������� ����ϴ�
    }

    
    /// <summary>
    /// �������� STransform ������ �����԰� ���ÿ� �κ��丮 ���� �����͸� �����մϴ�.
    /// </summary>
    protected override void SaveInventoryData( int slotNo )
    {
        SaveAllItemTransformInfo();         // �κ��丮 ���̺� �� ���� �������� STransform ������ ������Ʈ �մϴ�.
        base.SaveInventoryData(slotNo);     
    }












    /// <summary>
    /// �κ��丮�� �����ϰ� �ִ� ��� ������ ������ 3D ������Ʈ�� Transform ������ �ֽ�ȭ�մϴ�.<br/>
    /// 3D �������� �����ϴ� �κ��丮���� �ε� ���� ȣ��Ǿ����� �ϴ� �޼����Դϴ�.<br/>
    /// </summary>
    protected void LoadAllItemTransformInfo()
    {
        Dictionary<string, List<ItemInfo>> itemDic;                     // ������ ������ ������ �����մϴ�.
            
        if(inventory==null)
            Debug.Log("���� �κ��丮�� �������� �ʽ��ϴ�.");
        else if(inventory.dicLen == 0)
            Debug.Log("���� �κ��丮�� ������ �������� �ʽ��ϴ�.");

        for(int i=0; i<inventory.dicLen; i++)                           // �κ��丮 ������ ������ŭ �ݺ��մϴ�.
        {
            itemDic =inventory.GetItemDic( inventory.dicType[i] );      // ������ ������ ���� �κ��丮�� ������ �Ҵ�޽��ϴ�.
                          
            // ������ ������ ���ų� ����Ʈ�� �������� �ʴ´ٸ� ���� ������ �����մϴ�.
            if(itemDic==null || itemDic.Count==0)   
                continue;

            // �κ��丮 �������� ItemInfo�� �ϳ��� ������ �����ɴϴ�.
            foreach( List<ItemInfo> itemInfoList in itemDic.Values )    
            {
                foreach( ItemInfo itemInfo in itemInfoList )
                {
                    // �������� STransform ������ �ҷ��ͼ� Transform�� ����ȭ�����ݴϴ�.
                    itemInfo.SerializedTr.Deserialize( itemInfo.Item3dTr );
                }
            }
        }
    }


    /// <summary>
    /// �κ��丮�� �����ϰ� �ִ� ��� ������ ������ 3D ������Ʈ�� Transform ������ �ֽ�ȭ�մϴ�.<br/>
    /// 3D �������� �����ϴ� �κ��丮���� �ε� ���� ȣ��Ǿ����� �ϴ� �޼����Դϴ�.<br/>
    /// </summary>
    protected void SaveAllItemTransformInfo()
    {
        Dictionary<string, List<ItemInfo>> itemDic;                     // ������ ������ ������ �����մϴ�.
            
        for(int i=0; i<inventory.dicLen; i++)                           // �κ��丮 ������ ������ŭ �ݺ��մϴ�.
        {
            itemDic = inventory.GetItemDic( inventory.dicType[i] );      // ������ ������ ���� �κ��丮�� ������ �Ҵ�޽��ϴ�.
                          
            // ������ ������ ���ų� ����Ʈ�� �������� �ʴ´ٸ� ���� ������ �����մϴ�.
            if(itemDic==null || itemDic.Count==0)   
                continue;
            
            // �κ��丮 �������� ItemInfo�� �ϳ��� ������ �����ɴϴ�.
            foreach( List<ItemInfo> itemInfoList in itemDic.Values )    
            {
                foreach( ItemInfo itemInfo in itemInfoList )
                {        
                    // �������� STransform ������ �ҷ��ͼ� Transform�� �ݿ��Ͽ� �����մϴ�.
                    itemInfo.SerializedTr.Serialize( itemInfo.Item3dTr );  
                }
            }
        }
    }







}
