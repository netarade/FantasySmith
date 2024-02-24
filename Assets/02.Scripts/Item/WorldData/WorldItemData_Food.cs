using ItemData;
using System.Collections.Generic;

namespace WorldItemData
{
    public partial class WorldItem
    {
        private Dictionary<string, Item> InitDic_Food()
        {
            //Ű ��, (������ Ÿ��, �ѹ���, ��Ʈ�� ����, ������ ���־� �׷��� �迭, ��ȭ Ÿ��, ������ �������ͽ���, "������ ����") 
            return new Dictionary<string, Item>()
            {
                {
                    "��", new ItemFood( ItemType.Misc, "0100", "��", new VisualReferenceIndex(0),
                    MiscType.Food, new ItemStatus(0,0,30), "���� �� �ִ� ������ ���̴�.") 
                },
                {
                    "�����", new ItemFood( ItemType.Misc, "0101", "�����", new VisualReferenceIndex(1),
                    MiscType.Food, new ItemStatus(5,10,10), "�Ĺ��� ä���ؼ� ���� �� �ִ� �⺻���� ���� ���޿��Դϴ�.")
                },
                {
                    "����", new ItemFood( ItemType.Misc, "0102", "����", new VisualReferenceIndex(2),
                    MiscType.Food, new ItemStatus(5,10,10), "������ ���� ���� ��� ����� ������ �Դϴ�. ���� �� �ֽ��ϴ�.")
                },
                {
                    "�����", new ItemFood( ItemType.Misc, "0103", "�����", new VisualReferenceIndex(3),
                    MiscType.Food, new ItemStatus(10,15,5), "������ ����Ͽ� ���� �� �ִ� �丮 ����Դϴ�. �ǰ��� ���� ������ ������ �ʰ� ���� �� �ֽ��ϴ�.")
                },
                {
                    "��ⱸ��", new ItemFood( ItemType.Misc, "0104", "��ⱸ��", new VisualReferenceIndex(4),
                    MiscType.Food, new ItemStatus(30,30,0), "�˸°� ���� ���ִ� ��� �Դϴ�.")
                },
                {
                    "���� ����", new ItemFood( ItemType.Misc, "0105", "���� ����", new VisualReferenceIndex(5),
                    MiscType.Food, new ItemStatus(30,30,10), "������ ����⸦ ���� ����            �丮�Դϴ�.")
                },
                {
                    "��� ���� ����", new ItemFood( ItemType.Misc, "0106", "��� ���� ����", new VisualReferenceIndex(6),
                    MiscType.Food, new ItemStatus(80,80,30), "������ ������ũ�� ������ ����� �Ǹ��� �丮�Դϴ�.")
                },
                {
                    "���� ��ⱸ��", new ItemFood( ItemType.Misc, "0107", "���� ��ⱸ��", new VisualReferenceIndex(7),
                    MiscType.Food, new ItemStatus(100,100,40,0,2), "�߱� ���Ŀ� ������ ��� ���� �丮�Դϴ�. �ǰ��� �ſ� ���� �����Դϴ�.")
                },
            };




        }
    }

}