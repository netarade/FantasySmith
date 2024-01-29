using DataManagement;
using Newtonsoft.Json;
using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.Rendering;
using UnityEngine;


/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1105_최원준>
 * 1- 신규 클래스 작성 (ItemQuest, ItemBuilding 추가)
 * 
 * <v1.1 - 2024_1112_최원준>
 * 1- ItemEquip 신규 클래스 작성
 * 이유는 방어구 등 착용가능한 아이템 클래스를 공통만 따로 선별하기 위함
 * 
 * 
 * <v1.2 - 2024_0117_최원준>
 * 1- ItemBuilding의 hp를 public변수로 변경(수정가능해야 하므로), 변수명 Hp로 변경 
 * 
 * <v1.3 -2024_0118_최원준>
 * 1- ItemBuilding의 Hp를 Durability로 변경하였음.
 * 이유는 ItemInfo클래스에서 무기와 동일하게 Durability로 접근할 수 있게 하기 위함.
 * 
 * 2- ItemFood를 정의하고 스테이터스 수치를 증감시키는 ItemStatus 구조체를 갖도록 하였음.
 * 
 * <v2.0 - 2024_0124_최원준>
 * 1- ItemBuilding에 STransform 변수인 WorldTr을 추가
 * 이유는 아이템이 월드상태에서 인벤토리에 저장되는 아이템인 경우 저장할 수 있는 Transform정보를 가진채로 저장되어져야하기 때문
 * 
 * 2- isDecoration을 삭제하고 enum변수인 BuildingType buldingType을 추가하였음.
 * 이유는 장식용 속성 뿐만아니라 설치형 인벤토리인지 여부도 구분해야 하는데 이는 속성보다는 세부타입을 주어 구분하는것이 효율적이기 때문
 * 
 * 
 * 3- public 변수에 JsonPropert 애트리뷰트 삭제
 * 
 * 
 * <v2.1 - 2024_0127_최원준>
 * 1- ItemBuilding에 Clone메서드를 오버라이딩해서 구현하였음.
 * 이유는 Item 내부에 Stransform을 저장하고 있는 경우 참조값을 저장하기 때문에, 아이템이 Clone되어도 참조값을 공유하게 되어있음.
 * 따라서 새로운 STransform의 Clone도 같이 교체해서 반환해줘야함.
 *
 * <v2.2 - 2024_0128_최원준>
 * 1 - ItemStatus 내부에 speed 속성을 추가, 생성자 선택인자 옵션을 제거
 * 
 * <v2.3 - 2024_0130_최원준>
 * 1- STransform 관련 변수를 ItemWeapon에서 ItemEquip클래스로 이전
 * 
 * 2- 퀘스트 아이템도 ItemEquip을 상속하도록 변경
 * 
 */





namespace ItemData
{
    /// <summary>
    /// 장착 타입 - 무기, 헬멧, 상의, 하의, 장갑, 없음
    /// </summary>
    public enum EquipType { Weapon, Helmet, Armor, Pants, Gloves, None }

    /// <summary>
    /// 장비 아이템 클래스 - 무기 방어구등 착용가능한 모든 클래스의 추상적 부모로서 상속만 가능합니다
    /// </summary>
    public abstract class ItemEquip : Item
    {
        
        /// <summary>
        /// 장착 지점에 관한 위치 정보
        /// </summary>
        public STransform EquipTr;       
                
        /// <summary>
        /// 현재 장착 여부
        /// </summary>
        public bool isEquip;
        
        /// <summary>
        /// 아이템이 장착 중인 슬롯위치
        /// </summary>
        public int EquipSlotIndex;

        /// <summary>
        /// 아이템의 장착 타입 - 무기, 헬멧, 상의, 하의, 장갑, 없음
        /// </summary>
        public EquipType EquipType;


