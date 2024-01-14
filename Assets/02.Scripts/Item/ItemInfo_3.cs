using ItemData;
using System;
using UnityEngine;

/*
* [작업 사항]
* <v1.0 - 2024_0112_최원준>
* 1- 아이템 사용시 처리를 위한 분할 클래스 작성 
* 
* <v1.1 - 2024_0114_최원준>
* 1- OnItemEquip 메서드 작성
* OnItemWorldDrop의 setParent형태로 이루어지게 되며, 
* 다시 자기참조값을 반환하여 메서드를 사용한 다음 한줄에 저장하기 쉽도록 구현
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
    /// 장비 아이템을 장착합니다.<br/>
    /// 인자로 전달된 Transform과 동일한 위치와 회전정보를 가지며, 계층하위 자식으로서 속하게 됩니다.<br/><br/>
    /// *** 장비 아이템이 아니라면 예외가 발생합니다. ***
    /// </summary>
    /// <returns>해당 아이템의 ItemInfo 값을 다시 반환</returns>
    public ItemInfo OnItemEquip(Transform equipTr)
    {
        if( !(item is ItemEquip) )
            throw new Exception("아이템이 장착가능한 타입이 아닙니다.");

        OnItemWorldDrop(equipTr, true);
        return this;
    }
}
