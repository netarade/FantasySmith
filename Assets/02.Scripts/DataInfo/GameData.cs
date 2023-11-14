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
        /// ���� ���� ����� ���� �� string name���� �����ϴ� ����Ʈ���� ���� Ŭ���� �Դϴ�.
        /// </summary>
        public CraftableWeaponList craftableWeaponList;

        /// <summary>
        /// ��� ���õ� ����� name�� CraftProficincy����ü�� ���� �����Ͽ� ������ ������ �����ϰ� ���ݴϴ�.
        /// </summary>
        public Dictionary<string, CraftProficiency> proficiencyDic;

        

        /// <summary>
        /// ���� �÷��̾ �����ϰ� �ִ� �κ��丮 �����Դϴ�. ���ӿ�����Ʈ�� �����ϴ� weapList�� playerMiscList �� TotalMaxCount ���� �ֽ��ϴ�.
        /// </summary>
        public Inventory inventory
        {
            set{ 
                //saveInventory = new SerializableInventory( value );                
            }
            get{ 
                //Inventory loadInventory = new Inventory(saveInventory.weapList,saveInventory.miscList, saveInventory.TotalMaxCount);
                return new Inventory(); }
        }
        // �̱��� Ŭ���� �� �޼���
        
        private SerializableInventory saveInventory;
        private List<GameObject> ConvertListTypeItemToGameObject(List<Item> itemList)
        {
            List<GameObject> gameObjectList = new List<GameObject>();
            return gameObjectList;
        }





    }

    [Serializable]
    public class SerializableInventory
    {
        public List<Item> weapList;
        public List<Item> miscList;
        public int InventoryMaxCount;

        //public SerializableInventory( Inventory inventory )
        //{
        //    foreach( GameObject item in inventory.weapList )
        //        this.weapList.Add( item.GetComponent<ItemInfo>() );

        //    foreach( GameObject item in inventory.miscList )
        //        this.miscList.Add( item.GetComponent<ItemInfo>() );

        //    this.TotalMaxCount=inventory.TotalMaxCount;
        //}

        //public Inventory Load()
        //{
        //    CreateManager.instance.CreateItemByItemInfo

        //    for( int i = 0; i<weapList.Count; i++ )
        //        weapList[i].item.Type==ItemType.Misc


        //    Inventory loadInventory = new Inventory();
        //}

    }


}