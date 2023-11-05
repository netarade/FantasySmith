using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1105_최원준>
 * 1- ItemMisc 클래스 변수 간소화 - 프로퍼티 처리
 * 2- 각인을 나타내는 구조체 ItemEngraving 추가
 * 3- ItemMisc 클래스 내부에 ItemEngraving과 FirePower에 대한 정보를 포함하도록 하였음.
 * 아이템 생성 시 입력 한 세부 타입과 이름에 따라 정보를 알아서 집어넣도록 수정.
 * 
 */


namespace ItemData
{   
    
    /// <summary>
    /// 잡화 아이템의 상세 분류
    /// </summary>
    public enum MiscType { Basic, Additive, Fire, Attribute, Engraving, Etc } // 기본재료, 추가재료, 연료, 속성석, 각인석, 기타
        
    /// <summary>
    /// 각인석의 종류 - 물리,공속,흡혈,사격,피해
    /// </summary>
    public enum StatType { Power, Speed, Leech, Range, Splash }
    

    /// <summary>
    /// 잡화 아이템 - 기본 아이템과 다른점은 인벤토리에 중복해서 쌓을 수 있다는 점 (count가 존재)
    /// </summary>
    public sealed class ItemMisc : Item
    {        
        private int iInventoryCount;  // 인벤토리 중첩 횟수

        /// <summary>
        /// 잡화 아이템의 중첩횟수를 표시합니다. 기본 값은 1입니다.<br/>
        /// **인벤토리 수량이 99를 초과하면 예외를 발생시킵니다.**
        /// </summary>
        public int InventoryCount { 
            set { 
                    iInventoryCount = value; 

                    if(iInventoryCount>99)
                        throw new Exception("인벤토리 수량이 99를 초과하였습니다.");                    
                }
            get { return iInventoryCount; }
        }

        /// <summary>
        /// 잡화아이템 소분류 타입 - Basic, Additive, Fire 등이 있습니다.
        /// </summary>
        public MiscType EnumMiscType { get; }

        /// <summary>
        /// 잡화 아이템의 종류가 각인이라면 engraveInfo를 가집니다. 이 변수에 접근하여 추가 정보를 확인가능합니다.
        /// </summary>
        public ItemEngraving EngraveInfo {get;}

        /// <summary>
        /// 잡화 아이템의 종류가 연료라면 연료 고유의 화력을 가집니다. 연료가 아니라면 0입니다.
        /// </summary>
        public int FirePower {get;}

        public ItemMisc( ItemType mainType, MiscType subType, string No, string name, float price, ImageCollection image )
            : base( mainType, No, name, price, image ) 
        { 
            iInventoryCount = 1;
            EnumMiscType = subType;
            FirePower = 0;

            if(subType == MiscType.Engraving)           // 각인석이라면 이름을 그대로 넣어서 구조체 정보를 가지게 합니다.
                EngraveInfo = new ItemEngraving(name); 
            else if( subType == MiscType.Fire )         // 연료라면 화력을 조정합니다.
            {
                switch(name)
                {
                    case "나무" :
                        FirePower = 1;
                        break;
                    case "석탄" :
                        FirePower = 5;
                        break;
                    case "석유" :
                        FirePower = 20;
                        break;
                }
            }
        }

        /// <summary>
        /// 잡화 아이템의 객체를 쉽게 복제하여 줍니다.
        /// </summary>
        public override object Clone()
        {
            return this.MemberwiseClone();
        }
    }



    /// <summary>
    /// 각인석 - 무기를 각인시킬 때 사용
    /// </summary>
    public struct ItemEngraving             
    {
        private float fIncreaseValue;        // 증가 수치
        private float fMultiplier;           // 증가 배율
        
        /// <summary>
        /// 각인의 전체 이름 (ex. 초급 물리의 각인)
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 각인 등급 - 초급,중급,고급
        /// </summary>
        public ItemGrade Grade { get; }              

        /// <summary>
        /// 각인 종류 - 물리,공속,흡혈,사격,피해
        /// </summary>
        public StatType StatusType { get; }
        
        /// <summary>
        /// 각인 별 해당 스텟 최종 증가 수치
        /// </summary>
        public float LastIncrVal { get; }           


        /// <summary>
        /// 상태창에 표기 될 설명
        /// </summary>
        public string Desc { get; }                 

        /// <summary>
        /// 각인 등급 별 장비 아이템의 최종 성능 증가율 1.1f, 1.15f, 1.2f의 값을 가집니다.
        /// </summary>
        public float PerformMult { get; }               
        


        /// <summary>
        /// 원하는 각인 이름을 지정하여 생성하면 모든 정보를 데이터 테이블에 맞게 초기화하여 줍니다.<br/>
        /// * 생성 시 일치하는 이름이 아니라면 예외를 던집니다. *
        /// </summary>
        public ItemEngraving(string fullName)
        {
            Name = fullName;
            string strGrade = fullName.Split(" ")[0];                   // 풀네임의 첫부분
            string strType = fullName.Split(" ")[1].Substring(0, 2);    // 풀네임의 둘째 부분에서 '의'를 제외한 앞 두글자
            
            switch( strGrade )
            {
                case "초급" :
                    Grade = ItemGrade.Low;
                    fMultiplier = 1;
                    PerformMult = 1.1f;
                    break;
                
                case "중급":
                    Grade = ItemGrade.Medium;
                    fMultiplier = 2;
                    PerformMult = 1.15f;
                    break;
                case "고급":
                    Grade = ItemGrade.High;
                    fMultiplier = 3;
                    PerformMult = 1.2f;
                    break;
                    
                default :
                    throw new Exception("각인석의 이름에 일치하는 등급이 없습니다. 초급,중급,고급");
            }

            switch(strType)
            {
                case "물리" :
                    StatusType = StatType.Power;
                    fIncreaseValue = 10f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"물리 피해 {LastIncrVal} 증가" );
                    break;
                case "공속" :
                    StatusType = StatType.Speed;
                    fIncreaseValue = 0.1f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"공격 속도 {LastIncrVal}% 증가" );
                    break;
                case "흡혈" :
                    StatusType = StatType.Leech;
                    fIncreaseValue = 0.1f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"흡혈량 {LastIncrVal}% 증가" );
                    break;
                case "사격" :
                    StatusType = StatType.Range;
                    fIncreaseValue = 0.15f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"공격 사거리 {LastIncrVal}% 증가" );
                    break;
                case "피해" :
                    StatusType = StatType.Splash;
                    fIncreaseValue = 0.1f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"범위 피해 {LastIncrVal}% 증가" );
                    break;
                    
                default :
                    throw new Exception("각인석의 이름에 일치하는 종류가 없습니다. 물리,공속,흡혈,사격,피해");
            }

        }
    }







}
