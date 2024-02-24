using System.Collections.Generic;
using ItemData;

/*
 * 
* [�۾� ����]
* 
* <v1.0 - 2024_0117_�ֿ���>
* 1- ����Ŭ������ ���� ���� ������ �����Ͱ� ��� ��ųʸ��� ��ȯ�ϴ� �޼��� �ۼ�
* 
* <v2.0 - 2024_0126_�ֿ���>
*  1- �Ǽ��������� ��ȭ�� �Ϲ� ������ ����
* 
* 
*/



namespace WorldItemData
{
    public partial class WorldItem
    {

        // �Ǽ� �������� (�κ��丮 ���������� ǥ�������� ������)
        private Dictionary<string, Item> InitDic_BuildingMisc()
        {         
            return new Dictionary<string, Item>()
            {           
                //"Ű ���� ��", (��з�, "�ѹ���", "������ �̸�", �ν����ͺ信�� ������ȣ, �ߺз�, "������ ����")
                { 
                    "��", new ItemMisc( ItemType.Misc, "5000", "��", new VisualReferenceIndex(0),
                    MiscType.Building, "~~�ϴ¿뵵�� ���")    
                },

            };
        }


        // �Ǽ� �ܺξ����� (���忡���� ǥ������)
        private Dictionary<string, Item> InitDic_Building()
        {
            return new Dictionary<string, Item>()
        {           
            //"Ű ���� ��", (��з�, "�ѹ���", "������ �̸�", �ν����ͺ信�� ������ȣ, �ߺз�, �ܺ� ��Ŀ�, ������, "������ ����")
            {
                "������", new ItemBuilding( ItemType.Building, "5000", "������", new VisualReferenceIndex(0),
                BuildingType.Basic, 300, "���� �����ִ� ������Ʈ")
            },
            {
                "�����ٴ�", new ItemBuilding( ItemType.Building, "5001", "�����ٴ�", new VisualReferenceIndex(1),
                BuildingType.Basic, 300,"�ǹ��� �ٴ��� ä��� ������Ʈ")
            },
            {
                "��", new ItemBuilding( ItemType.Building, "5002", "��", new VisualReferenceIndex(2),
                BuildingType.Basic, 300,"�ǹ� ������ ����ϴ� ������Ʈ")
            },
            {
                "���", new ItemBuilding( ItemType.Building, "5003", "���", new VisualReferenceIndex(3),
                BuildingType.Basic, 300,"���踦 �����ų� ������ �� �ʿ��� ������Ʈ")
            },
            {
                "����Ʋ", new ItemBuilding( ItemType.Building, "5004", "����Ʋ", new VisualReferenceIndex(4),
                BuildingType.Basic, 300,"�Ǽ��� Ʋ�� �����ϴ� ������Ʈ")
            },
            {
                "����Ʋ", new ItemBuilding( ItemType.Building, "5005", "����Ʋ", new VisualReferenceIndex(5),
                BuildingType.Basic, 300,"�Ǽ��� ���� Ʋ�� �����ϴ� ������Ʈ")
            },
            {
                "ȶ��", new ItemBuilding( ItemType.Building, "5006", "ȶ��", new VisualReferenceIndex(6),
                BuildingType.Basic, 300,"�ֺ��� ������ ������Ʈ")
            },
            {
                "������ȶ��", new ItemBuilding( ItemType.Building, "5007", "������ȶ��", new VisualReferenceIndex(7),
                BuildingType.Basic, 300,"���� �ٴ� �ֺ��� ������ ������Ʈ")
            },
            {
                "��Ÿ��", new ItemBuilding( ItemType.Building, "5008", "��Ÿ��", new VisualReferenceIndex(8),
                BuildingType.Basic, 300,"��Ÿ�� ������Ʈ")
            },
            {
                "��ں�", new ItemBuilding( ItemType.Building, "5009", "��ں�", new VisualReferenceIndex(9),
                BuildingType.Basic, 300,"��Ÿ �������� ���ų� �ֺ��� ������ ������Ʈ")
            },
            {
                "���۴�", new ItemBuilding( ItemType.Building, "5010", "���۴�", new VisualReferenceIndex(10),
                BuildingType.Basic, 300,"������ ���� ������Ʈ")
            },
            {
                "������", new ItemBuilding( ItemType.Building, "5011", "������", new VisualReferenceIndex(11),
                BuildingType.Inventory, 300,"������ ���� ������Ʈ")
            },
            {
                "ħ��", new ItemBuilding( ItemType.Building, "5012", "ħ��", new VisualReferenceIndex(12),
                BuildingType.Basic, 300,"�ǳ� ���� ������ ������Ʈ")
            },
            {
                "ħ��", new ItemBuilding( ItemType.Building, "5013", "ħ��", new VisualReferenceIndex(13),
                BuildingType.Basic, 300,"�ǿ� ���� ������ ������Ʈ")
            },
            {
                "ȭ��", new ItemBuilding( ItemType.Building, "5014", "ȭ��", new VisualReferenceIndex(14),
                BuildingType.Basic, 300,"�������� ���� ������Ʈ")
            },
            {
                "������", new ItemBuilding( ItemType.Building, "5015", "������", new VisualReferenceIndex(15),
                BuildingType.Basic, 300,"���� ������Ʈ")
            },
        };
        }

    }
}
