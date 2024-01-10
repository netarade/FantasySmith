using Newtonsoft.Json;
using System;

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
 * 
 * <v5.1 - 2023_1224_최원준>
 * 1- Clone메서드 삭제 (Item클래스에서 같은 기능을 상속하므로)
 * 
 * <v6.0 - 2023_1226_최원준>
 * 1- OverlapCount 프로퍼티가 중첩갯수를 누적연산(+=)하던 것에서 대입연산(=)으로 변경
 * 세이브하고 로드시 역직렬화 메서드가 호출되면서 값ㅇ르 넣어줄 때 해당 프로퍼티의 호출이 이루어 지는데 
 * 이때 값이 iOverlapCount를 읽은 다음 다시 누적해버리기 때문에 2배가 되어버리는 현상이 발생하기 때문
 * 
 * 2- 자동구현프로퍼티 변수를 내부변수 하나 더 만들고 일반프로퍼티로 변경 및 JsonProperty와 JsonIgnore처리
 * (프로퍼티는 저장공간 낭비 및 로드시 프로퍼티는 set이 없기 때문에 정보가 반영되지 않기 때문)
 * 
 * <v6.1 - 2023_1229_최원준>
 * 1- 파일명 변경 ItemData_Misc->ItemMisc
 * 
 * <v6.2 - 2023_1230_최원준>
 * 1- MaxCount를 MaxOverlapCount로 이름변경
 * 
 * <v6.3 - 2023_1231_최원준>
 * 1- OverlapCount프로퍼티를 읽기전용 변수로 설정하고 수량을 조절할 때 SetOverlapCount 메서드를 호출하도록 변경
 * 2- SetOverlapCount메서드를 음의인자를 받도록 수정, 매개변수 count를 inCount로 수정
 * 
 * <v6.4 - 2024_0108_최원준>
 * 1- ItemImageCollection을 ItemVisualCollection명칭 변경관계로
 * 생성자의 매개변수명 imgRefIndex를 visualRefIndex 변경
 * 
 * <v7.0 - 2024_0111_최원준>
 * 1- 크래프팅 장르에 맞게 클래스 설계 변경
 * 2- 상세 분류 타입 및 퀘스트 아이템 상속 클래스 추가
 * 
 */


namespace ItemData
{   
    
    /// <summary>
    /// 잡화 아이템의 상세 분류
    /// </summary>
    public enum MiscType { Basic, Essential, Craft, Building, Tool, Potion } // 기본, 필수, 제작, 건설, 도구, 포션
        

    /// <summary>
    /// 퀘스트 아이템 - 잡화 아이템을 상속합니다
    /// </summary>
    [Serializable]
    public class ItemQuest : ItemMisc
    {
        public ItemQuest( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, MiscType subType, string desc )
            : base( mainType, No, name, visualRefIndex, subType, desc ) 
        { 
            
        }
    }


    /// <summary>
    /// 잡화 아이템 - 기본 아이템과 다른점은 인벤토리에 중복해서 쌓을 수 있다는 점 (count가 존재)
    /// </summary>
    [Serializable]
    public class ItemMisc : Item
    {        
        [JsonProperty] private int iOverlapCount = 0;   // 인벤토리 중첩 횟수        
        [JsonProperty] MiscType eMiscType;              // 서브타입 (잡화 소분류 타입)

        [JsonIgnore] public readonly int MaxOverlapCount = 99; // 잡화 아이템 최대 갯수


        /// <summary>
        /// 잡화 아이템의 중첩 갯수를 알려줍니다. <br/>
        /// 읽기전용이므로, 수량을 조절하기 위해서는 SetOverlapCount메서드를 호출하여야 합니다. <br/>
        /// </summary>
        [JsonIgnore] public int OverlapCount { get { return iOverlapCount; } }

        /// <summary>
        /// 잡화 아이템의 중첩횟수를 설정합니다.<br/>
        /// 음의 인자가 전달되면 기존의 수량을 감산하고, 양의 인자가 전달되면 수량을 가산합니다.<br/><br/>
        /// 
        /// 양의 인자가 전달되었을 때 최대수량 이상을 초과하는 경우 해당 초과 수량을 반환하여 줍니다<br/>
        /// 음의 인자가 전달되었을 때 더이 상 감산할 수 없는 경우(기존 수량이 0이된 경우) 나머지 초과 수량을 반환합니다.<br/>
        /// </summary>
        /// <param name="inCount"></param>
        /// <returns>인자로 전달된 수량이 아이템이 가질 수 있는 최대, 최소 수량을 초과하는 경우 남은 수량 인자를 반환합니다.</returns>
        public int SetOverlapCount(int inCount)
        {
            int remainCount;

            if( inCount>0 )     // 수량 인자로 양의 값이 들어온 경우
            {
                // 반환되는 갯수 설정 : 기존 수량+들어온 수량-최대수량
                remainCount = iOverlapCount + inCount - MaxOverlapCount; 

                if( remainCount > 0 )                   // 들어온수량을 기존수량에 더했을 때 최대 수량을 초과한다면
                {
                    iOverlapCount = MaxOverlapCount;    // 현재 아이템의 갯수를 최대수량으로 맞춰줍니다
                    return remainCount;                 // 나머지 수량을 반환합니다.
                }
                else if(remainCount<=0)                 // 반환할 나머지가 없다면
                {
                    iOverlapCount += inCount;           // 기존수량에 들어온 수량을 더해줍니다
                    return 0;                           // 0을 반환합니다
                }
            }
            else if( inCount<0 )    // 수량 인자로 음의 값이 들어온 경우
            {
                // 반환되는 갯수 설정 : 기존수량 - 뺄수량
                remainCount = iOverlapCount + inCount;

                if( remainCount>=0 )            // 반환할 나머지가 없는 경우
                {
                    iOverlapCount += inCount;   // 기존수량에 들어온 수량을 빼줍니다.
                    return 0;                   // 0을 반환합니다.
                }
                else if( remainCount<0 )        // 반환할 나머지가 있는 경우
                {
                    iOverlapCount = 0;          // 기존수량을 0으로 만들어줍니다.
                    return remainCount;         // 나머지 수량을 반환합니다.
                }
            }
            
            return 0;
        }



        
        /// <summary>
        /// 잡화아이템 소분류 타입 - Basic, Additive, Fire 등이 있습니다.
        /// </summary>
        [JsonIgnore] public MiscType MiscType { get{ return eMiscType; } }


        public ItemMisc( ItemType mainType, string No, string name, VisualReferenceIndex visualRefIndex, MiscType subType, string desc )
            : base( mainType, No, name, visualRefIndex, desc )
        { 
            iOverlapCount = 1;
            eMiscType = subType;            
        }

        
    }



    







}
