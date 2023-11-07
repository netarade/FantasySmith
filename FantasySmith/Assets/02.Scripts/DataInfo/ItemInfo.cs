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
 *
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
    /// 오브젝트에 item의 참조가 이루어졌다면 item이 가지고 있는 이미지를 반영합니다.<br/>
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

}