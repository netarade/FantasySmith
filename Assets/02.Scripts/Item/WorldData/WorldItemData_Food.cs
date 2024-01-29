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
                    "Water", new ItemFood( ItemType.Misc, "0100", "Water", new VisualReferenceIndex(0),
                    MiscType.Food, new ItemStatus(0,0,30), "Food") 
                },
                {
                    "Berry", new ItemFood( ItemType.Misc, "0101", "Berry", new VisualReferenceIndex(1),
                    MiscType.Food, new ItemStatus(5,10,10), "A basic source of nutrition obtained by gathering plants.")
                },
                {
                    "Mushroom", new ItemFood( ItemType.Misc, "0102", "Mushroom", new VisualReferenceIndex(2),
                    MiscType.Food, new ItemStatus(5,10,10), "It is an umbrella-like fungus that can be eaten.")
                },
                {
                    "Meat", new ItemFood( ItemType.Misc, "0103", "Meat", new VisualReferenceIndex(3),
                    MiscType.Food, new ItemStatus(10,15,5), "Cooking ingredients obtained by hunting animals.")
                },
                {
                    "GrilledMeat", new ItemFood( ItemType.Misc, "0104", "GrilledMeat", new VisualReferenceIndex(4),
                    MiscType.Food, new ItemStatus(30,30,0), "Meat cooked to be eaten.")
                },
                {
                    "GrilledMushroom", new ItemFood( ItemType.Misc, "0105", "GrilledMushroom", new VisualReferenceIndex(5),
                    MiscType.Food, new ItemStatus(30,30,10), "Food")
                },
                {
                    "������⺺��", new ItemFood( ItemType.Misc, "0106", "������⺺��", new VisualReferenceIndex(6),
                    MiscType.Food, new ItemStatus(80,80,30), "Food")
                },
                {
                    "�����ⱸ��", new ItemFood( ItemType.Misc, "0107", "�����ⱸ��", new VisualReferenceIndex(7),
                    MiscType.Food, new ItemStatus(100,100,40,0,2), "Food")
                },
            };




        }
    }

}