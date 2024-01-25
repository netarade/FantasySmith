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
 * 2- 
 * 
 * 
 */


public class WorldInventoryInfo : InventoryInfo
{
    public float saveInterval = 10f;

    protected override void Start()
    {        
        /** ȣ�� ���� ����: �ε�->���ͷ�Ƽ�꽺ũ��Ʈ �ʱ�ȭ �� ���Ի�����û->������ǥ�� ***/
        this.LoadInventoryData();         // ����� �÷��̾� �����͸� �ҷ��ɴϴ�. 
        interactive.Initialize(this);     // ���ͷ�Ƽ�� ��ũ��Ʈ �ʱ�ȭ�� �����մϴ�. 
        LoadAllItemTransformInfo();       // �������� 3D ������ ������Ʈ �մϴ�.

        // �ν����ͺ信�� ������ ����ŭ �� �κ��丮�� ��ϵǾ��ִ� ���� ������ ������ �����մϴ�.
        StartCoroutine( SaveAllItemTransformInfoInterval(saveInterval) );
    }



    /// <summary>
    /// SaveAllItemTransformInfo�޼��带 interval �������� �������ִ� �ڷ�ƾ�Դϴ�.
    /// </summary>
    IEnumerator SaveAllItemTransformInfoInterval(float interval)
    {
        while( true )
        {
            Debug.Log( "��������� ������ ���� ���Դϴ�." );
            SaveAllItemTransformInfo();
            yield return new WaitForSeconds( interval );
        }
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
