using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInventoryInfo : InventoryInfo
{

    protected override void Start()
    {        
        /** ȣ�� ���� ����: �ε�->���ͷ�Ƽ�꽺ũ��Ʈ �ʱ�ȭ �� ���Ի�����û->������ǥ�� ***/
        this.LoadInventoryData();         // ����� �÷��̾� �����͸� �ҷ��ɴϴ�. 
        interactive.Initialize(this);     // ���ͷ�Ƽ�� ��ũ��Ʈ �ʱ�ȭ�� �����մϴ�. 
        LoadAllItemTransformInfo();       // �������� 3D ������ ������Ʈ �մϴ�.

        StartCoroutine( SaveAllInfoTimeCheck(10.0f) );
    }


    IEnumerator SaveAllInfoTimeCheck(float checkDuration)
    {
        Debug.Log("��������� ������ ���� ���Դϴ�.");
        SaveAllItemTransformInfo();
        yield return new WaitForSeconds(checkDuration);
    }




    /// <summary>
    /// �κ��丮�� �����ϰ� �ִ� ��� ������ ������ 3D ������Ʈ�� Transform ������ �ֽ�ȭ�մϴ�.<br/>
    /// 3D �������� �����ϴ� �κ��丮���� �ε� ���� ȣ��Ǿ����� �ϴ� �޼����Դϴ�.<br/>
    /// </summary>
    protected void LoadAllItemTransformInfo()
    {
        Dictionary<string, List<ItemInfo>> itemDic;                     // ������ ������ ������ �����մϴ�.
            
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
                    itemInfo.SerializedTr.Deserialize( itemInfo.ItemTr );
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
            itemDic =inventory.GetItemDic( inventory.dicType[i] );      // ������ ������ ���� �κ��丮�� ������ �Ҵ�޽��ϴ�.
                          
            // ������ ������ ���ų� ����Ʈ�� �������� �ʴ´ٸ� ���� ������ �����մϴ�.
            if(itemDic==null || itemDic.Count==0)   
                continue;

            // �κ��丮 �������� ItemInfo�� �ϳ��� ������ �����ɴϴ�.
            foreach( List<ItemInfo> itemInfoList in itemDic.Values )    
            {
                foreach( ItemInfo itemInfo in itemInfoList )
                {
                    // �������� STransform ������ �ҷ��ͼ� Transform�� �ݿ��Ͽ� �����մϴ�.
                    itemInfo.SerializedTr.Serialize( itemInfo.ItemTr );
                }
            }
        }
    }






}
