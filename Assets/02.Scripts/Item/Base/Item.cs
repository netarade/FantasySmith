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
 * <v7.1 - 2023_1226_�ֿ���>
 * 1- ItemŬ������ �ٽ� abstractŬ������ �ٽ� �ѹ��Ͽ���.
 * ������ ����ȭ�Ͽ� ������ �� �ֻ��� Ŭ������ �����ع����� �ڽ��� �������� ������� ������ �ٽ� �ڽ����� ����ȯ�� ĳ���ÿ����� �߻��ϱ� ����
 * �ش� ������ �ذ��ϱ� ���� ������ ��, �� ���� �ڽ� Ŭ������ ��Ƽ� �����ϴ� �������� ���� ����
 * 
 * <v8.0 - 2023_1226_�ֿ���>
 * 1- ������ �� �ε��� ����ü ������ 0���� �ε�Ǵ� ������ �߻��Ͽ� 
 * ���캸�� Json���� ������ȭ �� ������Ƽ ���� ä����� �ϴµ� �ڵ����� ������Ƽ�� set�� ���� �������� �߰�
 * set�� �߰��ϸ� ������ ������ �ֱ⿡, ������Ƽ ������� �ٽ� �ѹ��Ͽ� ������Ƽ�� JsonIgnore ó���Ͽ� ���� �� �����ڿ� �Ҹ� ����
 * 
 * <v8.1 - 2023_1229_�ֿ���>
 * 1- ���ϸ� ���� ItemData->Item
 * 2- ImageReferenceIndex ���κ��� outerImgIdx���� meshFilterIdx�� materialIdx�� ����
 * 
 * <v8.2 - 2023_1230_�ֿ���>
 * 1- ItemType�� None�� �߰�
 * ���� ������ Null������ ����ϱ� ����
 * 
 * <v9.0 - 2024_0108_�ֿ���>
 * 1- ����ü�� ImageRefenreceIndex�� VisualReferenceIndex�� �����ϰ�
 * outerMeshFilter, outerMaterial ������ ���� �� outerPrefabIdx ������ �߰��Ͽ����ϴ�.
 * ������ ���Ϳ� ���͸����� ���� �����ϴ� �ͺ��� ������Ʈ ���·� �����ϴ� ���� ���� ������ 
 * CreateManager���� 2D�����տ� 3D�������� �ٿ��ִ� ������� �����ϱ�� �����Ͽ����ϴ�.
 * 
 * 2- ������ sImageRefIndex�� sVisualRefIndex�� ����, ������Ƽ ImageRefIndex�� VisualRefIndex�� ����
 * 
 * 3- ������ �Ű����� imageRefIndex�� visualRefIndex�� ����
 * 
 * <v10.0 - 2024_0111_�ֿ���>
 * 1- ũ������ �帣�� �°� Ŭ���� ���� ���� (�ʿ���� ���� ����)
 * 
 * <v10.1 - 2024_0115_�ֿ���>
 * 1- ���� �� ������Ƽ�� SlotIndex�� SlotIndexEach�� ����
 * 
 * <v10.2 - 2024_0124_�ֿ���>
 * 1- ItemŬ������ �����ؾ��� ������ onwerId�� �߰��Ͽ���
 * ������ ����� ��ġ�ϴ� �������� ��� �����ָ� �����ؼ� ����Ʈ ���� ���θ� �Ǵ��ϸ�, �����ϰ� �ҷ��;��� ��찡 �ֱ� ����
 * 
 * <v10.3 - 2024_0124_�ֿ���>
 * 1- owerId�� Ÿ���� string���� int�� ���� �� �ּ� ����
 * ������ ������� id�� �������� �ο��ϱ� ����
 * 
 * 
 */

namespace ItemData
{    
    /// <summary>
    /// �ܺ��� �������� ����� ������ �ε����� �����ϴ� ����ü �Դϴ�.
    /// </summary>
    [Serializable]
    public struct VisualReferenceIndex
    {
        /// <summary>
        /// �������� �κ��丮 �����̹��� �ε���
        /// </summary>
        public int innerImgIdx;

        /// <summary>
        /// �������� ����â �̹��� �ε���
        /// </summary>
        public int statusImgIdx;

        /// <summary>
        /// �������� ���� ������Ʈ �ε���
        /// </summary>
        public int outerPrefabIdx; 



