using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using CraftData;

/*
 * [���� ����]
 * ���� ���̺�� �ε忡 �ʿ��� ������ Ŭ���� ���� 
 * 
 * [�۾� ����]  
 * <v1.0 - 2023_1106_�ֿ���>
 * 1- �ʱ� GameData ����
 * 
 * <v1.1 - 2023_1106_�ֿ���
 * 1- Transform ���� ����, Ŭ���� CraftableWeaponList ���� �߰�
 * 2- �ּ� ����
 * 
 * <v1.2 - 2023_1108_�ֿ���>
 * 1- Inventory Ŭ������ ���ӿ�����Ʈ ����Ʈ�� �����ϰ� �ֱ� ������ ����ȵǴ� ������ ������ �˰�
 * ���������� ItemInfo ����Ʈ�� ����� ������ �õ��Ͽ���. 
 * �� �ٸ� �������� �ִµ� ItemInfo�� Image������Ʈ�� �����ϰ� �ֱ� ������ Ŭ���� ������ ����ȭ�ϱ� ����ٰ� ������.
 * 
 */

namespace DataManagement
{


    /// <summary>
    /// ���� ���� - ����Ƽ ���� Ŭ������ ���� �Ұ�. �⺻ �ڷ������� �����ϰų�, ����ü �Ǵ� Ŭ������ ����� �����Ѵ�. 
    /// </summary>
    [Serializable]           // �ν����Ϳ� �Ʒ��� Ŭ������ ��������.
    public class GameData
    {
        /// <summary>
        /// ���� �÷��� Ÿ��
        /// </summary>
        public float playTime;

        /// <summary>
        /// ��ȭ
        /// </summary>
        public float gold;

        /// <summary>
        /// ��ȭ
        /// </summary>
        public float silver;

        /// <summary>
        /// �÷��̾� ����
        /// </summary>
        public Transform playerTr;


        /// <summary>
        /// ���� �÷��̾ �����ϰ� �ִ� �κ��丮 �����Դϴ�. ���ӿ�����Ʈ�� �����ϴ� weapList�� playerMiscList �� InventoryMaxCount ���� �ֽ��ϴ�.
        /// </summary>
        public PlayerInventory inventory;


        /// <summary>
        /// ���� ���� ����� ���� �� string name���� �����ϴ� ����Ʈ���� ���� Ŭ���� �Դϴ�.
        /// </summary>
        public CraftableWeaponList craftableWeaponList;

        /// <summary>
        /// ��� ���õ� ����� name�� CraftProficincy����ü�� ���� �����Ͽ� ������ ������ �����ϰ� ���ݴϴ�.
        /// </summary>
        public Dictionary<string, CraftProficiency> proficiencyDic;
               










        
        // �̱��� Ŭ���� �� �޼���
        //public PlayerInventory saveInventory
        //{
        //    set { loadInventory=new SerializableInventory( value ); }
        //}

        //SerializableInventory loadInventory;

    }

    //[Serializable]
    //public class SerializableInventory
    //{
    //    public List<ItemInfo> weapList;
    //    public List<ItemInfo> miscList;
    //    public int InventoryMaxCount; 

    //    public SerializableInventory(PlayerInventory inventory)
    //    {
    //        foreach( GameObject item in inventory.weapList )
    //           this.weapList.Add( item.GetComponent<ItemInfo>() );

    //        foreach( GameObject item in inventory.miscList )
    //            this.miscList.Add( item.GetComponent<ItemInfo>() );

    //        this.InventoryMaxCount = inventory.InventoryMaxCount;
    //    }

    //    public PlayerInventory Load()
    //    {
    //        CreateManager.instance.CreateItemByItemInfo

    //        for(int i=0; i<weapList.Count; i++)
    //            weapList[i].item.Type == ItemType.Misc
            
    //        PlayerInventory loadInventory = new PlayerInventory();
    //    }

    //}


}