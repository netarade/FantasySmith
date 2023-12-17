using System;
using UnityEngine;
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

        
        private SerializableInventory savedInventory;

        /// <summary>
        /// ���� �÷��̾ �����ϰ� �ִ� �κ��丮 �����Դϴ�. <br/>
        /// Inventory �ν��Ͻ��� �����ϸ� ���մϴ�. (���ο� ����ȭ ������ Ŭ������ �ΰ� �ڵ� ��ȯ�� �̷�����ϴ�.)
        /// </summary>
        public Inventory inventory
        {
            set{ savedInventory = new SerializableInventory( value ); }
            get{ return new Inventory( savedInventory ); }
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
    public class STransform
    {
        public float x;
        public float y;
        public float z;
        public float xRot;
        public float yRot;
        public float zRot;

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
        public void Deserialize(ref Transform tr)
        {
            tr.position = new Vector3( x, y, z );
            tr.rotation = Quaternion.Euler( xRot, yRot, zRot );
        }
    }
       




    


}