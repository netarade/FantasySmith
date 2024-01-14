using Newtonsoft.Json;
using System;


/*
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- �ű� Ŭ���� �ۼ� (ItemQuest, ItemBuilding �߰�)
 * 
 * <v1.1 - 2024_1112_�ֿ���>
 * 1- ItemEquip �ű� Ŭ���� �ۼ�
 * ������ �� �� ���밡���� ������ Ŭ������ ���븸 ���� �����ϱ� ����
 * 
 */





namespace ItemData
{
    /// <summary>
    /// ����Ʈ ������ Ŭ���� - ������ ���� ����ϱ� ������ Item�� ����մϴ�.<br/>
    /// (Ư¡ - ���� ǥ������ ����, ������ ����Ʈ �� ����� �Ұ���, ��ü �ǿ� ǥ�õ��� ���� )
    /// </summary>
    [Serializable]
    public class ItemQuest : Item
    {
        public ItemQuest( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, string desc )
            : base( mainType, No, name, visualRefIndex, desc ) 
        { 
            
        }
    }

    /// <summary>
    /// ��� ������ Ŭ���� - ���� ���� ���밡���� ��� Ŭ������ �߻��� �θ� (��Ӹ� �����մϴ�)
    /// </summary>
    public abstract class ItemEquip : Item
    {
        public ItemEquip( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, string desc )
            : base( mainType, No, name, visualRefIndex, desc ) 
        { 
            
        }

    }


    /// <summary>
    /// ���� ������ - ItemMisc�� ���<br/>
    /// ( Ư¡ - ������Ʈ���� ���� ü���� ���� )
    /// </summary>
    [Serializable]
    public class ItemBuilding : ItemMisc
    {
        [JsonProperty] int hp;


        public ItemBuilding( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, MiscType subType, int hp, string desc )
            : base(mainType, No, name, visualRefIndex, subType, desc)
        {
            this.hp = hp;
        }

    }

}
