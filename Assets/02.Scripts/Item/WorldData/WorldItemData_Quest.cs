using System.Collections.Generic;
using ItemData;

/*
 * 
* [�۾� ����]
* 
* <v1.0 - 2024_0111_�ֿ���>
* 1- ����Ʈ ������ Ŭ�����߰��� ���ο� ���� ����
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
                "Ű", new ItemQuest( ItemType.Quest, "0000", "Ű", new VisualReferenceIndex(0),
                "é��1�� Ŭ�����ϱ� ���� ����Ʈ�� ������." ) 
            },
        };        
    }

}
}

