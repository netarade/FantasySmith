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
 */





namespace ItemData
{
    /// <summary>
    /// 퀘스트 아이템 클래스 - 별도의 탭을 사용하기 때문에 Item을 상속합니다.<br/>
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
    /// 장비 아이템 클래스 - 무기 방어구등 착용가능한 모든 클래스의 추상적 부모 (상속만 가능합니다)
    /// </summary>
    public abstract class ItemEquip : Item
    {
        public ItemEquip( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, string desc )
            : base( mainType, No, name, visualRefIndex, desc ) 
        { 
            
        }

    }


    /// <summary>
    /// 빌딩 아이템 - ItemMisc을 상속<br/>
    /// ( 특징 - 오브젝트마다 고유 체력이 있음 )
    /// </summary>
    [Serializable]
    public class ItemBuilding : ItemMisc
    {
        [JsonProperty] int hp;


        public ItemBuilding( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, MiscType subType, int hp, string desc )
            : base(mainType, No, name, visualRefIndex, subType, desc)
        {
            this.hp = hp;
        }

    }

}
