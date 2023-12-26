using System;
using UnityEngine;
using CraftData;
using System.Collections.Generic;
using Newtonsoft.Json;

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
 * 
 * <v3.1 - 2023_1222_최원준>
 * 1- private변수를 직렬화하기 위해 [JsonProperty] 어트리뷰트를 추가하였음
 * 2- STransform 클래스에 Seiralize메서드를 추가하여 일관성을 주었으며, 주석 보완
 * 
 * <v4.0 - 2023_1227_최원준>
 * 1- GameData클래스의 변수와 메서드들을 PlayerBasicData와 PlayerInvenData로 나눈 후, GameData를 인터페이스 처리하였음.
 * 이유는 세이브와 로드를 관리할 작업자나 사용되어지는 스크립트 파일 위치가 틀리기 때문
 * 
 * 2- SerializableInventory의 LoadInventory 및 SaveInventory메서드를 삭제하고
 * 클래스명을 SInventory로 변경 후 해당 클래스 내부에 Serialize 메서드와 Deserialize메서드를 호출하게 하였음.
 * 이유는 STransform과 메서드를 비슷하게 만들어 사용에 일관성을 주기 위함.
 * 
 */



namespace DataManagement
{    
    /// <summary>
    /// 데이터 세이브 및 로드를 위한 인터페이스입니다. <br/>
    /// 세이브할 데이터 클래스를 만든 후 이 인터페이스를 상속해서 Save와 Load를 해야 합니다. <br/>
    /// </summary>
    public interface GameData { }

    
    /// <summary>
    /// 주의 사항 - 유니티 전용 클래스는 저장 불가. 기본 자료형으로 저장하거나, 구조체 또는 클래스를 만들어 저장해야 합니다.
    /// </summary>
    public class PlayerBasicData : GameData
    {
        /// <summary>
        /// 누적 플레이 타임
        /// </summary>
        public float playTime;

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
        public PlayerBasicData()
        {            
            playTime = 0f;
            playerTr = new STransform();    // 새로운 직렬화 트랜스폼 생성
        }

    }



    /// <summary>
    /// 주의 사항 - 유니티 전용 클래스는 저장 불가. 기본 자료형으로 저장하거나, 구조체 또는 클래스를 만들어 저장해야 합니다.
    /// </summary>
    public class PlayerInvenData : GameData
    {
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

        /// <summary>
        /// 직렬화되어 저장 되어있는 플레이어의 인벤토리입니다.<br/>
        /// 저장시 Serialize메서드를 기존 Inventory클래스의 인스턴스를 인자로 전달하여 호출하고,<br/>
        /// 로드시 Deserialize메서드를 사용해서 기존 Inventory 클래스의 인스턴스를 반환받아 사용하세요.
        /// </summary>
        public SInventory savedInventory;

       

        /// <summary>
        /// DataManager에서 Load메서드에서 새로운 GameData를 생성하기 위한 생성자입니다.<br/>
        /// 기존의 데이터가 없을 경우 사용됩니다.
        /// </summary>
        public PlayerInvenData()
        {
            gold = 0;
            silver = 0;

            craftDic = new Craftdic();
            savedInventory = new SInventory();     // 새로운 직렬화 인벤토리 생성  
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