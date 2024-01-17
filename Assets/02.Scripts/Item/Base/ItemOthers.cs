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
    /// 건설재료 - ItemMisc을 상속하며 오브젝트마다 고유의 내구도가 있습니다.
    /// </summary>
    [Serializable]
    public class ItemBuilding : ItemMisc
    {
        [JsonProperty] public int Durability;   // 건설재료의 내구도


        public ItemBuilding( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex
            , MiscType subType, int Durability, string desc )
            : base(mainType, No, name, visualRefIndex, subType, desc)
        {
            this.Durability = Durability;
        }

    }

}
