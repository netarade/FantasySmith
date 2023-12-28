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
 * 
 */
namespace ItemData
{    
    /// <summary>
    /// 무기 아이템의 상세 분류
    /// </summary>
    public enum WeaponType { Sword, Blade, Spear, Dagger, Thin, Axe, Blunt, Bow, Crossbow, Claw, Whip } //검,도,창,단검,세검,활,석궁,클로,채찍

    /// <summary>
    /// 6대 속성 - 수,금,지,화,풍,무
    /// </summary>
    public enum AttributeType { Water, Gold, Earth, Fire, Wind, None }
   
    /// <summary>
    /// 장비의 최종 등급 - 노말, 매직, 레어, 에픽, 유니크, 레전드
    /// </summary>
    public enum Rarity { Normal, Magic, Rare, Epic, Unique, Legend }
    


    /// <summary>
    /// 일반 무기 아이템
    /// </summary>
    [Serializable]
    public class ItemWeapon : Item
    {       
        /*** 기본 고유 정보 ***/
        [JsonProperty] protected WeaponType eWeaponType;               // 무기 소분류 타입 (검,창,활...)
        [JsonProperty] protected ItemGrade eBasicGrade;                // 무기 기본 분류 타입 (초급, 중급, 고급)
        [JsonProperty] protected int iPower;                           // 공격력
        [JsonProperty] protected int iDurability;                      // 내구
        [JsonProperty] protected float fSpeed;                         // 공격속도
        [JsonProperty] protected int iWeight;                          // 무게
        [JsonProperty] protected AttributeType eAttribute;             // 고유 속성


        /** 아이템 업그레이드 정보 **/
        [JsonProperty] protected int iEnhanceNum;                      // 무기 강화 횟수
        [JsonProperty] protected bool bIsAttrUnlocked;                 // 속성 해방 여부       
        [JsonProperty] protected int iRemainEngraveNum;                // 남은 각인 횟수 
        [JsonProperty] protected int iMaxEngraveNum;                   // 최대 각인 횟수 (무기 분류 등급에 따른 제한)
        [JsonProperty] protected ItemEngraving[] sArrEquipEngrave;     // 장착 중인 각인
        [JsonProperty] protected float fBasePerformance;               // 장비 기본 성능 (일반 무기-100, 제작무기-제작에따른 변화)
                    


        /// <summary>
        /// 무기 아이템 소분류 타입 - Sword, Blade, Spear, Dagger 등이 있습니다.
        /// </summary>
        [JsonIgnore] public WeaponType WeaponType { get { return eWeaponType; } }
        /// <summary>
        /// 무기 아이템의 기본 분류 타입 - 초급, 중급, 고급
        /// </summary>
        [JsonIgnore] public ItemGrade BasicGrade { get { return eBasicGrade; } }
        
        /// <summary>
        /// 남은 각인 횟수를 자동으로 연산해서 반환합니다.
        /// </summary>
        [JsonIgnore] public int RemainEngraveNum { get{ return iRemainEngraveNum; } }
        
        /// <summary>
        /// 실제 각인이 장착된 갯수입니다.
        /// </summary>
        [JsonIgnore] public int EquipEngraveNum { get{ return iMaxEngraveNum-iRemainEngraveNum; } }
        
        /// <summary>
        /// 현재 장착중인 각인 배열 정보를 반환합니다. <br/>
        /// 빈 각인의 경우에도 구조체가 할당되어 있으므로, 각인 배열의 갯수는 EquipEngraveNum을 사용해서 확인하여야 합니다.
        /// </summary>
        [JsonIgnore] public ItemEngraving[] EquipEngraveArrInfo
        {
            get{ return sArrEquipEngrave;} 
        }

        /// <summary>
        /// 무기에 각인을 장착하는 메서드 입니다. 장착하고자 할 각인 이름이 필요하며, 남아있는 각인 슬롯이 없다면 false를 반환합니다.
        /// </summary>
        public bool Engrave(string engravingName)
        {
            if(iRemainEngraveNum==0)
                return false;

            sArrEquipEngrave[EquipEngraveNum] = new ItemEngraving(engravingName);
            iRemainEngraveNum--;
            return true;
        }




        /// <summary>
        /// 아이템의 기본 성능입니다. 기본값은 100이며, 제작무기의 경우 이 성능에 변동을 주어야 합니다.
        /// </summary>
        [JsonIgnore] public float BasePerformance { get{ return fBasePerformance; } set{ fBasePerformance=value; } }

        /// <summary>
        /// 아이템의 최종 성능을 자동으로 연산하여 반환합니다. 기본 성능에 속성(1.5f)과 각인(1.1f,1.15f,1.2f)을 각각 독립 연산으로 곱한 수치입니다.
        /// </summary>
        [JsonIgnore] public float LastPerformance {
            get{ 
                float iLastMult = fBasePerformance; // 기본성능에서
                float attributeMult = bIsAttrUnlocked? 1f : 1.5f;    // true 일 때(잠겨있을 때) 1
                
                iLastMult *= attributeMult;         // 속성석의 배율을 곱한다.
                               
                for(int i=0; i<EquipEngraveNum; i++)
                    iLastMult *= sArrEquipEngrave[i].PerformMult;  // 각 각인석의 배율을 곱한다.

                return iLastMult;                 
               }            
         }
        
