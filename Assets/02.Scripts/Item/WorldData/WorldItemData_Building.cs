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
                "��", new ItemBuilding( ItemType.Building, "5000", "��", new VisualReferenceIndex(0),
                BuildingType.Basic, 10, "~~�ϴ¿뵵�� ���")    
            },
            { 
                "��Ÿ��", new ItemBuilding( ItemType.Building, "5001", "��Ÿ��", new VisualReferenceIndex(1),
                BuildingType.Basic, 15, "~~�ϴ¿뵵�� ���") 
            },
            { 
                "�κ��丮", new ItemBuilding( ItemType.Building, "5002", "�κ��丮", new VisualReferenceIndex(2),
                BuildingType.Inventory ,15, "~~�ϴ¿뵵�� ���") 
            },
        };
    }





    private Dictionary<string, Item> InitDic_BuildingMisc()
    {         
        return new Dictionary<string, Item>()
        {           
            //"Ű ���� ��", (��з�, "�ѹ���", "������ �̸�", �ν����ͺ信�� ������ȣ, �ߺз�, "������ ����")
            { 
                "��", new ItemMisc( ItemType.Misc, "5000", "��", new VisualReferenceIndex(0),
                MiscType.Building, "~~�ϴ¿뵵�� ���")    
            },
            { 
                "����", new ItemMisc( ItemType.Misc, "5000", "����", new VisualReferenceIndex(1),
                MiscType.Building, "~~�ϴ¿뵵�� ���")    
            },


        };
    }


}
}
