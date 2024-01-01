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
    /// <summary>
    /// ��� ���� ������ ����� �����ϰ� �ִ� ��ųʸ� �����Դϴ�. Monobehaviour�� ������� �����Ƿ� �����ؼ� ������ �޾ƾ� �մϴ�.
    /// </summary>
    public partial class WorldItem
    {
        public readonly Dictionary<string, Item> miscDic = new Dictionary<string, Item>()
        {           //0x1000
            { "ö", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000000", "ö", 3.0f, new ImageReferenceIndex(0) ) },
            { "��ö", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000001", "��ö", 5.0f, new ImageReferenceIndex(1) ) },
            { "��ö", new ItemMisc( ItemType.Misc, MiscType.Basic, "0000002", "��ö", 7.0f, new ImageReferenceIndex(2) ) },
            { "�̽���", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000003", "�̽���", 20.0f, new ImageReferenceIndex(3) ) }, 
            { "�ڹ�Ʈ", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000004", "�ڹ�Ʈ", 16.0f, new ImageReferenceIndex(4) ) },
            { "ƼŸ��", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000005", "ƼŸ��", 25.0f, new ImageReferenceIndex(5) ) },
            { "�����ϸ���", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000006", "�����ϸ���", 60.0f, new ImageReferenceIndex(6) ) },
            { "�ܴ��� ��������", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000007", "�ܴ��� ��������", 2.0f, new ImageReferenceIndex(7) ) },
            { "ưư�� ��������", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000008", "ưư�� ��������", 3.0f, new ImageReferenceIndex(8) ) },
            { "������ ��������", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000009", "������ ��������", 4.0f, new ImageReferenceIndex(9) ) },
            { "�ε巯�� ��������", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000010", "�ε巯�� ��������", 5.0f, new ImageReferenceIndex(10) ) },
            { "������ ��������", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000011", "������ ��������", 20.0f, new ImageReferenceIndex(11) ) },
            { "�ູ���� ��������", new ItemMisc( ItemType.Misc, MiscType.Basic,"0000012", "�ູ���� ��������", 40.0f, new ImageReferenceIndex(12) ) },

            { "�ϴ��� ��", new ItemMisc( ItemType.Misc, MiscType.Additive, "0000300", "�ϴ��� ��", 100.0f, new ImageReferenceIndex(0) ) },
            { "��Ÿ�� ����", new ItemMisc( ItemType.Misc, MiscType.Additive, "0000301", "��Ÿ�� ����", 100.0f, new ImageReferenceIndex(1) ) },
            { "������ ����", new ItemMisc( ItemType.Misc, MiscType.Additive, "0000302", "������ ����", 100.0f, new ImageReferenceIndex(2) ) },
            { "������ ��ȭ", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000303", "������ ��ȭ", 100.0f, new ImageReferenceIndex(3) ) }, 
            { "���� ����", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000304", "���� ����", 30.0f, new ImageReferenceIndex(4) ) },
            { "�ź��� ����", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000305", "�ź��� ����", 30.0f, new ImageReferenceIndex(5) ) },
            { "ũ�� ����", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000306", "ũ�� ����", 25.0f, new ImageReferenceIndex(6) ) },
            { "���� ����", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000307", "���� ����", 20.0f, new ImageReferenceIndex(7) ) },
            { "���� ����", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000308", "���� ����", 15.0f, new ImageReferenceIndex(8) ) },
            { "���", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000309", "���", 60.0f, new ImageReferenceIndex(9) ) },
            { "��伮", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000310", "��伮", 40.0f, new ImageReferenceIndex(10) ) },
            { "��", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000311", "��", 30.0f, new ImageReferenceIndex(11) ) },
            { "��", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000312", "��", 20.0f, new ImageReferenceIndex(12) ) },
            { "����", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000313", "����", 2.0f, new ImageReferenceIndex(13) ) },
            { "�ع�ġ", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000314", "�ع�ġ", 1.0f, new ImageReferenceIndex(14) ) },
            { "������ ��", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000315", "������ ��", 15.0f, new ImageReferenceIndex(15) ) },
            { "���� ����", new ItemMisc( ItemType.Misc, MiscType.Additive,"0000316", "���� ����", 6.0f, new ImageReferenceIndex(16) ) },
            
            { "�ʱ� ������ ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000700", "�ʱ� ������ ����", 50.0f, new ImageReferenceIndex(0) ) },
            { "�ʱ� ������ ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000701", "�ʱ� ������ ����", 50.0f, new ImageReferenceIndex(1) ) },
            { "�ʱ� ������ ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000702", "�ʱ� ������ ����", 50.0f, new ImageReferenceIndex(2) ) },
            { "�ʱ� ����� ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000703", "�ʱ� ����� ����", 50.0f, new ImageReferenceIndex(3) ) },
            { "�ʱ� ������ ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000704", "�ʱ� ������ ����", 50.0f, new ImageReferenceIndex(4) ) },
            { "�߱� ������ ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000705", "�߱� ������ ����", 125.0f, new ImageReferenceIndex(0) ) },
            { "�߱� ������ ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000706", "�߱� ������ ����", 125.0f, new ImageReferenceIndex(1) ) },
            { "�߱� ������ ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000707", "�߱� ������ ����", 125.0f, new ImageReferenceIndex(2) ) },
            { "�߱� ����� ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000708", "�߱� ����� ����", 125.0f, new ImageReferenceIndex(3) ) },
            { "�߱� ������ ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000709", "�߱� ������ ����", 125.0f, new ImageReferenceIndex(4) ) },
            { "��� ������ ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000710", "��� ������ ����", 200.0f, new ImageReferenceIndex(0) ) },
            { "��� ������ ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000711", "��� ������ ����", 200.0f, new ImageReferenceIndex(1) ) },
            { "��� ������ ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000712", "��� ������ ����", 200.0f, new ImageReferenceIndex(2) ) },
            { "��� ����� ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000713", "��� ����� ����", 200.0f, new ImageReferenceIndex(3) ) },
            { "��� ������ ����", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000714", "��� ������ ����", 200.0f, new ImageReferenceIndex(4) ) },
            
            { "����", new ItemMisc( ItemType.Misc, MiscType.Fire, "0000600", "����", 1.0f, new ImageReferenceIndex(5) ) },
            { "��ź", new ItemMisc( ItemType.Misc, MiscType.Fire, "0000601", "��ź", 6.0f, new ImageReferenceIndex(6) ) },
            { "����", new ItemMisc( ItemType.Misc, MiscType.Fire, "0000602", "����", 15.0f, new ImageReferenceIndex(7) ) },
            { "��ȭ��", new ItemMisc( ItemType.Misc, MiscType.Enhancement,"0000610", "��ȭ��", 50.0f, new ImageReferenceIndex(9) ) }, 
            
            { "�Ӽ���-��", new ItemMisc( ItemType.Misc, MiscType.Attribute,"0000611", "�Ӽ���-��", 500.0f, new ImageReferenceIndex(8) ) }, 
            { "�Ӽ���-��", new ItemMisc( ItemType.Misc, MiscType.Attribute,"0000612", "�Ӽ���-��", 500.0f, new ImageReferenceIndex(8) ) }, 
            { "�Ӽ���-��", new ItemMisc( ItemType.Misc, MiscType.Attribute,"0000613", "�Ӽ���-��", 500.0f, new ImageReferenceIndex(8) ) }, 
            { "�Ӽ���-ȭ", new ItemMisc( ItemType.Misc, MiscType.Attribute,"0000614", "�Ӽ���-ȭ", 500.0f, new ImageReferenceIndex(8) ) }, 
            { "�Ӽ���-ǳ", new ItemMisc( ItemType.Misc, MiscType.Attribute,"0000615", "�Ӽ���-ǳ", 500.0f, new ImageReferenceIndex(8) ) }, 
        };        
    }
}
