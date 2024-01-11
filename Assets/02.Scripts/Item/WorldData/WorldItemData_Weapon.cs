using System.Collections.Generic;
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
*/



namespace WorldItemData
{
public partial class WorldItem
{
    private Dictionary<string, Item> InitDic_Weapon()
    {        
        return new Dictionary<string, Item>()
        {
            /*** �� ***/
            { 
                "�յ���", new ItemWeapon( ItemType.Weapon, "2000", "�յ���", new VisualReferenceIndex(0),
                WeaponType.Sword, 10, 100, "������ ����ֱ�" )
            },
        };    
    }
}
}
