using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

/*
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1102_�ֿ���>
 * 1 - �����ۼ� �� �ּ�ó��
 * 
 * <v2.0 - 2023_1103_�ֿ���>
 * 1 - �ּ� ����
 * 2 - �̹��� ������Ʈ ��� ������ Start�޼��忡�� OnEnable�� ����
 * �ν��Ͻ��� �����Ǿ� �̹��� ������Ʈ�� ��� �����ϸ� OnItemAdded�� ȣ�� ������ ���ü��� ������ ��������Ʈ �̹����� ������� �ʴ´�.
 * 
 * 
 */


/// <summary>
/// ���� ���� ������ ������Ʈ�� �� Ŭ������ ������Ʈ�� �������մϴ�.
/// </summary>
public class ItemInfo : MonoBehaviour
{
    public Item item;               // ��� ������ Ŭ������ ���� ������ ����
    public Image innerImage;        // �������� �κ��丮���� ������ �̹��� (������Ʈ�� �̹��� ������Ʈ�� ���Ѵ�.)
    public Text countTxt;

    private void OnEnable()
    {
        innerImage = GetComponent<Image>();
        countTxt = GetComponentInChildren<Text>();
        countTxt.enabled = false;
    }

    /// <summary>
    /// ������Ʈ�� item�� ������ �̷�����ٸ� item�� ������ �ִ� �̹����� �ݿ��մϴ�.<br/>
    /// ** �ν��Ͻ��� �����ϰ� �ִ� ���� �������� ���ٸ� ���ܸ� �߻���ŵ�ϴ�. **
    /// </summary>
    public void OnItemAdded()
    {
        if( item==null )
            throw new Exception( "�� ������Ʈ�� �������� �����ϰ� ���� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���." );
        
        innerImage.sprite = item.Image.innerSprite;   // ���� �������� �̹��� ������ �����ɴϴ�.

        if( item.Type==ItemType.Misc )                // ��ȭ �������� ��ø ������ ǥ���մϴ�.
        {
            countTxt.enabled=true;
            countTxt.text = ((ItemMisc)item).InventoryCount.ToString();
        }
    }     

}