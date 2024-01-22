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
 * 
 * <v1.2 - 2024_0117_�ֿ���>
 * 1- ItemBuilding�� hp�� public������ ����(���������ؾ� �ϹǷ�), ������ Hp�� ���� 
 * 
 * <v1.3 -2024_0118_�ֿ���>
 * 1- ItemBuilding�� Hp�� Durability�� �����Ͽ���.
 * ������ ItemInfoŬ�������� ����� �����ϰ� Durability�� ������ �� �ְ� �ϱ� ����.
 * 
 * 2- ItemFood�� �����ϰ� �������ͽ� ��ġ�� ������Ű�� ItemStatus ����ü�� ������ �Ͽ���.
 * 
 */





namespace ItemData
{
    /// <summary>
    /// ����Ʈ ������ Ŭ���� - ������ ���� ����ϱ� ������ ItemType�� ��з� ���ؿ� ���ϸ�, Item�� ����մϴ�.<br/>
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
    /// ��� ������ Ŭ���� - ���� ���� ���밡���� ��� Ŭ������ �߻��� �θ�μ� ��Ӹ� �����մϴ�
    /// </summary>
    public abstract class ItemEquip : Item
    {
        public ItemEquip( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, string desc )
            : base( mainType, No, name, visualRefIndex, desc ) 
        { 
            
        }

    }


    /// <summary>
    /// �Ǽ������� - ItemMisc�� ����ϸ� ������Ʈ���� ������ �������� �ֽ��ϴ�.<br/>
    /// ��Ŀ�� �������� ���е˴ϴ�.<br/>
    /// ������ �Ϲ� ��ȭ�����۰� �����ϰ� �κ��丮�� �Űܴٴ� �� ������,<br/>
    /// ��Ŀ��� �ܺλ��·� �׻� �����ϸ� ����ǰ� �ҷ������� �մϴ�. (������ȭ ��,�κ��丮 ���η� 2D ���°� �� �� �����ϴ�.)
    /// </summary>
    [Serializable]
    public class ItemBuilding : ItemMisc
    {
        [JsonProperty] public bool isDecoration;    // ��Ŀ� �Ӽ� (�������, ��Ŀ����� ����)
        [JsonProperty] public int Durability;       // �Ǽ��������� ������

        public ItemBuilding( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex
            , MiscType subType, bool isDecoration, int Durability, string desc )
            : base(mainType, No, name, visualRefIndex, subType, desc)
        {
            this.isDecoration = isDecoration;
            this.Durability = Durability;
        }
    }





    [Serializable]
    public class ItemFood : ItemMisc
    {
        public ItemStatus status;

        public ItemFood( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex
            , MiscType subType, ItemStatus status, string desc )
            : base(mainType, No, name, visualRefIndex, subType, desc)
        {
            this.status = status;
        }
    }



    /// <summary>
    /// �������� �����ϰ� �ִ� �������ͽ� ��ġ�� ��Ÿ���� ����ü�Դϴ�.<br/>
    /// ü��, ���, ����, ü�� 4���� ������ �ֽ��ϴ�.<br/>
    /// ���� ���� ������ �ش� ��ġ��ŭ �÷��̾��� �������ͽ��� ���ҽ�Ű��, ���� ���� ������ ������ŵ�ϴ�.
    /// </summary>
    [Serializable]
    public struct ItemStatus
    {
        /// <summary>
        /// ü��
        /// </summary>
        public float hp;            
        
        /// <summary>
        /// ���
        /// </summary>
        public float hunger;        

        /// <summary>
        /// ����
        /// </summary>
        public float thirsty;       

        /// <summary>
        /// ü��
        /// </summary>
        public float temparature;   
        
        /// <summary>
        /// ü��, ���, ����, ü��
        /// </summary>
        public ItemStatus(float hp, float hunger, float thirsty, float temparature=0f)
        {
            this.hp = hp;
            this.hunger = hunger;
            this.thirsty = thirsty;
            this.temparature = temparature;
        }
    }







}
