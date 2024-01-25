using System.Collections.Generic;
using ItemData;

/*
 * 
* [작업 사항]
* 
* <v1.0 - 2024_0117_최원준>
* 1- 분할클래스를 통해 빌딩 아이템 데이터가 담긴 딕셔너리를 반환하는 메서드 작성
* 
* <v1.1 - 2024_0125_최원준>
* 1- ItemBuilding클래스의 속성 수정으로 인하여 생성자 세부사항을 변경
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
                "벽", new ItemBuilding( ItemType.Building, "5000", "벽", new VisualReferenceIndex(0),
                BuildingType.Basic, 10, "~~하는용도로 사용")    
            },
            { 
                "울타리", new ItemBuilding( ItemType.Building, "5001", "울타리", new VisualReferenceIndex(1),
                BuildingType.Basic, 15, "~~하는용도로 사용") 
            },
            { 
                "인벤토리", new ItemBuilding( ItemType.Building, "5002", "인벤토리", new VisualReferenceIndex(2),
                BuildingType.Inventory ,15, "~~하는용도로 사용") 
            },
        };
    }





    private Dictionary<string, Item> InitDic_BuildingMisc()
    {         
        return new Dictionary<string, Item>()
        {           
            //"키 접근 값", (대분류, "넘버링", "아이템 이름", 인스펙터뷰에서 참조번호, 중분류, "아이템 설명")
            { 
                "흙", new ItemMisc( ItemType.Misc, "5000", "흙", new VisualReferenceIndex(0),
                MiscType.Building, "~~하는용도로 사용")    
            },
            { 
                "나무", new ItemMisc( ItemType.Misc, "5000", "나무", new VisualReferenceIndex(1),
                MiscType.Building, "~~하는용도로 사용")    
            },


        };
    }


}
}
