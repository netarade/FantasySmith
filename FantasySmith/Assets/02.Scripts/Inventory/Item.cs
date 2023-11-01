using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

/*
 * [�۾� ����]
 * 
 * v1.0 - 2023_1101_�ֿ���
 * �����ۼ� �� �׽�Ʈ �Ϸ�
 */

namespace ItemData
{
    public enum ItemType { Basic, Additive, Fire, Attribute, Engraving, Sword, Bow };



    /// <summary>
    /// �⺻ ������ �߻� Ŭ���� - �ν��Ͻ��� �������� ���մϴ�. �ݵ�� ����Ͽ� ����ϼ���.
    /// </summary>
    public abstract class Item
    {
        protected ItemType enumType;
        protected string strNo;
        protected string strName;
        protected float fPrice;
        protected ImageCollection icImage;    // �������� �ܺο��� ���߾��� �̹���
                
        public ItemType Type { get { return enumType; } }
        public string Name { get { return strName; } }
        public float Price { get { return fPrice; } }

        public Item()
        {
            Debug.Log("Basic Item �����Ǿ����ϴ�.");            
        }

        public Item( ItemType type, string No, string name, float price)
        {
            enumType = type;
            strName = name;
            strNo = No;
            fPrice = price;             
        }

        public Item( ItemType type, string No, string name, float price, ImageCollection image ) 
            : this(type, No, name, price)
        {            
            icImage = image;
        }
    }



    /// <summary>
    /// ��ȭ ������ - �⺻ �����۰� �ٸ����� �κ��丮�� �ߺ��ؼ� ���� �� �ִٴ� ��
    /// </summary>
    public class ItemMisc : Item
    {        
        private int count;
        public int Count { 
            set { count = value; }
            get { return count; } 
        }

        public ItemMisc()
        {
            Debug.Log("Misc �����Ǿ����ϴ�.");
        }

        public ItemMisc(ItemType type, string No, string name, float price ) 
            : base( type, No, name, price )
        { }

        public ItemMisc( ItemType type, string No, string name, float price, ImageCollection image )
            : base( type, No, name, price, image ) 
        { }
    }


}
