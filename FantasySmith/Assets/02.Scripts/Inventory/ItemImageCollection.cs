using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1101_최원준>
 * 1 - 최초작성 및 테스트 완료
 * 
 * <v1.1 - 2023_1102_최원준>
 * 1 - 인벤토리 외부에서 보여질 이미지인 outerSpriteImage와, outerMaterial을 삭제하였음.  
 * 그 이유는 한 오브젝트에 2d 렌더링 컴포넌트와 3d 렌더링 컴포넌트를 동시에 관리하기에는 
 * (transform의 조정 및 관리와 컴포넌트 on,off 및 이미지 스위칭 측면에서) 비효율적이라고 판단되어기에,
 * 
 * 인벤토리에서 외부로 아이템이 반출될 때는 새롭게 오브젝트를 생성하여 보여주도록 한다.
 * (2d, 3d오브젝트를 따로 관리)
 * 
 * 2 - 주석 처리
 * 
 */


/// <summary>
/// 인스펙터뷰 상으로 쉽게 등록하기 위하여 클래스 내부에 이미지 집합체를 포함합니다.
/// </summary>
public class ItemImageCollection : MonoBehaviour
{
    /// <summary>
    /// 인스펙터 뷰 상에서 등록한 이미지의 집합 구조체입니다.
    /// </summary>
    public ImageCollection[] icArrImg;
}

/// <summary>
/// 실제 이미지 집합입니다. 직렬화 되어 인스펙터 뷰 상에서 보여질 수 있습니다.
/// </summary>
[System.Serializable]
public struct ImageCollection
{
    public string itemDesc;         // 인스펙터뷰에서 보여질 아이템 이름 또는 설명
    public Sprite innerSprite;      // 인벤토리 내부에서 보여질 이미지
    public Sprite statusSprite;     // 인벤토리 상태창에서 커서를 포커싱했을 때 보여질 이미지
}
