using InventoryManagement;
using ItemData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * [�۾�����]
 * 
 * <v1.0 - 2024_0130_�ֿ���>
 * 1- ��ũ��Ʈ �ۼ� ����
 * ������ ������ Item2dTr������ �ִ� ���� ������Ʈ�� �����и��ؼ� ��� �ڸ���� �־�� �ϸ�,
 * ���� �������� ������ ������ �־ �ܺο� �����Ǿ��ִ� ������Ʈ ��� ������ ������ �˷��ִ� ������ �ϰԵ�
 * 
 * 2- ������ ������ �Էµ� �� ���� �������� UpdateInfo�� ȣ�����ֵ��� ����
 * 
 * 3- ���̾������� UpdatePositionInfo�� ���� �������� UpdatePositionInfo�� ����� ������ �ϵ��� ����
 * 
 * 3- ���̾������� 2D����� �ߴ��ϴ� SwitchAppearAs2D�޼��� ����
 * 
 * 4- ���̾����۵� ������������ ���� ������ ������ �������� ����â�� ���� �������� �ݹ� �޼��� ����
 * 
 * <v1.1 - 2024_0131_�ֿ���>
 * (�̽�) ���� �̹����� ���� ������ �̹����� ��ø�Ǿ� Ŀ�������Ƿ� ���� �� �� ���·� �־�� �ϸ�,
 * ���� �������� ����� �Ѵ�.
 * 
 * <v1.2 - 2024_0214_�ֿ���>
 * 1- ���� ������Ʈ�� ��ư�� �߰��ϰ� �ʱ�ȭ�ϴ� ���� �߰�
 * ������ �������� �Ǵ��Ͽ� ������ �����ϰ� ������ �̹����� �������� �Ͼ���� �ϱ� ����
 * 
 * 2- �޼���� UpdateInfo�� InitItemInfo�� ����, �ּ� �Է�
 * 
 * 3- OnItemEquip �ű� �޼��带 �ۼ�
 * ������ ������ �Ͼ�� ���� ���� ������Ʈ Ȱ��ȭ ��Ȱ��ȭ �� ��ġ������ ������Ʈ ���ִ� ���� �޼���
 * 
 * <v1.3 - 2024_0222_�ֿ���>
 * 1- SwitchAppearAs2D�޼����� �Ű������� isWorldPositioned�� isOperateAs2d�� ���� �� ���� ���޵� �ݴ�� ����
 * (�������� 3D���� �Ӹ��ƴ϶� �κ��丮 �¿��������� �������� �����ϴ� ��찡 �����Ƿ�)
 * 
 * 2- �޼��� ȣ�� �� dummyBtn.interactable�Ӽ��� ���� �����ϵ��� ����
 *
 * 3- SwitchAppearAs2D�޼������ OperateSwitchAs2d�� ����
 * 
 * <v1.4 - 2024_0223_�ֿ���>
 * 1- DummyBtn �б����� ������Ƽ �߰�
 * ItemSelect��ũ��Ʈ���� �Ͻ������� �������� ������ ������ Interactable�Ӽ��� �����ϱ� ���� 
 * 
 */

