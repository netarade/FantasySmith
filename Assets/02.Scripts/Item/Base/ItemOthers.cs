using Newtonsoft.Json;
using System;


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
 */





namespace ItemData
{
    /// <summary>
    /// 퀘스트 아이템 클래스 - 별도의 탭을 사용하기 때문에 ItemType의 대분류 기준에 속하며, Item을 상속합니다.<br/>
    /// (특징 - 수량 표시하지 않음, 아이템 셀렉트 및 드랍이 불가능, 전체 탭에 표시되지 않음 )
    /// </summary>
    [Serializable]
    public class ItemQuest : Item
    {
        public ItemQuest( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, string desc )
            : base( mainType, No, name, visualRefIndex, desc ) 
        { 
            
        }
    }

    /// <summary>
    /// 장비 아이템 클래스 - 무기 방어구등 착용가능한 모든 클래스의 추상적 부모로서 상속만 가능합니다
    /// </summary>
    public abstract class ItemEquip : Item
    {
        public ItemEquip( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, string desc )
            : base( mainType, No, name, visualRefIndex, desc ) 
        { 
            
        }

    }


    /// <summary>
    /// 건설아이템 - ItemMisc을 상속하며 오브젝트마다 고유의 내구도가 있습니다.<br/>
    /// 장식용과 재료용으로 구분됩니다.<br/>
    /// 재료용은 일반 잡화아이템과 동일하게 인벤토리를 옮겨다닐 수 있지만,<br/>
    /// 장식용은 외부상태로 항상 존재하며 저장되고 불러와져야 합니다. (아이템화 즉,인벤토리 내부로 2D 상태가 될 수 없습니다.)
    /// </summary>
    [Serializable]
    public class ItemBuilding : ItemMisc
    {
        [JsonProperty] public bool isDecoration;    // 장식용 속성 (재료인지, 장식용인지 여부)
        [JsonProperty] public int Durability;       // 건설아이템의 내구도

        public ItemBuilding( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex
            , MiscType subType, bool isDecoration, int Durability, string desc )
            : base(mainType, No, name, visualRefIndex, subType, desc)
        {
            this.isDecoration = isDecoration;
            this.Durability = Durability;
        }
    }





    [Serializable]
    public class ItemFood : ItemMisc
    {
        public ItemStatus status;

        public ItemFood( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex
            , MiscType subType, ItemStatus status, string desc )
            : base(mainType, No, name, visualRefIndex, subType, desc)
        {
            this.status = status;
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
        /// 체력, 허기, 갈증, 체온
        /// </summary>
        public ItemStatus(float hp, float hunger, float thirsty, float temparature=0f)
        {
            this.hp = hp;
            this.hunger = hunger;
            this.thirsty = thirsty;
            this.temparature = temparature;
        }
    }







}