        /// <summary>
        /// �κ��丮 �����̹���, �����̹���, �ܺ��̹����� �ε����ѹ��� �����ϰ� �����Ͽ� �ε��� ����ü�� �����մϴ�.
        /// </summary>
        public VisualReferenceIndex(int index)
        {
            innerImgIdx = index;
            statusImgIdx = index;
            outerPrefabIdx = index;
        }

        /// <summary>
        /// �κ��丮 �����̹���, �����̹���, �ܺ��̹����� �ε����ѹ��� ���� �����Ͽ� �ε��� ����ü�� �����մϴ�.
        /// </summary>
        public VisualReferenceIndex(int innerImageIndex, int statusImageIndex, int outerPrefabIndex )
        {
            innerImgIdx = innerImageIndex;
            statusImgIdx = statusImageIndex;
            outerPrefabIdx = outerPrefabIndex;
        }

    }


    /// <summary>
    /// �������� ��з��� ���� ��ȭ, ����, ����Ʈ 3���� ������ �ֽ��ϴ�.
    /// </summary>
    public enum ItemType { Misc, Weapon, Quest ,None };
    

    /// <summary>
    /// �⺻ ������ Ŭ���� - �ݵ�� ����Ͽ� ����ϼ���.
    /// </summary>  
    [Serializable]
    public abstract class Item : ICloneable
    {   
        [JsonProperty] ItemType eType;
        [JsonProperty] string sNo;
        [JsonProperty] string sName;
        [JsonProperty] string sDesc;

        [JsonProperty] VisualReferenceIndex sVisualRefIndex;
        [JsonProperty] int iSlotIndexEach;
        [JsonProperty] int iSlotIndexAll;
        [JsonProperty] int iOwnerId;





        /// <summary>
        /// �ش� �������� ��з� ���� ������ ����� Weapon, ��ȭ�� Misc���� ��Ÿ���ϴ�.
        /// </summary>
        [JsonIgnore] public ItemType Type { get {return eType; } }

        /// <summary>
        /// �ش� �������� ������ ���̺� ���ǵ� �ѹ��μ� 0001000 ���� �ѹ����� �����ϴ�. 
        /// </summary>
        [JsonIgnore] public string No { get {return sNo;} } 


        /// <summary>
        /// �ش� �������� ������ ���̺� ���� �Ǿ��ִ� �̸�����, string ������ �����Դϴ�.
        /// </summary>
        [JsonIgnore] public string Name { get {return sName;} }

                
        /// <summary>
        /// �ش� �������� ���Ǹ� �������� ǥ�����ִ� ��������, string ������ �����Դϴ�.
        /// </summary>
        [JsonIgnore] public string Desc { get {return sDesc;} }


        
        /// <summary>
        /// �ش� �������� �̹����� ǥ���ϴ� �ε��� ������ ��� ����ü �����Դϴ�. 
        /// </summary>
        [JsonIgnore] public VisualReferenceIndex VisualRefIndex { get {return sVisualRefIndex;} }
        
        /// <summary>
        /// �ش� �������� ��� ���� �ε��� �����Դϴ�. �������� ������ �̵��� �� ���� �� ������ �����ؾ� �մϴ�.
        /// </summary>
        [JsonIgnore] public int SlotIndexEach { get{return iSlotIndexEach;} set{iSlotIndexEach = value;} }

        /// <summary>
        /// �ش� �������� ��� ��ü ���Կ� ���� �ε��� �����Դϴ�.
        /// </summary>
        [JsonIgnore] public int SlotIndexAll { get{return iSlotIndexAll; } set{iSlotIndexAll=value;} }


        /// <summary>
        /// �ش� �������� �����ڸ� �ĺ��� �� �ִ� ���� ��ȣ�� ���մϴ�.<br/>
        /// � �κ��丮�� ����Ǵ� ���� ���� �ش� �������� ������Id�� �����Ǿ����ϴ�.
        /// </summary>
        [JsonIgnore] public int OwnerId { get { return iOwnerId; } set { iOwnerId=value; } }





        public Item( ItemType type, string No, string name, VisualReferenceIndex visualRefIndex, string desc ) 
        {
            eType = type;
            sName = name;
            sNo = No;
            sVisualRefIndex = visualRefIndex;
            sDesc = desc;
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
            Debug.Log("SlotIndexEach : " + SlotIndexEach);
            Debug.Log("SlotIndexAll : " + SlotIndexAll);
            Debug.Log("Desc : " + sDesc);
        }
    }
}