        /// <summary>
        /// 장비의 최종 성능에 따른 최종 등급입니다. ( 노말, 매직, 레어, 에픽, 유니크, 레전드 )<br/>
        /// </summary>
        public Rarity LastGrade
        {
            get{                
                if( LastPerformance>1800f)
                    return Rarity.Legend;                
                else if( LastPerformance>1300f )
                    return Rarity.Unique;                
                else if( LastPerformance>800f )
                    return Rarity.Epic;            
                else if( LastPerformance>500f )
                    return Rarity.Rare;            
                else if( LastPerformance>300f )
                    return Rarity.Magic;
                else if( LastPerformance>=100f )
                    return Rarity.Normal;
                else
                    Debug.Log("최종수치가 100미만 입니다. 확인하여 주세요.");
                
                return Rarity.Normal;
            }
        }



        
        /// <summary>
        /// 공격력은 최종성능의 백분율과 1:1관계를 가집니다.
        /// </summary>
        [JsonIgnore] public int Power { get{return (int)Mathf.Round(iPower*LastPerformance/100f);} }

        /// <summary>
        /// 내구도는 최종성능의 백분율과 1:0.5관계를 가집니다. 
        /// </summary>
        [JsonIgnore] public int Durability { get{return (int)Mathf.Round(iPower*LastPerformance/200f);} }
        [JsonIgnore] public float Speed { get{return fSpeed;} }
        [JsonIgnore] public int Weight { get{return iWeight;} }  

        /// <summary>
        /// 무기의 고유 속성입니다. 개방 여부와 상관이 없습니다.
        /// </summary>
        [JsonIgnore] public AttributeType OrinalAttribute { get{return eAttribute;} }  

        /// <summary>
        /// 현재 무기의 속성입니다. 개방되지 않았다면 무속성입니다.
        /// </summary>
        [JsonIgnore] public AttributeType CurrentAttribute { 
            get{ 
                if(bIsAttrUnlocked)
                    return AttributeType.None;
                else
                    return eAttribute;                
                }             
        }
                
        /// <summary>
        /// 장비의 속성개방 여부를 볼 수 있습니다.
        /// </summary>
        [JsonIgnore] public bool IsAttrUnlocked { get{ return bIsAttrUnlocked; } }
                


        public bool AttributeUnlock(AttributeType attribute)
        {
            if(IsAttrUnlocked)          // 무기가 이미 해방되어있다면 실행하지 않음.
                return false;

            if(attribute == eAttribute) // 들어온 속성이 고유속성과 일치한다면 성공
            {
                bIsAttrUnlocked = true;
                return true;
            }
            else                        // 들어온 속성이 고유속성과 일치하지 않는다면 실패.
                return false;           
        }






        /// <summary>
        /// 장비 아이템의 현재 강화 횟수입니다. 플레이어의 상호작용에 의해 수정이 가능합니다.
        /// </summary>
        [JsonIgnore] public int EnhanceNum { 
            get{return iEnhanceNum;}
            set{
                    if( 0<=iEnhanceNum && iEnhanceNum<=9 )  //강화의 수정 허용은 0에서 9까지
                        iEnhanceNum = value;
               }
        }


        /// <summary>
        /// 무기 아이템의 최초 생성을 위한 생성자 옵션
        /// </summary>
        public ItemWeapon(ItemType mainType, WeaponType subType, string No, string name, float price, ImageReferenceIndex imgRefIdx // 아이템 기본 정보 
            ,ItemGrade basicGrade, int power, int durability, float speed, int weight, AttributeType attribute                      // 무기 고유 정보
        ) : base( mainType, No, name, price, imgRefIdx )
        {
            eWeaponType = subType;
            eBasicGrade = basicGrade;
            iPower = power;
            iDurability = durability;
            fSpeed = speed;
            iWeight = weight;
            eAttribute = attribute;

            iEnhanceNum = 0;                                        // 기본으로 강화 되어있지 않음.
            bIsAttrUnlocked = true;                                  // 기본으로 속성이 잠겨있음.
        
            iMaxEngraveNum = (int)basicGrade + 1;                   // 최대 각인횟수는 변하지 않음 (초급:1, 중급:2, 고급:3)
            iRemainEngraveNum = iMaxEngraveNum;                     // 남은 각인횟수는 각인을 끼울 시 하나 씩 줄어듬
            sArrEquipEngrave = new ItemEngraving[iMaxEngraveNum];  // 최초 각인 배열의 크기는 최대 각인 횟수와 동일(기본 등급 별 제한)
            fBasePerformance = 100f;                                // 제작아이템이 아닌 경우 기본 성능 100f
        }

    }
}
