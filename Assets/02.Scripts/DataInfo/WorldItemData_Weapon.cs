using System.Collections.Generic;
using ItemData;

/*
 * 
* [�۾� ����]
* 
* <v1.0 - 2023_1221_�ֿ���>
* 1- CreateManager�� CreateAllItemDictionary�޼����� ������ �и����� Ŭ���� ����
* 
*/



namespace WorldItemData
{
    public partial class WorldItem
    {
        public Dictionary<string, Item> weaponDic=new Dictionary<string, Item>()
        {
            /*** �� ***/
            { "ö ��", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001000", "ö ��", 10.0f, new ImageReferenceIndex(0)
                , ItemGrade.Low, 10, 100, 1.0f, 10, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("ö",2)} 
                , new CraftMaterial[]{new CraftMaterial("����",1)} 
                , Recipie.Eu) 
            },
            { "��ö ��", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001001", "��ö ��", 20.0f, new ImageReferenceIndex(1)
                , ItemGrade.Low, 12, 100, 1.0f, 18, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("��ö",2), new CraftMaterial("ö",1)}
                , new CraftMaterial[]{new CraftMaterial("����",2)}
                , Recipie.Na)
            },
            { "�̽��� ��", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001002", "�̽��� ��", 40.0f, new ImageReferenceIndex(2) 
                , ItemGrade.Low, 18, 100, 1.0f, 10, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("�̽���",2), new CraftMaterial("��ö",1),new CraftMaterial("ö",1)}
                , new CraftMaterial[]{new CraftMaterial("��",2)}
                , Recipie.Ma)
            
            },
            { "��ö ��", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001003", "��ö ��", 60.0f, new ImageReferenceIndex(3) 
                , ItemGrade.Low, 23, 100, 1.0f, 40, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("��ö",2), new CraftMaterial("��ö",3)}
                , new CraftMaterial[]{new CraftMaterial("����",3)}
                , Recipie.Ga)
            },
            { "����ÿ", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001004", "����ÿ", 80.0f, new ImageReferenceIndex(4) 
                , ItemGrade.Low, 25, 100, 1.0f, 20, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("�̽���",2), new CraftMaterial("��ö",2),new CraftMaterial("��ö",1)}
                , new CraftMaterial[]{new CraftMaterial("����",2),new CraftMaterial("��伮",1)}
                , Recipie.Da)
            },


            { "����Į��", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001100", "����Į��", 120.0f, new ImageReferenceIndex(5)
                , ItemGrade.Medium, 36, 150, 1.0f, 25, AttributeType.Water
                , new CraftMaterial[]{new CraftMaterial("�̽���",7)}
                , new CraftMaterial[]{new CraftMaterial("���� ����",5)}
                , Recipie.Da)
            },
            { "������ ��", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001101", "������ ��", 180.0f, new ImageReferenceIndex(6)
                , ItemGrade.Medium, 38, 150, 1.0f, 30, AttributeType.Wind
                , new CraftMaterial[]{new CraftMaterial("ƼŸ��",2),new CraftMaterial("�̽���",4)}
                , new CraftMaterial[]{new CraftMaterial("���",5)}
                , Recipie.Ga)
            },
            { "������ ��", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001102", "������ ��", 150.0f, new ImageReferenceIndex(7)
                , ItemGrade.Medium, 48, 150, 1.0f, 30, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("ƼŸ��",1),new CraftMaterial("�̽���",3),new CraftMaterial("��ö",2)}
                , new CraftMaterial[]{new CraftMaterial("��",3),new CraftMaterial("��",2)}
                , Recipie.Na)
            },
            { "�ƹ� �ҵ�", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001103", "�ƹ� �ҵ�", 110.0f, new ImageReferenceIndex(8) 
                , ItemGrade.Medium, 32, 150, 1.0f, 12, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("ƼŸ��",1),new CraftMaterial("�̽���",3)}
                , new CraftMaterial[]{new CraftMaterial("��",2),new CraftMaterial("��伮",2)}
                , Recipie.Ma)
            },
            { "���ö�", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001104", "���ö�", 100.0f, new ImageReferenceIndex(9) 
                , ItemGrade.Medium, 38, 150, 1.0f, 20, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("�̽���",4),new CraftMaterial("��ö",2)}
                , new CraftMaterial[]{new CraftMaterial("��",3),new CraftMaterial("ũ�� ����",1)}
                , Recipie.Ee)
            },

            { "õ���� ����", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001200", "õ���� ����", 680.0f, new ImageReferenceIndex(10)
                , ItemGrade.High, 80, 200, 1.10f, 10, AttributeType.Wind
                , new CraftMaterial[]{new CraftMaterial("�����ϸ���",3),new CraftMaterial("ƼŸ��",5),new CraftMaterial("�̽���",10)}
                , new CraftMaterial[]{new CraftMaterial("�ϴ��� ��",2), new CraftMaterial("���� ����",5)}
                , Recipie.Da)
            },
            { "�ĸ��� �Ҳ�", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001201", "�ĸ��� �Ҳ�", 640.0f, new ImageReferenceIndex(11)
                , ItemGrade.High, 105, 200, 1.10f, 30, AttributeType.Fire
                , new CraftMaterial[]{new CraftMaterial("�����ϸ���",5),new CraftMaterial("ƼŸ��",10),new CraftMaterial("�̽���",3)}
                , new CraftMaterial[]{new CraftMaterial("��Ÿ�� ����",1), new CraftMaterial("�ź��� ����",5)}
                , Recipie.Ra)
            },
            { "�����", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001202", "�����", 480.0f, new ImageReferenceIndex(12) 
                , ItemGrade.High, 120, 200, 1.10f, 50, AttributeType.Wind
                , new CraftMaterial[]{new CraftMaterial("�����ϸ���",3),new CraftMaterial("ƼŸ��",5),new CraftMaterial("��ö",8)}
                , new CraftMaterial[]{new CraftMaterial("��",2),new CraftMaterial("���� ����",5)}
                , Recipie.Ma)
            },
            { "�̽�ƿ����", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001203", "�̽�ƿ����", 700.0f, new ImageReferenceIndex(13) 
                , ItemGrade.High, 95, 200, 1.10f, 25, AttributeType.Gold
                , new CraftMaterial[]{new CraftMaterial("�����ϸ���",5),new CraftMaterial("�̽���",5),new CraftMaterial("�ڹ�Ʈ",12)}
                , new CraftMaterial[]{new CraftMaterial("�ź��� ����",5),new CraftMaterial("������ ����",2)}
                , Recipie.Ra)
            },
            { "ũ������", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001204", "ũ������", 350.0f, new ImageReferenceIndex(14) 
                , ItemGrade.High, 80, 200, 1.10f, 30, AttributeType.Earth
                , new CraftMaterial[]{new CraftMaterial("�̽���",13),new CraftMaterial("��ö",6)}
                , new CraftMaterial[]{new CraftMaterial("��",2),new CraftMaterial("���",2), new CraftMaterial("ũ�� ����",3)}
                , Recipie.Ga)
            },

            /**** Ȱ ****/
            { "��ɲ��� Ȱ", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008000", "��ɲ��� Ȱ", 10.0f, new ImageReferenceIndex(0)
                , ItemGrade.Low, 10, 100, 0.8f, 10, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("ö",3), new CraftMaterial("�ܴ��� ��������",3)} 
                , null 
                , Recipie.Eu) 
            },
            { "�� ����", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008001", "�� ����", 20.0f, new ImageReferenceIndex(1)
                , ItemGrade.Low, 12, 100, 0.8f, 18, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("ö",5), new CraftMaterial("�ε巯�� ��������",2)}
                , new CraftMaterial[]{new CraftMaterial("������ ��",2)}
                , Recipie.Na)
            },
            { "�Ĺ� ����", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008002", "�Ĺ� ����", 40.0f, new ImageReferenceIndex(2) 
                , ItemGrade.Low, 18, 100, 0.8f, 10, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("��ö",5), new CraftMaterial("�ܴ��� ��������",2)}
                , null
                , Recipie.Ma)
            
            },
            { "��ġ���� Ȱ", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008003", "��ġ���� Ȱ", 60.0f, new ImageReferenceIndex(3) 
                , ItemGrade.Low, 23, 100, 0.8f, 40, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("ö",3), new CraftMaterial("������ ��������",2)}
                , new CraftMaterial[]{new CraftMaterial("�ع�ġ",4)}
                , Recipie.Ga)
            },
            { "�������� Ȱ", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008004", "�������� Ȱ", 80.0f, new ImageReferenceIndex(4) 
                , ItemGrade.Low, 25, 100, 0.8f, 20, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("ö",4), new CraftMaterial("������ ��������",3)}
                , new CraftMaterial[]{new CraftMaterial("���°���",2)}
                , Recipie.Da)
            },


            { "���̴� ����", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008100", "���̴� ����", 120.0f, new ImageReferenceIndex(5)
                , ItemGrade.Medium, 36, 150, 0.8f, 25, AttributeType.Water
                , new CraftMaterial[]{new CraftMaterial("ƼŸ��",5),new CraftMaterial("�ܴ��� ��������",5)}
                , new CraftMaterial[]{new CraftMaterial("���",3)}
                , Recipie.Da)
            },
            { "�̱� ����", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008101", "�̱� ����", 180.0f, new ImageReferenceIndex(6)
                , ItemGrade.Medium, 38, 150, 0.8f, 30, AttributeType.Wind
                , new CraftMaterial[]{new CraftMaterial("�ڹ�Ʈ",6),new CraftMaterial("ưư�� ��������",2)}
                , new CraftMaterial[]{new CraftMaterial("���� ����",4)}
                , Recipie.Ra)
            },
            { "ź��", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008102", "ź��", 150.0f, new ImageReferenceIndex(7) 
                , ItemGrade.Medium, 48, 150, 0.8f, 30, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("�̽���",5),new CraftMaterial("�ε巯�� ��������",4)}
                , new CraftMaterial[]{new CraftMaterial("������ ��",3)}
                , Recipie.Ma)
            },
            { "����", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008103", "����", 110.0f, new ImageReferenceIndex(8) 
                , ItemGrade.Medium, 32, 150, 0.8f, 12, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("�̽���",4),new CraftMaterial("������ ��������",3)}
                , new CraftMaterial[]{new CraftMaterial("ũ�� ����",5)}
                , Recipie.Ee)
            },
            { "�߰����� Ȱ", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008104", "�߰����� Ȱ", 100.0f, new ImageReferenceIndex(9) 
                , ItemGrade.Medium, 38, 150, 0.8f, 20, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("��ö",3),new CraftMaterial("������ ��������",5)}
                , new CraftMaterial[]{new CraftMaterial("���� ����",4)}
                , Recipie.Ga)
            },


            { "��ǳ �ĸ�Ƽ��", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008200", "��ǳ �ĸ�Ƽ��", 450.0f, new ImageReferenceIndex(10)
                , ItemGrade.High, 80, 200, 0.88f, 16, AttributeType.Wind
                , new CraftMaterial[]{new CraftMaterial("ƼŸ��",7),new CraftMaterial("�̽���",13),new CraftMaterial("������ ��������",3)}
                , new CraftMaterial[]{new CraftMaterial("�ϴ��� ��",4), new CraftMaterial("ũ�� ����",3)}
                , Recipie.Da)
            },
            { "�Ǵн��� �ӻ���", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008201", "�Ǵн��� �ӻ���", 640.0f, new ImageReferenceIndex(11)
                , ItemGrade.High, 105, 200, 0.88f, 28, AttributeType.Fire
                , new CraftMaterial[]{new CraftMaterial("�����ϸ���",3),new CraftMaterial("ƼŸ��",14),new CraftMaterial("�ູ���� ��������",4)}
                , new CraftMaterial[]{new CraftMaterial("��Ÿ�� ����",1), new CraftMaterial("������ ����",3)}
                , Recipie.Ga)
            },
            { "�¾��� Ȱ", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008202", "�¾��� Ȱ", 530.0f, new ImageReferenceIndex(12) 
                , ItemGrade.High, 120, 200, 0.88f, 50, AttributeType.Fire
                , new CraftMaterial[]{new CraftMaterial("�����ϸ���",7),new CraftMaterial("��ö",18),new CraftMaterial("������ ��������",5)}
                , new CraftMaterial[]{new CraftMaterial("��Ÿ�� ����",3),new CraftMaterial("�ϴ��� ��",2)}
                , Recipie.Ra)
            },
            { "����� �ű�", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008203", "����� �ű�", 480.0f, new ImageReferenceIndex(13) 
                , ItemGrade.High, 95, 200, 0.88f, 32, AttributeType.Earth
                , new CraftMaterial[]{new CraftMaterial("�����ϸ���",5),new CraftMaterial("�ڹ�Ʈ",14),new CraftMaterial("�ູ���� ��������",3)}
                , new CraftMaterial[]{new CraftMaterial("������ ��ȭ",7)}
                , Recipie.Na)
            },
            { "�ĸ��� Ȱ", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008204", "�ĸ��� Ȱ", 420.0f, new ImageReferenceIndex(14) 
                , ItemGrade.High, 80, 200, 0.88f, 36, AttributeType.Gold
                , new CraftMaterial[]{new CraftMaterial("�����ϸ���",5),new CraftMaterial("�̽���",11),new CraftMaterial("�ູ���� ��������",4)}
                , new CraftMaterial[]{new CraftMaterial("�ź��� ����",4),new CraftMaterial("������ ����",1)}
                , Recipie.Ma)
            }

        };    
    }
}
