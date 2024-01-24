using Newtonsoft.Json;
using System;
using UnityEngine;

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
 * 
 * <v6.0 - 2023_1222_최원준>
 * 1- private변수를 직렬화하기 위해 [JsonProperty] 어트리뷰트를 추가하였음
 * 
 * <v7.0 - 2023_1224_최원준>
 * 1- Item클래스의 추상클래스 기능을 삭제하고, Clone메서드의 abstract구문 삭제
 * 이유는 JSON에서 직렬화할 때 인스턴스화 할 수 없다는 오류가 자주 등장하기 때문
 * 2- 모든 기본 변수를 삭제하고 자동구현 프로퍼티로 사용하였습니다.
 * 이유는 생성되어질 때 한번 입력되면 더 이상 정보의 변동이 필요없고 읽기 전용으로 할당되어야 하는 속성이기 때문입니다.
 * 
 * <v7.1 - 2023_1226_최원준>
 * 1- Item클래스를 다시 abstract클래스로 다시 롤백하였음.
 * 이유는 직렬화하여 저장할 때 최상위 클래스로 저장해버리면 자식의 형정보가 사라지기 때문에 다시 자식으로 형변환시 캐스팅오류가 발생하기 때문
 * 해당 오류를 해결하기 위해 저장할 때, 각 개별 자식 클래스로 담아서 저장하는 형식으로 구현 중임
 * 
 * <v8.0 - 2023_1226_최원준>
 * 1- 저장할 때 인덱스 구조체 정보가 0으로 로드되는 현상이 발생하여 
 * 살펴보니 Json에서 역직렬화 시 프로퍼티 값을 채워줘야 하는데 자동구현 프로퍼티에 set이 없기 때문임을 발견
 * set을 추가하면 보안의 단점이 있기에, 프로퍼티 방식으로 다시 롤백하여 프로퍼티는 JsonIgnore 처리하여 저장 및 연산자원 소모를 방지
 * 
 * <v8.1 - 2023_1229_최원준>
 * 1- 파일명 변경 ItemData->Item
 * 2- ImageReferenceIndex 내부변수 outerImgIdx에서 meshFilterIdx와 materialIdx로 수정
 * 
 * <v8.2 - 2023_1230_최원준>
 * 1- ItemType에 None을 추가
 * 선택 인자의 Null값으로 사용하기 위해
 * 
 * <v9.0 - 2024_0108_최원준>
 * 1- 구조체명 ImageRefenreceIndex를 VisualReferenceIndex로 변경하고
 * outerMeshFilter, outerMaterial 변수를 삭제 및 outerPrefabIdx 변수를 추가하였습니다.
 * 이유는 필터와 머터리얼을 따로 관리하는 것보다 오브젝트 형태로 관리하는 것이 쉽기 때문에 
 * CreateManager에서 2D프리팹에 3D프리팹을 붙여주는 방식으로 구현하기로 변경하였습니다.
 * 
 * 2- 변수명 sImageRefIndex를 sVisualRefIndex로 변경, 프로퍼티 ImageRefIndex를 VisualRefIndex로 변경
 * 
 * 3- 생성자 매개변수 imageRefIndex를 visualRefIndex로 변경
 * 
 * <v10.0 - 2024_0111_최원준>
 * 1- 크래프팅 장르에 맞게 클래스 설계 변경 (필요없는 정의 삭제)
 * 
 * <v10.1 - 2024_0115_최원준>
 * 1- 변수 및 프로퍼티명 SlotIndex를 SlotIndexEach로 변경
 * 
 * <v10.2 - 2024_0124_최원준>
 * 1- Item클래스에 저장해야할 변수로 onwerId를 추가하였음
 * 이유는 월드상에 설치하는 아이템의 경우 소유주를 설정해서 퀘스트 성공 여부를 판단하며, 저장하고 불러와야할 경우가 있기 때문
 * 
 * <v10.3 - 2024_0124_최원준>
 * 1- owerId의 타입을 string에서 int로 변경 및 주석 수정
 * 이유는 순번대로 id를 증가시켜 부여하기 위함
 * 
 * 
 */

namespace ItemData
{    
    /// <summary>
    /// 외부의 아이템의 모습을 참조할 인덱스를 저장하는 구조체 입니다.
    /// </summary>
    [Serializable]
    public struct VisualReferenceIndex
    {
        /// <summary>
        /// 아이템의 인벤토리 내부이미지 인덱스
        /// </summary>
        public int innerImgIdx;

        /// <summary>
        /// 아이템의 상태창 이미지 인덱스
        /// </summary>
        public int statusImgIdx;

        /// <summary>
        /// 아이템의 월드 오브젝트 인덱스
        /// </summary>
        public int outerPrefabIdx; 



