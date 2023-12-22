using Newtonsoft.Json;
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
 * <v1.1 - 2023_1109_최원준>
 * 1- 강화석을 추가하기 위해 MiscType의 Attribute를 Enhancement로 통일
 * 
 * <v2.0 - 2023_1112_최원준>
 * 1- 인벤토리 수량이 99를 초과한 경우 예외를 발생시키는 것이 아니라 새로운 슬롯에 생성되게 수정하였음.
 * 그에 따라 CreateManager의 Create메서드도 수정
 * 
 * <v2.1 - 2023_1115_최원준>
 * 1- 잡화 아이템의 최대 수량을 static 클래스 변수 MaxCount로 선언 및 주석 및 설명 수정
 * 
 * <v2.2 - 2023_1116_최원준>
 * 1- 아이템 클래스 구조 변경으로 인한 생성자내부의 이미지 참조 변수를 이미지 인덱스 변수로 수정
 * 
 * <v3.0 - 2023_1216_최원준>
 * 1- MiscType EnumMiscType 변수명을 eMiscType으로 수정 
 * 
 * 2- MiscType에 Attribute 추가. 이유는 속성석 아이템이 만들어질 때 속성정보를 넣고 싶기 때문
 * 
 * 3- AttributeType eAttributeType 변수를 추가하고 생성자에서 속성석 정보를 받을 때마다 분기해서 들어가도록 하였음.
 * 
 * 4- 각인석 정보 구조체에서 상태창 이미지 인덱스 정보를 가지도록 하고,
 * 아이템 생성자에서 각인석이 일 때 상태창 이미지 인덱스를 추가로 받도록 수정하였음 
 * (이유는 상태창을 보여 줄 때 각인석 정보 구조체에 해당 이미지 정보를 갖고 있지 않으면 이름을 통해서 일일이 이미지를 찾아야 하기 때문
 * 
 * 5- 각인석 정보 구조체의 생성자를 이름만 받는 생성자와 이름과 상태창 이미지 인덱스를 받는 생성자 둘로 나누었음.
 * 이유는 ItemMisc에서는 직접 이미지 인덱스를 넣어줄 수 있지만, ItemWeapon의 Engrave 기능에서는 이름만 줄 수 있기 때문
 * 
 * 
 * <v4.0 - 2023_1219_최원준>
 * 1- 중첩변수와 프로퍼티 iInventoryCount와 InventoryCount를 iOverlapCount, OverlapCount로 이름변경
 * 2- static변수이던 MaxCount를 readonly로 변경
 * 3- 아이템 초과갯수 
 * 
 * <v4.1 - 2023_1220_최원준>
 * 1- 중첩수량을 지정하는 메서드 SetOverlapCount 추가하여 수량을 입력 시 초과수량을 반환받도록 하였음
 * 
 * <v5.0 - 2023_1222_최원준>
 * 1- private변수를 직렬화하기 위해 [JsonProperty] 어트리뷰트를 추가하였음
 * 2- SetOverlapCount 메서드 내부 조건식 ==0에서 <=0으로 수정
 */


namespace ItemData
{   
    
    /// <summary>
    /// 잡화 아이템의 상세 분류
    /// </summary>
    public enum MiscType { Basic, Additive, Fire, Attribute, Enhancement, Engraving, Etc } // 기본재료, 추가재료, 연료, 속성석, 강화석, 각인석, 기타
        
    /// <summary>
    /// 각인석의 종류 - 물리,공속,흡혈,사격,피해
    /// </summary>
    public enum StatType { Power, Speed, Leech, Range, Splash }
    
    /// <summary>
    /// 잡화 아이템 - 기본 아이템과 다른점은 인벤토리에 중복해서 쌓을 수 있다는 점 (count가 존재)
    /// </summary>
    [Serializable]
    public sealed class ItemMisc : Item
    {        
        [JsonProperty] private int iOverlapCount = 0;  // 인벤토리 중첩 횟수
        public readonly int MaxCount = 99;
        
        /// <summary>
        /// 잡화 아이템의 중첩횟수를 표시합니다. 기본 값은 1입니다.<br/>
        /// **해당 프로퍼티를 통해 중첩횟수를 설정할 시 ItemMisc.MaxCount를 초과하면 예외를 발생시킵니다.**
        /// SetOverlapCount를 통해 남은 수량을 반환받아야 합니다.
        /// </summary>
        public int OverlapCount { 
            set { 
                    iOverlapCount += value;         // 값이 들어오면 누적시켜 줍니다.

                    if(iOverlapCount > MaxCount)    // 중첩횟수가 최대갯수보다 크다면
                        throw new Exception("최대 수량에 도달하였습니다.");
                }
            get { return iOverlapCount; } //중첩갯수를 반환합니다
        }


        /// <summary>
        /// 잡화 아이템의 중첩횟수를 설정합니다. 수량을 인자로 넣으면 초과수량을 반환하여 줍니다
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public int SetOverlapCount(int count)
        {
            int remainCount = iOverlapCount+count-MaxCount; // 반환되는 갯수 설정 : 기존 수량+들어온 수량-최대수량

            if( remainCount > 0 )                           // 들어온수량을 기존수량에 더했을 때 최대 수량을 초과한다면
            {
                iOverlapCount=MaxCount;                     // 현재 아이템의 갯수를 최대수량으로 맞춰줍니다
                return remainCount;                         // 나머지를 반환해줍니다
            }
            else if(remainCount<=0)                         // 반환할 나머지가 없다면
            {
                iOverlapCount += count;                     // 기존수량에 들어온 수량을 더해줍니다
            }

            return 0;                                       // 0을 반환합니다
        }




        /// <summary>
        /// 잡화아이템 소분류 타입 - Basic, Additive, Fire 등이 있습니다.
        /// </summary>
        public MiscType eMiscType { get; }

        /// <summary>
        /// 잡화 아이템의 종류가 각인이라면 engraveInfo를 가집니다. 이 변수에 접근하여 추가 정보를 확인가능합니다.
        /// </summary>
        public ItemEngraving EngraveInfo {get;}

        /// <summary>
        /// 잡화 아이템의 종류가 속성석이라면 eAttributeType을 가집니다.
        /// </summary>
        public AttributeType eAttributeType { get; }


        /// <summary>
        /// 잡화 아이템의 종류가 연료라면 연료 고유의 화력을 가집니다. 연료가 아니라면 0입니다.
        /// </summary>
        public int FirePower {get;}

        public ItemMisc( ItemType mainType, MiscType subType, string No, string name, float price, ImageReferenceIndex imgRefIdx )
            : base( mainType, No, name, price, imgRefIdx ) 
        { 
            iOverlapCount = 1;
            eMiscType = subType;
            FirePower = 0;

            if(subType == MiscType.Engraving)           // 각인석이라면 이름과 상태창 이미지 인덱스를 넣어서 구조체 정보를 가지게 합니다.
                EngraveInfo = new ItemEngraving(name, imgRefIdx.statusImgIdx); 
            else if( subType == MiscType.Attribute )    // 속성석이라면 '-'이후의 이름을 참고하여 정보를 가지게 합니다 
            {
                switch( name.Split('-')[1] )
                {
                    case "수":
                        eAttributeType = AttributeType.Water;
                        break;
                    case "금":
                        eAttributeType = AttributeType.Gold;
                        break;                    
                    case "지":
                        eAttributeType = AttributeType.Earth; 
                        break;                        
                    case "화":
                        eAttributeType = AttributeType.Fire; 
                        break;
                    case "풍":
                        eAttributeType = AttributeType.Wind; 
                        break;
                    default:
                        throw new Exception("속성석의 이름이 적절치 않습니다.");
                }
            }
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
                        FirePower = 10;
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
    [Serializable]
    public struct ItemEngraving             
    {
        [JsonProperty] private float fIncreaseValue;        // 증가 수치
        [JsonProperty] private float fMultiplier;           // 증가 배율
        
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
        /// 각인이 가지는 상태창 이미지의 인덱스 번호
        /// </summary>
        public int StatusImageIdx {get;}


        /// <summary>
        /// 상태창 이미지 인덱스를 인자로 주어 각인 정보 구조체를 생성합니다.
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="statusImageIndex"></param>
        public ItemEngraving( string fullName, int statusImageIndex ) : this(fullName)            
        {
            StatusImageIdx = statusImageIndex; // 상태창 이미지 인덱스 설정                         
        }

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
                    StatusImageIdx = 0; // 상태창 이미지 인덱스 직접 설정
                    break;
                case "공속" :
                    StatusType = StatType.Speed;
                    fIncreaseValue = 0.1f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"공격 속도 {LastIncrVal}% 증가" );
                    StatusImageIdx = 1; // 상태창 이미지 인덱스 직접 설정
                    break;
                case "흡혈" :
                    StatusType = StatType.Leech;
                    fIncreaseValue = 0.1f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"흡혈량 {LastIncrVal}% 증가" );
                    StatusImageIdx = 2; // 상태창 이미지 인덱스 직접 설정
                    break;
                case "사격" :
                    StatusType = StatType.Range;
                    fIncreaseValue = 0.15f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"공격 사거리 {LastIncrVal}% 증가" );
                    StatusImageIdx = 3; // 상태창 이미지 인덱스 직접 설정
                    break;
                case "피해" :
                    StatusType = StatType.Splash;
                    fIncreaseValue = 0.1f;
                    LastIncrVal = fIncreaseValue * fMultiplier;
                    Desc = string.Format( $"범위 피해 {LastIncrVal}% 증가" );
                    StatusImageIdx = 4; // 상태창 이미지 인덱스 직접 설정
                    break;
                    
                default :
                    throw new Exception("각인석의 이름에 일치하는 종류가 없습니다. 물리,공속,흡혈,사격,피해");
            }

        }
    }







}
