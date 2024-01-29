using System.Collections.Generic;
using DataManagement;
using ItemData;

/*
 * 
* [�۾� ����]
* 
* <v1.0 - 2024_0111_�ֿ���>
* 1- ����Ʈ ������ Ŭ�����߰��� ���ο� ���� ����
* 
* <v1.1 - 2024_0130_�ֿ���>
* 1- ����Ʈ �������� ItemEquip�� ����ϸ鼭 LeatherHood�� STransform���� �������ڸ� �����ڿ� �Է�
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
                "LeatherHood", new ItemQuest( ItemType.Quest, "0000", "Leatherhood", new VisualReferenceIndex(0),
                "Raincoat made of leather.", EquipType.Helmet, STransform.GetSTransform(0) )
            },
            {
                "MysteriousStone", new ItemQuest( ItemType.Quest, "0002", "MysteriousStone", new VisualReferenceIndex(2),
                "A stone that doesn't fall to the ground." )
            },
            {
                "LeatherMap", new ItemQuest( ItemType.Quest, "0001", "LeatherMap", new VisualReferenceIndex(1),
                "Map made of leather." )
            },
            {
                "BrokenDevice", new ItemQuest( ItemType.Quest, "0004", "BrokenDevice", new VisualReferenceIndex(4),
                "A compass that keeps spinning like it's broken." )
            },
            {
                "MonsterEye", new ItemQuest( ItemType.Quest, "0003", "MonsterEye", new VisualReferenceIndex(3),
                "By-products obtained from hunting strange monsters" )
            },
            {
                "Glider", new ItemQuest( ItemType.Quest, "0005", "Glider", new VisualReferenceIndex(5),
                "I think I can escape from here if I use this." )
            },
        };        
    }

}
}

