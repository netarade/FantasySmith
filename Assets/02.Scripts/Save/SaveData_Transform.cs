using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




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
    public class STransform
    {
        public float x;
        public float y;
        public float z;
        public float xRot;
        public float yRot;
        public float zRot;


        /// <summary>
        /// 전달인자가 없을 때 새로운 STransform을 만들고 싶을 때 호출하는 생성자입니다. 모든 정보가 0으로 초기화됩니다.
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
        /// 전달 받은 Transform 컴포넌트의 위치와 회전정보만 가지고 와서 STransform 인스턴스를 생성하여 반환합니다.<br/>
        /// 내부적으로 Serialize메서드를 전달받은 Transform 인스턴스를 인자로 넣어 호출합니다. (즉, Serialize메서드를 호출한것과 동일합니다.)
        /// </summary>
        public STransform(Transform tr)
        {
            Serialize(tr);
        }


        /// <summary>
        /// 인자로 원본 캐릭터의 위치정보를 전달하면 STransform에 정보를 자동으로 입력하여 줍니다.<br/>
        /// 기본 생성자로 STransform을 생성한 경우에 사용하세요.
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
        /// 저장되어있는 STransform의 위치와 회전 정보를 전달한 Transform 컴포넌트 인자에 동기화 시켜줍니다.<br/>
        /// 즉, 직렬화 가능한 STransform 변수를 로드하여 불러온 경우에 Transform 정보와 동기화 시켜 저장값을 적용시킵니다.
        /// </summary>
        public void Deserialize( Transform tr )
        {
            tr.position=new Vector3( x, y, z );
            tr.rotation=Quaternion.Euler( xRot, yRot, zRot );
        }
    }





    
}
