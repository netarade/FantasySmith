using System;
using UnityEngine;
using CraftData;
using System.Collections.Generic;

/*
 * [���� ����]
 * ���� ���̺�� �ε忡 �ʿ��� ������ Ŭ���� ���� 
 * 
 * [�۾� ����]  
 * <v1.0 - 2023_1106_�ֿ���>
 * 1- �ʱ� GameData ����
 * 
 * <v1.1 - 2023_1106_�ֿ���
 * 1- Transform ���� ����, Ŭ���� Craftdic ���� �߰�
 * 2- �ּ� ����
 * 
 * <v1.2 - 2023_1108_�ֿ���>
 * 1- Inventory Ŭ������ ���ӿ�����Ʈ ����Ʈ�� �����ϰ� �ֱ� ������ ����ȵǴ� ������ ������ �˰�
 * ���������� ItemInfo ����Ʈ�� ����� ������ �õ��Ͽ���. 
 * �� �ٸ� �������� �ִµ� ItemInfo�� Image������Ʈ�� �����ϰ� �ֱ� ������ Ŭ���� ������ ����ȭ�ϱ� ����ٰ� ������.
 * 
 * <v2.0 - 2023_1119_�ֿ���>
 * 1- Inventory Ŭ������ ����ȭ �� ������ȭ �����Ϸ�
 * 2- SerializableInventory Ŭ������ Inventory ���Ϸ� ��ġ�� �ű�.
 * �� ��ȯ�ÿ��� ����ϱ� ����
 * 3- �÷��̾� ��ġ, ȸ�� ������ ��� ���� STransform Ŭ���� ����
 * 
 * <v2.1 - 2023_1218_�ֿ���>
 * 1- ��ȭ ��ȭ ���� int������ ����
 * 
 * <v3.0 - 2023_1222_�ֿ���>
 * 1- DataManagerŬ������ Load�޼��忡�� GameData �⺻�����ڸ� ȣ���ϴµ� ���ǰ� �Ǿ����� �ʾ� ���Ӱ� ���� (STransform �⺻ ������ ����)
 * 
 * 2- saveInventory������ private���� public���� �����Ͽ���.
 * public�ƴ� ������ Json���� ����ȭ�� �Ұ����� ������ �Ǳ� ����
 * 
 * 3- �κ��丮 Ŭ������ ������Ƽ�� ����ȭ �κ��丮 Ŭ������ ��ȯ�Ͽ� �����ϴ� ���� �޼��带 ���� ��ȯ�Ͽ� �����ϵ��� ������.
 * (Inventory������Ƽ ����, SaveInventory, LoadInventory�޼��带 �߰�)
 * =>������ ������Ƽ�� set������ ����ȭ������ ������ �����Ѵٰ� �ϴ��� ������Ƽ ��ü�� ���ڸ��� ������ �Ǳ� ������ 
 * GameData�� �ش� ������Ƽ�� ���ԵǾ� ������ ����ȭ ó���� �Ұ����� ���� �߰��Ͽ���. 
 * (Inventory Ŭ������ List<GameObject>�� �����ϱ� ������ ������Ƽ�� GameData�� ���� ���� ����ȭó�� �Ұ����� ������ �ȵǾ���.)
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
        /// �÷��̾� ��ġ, ȸ�� ������ ��� �����Դϴ�.<br/>
        /// ���� �� Transform ������Ʈ�� �����ؼ� ���� �ؾߵǸ�, �ҷ� �ö��� Deserialize �޼��忡 ������ Transform ���ڸ� �����Ͽ� ������ ���޹޽��ϴ�. 
        /// </summary>
        public STransform playerTr;
                
        /// <summary>
        /// ��ȭ
        /// </summary>
        public int gold;

        /// <summary>
        /// ��ȭ
        /// </summary>
        public int silver;

        /// <summary>
        /// �÷��̾��� ���� ���� ��ϰ� ���õ�, ������ ���� Ƚ�� ������ ��� �ִ� ���� ���� Ŭ���� �Դϴ�.
        /// </summary>
        public Craftdic craftDic;

        
        public SerializableInventory savedInventory;

        
        public Inventory LoadInventory()
        {
            return new Inventory(savedInventory);
        }

        public void SaveInventory(Inventory inventory)
        {
            savedInventory = new SerializableInventory(inventory);
        }


        /// <summary>
        /// DataManager���� Load�޼��忡�� ���ο� GameData�� �����ϱ� ���� �������Դϴ�.<br/>
        /// ������ �����Ͱ� ���� ��� ���˴ϴ�.
        /// </summary>
        public GameData()
        {
            playTime = 0f;
            playerTr = new STransform();

            gold = 0;
            silver = 0;

            craftDic = new Craftdic();
            SaveInventory(new Inventory());     // ���ο� ����ȭ �κ��丮 ����  
        }
    }


    /// <summary>
    /// ��ġ, ȸ�� ������ ����ȭ�ϱ� ���� Ŭ�����Դϴ�.<br/> 
    /// ������ ���� �ݵ�� Transform ������Ʈ�� �����ؼ� STransform �ν��Ͻ��� �����ؾ� �մϴ�.<br/>
    /// �ҷ��� ���� STransform �ν��Ͻ��� Deserialize�޼��带 ȣ���ؾ� �մϴ�.<br/><br/>
    /// [��� ����]<br/>
    /// STransform playerTr = new STransform(transform); <br/>
    /// playerTr.DeSerialize(ref transform);
    /// </summary>
    [Serializable]
    public class STransform
    {
        public float x;
        public float y;
        public float z;
        public float xRot;
        public float yRot;
        public float zRot;


        /// <summary>
        /// �������ڰ� ���� �� ���ο� STransform�� ����� ���� �� ȣ���ϴ� �������Դϴ�. ��� ������ 0���� �ʱ�ȭ�˴ϴ�.
        /// </summary>
        public STransform()
        {
            x = 0f;
            y = 0f;
            z = 0f;
            xRot = 0f;
            yRot = 0f;
            zRot = 0f;
        }


        /// <summary>
        /// ���� ���� Transform ������Ʈ�� ��ġ�� ȸ�������� ������ �ͼ� STransform �ν��Ͻ��� �����Ͽ� ��ȯ�մϴ�.
        /// </summary>
        public STransform(Transform tr)
        {
            x = tr.position.x;
            y = tr.position.y;
            z = tr.position.z;
            xRot = tr.rotation.x;
            yRot = tr.rotation.y;
            zRot = tr.rotation.z;
        }

        /// <summary>
        /// ����Ǿ��ִ� STransform�� ��ġ�� ȸ�� ������ ������ Transform ������Ʈ ���ڿ� ����ȭ �����ݴϴ�.
        /// </summary>
        public void Deserialize( ref Transform tr )
        {
            tr.position=new Vector3( x, y, z );
            tr.rotation=Quaternion.Euler( xRot, yRot, zRot );
        }
    }
       




    


}