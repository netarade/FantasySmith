using System.Collections.Generic;
using ItemData;

/*
 * 
* [작업 사항]
* 
* <v1.0 - 2023_1221_최원준>
* 1- CreateManager의 CreateAllItemDictionary메서드의 정보를 분리시켜서 클래스 생성
* 
* <V1.1 - 2023_1230_최원준>
* 1- 딕셔너리에 readonly속성 추가하여 아이템 속성의 기본값 수정을 방지
* 
*/

namespace WorldItemData
{
public partial class WorldItem
{
    private Dictionary<string, Item> InitDic_Misc()
    {        
        return new Dictionary<string, Item>()
        {           
            {
                "진흙", new ItemMisc( ItemType.Misc, "0000", "진흙", new VisualReferenceIndex(0),
                MiscType.Basic, "물과 흙을 섞은 것으로 건축자재로 사용할 수 있을 것 같습니다.")
            },
            {
                "실", new ItemMisc( ItemType.Misc, "0001", "실", new VisualReferenceIndex(1),
                MiscType.Basic, "이것으로 무엇인가 묶을 수 있어 보입니다.")
            },
            {
                "덩쿨", new ItemMisc( ItemType.Misc, "0002", "덩쿨", new VisualReferenceIndex(2),
                MiscType.Basic, "무언가를 묶을 수 있는 가장 원시적인 재료.")
            },
            {
                "돌", new ItemMisc( ItemType.Misc, "0003", "돌", new VisualReferenceIndex(3),
                MiscType.Basic, "바닥에 아무렇게나 널부러져 있는 흔한 돌입니다.")
            },
            {
                "통나무", new ItemMisc( ItemType.Misc, "0004", "통나무", new VisualReferenceIndex(4),
                MiscType.Basic, "이것으로 무엇이든 만들 수 있을 것 같습니다.")
            },
            {
                "뼈", new ItemMisc( ItemType.Misc, "0005", "뼈", new VisualReferenceIndex(5),
                MiscType.Basic, "이곳의 동물뼈 강도는 금속에도 버금가리라 생각됩니다.")
            },
            {
                "가죽", new ItemMisc( ItemType.Misc, "0006", "가죽", new VisualReferenceIndex(6),
                MiscType.Basic, "가죽에는 이곳에서 살아갈 수 있는 많은 비밀이 숨겨져 있을 겁니다.")
            },            
            {
                "끈", new ItemMisc( ItemType.Misc, "0007", "끈", new VisualReferenceIndex(7),
                MiscType.Basic, "여러가닥의 실을 엮어 만든 질긴 끈입니다.")
            },
        };




    }


}
}