        public ItemEquip( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, string desc,
            EquipType equipType, STransform equipTr )
            : base( mainType, No, name, visualRefIndex, desc ) 
        { 
            this.EquipTr = equipTr;
            this.EquipType = equipType;
            isEquip = false;
            EquipSlotIndex = -1;
        }

    }


    
    /// <summary>
    /// 퀘스트 아이템 클래스 - 별도의 탭을 사용하기 때문에 ItemType의 대분류 기준에 속하며, Item을 상속합니다.<br/>
    /// (특징 - 수량 표시하지 않음, 아이템 셀렉트 및 드랍이 불가능, 전체 탭에 표시되지 않음 )
    /// </summary>
    [Serializable]
    public class ItemQuest : ItemEquip
    {
        public ItemQuest( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, string desc,
            EquipType equipType=EquipType.None, STransform equipTr=null )
            : base( mainType, No, name, visualRefIndex, desc, equipType, equipTr )
        { 

        }
    }




    /// <summary>
    /// 빌딩 아이템의 세부종류로서 현재 재료아이템, 장식용아이템, 인벤토리, 없음 등의 종류가 있습니다.
    /// </summary>
    public enum BuildingType { Basic, Inventory, None }

    /// <summary>
    /// 건설아이템 - Item을 상속하며 오브젝트마다 고유의 내구도가 있습니다.<br/>
    /// 장식용과 재료용으로 구분됩니다.<br/>
    /// 재료용은 일반 잡화아이템과 동일하게 인벤토리를 옮겨다닐 수 있지만,<br/>
    /// 장식용은 외부상태로 항상 존재하며 저장되고 불러와져야 합니다. (아이템화 즉,인벤토리 내부로 2D 상태가 될 수 없습니다.)
    /// </summary>
    [Serializable]
    public class ItemBuilding : Item
    {
        public int Durability;              // 건설아이템의 내구도
        public BuildingType buildingType;   // 세부 종류 (재료, 장식용, 인벤토리)
        public STransform WorldTr;          // 아이템이 월드에 놓여지는 변환정보


        public ItemBuilding( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex
            , BuildingType buildingType, int durability, string desc )
            : base(mainType, No, name, visualRefIndex, desc)
        {
            this.buildingType = buildingType;
            Durability = durability;
            WorldTr = new STransform();
        }


        /// <summary>
        /// 건설 아이템의 클론 생성 시 호출되는 메서드입니다.<br/>
        /// 멤버의 값을 복사해서 반환하며, 내부 클래스 참조값 또한 그대로 반환하지 않고 새로운 인스턴스를 만들어 참조값을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            ItemBuilding itemBuilding = this.MemberwiseClone() as ItemBuilding;
            itemBuilding.WorldTr = (STransform) WorldTr.Clone();    // 값을 복사해서 인스턴스를 만들고 새로운 참조값을 저장합니다.
            return itemBuilding;
        }

    }







    [Serializable]
    public class ItemFood : ItemMisc
    {
        public ItemStatus Status;

        public ItemFood( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex
            , MiscType subType, ItemStatus status, string desc )
            : base(mainType, No, name, visualRefIndex, subType, desc)
        {
            Status = status;
        }
    }



    /// <summary>
    /// 아이템이 보유하고 있는 스테이터스 수치를 나타내는 구조체입니다.<br/>
    /// 체력, 허기, 갈증, 체온 4가지 종류가 있습니다.<br/>
    /// 음의 값을 가지면 해당 수치만큼 플레이어의 스테이터스를 감소시키며, 양의 값을 가지면 증가시킵니다.
    /// </summary>
    [Serializable]
    public struct ItemStatus
    {
        /// <summary>
        /// 체력
        /// </summary>
        public float hp;            
        
        /// <summary>
        /// 허기
        /// </summary>
        public float hunger;        

        /// <summary>
        /// 갈증
        /// </summary>
        public float thirsty;       

        /// <summary>
        /// 체온
        /// </summary>
        public float temparature;   
        
        /// <summary>
        /// 이동 속도
        /// </summary>
        public float speed;


        /// <summary>
        /// 체력, 허기, 갈증, 체온, 스피드
        /// </summary>
        public ItemStatus(float hp, float hunger, float thirsty, float temparature=0f, float speed=0f)
        {
            this.hp = hp;
            this.hunger = hunger;
            this.thirsty = thirsty;
            this.temparature = temparature;
            this.speed = speed;
        }
    }







}
