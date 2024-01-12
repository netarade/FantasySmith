using ItemData;
using System;
using UnityEngine;

/*
* [�۾� ����]
* <v1.0 - 2024_0112_�ֿ���>
* 1- ������ ���� ó���� ���� ���� Ŭ���� �ۼ� 
* 
* 
*/

public partial class ItemInfo : MonoBehaviour
{    
    public void OnItemUse()
    {
        if(item is ItemEquip)
        {
            OnItemEquip();
        }
    }

    public void OnItemEquip()
    {
        if( !(item is ItemEquip) )
            throw new Exception("�������� ���������� Ÿ���� �ƴմϴ�.");

        OnItemWorldDrop(null, true);

    }
}
