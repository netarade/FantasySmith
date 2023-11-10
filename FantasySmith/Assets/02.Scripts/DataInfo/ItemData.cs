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
 * <v1.0 - 2023_1101_�ֿ���>
 * 1- �����ۼ� �� �׽�Ʈ �Ϸ�
 *  
 * <v1.1 - 2023_1102_�ֿ���>  
 * 1- ���ϸ��� Item���� ItemData�� �����Ͽ���. (���� Ŭ������ �����ϱ� ����)
 * 
 * 2- ItemŬ������ ItemType�� ��з� �׸����� ����.
 * ��ӹ��� Ŭ������ �ش�Ŭ������ �°� �ߺз� Type�� �������� �Ͽ���.
 * ���� Ŭ������ �ߺз� Type ���� �� �����ڵ� ����.
 * 
 * 3- ������ ����ȭ
 * �������� ������ �̸� ����� ���� �� 
 * �̹����� ���� �ʴ� �����ڳ�, ����Ʈ �����ڴ� ������� ���� �����̹Ƿ�.
 * ��, ��� �����Ϳ� �̹������� �ְ� �������� ����.
 * 
 * 4- Item Ŭ������ ICloneable �������̽� �߻����·� ���� �� �� �ڽ� Ŭ�������� ���� �����ϵ��� ����.
 * ������ ������ �������� ���Ӱ� �����ϴ� ���̹Ƿ�, �Ϲ����� ����������δ� ��ü�� �̸� ������ �������� �Ѵ�.
 * ������ ���� ���� ������ ���ο� ��ü�� �Ҵ���� �� �ְ� �Ѵ�.
 * 
 * <v2.0 - 2023_1103_�ֿ���>
 * 1- �������� ���� �����ִ� �κ��丮�� ���� �ε����� ������ ������ �ֵ��� �Ͽ���.
 * �κ��丮 ����Ʈ�� �������� ���� �� ���������� �־ �����ϸ�, 
 * �κ��丮���� �������� ������ ������ ��, ���� ������ �ε��� ����Ʈ�� �����ϴ� �ͺ��� �� ���� ���� ���� ����.
 *  
 * 2- ItemDebugInfo �޼��� �߰�. ȣ�� �� ������ ������ ����׻����� ǥ��
 * 
 * <v3.0 - 2023_1105_�ֿ���>
 * 1- ���� Ÿ�Կ� ���� �Ķ���� �߰�
 * 
 * <v4.0 - 2023_1105_�ֿ���>
 * 1- ���⵵�� ���� ��ȭ,����������� Ŭ������ �� ���Ϸ� �и�, ���� ���Ͽ��� �⺻ ������ Ŭ������ ���д�.
 * 
 */

namespace ItemData
{    
    /// <summary>
    /// �������� ��з�
    /// </summary>
    public enum ItemType { Misc, Weapon };
    
    /// <summary>
    /// ������ ���� ���
    /// </summary>
    public enum ItemGrade { Low, Medium, High }


    
    /// <summary>
    /// �⺻ ������ �߻� Ŭ���� - �ν��Ͻ��� �������� ���մϴ�. �ݵ�� ����Ͽ� ����ϼ���. ����� Ŭ������ ICloneable�� �����ؾ��մϴ�.
    /// </summary>
    public abstract class Item : ICloneable
    {
        protected ItemType enumType;        // Ÿ��
        protected string strNo;             // ��ȣ
        protected string strName;           // �̸�
        protected float fPrice;             // ����   
        protected ImageCollection icImage;  // �������� �κ��丮 ���� �̹���, ����â �̹��� ���� �����Ѵ�.
        protected int slotIndex;            // �������� �ڽ��� �����ִ� �κ��丮�� ���� �ε��� ������ ������.
        
        public ItemType Type { get { return enumType; } }
        public string Name { get { return strName; } }
        public float Price { get { return fPrice; } }
        
        /// <summary>
        /// �������� �̹����� ǥ���ϴ� ����ü�� ��� �����Դϴ�. 
        /// </summary>
        public ImageCollection Image { 
            get { return icImage;} 
            set { icImage = value; }
        }
        
        /// <summary>
        /// �ش� �������� ��� ���� �ε��� �����Դϴ�. �������� ������ �̵��� �� ���� �� ������ �����ؾ� �մϴ�.
        /// </summary>
        public int SlotIndex 
        { 
            set { slotIndex = value; }
            get { return slotIndex; } 
        }

        public Item( ItemType type, string No, string name, float price, ImageCollection image ) 
        {
            enumType = type;
            strName = name;
            strNo = No;
            fPrice = price; 
            icImage = image;
        }

        public abstract object Clone();
        public void ItemDeubgInfo()
        {
            Debug.Log("Type : " + enumType);
            Debug.Log("No : " + strNo);
            Debug.Log("Name : " + strName);
            Debug.Log("Price : " + fPrice);
            Debug.Log("SlotIndex : " + slotIndex);
            Debug.Log("ImageDesc : " + icImage.itemDesc);
        }
    }
}