public class DummyInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image dummyImg;                 // ���� �������� ���� ���� �� �������� ���� �̹����Դϴ�.
    RectTransform dummyRectTr;      // ���� �������� ���� ���� �� �������� ���� ������Ʈ�� RectTransform �������Դϴ�.   
    ItemInfo equipItemInfo;         // ���̰� ����� ���� ������ �����Դϴ�
    Button dummyBtn;                // ���� ������Ʈ�� �������� �� �ִ� ��ư



    /// <summary>
    /// ���� �̹����� RectTransform�� ��ȯ�մϴ�.
    /// </summary>
    public RectTransform DummyRectTr { get { return dummyRectTr; } }
     

    /// <summary>
    /// ���� ���� ���� ������ ������ ��ȯ
    /// </summary>
    public ItemInfo EquipItemInfo { get { return equipItemInfo; } }
    

    /// <summary>
    /// ���� �̹��� ������Ʈ ���� ���� ��ȯ�մϴ�.
    /// </summary>
    public Image DummyImg {  get { return dummyImg; } }

    /// <summary>
    /// ���� ��ư ������Ʈ ���� ���� ��ȯ�մϴ�.
    /// </summary>
    public Button DummyBtn { get { return dummyBtn; } }

    private void Awake()
    {
        dummyRectTr = GetComponent<RectTransform>();
        dummyImg = GetComponent<Image>();
        dummyBtn = GetComponent<Button>();

        // ���� �� ���� �̹����� ��ư�� ��Ȱ��ȭ�մϴ�.
        dummyImg.enabled = false;           
        dummyBtn.enabled = false;
    }


    /// <summary>
    /// �������� ������ �� ȣ������� �� �޼����Դϴ�.<br/>
    /// ���̿� ������ ������ �����Ͽ� �ʱ�ȭ�� �����մϴ�.
    /// </summary>
    public void InitItemInfo(ItemInfo itemInfo)
    {
        equipItemInfo = itemInfo;
        dummyImg.sprite = itemInfo.innerSprite;
    }


    /// <summary>
    /// ���� ������Ʈ�� 2D ����� �����ϰų� Ȱ��ȭ�մϴ�.<br/>
    /// ���ڷ� 2D ���� ���θ� ���޹޽��ϴ�.
    /// </summary>
    public void OperateSwitchAs2d(bool isOperateAs2d)
    {
        dummyImg.raycastTarget = isOperateAs2d;    // �̹����� ����ĳ���� ���θ� ��ȯ�մϴ�.
        dummyBtn.interactable = isOperateAs2d;     // ��ư�� ��ȣ�ۿ� ���θ� ��ȯ�մϴ�.
    }


    /// <summary>
    /// ������ ���� �� ���� �� ȣ���ؾ� �� �޼����Դϴ�.<br/>
    /// ���� ���� ��� ������ ������Ʈ �մϴ�.
    /// </summary>
    public void OnItemEquip(bool isEquip)
    {
        // �������� ������°� �Ǿ��ٸ�,
        if(isEquip)
        {                        
            DummyImg.enabled = true;    // ���� �̹����� Ȱ��ȭ�մϴ�.
            dummyBtn.enabled = true;    // ���� ��ư�� Ȱ��ȭ�մϴ�.
        }

        // �������� ���� ���� ���°� �Ǿ��ٸ�,
        else
        {
            DummyImg.enabled = false;   // ���� �̹����� ��Ȱ��ȭ�մϴ�.
            dummyBtn.enabled = false;   // ���� ��ư�� ��Ȱ��ȭ�մϴ�.    
        }

        // ���� ������Ʈ�� �������� ������Ʈ�մϴ�.
        UpdatePositionInfo();
    }




    /// <summary>
    /// ���� �������� ���� ����Ʈ�� �����ִٸ� ���� �������� ��ġ�� ���� �ε����� �°� �ֽ�ȭ�����ݴϴ�.<br/>
    /// ���� �ε����� �߸��Ǿ� �ִٸ� �ٸ� ��ġ�� �̵� �� �� �ֽ��ϴ�.<br/><br/>
    /// ** �������� ���� �� �ְų� ���� ������ �� ���� ��� ���ܸ� �����ϴ�. **<br/>
    /// </summary>
    public void UpdatePositionInfo()
    {
        // ���� ����Ʈ�� ������ �����Ǿ����� �ʴٸ� ���������� �������� �ʽ��ϴ�.
        if( equipItemInfo.SlotListTr.childCount==0 )
        {
            Debug.Log( "���� ������ �������� ���� �����Դϴ�." );
            return;
        }

        // �������� Ÿ���� �޾ƿɴϴ�.
        ItemType itemType = equipItemInfo.Item.Type;


        /**** �������� �ǿ� ǥ�� ���� ���� ���� ****/
        // 1. ��ü���� ��� ����Ʈ �������� �����ϰ� ǥ���մϴ�.
        // 2. ��ü���� �ƴ϶�� ����Ȱ��ȭ �ǰ� �������� ���� ���� ��ġ�ؾ� ǥ���մϴ�.

        // �������� �������°� �ƴ� ��� ����ġ�� ���ư��ϴ�.
        if( !equipItemInfo.IsEquip )
        {
            MoveToItem2dTr();
            return;
        }

        // ���� Ȱ��ȭ���� ��ü ���ΰ��
        if(equipItemInfo.CurActiveTab == TabType.All)
        {
            // ����Ʈ �������̶��, EmptyList�� �̵��մϴ�.
            if( itemType==ItemType.Quest )
            {
                MoveToItem2dTr(); // �� �������� �� ����Ʈ�� �̵���ŵ�ϴ�.
                return;
            }
        }
        // �������� ��ü���� �ƴ� ���, �������� ��Ÿ���� ����Ȱ��ȭ ���� �ƴ϶��
        else if( Inventory.ConvertItemTypeToTabType(equipItemInfo.Item.Type) != equipItemInfo.CurActiveTab )
        {
            MoveToItem2dTr();      // �� �������� �� ����Ʈ�� �̵���ŵ�ϴ�.
            return;
        }


        MoveToSlot();   // �������� �������� �̵���ŵ�ϴ�.



    }


    
    /// <summary>
    /// ���� �������� ������ ������ 2d ������Ʈ �������� �̵���Ű�� �޼����Դϴ�.
    /// </summary>
    /// <exception cref="Exception"></exception>
    private void MoveToItem2dTr()
    {
        if(equipItemInfo.IsWorldPositioned)
            throw new Exception("���� ������ ���� �������� �̵��� �� �����ϴ�.");

        dummyRectTr.SetParent(equipItemInfo.Item2dTr, false);
    }

    /// <summary>
    /// �������� �ε��� ������ �����Ͽ� �ش�Ǵ� �������� ���� �������� �̵������ִ� �޼����Դϴ�.<br/>
    /// ���� Ȱ��ȭ ���� �� ������ ���� �̵����Ѿ� �ϹǷ� �����ؼ� ����ؾ� �մϴ�.<br/>
    /// </summary>
    private void MoveToSlot()
    {
        if(equipItemInfo.IsWorldPositioned)
            throw new Exception("���� ������ ���� �������� �̵��� �� �����ϴ�.");
        if(equipItemInfo.SlotListTr==null)
            throw new Exception("���� ������ �������� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");
                        
        // ���� Ȱ��ȭ ���� ���� ������� � �ε����� �������� �����մϴ�.
        int activeTabSlotIndex = equipItemInfo.SlotIndexActiveTab;
        
        // �������� ũ�⸦ ���Ը���Ʈ�� cellũ��� �����ϰ� ����ϴ�.(������ ũ��� �����ϰ� ����ϴ�.)
        dummyRectTr.sizeDelta = equipItemInfo.InventoryInfo.CellSize;

        // �������� �θ� �ش� �������� �����մϴ�.
        dummyRectTr.SetParent( equipItemInfo.SlotListTr.GetChild(activeTabSlotIndex) );  

        // ��ġ�� ȸ������ �����մϴ�.
        dummyRectTr.localPosition = Vector3.zero;
        dummyRectTr.localRotation = Quaternion.identity;
    }



    /// <summary>
    /// ���� ������ ������ ���ٿ� ���� �ݹ� �޼��� ���� �������� ������ �������� ����â�� ���ų� �����մϴ�
    /// </summary>
    public void OnPointerEnter( PointerEventData eventData )
    {
        
        if(equipItemInfo.StatusWindowInteractive==null)
            return;

        equipItemInfo.StatusWindowInteractive.OnItemPointerEnter(equipItemInfo);
    }


    
    /// <summary>
    /// ���� ������ ������ ���ٿ� ���� �ݹ� �޼��� ���� �������� ������ �������� ����â�� ���ų� �����մϴ�
    /// </summary>
    public void OnPointerExit( PointerEventData eventData )
    {
        if(equipItemInfo.StatusWindowInteractive==null)
            return;

        equipItemInfo.StatusWindowInteractive.OnItemPointerExit();
    }
}
