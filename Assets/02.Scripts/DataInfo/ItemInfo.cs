using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1102_최원준>
 * 1- 최초작성 및 주석처리
 * 
 * <v2.0 - 2023_1103_최원준>
 * 1- 주석 수정
 * 2- 이미지 컴포넌트 잡는 구문을 Start메서드에서 OnEnable로 변경
 * 인스턴스가 생성되어 이미지 컴포넌트를 잡기 시작하면 OnItemAdded와 호출 시점이 동시성을 가져서 스프라이트 이미지가 변경되지 않는다.
 * 
 * <v3.0 - 2023-1105_최원준>
 * 1- 개념아이템 변수인 item을 프로퍼티화 시켜서 set이 호출되었을 때 OnItemAdded()가 호출되도록 변경 
 * OnItemAdded는 private처리 및 내부 예외처리 구문 삭제
 *
 *<v4.0 - 2023_1108_최원준>
 *1- 아이템이 파괴될 때 정보를 저장하도록 구현하였으나, 모든아이템의 기록이 저장되지 않는 문제발생
 *=> 아이템쪽에서 파괴될떄마다 딕셔너리를 생성해서 CraftManager쪽에서 마지막에 한번 미리 생성해주도록 변경
 *
 *2- OnItemAdded 메서드 주석추가
 *
 *3- UpdateCountTxt 메서드 추가
 * 아이템 수량이 변경 될때 동적으로 텍스트를 수정해주도록 하였음.
 * item쪽에서 메서드를 가지고 있음으로 해서 편리한 접근성 확보.
 *
 */


/// <summary>
/// 게임 상의 아이템 오브젝트는 이 클래스를 컴포넌트로 가져야합니다.
/// </summary>
public class ItemInfo : MonoBehaviour
{
    public Item item;               // 모든 아이템 클래스를 관리 가능한 변수
    public Image innerImage;        // 아이템이 인벤토리에서 보여질 이미지 (오브젝트의 이미지 컴포넌트를 말한다.)
    public Text countTxt;

    public Item Item                // 외부에서 개념아이템을 참조시킬 때 호출해야할 프로퍼티
    {
        set {
                item =  value;
                OnItemAdded();      // 개념아이템 참조값이 들어오면 내부에서 자동 호출해준다. 
            }
        get {return item;}
    }


    /// <summary>
    /// 이미지 컴포넌트를 잡는 우선순위를 높이기 위해 OnEnable 사용
    /// </summary>
    private void OnEnable()
    {
        innerImage = GetComponent<Image>();
        countTxt = GetComponentInChildren<Text>();
        countTxt.enabled = false;
    }

    /// <summary>
    /// 오브젝트에 item의 참조가 이루어졌다면 item이 가지고 있는 이미지를 반영하고 잡화아이템의 경우 중첩 횟수까지 최신화 합니다.<br/>
    /// private 로 내부 자동 호출이 이루어지는 메서드이기에 사용자는 신경쓸 필요가 없습니다.
    /// </summary>
    private void OnItemAdded()
    {
        innerImage.sprite = item.Image.innerSprite;   // 현재 아이템의 이미지 정보를 가져옵니다.

        if( item.Type==ItemType.Misc )                // 잡화 아이템의 중첩 갯수를 표시합니다.
        {
            countTxt.enabled=true;
            countTxt.text = ((ItemMisc)item).InventoryCount.ToString();
        }
    }

    /// <summary>
    /// 잡화 아이템의 중첩횟수를 동적으로 수정합니다. 잡화 아이템의 수량이 변경될 때 마다 호출해 주십시오.
    /// </summary>
    public void UpdateCountTxt()
    {
        if( item.Type == ItemType.Misc )
            countTxt.text = ((ItemMisc)item).InventoryCount.ToString();
    }




    /// <summary>
    /// 아이템이 파괴 되기전 정보를 넘겨주기 위한 메서드 (현재 임시로 이름과 수량만 넘겨주고 새로운 아이템을 생성하는 형식으로 되어 있다.)
    /// </summary>
    private void OnDestroy()
    {
        if( item.Type==ItemType.Misc )
        {
            CraftManager.instance.miscSaveDic.Add( item.Name, ( (ItemMisc)item ).InventoryCount );    //이름과 수량을 저장
            Debug.Log( item.Name );
        }
        else if( item.Type==ItemType.Weapon )
        {
            CraftManager.instance.weapSaveDic.Add( item.Name, 0 );    //이름을 저장
            Debug.Log( item.Name );
        }
    }

}