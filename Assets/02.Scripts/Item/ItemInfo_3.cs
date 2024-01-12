using ItemData;
using System;
using UnityEngine;

/*
* [작업 사항]
* <v1.0 - 2024_0112_최원준>
* 1- 아이템 사용시 처리를 위한 분할 클래스 작성 
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
            throw new Exception("아이템이 장착가능한 타입이 아닙니다.");

        OnItemWorldDrop(null, true);

    }
}
