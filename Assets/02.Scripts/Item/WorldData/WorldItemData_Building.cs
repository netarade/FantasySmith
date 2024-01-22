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
            //"키 접근 값", (대분류, "넘버링", "아이템 이름", 인스펙터뷰에서 참조번호, 중분류, 외부 장식용, 내구도, "아이템 설명")
            { 
                "벽", new ItemBuilding( ItemType.Misc, "5000", "벽", new VisualReferenceIndex(0),
                MiscType.Building, true, 10, "~~하는용도로 사용")    
            },
            { 
                "점토", new ItemBuilding( ItemType.Misc, "5001", "점토", new VisualReferenceIndex(1),
                MiscType.Building, false, 15,"~~하는용도로 사용") 
            },
            { 
                "흙", new ItemBuilding( ItemType.Misc, "5002", "흙", new VisualReferenceIndex(2),
                MiscType.Building, false ,15,"~~하는용도로 사용") 
            },
        };




    }

}
}
