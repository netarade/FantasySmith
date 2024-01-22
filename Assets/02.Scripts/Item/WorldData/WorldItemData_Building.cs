using System.Collections.Generic;
using ItemData;

/*
 * 
* [�۾� ����]
* 
* <v1.0 - 2024_0117_�ֿ���>
* 1- ����Ŭ������ ���� ���� ������ �����Ͱ� ��� ��ųʸ��� ��ȯ�ϴ� �޼��� �ۼ�
* 
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
                MiscType.Building, true, 10, "~~�ϴ¿뵵�� ���")    
            },
            { 
                "����", new ItemBuilding( ItemType.Misc, "5001", "����", new VisualReferenceIndex(1),
                MiscType.Building, false, 15,"~~�ϴ¿뵵�� ���") 
            },
            { 
                "��", new ItemBuilding( ItemType.Misc, "5002", "��", new VisualReferenceIndex(2),
                MiscType.Building, false ,15,"~~�ϴ¿뵵�� ���") 
            },
        };




    }

}
}
