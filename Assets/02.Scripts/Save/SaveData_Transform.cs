using CreateManagement;
using ItemData;
using System;
using UnityEngine;

/*
 * [작업 사항]
 * 
 * <v1.0 - 최원준>
 * 1- 최초작성 및 주석처리
 * 
 * <v2.0 - 2024_0123_최원준>
 * 1- STransform에 스케일 변수를 추가
 * 
 * 2- ItemWeapon생성자에서 인스펙터에 정의되어있는 Transform을 바로 변환하여 반환하는 static메서드 GetSTransform을 추가
 * 
 * 3- 디폴트 생성자의 초기화 코드를 Initialize메서드내부에 넣고 재사용
 * 
 * <v3.0 - 2024_0127_최원준>
 * 1- 회전값 저장 변수 w를 추가하였음
 * 이유는 Transform의 rotation값은 기본적으로 오일러각이 아닌 쿼터니언 각이기 때문
 * 
 * 2- SetLossyScale메서드를 추가하여 절대값 정보를 역직렬화 할 때 잠시 부모를 변경하여 초기화하도록 설정
 * 
 * 3- STransform의 두번째 인자로 isLocal 불변수를 만들고, 로컬로 저장할 지, 로드할 지를 결정할 수 있게 하였음
 * 
 * 4- STransform의 세번째 인자로 기준 계층 부모 정보를 받아서, 현재 부모 계층에 속해있지 않아도 해당 기준 계층을 기준으로 한 로컬정보를 저장할 수 있도록 하였음
 * 
 * 5- STransform의 값만 복사하여 새로운 인스턴스를 반환할 수 있도록 Clone인터페이스를 구현하였음.
 * (Item 내부에 Stransform을 저장하고 있는 경우 참조값을 저장하기 때문에, 아이템이 Clone되어도 참조값을 공유하게 되어있음.
 * 따라서 새로운 STransform의 Clone도 같이 교체해서 반환해줘야함.)
 * 
 * 
 * (이슈)
 * 1- STransform의 저장 시 절대값으로 저장해야 하는 경우와 로컬 정보를 저장해야 하는 경우가 있음.
 * 월드 상태 - 절대값 정보, 착용 위치 - 로컬 정보를 기억해야 함.
 * (착용 지점에 전송하는 경우 부모는 착용 Transform에 위치시키지만 세부적인 조정을 로컬로 해야하는 경우가 있기 때문)
 * 
 * 2- 로컬정보를 저장할 때 부모 계층에 속해있지 않은 경우 (즉 자신이 최상위 부모인 경우 로컬 정보가 올바로 저장되지 않음)
 * 
 * <v3.1 - 2024_0130_최원준>
 * 1- GetSTransform호출 인자로 IVCType을 추가하여 다른 IVC도 참조할 수 있도록 변경
 * 
 */


namespace DataManagement
{
    /// <summary>
    /// 주의 사항 - 유니티 전용 클래스는 저장 불가. 기본 자료형으로 저장하거나, 구조체 또는 클래스를 만들어 저장해야 합니다.
    /// </summary>
    public class TransformSaveData : SaveData
    {
        /// <summary>
        /// 플레이어 위치, 회전 정보를 담는 변수입니다.<br/>
        /// 저장 할때는 Serialize메서드를 호출 해야되며,<br/>
        /// 불러 올때는 Deserialize 메서드에 연동할 Transform 인자를 전달하여 정보를 전달받습니다. 
        /// </summary>
        public STransform playerTr;

        /// <summary>
        /// DataManager에서 Load메서드에서 새로운 GameData를 생성하기 위한 생성자입니다.<br/>
        /// 기존의 데이터가 없을 경우 사용됩니다.
        /// </summary>
        public TransformSaveData()
        {
            playerTr = new STransform();    // 새로운 직렬화 트랜스폼 생성 
        }
    }










    /// <summary>
    /// 위치, 회전 정보를 직렬화하기 위한 클래스입니다.<br/> 
    /// 저장할 때는 반드시 Transform 컴포넌트를 전달해서 STransform 인스턴스를 생성해야 합니다.<br/>
    /// 불러올 때는 STransform 인스턴스의 Deserialize메서드를 호출해야 합니다.<br/><br/>
    /// [사용 예시]<br/>
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
        /// 전달인자가 없을 때 새로운 STransform을 만들고 싶을 때 호출하는 생성자입니다. 모든 정보가 0으로 초기화됩니다.
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
        /// 인스펙터로 정의되어있는 VisualCollection-Weapon 인덱스를 인자로 받아<br/>
        /// Transform의 로컬 정보를 전달받아 새로운 STransform을 반환합니다.(저장을 위한 자료형으로 사용되어 집니다.)<br/><br/>
        /// 로컬 정보를 저장할 때 기준 부모를 결정할 수 있습니다. (Transform 정보가 부모 계층 basisParentTr에 속해있지 않은 경우 사용합니다.)
        /// </summary>
        public static STransform GetSTransform(IVCType ivcType, int vcIndex, Transform basisParentTr=null)
        {
            //들어온 인자를 바탕으로 참조할 인덱스를 설정합니다.
            int ivcIdx = (int)ivcType;      

            ItemVisualCollection ivc = 
                GameObject.FindWithTag("GameController").
                transform.GetChild(0).GetChild(ivcIdx).GetComponent<ItemVisualCollection>();

            if (ivc == null)
                throw new Exception("ivc참조가 잡혀있지 않습니다.");
           
            // 인덱스가 배열의 접근범위를 초기환 경우 기본 STransform 반환
            if(vcIndex >= ivc.vcArr.Length)
                return new STransform();
                
            // 인덱스가 배열의 접근범위 이내인 경우 VisualCollection의 Transform정보를 변환하여 STransform반환
            return new STransform(ivc.vcArr[vcIndex].equipLocalTr, true, basisParentTr);
        }   




