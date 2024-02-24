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
                "����", new ItemMisc( ItemType.Misc, "0000", "����", new VisualReferenceIndex(0),
                MiscType.Basic, "���� ���� ���� ������ ��������� ����� �� ���� �� �����ϴ�.")
            },
            {
                "��", new ItemMisc( ItemType.Misc, "0001", "��", new VisualReferenceIndex(1),
                MiscType.Basic, "�̰����� �����ΰ� ���� �� �־� ���Դϴ�.")
            },
            {
                "����", new ItemMisc( ItemType.Misc, "0002", "����", new VisualReferenceIndex(2),
                MiscType.Basic, "���𰡸� ���� �� �ִ� ���� �������� ���.")
            },
            {
                "��", new ItemMisc( ItemType.Misc, "0003", "��", new VisualReferenceIndex(3),
                MiscType.Basic, "�ٴڿ� �ƹ����Գ� �κη��� �ִ� ���� ���Դϴ�.")
            },
            {
                "�볪��", new ItemMisc( ItemType.Misc, "0004", "�볪��", new VisualReferenceIndex(4),
                MiscType.Basic, "�̰����� �����̵� ���� �� ���� �� �����ϴ�.")
            },
            {
                "��", new ItemMisc( ItemType.Misc, "0005", "��", new VisualReferenceIndex(5),
                MiscType.Basic, "�̰��� ������ ������ �ݼӿ��� ���ݰ����� �����˴ϴ�.")
            },
            {
                "����", new ItemMisc( ItemType.Misc, "0006", "����", new VisualReferenceIndex(6),
                MiscType.Basic, "���׿��� �̰����� ��ư� �� �ִ� ���� ����� ������ ���� �̴ϴ�.")
            },            
            {
                "��", new ItemMisc( ItemType.Misc, "0007", "��", new VisualReferenceIndex(7),
                MiscType.Basic, "���������� ���� ���� ���� ���� ���Դϴ�.")
            },
        };




    }


}
}
