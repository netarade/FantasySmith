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
    /// <summary>
    /// 모든 월드 아이템 목록을 보유하고 있는 딕셔너리 집합입니다. Monobehaviour를 상속하지 않으므로 생성해서 정보를 받아야 합니다.
    /// </summary>
    public partial class WorldItem
    {
        public readonly Dictionary<string, Item> miscDic = new Dictionary<string, Item>()
        {           //0x1000
            { "철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000000", "철", 3.0f, new ImageReferenceIndex(0) ) },
            { "강철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000001", "강철", 5.0f, new ImageReferenceIndex(1) ) },
            { "흑철", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000002", "흑철", 7.0f, new ImageReferenceIndex(2) ) },
            { "미스릴", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000003", "미스릴", 20.0f, new ImageReferenceIndex(3) ) }, 
            { "코발트", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000004", "코발트", 16.0f, new ImageReferenceIndex(4) ) },
            { "티타늄", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000005", "티타늄", 25.0f, new ImageReferenceIndex(5) ) },
            { "오리하르콘", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000006", "오리하르콘", 60.0f, new ImageReferenceIndex(6) ) },
            { "단단한 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000007", "단단한 나뭇가지", 2.0f, new ImageReferenceIndex(7) ) },
            { "튼튼한 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000008", "튼튼한 나뭇가지", 3.0f, new ImageReferenceIndex(8) ) },
            { "가벼운 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000009", "가벼운 나뭇가지", 4.0f, new ImageReferenceIndex(9) ) },
            { "부드러운 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000010", "부드러운 나뭇가지", 5.0f, new ImageReferenceIndex(10) ) },
            { "엘프의 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000011", "엘프의 나뭇가지", 20.0f, new ImageReferenceIndex(11) ) },
            { "축복받은 나뭇가지", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000012", "축복받은 나뭇가지", 40.0f, new ImageReferenceIndex(12) ) },

            { "하늘의 룬", new ItemMisc( ItemType.Misc, MiscType.Additive, "0000300", "하늘의 룬", 100.0f, new ImageReferenceIndex(0) ) },
            { "불타는 심장", new ItemMisc( ItemType.Misc, MiscType.Additive, "0000301", "불타는 심장", 100.0f, new ImageReferenceIndex(1) ) },
            { "영롱한 구슬", new ItemMisc( ItemType.Misc, MiscType.Additive, "0000302", "영롱한 구슬", 100.0f, new ImageReferenceIndex(2) ) },
            { "요정의 목화", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000303", "요정의 목화", 100.0f, new ImageReferenceIndex(3) ) }, 
            { "달의 조각", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000304", "달의 조각", 30.0f, new ImageReferenceIndex(4) ) },
            { "신비한 조각", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000305", "신비한 조각", 30.0f, new ImageReferenceIndex(5) ) },
            { "크롬 결정", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000306", "크롬 결정", 25.0f, new ImageReferenceIndex(6) ) },
            { "수은 결정", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000307", "수은 결정", 20.0f, new ImageReferenceIndex(7) ) },
            { "얼음 결정", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000308", "얼음 결정", 15.0f, new ImageReferenceIndex(8) ) },
            { "백금", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000309", "백금", 60.0f, new ImageReferenceIndex(9) ) },
            { "흑요석", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000310", "흑요석", 40.0f, new ImageReferenceIndex(10) ) },
            { "금", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000311", "금", 30.0f, new ImageReferenceIndex(11) ) },
            { "은", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000312", "은", 20.0f, new ImageReferenceIndex(12) ) },
            { "점토", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000313", "점토", 2.0f, new ImageReferenceIndex(13) ) },
            { "솜뭉치", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000314", "솜뭉치", 1.0f, new ImageReferenceIndex(14) ) },
            { "물소의 뿔", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000315", "물소의 뿔", 15.0f, new ImageReferenceIndex(15) ) },
            { "짐승 가죽", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000316", "짐승 가죽", 6.0f, new ImageReferenceIndex(16) ) },
            
            { "초급 물리의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000700", "초급 물리의 각인", 50.0f, new ImageReferenceIndex(0) ) },
            { "초급 공속의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000701", "초급 공속의 각인", 50.0f, new ImageReferenceIndex(1) ) },
            { "초급 흡혈의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000702", "초급 흡혈의 각인", 50.0f, new ImageReferenceIndex(2) ) },
            { "초급 사격의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000703", "초급 사격의 각인", 50.0f, new ImageReferenceIndex(3) ) },
            { "초급 피해의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000704", "초급 피해의 각인", 50.0f, new ImageReferenceIndex(4) ) },
            { "중급 물리의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000705", "중급 물리의 각인", 125.0f, new ImageReferenceIndex(0) ) },
            { "중급 공속의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000706", "중급 공속의 각인", 125.0f, new ImageReferenceIndex(1) ) },
            { "중급 흡혈의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000707", "중급 흡혈의 각인", 125.0f, new ImageReferenceIndex(2) ) },
            { "중급 사격의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000708", "중급 사격의 각인", 125.0f, new ImageReferenceIndex(3) ) },
            { "중급 피해의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000709", "중급 피해의 각인", 125.0f, new ImageReferenceIndex(4) ) },
            { "고급 물리의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000710", "고급 물리의 각인", 200.0f, new ImageReferenceIndex(0) ) },
            { "고급 공속의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000711", "고급 공속의 각인", 200.0f, new ImageReferenceIndex(1) ) },
            { "고급 흡혈의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000712", "고급 흡혈의 각인", 200.0f, new ImageReferenceIndex(2) ) },
            { "고급 사격의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000713", "고급 사격의 각인", 200.0f, new ImageReferenceIndex(3) ) },
            { "고급 피해의 각인", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000714", "고급 피해의 각인", 200.0f, new ImageReferenceIndex(4) ) },
            
            { "나무", new ItemMisc( ItemType.Misc, MiscType.Fire, "0000600", "나무", 1.0f, new ImageReferenceIndex(5) ) },
            { "석탄", new ItemMisc( ItemType.Misc, MiscType.Fire, "0000601", "석탄", 6.0f, new ImageReferenceIndex(6) ) },
            { "석유", new ItemMisc( ItemType.Misc, MiscType.Fire, "0000602", "석유", 15.0f, new ImageReferenceIndex(7) ) },
            { "강화석", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000610", "강화석", 50.0f, new ImageReferenceIndex(9) ) }, 
            
            { "속성석-수", new ItemMisc( ItemType.Misc, MiscType.Attribute,"0000611", "속성석-수", 500.0f, new ImageReferenceIndex(8) ) }, 
            { "속성석-금", new ItemMisc( ItemType.Misc, MiscType.Attribute,"0000612", "속성석-금", 500.0f, new ImageReferenceIndex(8) ) }, 
            { "속성석-지", new ItemMisc( ItemType.Misc, MiscType.Attribute,"0000613", "속성석-지", 500.0f, new ImageReferenceIndex(8) ) }, 
            { "속성석-화", new ItemMisc( ItemType.Misc, MiscType.Attribute,"0000614", "속성석-화", 500.0f, new ImageReferenceIndex(8) ) }, 
            { "속성석-풍", new ItemMisc( ItemType.Misc, MiscType.Attribute,"0000615", "속성석-풍", 500.0f, new ImageReferenceIndex(8) ) }, 
        };        
    }
}
