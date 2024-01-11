
/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1105_최원준>
 * 1- 제작무기(ItemCraftWeapon) 클래스 파일 분리.
 * 2- 각 종 기본 변수 및 프로퍼티, 생성자와 메서드 구현
 * 3- 주석처리
 * 
 * <v1.1 - 2023_1106_최원준>
 * 1- CraftMaterial 구조체 생성자 추가
 * 2- 각 속성의 정보를 볼 수 있는 프로퍼티 추가
 * 3- 프로퍼티 명 수정 
 * 
 * <v1.2 - 2023_1116_최원준>
 * 1- 아이템 클래스 구조 변경으로 인한 생성자내부의 이미지 참조 변수를 이미지 인덱스 변수로 수정
 * 
 * <v2.0 - 2023_1222_최원준>
 * 1- private변수를 직렬화하기 위해 [JsonProperty] 어트리뷰트를 추가하였음
 * 
 * <v2.1 - 2023_1224_최원준>
 * 1- Clone메서드 삭제 (Item클래스에서 같은 기능을 상속하므로)
 * 
 * <v3.0 - 2023_1226_최원준>
 * 1- 프로퍼티 모두 JsonIgnore처리
 * (프로퍼티는 저장공간 낭비 및 로드시 프로퍼티는 set이 없기 때문에 정보가 반영되지 않기 때문)
 * 2- cmArr 변수를 sArr명칭으로 변경
 * 
 * <v3.1 - 2023_1229_최원준>
 * 1- 파일명 변경 ItemData_CraftWeapon->ItemCraftWeapon
 * 
 * <v3.2 - 2024_0108_최원준>
 * 1- ItemImageCollection을 ItemVisualCollection명칭 변경관계로
 * 생성자의 매개변수명 imgRefIndex를 visualRefIndex 변경
 * 
 * <v4.0 - 2024_0111_최원준>
 * 1- 크래프팅 장르에 맞게 클래스 설계 변경으로 인한 주석처리
 * 
 */

//namespace ItemData
//{
//    /// <summary>
//    /// 무기 제작에 필요한 재료 - 재료 이름과 갯수
//    /// </summary>
//    [Serializable]
//    public struct CraftMaterial
//    {
//        public string name;
//        public int count;

//        public CraftMaterial(string name, int count)
//        {
//            this.name = name;
//            this.count = count;
//        }
//    }
        
//    /// <summary>
//    /// 레시피의 종류 - ㄱ, ㄴ, ㄷ, ㄹ, ㅁ, ㅡ, ㅣ
//    /// </summary>
//    public enum Recipie { Ga,Na,Da,Ra,Ma,Eu,Ee }


    
//    /// <summary>
//    /// 제작 무기 아이템
//    /// </summary>
//    [Serializable]
//    public sealed class ItemCraftWeapon : ItemWeapon
//    {
//        /*** 제작 관련 속성 ***/
//        [JsonProperty] private float fCraftChance;                 // 제작 확률 (기본 등급에 따라 결정 90%, 60%, 25%)
//        [JsonProperty] private Recipie enumRecipie;                        // 2단계 레시피
//        [JsonProperty] private CraftMaterial[] sArrBaseMaterial;          // 제작 시 필요한 기본 재료
//        [JsonProperty] private CraftMaterial[] sArrAdditiveMaterial;      // 제작 시 필요한 추가 재료
//        [JsonProperty] private int iFirePower;                   // 2단계 제작에 필요한 화력

//        /// <summary>
//        /// 제작 무기의 기본 제작 확률입니다.
//        /// </summary>
//        [JsonIgnore] public float BaseCraftChance { get{ return fCraftChance; } }

//        /// <summary>
//        /// 제작 무기의 레시피 입니다.
//        /// </summary>
//        [JsonIgnore] public Recipie EnumRecipie { get{ return enumRecipie; } }
        
//        /// <summary>
//        /// 제작 무기의 기본 재료입니다. 2단계 제작에 사용되며, 구조체 배열을 참조하고 있습니다.
//        /// </summary>
//        [JsonIgnore] public CraftMaterial[] BaseMaterials { get{ return sArrBaseMaterial; } }

//        /// <summary>
//        /// 제작 무기의 추가 재료입니다. 3단계 제작에 사용되며, 구조체 배열을 참조하고 있습니다.
//        /// </summary>
//        [JsonIgnore] public CraftMaterial[] AdditiveMaterals { get{ return sArrAdditiveMaterial; } }

//        /// <summary>
//        /// 제작에 필요한 화력입니다. 2단계에 제작에 사용됩니다.
//        /// </summary>
//        [JsonIgnore] public int FirePower { get{ return iFirePower; } }


//        public ItemCraftWeapon(ItemType mainType, WeaponType subType, string No, string name, float price, VisualReferenceIndex visualRefIndex // 아이템 기본 정보 
//            , ItemGrade basicGrade, int power, int durability, float speed, int weight, AttributeType attribute                         // 무기 고유 정보
//            , CraftMaterial[] baseMaterial, CraftMaterial[] additiveMaterial, Recipie recipie                                           // 제작 관련 정보   
//        ) : base( mainType, subType, No, name, price, visualRefIndex, basicGrade, power, durability, speed, weight, attribute )
//        {          

//            /*** 제작 관련 정보 ***/
//            switch(basicGrade)
//            {
//                case ItemGrade.Low :
//                    fCraftChance = 0.9f;
//                    break;
//                case ItemGrade.Medium :
//                    fCraftChance = 0.6f;
//                    break;
//                case ItemGrade.High :
//                    fCraftChance = 0.25f;
//                    break;
//            }

//            sArrBaseMaterial = baseMaterial;
//            sArrAdditiveMaterial = additiveMaterial;
//            enumRecipie = recipie;
            
//            iFirePower = 0;
//            for(int i=0; i<baseMaterial.Length; i++)
//                iFirePower += baseMaterial[i].count;        // 2단계 화력 - 기본 재료의 수에 따라 결정 
//        }

        
//    }

//}
