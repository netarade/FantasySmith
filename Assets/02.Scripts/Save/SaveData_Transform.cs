using CreateManagement;
using ItemData;
using System;
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
 * <v3.0 - 2024_0127_�ֿ���>
 * 1- ȸ���� ���� ���� w�� �߰��Ͽ���
 * ������ Transform�� rotation���� �⺻������ ���Ϸ����� �ƴ� ���ʹϾ� ���̱� ����
 * 
 * 2- SetLossyScale�޼��带 �߰��Ͽ� ���밪 ������ ������ȭ �� �� ��� �θ� �����Ͽ� �ʱ�ȭ�ϵ��� ����
 * 
 * 3- STransform�� �ι�° ���ڷ� isLocal �Һ����� �����, ���÷� ������ ��, �ε��� ���� ������ �� �ְ� �Ͽ���
 * 
 * 4- STransform�� ����° ���ڷ� ���� ���� �θ� ������ �޾Ƽ�, ���� �θ� ������ �������� �ʾƵ� �ش� ���� ������ �������� �� ���������� ������ �� �ֵ��� �Ͽ���
 * 
 * 5- STransform�� ���� �����Ͽ� ���ο� �ν��Ͻ��� ��ȯ�� �� �ֵ��� Clone�������̽��� �����Ͽ���.
 * (Item ���ο� Stransform�� �����ϰ� �ִ� ��� �������� �����ϱ� ������, �������� Clone�Ǿ �������� �����ϰ� �Ǿ�����.
 * ���� ���ο� STransform�� Clone�� ���� ��ü�ؼ� ��ȯ�������.)
 * 
 * 
 * (�̽�)
 * 1- STransform�� ���� �� ���밪���� �����ؾ� �ϴ� ���� ���� ������ �����ؾ� �ϴ� ��찡 ����.
 * ���� ���� - ���밪 ����, ���� ��ġ - ���� ������ ����ؾ� ��.
 * (���� ������ �����ϴ� ��� �θ�� ���� Transform�� ��ġ��Ű���� �������� ������ ���÷� �ؾ��ϴ� ��찡 �ֱ� ����)
 * 
 * 2- ���������� ������ �� �θ� ������ �������� ���� ��� (�� �ڽ��� �ֻ��� �θ��� ��� ���� ������ �ùٷ� ������� ����)
 * 
 * <v3.1 - 2024_0130_�ֿ���>
 * 1- GetSTransformȣ�� ���ڷ� IVCType�� �߰��Ͽ� �ٸ� IVC�� ������ �� �ֵ��� ����
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
    public class STransform : ICloneable
    {
        public float xPos;
        public float yPos;
        public float zPos;

        public float xRot;
        public float yRot;
        public float zRot;
        public float wRot;

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

        public STransform( Vector3 vec3 )
        {
            xPos = vec3.x;
            yPos = vec3.y;
            zPos = vec3.z;
        }


        /// <summary>
        /// �ν����ͷ� ���ǵǾ��ִ� VisualCollection-Weapon �ε����� ���ڷ� �޾�<br/>
        /// Transform�� ���� ������ ���޹޾� ���ο� STransform�� ��ȯ�մϴ�.(������ ���� �ڷ������� ���Ǿ� ���ϴ�.)<br/><br/>
        /// ���� ������ ������ �� ���� �θ� ������ �� �ֽ��ϴ�. (Transform ������ �θ� ���� basisParentTr�� �������� ���� ��� ����մϴ�.)
        /// </summary>
        public static STransform GetSTransform(IVCType ivcType, int vcIndex, Transform basisParentTr=null)
        {
            //���� ���ڸ� �������� ������ �ε����� �����մϴ�.
            int ivcIdx = (int)ivcType;      

            ItemVisualCollection ivc = 
                GameObject.FindWithTag("GameController").
                transform.GetChild(0).GetChild(ivcIdx).GetComponent<ItemVisualCollection>();

            if (ivc == null)
                throw new Exception("ivc������ �������� �ʽ��ϴ�.");
           
            // �ε����� �迭�� ���ٹ����� �ʱ�ȯ ��� �⺻ STransform ��ȯ
            if(vcIndex >= ivc.vcArr.Length)
                return new STransform();
                
            // �ε����� �迭�� ���ٹ��� �̳��� ��� VisualCollection�� Transform������ ��ȯ�Ͽ� STransform��ȯ
            return new STransform(ivc.vcArr[vcIndex].equipLocalTr, true, basisParentTr);
        }   




        /// <summary>
        /// ���� ���� Transform ������Ʈ�� STransform �ν��Ͻ��� �����Ͽ� ��ȯ�մϴ�.<br/>
        /// ���������� Serialize�޼��带 ���޹��� Transform �ν��Ͻ��� ���ڷ� �־� ȣ���մϴ�. 
        /// (��, Serialize�޼��带 ȣ���ѰͰ� �����մϴ�.)<br/><br/>
        /// ���� ������ ������ ������, ���� ������ ������ ������ ������ �� �ֽ��ϴ�. (�⺻��: ��������)<br/>
        /// ���� ������ ������ �� ����° ���ڷ� ���� �θ� ������ �� �ֽ��ϴ�.
        /// (ù��° Transform tr�� �θ� ���� basisParentTr�� �������� ���� ��� ���)
        /// </summary>
        public STransform(Transform tr, bool isLocal=false, Transform basisParentTr=null)
        {
            Serialize(tr, isLocal, basisParentTr);
        }


        /// <summary>
        /// STransform�� ��� ���� �⺻ ������ �ʱ�ȭ�մϴ�.<br/>
        /// ����Ʈ �����ڸ� ���� ���ο� ���� �Ǵ� Transform���� null�� ���޵� ��쿡 ȣ��˴ϴ�.
        /// </summary>
        public void Initialize()
        {
            xPos = 0f;
            yPos = 0f;
            zPos = 0f;

            xRot = Quaternion.identity.x;
            yRot = Quaternion.identity.y;
            zRot = Quaternion.identity.z;
            wRot = Quaternion.identity.w;

            xScale = 1f;
            yScale = 1f;
            zScale = 1f;
        }



        /// <summary>
        /// ���ڷ� ���� ĳ������ ��ġ������ �����ϸ� STransform�� ������ �ڵ����� �Է��Ͽ� �����մϴ�.<br/>
        /// �⺻ �����ڷ� STransform�� ������ ��쿡 ����ϼ���.<br/><br/>
        /// ���� ������ ������ ������, ���� ������ ������ ������ ������ �� �ֽ��ϴ�. (�⺻��: ��������)<br/>
        /// ���� ������ ������ �� ����° ���ڷ� ���� �θ� ������ �� �ֽ��ϴ�.
        /// (ù��° Transform tr�� �θ� ���� basisParentTr�� �������� ���� ��� ���)
        /// </summary>
        /// <returns>������ �Է� �� STransform �ν��Ͻ��� ��ȯ�մϴ�.</returns>
        public STransform Serialize(Transform tr, bool isLocal=false, Transform basisParentTr=null)
        {
            // tr���� ���޵��� ���� ��� ���� tr�� ������� �ʰ� �⺻�� �ʱ�ȭ
            if( tr==null )
            {
                Initialize();
                return this;
            }
            // ���� ������ �����ϴ� ���
            if( isLocal )
            {
                Transform prevParentTr = null;  

                if( basisParentTr!=null )           // ���� �θ� �ɼ��� �� ���
                {
                    prevParentTr = tr.parent;       // ���� ���� �θ� ������ ���
                    tr.SetParent( basisParentTr );  // ��� ������ �θ� ���� �θ�� �����մϴ�.
                }

                xPos=tr.localPosition.x;
                yPos=tr.localPosition.y;
                zPos=tr.localPosition.z;
                
                xRot=tr.localRotation.x;
                yRot=tr.localRotation.y;
                zRot=tr.localRotation.z;
                wRot=tr.localRotation.w;
                
                xScale=tr.localScale.x;
                yScale=tr.localScale.y;
                zScale=tr.localScale.z;

                
                if( basisParentTr!=null )           
                    tr.SetParent(prevParentTr);     // ��� ������ �Է� �� ������ �θ� �ٽ� ���󺹱� ���ݴϴ�.
            }
            else
            {                
                xPos=tr.position.x;
                yPos=tr.position.y;
                zPos=tr.position.z;

                xRot=tr.rotation.x;
                yRot=tr.rotation.y;
                zRot=tr.rotation.z;
                wRot=tr.rotation.w;

                xScale=tr.lossyScale.x;
                yScale=tr.lossyScale.y;
                zScale=tr.lossyScale.z;
            }

            return this;
        }


        /// <summary>
        /// ����Ǿ��ִ� STransform�� ��ġ�� ȸ�� ������ ������ Transform ������Ʈ ���ڿ� ����ȭ �����ݴϴ�.<br/>
        /// ��, ����ȭ ������ STransform ������ �ε��Ͽ� �ҷ��� ��쿡 Transform ������ ����ȭ ���� ���尪�� �����ŵ�ϴ�.<br/><br/>
        /// 
        /// ���� ������ �ε��� ������, ���� ������ �ε��� ������ ������ �� �ֽ��ϴ�. (�⺻��: ��������)
        /// </summary>
        public void Deserialize(Transform tr, bool isLocal=false)
        {
            if( isLocal )
            {
                tr.localPosition = new Vector3( xPos, yPos, zPos );
                tr.localRotation = new Quaternion( xRot, yRot, zRot, wRot );
                tr.localScale = new Vector3( xScale, yScale, zScale );
            }
            else
            {
                tr.position=new Vector3( xPos, yPos, zPos );
                tr.rotation=new Quaternion( xRot, yRot, zRot, wRot );
                SetLossyScale( tr, new Vector3( xScale, yScale, zScale ) ); // tr�� ���� ũ�⸦ �����մϴ�.
            }
        }

        public override string ToString()
        {            
            string str = string.Format( 
                $"pos({new Vector3(xPos,yPos, zPos)})\n" +
                $"rot({new Quaternion(xRot,yRot,zRot,wRot).eulerAngles})\n" +
                $"scale({new Vector3(xScale, yScale, zScale)})\n"
                );

            return str;
        }


        /// <summary>
        /// ���ڷ� ���޵� Transform�� ���� ũ�⸦ �����մϴ�.<br/>
        /// �θ� ��ġ�� �����ߴٰ� ������ ������ �ٽ� ������� �ǵ����ִ� ����� ���� �ֽ��ϴ�.
        /// </summary>
        public static void SetLossyScale(Transform tr, Vector3 lossyScale)
        {        
            Transform prevParentTr = tr.parent;
            tr.SetParent(null);
            tr.localScale = lossyScale;
            tr.SetParent(prevParentTr);
        }

        /// <summary>
        /// STransform�� ���� �����Ͽ� ���ο� �ν��Ͻ��� ����� ��ȯ�մϴ�.
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }






}
