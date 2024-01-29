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
            {
                "Mud", new ItemMisc( ItemType.Misc, "0000", "Mud", new VisualReferenceIndex(0),
                MiscType.Basic, "Soil mixed with water.")
            },
            {
                "Thread", new ItemMisc( ItemType.Misc, "0001", "Thread", new VisualReferenceIndex(1),
                MiscType.Basic, "Materials can be bundled")
            },
            {
                "Vine", new ItemMisc( ItemType.Misc, "0002", "Vine", new VisualReferenceIndex(2),
                MiscType.Basic, "A primitive material that can be used to weave materials.")
            },
            {
                "Stone", new ItemMisc( ItemType.Misc, "0003", "Stone", new VisualReferenceIndex(3),
                MiscType.Basic, "The most primitive material you can pick up from the ground.")
            },
            {
                "Log", new ItemMisc( ItemType.Misc, "0004", "Log", new VisualReferenceIndex(4),
                MiscType.Basic, "Tree trimmed for use.")
            },
            {
                "Bone", new ItemMisc( ItemType.Misc, "0005", "Bone", new VisualReferenceIndex(5),
                MiscType.Basic, "The animal bones here seem much harder than stones.")
            },
            {
                "Leather", new ItemMisc( ItemType.Misc, "0006", "Leather", new VisualReferenceIndex(6),
                MiscType.Basic, "I think this can prevent dangerous rain.")
            },            
            {
                "Rope", new ItemMisc( ItemType.Misc, "0007", "Rope", new VisualReferenceIndex(7),
                MiscType.Basic, "It is a product made by weaving several threads.")
            },
        };




    }


}
}
