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
 * 1- 최초작성 및 테스트 완료
 *  
 * <v1.1 - 2023_1102_최원준>  
 * 1- 파일명을 Item에서 ItemData로 수정하였음. (여러 클래스를 포함하기 때문)
 * 
 * 2- Item클래스의 ItemType을 대분류 항목으로 수정.
 * 상속받은 클래스가 해당클래스에 맞게 중분류 Type을 가지도록 하였음.
 * 하위 클래스에 중분류 Type 생성 및 생성자도 수정.
 * 
 * 3- 생성자 간소화
 * 아이템의 원형을 미리 만들어 놓을 때 
 * 이미지를 넣지 않는 생성자나, 디폴트 생성자는 사용하지 않을 예정이므로.
 * 즉, 모든 데이터와 이미지들을 넣고 만들어놓을 예정.
 * 
 * 4- Item 클래스에 ICloneable 인터페이스 추상형태로 구현 및 각 자식 클래스에서 직접 구현하도록 만듬.
 * 아이템 제작은 아이템을 새롭게 생성하는 것이므로, 일반적인 참조방식으로는 객체를 미리 여러개 만들어놔야 한다.
 * 복제를 통해 제작 시점에 새로운 객체를 할당받을 수 있게 한다.
 * 
 * <v2.0 - 2023_1103_최원준>
 * 1- 아이템이 현재 놓여있는 인벤토리의 슬롯 인덱스를 정보로 가지고 있도록 하였음.
 * 인벤토리 리스트에 아이템을 담을 때 슬롯정보를 넣어서 저장하면, 
 * 인벤토리에서 아이템의 정보를 꺼냈을 때, 따로 슬롯의 인덱스 리스트를 관리하는 것보다 한 번에 보기 쉽기 때문.
 *  
 * 2- ItemDebugInfo 메서드 추가. 호출 시 간단한 정보를 디버그상으로 표현
 * 
 * <v3.0 - 2023_1105_최원준>
 * 1- 무기 타입에 각종 파라미터 추가
 * 
 * <v4.0 - 2023_1105_최원준>
 * 1- 복잡도로 인해 잡화,무기아이템의 클래스를 각 파일로 분리, 현재 파일에는 기본 아이템 클래스만 놔둔다.
 * 
 * <v5.0 - 2023_1116_최원준>
 * 1- 데이터 전송을 위해 Item클래스의 ImageCollection 멤버변수 icImage를 제거하고 ImageReferenceIndex 구조체 변수 sImgRefIdx로 변경
 * (Item클래스의 이미지 직접 저장 방식에서 이미지 인덱스 저장방식으로 변경)
 * 
 * 2- slotIndex는 각 아이템 종류 탭의 인덱스로 처리하고 전체 탭에서의 인덱스를 처리하기 위한 slotIndexAll을 추가하였음.
 */

namespace ItemData
{    
    /// <summary>
    /// 아이템의 대분류
    /// </summary>
    public enum ItemType { Misc, Weapon };
    
    /// <summary>
    /// 아이템 고유 등급
    /// </summary>
    public enum ItemGrade { Low, Medium, High }


    /// <summary>
    /// 외부의 아이템 이미지를 참조할 인덱스가 모여있는 구조체 입니다.
    /// </summary>
    [Serializable]
    public struct ImageReferenceIndex
    {
        /// <summary>
        /// 인벤토리 내부이미지 인덱스
        /// </summary>
        public int innerImgIdx;

        /// <summary>
        /// 상태창 이미지 인덱스
        /// </summary>
        public int statusImgIdx;

        /// <summary>
        /// 인벤토리 외부이미지 인덱스
        /// </summary>
        public int outerImgIdx; 

        /// <summary>
        /// 인벤토리 내부이미지, 상태이미지, 외부이미지의 인덱스넘버를 동일하게 지정하여 인덱스 구조체를 생성합니다.
        /// </summary>
        public ImageReferenceIndex(int index)
        {
            innerImgIdx = index;
            statusImgIdx = index;
            outerImgIdx = index;
        }

        /// <summary>
        /// 인벤토리 내부이미지, 상태이미지, 외부이미지의 인덱스넘버를 따로 지정하여 인덱스 구조체를 생성합니다.
        /// </summary>
        public ImageReferenceIndex(int innerImageIndex, int statusImageIndex, int outerImageIndex )
        {
            innerImgIdx = innerImageIndex;
            statusImgIdx = statusImageIndex;
            outerImgIdx = outerImageIndex;
        }

    }


    
    /// <summary>
    /// 기본 아이템 추상 클래스 - 인스턴스를 생성하지 못합니다. 반드시 상속하여 사용하세요. 상속한 클래스는 ICloneable을 구현해야합니다.
    /// </summary>
    [Serializable]
    public abstract class Item : ICloneable
    {
        protected ItemType enumType;        // 타입
        protected string strNo;             // 번호
        protected string strName;           // 이름
        protected float fPrice;             // 가격   

        protected int slotIndex;            // 아이템은 자신이 놓여있는 인벤토리의 슬롯 인덱스 정보를 가진다.
        protected int slotIndexAll;         // 아이템의 전체 탭에서의 위치를 결정
        
        protected ImageReferenceIndex sImgRefIdx;    // 아이템의 이미지를 참조할 인덱스 정보를 가진 구조체 변수 


        public ItemType Type { get { return enumType; } }
        public string Name { get { return strName; } }
        public float Price { get { return fPrice; } }
        
        /// <summary>
        /// 아이템의 이미지를 표현하는 인덱스 정보가 담긴 구조체 변수입니다. 
        /// </summary>
        public ImageReferenceIndex sImageRefIndex { 
            get { return sImgRefIdx;} 
            set { sImgRefIdx = value; }
        }
        
        /// <summary>
        /// 해당 아이템이 담긴 슬롯 인덱스 정보입니다. 아이템이 슬롯을 이동할 때 마다 이 정보를 변경해야 합니다.
        /// </summary>
        public int SlotIndex 
        { 
            set { slotIndex = value; }
            get { return slotIndex; } 
        }

        /// <summary>
        /// 해당 아이템이 담긴 전체 슬롯에 대한 인덱스 정보입니다.
        /// </summary>
        public int SlotIndexAll { set { slotIndexAll=value;} get { return slotIndexAll;} }

        public Item( ItemType type, string No, string name, float price, ImageReferenceIndex imageRefIndex ) 
        {
            enumType = type;
            strName = name;
            strNo = No;
            fPrice = price; 
            sImgRefIdx = imageRefIndex;
        }

        public abstract object Clone();
        public void ItemDeubgInfo()
        {
            Debug.Log("Type : " + enumType);
            Debug.Log("No : " + strNo);
            Debug.Log("Name : " + strName);
            Debug.Log("Price : " + fPrice);
            Debug.Log("SlotIndex : " + slotIndex);
            Debug.Log("SlotIndexAll : " + slotIndexAll);
        }
    }
}
