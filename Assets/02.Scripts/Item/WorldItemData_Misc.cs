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
            { "철", new ItemMisc( ItemType.Misc, "0000", "철", new VisualReferenceIndex(0),
               MiscType.Basic, "~~하는용도") 
            },
        };        
    }

    private Dictionary<string, Item> InitDic_Quest()
    {        
        return new Dictionary<string, Item>()
        {           
            { "철", new ItemMisc( ItemType.Misc, "0000", "철", new VisualReferenceIndex(0),
               MiscType.Basic, "~~하는용도") 
            },
        };        
    }

}
}
