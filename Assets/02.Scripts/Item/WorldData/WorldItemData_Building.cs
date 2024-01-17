using System.Collections.Generic;
using ItemData;

/*
 * 
* [작업 사항]
* 
* <v1.0 - 2024_0117_최원준>
* 1- 분할클래스를 통해 빌딩 아이템 데이터가 담긴 딕셔너리를 반환하는 메서드 작성
* 
* 
* 
* 
*/



namespace WorldItemData
{
public partial class WorldItem
{
    private Dictionary<string, Item> InitDic_Building()
    {         
        return new Dictionary<string, Item>()
        {           
            { 
                "벽", new ItemBuilding( ItemType.Misc, "5000", "벽", new VisualReferenceIndex(0),
                MiscType.Building, 10, "~~하는용도로 사용")    // 세부타입, 내구도, 아이템설명
            },
            { 
                "점토", new ItemBuilding( ItemType.Misc, "5001", "점토", new VisualReferenceIndex(1),
                MiscType.Building, 15,"~~하는용도로 사용") 
            },
            { 
                "흙", new ItemBuilding( ItemType.Misc, "5002", "흙", new VisualReferenceIndex(2),
                MiscType.Building, 15,"~~하는용도로 사용") 
            },
        };




    }

}
}
