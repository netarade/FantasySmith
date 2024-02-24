using ItemData;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/*
* [�۾� ����]
* 
* <v1.0 - 2023_1102_�ֿ���>
* 1- �������� SetOverlapCount, Remove, IsEnoughOverlapCount�޼��� ItemInfoŬ�������� �Űܿ�
* ������ ������ ������Ű�ų�, ������Ű�ų�, ������ ���ȯ�� Ȯ���ϴ� ���
* 
* 2- �������� SetOverlapCount, IsEnoughOverlapCount �޼��� ����
* ������ ������ ��ü���� ������, �����˻� ����� �ְԵǸ�, �κ��丮�� �ִ� ���¿� ���»��¸� �����ؼ� �ڵ带 �־�� �ϱ� �����̰�,
* �κ��丮�� ������ ������ ������ ����ȭ ������ �߻��� ���ɼ��� ũ�� ����
* 
* 3- Remove�޼��嵵 ����
* Inventory ���������� ������ ItemInfo�� ��ȯ�ϸ� �״��� �����ϸ� �����̸� �� �ʿ䰡 ���⶧�� 
* 
* 
* <v2.0 - 2024_0104_�ֿ���>
* 1- SlotDrop�� ���� ���� �޼������ ItemInfo.cs���� �Űܿ�
* 
* <v2.1 - 2024_0105_�ֿ���>
* 1- MoveSlotToAnotherListSlot�޼��� ���� IsSlotEnough�� ItemType��� ȣ�⿡�� ItemInfo��� ȣ��� ����
* => ��ȭ�������� ��� ������ �ʿ����� ���� ��찡 �ֱ� ����
* 
* 2- SlotDrop �̺�Ʈ �߻� �� MoveSlotInSameListSlot������ isActiveTabAll�� �� slotIndex�� ���� �ִ� ���� slotIndexAll�� ����
* 
* <2.2 - 2024_0106_�ֿ���
* 1- ItemPointerStatusWindow�� �����ϴ� �ڵ带 �Ϻ� �Űܿ�.
* �������� ����â �ڵ带 ��� �ִ����� ����â���� �ű�� ������ �̺�Ʈ �ÿ��� �ش� ����â�ڵ带 ȣ���ϴ� ������� ����
* 
* 2- Pointer Enter�� PointerExit�̺�Ʈ ��� �� �������� ������ ������ �Ͼ ������ ����â�� �޼��带 ȣ��
* 
* <v2.3 -2024_0112_�ֿ���>
* 1- �׽�Ʈ�� �޼��� PrintDebugInfo ����
* 
* <v2.4 - 2024_0116_�ֿ���>
* 1- MoveSlotToAnotherListSlot�޼��� ���ο�
* ���� ��� ���� �˻� �޼��带 IsSlotEnough�� IsSlotEnoughCertain���� ����
* 
* <v2.5 - 2024_0116_�ֿ���>
* 1- MoveSlotToAnotherListSlot�޼��忡�� 
* �������� �����κ��丮�� �������� �ű� �κ��丮�� �������� ��ġ���� ������ �����ϵ��� ó��
* 
* 2- MoveSlotToAnotherListSlot�޼��忡�� ���� ���Կ� ����ǵ��� AddItem�޼��带 AddItemToSlot���� ����
* 
* <v3.0 - 2024_0123_�ֿ���>
* 1- �޼���� ���� 
* MoveSlotInSameListSlot    -> MoveSlotToSlotSameInventory
* MoveSlotToAnotherListSlot ->  MoveSlotToSlotAnotherInventory
* 
* 2- MoveSlotToSlotAnotherInventory�޼��忡�� Ÿ �κ��丮 �� �̵� �����߰�
* ����ġ(�ű� �� �����ֱ� ����) + ������ġ(�ش������ �������� �ؾ��ϱ� ����)
* 
* 
* <v3.1 - 2024_0129_�ֿ���>
* 1- MoveSlotToSlotSameInventory �޼��� ���� �ε����� ������
* InventoryInfo�� IsShareAll(��ü ���� ����)�ɼǿ� ���� �޶������� ����
* 
* 
* <v3.3 - 2024_0223_�ֿ���>
* 1- MoveSlotToSlotAnotherInventory�޼��忡�� Ÿ �κ��丮 ��� �� 
* ���� �κ��丮���� RemoveItem�� �� �ι�° ���ڷ� isUnregister�ɼ��� Ȱ��ȭ���Ѽ� 2d���� �״�� ��Ͽ��� �������־���.
* => ������ ������·� �����ϰ� �Ǹ� DimensionShift�� �Ͼ�Եǰ� ������°� �� �� �ٽ� AddItem���� DimensionShift�� 2D���¸� ȸ���ϰ� �Ǵµ�
* �� �� �������� ĵ�����׷��� ItemSelect��ũ��Ʈ���� ������ �� ������ ���°� Add�����μ� �ٽ� ȸ���ǹ����� ������ ��� �� �� �������� �Ͼ�Ե�.
* 
* 2- MoveSlotToSlotAnotherInventory�޼��忡�� ���� �� ����â�� ǥ�õǰ� �����ִ� ������ �־ �����ϴ� �ڵ带 �߰�
* 
* <3.4 - 2024_0223_�ֿ���>
* 1- OnItemSlotDrop�޼��忡�� �̵��� �κ��丮�� �����Ե�� ���� �˻繮�� �߰�
* => ������ ItemSelect���� �ι� �˻縦 �����ϴ� ���·� �Ǿ��־�����, 
* ������ ���ҽÿ��� ���� �κ��丮�� ������ �������� �����Ե�ӿ�ǿ� �´��� �˻��ؾ� �ϱ� ����
* 
* 2- OnItemSlotDrop�޼����� �Ű����� callerSlotTr�� nextDropSlotTr�� ����
* 
* 3- ���� ����� � �����ۿ��� �Ͼ���� �˱� ���� ����׸޽����� itemRectTr�� �ν��Ͻ�ID�� �߰�
* 
* <v3.5 - 2024_0224_�ֿ���>
* 1- MoveSlotToSlotSameInventory �� MoveSlotToSlotAnotherInventory�޼����� ���� �ڵ带 ���� �����ϰ� �κ� �޼���ȭ
* ������ ������ ó���ϴ� �κ� �޼��� MoveThisItemInSameInventory, SwapNextItemInSameInventory�� ����������,
* �ݺ� ó�� �޼��� IsAbleToFill, FillSameItemOverlapCount �� �űԷ� �ۼ��Ͽ���.
* 
* 2- �����Ե�ӿ�ǰ� �⺻���Ե�ӿ���� ������ �޼���� ������ �˻��ϰ�,
* ���� �����۰� ������ ������ ����� �˻����·� ����
* 
* 3- ����ϱ����� ����� ������ �������� �����Ѵٸ�, ����â�� ���� ��� ������ �������� ���� �ڵ带 �߰�
* ����, �������� �������� ���̰� �˻��Ǳ� ������ ���� ������������ �����ü� �ִ� �޼��带 ȣ���ϴ� ������ ����
* 
* 4- ��� ���� �� ����ġ�� �����ֱ� ����, �������̾��� ��� �߰��� �����ڸ������� �ʱ�ȭ���ִ� �ڵ�ȣ��
* 
* 5- �������� �ʰ� ���� ��ȭ�������� ä��� ��쿡�� ����� ����ó��������, �������Ե���� �ֽ�ȭ���� ���ϵ��� ���¸� �ϳ� �� ����
* => �̴� prevDropSlotTr�� ���Ǻб�� �ؼ� ���� �κ��丮 ���Ե���̳� �ٸ� �κ��丮 ���Ե���̳İ� �������� ������
* �ٸ� �κ��丮���� ��ø��Ų ��쿡 �������Ե�� �ֽ�ȭ��Ű�� �Ǹ� �Ű��� ������ �ǴܵǾ�, ���� ���� ��� �� �ٲ��κ��丮�� �������� ���Ե���� �ǹ���. 
* 
* 
* 
*/


