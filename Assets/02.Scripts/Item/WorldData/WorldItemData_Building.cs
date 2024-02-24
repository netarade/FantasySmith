using System.Collections.Generic;
using ItemData;

/*
 * 
* [작업 사항]
* 
* <v1.0 - 2024_0117_최원준>
* 1- 분할클래스를 통해 빌딩 아이템 데이터가 담긴 딕셔너리를 반환하는 메서드 작성
* 
* <v2.0 - 2024_0126_최원준>
*  1- 건설아이템의 잡화와 일반 종류를 구분
* 
* 
*/



namespace WorldItemData
{
    public partial class WorldItem
    {

        // 건설 재료아이템 (인벤토리 내부적으로 표현가능한 아이템)
        private Dictionary<string, Item> InitDic_BuildingMisc()
        {         
            return new Dictionary<string, Item>()
            {           
                //"키 접근 값", (대분류, "넘버링", "아이템 이름", 인스펙터뷰에서 참조번호, 중분류, "아이템 설명")
                { 
                    "흙", new ItemMisc( ItemType.Misc, "5000", "흙", new VisualReferenceIndex(0),
                    MiscType.Building, "~~하는용도로 사용")    
                },

            };
        }


        // 건설 외부아이템 (월드에서만 표현가능)
        private Dictionary<string, Item> InitDic_Building()
        {
            return new Dictionary<string, Item>()
        {           
            //"키 접근 값", (대분류, "넘버링", "아이템 이름", 인스펙터뷰에서 참조번호, 중분류, 외부 장식용, 내구도, "아이템 설명")
            {
                "나무벽", new ItemBuilding( ItemType.Building, "5000", "나무벽", new VisualReferenceIndex(0),
                BuildingType.Basic, 300, "적을 막아주는 오브젝트")
            },
            {
                "나무바닥", new ItemBuilding( ItemType.Building, "5001", "나무바닥", new VisualReferenceIndex(1),
                BuildingType.Basic, 300,"건물의 바닥을 채우는 오브젝트")
            },
            {
                "문", new ItemBuilding( ItemType.Building, "5002", "문", new VisualReferenceIndex(2),
                BuildingType.Basic, 300,"건물 출입을 담당하는 오브젝트")
            },
            {
                "계단", new ItemBuilding( ItemType.Building, "5003", "계단", new VisualReferenceIndex(3),
                BuildingType.Basic, 300,"층계를 오르거나 내려갈 때 필요한 오브젝트")
            },
            {
                "수평틀", new ItemBuilding( ItemType.Building, "5004", "수평틀", new VisualReferenceIndex(4),
                BuildingType.Basic, 300,"건설할 틀을 제공하는 오브젝트")
            },
            {
                "수직틀", new ItemBuilding( ItemType.Building, "5005", "수직틀", new VisualReferenceIndex(5),
                BuildingType.Basic, 300,"건설할 수직 틀을 제공하는 오브젝트")
            },
            {
                "횃불", new ItemBuilding( ItemType.Building, "5006", "횃불", new VisualReferenceIndex(6),
                BuildingType.Basic, 300,"주변을 밝히는 오브젝트")
            },
            {
                "벽걸이횃불", new ItemBuilding( ItemType.Building, "5007", "벽걸이횃불", new VisualReferenceIndex(7),
                BuildingType.Basic, 300,"벽에 붙는 주변을 밝히는 오브젝트")
            },
            {
                "울타리", new ItemBuilding( ItemType.Building, "5008", "울타리", new VisualReferenceIndex(8),
                BuildingType.Basic, 300,"울타리 오브젝트")
            },
            {
                "모닥불", new ItemBuilding( ItemType.Building, "5009", "모닥불", new VisualReferenceIndex(9),
                BuildingType.Basic, 300,"기타 아이템을 굽거나 주변을 밝히는 오브젝트")
            },
            {
                "제작대", new ItemBuilding( ItemType.Building, "5010", "제작대", new VisualReferenceIndex(10),
                BuildingType.Basic, 300,"아이템 제작 오브젝트")
            },
            {
                "보관함", new ItemBuilding( ItemType.Building, "5011", "보관함", new VisualReferenceIndex(11),
                BuildingType.Inventory, 300,"아이템 저장 오브젝트")
            },
            {
                "침대", new ItemBuilding( ItemType.Building, "5012", "침대", new VisualReferenceIndex(12),
                BuildingType.Basic, 300,"실내 수면 가능한 오브젝트")
            },
            {
                "침낭", new ItemBuilding( ItemType.Building, "5013", "침낭", new VisualReferenceIndex(13),
                BuildingType.Basic, 300,"실외 수면 가능한 오브젝트")
            },
            {
                "화로", new ItemBuilding( ItemType.Building, "5014", "화로", new VisualReferenceIndex(14),
                BuildingType.Basic, 300,"아이템을 굽는 오브젝트")
            },
            {
                "정수기", new ItemBuilding( ItemType.Building, "5015", "정수기", new VisualReferenceIndex(15),
                BuildingType.Basic, 300,"정수 오브젝트")
            },
        };
        }

    }
}
