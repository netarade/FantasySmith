using System.Collections.Generic;
using CreateManagement;
using DataManagement;
using ItemData;

/*
 * 
* [�۾� ����]
* 
* <v1.0 - 2023_1221_�ֿ���>
* 1- CreateManager�� CreateAllItemDictionary�޼����� ������ �и����� Ŭ���� ����
* 
* <v1.1 -2023_1227_�ֿ���>
* 1- �յ��� ������ �߰�
* 
* <V1.2 - 2023_1230_�ֿ���>
* 1- ��ųʸ��� readonly�Ӽ� �߰��Ͽ� ������ �Ӽ��� �⺻�� ������ ����
* 
* <v1.3 - 2023_1231_�ֿ���>
* 1- ������ weaponDic�� weapDic���� ����
* 
* <v2.0 - 2024_0118_�ֿ���>
* 1- ũ������ �����̹� ���� �����͸� �޾Ƽ� �ٿ����� ��, WeaponType ����
* 
* <v2.1 - 2024_0123_�ֿ���>
* 1- ���������� ��Ÿ���� STransform equipTr���� �����ڿ��� �޵��� ����
* 
* <v2.2 - 2024_0130_�ֿ���>
* 1- ItemWeapon�� �߻�Ŭ������ ItemEquip�� STransform�� �������� �����ϸ鼭, ������ ��ġ ����
* 
* 2- GetSTransformȣ�� ���ڷ� IVCType�� �߰�
* 
*/



namespace WorldItemData
{
public partial class WorldItem
{
    private Dictionary<string, Item> InitDic_Weapon()
    {    

        return new Dictionary<string, Item>()
        {
            //Ű ��, (������ Ÿ��, �ѹ���, ��Ʈ�� ����, ������ ���־� �׷��� �迭, ���� Ÿ��, ���ݷ�, ������, "������ ����") 
            {
                "�� ����", new ItemWeapon( ItemType.Weapon, "0000", "�� ����", new VisualReferenceIndex(0),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 0), WeaponType.Axe, 50, 100, "���� ���� �⺻���� �����Դϴ�." )
            },
            {
                "�� ���", new ItemWeapon( ItemType.Weapon, "0001", "�� ���", new VisualReferenceIndex(1),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 1), WeaponType.Pickax, 50, 100, "���� ���� �⺻���� ����Դϴ�." )
            },
            {
                "�� â", new ItemWeapon( ItemType.Weapon, "0002", "�� â", new VisualReferenceIndex(2),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 2), WeaponType.Spear, 120, 100, "���� ���� â�Դϴ�. ���� �� ���� �����ϴ�." )
            },
            {
                "���� Ȱ", new ItemWeapon( ItemType.Weapon, "0003", "���� Ȱ", new VisualReferenceIndex(3),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 3), WeaponType.Bow, 100, 100, "������ ���� Ȱ�Դϴ�. ���� �������� ��⿡ �����մϴ�." )
            },
            {
                "�� Į", new ItemWeapon( ItemType.Weapon, "0004", "�� Į", new VisualReferenceIndex(4),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 4), WeaponType.Sword, 0, 100, "ä���̳� ��ü�� �ʿ��� �⺻���� �����Դϴ�." )
            },
            {
                "���� ����", new ItemWeapon( ItemType.Weapon, "0005", "���� ����", new VisualReferenceIndex(5),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 5), WeaponType.Tool, 0, 100, "���� ���� �� �ִ� ������ ���� �����Դϴ�." )
            },
            {
                "������ ���� ����", new ItemWeapon( ItemType.Weapon, "0006", "������ ���� ����", new VisualReferenceIndex(6),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 6), WeaponType.Tool, 0, 100, "���� ���� ��� �ִ� ���� �����Դϴ�." )
            },
            {
                "���� ��ġ", new ItemWeapon( ItemType.Weapon, "0007", "���� ��ġ", new VisualReferenceIndex(7),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 7), WeaponType.Blunt, 0, 100, "�̰����� �� �� ������ ���� �̷� �� �ֽ��ϴ�." )
            },
            {
                "�� ����", new ItemWeapon( ItemType.Weapon, "0008", "�� ����", new VisualReferenceIndex(8),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 8), WeaponType.Axe, 50, 100, "���� ���� �����Դϴ�. �� ������ ���� ������ �����ϴ�." )
            },
            {
                "�� ���", new ItemWeapon( ItemType.Weapon, "0009", "�� ���", new VisualReferenceIndex(9),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 9), WeaponType.Pickax, 50, 100, "���� ���� ����Դϴ�. �� ��̿� ���� ������ �����ϴ�." )
            },
            {
                "�� â", new ItemWeapon( ItemType.Weapon, "0010", "�� â", new VisualReferenceIndex(10),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 10), WeaponType.Spear, 150, 100, "���� ���� â�Դϴ�. �ı����̰� ����մϴ�." )
            },
            {
                "�� Į", new ItemWeapon( ItemType.Weapon, "0011", "�� Į", new VisualReferenceIndex(11),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 11), WeaponType.Sword, 0, 100, "ä���� ��ü�� ���� �� Ưȭ�� �����Դϴ�." )
            },
            {
                "�� Ȱ", new ItemWeapon( ItemType.Weapon, "0012", "�� Ȱ", new VisualReferenceIndex(12),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 12), WeaponType.Bow, 130, 100, "�ı����� Ȱ�Դϴ�. �̰͸� �ִٸ� �� �̻� �η��� �ʽ��ϴ�." )
            },
        };    
    }
}
}
