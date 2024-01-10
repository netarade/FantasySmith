using System.Collections.Generic;
using ItemData;

/*
 * 
* [�۾� ����]
* 
* <v1.0 - 2023_1221_�ֿ���>
* 1- CreateManager�� CreateAllItemDictionary�޼����� ������ �и����Ѽ� Ŭ���� ����
* 
* <V1.1 - 2023_1230_�ֿ���>
* 1- ��ųʸ��� readonly�Ӽ� �߰��Ͽ� ������ �Ӽ��� �⺻�� ������ ����
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
            { "ö", new ItemMisc( ItemType.Misc, "0000", "ö", new VisualReferenceIndex(0),
               MiscType.Basic, "~~�ϴ¿뵵") 
            },
        };        
    }

    private Dictionary<string, Item> InitDic_Quest()
    {        
        return new Dictionary<string, Item>()
        {           
            { "ö", new ItemMisc( ItemType.Misc, "0000", "ö", new VisualReferenceIndex(0),
               MiscType.Basic, "~~�ϴ¿뵵") 
            },
        };        
    }

}
}
