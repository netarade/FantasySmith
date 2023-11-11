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
 * 1- �����ۼ� �� �ּ�ó��
 * 
 * <v2.0 - 2023_1103_�ֿ���>
 * 1- �ּ� ����
 * 2- �̹��� ������Ʈ ��� ������ Start�޼��忡�� OnEnable�� ����
 * �ν��Ͻ��� �����Ǿ� �̹��� ������Ʈ�� ��� �����ϸ� OnItemAdded�� ȣ�� ������ ���ü��� ������ ��������Ʈ �̹����� ������� �ʴ´�.
 * 
 * <v3.0 - 2023-1105_�ֿ���>
 * 1- ��������� ������ item�� ������Ƽȭ ���Ѽ� set�� ȣ��Ǿ��� �� OnItemAdded()�� ȣ��ǵ��� ���� 
 * OnItemAdded�� privateó�� �� ���� ����ó�� ���� ����
 *
 *<v4.0 - 2023_1108_�ֿ���>
 *1- �������� �ı��� �� ������ �����ϵ��� �����Ͽ�����, ���������� ����� ������� �ʴ� �����߻�
 *=> �������ʿ��� �ı��ɋ����� ��ųʸ��� �����ؼ� CraftManager�ʿ��� �������� �ѹ� �̸� �������ֵ��� ����
 *
 *2- OnItemAdded �޼��� �ּ��߰�
 *
 *3- UpdateCountTxt �޼��� �߰�
 * ������ ������ ���� �ɶ� �������� �ؽ�Ʈ�� �������ֵ��� �Ͽ���.
 * item�ʿ��� �޼��带 ������ �������� �ؼ� ���� ���ټ� Ȯ��.
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

    public Item Item                // �ܺο��� ����������� ������ų �� ȣ���ؾ��� ������Ƽ
    {
        set {
                item =  value;
                OnItemAdded();      // ��������� �������� ������ ���ο��� �ڵ� ȣ�����ش�. 
            }
        get {return item;}
    }


    /// <summary>
    /// �̹��� ������Ʈ�� ��� �켱������ ���̱� ���� OnEnable ���
    /// </summary>
    private void OnEnable()
    {
        innerImage = GetComponent<Image>();
        countTxt = GetComponentInChildren<Text>();
        countTxt.enabled = false;
    }

    /// <summary>
    /// ������Ʈ�� item�� ������ �̷�����ٸ� item�� ������ �ִ� �̹����� �ݿ��ϰ� ��ȭ�������� ��� ��ø Ƚ������ �ֽ�ȭ �մϴ�.<br/>
    /// private �� ���� �ڵ� ȣ���� �̷������ �޼����̱⿡ ����ڴ� �Ű澵 �ʿ䰡 �����ϴ�.
    /// </summary>
    private void OnItemAdded()
    {
        innerImage.sprite = item.Image.innerSprite;   // ���� �������� �̹��� ������ �����ɴϴ�.

        if( item.Type==ItemType.Misc )                // ��ȭ �������� ��ø ������ ǥ���մϴ�.
        {
            countTxt.enabled=true;
            countTxt.text = ((ItemMisc)item).InventoryCount.ToString();
        }
    }

    /// <summary>
    /// ��ȭ �������� ��øȽ���� �������� �����մϴ�. ��ȭ �������� ������ ����� �� ���� ȣ���� �ֽʽÿ�.
    /// </summary>
    public void UpdateCountTxt()
    {
        if( item.Type == ItemType.Misc )
            countTxt.text = ((ItemMisc)item).InventoryCount.ToString();
    }




    /// <summary>
    /// �������� �ı� �Ǳ��� ������ �Ѱ��ֱ� ���� �޼��� (���� �ӽ÷� �̸��� ������ �Ѱ��ְ� ���ο� �������� �����ϴ� �������� �Ǿ� �ִ�.)
    /// </summary>
    private void OnDestroy()
    {
        if( item.Type==ItemType.Misc )
        {
            CraftManager.instance.miscSaveDic.Add( item.Name, ( (ItemMisc)item ).InventoryCount );    //�̸��� ������ ����
            Debug.Log( item.Name );
        }
        else if( item.Type==ItemType.Weapon )
        {
            CraftManager.instance.weapSaveDic.Add( item.Name, 0 );    //�̸��� ����
            Debug.Log( item.Name );
        }
    }

}