        /// <summary>
        /// 인벤토리 내부이미지, 상태이미지, 외부이미지의 인덱스넘버를 동일하게 지정하여 인덱스 구조체를 생성합니다.
        /// </summary>
        public VisualReferenceIndex(int index)
        {
            innerImgIdx = index;
            statusImgIdx = index;
            outerPrefabIdx = index;
        }

        /// <summary>
        /// 인벤토리 내부이미지, 상태이미지, 외부이미지의 인덱스넘버를 따로 지정하여 인덱스 구조체를 생성합니다.
        /// </summary>
        public VisualReferenceIndex(int innerImageIndex, int statusImageIndex, int outerPrefabIndex )
        {
            innerImgIdx = innerImageIndex;
            statusImgIdx = statusImageIndex;
            outerPrefabIdx = outerPrefabIndex;
        }

    }


    /// <summary>
    /// 아이템의 대분류로 현재 잡화, 무기, 퀘스트 3가지 종류가 있습니다.
    /// </summary>
    public enum ItemType { Misc, Weapon, Quest ,None };
    

    /// <summary>
    /// 기본 아이템 클래스 - 반드시 상속하여 사용하세요.
    /// </summary>  
    [Serializable]
    public abstract class Item : ICloneable
    {   
        [JsonProperty] ItemType eType;
        [JsonProperty] string sNo;
        [JsonProperty] string sName;
        [JsonProperty] string sDesc;

        [JsonProperty] VisualReferenceIndex sVisualRefIndex;
        [JsonProperty] int iSlotIndexEach;
        [JsonProperty] int iSlotIndexAll;
        [JsonProperty] int iOwnerId;





        /// <summary>
        /// 해당 아이템의 대분류 상의 종류로 무기는 Weapon, 잡화는 Misc등을 나타냅니다.
        /// </summary>
        [JsonIgnore] public ItemType Type { get {return eType; } }

        /// <summary>
        /// 해당 아이템의 아이템 테이블에 정의된 넘버로서 0001000 등의 넘버링을 가집니다. 
        /// </summary>
        [JsonIgnore] public string No { get {return sNo;} } 


        /// <summary>
        /// 해당 아이템의 아이템 테이블에 정의 되어있는 이름으로, string 형식의 변수입니다.
        /// </summary>
        [JsonIgnore] public string Name { get {return sName;} }

                
        /// <summary>
        /// 해당 아이템의 정의를 유저에게 표현해주는 설명으로, string 형식의 변수입니다.
        /// </summary>
        [JsonIgnore] public string Desc { get {return sDesc;} }


        
        /// <summary>
        /// 해당 아이템의 이미지를 표현하는 인덱스 정보가 담긴 구조체 변수입니다. 
        /// </summary>
        [JsonIgnore] public VisualReferenceIndex VisualRefIndex { get {return sVisualRefIndex;} }
        
        /// <summary>
        /// 해당 아이템이 담긴 슬롯 인덱스 정보입니다. 아이템이 슬롯을 이동할 때 마다 이 정보를 변경해야 합니다.
        /// </summary>
        [JsonIgnore] public int SlotIndexEach { get{return iSlotIndexEach;} set{iSlotIndexEach = value;} }

        /// <summary>
        /// 해당 아이템이 담긴 전체 슬롯에 대한 인덱스 정보입니다.
        /// </summary>
        [JsonIgnore] public int SlotIndexAll { get{return iSlotIndexAll; } set{iSlotIndexAll=value;} }


        /// <summary>
        /// 해당 아이템의 소유자를 식별할 수 있는 고유 번호를 말합니다.<br/>
        /// 어떤 인벤토리에 저장되는 지에 따라 해당 아이템의 소유자Id가 결정되어집니다.
        /// </summary>
        [JsonIgnore] public int OwnerId { get { return iOwnerId; } set { iOwnerId=value; } }





        public Item( ItemType type, string No, string name, VisualReferenceIndex visualRefIndex, string desc ) 
        {
            eType = type;
            sName = name;
            sNo = No;
            sVisualRefIndex = visualRefIndex;
            sDesc = desc;
        }

        /// <summary>
        /// 해당 아이템의 객체를 복사해서 반환해주는 메서드입니다.<br/>
        /// 기본 객체의 =연산은 참조전달이므로 하나의 인스턴스를 공유하게 되는데 이를 방지하여 새로운 인스턴스를 갖게 합니다.<br/>
        /// </summary>
        public object Clone() { return this.MemberwiseClone(); }


        /// <summary>
        /// 해당 아이템의 정보를 디버그 창에 출력해주는 메서드입니다.
        /// </summary>
        public void ItemDeubgInfo()
        {
            Debug.Log("Type : " + Type);
            Debug.Log("No : " + No);
            Debug.Log("Name : " + Name);
            Debug.Log("SlotIndexEach : " + SlotIndexEach);
            Debug.Log("SlotIndexAll : " + SlotIndexAll);
            Debug.Log("Desc : " + sDesc);
        }
    }
}
