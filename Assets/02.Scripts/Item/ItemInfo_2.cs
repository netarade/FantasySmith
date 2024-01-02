using ItemData;
using System;
using UnityEngine;


public partial class ItemInfo : MonoBehaviour
{
    /*
     * [�۾� ����]
     * 
     * <v1.0 - 2023_1102_�ֿ���>
     * 1- �������� SetOverlapCount, Remove, IsEnoughOverlapCount�޼��� ����
     * ������ ������ ������Ű�ų�, ������Ű�ų�, ������ ���ȯ�� Ȯ���ϴ� ���
     * 
     * 
     */

    
    /// <summary>
    /// �ش� ��ȭ �������� ��ø������ �����Ͽ� �ݴϴ�.<br/><br/>
    /// ���� ���ڸ� ���� ���� ��, �ش� �������� �ִ� ��ø������ �ʰ��ϴ� ��� ������ �ʰ������� ��ȯ�մϴ�.<br/><br/>
    /// ���� ���ڸ� ���� ���� ��, �ش� �������� �ּ� ��ø����(0)�� �ʰ��ϴ� ��� 
    /// �������� �ı� �� �κ��丮 ��Ͽ��� �����ϰ�, ������ ������ ��ȯ�մϴ�.<br/><br/>
    /// ***** �ش� �������� ��ȭ�������� �ƴ϶�� ���ܸ� �����ϴ�. *****<br/>
    /// ** �ܼ��� �������� ������ Ȯ���ϰ� ���� ��� IsEnough�޼��带 ����ϱ⸦ �ٶ��ϴ�. **<br/>
    /// </summary>
    /// <param name="inCount"></param>
    /// <returns>��ȯ�Ǵ� ������ ���� �Ǵ� ���ҽ�Ű�� �ִ�, �ּ� �Ѱ迡 �����Ͽ� ���� �����̸�, 0�̰ų�, ���� �����̰ų�, ���� �����Դϴ�.</returns>
    public int SetOverlapCount(int inCount)
    {
        if( item.Type!=ItemType.Misc )
            throw new Exception( "��ȭ�������� �ƴմϴ�. Ȯ���Ͽ��ּ���." );

        int remainCount=0;
        ItemMisc itemMisc = (ItemMisc)item;

        remainCount = itemMisc.SetOverlapCount( inCount );
                
        if(itemMisc.OverlapCount==0)    // �������� ������ 0�� �Ǿ��ٸ�,
            Remove();                   // �� �������� ���� �޼��带 ȣ���մϴ�.
        
        return remainCount;             // ���� ������ ��ȯ�մϴ�.
    }


    /// <summary>
    /// �������� �κ��丮 ��Ͽ��� �����ϰ�, �������ִ� �޼����Դϴ�.<br/>
    /// �ı���Ű�� ������ ������ ������ ������ ó���ϱ� ���� ������ �ð��� ���ڷ� �� �� �ֽ��ϴ�. (�⺻ ���������� �ð��� 0.001���Դϴ�.)<br/>
    /// </summary>
    public void Remove(float timeToRemove=0.001f)
    {
        inventoryInfo.RemoveItem(this);             // �������� ���� �κ��丮 ��Ͽ��� �����մϴ�.   
        itemRectTr.SetParent(emptyListTr, false);   // �����ϱ� �� ��������� �ٷ� �ű�ϴ�.         
        Destroy( this.gameObject, timeToRemove );   // �ٸ� ���� ȣ���� ���Ͽ� ���ڷ� ���� �ð� ��ŭ �ణ �ʰ� �ı��մϴ�.
    }





    /// <summary>
    /// �� �������� ��ø������ ����� �� Ȯ���ϴ� �޼����Դϴ�.<br/><br/>
    /// ** ȣ�� �� �������� ��ȭ ������ �ƴ϶�� ���ܸ� �߻���ŵ�ϴ�. **
    /// </summary>
    /// <returns>���ڷ� ���� �������� ũ�ų� ���ٸ� true�� �۴ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool IsEnoughOverlapCount(int overlapCount)
    {
        if(Item.Type != ItemType.Misc ) 
            throw new Exception("�������� ������ ��ȭ�������� �ƴմϴ�. Ȯ���Ͽ� �ּ���.");
        
        ItemMisc itemMisc = (ItemMisc)item;

        // �������� ��ø������ ���ڷ� ���� �������� ���ų� ũ�ٸ� ����Ѱ����� �Ǵ�
        if(itemMisc.OverlapCount >= overlapCount)   
            return true;
        else
            return false;
    }



    

}
