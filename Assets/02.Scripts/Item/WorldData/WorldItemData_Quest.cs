using System.Collections.Generic;
using CreateManagement;
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
* 2- GetSTransformȣ�� ���ڷ� IVCType�� �߰�
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
                "���� �ĵ�", new ItemQuest( ItemType.Quest, "0000", "���� �ĵ�", new VisualReferenceIndex(0),
                "�������� ���� ����Դϴ�. �̷������� ������ �ſ� �پ�ϴ�.", EquipType.Helmet, STransform.GetSTransform(IVCType.Quest, 0) )
            },
            {
                "�ű��� ��", new ItemQuest( ItemType.Quest, "0001", "�ű��� ��", new VisualReferenceIndex(1),
                "���� �������� �ʰ� �յ� �� �ִ� ���Դϴ�." )
            },
            {
                "���� ����", new ItemQuest( ItemType.Quest, "0002", "���� ����", new VisualReferenceIndex(2),
                "��ġ�� ����ϱ� ���� �������� ������ ��������ϴ�." )
            },
            {
                "�μ��� ��ġ", new ItemQuest( ItemType.Quest, "0003", "�μ��� ��ġ", new VisualReferenceIndex(3),
                "Ž�缱�� �߶��� �� ������ ���� �����ġ �Դϴ�. ������ ���峪 �۵����� �ʽ��ϴ�." )
            },
            {
                "������ ��", new ItemQuest( ItemType.Quest, "0004", "������ ��", new VisualReferenceIndex(4),
                "���� ���� �ִ� ������ óġ�ϰ� ���� �λ깰. �̰��� ��� ���� �Ȱ��� ��������ϴ�." )
            },
            {
                "�۶��̴�", new ItemQuest( ItemType.Quest, "0005", "�۶��̴�", new VisualReferenceIndex(5),
                "�̰� ���� �����ϰ� �̰����� Ż���� �� ���� �� �����ϴ�. ����." )
            },
        };        
    }

}
}

