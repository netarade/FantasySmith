using System.Collections.Generic;
using DataManagement;
using ItemData;

/*
 * 
* [작업 사항]
* 
* <v1.0 - 2024_0111_최원준>
* 1- 퀘스트 아이템 클래스추가로 새로운 파일 생성
* 
* <v1.1 - 2024_0130_최원준>
* 1- 퀘스트 아이템이 ItemEquip을 상속하면서 LeatherHood가 STransform관련 선택인자를 생성자에 입력
* 
*/

namespace WorldItemData
{
public partial class WorldItem
{
    private Dictionary<string, Item> InitDic_Quest()
    {        
        return new Dictionary<string, Item>()
        {            
            {
                "LeatherHood", new ItemQuest( ItemType.Quest, "0000", "Leatherhood", new VisualReferenceIndex(0),
                "Raincoat made of leather.", EquipType.Helmet, STransform.GetSTransform(0) )
            },
            {
                "MysteriousStone", new ItemQuest( ItemType.Quest, "0002", "MysteriousStone", new VisualReferenceIndex(2),
                "A stone that doesn't fall to the ground." )
            },
            {
                "LeatherMap", new ItemQuest( ItemType.Quest, "0001", "LeatherMap", new VisualReferenceIndex(1),
                "Map made of leather." )
            },
            {
                "BrokenDevice", new ItemQuest( ItemType.Quest, "0004", "BrokenDevice", new VisualReferenceIndex(4),
                "A compass that keeps spinning like it's broken." )
            },
            {
                "MonsterEye", new ItemQuest( ItemType.Quest, "0003", "MonsterEye", new VisualReferenceIndex(3),
                "By-products obtained from hunting strange monsters" )
            },
            {
                "Glider", new ItemQuest( ItemType.Quest, "0005", "Glider", new VisualReferenceIndex(5),
                "I think I can escape from here if I use this." )
            },
        };        
    }

}
}

