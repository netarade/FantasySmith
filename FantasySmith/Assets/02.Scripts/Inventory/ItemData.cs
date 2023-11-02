using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1101_최원준>
 * 1 - 최초작성 및 테스트 완료
 *  
 * <v1.1 - 2023_1102_최원준>  
 * 1 - 파일명을 Item에서 ItemData로 수정하였음. (여러 클래스를 포함하기 때문)
 * 
 * 2 - Item클래스의 ItemType을 대분류 항목으로 수정.
 * 상속받은 클래스가 해당클래스에 맞게 중분류 Type을 가지도록 하였음.
 * 하위 클래스에 중분류 Type 생성 및 생성자도 수정.
 * 
 * 3 - 생성자 간소화
 * 아이템의 원형을 미리 만들어 놓을 때 
 * 이미지를 넣지 않는 생성자나, 디폴트 생성자는 사용하지 않을 예정이므로.
 * 즉, 모든 데이터와 이미지들을 넣고 만들어놓을 예정.
 * 
 * 4 - Item 클래스에 ICloneable 인터페이스 추상형태로 구현 및 각 자식 클래스에서 직접 구현하도록 만듬.
 * 아이템 제작은 아이템을 새롭게 생성하는 것이므로, 일반적인 참조방식으로는 객체를 미리 여러개 만들어놔야 한다.
 * 복제를 통해 제작 시점에 새로운 객체를 할당받을 수 있게 한다.
 * 
 */

namespace ItemData
{
    public enum ItemType { Misc, Weapon };
    public enum MiscType { Basic, Additive, Fire, Attribute, Engraving, Etc } // 기본재료, 추가재료, 연료, 속성석, 각인석, 기타
    public enum WeaponType { Sword, Blade, Spear, Dagger, Thin, Axe, Blunt, Bow, Crossbow, Claw, Whip } //검,도,창,단검,세검,활,석궁,클로,채찍


    /// <summary>
    /// 기본 아이템 추상 클래스 - 인스턴스를 생성하지 못합니다. 반드시 상속하여 사용하세요. 상속한 클래스는 ICloneable을 구현해야합니다.
    /// </summary>
    public abstract class Item : ICloneable
    {
        protected ItemType enumType;        // 타입
        protected string strNo;             // 번호
        protected string strName;           // 이름
        protected float fPrice;             // 가격   
        protected ImageCollection icImage;  // 아이템의 인벤토리 내부 이미지, 상태창 이미지 등을 저장한다.
                
        public ItemType Type { get { return enumType; } }
        public string Name { get { return strName; } }
        public float Price { get { return fPrice; } }
        public ImageCollection Image { 
            get { return icImage;} 
            set { icImage = value; }
            }

        public Item( ItemType type, string No, string name, float price, ImageCollection image ) 
        {
            enumType = type;
            strName = name;
            strNo = No;
            fPrice = price; 
            icImage = image;
        }

        public abstract object Clone();
    }


    /// <summary>
    /// 잡화 아이템 - 기본 아이템과 다른점은 인벤토리에 중복해서 쌓을 수 있다는 점 (count가 존재)
    /// </summary>
    public sealed class ItemMisc : Item
    {        
        private int inventoryCount;  // 인벤토리 중첩 횟수

        /// <summary>
        /// 잡화 아이템의 중첩횟수를 표시합니다. 초기 딕셔너리에는 0이 들어있으니 반드시 갯수를 지정하거나 증가해주세요.
        /// </summary>
        public int InventoryCount { 
            set { inventoryCount = value; }
            get { return inventoryCount; }
        }

        private MiscType enumMiscType;

        /// <summary>
        /// 잡화아이템 소분류 타입 - Basic, Additive, Fire 등이 있습니다.
        /// </summary>
        public MiscType EnumMiscType { get { return enumMiscType; } }

        public ItemMisc( ItemType mainType, MiscType subType, string No, string name, float price, ImageCollection image )
            : base( mainType, No, name, price, image ) 
        { 
            inventoryCount = 0;
            enumMiscType = subType;
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
    /// 무기 아이템
    /// </summary>
    public class ItemWeapon : Item
    {
        protected WeaponType enumWeaponType;    // 무기 소분류 타입
        /// <summary>
        /// 무기 아이템 소분류 타입 - Sword, Blade, Spear, Dagger 등이 있습니다.
        /// </summary>
        public WeaponType EnumWeaponType { get { return enumWeaponType; } }

        public ItemWeapon(ItemType mainType, WeaponType subType, string No, string name, float price, ImageCollection image )
            : base( mainType, No, name, price, image ) 
        {
            enumWeaponType = subType;
        }

        /// <summary>
        /// 무기 아이템의 객체를 쉽게 복제하여 줍니다.
        /// </summary>
        public override object Clone()
        {
            return this.MemberwiseClone();
        }
    }


    /// <summary>
    /// 제작 숙련도 관련 속성들을 모아놓은 클래스로서 플레이어가 아이템을 생성하게 되는 즉시 가지고 있어야 할 목록에 들어가게될 집합입니다.
    /// </summary>
    public class CraftProficiency
    {        
        private int proficiency;
        /// <summary>
        /// 장비의 제작 숙련도를 기록
        /// </summary>
        public int Proficiency
        {
            set {  proficiency = Mathf.Clamp(value, 0, 100); }    
            get { return proficiency; }
        }

        private int recipieHitCount;
        /// <summary>
        /// 레시피가 정확하게 맞은 횟수
        /// </summary>
        public int RecipieHitCount 
        { 
            get { return recipieHitCount; } 
            set { recipieHitCount = value; }
        }

        /// <summary>
        /// 멤버의 모든 값을 0으로 초기화하여 생성합니다.
        /// </summary>
        public CraftProficiency()
        {
            proficiency = 0;
            RecipieHitCount = 0;
        }
    }



}
