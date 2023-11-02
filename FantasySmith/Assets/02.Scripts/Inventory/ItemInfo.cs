using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using UnityEngine.UI;
using System;

/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1102_최원준>
 * 1 - 최초작성 및 주석처리
 * 
 */


/// <summary>
/// 게임 상의 아이템 오브젝트는 이 클래스를 컴포넌트로 가져야합니다.
/// </summary>
public class ItemInfo : MonoBehaviour
{
    public Item item;               // 모든 아이템 클래스를 관리 가능한 변수
    public Image innerImage;        // 아이템이 인벤토리에서 보여질 이미지 (오브젝트의 이미지 컴포넌트를 말한다.)

    private void Start()
    {
        innerImage = GetComponent<Image>();
    }

    /// <summary>
    /// 아이템 오브젝트가 새롭게 생성되었을 때 이미지를 반영해야 합니다.
    /// </summary>
    public void OnItemAdded()
    {
        if(item != null)
            innerImage.sprite = item.Image.innerSprite;
        else
            throw new Exception("이 오브젝트는 아이템이 초기화가 이루어지지 않았습니다. 확인하여 주세요.");
    }     

}