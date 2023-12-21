using System;
using UnityEngine;
using CraftData;
using System.Collections.Generic;

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
 * <v3.0 - 2023_1222_최원준>
 * 1- DataManager클래스의 Load메서드에서 GameData 기본생성자를 호출하는데 정의가 되어있지 않아 새롭게 정의 (STransform 기본 생성자 포함)
 * 
 * 2- saveInventory변수를 private에서 public으로 변경하였음.
 * public아닌 변수는 Json에서 직렬화가 불가능한 변수가 되기 때문
 * 
 * 3- 인벤토리 클래스를 프로퍼티로 직렬화 인벤토리 클래스로 변환하여 저장하던 것을 메서드를 통해 변환하여 저장하도록 수정함.
 * (Inventory프로퍼티 삭제, SaveInventory, LoadInventory메서드를 추가)
 * =>이유는 프로퍼티는 set을통해 직렬화가능한 변수에 저장한다고 하더라도 프로퍼티 자체에 받자마자 저장이 되기 때문에 
 * GameData에 해당 프로퍼티가 포함되어 있으면 직렬화 처리가 불가능한 것을 발견하였음. 
 * (Inventory 클래스는 List<GameObject>를 포함하기 때문에 프로퍼티로 GameData에 담기는 순간 직렬화처리 불가능해 저장이 안되었음.)
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
        /// DataManager에서 Load메서드에서 새로운 GameData를 생성하기 위한 생성자입니다.<br/>
        /// 기존의 데이터가 없을 경우 사용됩니다.
        /// </summary>
        public GameData()
        {
            playTime = 0f;
            playerTr = new STransform();

            gold = 0;
            silver = 0;

            craftDic = new Craftdic();
            SaveInventory(new Inventory());     // 새로운 직렬화 인벤토리 생성  
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
        public void Deserialize( ref Transform tr )
        {
            tr.position=new Vector3( x, y, z );
            tr.rotation=Quaternion.Euler( xRot, yRot, zRot );
        }
    }
       




    


}