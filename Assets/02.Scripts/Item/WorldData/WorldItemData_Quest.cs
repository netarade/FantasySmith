using System.Collections.Generic;
using ItemData;

/*
 * 
* [작업 사항]
* 
* <v1.0 - 2024_0111_최원준>
* 1- 퀘스트 아이템 클래스추가로 새로운 파일 생성
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
                "키", new ItemQuest( ItemType.Quest, "0000", "키", new VisualReferenceIndex(0),
                "챕터1을 클리어하기 위한 퀘스트용 아이템." ) 
            },
        };        
    }

}
}

