using DataManagement;
using Newtonsoft.Json;
using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.Rendering;
using UnityEngine;


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
 * <v2.0 - 2024_0124_�ֿ���>
 * 1- ItemBuilding�� STransform ������ WorldTr�� �߰�
 * ������ �������� ������¿��� �κ��丮�� ����Ǵ� �������� ��� ������ �� �ִ� Transform������ ����ä�� ����Ǿ������ϱ� ����
 * 
 * 2- isDecoration�� �����ϰ� enum������ BuildingType buldingType�� �߰��Ͽ���.
 * ������ ��Ŀ� �Ӽ� �Ӹ��ƴ϶� ��ġ�� �κ��丮���� ���ε� �����ؾ� �ϴµ� �̴� �Ӽ����ٴ� ����Ÿ���� �־� �����ϴ°��� ȿ�����̱� ����
 * 
 * 
 * 3- public ������ JsonPropert ��Ʈ����Ʈ ����
 * 
 * 
 * <v2.1 - 2024_0127_�ֿ���>
 * 1- ItemBuilding�� Clone�޼��带 �������̵��ؼ� �����Ͽ���.
 * ������ Item ���ο� Stransform�� �����ϰ� �ִ� ��� �������� �����ϱ� ������, �������� Clone�Ǿ �������� �����ϰ� �Ǿ�����.
 * ���� ���ο� STransform�� Clone�� ���� ��ü�ؼ� ��ȯ�������.
 *
 * <v2.2 - 2024_0128_�ֿ���>
 * 1 - ItemStatus ���ο� speed �Ӽ��� �߰�, ������ �������� �ɼ��� ����
 * 
 * <v2.3 - 2024_0130_�ֿ���>
 * 1- STransform ���� ������ ItemWeapon���� ItemEquipŬ������ ����
 * 
 * 2- ����Ʈ �����۵� ItemEquip�� ����ϵ��� ����
 * 
 */





namespace ItemData
{
    /// <summary>
    /// ���� Ÿ�� - ����, ���, ����, ����, �尩, ����
    /// </summary>
    public enum EquipType { Weapon, Helmet, Armor, Pants, Gloves, None }

    /// <summary>
    /// ��� ������ Ŭ���� - ���� ���� ���밡���� ��� Ŭ������ �߻��� �θ�μ� ��Ӹ� �����մϴ�
    /// </summary>
    public abstract class ItemEquip : Item
    {
        
        /// <summary>
        /// ���� ������ ���� ��ġ ����
        /// </summary>
        public STransform EquipTr;       
                
        /// <summary>
        /// ���� ���� ����
        /// </summary>
        public bool isEquip;
        
        /// <summary>
        /// �������� ���� ���� ������ġ
        /// </summary>
        public int EquipSlotIndex;

        /// <summary>
        /// �������� ���� Ÿ�� - ����, ���, ����, ����, �尩, ����
        /// </summary>
        public EquipType EquipType;


        public ItemEquip( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, string desc,
            EquipType equipType, STransform equipTr )
            : base( mainType, No, name, visualRefIndex, desc ) 
        { 
            this.EquipTr = equipTr;
            this.EquipType = equipType;
            isEquip = false;
            EquipSlotIndex = -1;
        }

    }


    
    /// <summary>
    /// ����Ʈ ������ Ŭ���� - ������ ���� ����ϱ� ������ ItemType�� ��з� ���ؿ� ���ϸ�, Item�� ����մϴ�.<br/>
    /// (Ư¡ - ���� ǥ������ ����, ������ ����Ʈ �� ����� �Ұ���, ��ü �ǿ� ǥ�õ��� ���� )
    /// </summary>
    [Serializable]
    public class ItemQuest : ItemEquip
    {
        public ItemQuest( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, string desc,
            EquipType equipType=EquipType.None, STransform equipTr=null )
            : base( mainType, No, name, visualRefIndex, desc, equipType, equipTr )
        { 

        }
    }




    /// <summary>
    /// ���� �������� ���������μ� ���� ��������, ��Ŀ������, �κ��丮, ���� ���� ������ �ֽ��ϴ�.
    /// </summary>
    public enum BuildingType { Basic, Inventory, None }

    /// <summary>
    /// �Ǽ������� - Item�� ����ϸ� ������Ʈ���� ������ �������� �ֽ��ϴ�.<br/>
    /// ��Ŀ�� �������� ���е˴ϴ�.<br/>
    /// ������ �Ϲ� ��ȭ�����۰� �����ϰ� �κ��丮�� �Űܴٴ� �� ������,<br/>
    /// ��Ŀ��� �ܺλ��·� �׻� �����ϸ� ����ǰ� �ҷ������� �մϴ�. (������ȭ ��,�κ��丮 ���η� 2D ���°� �� �� �����ϴ�.)
    /// </summary>
    [Serializable]
    public class ItemBuilding : Item
    {
        public int Durability;              // �Ǽ��������� ������
        public BuildingType buildingType;   // ���� ���� (���, ��Ŀ�, �κ��丮)
        public STransform WorldTr;          // �������� ���忡 �������� ��ȯ����


        public ItemBuilding( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex
            , BuildingType buildingType, int durability, string desc )
            : base(mainType, No, name, visualRefIndex, desc)
        {
            this.buildingType = buildingType;
            Durability = durability;
            WorldTr = new STransform();
        }


        /// <summary>
        /// �Ǽ� �������� Ŭ�� ���� �� ȣ��Ǵ� �޼����Դϴ�.<br/>
        /// ����� ���� �����ؼ� ��ȯ�ϸ�, ���� Ŭ���� ������ ���� �״�� ��ȯ���� �ʰ� ���ο� �ν��Ͻ��� ����� �������� ��ȯ�մϴ�.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            ItemBuilding itemBuilding = this.MemberwiseClone() as ItemBuilding;
            itemBuilding.WorldTr = (STransform) WorldTr.Clone();    // ���� �����ؼ� �ν��Ͻ��� ����� ���ο� �������� �����մϴ�.
            return itemBuilding;
        }

    }







    [Serializable]
    public class ItemFood : ItemMisc
    {
        public ItemStatus Status;

        public ItemFood( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex
            , MiscType subType, ItemStatus status, string desc )
            : base(mainType, No, name, visualRefIndex, subType, desc)
        {
            Status = status;
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
        /// �̵� �ӵ�
        /// </summary>
        public float speed;


        /// <summary>
        /// ü��, ���, ����, ü��, ���ǵ�
        /// </summary>
        public ItemStatus(float hp, float hunger, float thirsty, float temparature=0f, float speed=0f)
        {
            this.hp = hp;
            this.hunger = hunger;
            this.thirsty = thirsty;
            this.temparature = temparature;
            this.speed = speed;
        }
    }







}
