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
 * v1.0 - 2023_1101_최원준
 * 최초작성 및 테스트 완료
 */

namespace ItemData
{
    public enum ItemType { Basic, Additive, Fire, Attribute, Engraving, Sword, Bow };



    /// <summary>
    /// 기본 아이템 추상 클래스 - 인스턴스를 생성하지 못합니다. 반드시 상속하여 사용하세요.
    /// </summary>
    public abstract class Item
    {
        protected ItemType enumType;
        protected string strNo;
        protected string strName;
        protected float fPrice;
        protected ImageCollection icImage;    // 아이템이 외부에서 비추어질 이미지
                
        public ItemType Type { get { return enumType; } }
        public string Name { get { return strName; } }
        public float Price { get { return fPrice; } }

        public Item()
        {
            Debug.Log("Basic Item 생성되었습니다.");            
        }

        public Item( ItemType type, string No, string name, float price)
        {
            enumType = type;
            strName = name;
            strNo = No;
            fPrice = price;             
        }

        public Item( ItemType type, string No, string name, float price, ImageCollection image ) 
            : this(type, No, name, price)
        {            
            icImage = image;
        }
    }



    /// <summary>
    /// 잡화 아이템 - 기본 아이템과 다른점은 인벤토리에 중복해서 쌓을 수 있다는 점
    /// </summary>
    public class ItemMisc : Item
    {        
        private int count;
        public int Count { 
            set { count = value; }
            get { return count; } 
        }

        public ItemMisc()
        {
            Debug.Log("Misc 생성되었습니다.");
        }

        public ItemMisc(ItemType type, string No, string name, float price ) 
            : base( type, No, name, price )
        { }

        public ItemMisc( ItemType type, string No, string name, float price, ImageCollection image )
            : base( type, No, name, price, image ) 
        { }
    }


}
