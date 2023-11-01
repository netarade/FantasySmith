using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
/*
 * [작업 사항]
 * 
 * v1.0 - 2023_1101_최원준
 * 최초작성 및 테스트 완료
 */
public class ItemImageCollection : MonoBehaviour
{
    public ImageCollection[] imgArr;
}

[System.Serializable]
public struct ImageCollection
{
    public string itemDesc;         // 간단한 설명
    public Sprite innerSprite;      // 인벤토리 내부에서 보여질 이미지
    public Sprite statusSprite;     // 인벤토리 상태창에서 커서를 포커싱했을 때 보여질 이미지
    public Sprite outerSprite;      // 인벤토리 외부에서 보여질 2d 이미지
    public Material outerMaterial;  // 인벤토리 외부에서 보여질 3d 머터리얼
}
