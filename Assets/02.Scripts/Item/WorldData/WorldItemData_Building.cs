using System.Collections.Generic;
using ItemData;

/*
 * 
* [�۾� ����]
* 
* <v1.0 - 2024_0117_�ֿ���>
* 1- ����Ŭ������ ���� ���� ������ �����Ͱ� ��� ��ųʸ��� ��ȯ�ϴ� �޼��� �ۼ�
* 
* <v1.1 - 2024_0125_�ֿ���>
* 1- ItemBuildingŬ������ �Ӽ� �������� ���Ͽ� ������ ���λ����� ����
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
            //"Ű ���� ��", (��з�, "�ѹ���", "������ �̸�", �ν����ͺ信�� ������ȣ, �ߺз�, �ܺ� ��Ŀ�, ������, "������ ����")
            { 
                "��", new ItemBuilding( ItemType.Misc, "5000", "��", new VisualReferenceIndex(0),
                MiscType.Building, BuildingType.Decoration, 10, "~~�ϴ¿뵵�� ���")    
            },
            { 
                "����", new ItemBuilding( ItemType.Misc, "5001", "����", new VisualReferenceIndex(1),
                MiscType.Building, BuildingType.Material, 15,"~~�ϴ¿뵵�� ���") 
            },
            { 
                "�κ��丮", new ItemBuilding( ItemType.Misc, "5002", "�κ��丮", new VisualReferenceIndex(2),
                MiscType.Building, BuildingType.Inventory ,15,"~~�ϴ¿뵵�� ���") 
            },
        };




    }

}
}
