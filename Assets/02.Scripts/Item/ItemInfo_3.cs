using ItemData;
using System;
using UnityEngine;

/*
* [�۾� ����]
* <v1.0 - 2024_0112_�ֿ���>
* 1- ������ ���� ó���� ���� ���� Ŭ���� �ۼ� 
* 
* <v1.1 - 2024_0114_�ֿ���>
* 1- OnItemEquip �޼��� �ۼ�
* OnItemWorldDrop�� setParent���·� �̷������ �Ǹ�, 
* �ٽ� �ڱ��������� ��ȯ�Ͽ� �޼��带 ����� ���� ���ٿ� �����ϱ� ������ ����
* 
*/

public partial class ItemInfo : MonoBehaviour
{    
    public void OnItemUse()
    {
        
    }

    public void OnItemDrink()
    {

    }


    /// <summary>
    /// ��� �������� �����մϴ�.<br/>
    /// ���ڷ� ���޵� Transform�� ������ ��ġ�� ȸ�������� ������, �������� �ڽ����μ� ���ϰ� �˴ϴ�.<br/><br/>
    /// *** ��� �������� �ƴ϶�� ���ܰ� �߻��մϴ�. ***
    /// </summary>
    /// <returns>�ش� �������� ItemInfo ���� �ٽ� ��ȯ</returns>
    public ItemInfo OnItemEquip(Transform equipTr)
    {
        if( !(item is ItemEquip) )
            throw new Exception("�������� ���������� Ÿ���� �ƴմϴ�.");

        OnItemWorldDrop(equipTr, true);
        return this;
    }
}
