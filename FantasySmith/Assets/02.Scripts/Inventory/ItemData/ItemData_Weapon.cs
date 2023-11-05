using System;
using System.Collections;
using System.Collections.Generic;
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
 * 1- System 자료형과 겹치는 관계로 Attribute 를 EnumAttribute로 변경
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
    public enum EnumAttribute { Water, Gold, Earth, Fire, Wind, None }
   
    /// <summary>
    /// 장비의 최종 등급 - 노말, 매직, 레어, 에픽, 유니크, 레전드
    /// </summary>
    public enum Rarity { Normal, Magic, Rare, Epic, Unique, Legend }
    


    /// <summary>
    /// 일반 무기 아이템
    /// </summary>
    public class ItemWeapon : Item
    {       
        /*** 기본 고유 정보 ***/
        protected WeaponType enumWeaponType;    // 무기 소분류 타입
        protected ItemGrade enumBasicGrade;     // 기본 분류 등급 (초급, 중급, 고급)
        protected int iPower;                   // 공격력
        protected int iDurability;              // 내구
        protected float fSpeed;                 // 공격속도
        protected int iWeight;                  // 무게
        protected EnumAttribute enumAttribute;      // 고유 속성


        /** 아이템 업그레이드 정보 **/
        protected float iEnhanceNum;                    // 무기 강화 횟수
        protected bool isAttrUnlocked;                  // 속성 해방 여부       
        protected int iRemainEngraveNum;                   // 남은 각인 횟수 (무기 분류 등급에 따른 제한)
        protected ItemEngraving[] ieArrEquipEngrave;    // 장착 중인 각인
        protected float fBasePerformance;               // 장비 기본 성능 (일반 무기-100, 제작무기-제작에따른 변화)
                    
        
        /// <summary>
        /// 남은 각인 횟수를 자동으로 연산해서 반환합니다.
        /// </summary>
        public int RemainEngraveNum { get{ return iRemainEngraveNum; } }
        
        /// <summary>
        /// 현재 장착중인 각인 배열입니다.
        /// </summary>
        public ItemEngraving[] EquipEngrave { get{ return ieArrEquipEngrave;} }

        /// <summary>
        /// 무기에 각인을 장착하는 메서드 입니다. 장착하고자 할 각인 이름이 필요하며, 남아있는 각인 슬롯이 없다면 false를 반환합니다.
        /// </summary>
        public bool Engrave(string engravingName)
        {
            if(iRemainEngraveNum==0)
                return false;

            EquipEngrave[3-iRemainEngraveNum] = new ItemEngraving(engravingName);
            iRemainEngraveNum--;
            return true;
        }




        /// <summary>
        /// 아이템의 기본 성능입니다. 기본값은 100이며, 제작무기의 경우 이 성능에 변동을 주어야 합니다.
        /// </summary>
        public float BasePerformance { get{ return fBasePerformance; } set{ fBasePerformance=value; } }

        /// <summary>
        /// 아이템의 최종 성능을 자동으로 연산하여 반환합니다. 기본 성능에 속성(1.5f)과 각인(1.1f,1.15f,1.2f)을 각각 독립 연산으로 곱한 수치입니다.
        /// </summary>
        public float LastPerformance {
            get{ 
                float iLastMult = fBasePerformance; // 기본성능에서
                float attributeMult = isAttrUnlocked? 1f : 1.5f;
                
                iLastMult *= attributeMult;         // 속성석의 배율을 곱한다.

                for(int i=0; i<ieArrEquipEngrave.Length-RemainEngraveNum; i++)
                    iLastMult *= ieArrEquipEngrave[i].PerformMult;  // 각 각인석의 배율을 곱한다.

                return iLastMult;                 
               }            
         }
        
        /// <summary>
        /// 장비의 최종 성능에 따른 최종 등급입니다. ( 노말, 매직, 레어, 에픽, 유니크, 레전드 )<br/>
        /// </summary>
        public Rarity LastGrade
        {
            get{                
                if( LastPerformance<100)
                    Debug.Log("최종수치가 100미만 입니다. 확인하여 주세요.");

                if( LastPerformance>=100 )
                    return Rarity.Normal;
                else if( LastPerformance>300 )
                    return Rarity.Magic;
                else if( LastPerformance>500 )
                    return Rarity.Rare;
                else if( LastPerformance>800 )
                    return Rarity.Epic;
                else if( LastPerformance>1300 )
                    return Rarity.Unique;
                else
                    return Rarity.Legend;
            }
        }



        /// <summary>
        /// 무기 아이템 소분류 타입 - Sword, Blade, Spear, Dagger 등이 있습니다.
        /// </summary>
        public WeaponType EnumWeaponType { get {return enumWeaponType;} }
        /// <summary>
        /// 무기 아이템의 기본 분류 타입 - 초급, 중급, 고급
        /// </summary>
        public ItemGrade BasicGrade { get {return enumBasicGrade;} }

        /// <summary>
        /// 공격력은 최종성능의 백분율과 1:1관계를 가집니다.
        /// </summary>
        public int Power { get{return (int)Mathf.Round(iPower*LastPerformance/100f);} }

        /// <summary>
        /// 내구도는 최종성능의 백분율과 1:0.5관계를 가집니다. 
        /// </summary>
        public int Durability { get{return (int)Mathf.Round(iPower*LastPerformance/200f);} }
        public float Speed { get{return fSpeed;} }
        public int Weight { get{return iWeight;} }  

        /// <summary>
        /// 무기의 고유 속성입니다. 개방 여부와 상관이 없습니다.
        /// </summary>
        public EnumAttribute OriAttribute { get{return enumAttribute;} }  

        /// <summary>
        /// 현재 무기의 속성입니다. 개방되지 않았다면 무속성입니다.
        /// </summary>
        public EnumAttribute CurAttribute { 
            get{ 
                if(isAttrUnlocked)
                    return EnumAttribute.None;
                else
                    return enumAttribute;                
                }             
        }
                
        /// <summary>
        /// 장비의 속성개방 여부를 보거나 수정합니다.
        /// </summary>
        public bool IsAttrUnlocked { get{ return isAttrUnlocked; } set{ isAttrUnlocked=value; } }
                
        /// <summary>
        /// 장비 아이템의 현재 강화 횟수입니다. 플레이어의 상호작용에 의해 수정이 가능합니다.
        /// </summary>
        public float EnhanceNum { 
            get{return iEnhanceNum;}
            set{
                    if( 0<=iEnhanceNum && iEnhanceNum<=9 )  //강화의 수정 허용은 0에서 9까지
                        iEnhanceNum = value; 
               }
        }



        public ItemWeapon(ItemType mainType, WeaponType subType, string No, string name, float price, ImageCollection image   // 아이템 기본 정보 
            ,ItemGrade basicGrade, int power, int durability, float speed, int weight, EnumAttribute attribute                    // 무기 고유 정보
        ) : base( mainType, No, name, price, image )
        {
            enumWeaponType = subType;
            enumBasicGrade = basicGrade;
            iPower = power;
            iDurability = durability;
            fSpeed = speed;
            iWeight = weight;
            enumAttribute = attribute;

            iEnhanceNum = 0;                                            // 기본으로 강화 되어있지 않음.
            isAttrUnlocked = false;                                     // 기본으로 속성이 개방되어 있지 않음.
            iRemainEngraveNum = (int)basicGrade + 1;                    // 남은 각인횟수 (초급:1, 중급:2, 고급:3)
            ieArrEquipEngrave = new ItemEngraving[iRemainEngraveNum];   // 각인 배열의 크기는 남은 각인 횟수와 동일(기본 등급 별 제한)
            fBasePerformance = 100f;                                    // 제작아이템이 아닌 경우 기본 성능 100f
        }


        /// <summary>
        /// 무기 아이템의 객체를 쉽게 복제하여 줍니다.
        /// </summary>
        public override object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
