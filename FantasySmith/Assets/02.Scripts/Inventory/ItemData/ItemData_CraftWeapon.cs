using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1105_최원준>
 * 1- 제작무기(ItemCraftWeapon) 클래스 파일 분리.
 * 2- 각 종 기본 변수 및 프로퍼티, 생성자와 메서드 구현
 * 3- 주석처리
 * 
 */
namespace ItemData
{
    /// <summary>
    /// 무기 제작에 필요한 재료 - 재료 이름과 갯수
    /// </summary>
    public struct CraftMaterial
    {
        public string name;
        public int count;
    }
        
    /// <summary>
    /// 레시피의 종류 - ㄱ, ㄴ, ㄷ, ㄹ, ㅁ, ㅡ, ㅣ
    /// </summary>
    public enum Recipie { Ga,Na,Da,Ra,Ma,Eu,Ee }


    
    /// <summary>
    /// 제작 무기 아이템
    /// </summary>
    public class ItemCraftWeapon : ItemWeapon
    {
        /*** 제작 관련 속성 ***/
        private float fCraftChance;                 // 제작 확률 (기본 등급에 따라 결정 90%, 60%, 25%)
        Recipie enumRecipie;                        // 2단계 레시피
        CraftMaterial[] cmArrBaseMaterial;          // 제작 시 필요한 기본 재료
        CraftMaterial[] cmArrAdditiveMaterial;      // 제작 시 필요한 추가 재료
        protected int iFirePower;                   // 2단계 제작에 필요한 화력


        public ItemCraftWeapon(ItemType mainType, WeaponType subType, string No, string name, float price, ImageCollection image // 아이템 기본 정보 
            , ItemGrade basicGrade, int power, int durability, float speed, int weight, Attribute attribute                      // 무기 고유 정보
            , CraftMaterial[] baseMaterial, CraftMaterial[] additiveMaterial, Recipie recipie                                    // 제작 관련 정보   
        ) : base( mainType, subType, No, name, price, image, basicGrade, power, durability, speed, weight, attribute )
        {          

            /*** 제작 관련 정보 ***/
            switch(basicGrade)
            {
                case ItemGrade.Low :
                    fCraftChance = 0.9f;
                    break;
                case ItemGrade.Medium :
                    fCraftChance = 0.6f;
                    break;
                case ItemGrade.High :
                    fCraftChance = 0.25f;
                    break;
            }

            cmArrBaseMaterial = baseMaterial;
            cmArrAdditiveMaterial = additiveMaterial;
            enumRecipie = recipie;
            
            iFirePower = 0;
            for(int i=0; i<baseMaterial.Length; i++)
                iFirePower += baseMaterial[i].count;        // 2단계 화력 - 기본 재료의 수에 따라 결정 
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
