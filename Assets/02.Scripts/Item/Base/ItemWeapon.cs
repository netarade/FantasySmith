using Newtonsoft.Json;
using System;
using UnityEngine;

/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1105_최원준>
 * 1- 일반무기(ItemWeapon) 클래스 파일 분리.
 * 2- 각 종 기본 변수 및 프로퍼티, 생성자와 메서드 구현
 * 3- 주석처리
 * 
 * <v1.1 - 2023_1106_최원준>
 * 1- System 자료형과 겹치는 관계로 Enhancement 를 EnumAttribute로 변경
 * 
 * <v1.2 - 2023_1106_최원준>
 * 1- 강화 횟수가 float형 이었던 점을 int로 수정
 * 2- 속성 잠금이 초기에 false로 되있던 점을 true로 수정
 * 3- LastPerformance - 최종성능이 높을 수록 먼저 표현되도록 if문의 순서를 조절
 * 
 * <v1.3 - 2023_1116_최원준>
 * 1- 아이템 클래스 구조 변경으로 인한 생성자내부의 이미지 참조 변수를 이미지 인덱스 변수로 수정
 * 
 * <v2.0 - 2023_1216_최원준>
 * 1- 각인배열의 최대 수량 iMaxEngraveNum을 추가하고 생성자에서 최대 수량을 초기화
 * 2- 현재 장착중인 각인의 갯수를 반환하는 EquipEngraveNum 프로퍼티 추가
 * 3- Engrave메서드에 3-iRemainEngraveNum 인덱스로 잡혀있던 점을 EquipEngraveNum로 수정. (인덱스 접근 오류를 수정)
 * 4- Engrave메서드에 EquipEngraveArrInfo에 접근하여 값을 수정 하던 점을 ieArrEquipEngrave에 접근하여 수정하는 것으로 변경
 * 5- IsAttrUnlocked 프로퍼티로 속성 개방여부를 수정할 수 있던 점을 삭제하고 새로운 메서드를 만듬.
 * 6- 열거형 EnumAttribute를 AttributeType으로 수정, enumAttribute변수명을 eAttribute로 수정
 * 
 * <v3.0 - 2023_1222_최원준>
 * 1- private변수를 직렬화하기 위해 [JsonProperty] 어트리뷰트를 추가하였음
 * 
 * <v3.1 - 2023_1224_최원준>
 * 1- Clone메서드 삭제 (Item클래스에서 같은 기능을 상속하므로)
 * 2- 기본 protected 변수 enumWeaponType과 enumBasicGrade을 삭제하고, 
 * get만 가능한 자동 구현 프로퍼티 eWeaponType, eBasicGrade으로 변경
 *  
 * <v4.0 - 2023_1226_최원준>
 * 1- 자동구현 프로퍼티를 일반프로퍼티로 변경, 내부변수 JsonProperty처리 및 프로퍼티 JsonIgnore처리
 * (프로퍼티는 저장공간 낭비 및 로드시 프로퍼티는 set이 없기 때문에 정보가 반영되지 않기 때문)
 * 
 * <v4.1 - 2023_1229_최원준>
 * 1- 파일명 변경 ItemData_Weapon->ItemWeapon
 * 
 * <v4.2 - 2024_0108_최원준>
 * 1- ItemImageCollection을 ItemVisualCollection명칭 변경관계로
 * 생성자의 매개변수명 imgRefIndex를 visualRefIndex 변경
 * 
 * <v5.0 - 2024_0111_최원준>
 * 1- 크래프팅 장르에 맞게 클래스 설계 변경
 * 
 * <v5.1 - 2024_0112_최원준>
 * 1- ItemWeapon클래스의 부모를 Item이 아니라 ItemEquip으로 수정
 * 이유는 방어구 등 착용가능한 아이템 클래스만 따로 선별하기 위함
 * 
 * <V5.2 - 2024_0117_최원준>
 * 1- WeaponType에 PICKAX와 NONE 추가
 * 
 * <v5.3 - 2024_0118_최원준>
 * 1- WeaponType의 PICKAX와 NONE 변수명 Pickax, None으로 변경 및 주석 수정
 * 2- 프로퍼티 Power와 Durability를 수정가능하도록 변경하였음. 
 * 이유는 Info클래스에서 접근 가능해야 하므로
 * 
 * (수정예정)
 * 1- Pickax를 삭제하고 Production 또는 Tool으로 변경할 예정 (생산계열 또는 도구 아이템이므로)
 * (ItemInfo_4.cs의 weaponTypeString도 수정해야함)
 * 
 */
namespace ItemData
{    
    // ItemInfo_4.cs의 weaponTypeString도 수정할것!
    /// <summary>
    /// 무기 아이템의 상세 분류 (검, 창, 도끼, 둔기, 활, 곡괭이, 생산, 도구, 기타, 없음)
    /// </summary>
    public enum WeaponType { Sword, Spear, Axe, Blunt, Bow, Pickax, Production, Tool, Etc, None } 


    /// <summary>
    /// 일반 무기 아이템
    /// </summary>
    [Serializable]
    public class ItemWeapon : ItemEquip
    {       
        /*** 기본 고유 정보 ***/
        [JsonProperty] protected WeaponType eWeaponType;   // 무기 소분류 타입 (검,창,활...)
        [JsonProperty] protected int iPower;               // 공격력
        [JsonProperty] protected int iDurability;          // 내구

        /** 아이템 업그레이드 정보 **/
        [JsonProperty] protected float fPowerMult;         // 장비 기본 성능 (일반 무기-100, 제작무기-제작에따른 변화)
                    


        /// <summary>
        /// 무기 아이템 소분류 타입 - Sword, Blade, Spear, Dagger 등이 있습니다.
        /// </summary>
        [JsonIgnore] public WeaponType WeaponType { get { return eWeaponType; } }
        
        /// <summary>
        /// 아이템의 공격력에 곱해지는 수치입니다. 기본값은 1이며, 제작무기의 경우 이 성능에 변동을 줄 수 있습니다.
        /// </summary>
        [JsonIgnore] public float PowerMult { get{ return fPowerMult; } set{ fPowerMult=value; } }

        
        /// <summary>
        /// 아이템의 공격력입니다. 수정가능한 변수입니다.
        /// </summary>
        [JsonIgnore] public int Power { 
                get { return (int)Mathf.Round(iPower*PowerMult); }  
                set { iPower=value; } 
            }

        /// <summary>
        /// 아이템의 내구도 입니다. 수정가능한 변수입니다.
        /// </summary>
        [JsonIgnore] public int Durability { 
                get { return iDurability; }
                set { iDurability=value; }
            }
        
        

        /// <summary>
        /// 무기 아이템의 최초 생성을 위한 생성자 옵션
        /// </summary>
        public ItemWeapon(

            // 아이템 기본 정보 
            ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, 

            // 무기 고유 정보
            WeaponType subType, int power, int durability, string desc
            
        ) : base( mainType, No, name, visualRefIndex, desc )
        {
            eWeaponType = subType;
            iPower = power;
            iDurability = durability;
            fPowerMult = 1f;                               
        }

    }
}
