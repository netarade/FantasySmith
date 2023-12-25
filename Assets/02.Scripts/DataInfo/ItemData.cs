using Newtonsoft.Json;
using System;
using UnityEngine;

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
 * 
 * <v6.0 - 2023_1222_�ֿ���>
 * 1- private������ ����ȭ�ϱ� ���� [JsonProperty] ��Ʈ����Ʈ�� �߰��Ͽ���
 * 
 * <v7.0 - 2023_1224_�ֿ���>
 * 1- ItemŬ������ �߻�Ŭ���� ����� �����ϰ�, Clone�޼����� abstract���� ����
 * ������ JSON���� ����ȭ�� �� �ν��Ͻ�ȭ �� �� ���ٴ� ������ ���� �����ϱ� ����
 * 2- ��� �⺻ ������ �����ϰ� �ڵ����� ������Ƽ�� ����Ͽ����ϴ�.
 * ������ �����Ǿ��� �� �ѹ� �ԷµǸ� �� �̻� ������ ������ �ʿ���� �б� �������� �Ҵ�Ǿ�� �ϴ� �Ӽ��̱� �����Դϴ�.
 * 
 */

namespace ItemData
{    
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
    /// �������� ��з��� ��ȭ, ���� ���� ������ �ֽ��ϴ�.
    /// </summary>
    public enum ItemType { Misc, Weapon };
    
    /// <summary>
    /// ������ ���� ������� �ʱ�, �߱�, ����� �ֽ��ϴ�.
    /// </summary>
    public enum ItemGrade { Low, Medium, High }
        
    /// <summary>
    /// �⺻ ������ Ŭ���� - �ݵ�� ����Ͽ� ����ϼ���.
    /// </summary>  
    [Serializable]
    public class Item : ICloneable
    {   
        /// <summary>
        /// �ش� �������� ��з� ���� ������ ����� Weapon, ��ȭ�� Misc���� ��Ÿ���ϴ�.
        /// </summary>
        public ItemType Type { get; }

        /// <summary>
        /// �ش� �������� ������ ���̺� ���ǵ� �ѹ��μ� 0001000 ���� �ѹ����� �����ϴ�. 
        /// </summary>
        public string No {get;}


        /// <summary>
        /// �ش� �������� ������ ���̺� ���� �Ǿ��ִ� �̸�����, string ������ �����Դϴ�.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// �ش� �������� �������� float������ �����Դϴ�.
        /// </summary>
        public float Price { get; set; }
        
        /// <summary>
        /// �ش� �������� �̹����� ǥ���ϴ� �ε��� ������ ��� ����ü �����Դϴ�. 
        /// </summary>
        public ImageReferenceIndex sImageRefIndex { get; }
        
        /// <summary>
        /// �ش� �������� ��� ���� �ε��� �����Դϴ�. �������� ������ �̵��� �� ���� �� ������ �����ؾ� �մϴ�.
        /// </summary>
        public int SlotIndex { get; set; }

        /// <summary>
        /// �ش� �������� ��� ��ü ���Կ� ���� �ε��� �����Դϴ�.
        /// </summary>
        public int SlotIndexAll { get; set; }

        public Item( ItemType type, string No, string name, float price, ImageReferenceIndex imageRefIndex ) 
        {
            Type = type;
            Name = name;
            this.No = No;
            Price = price; 
            sImageRefIndex = imageRefIndex;
        }

        /// <summary>
        /// �ش� �������� ��ü�� �����ؼ� ��ȯ���ִ� �޼����Դϴ�.<br/>
        /// �⺻ ��ü�� =������ ���������̹Ƿ� �ϳ��� �ν��Ͻ��� �����ϰ� �Ǵµ� �̸� �����Ͽ� ���ο� �ν��Ͻ��� ���� �մϴ�.<br/>
        /// </summary>
        public object Clone() { return this.MemberwiseClone(); }


        /// <summary>
        /// �ش� �������� ������ ����� â�� ������ִ� �޼����Դϴ�.
        /// </summary>
        public void ItemDeubgInfo()
        {
            Debug.Log("Type : " + Type);
            Debug.Log("No : " + No);
            Debug.Log("Name : " + Name);
            Debug.Log("Price : " + Price);
            Debug.Log("SlotIndex : " + SlotIndex);
            Debug.Log("SlotIndexAll : " + SlotIndexAll);
        }
    }
}