public partial class ItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string strItemDropSpace = "ItemDropSpace";  // ������ ������ �ִ� �±�    
        
    /// <summary>
    /// �����ۿ��� Ŀ���� ��� ���� �ڵ����� ������ �������ͽ� â�� ����ݴϴ�.
    /// </summary>
    public void OnPointerEnter( PointerEventData eventData )
    {
        if(statusInteractive==null)
            return;

        statusInteractive.OnItemPointerEnter(this);

        
        // +++ (�����ڵ�_������) ����â ���� ����  
        // SoundManager.instance.UISound(SoundManager.UI.MainTab_Click, ownerTr, 1.0f);
    }

    /// <summary>
    /// �����ۿ��� Ŀ���� ���� ���� �ڵ����� ������ �������ͽ� â�� ������ϴ�.
    /// </summary>
    public void OnPointerExit( PointerEventData eventData )
    {
        if(statusInteractive==null)
            return;

        statusInteractive.OnItemPointerExit();
    }



    /// <summary>
    /// �������� ���� ����� �߻��� �� �������� �̵���Ű�� ������ �����ϱ� ���Ͽ� ȣ������� �ϴ� �޼����Դϴ�.<br/>
    /// ���� Ȥ�� Ÿ �κ��丮�� ���� �� ��� �߻��� ����մϴ�.<br/><br/>
    /// ����->���� �κ��丮 ���� : �������� �ʽ��ϴ�. ���� �������� �ִٸ� ��ġ�� ��ȯ�մϴ�.<br/>
    /// ����->�ٸ� �κ��丮 ���� : ���ڸ��� ���ٸ� �����մϴ�. ���� �������� �ִٸ� �����մϴ�.<br/>
    /// </summary>
    /// <returns>���� ��ӿ� ���� �� true�� ���� �� false�� ��ȯ�մϴ�.</returns>
    public bool OnItemSlotDrop( Transform nextDropSlotTr )
    {        
        // ȣ�� ���ڰ� ���޵��� �ʾҴ��� �˻�
        if( nextDropSlotTr==null)
            throw new Exception("������ ������ ���޵��� �ʾҽ��ϴ�. �ùٸ� ���� ����̺�Ʈ ȣ������ Ȯ���Ͽ� �ּ���.");

        bool isNextSlot = (nextDropSlotTr.tag == strItemDropSpace);
        bool isPrevSlot = (prevDropSlotTr.tag == strItemDropSpace);
        
        // ȣ���ڰ� �������� �˻�
        if( !isNextSlot )
            throw new Exception("�������ڰ� ������ �ƴմϴ�. �ùٸ� ���� ����̺�Ʈ ȣ������ Ȯ���Ͽ� �ּ���.");
        // ���� ȣ���� ������ �������� �˻�
        else if( !isPrevSlot )
            throw new Exception("����->������ ����̺�Ʈ�� �߻��Ͽ����ϴ�. �ùٸ� ���� ����̺�Ʈ ȣ������ Ȯ���Ͽ� �ּ���.");
                

        // ���� ���� ȣ���ڿ� ���� ����̺�Ʈ ȣ���ڰ� ���ٸ�,
        if(nextDropSlotTr==prevDropSlotTr)        
        {   
            print( "transformID: " + itemRectTr.GetInstanceID() + "���� �κ��丮 ���� ���� �� ��� �߻�");
            return MoveSlotToSlotSameInventory(nextDropSlotTr);            // ������ �κ��丮�� ������ ����->���������� �̵�
        }
        else
        {
            // ���� ����̺�Ʈ ȣ���ڿ� �θ� ���ٸ�(������ ���� ����Ʈ������ �̵��̶��)
            if( nextDropSlotTr.parent == prevDropSlotTr.parent)
            {
                print( "transformID: " + itemRectTr.GetInstanceID() + "���� �κ��丮 Ÿ ���� �� ��� �߻�");
                return MoveSlotToSlotSameInventory(nextDropSlotTr);       // ���� �κ��丮 Ÿ ���� �� �̵�
            }
            else
            {
                print( "transformID: " + itemRectTr.GetInstanceID() + "Ÿ �κ��丮 Ÿ ���� �� ��� �߻�");
                return MoveSlotToSlotAnotherInventory(nextDropSlotTr);    // Ÿ �κ��丮 ���������� �̵�
            }
        }   
    }







    /// <summary>
    /// �������� ���� ���Կ��� �ٸ� �������� �̵����� �ִ� �޼����Դϴ�.<br/>
    /// ������ �ڽ��ε����� �о� �鿩 �����ۿ� ������� �ְ�, ������ ������Ʈ�� ��ġ�� ������Ʈ �մϴ�.<br/>
    /// ���� ���Կ� �̹� �ٸ� ������ ������Ʈ�� �ִٸ� ������ �ε��� ������ ��ġ�� ��ȯ�մϴ�.<br/>
    /// </summary>
    /// <returns>�ٸ� �κ��丮���� �̵� ���� �� true�� ��ȯ, ���� �� false�� ��ȯ</returns>
    private bool MoveSlotToSlotSameInventory(Transform nextSlotTr)
    {
        // �κ��丮 ���� �̵��� ���� �������θ� ��Ÿ���� ���� ������ ����ó�� �Ǿ����� �մϴ�. (�⺻��: ����)
        bool isSuccess = false;      
                
        // ��ȭ �������� ä��⸦ �����ߴ� �� ���θ� ����մϴ�. (�⺻��: ä���� ����)
        bool isFilled = false;
        
        // ���� �������� ������ ������ ��ġ�ߴ� ������ Transform �������� ���մϴ�.
        Transform thisItemSlotTr = inventoryInfo.SlotListTr.GetChild(this.SlotIndexActiveTab);

        // ����� ���Կ� ����ִ� �ٲ� �������� ������ ĳ���մϴ�.
        ItemInfo swapItemInfo = null;

        // �ű� ���Կ� �������� �̹� �����Ѵٸ�,
        if( nextSlotTr.childCount==1 )
        {
            // ���Կ� ��� ���� ������ ������ �����ɴϴ�. (�������� ��� ���̰� �ƴ� ���� ������ ������ ����) 
            swapItemInfo = inventoryInfo.GetSlotItemInfo( nextSlotTr );
                        
            // ��� �������� �������� �Ͻ������� �����ϴ�.
            swapItemInfo.ItemSelect.SelectPreventTemporary();
                        
            // ��� �������� ����â�� �����մϴ�. 
            swapItemInfo.statusInteractive.OnItemPointerExit();
        }



        // ���� �������� ���� �κ��丮�� ���������� ��� ���� ����� �˻��մϴ�.
        if( inventoryInfo.IsAbleToSlotDrop(this, nextSlotTr) )
        {
            // ������ ����ִٸ�,
            if( nextSlotTr.childCount==0 )
            {
                // ���� �κ��丮�� �������̶�� ���� �����ۿ� ���� �������� ������ �ֽ�ȭ �մϴ�.
                if(inventoryInfo.IsQuickSlot)
                    inventoryInfo.ThisQuickSlot.OnQuickSlotDrop(this, nextSlotTr);

                // ���� �������� �̵�ó�� �մϴ�.
                MoveThisItemInSameInventory( nextSlotTr );

                // ����ó�� �մϴ�.
                isSuccess=true;
            }


            // ������ ������� �ʴٸ�,
            else if( nextSlotTr.childCount==1 )
            {                     
                // ���� �� �������� ���� �������� �κ��丮�� ���� �ڸ��� ��� ���� ���θ� �Ǵ��մϴ�.
                if( inventoryInfo.IsAbleToSlotDrop( swapItemInfo, thisItemSlotTr ) )
                {
                    // ���� ��ø������ ��ȭ�������̶��,
                    if( this.IsAbleToFill(swapItemInfo) )
                    {
                        // ������ �������� ��ø��Ų �� ���� ������ �����ϸ�, �������� �ٽ� �����մϴ�. 
                        if( FillSameItemOverlapCount( swapItemInfo )!=0 )
                            this.itemSelect.ReselectUntilDeselect();

                        // ä����¸� Ȱ��ȭ �մϴ�.
                        isFilled = true;

                        // ����ó�� �մϴ�.
                        isSuccess=true;
                    }

                    // ���� ��ø������ ��ȭ�������� �ƴ϶��, 
                    else
                    {
                        // ���� �κ��丮�� �������̶�� ������ �����ۿ� ���� �������� ������ �ֽ�ȭ �մϴ�.
                        if( inventoryInfo.IsQuickSlot )
                        {
                            inventoryInfo.ThisQuickSlot.OnQuickSlotDrop( swapItemInfo, thisItemSlotTr );
                            inventoryInfo.ThisQuickSlot.OnQuickSlotDrop( this, nextSlotTr );
                        }

                        // ���� �����۰� ���� �������� ������ ó���մϴ�.
                        SwapNextItemInSameInventory( swapItemInfo, nextSlotTr );

                        //����ó�� �մϴ�.
                        isSuccess=true;
                    }
                }

            }

            // ���Կ� �ڽ��� 2�� �̻��� ��� - ���� ó��
            else  
                throw new Exception( "���Կ� �ڽ��� 2�� �̻� �����ֽ��ϴ�. Ȯ���Ͽ� �ּ���." );
        }


        // ������ �̵��� ������ ���
        if( isSuccess )
        {                        
            // ä���� ���� ��쿡��, ���� ������ �ֽ�ȭ�մϴ�.
            if( !isFilled )
                prevDropSlotTr=nextSlotTr;
        }
        // ������ �̵��� ������ ���
        else
        {   
            // ���� �������� ��� ���� �κ��丮�� �������� ��� ���� �����ۿ� ���� �ڸ� ������ �ٽ� �ʱ�ȭ �մϴ�. 
            if(inventoryInfo.IsQuickSlot)
                inventoryInfo.ThisQuickSlot.OnQuickSlotDrop(this, thisItemSlotTr);

            // �������� ����ġ�� �ǵ����ϴ�.
            UpdatePositionInfo();  
        }
        
        // �������θ� ��ȯ�մϴ�.
        return isSuccess;          
    }


    /// <summary>
    /// ���� �������� ���� �κ��丮 ���� �ٸ� �������� �̵�ó�����ִ� �޼����Դϴ�.<br/>
    /// �̵��� ������ Transform ���� ���� �����ؾ� �մϴ�.
    /// </summary>
    private void MoveThisItemInSameInventory( Transform nextSlotTr )
    {
        // �ش� ������ �ڽĹ�ȣ�� �����Ͽ� ���� ���� �ε����� ���մϴ�.
        int nextSlotIdx = nextSlotTr.GetSiblingIndex();

        // ��ü ���� ���� �ɼ��� Ȱ��ȭ �Ǿ��ִ� ����� �ε��� ����
        if( inventoryInfo.IsShareAll )
        {
            // (����Ʈ �������� �����ϰ�) ��ü �����ε����� ���� ���� �ε����� �����ϰ� �����ݴϴ�.
            if( item.Type!=ItemType.Quest )
                item.SlotIndexAll=nextSlotIdx;
            item.SlotIndexEach=nextSlotIdx;
        }
        // ��ü ���� ���� �ɼ��� ��Ȱ��ȭ �Ǿ��ִ� ����� �ε��� ����
        else
        {
            // Ȱ��ȭ ���� �ǿ� ���� �� �������� ���� �ε��� ������ �����մϴ�.
            if( isActiveTabAll )
                item.SlotIndexAll=nextSlotIdx;
            else
                item.SlotIndexEach=nextSlotIdx;
        }

        // �ش� ������ ��ġ������ ������Ʈ �մϴ�.
        UpdatePositionInfo();
    }


    /// <summary>
    /// ���� �κ��丮 ���ο� �ִ� �������� ���� ��ȯ ó���ϴ� ������ �޼����Դϴ�.<br/>
    /// ��ȯ�� ������ ������ ������ ��ȯ�� ���� ������ �����ؾ� �մϴ�.
    /// </summary>
    private void SwapNextItemInSameInventory( ItemInfo swapItemInfo, Transform nextSlotTr )
    {
        // �ش� ������ �ڽĹ�ȣ�� �����Ͽ� ���� ���� �ε����� ���մϴ�.
        int nextSlotIdx = nextSlotTr.GetSiblingIndex();

        // ��ü ���� ���� �ɼ��� Ȱ��ȭ �Ǿ��ִ� ����� �ε��� ���� 
        if( inventoryInfo.IsShareAll )
        {
            // (����Ʈ �������� �����ϰ�) ��ü �����ε����� ���� ���� �ε����� �����ϰ� �����ݴϴ�.
            swapItemInfo.SlotIndexEach=item.SlotIndexEach;
            item.SlotIndexEach=nextSlotIdx;

            if( swapItemInfo.Type!=ItemType.Quest )
            {
                swapItemInfo.SlotIndexAll=item.SlotIndexAll;
                item.SlotIndexAll=nextSlotIdx;
            }
        }
        // ��ü ���� ���� �ɼ��� ��Ȱ��ȭ �Ǿ��ִ� ����� �ε��� ����
        else
        {
            // Ȱ��ȭ ���� �ǿ� ���� �� �������� �ε��� ������ �����մϴ�.
            if( isActiveTabAll )
            {
                swapItemInfo.SlotIndexAll=item.SlotIndexAll;  // �ٲܾ������� �ε����� �� �������� ���� ������ �ε����� �־��ݴϴ�.      
                item.SlotIndexAll=nextSlotIdx;                // �� �������� �ε����� �ٲ� ������ ��ġ�� �����մϴ�.
            }
            else
            {
                swapItemInfo.SlotIndexEach=item.SlotIndexEach;
                item.SlotIndexEach=nextSlotIdx;
            }
        }

        swapItemInfo.UpdatePositionInfo();      // �ٲ� �������� ��ġ ������ ������Ʈ �մϴ�.
        UpdatePositionInfo();                   // �� �������� ��ġ ������ ������Ʈ �մϴ�.
    }






    /// <summary>
    /// �������� ���� �κ��丮�� ���Կ��� �ٸ� �κ��丮�� �������� �̵������ִ� �޼����Դϴ�.<br/>
    /// �ش� �κ��丮�� ���� �ڸ��� �ִ��� �ű�� ���� Ȯ���Ͽ� �ڸ��� ����ϴٸ�<br/>
    /// ������ �κ��丮 ��Ͽ��� �� �������� �����ϰ�, �ű� �κ��丮�� ��� ������ �ֽ�ȭ�Ͽ� �ݴϴ�.<br/>
    /// �̴� ��ư ������ �������� �Űܾ� �� ��찡 �ֱ� �����Դϴ�.<br/>
    /// </summary>
    /// <returns>���ο� �κ��丮 ���Կ� ���� �ڸ��� �ִ°�� true��, �����ڸ��� ���ų� �ش� ���Կ� ������ �������� �ִٸ� false�� ��ȯ</returns>
    private bool MoveSlotToSlotAnotherInventory(Transform nextSlotTr)
    {        
        // ��ȯ�� ��� ���� ���¸� �ʱ�ȭ�մϴ� (�⺻��: ����)
        bool isSucess = false;
                
        // ��ȭ �������� ä��⸦ �����ߴ� �� ���θ� ����մϴ�. (�⺻��: ä���� ����)
        bool isFilled = false;

        // ���� �������� ������ ������ ��ġ�ߴ� ������ Transform �������� ĳ���մϴ�.
        Transform thisItemSlotTr = inventoryInfo.SlotListTr.GetChild( this.SlotIndexActiveTab );

        // ���ڷ� ������ ������ ���������� ������� ���� �κ��丮 ���� ���� ĳ���մϴ�.
        Transform nextInventoryTr = nextSlotTr.parent.parent.parent.parent;
        InventoryInfo nextInventoryInfo = nextInventoryTr.GetComponent<InventoryInfo>();

        // ����� ���Կ� ����ִ� �ٲ� �������� ������ ĳ���մϴ�.
        ItemInfo swapItemInfo = null;
        

        // �ű� ���Կ� �������� �̹� �����Ѵٸ�,
        if( nextSlotTr.childCount==1 )
        {
            // ���Կ� ��� ���� ������ ������ �����ɴϴ�. (�������� ��� ���̰� �ƴ� ���� ������ ������ ����) 
            swapItemInfo = nextInventoryInfo.GetSlotItemInfo( nextSlotTr );
                                    
            // ��� �������� �������� �Ͻ������� �����ϴ�.
            swapItemInfo.ItemSelect.SelectPreventTemporary();
                        
            // ���� �� �������� ����â�� �����մϴ�. 
            swapItemInfo.statusInteractive.OnItemPointerExit();
        }



        // ���� �������� �ٸ� �κ��丮���� ��� ���� ���θ� �Ǵ��մϴ�.
        if( nextInventoryInfo.IsAbleToSlotDrop( this, nextSlotTr ) )
        {
            // ���� �κ��丮 ���Կ� �������� ������� �ʴٸ�,
            if( nextSlotTr.childCount==0 )
            {
                // ���� �κ��丮���� ���� �������� 2D���·� �����մϴ�.
                inventoryInfo.RemoveItem( this, true );
                                
                // �ű� �κ��丮�� �������̶�� ���� �����ۿ� ���� �������� ������ �ֽ�ȭ�մϴ�.
                if( nextInventoryInfo.IsQuickSlot )
                    nextInventoryInfo.ThisQuickSlot.OnQuickSlotDrop( this, nextSlotTr );

                // ���� �������� ���ο� �κ��丮�� �ش� ���Կ� �߰��մϴ�.
                nextInventoryInfo.AddItemToSlot( this, nextSlotTr, false );

                // ��� ���¸� �������� �����մϴ�.
                isSucess=true;
            }

            // ���� �κ��丮 ���Կ� �ٸ� �������� ����ִٸ�,
            else
            {
               
                // ���� �� �������� ���� �������� �κ��丮�� ���� �ڸ��� ��� ���� ���θ� �Ǵ��մϴ�.
                if( inventoryInfo.IsAbleToSlotDrop( swapItemInfo, thisItemSlotTr ) )
                {
                    // ���� ��ø������ �������̶��, ��ø�� �����մϴ�.
                    if( this.IsAbleToFill( swapItemInfo ) )
                    {
                        // ���� �̸��� �������� ä��� ���� ������ �����Ѵٸ�, �������� �ٽ� ������ �ݴϴ�.
                        if( FillSameItemOverlapCount( swapItemInfo )!=0 )
                            this.itemSelect.ReselectUntilDeselect();
                        
                        // ä����¸� Ȱ��ȭ�մϴ�.
                        isFilled = true;

                        // ��� ���¸� �������� �����մϴ�.
                        isSucess=true;

                    }
                    // ���� ��ø������ �������� �ƴ϶��, ������ �����մϴ�.
                    else
                    {
                        // ���� �κ��丮�� �������̶�� ������ �����ۿ� ���� �������� ������ �ֽ�ȭ �մϴ�.
                        if( inventoryInfo.IsQuickSlot )
                            inventoryInfo.ThisQuickSlot.OnQuickSlotDrop( swapItemInfo, thisItemSlotTr );
                        
                        // �ű� �κ��丮�� �������̶�� ���� �����ۿ� ���� �������� ������ �ֽ�ȭ�մϴ�.
                        if( nextInventoryInfo.IsQuickSlot )
                            nextInventoryInfo.ThisQuickSlot.OnQuickSlotDrop( this, nextSlotTr );


                        // (������ �߰� �� ���������� �������Ƿ�) ��ȯ�� �������� �ڸ��� ������ Ȱ���� �ε����� ��Ȱ���� �ε����� ����մϴ�. 
                        int nextSlotIndexActiveTab = swapItemInfo.SlotIndexActiveTab;
                        int nextSlotIndexInactiveTab = swapItemInfo.SlotIndexInactiveTab;

                        // (������ ���� �� �κ��丮 ������ �������Ƿ�) ���� �������� ��� �κ��丮�� ����մϴ�.
                        InventoryInfo rInventoryInfo = this.inventoryInfo;
                        
                        // ���� �κ��丮 ��Ͽ��� �� �������� 2D���� �״�� �����մϴ�.
                        rInventoryInfo.RemoveItem( this, true );
                        nextInventoryInfo.RemoveItem( swapItemInfo, true );

                        // ���� �κ��丮�� ���� �������� �ٲ㼭 ���� ������ �ڸ��� �״�� �߰��մϴ�.
                        rInventoryInfo.AddItemToSlot( swapItemInfo, this.SlotIndexActiveTab, this.SlotIndexInactiveTab, false );
                        nextInventoryInfo.AddItemToSlot( this, nextSlotIndexActiveTab, nextSlotIndexInactiveTab, false );

                        // ��� ���¸� �������� �����մϴ�.
                        isSucess=true;
                    }

                }
            }
        }




        // ������ �̵��� ������ ���
        if( isSucess )
        {
            // ä���� ���� ��쿡��, ���� ������ �ֽ�ȭ�մϴ�.
            if( !isFilled )
                prevDropSlotTr=nextSlotTr;  
        }
        // ������ �̵��� ������ ���
        else
        {            
            // ���� �������� ��� ���� �κ��丮�� �������� ��� ���� �����ۿ� ���� �ڸ� ������ �ٽ� �ʱ�ȭ �մϴ�. 
            if(inventoryInfo.IsQuickSlot)
                inventoryInfo.ThisQuickSlot.OnQuickSlotDrop(this, thisItemSlotTr);

            // ����ġ�� �ǵ����ϴ�.
            UpdatePositionInfo();   
        }

        // �������θ� ��ȯ�մϴ�.
        return isSucess;
    }


    /// <summary>
    /// �� �������� ��ø ������ �� ���θ� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>�� �������� ��� ��ȭ�������̰�, �̸��� �����ϴٸ� true�� ��ȯ, �ƴ϶�� false�� ��ȯ</returns>
    private bool IsAbleToFill(ItemInfo targetItemInfo)
    {        
        if( this.Type==ItemType.Misc && targetItemInfo.Type==ItemType.Misc
                        && this.Name==targetItemInfo.Name )
            return true;
        else
            return false;
    }



    /// <summary>
    /// �̸��� ������ ��ȭ�������� ��ø������ ä���ִ� �޼����Դϴ�.<br/>
    /// ���� �������� ��ø������ ���ҽ��� ���ڷ� ������ �������� ��ø������ ä���� ��,
    /// �ؽ�Ʈ ������ �ֽ�ȭ �ϰ�, �ı����θ� üũ�մϴ�.
    /// </summary>
    /// <returns>���� �������� ��ø������ ���ڷ� ������ ��ǥ �����ۿ� ä��� ���� ������ ��ȯ</returns>
    private int FillSameItemOverlapCount( ItemInfo destItemInfo )
    {
        if(destItemInfo==null)
            throw new Exception("���ڷ� null�������� ���޵Ǿ����ϴ�.");

        ItemMisc srcItemMisc = this.item as ItemMisc;
        ItemMisc destItemMisc = destItemInfo.item as ItemMisc;

        if (srcItemMisc == null || destItemMisc == null )
            throw new Exception("��ȭ �������� �ƴմϴ�.");

        if(srcItemMisc.Name != destItemMisc.Name)
            throw new Exception("�̸��� �������� ���� �������� ��ø�� �� �����ϴ�.");


        // ó�� ���� ������ ����մϴ�.
        int startCount = srcItemMisc.OverlapCount;

        // ��ø�� �������� ������ �ִ� ���ġ���� ä��� ���� ������ ��ȯ�޽��ϴ�.
        int lastCount = destItemMisc.AccumulateOverlapCount( startCount );

        // �پ�� ������ŭ ���� �������� ������ ���ҽ��� �ݴϴ�.
        srcItemMisc.AccumulateOverlapCount( lastCount-startCount );


        /*** ä��� �� ���� �������� ���� ������Ʈ ***/

        // ��ø�� �������� �ؽ�Ʈ ������ ������Ʈ �մϴ�. 
        destItemInfo.UpdateTextInfo();

        // ������ ���� �������� �ؽ�Ʈ ������ ������Ʈ�ϰ� �ı����θ� üũ�մϴ�.
        this.UpdateTextInfo(); 
        this.CheckDestroyInfo();

        return lastCount;
    }



}
