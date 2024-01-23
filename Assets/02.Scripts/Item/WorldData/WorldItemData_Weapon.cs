using System.Collections.Generic;
using DataManagement;
using ItemData;

/*
 * 
* [작업 사항]
* 
* <v1.0 - 2023_1221_최원준>
* 1- CreateManager의 CreateAllItemDictionary메서드의 정보를 분리시켜 클래스 생성
* 
* <v1.1 -2023_1227_최원준>
* 1- 손도끼 아이템 추가
* 
* <V1.2 - 2023_1230_최원준>
* 1- 딕셔너리에 readonly속성 추가하여 아이템 속성의 기본값 수정을 방지
* 
* <v1.3 - 2023_1231_최원준>
* 1- 변수명 weaponDic을 weapDic으로 변경
* 
* <v2.0 - 2024_0118_최원준>
* 1- 크래프팅 서바이벌 전용 데이터를 받아서 붙여넣은 후, WeaponType 수정
* 
* <v2.1 - 2024_0123_최원준>
* 1- 착용지점을 나타내는 STransform equipTr값을 생성자에서 받도록 수정
* 
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
            //키 값, (아이템 타입, 넘버링, 스트링 네임, 아이템 비주얼 그래픽 배열, 무기 타입, 공격력, 내구도, "아이템 설명") 
            {
                "StoneAxe", new ItemWeapon( ItemType.Weapon, "0000", "StoneAxe", new VisualReferenceIndex(0),
                WeaponType.Axe, STransform.GetSTransform(0), 50, 100, "It is the most basic Axe made of stone." )
            },
            {
                "StonePickaxe", new ItemWeapon( ItemType.Weapon, "0001", "StonePickaxe", new VisualReferenceIndex(1),
                WeaponType.Pickax, STransform.GetSTransform(1), 50, 100, "It is the most basic Pickaxe made of stone." )
            },
            {
                "StoneSpear", new ItemWeapon( ItemType.Weapon, "0002", "StoneSpear", new VisualReferenceIndex(2),
                WeaponType.Spear, STransform.GetSTransform(2), 120, 100, "The most basic weapon to protect your body." )
            },
            {
                "WoodenBow", new ItemWeapon( ItemType.Weapon, "0003", "WoodenBow", new VisualReferenceIndex(3),
                WeaponType.Bow, STransform.GetSTransform(3), 100, 100, "It is a weapon for hunting fast animals." )
            },
            {
                "StoneKnife", new ItemWeapon( ItemType.Weapon, "0004", "StoneKnife", new VisualReferenceIndex(4),
                WeaponType.Sword, STransform.GetSTransform(4), 0, 100, "A tool for collecting plants." )
            },
            {
                "WoodBottle", new ItemWeapon( ItemType.Weapon, "0005", "WoodBottle", new VisualReferenceIndex(5),
                WeaponType.Tool, STransform.GetSTransform(5), 0, 100, "It is a tool for holding water and drinking it." )
            },
            {
                "WoodBottleFilledWithWater", new ItemWeapon( ItemType.Weapon, "0006", "WoodBottleFilledWithWater", new VisualReferenceIndex(6),
                WeaponType.Tool, STransform.GetSTransform(6), 0, 100, "Can drink water." )
            },
            {
                "BuildingHammer", new ItemWeapon( ItemType.Weapon, "0007", "BuildingHammer", new VisualReferenceIndex(7),
                WeaponType.Blunt, STransform.GetSTransform(7), 0, 100, "It is a construction tool." )
            },
            {
                "BoneAxe", new ItemWeapon( ItemType.Weapon, "0008", "BoneAxe", new VisualReferenceIndex(8),
                WeaponType.Axe, STransform.GetSTransform(8), 50, 100, "It is the most basic Axe made of bone." )
            },
            {
                "BonePickaxe", new ItemWeapon( ItemType.Weapon, "0009", "BonePickaxe", new VisualReferenceIndex(9),
                WeaponType.Pickax, STransform.GetSTransform(9), 50, 100, "It is the most basic Pickaxe made of bone." )
            },
            {
                "BoneSpear", new ItemWeapon( ItemType.Weapon, "0010", "BoneSpear", new VisualReferenceIndex(10),
                WeaponType.Spear, STransform.GetSTransform(10), 150, 100, "A stronger weapon to protect my body." )
            },
            {
                "BoneKnife", new ItemWeapon( ItemType.Weapon, "0011", "BoneKnife", new VisualReferenceIndex(11),
                WeaponType.Sword, STransform.GetSTransform(11), 0, 100, "A tool to collect plants faster." )
            },
            {
                "BoneBow", new ItemWeapon( ItemType.Weapon, "0012", "BoneBow", new VisualReferenceIndex(12),
                WeaponType.Bow, STransform.GetSTransform(12), 0, 100, "It is the most basic Bow made of bone." )
            },
        };    
    }
}
}
