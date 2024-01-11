using Newtonsoft.Json;
using System;


/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1105_최원준>
 * 1- 신규 클래스 작성
 * 
 */





namespace ItemData
{
    /// <summary>
    /// 퀘스트 아이템 - Item을 상속합니다.<br/>
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
