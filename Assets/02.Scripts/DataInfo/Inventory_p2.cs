using ItemData;
using System.Collections.Generic;
using UnityEngine;

/*
 * [�۾� ����]  
 * <v1.0 - 2023_1121_�ֿ���>
 * 1- �ʱ� ���� Ŭ���� ����
 * AddItem�޼��� �߰� - CreateManager�� CreateItemToNearstSlot�޼��� ȣ�� ���
 * 
 * <v1.1 -2023_1121_�ֿ���>
 * 1- ����Ŭ������ ����鼭 MonoBehaviour�� ������ �ʾ� new Ű���� ��� �߻��Ͽ� ����.
 * 
 * <v1.2 - 2023_1122_�ֿ���>
 * 1- �κ��丮�� �ε�Ǿ��� �� ������ ������Ʈ�� ����ġ�� ��ų �� �ֵ��� UpdateAllItem�޼��� �߰�
 * 
 */

namespace CraftData
{    
    public partial class Inventory
    {
        public void AddItem(string itemName, int count=1)
        {
            CreateManager.instance.CreateItemToNearstSlot(this, itemName, count);
        }

        public void RemoveItem(string itemName)
        {

        }


        public void UpdateAllItem()
        {
            foreach( List<GameObject> objList in weapDic.Values )          // ����������� ���ӿ�����Ʈ ����Ʈ�� �ϳ��� ������
            {
                for(int i=0; i<objList.Count; i++)                         // ����Ʈ�� ���ӿ�����Ʈ�� ��� �����ɴϴ�.
                    objList[i].GetComponent<ItemInfo>().OnItemChanged();   // item ��ũ��Ʈ�� �ϳ��� ������ OnItemChnaged�޼��带 ȣ���մϴ�.
            }

            foreach( List<GameObject> objList in miscDic.Values ) // ��ȭ�������� ���ӿ�����Ʈ ����Ʈ�� �ϳ��� ������
            {
                for(int i=0; i<objList.Count; i++)                         // ����Ʈ�� ���ӿ�����Ʈ�� ��� �����ɴϴ�.
                    objList[i].GetComponent<ItemInfo>().OnItemChanged();   // item ��ũ��Ʈ�� �ϳ��� ������ OnItemChnaged�޼��带 ȣ���մϴ�.
            }      
        }


    }
}