        /// <summary>
        /// 전달 받은 Transform 컴포넌트의 STransform 인스턴스를 생성하여 반환합니다.<br/>
        /// 내부적으로 Serialize메서드를 전달받은 Transform 인스턴스를 인자로 넣어 호출합니다. 
        /// (즉, Serialize메서드를 호출한것과 동일합니다.)<br/><br/>
        /// 로컬 정보를 저장할 것인지, 월드 정보를 저장할 것인지 결정할 수 있습니다. (기본값: 절대정보)<br/>
        /// 로컬 정보를 저장할 때 세번째 인자로 기준 부모를 결정할 수 있습니다.
        /// (첫번째 Transform tr이 부모 계층 basisParentTr에 속해있지 않은 경우 사용)
        /// </summary>
        public STransform(Transform tr, bool isLocal=false, Transform basisParentTr=null)
        {
            Serialize(tr, isLocal, basisParentTr);
        }


        /// <summary>
        /// STransform의 모든 값을 기본 값으로 초기화합니다.<br/>
        /// 디폴트 생성자를 통한 새로운 생성 또는 Transform값이 null로 전달된 경우에 호출됩니다.
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
        /// 인자로 원본 캐릭터의 위치정보를 전달하면 STransform에 정보를 자동으로 입력하여 저장합니다.<br/>
        /// 기본 생성자로 STransform을 생성한 경우에 사용하세요.<br/><br/>
        /// 로컬 정보를 저장할 것인지, 월드 정보를 저장할 것인지 결정할 수 있습니다. (기본값: 절대정보)<br/>
        /// 로컬 정보를 저장할 때 세번째 인자로 기준 부모를 결정할 수 있습니다.
        /// (첫번째 Transform tr이 부모 계층 basisParentTr에 속해있지 않은 경우 사용)
        /// </summary>
        /// <returns>정보가 입력 된 STransform 인스턴스를 반환합니다.</returns>
        public STransform Serialize(Transform tr, bool isLocal=false, Transform basisParentTr=null)
        {
            // tr값이 전달되지 않은 경우 값을 tr을 사용하지 않고 기본값 초기화
            if( tr==null )
            {
                Initialize();
                return this;
            }
            // 로컬 정보를 저장하는 경우
            if( isLocal )
            {
                Transform prevParentTr = null;  

                if( basisParentTr!=null )           // 기준 부모 옵션을 준 경우
                {
                    prevParentTr = tr.parent;       // 이전 계층 부모 정보를 기록
                    tr.SetParent( basisParentTr );  // 잠시 계층의 부모를 기준 부모로 변경합니다.
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
                    tr.SetParent(prevParentTr);     // 모든 정보를 입력 후 계층의 부모를 다시 원상복구 해줍니다.
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
        /// 저장되어있는 STransform의 위치와 회전 정보를 전달한 Transform 컴포넌트 인자에 동기화 시켜줍니다.<br/>
        /// 즉, 직렬화 가능한 STransform 변수를 로드하여 불러온 경우에 Transform 정보와 동기화 시켜 저장값을 적용시킵니다.<br/><br/>
        /// 
        /// 로컬 정보를 로드할 것인지, 월드 정보를 로드할 것인지 결정할 수 있습니다. (기본값: 절대정보)
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
                SetLossyScale( tr, new Vector3( xScale, yScale, zScale ) ); // tr의 절대 크기를 수정합니다.
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
        /// 인자로 전달된 Transform의 절대 크기를 수정합니다.<br/>
        /// 부모 위치를 저장했다가 스케일 변경후 다시 원래대로 되돌려주는 기능을 갖고 있습니다.
        /// </summary>
        public static void SetLossyScale(Transform tr, Vector3 lossyScale)
        {        
            Transform prevParentTr = tr.parent;
            tr.SetParent(null);
            tr.localScale = lossyScale;
            tr.SetParent(prevParentTr);
        }

        /// <summary>
        /// STransform의 값만 복사하여 새로운 인스턴스를 만들어 반환합니다.
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }






}
