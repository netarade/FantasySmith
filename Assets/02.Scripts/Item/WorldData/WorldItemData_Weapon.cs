using System.Collections.Generic;
using CreateManagement;
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
* <v2.2 - 2024_0130_최원준>
* 1- ItemWeapon의 추상클래스인 ItemEquip이 STransform을 가지도록 변경하면서, 생성자 위치 조절
* 
* 2- GetSTransform호출 인자로 IVCType을 추가
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
                "돌 도끼", new ItemWeapon( ItemType.Weapon, "0000", "돌 도끼", new VisualReferenceIndex(0),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 0), WeaponType.Axe, 50, 100, "돌로 만든 기본적인 도끼입니다." )
            },
            {
                "돌 곡괭이", new ItemWeapon( ItemType.Weapon, "0001", "돌 곡괭이", new VisualReferenceIndex(1),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 1), WeaponType.Pickax, 50, 100, "돌로 만든 기본적인 곡괭이입니다." )
            },
            {
                "돌 창", new ItemWeapon( ItemType.Weapon, "0002", "돌 창", new VisualReferenceIndex(2),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 2), WeaponType.Spear, 120, 100, "돌로 만든 창입니다. 없는 것 보단 낫습니다." )
            },
            {
                "나무 활", new ItemWeapon( ItemType.Weapon, "0003", "나무 활", new VisualReferenceIndex(3),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 3), WeaponType.Bow, 100, 100, "나무로 만든 활입니다. 빠른 동물들을 잡기에 적합합니다." )
            },
            {
                "돌 칼", new ItemWeapon( ItemType.Weapon, "0004", "돌 칼", new VisualReferenceIndex(4),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 4), WeaponType.Sword, 0, 100, "채집이나 해체에 필요한 기본적인 도구입니다." )
            },
            {
                "나무 물병", new ItemWeapon( ItemType.Weapon, "0005", "나무 물병", new VisualReferenceIndex(5),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 5), WeaponType.Tool, 0, 100, "물을 담을 수 있는 나무로 만든 물병입니다." )
            },
            {
                "가득찬 나무 물병", new ItemWeapon( ItemType.Weapon, "0006", "가득찬 나무 물병", new VisualReferenceIndex(6),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 6), WeaponType.Tool, 0, 100, "물이 가득 담겨 있는 나무 물병입니다." )
            },
            {
                "건축 망치", new ItemWeapon( ItemType.Weapon, "0007", "건축 망치", new VisualReferenceIndex(7),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 7), WeaponType.Blunt, 0, 100, "이것으로 내 집 마련의 꿈을 이룰 수 있습니다." )
            },
            {
                "뼈 도끼", new ItemWeapon( ItemType.Weapon, "0008", "뼈 도끼", new VisualReferenceIndex(8),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 8), WeaponType.Axe, 50, 100, "뼈로 만든 도끼입니다. 돌 도끼에 비해 성능이 좋습니다." )
            },
            {
                "뼈 곡괭이", new ItemWeapon( ItemType.Weapon, "0009", "뼈 곡괭이", new VisualReferenceIndex(9),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 9), WeaponType.Pickax, 50, 100, "뼈로 만든 곡괭이입니다. 돌 곡괭이에 비해 성능이 좋습니다." )
            },
            {
                "뼈 창", new ItemWeapon( ItemType.Weapon, "0010", "뼈 창", new VisualReferenceIndex(10),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 10), WeaponType.Spear, 150, 100, "뼈로 만든 창입니다. 파괴적이고 흉악합니다." )
            },
            {
                "뼈 칼", new ItemWeapon( ItemType.Weapon, "0011", "뼈 칼", new VisualReferenceIndex(11),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 11), WeaponType.Sword, 0, 100, "채집과 해체에 한층 더 특화된 도구입니다." )
            },
            {
                "뼈 활", new ItemWeapon( ItemType.Weapon, "0012", "뼈 활", new VisualReferenceIndex(12),
                EquipType.Weapon, STransform.GetSTransform(IVCType.Weapon, 12), WeaponType.Bow, 130, 100, "파괴적인 활입니다. 이것만 있다면 더 이상 두렵지 않습니다." )
            },
        };    
    }
}
}
