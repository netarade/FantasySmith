using CreateManagement;
using System;
using System.Collections;
using System.Collections.Generic;
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
        /// 전달인자가 없을 때 새로운 STransform을 만들고 싶을 때 호출하는 생성자입니다. 모든 정보가 0으로 초기화됩니다.
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
        /// 인스펙터로 정의되어있는 VisualCollection-Weapon 인덱스를 인자로 받아<br/>
        /// Transform weaponTr을 변환하여 새로운 STransform을 반환합니다.(저장을 위한 자료형으로 사용되어 집니다.)
        /// </summary>
        public static STransform GetSTransform(int vcIndex)
        {
            ItemVisualCollection ivcWeapon = 
                GameObject.FindWithTag("GameController").
                transform.GetChild(0).GetChild(0).GetComponent<ItemVisualCollection>();

            if (ivcWeapon == null)
                throw new Exception("ivc참조가 잡혀있지 않습니다.");
           
            // 인덱스가 배열의 접근범위를 초기환 경우 기본 STransform 반환
            if(vcIndex >= ivcWeapon.vcArr.Length)
                return new STransform();
                
            // 인덱스가 배열의 접근범위 이내인 경우 VisualCollection의 Transform정보를 변환하여 STransform반환
            return new STransform(ivcWeapon.vcArr[vcIndex].weaponTr);
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
        /// STransform의 모든 값을 기본 값으로 초기화합니다.<br/>
        /// 디폴트 생성자를 통한 새로운 생성 또는 Transform값이 null로 전달된 경우에 호출됩니다.
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
        /// 인자로 원본 캐릭터의 위치정보를 전달하면 STransform에 정보를 자동으로 입력하여 줍니다.<br/>
        /// 기본 생성자로 STransform을 생성한 경우에 사용하세요.
        /// </summary>
        public STransform Serialize(Transform tr)
        {
            // tr값이 전달되지 않은 경우 값을 tr을 사용하지 않고 기본값 초기화
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
        /// 저장되어있는 STransform의 위치와 회전 정보를 전달한 Transform 컴포넌트 인자에 동기화 시켜줍니다.<br/>
        /// 즉, 직렬화 가능한 STransform 변수를 로드하여 불러온 경우에 Transform 정보와 동기화 시켜 저장값을 적용시킵니다.
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
