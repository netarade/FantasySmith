using CreateManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * [�۾� ����]
 * 
 * <v1.0 - �ֿ���>
 * 1- �����ۼ� �� �ּ�ó��
 * 
 * <v2.0 - 2024_0123_�ֿ���>
 * 1- STransform�� ������ ������ �߰�
 * 
 * 2- ItemWeapon�����ڿ��� �ν����Ϳ� ���ǵǾ��ִ� Transform�� �ٷ� ��ȯ�Ͽ� ��ȯ�ϴ� static�޼��� GetSTransform�� �߰�
 * 
 * 3- ����Ʈ �������� �ʱ�ȭ �ڵ带 Initialize�޼��峻�ο� �ְ� ����
 * 
 */


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
        public float xScale;
        public float yScale;
        public float zScale;


        /// <summary>
        /// �������ڰ� ���� �� ���ο� STransform�� ����� ���� �� ȣ���ϴ� �������Դϴ�. ��� ������ 0���� �ʱ�ȭ�˴ϴ�.
        /// </summary>
        public STransform()
        {
            Initialize();
        }

        public STransform(GameObject equipWeapons)
        {
            this.x = equipWeapons.transform.position.x;
            this.y = equipWeapons.transform.position.y;
            this.z = equipWeapons.transform.position.z;
            this.xRot = equipWeapons.transform.rotation.x;
            this.yRot = equipWeapons.transform.rotation.y;
            this.zRot = equipWeapons.transform.rotation.z;
            this.xScale = equipWeapons.transform.localScale.x;
            this.yScale = equipWeapons.transform.localScale.y;
            this.zScale = equipWeapons.transform.localScale.z;
        }


        /// <summary>
        /// �ν����ͷ� ���ǵǾ��ִ� VisualCollection-Weapon �ε����� ���ڷ� �޾�<br/>
        /// Transform weaponTr�� ��ȯ�Ͽ� ���ο� STransform�� ��ȯ�մϴ�.(������ ���� �ڷ������� ���Ǿ� ���ϴ�.)
        /// </summary>
        public static STransform GetSTransform(int vcIndex)
        {
            ItemVisualCollection ivcWeapon = 
                GameObject.FindWithTag("GameController").
                transform.GetChild(0).GetChild(0).GetComponent<ItemVisualCollection>();

            if (ivcWeapon == null)
                throw new Exception("ivc������ �������� �ʽ��ϴ�.");
           
            // �ε����� �迭�� ���ٹ����� �ʱ�ȯ ��� �⺻ STransform ��ȯ
            if(vcIndex >= ivcWeapon.vcArr.Length)
                return new STransform();
                
            // �ε����� �迭�� ���ٹ��� �̳��� ��� VisualCollection�� Transform������ ��ȯ�Ͽ� STransform��ȯ
            return new STransform(ivcWeapon.vcArr[vcIndex].weaponTr);
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
        /// STransform�� ��� ���� �⺻ ������ �ʱ�ȭ�մϴ�.<br/>
        /// ����Ʈ �����ڸ� ���� ���ο� ���� �Ǵ� Transform���� null�� ���޵� ��쿡 ȣ��˴ϴ�.
        /// </summary>
        public void Initialize()
        {
            x = 0f;
            y = 0f;
            z = 0f;
            xRot = 0f;
            yRot = 0f;
            zRot = 0f;
            xScale = 1f;
            yScale = 1f;
            zScale = 1f;
        }



        /// <summary>
        /// ���ڷ� ���� ĳ������ ��ġ������ �����ϸ� STransform�� ������ �ڵ����� �Է��Ͽ� �ݴϴ�.<br/>
        /// �⺻ �����ڷ� STransform�� ������ ��쿡 ����ϼ���.
        /// </summary>
        public STransform Serialize(Transform tr)
        {
            // tr���� ���޵��� ���� ��� ���� tr�� ������� �ʰ� �⺻�� �ʱ�ȭ
            if( tr==null )
            {
                Initialize();
                return this;
            }

            x = tr.position.x;
            y = tr.position.y;
            z = tr.position.z;
            xRot = tr.rotation.x;
            yRot = tr.rotation.y;
            zRot = tr.rotation.z;
            xScale = tr.localScale.x;
            yScale = tr.localScale.y;
            zScale = tr.localScale.z;

            return this;
        }


        /// <summary>
        /// ����Ǿ��ִ� STransform�� ��ġ�� ȸ�� ������ ������ Transform ������Ʈ ���ڿ� ����ȭ �����ݴϴ�.<br/>
        /// ��, ����ȭ ������ STransform ������ �ε��Ͽ� �ҷ��� ��쿡 Transform ������ ����ȭ ���� ���尪�� �����ŵ�ϴ�.
        /// </summary>
        public void Deserialize(Transform tr)
        {
            tr.position = new Vector3(x, y, z);
            tr.rotation = Quaternion.Euler(xRot, yRot, zRot);
            tr.localScale = new Vector3(xScale, yScale, zScale);
        }

        public override string ToString()
        {            
            string str = string.Format( 
                $"worldPos({x},{y},{y})\n" +
                $"worldRot({xRot},{yRot},{zRot})\n" +
                $"localScale({xScale},{yScale},{zScale})\n" );

            return str;
        }
    }






}
