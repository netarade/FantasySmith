using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace DataManagement
{ 
    /// <summary>
    /// ���� ���� - ����Ƽ ���� Ŭ������ ���� �Ұ�. �⺻ �ڷ������� �����ϰų�, ����ü �Ǵ� Ŭ������ ����� �����ؾ� �մϴ�.
    /// </summary>
    public class TransformSaveData : SaveData
    {
        /// <summary>
        /// �÷��̾� ��ġ, ȸ�� ������ ��� �����Դϴ�.<br/>
        /// ���� �Ҷ��� Serialize�޼��带 ȣ�� �ؾߵǸ�,<br/>
        /// �ҷ� �ö��� Deserialize �޼��忡 ������ Transform ���ڸ� �����Ͽ� ������ ���޹޽��ϴ�. 
        /// </summary>
        public STransform playerTr;
                   
        /// <summary>
        /// DataManager���� Load�޼��忡�� ���ο� GameData�� �����ϱ� ���� �������Դϴ�.<br/>
        /// ������ �����Ͱ� ���� ��� ���˴ϴ�.
        /// </summary>
        public TransformSaveData()
        {
            playerTr = new STransform();    // ���ο� ����ȭ Ʈ������ ���� 
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
        /// ���� ���� Transform ������Ʈ�� ��ġ�� ȸ�������� ������ �ͼ� STransform �ν��Ͻ��� �����Ͽ� ��ȯ�մϴ�.<br/>
        /// ���������� Serialize�޼��带 ���޹��� Transform �ν��Ͻ��� ���ڷ� �־� ȣ���մϴ�. (��, Serialize�޼��带 ȣ���ѰͰ� �����մϴ�.)
        /// </summary>
        public STransform(Transform tr)
        {
            Serialize(tr);
        }


        /// <summary>
        /// ���ڷ� ���� ĳ������ ��ġ������ �����ϸ� STransform�� ������ �ڵ����� �Է��Ͽ� �ݴϴ�.<br/>
        /// �⺻ �����ڷ� STransform�� ������ ��쿡 ����ϼ���.
        /// </summary>
        public void Serialize( Transform tr )
        {
            x = tr.position.x;
            y = tr.position.y;
            z = tr.position.z;
            xRot = tr.rotation.x;
            yRot = tr.rotation.y;
            zRot = tr.rotation.z;
        }


        /// <summary>
        /// ����Ǿ��ִ� STransform�� ��ġ�� ȸ�� ������ ������ Transform ������Ʈ ���ڿ� ����ȭ �����ݴϴ�.<br/>
        /// ��, ����ȭ ������ STransform ������ �ε��Ͽ� �ҷ��� ��쿡 Transform ������ ����ȭ ���� ���尪�� �����ŵ�ϴ�.
        /// </summary>
        public void Deserialize( Transform tr )
        {
            tr.position=new Vector3( x, y, z );
            tr.rotation=Quaternion.Euler( xRot, yRot, zRot );
        }
    }





    
}
