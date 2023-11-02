using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using UnityEngine.UI;
using System;

/*
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1102_�ֿ���>
 * 1 - �����ۼ� �� �ּ�ó��
 * 
 */


/// <summary>
/// ���� ���� ������ ������Ʈ�� �� Ŭ������ ������Ʈ�� �������մϴ�.
/// </summary>
public class ItemInfo : MonoBehaviour
{
    public Item item;               // ��� ������ Ŭ������ ���� ������ ����
    public Image innerImage;        // �������� �κ��丮���� ������ �̹��� (������Ʈ�� �̹��� ������Ʈ�� ���Ѵ�.)

    private void Start()
    {
        innerImage = GetComponent<Image>();
    }

    /// <summary>
    /// ������ ������Ʈ�� ���Ӱ� �����Ǿ��� �� �̹����� �ݿ��ؾ� �մϴ�.
    /// </summary>
    public void OnItemAdded()
    {
        if(item != null)
            innerImage.sprite = item.Image.innerSprite;
        else
            throw new Exception("�� ������Ʈ�� �������� �ʱ�ȭ�� �̷������ �ʾҽ��ϴ�. Ȯ���Ͽ� �ּ���.");
    }     

}