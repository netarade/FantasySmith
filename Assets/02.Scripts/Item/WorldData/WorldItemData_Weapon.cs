using System.Collections.Generic;
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
*/



namespace WorldItemData
{
public partial class WorldItem
{
    private Dictionary<string, Item> InitDic_Weapon()
    {        
        return new Dictionary<string, Item>()
        {
            /*** 검 ***/
            { 
                "손도끼", new ItemWeapon( ItemType.Weapon, "2000", "손도끼", new VisualReferenceIndex(0),
                WeaponType.Sword, 10, 100, "아이템 설명넣기" )
            },
        };    
    }
}
}
