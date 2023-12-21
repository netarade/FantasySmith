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
 * <v5.0 - 2023_1116_�ֿ���>
 * 1- ������ ������ ���� ItemŬ������ ImageCollection ������� icImage�� �����ϰ� ImageReferenceIndex ����ü ���� sImgRefIdx�� ����
 * (ItemŬ������ �̹��� ���� ���� ��Ŀ��� �̹��� �ε��� ���������� ����)
 * 
 * 2- slotIndex�� �� ������ ���� ���� �ε����� ó���ϰ� ��ü �ǿ����� �ε����� ó���ϱ� ���� slotIndexAll�� �߰��Ͽ���.
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
    /// �ܺ��� ������ �̹����� ������ �ε����� ���ִ� ����ü �Դϴ�.
    /// </summary>
    [Serializable]
    public struct ImageReferenceIndex
    {
        /// <summary>
        /// �κ��丮 �����̹��� �ε���
        /// </summary>
        public int innerImgIdx;

        /// <summary>
        /// ����â �̹��� �ε���
        /// </summary>
        public int statusImgIdx;

        /// <summary>
        /// �κ��丮 �ܺ��̹��� �ε���
        /// </summary>
        public int outerImgIdx; 

        /// <summary>
        /// �κ��丮 �����̹���, �����̹���, �ܺ��̹����� �ε����ѹ��� �����ϰ� �����Ͽ� �ε��� ����ü�� �����մϴ�.
        /// </summary>
        public ImageReferenceIndex(int index)
        {
            innerImgIdx = index;
            statusImgIdx = index;
            outerImgIdx = index;
        }

        /// <summary>
        /// �κ��丮 �����̹���, �����̹���, �ܺ��̹����� �ε����ѹ��� ���� �����Ͽ� �ε��� ����ü�� �����մϴ�.
        /// </summary>
        public ImageReferenceIndex(int innerImageIndex, int statusImageIndex, int outerImageIndex )
        {
            innerImgIdx = innerImageIndex;
            statusImgIdx = statusImageIndex;
            outerImgIdx = outerImageIndex;
        }

    }


    
    /// <summary>
    /// �⺻ ������ �߻� Ŭ���� - �ν��Ͻ��� �������� ���մϴ�. �ݵ�� ����Ͽ� ����ϼ���. ����� Ŭ������ ICloneable�� �����ؾ��մϴ�.
    /// </summary>
    [Serializable]
    public abstract class Item : ICloneable
    {
        protected ItemType enumType;        // Ÿ��
        protected string strNo;             // ��ȣ
        protected string strName;           // �̸�
        protected float fPrice;             // ����   

        protected int slotIndex;            // �������� �ڽ��� �����ִ� �κ��丮�� ���� �ε��� ������ ������.
        protected int slotIndexAll;         // �������� ��ü �ǿ����� ��ġ�� ����
        
        protected ImageReferenceIndex sImgRefIdx;    // �������� �̹����� ������ �ε��� ������ ���� ����ü ���� 


        public ItemType Type { get { return enumType; } }
        public string Name { get { return strName; } }
        public float Price { get { return fPrice; } }
        
        /// <summary>
        /// �������� �̹����� ǥ���ϴ� �ε��� ������ ��� ����ü �����Դϴ�. 
        /// </summary>
        public ImageReferenceIndex sImageRefIndex { 
            get { return sImgRefIdx;} 
            set { sImgRefIdx = value; }
        }
        
        /// <summary>
        /// �ش� �������� ��� ���� �ε��� �����Դϴ�. �������� ������ �̵��� �� ���� �� ������ �����ؾ� �մϴ�.
        /// </summary>
        public int SlotIndex 
        { 
            set { slotIndex = value; }
            get { return slotIndex; } 
        }

        /// <summary>
        /// �ش� �������� ��� ��ü ���Կ� ���� �ε��� �����Դϴ�.
        /// </summary>
        public int SlotIndexAll { set { slotIndexAll=value;} get { return slotIndexAll;} }

        public Item( ItemType type, string No, string name, float price, ImageReferenceIndex imageRefIndex ) 
        {
            enumType = type;
            strName = name;
            strNo = No;
            fPrice = price; 
            sImgRefIdx = imageRefIndex;
        }

        public abstract object Clone();
        public void ItemDeubgInfo()
        {
            Debug.Log("Type : " + enumType);
            Debug.Log("No : " + strNo);
            Debug.Log("Name : " + strName);
            Debug.Log("Price : " + fPrice);
            Debug.Log("SlotIndex : " + slotIndex);
            Debug.Log("SlotIndexAll : " + slotIndexAll);
        }
    }
}
