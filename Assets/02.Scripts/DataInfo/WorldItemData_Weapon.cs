using System.Collections.Generic;
using ItemData;

/*
 * 
* [작업 사항]
* 
* <v1.0 - 2023_1221_최원준>
* 1- CreateManager의 CreateAllItemDictionary메서드의 정보를 분리시켜 클래스 생성
* 
*/



namespace WorldItemData
{
    public partial class WorldItem
    {
        public Dictionary<string, Item> weaponDic=new Dictionary<string, Item>()
        {
            /*** 검 ***/
            { "철 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001000", "철 검", 10.0f, new ImageReferenceIndex(0)
                , ItemGrade.Low, 10, 100, 1.0f, 10, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("철",2)} 
                , new CraftMaterial[]{new CraftMaterial("점토",1)} 
                , Recipie.Eu) 
            },
            { "강철 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001001", "강철 검", 20.0f, new ImageReferenceIndex(1)
                , ItemGrade.Low, 12, 100, 1.0f, 18, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("강철",2), new CraftMaterial("철",1)}
                , new CraftMaterial[]{new CraftMaterial("점토",2)}
                , Recipie.Na)
            },
            { "미스릴 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001002", "미스릴 검", 40.0f, new ImageReferenceIndex(2) 
                , ItemGrade.Low, 18, 100, 1.0f, 10, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",2), new CraftMaterial("강철",1),new CraftMaterial("철",1)}
                , new CraftMaterial[]{new CraftMaterial("은",2)}
                , Recipie.Ma)
            
            },
            { "흑철 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001003", "흑철 검", 60.0f, new ImageReferenceIndex(3) 
                , ItemGrade.Low, 23, 100, 1.0f, 40, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("흑철",2), new CraftMaterial("강철",3)}
                , new CraftMaterial[]{new CraftMaterial("점토",3)}
                , Recipie.Ga)
            },
            { "프레첼", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001004", "프레첼", 80.0f, new ImageReferenceIndex(4) 
                , ItemGrade.Low, 25, 100, 1.0f, 20, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",2), new CraftMaterial("강철",2),new CraftMaterial("흑철",1)}
                , new CraftMaterial[]{new CraftMaterial("점토",2),new CraftMaterial("흑요석",1)}
                , Recipie.Da)
            },


            { "얼음칼날", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001100", "얼음칼날", 120.0f, new ImageReferenceIndex(5)
                , ItemGrade.Medium, 36, 150, 1.0f, 25, AttributeType.Water
                , new CraftMaterial[]{new CraftMaterial("미스릴",7)}
                , new CraftMaterial[]{new CraftMaterial("얼음 결정",5)}
                , Recipie.Da)
            },
            { "백은의 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001101", "백은의 검", 180.0f, new ImageReferenceIndex(6)
                , ItemGrade.Medium, 38, 150, 1.0f, 30, AttributeType.Wind
                , new CraftMaterial[]{new CraftMaterial("티타늄",2),new CraftMaterial("미스릴",4)}
                , new CraftMaterial[]{new CraftMaterial("백금",5)}
                , Recipie.Ga)
            },
            { "기사단의 검", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001102", "기사단의 검", 150.0f, new ImageReferenceIndex(7)
                , ItemGrade.Medium, 48, 150, 1.0f, 30, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("티타늄",1),new CraftMaterial("미스릴",3),new CraftMaterial("강철",2)}
                , new CraftMaterial[]{new CraftMaterial("금",3),new CraftMaterial("은",2)}
                , Recipie.Na)
            },
            { "아밍 소드", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001103", "아밍 소드", 110.0f, new ImageReferenceIndex(8) 
                , ItemGrade.Medium, 32, 150, 1.0f, 12, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("티타늄",1),new CraftMaterial("미스릴",3)}
                , new CraftMaterial[]{new CraftMaterial("금",2),new CraftMaterial("흑요석",2)}
                , Recipie.Ma)
            },
            { "하플랑", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001104", "하플랑", 100.0f, new ImageReferenceIndex(9) 
                , ItemGrade.Medium, 38, 150, 1.0f, 20, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",4),new CraftMaterial("강철",2)}
                , new CraftMaterial[]{new CraftMaterial("은",3),new CraftMaterial("크롬 결정",1)}
                , Recipie.Ee)
            },

            { "천공의 지배", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001200", "천공의 지배", 680.0f, new ImageReferenceIndex(10)
                , ItemGrade.High, 80, 200, 1.10f, 10, AttributeType.Wind
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",3),new CraftMaterial("티타늄",5),new CraftMaterial("미스릴",10)}
                , new CraftMaterial[]{new CraftMaterial("하늘의 룬",2), new CraftMaterial("수은 결정",5)}
                , Recipie.Da)
            },
            { "파멸의 불꽃", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001201", "파멸의 불꽃", 640.0f, new ImageReferenceIndex(11)
                , ItemGrade.High, 105, 200, 1.10f, 30, AttributeType.Fire
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",5),new CraftMaterial("티타늄",10),new CraftMaterial("미스릴",3)}
                , new CraftMaterial[]{new CraftMaterial("불타는 심장",1), new CraftMaterial("신비한 조각",5)}
                , Recipie.Ra)
            },
            { "듀란달", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001202", "듀란달", 480.0f, new ImageReferenceIndex(12) 
                , ItemGrade.High, 120, 200, 1.10f, 50, AttributeType.Wind
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",3),new CraftMaterial("티타늄",5),new CraftMaterial("흑철",8)}
                , new CraftMaterial[]{new CraftMaterial("금",2),new CraftMaterial("달의 조각",5)}
                , Recipie.Ma)
            },
            { "미스틸테인", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001203", "미스틸테인", 700.0f, new ImageReferenceIndex(13) 
                , ItemGrade.High, 95, 200, 1.10f, 25, AttributeType.Gold
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",5),new CraftMaterial("미스릴",5),new CraftMaterial("코발트",12)}
                , new CraftMaterial[]{new CraftMaterial("신비한 조각",5),new CraftMaterial("영롱한 구슬",2)}
                , Recipie.Ra)
            },
            { "크시포스", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Sword, "0001204", "크시포스", 350.0f, new ImageReferenceIndex(14) 
                , ItemGrade.High, 80, 200, 1.10f, 30, AttributeType.Earth
                , new CraftMaterial[]{new CraftMaterial("미스릴",13),new CraftMaterial("강철",6)}
                , new CraftMaterial[]{new CraftMaterial("은",2),new CraftMaterial("백금",2), new CraftMaterial("크롬 결정",3)}
                , Recipie.Ga)
            },

            /**** 활 ****/
            { "사냥꾼의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008000", "사냥꾼의 활", 10.0f, new ImageReferenceIndex(0)
                , ItemGrade.Low, 10, 100, 0.8f, 10, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("철",3), new CraftMaterial("단단한 나뭇가지",3)} 
                , null 
                , Recipie.Eu) 
            },
            { "롱 보우", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008001", "롱 보우", 20.0f, new ImageReferenceIndex(1)
                , ItemGrade.Low, 12, 100, 0.8f, 18, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("철",5), new CraftMaterial("부드러운 나뭇가지",2)}
                , new CraftMaterial[]{new CraftMaterial("물소의 뿔",2)}
                , Recipie.Na)
            },
            { "컴뱃 보우", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008002", "컴뱃 보우", 40.0f, new ImageReferenceIndex(2) 
                , ItemGrade.Low, 18, 100, 0.8f, 10, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("강철",5), new CraftMaterial("단단한 나뭇가지",2)}
                , null
                , Recipie.Ma)
            
            },
            { "양치기의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008003", "양치기의 활", 60.0f, new ImageReferenceIndex(3) 
                , ItemGrade.Low, 23, 100, 0.8f, 40, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("철",3), new CraftMaterial("가벼운 나뭇가지",2)}
                , new CraftMaterial[]{new CraftMaterial("솜뭉치",4)}
                , Recipie.Ga)
            },
            { "도망자의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008004", "도망자의 활", 80.0f, new ImageReferenceIndex(4) 
                , ItemGrade.Low, 25, 100, 0.8f, 20, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("철",4), new CraftMaterial("가벼운 나뭇가지",3)}
                , new CraftMaterial[]{new CraftMaterial("짐승가죽",2)}
                , Recipie.Da)
            },


            { "샤이닝 보우", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008100", "샤이닝 보우", 120.0f, new ImageReferenceIndex(5)
                , ItemGrade.Medium, 36, 150, 0.8f, 25, AttributeType.Water
                , new CraftMaterial[]{new CraftMaterial("티타늄",5),new CraftMaterial("단단한 나뭇가지",5)}
                , new CraftMaterial[]{new CraftMaterial("백금",3)}
                , Recipie.Da)
            },
            { "이글 보우", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008101", "이글 보우", 180.0f, new ImageReferenceIndex(6)
                , ItemGrade.Medium, 38, 150, 0.8f, 30, AttributeType.Wind
                , new CraftMaterial[]{new CraftMaterial("코발트",6),new CraftMaterial("튼튼한 나뭇가지",2)}
                , new CraftMaterial[]{new CraftMaterial("수은 결정",4)}
                , Recipie.Ra)
            },
            { "탄궁", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008102", "탄궁", 150.0f, new ImageReferenceIndex(7) 
                , ItemGrade.Medium, 48, 150, 0.8f, 30, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",5),new CraftMaterial("부드러운 나뭇가지",4)}
                , new CraftMaterial[]{new CraftMaterial("물소의 뿔",3)}
                , Recipie.Ma)
            },
            { "각궁", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008103", "각궁", 110.0f, new ImageReferenceIndex(8) 
                , ItemGrade.Medium, 32, 150, 0.8f, 12, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("미스릴",4),new CraftMaterial("가벼운 나뭇가지",3)}
                , new CraftMaterial[]{new CraftMaterial("크롬 결정",5)}
                , Recipie.Ee)
            },
            { "추격자의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008104", "추격자의 활", 100.0f, new ImageReferenceIndex(9) 
                , ItemGrade.Medium, 38, 150, 0.8f, 20, AttributeType.None
                , new CraftMaterial[]{new CraftMaterial("흑철",3),new CraftMaterial("가벼운 나뭇가지",5)}
                , new CraftMaterial[]{new CraftMaterial("짐승 가죽",4)}
                , Recipie.Ga)
            },


            { "취풍 파르티아", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008200", "취풍 파르티아", 450.0f, new ImageReferenceIndex(10)
                , ItemGrade.High, 80, 200, 0.88f, 16, AttributeType.Wind
                , new CraftMaterial[]{new CraftMaterial("티타늄",7),new CraftMaterial("미스릴",13),new CraftMaterial("엘프의 나뭇가지",3)}
                , new CraftMaterial[]{new CraftMaterial("하늘의 룬",4), new CraftMaterial("크롬 결정",3)}
                , Recipie.Da)
            },
            { "피닉스의 속삭임", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008201", "피닉스의 속삭임", 640.0f, new ImageReferenceIndex(11)
                , ItemGrade.High, 105, 200, 0.88f, 28, AttributeType.Fire
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",3),new CraftMaterial("티타늄",14),new CraftMaterial("축복받은 나뭇가지",4)}
                , new CraftMaterial[]{new CraftMaterial("불타는 심장",1), new CraftMaterial("영롱한 구슬",3)}
                , Recipie.Ga)
            },
            { "태양의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008202", "태양의 활", 530.0f, new ImageReferenceIndex(12) 
                , ItemGrade.High, 120, 200, 0.88f, 50, AttributeType.Fire
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",7),new CraftMaterial("흑철",18),new CraftMaterial("엘프의 나뭇가지",5)}
                , new CraftMaterial[]{new CraftMaterial("불타는 심장",3),new CraftMaterial("하늘의 룬",2)}
                , Recipie.Ra)
            },
            { "장미의 신궁", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008203", "장미의 신궁", 480.0f, new ImageReferenceIndex(13) 
                , ItemGrade.High, 95, 200, 0.88f, 32, AttributeType.Earth
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",5),new CraftMaterial("코발트",14),new CraftMaterial("축복받은 나뭇가지",3)}
                , new CraftMaterial[]{new CraftMaterial("요정의 목화",7)}
                , Recipie.Na)
            },
            { "파마의 활", new ItemCraftWeapon( ItemType.Weapon, WeaponType.Bow, "0008204", "파마의 활", 420.0f, new ImageReferenceIndex(14) 
                , ItemGrade.High, 80, 200, 0.88f, 36, AttributeType.Gold
                , new CraftMaterial[]{new CraftMaterial("오리하르콘",5),new CraftMaterial("미스릴",11),new CraftMaterial("축복받은 나뭇가지",4)}
                , new CraftMaterial[]{new CraftMaterial("신비한 조각",4),new CraftMaterial("영롱한 구슬",1)}
                , Recipie.Ma)
            }

        };    
    }
}
