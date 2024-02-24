using System.Collections.Generic;
using CreateManagement;
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
* 2- GetSTransform호출 인자로 IVCType을 추가
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
                "가죽 후드", new ItemQuest( ItemType.Quest, "0000", "가죽 후드", new VisualReferenceIndex(0),
                "가죽으로 만든 비옷입니다. 이래보여도 성능이 매우 뛰어납니다.", EquipType.Helmet, STransform.GetSTransform(IVCType.Quest, 0) )
            },
            {
                "신기한 돌", new ItemQuest( ItemType.Quest, "0001", "신기한 돌", new VisualReferenceIndex(1),
                "땅에 떨어지지 않고 둥둥 떠 있는 돌입니다." )
            },
            {
                "가죽 지도", new ItemQuest( ItemType.Quest, "0002", "가죽 지도", new VisualReferenceIndex(2),
                "위치를 기록하기 위해 가죽으로 지도를 만들었습니다." )
            },
            {
                "부서진 장치", new ItemQuest( ItemType.Quest, "0003", "부서진 장치", new VisualReferenceIndex(3),
                "탐사선이 추락할 때 떨어져 나간 통신장치 입니다. 지금은 고장나 작동하지 않습니다." )
            },
            {
                "괴물의 눈", new ItemQuest( ItemType.Quest, "0004", "괴물의 눈", new VisualReferenceIndex(4),
                "길을 막고 있는 괴물을 처치하고 얻은 부산물. 이것을 얻는 순간 안개가 사라졌습니다." )
            },
            {
                "글라이더", new ItemQuest( ItemType.Quest, "0005", "글라이더", new VisualReferenceIndex(5),
                "이걸 쓰면 안전하게 이곳에서 탈출할 수 있을 것 같습니다. 제발." )
            },
        };        
    }

}
}

