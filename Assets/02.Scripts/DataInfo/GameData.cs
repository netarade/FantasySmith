using System;
using UnityEngine;
using CraftData;

/*
 * [파일 목적]
 * 게임 세이브와 로드에 필요한 데이터 클래스 구성 
 * 
 * [작업 사항]  
 * <v1.0 - 2023_1106_최원준>
 * 1- 초기 GameData 설정
 * 
 * <v1.1 - 2023_1106_최원준
 * 1- Transform 변수 제거, 클래스 Craftdic 변수 추가
 * 2- 주석 수정
 * 
 * <v1.2 - 2023_1108_최원준>
 * 1- Inventory 클래스가 게임오브젝트 리스트를 포함하고 있기 때문에 저장안되는 문제가 있음을 알고
 * 내부적으로 ItemInfo 리스트로 만들어 보려고 시도하였음. 
 * 또 다른 문제점이 있는데 ItemInfo는 Image컴포넌트를 포함하고 있기 때문에 클래스 구조상 직렬화하기 힘들다고 생각됨.
 * 
 * <v2.0 - 2023_1119_최원준>
 * 1- Inventory 클래스의 직렬화 및 역직렬화 구현완료
 * 2- SerializableInventory 클래스를 Inventory 파일로 위치로 옮김.
 * 씬 전환시에도 사용하기 위해
 * 3- 플레이어 위치, 회전 정보를 담기 위한 STransform 클래스 정의
 * 
 * <v2.1 - 2023_1218_최원준>
 * 1- 금화 은화 변수 int형으로 변경
 * 
 */

namespace DataManagement
{


    /// <summary>
    /// 주의 사항 - 유니티 전용 클래스는 저장 불가. 기본 자료형으로 저장하거나, 구조체 또는 클래스를 만들어 저장한다. 
    /// </summary>
    [Serializable]           // 인스펙터에 아래의 클래스가 보여진다.
    public class GameData
    {
        /// <summary>
        /// 누적 플레이 타임
        /// </summary>
        public float playTime;

        /// <summary>
        /// 플레이어 위치, 회전 정보를 담는 변수입니다.<br/>
        /// 저장 할 Transform 컴포넌트를 전달해서 생성 해야되며, 불러 올때는 Deserialize 메서드에 연동할 Transform 인자를 전달하여 정보를 전달받습니다. 
        /// </summary>
        public STransform playerTr;


        
        /// <summary>
        /// 금화
        /// </summary>
        public int gold;

        /// <summary>
        /// 은화
        /// </summary>
        public int silver;

        /// <summary>
        /// 플레이어의 제작 가능 목록과 숙련도, 레시피 맞춘 횟수 정보가 담겨 있는 제작 관련 클래스 입니다.
        /// </summary>
        public Craftdic craftDic;

        
        private SerializableInventory savedInventory;

        /// <summary>
        /// 현재 플레이어가 보관하고 있는 인벤토리 정보입니다. <br/>
        /// Inventory 인스턴스를 전달하며 반합니다. (내부에 직렬화 가능한 클래스를 두고 자동 변환이 이루어집니다.)
        /// </summary>
        public Inventory inventory
        {
            set{ savedInventory = new SerializableInventory( value ); }
            get{ return new Inventory( savedInventory ); }
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
    public class STransform
    {
        public float x;
        public float y;
        public float z;
        public float xRot;
        public float yRot;
        public float zRot;

        /// <summary>
        /// 전달 받은 Transform 컴포넌트의 위치와 회전정보만 가지고 와서 STransform 인스턴스를 생성하여 반환합니다.
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
        /// 저장되어있는 STransform의 위치와 회전 정보를 전달한 Transform 컴포넌트 인자에 동기화 시켜줍니다.
        /// </summary>
        public void Deserialize(ref Transform tr)
        {
            tr.position = new Vector3( x, y, z );
            tr.rotation = Quaternion.Euler( xRot, yRot, zRot );
        }
    }
       




    


}