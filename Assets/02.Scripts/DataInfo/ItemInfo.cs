using UnityEngine;
using UnityEngine.UI;
using ItemData;

/*
 * [작업 사항]
 * 
 * <v1.0 - 2023_1102_최원준>
 * 1- 최초작성 및 주석처리
 * 
 * <v2.0 - 2023_1103_최원준>
 * 1- 주석 수정
 * 2- 이미지 컴포넌트 잡는 구문을 Start메서드에서 OnEnable로 변경
 * 인스턴스가 생성되어 이미지 컴포넌트를 잡기 시작하면 OnItemAdded와 호출 시점이 동시성을 가져서 스프라이트 이미지가 변경되지 않는다.
 * 
 * <v3.0 - 2023-1105_최원준>
 * 1- 개념아이템 변수인 item을 프로퍼티화 시켜서 set이 호출되었을 때 OnItemChanged()가 호출되도록 변경 
 * OnItemAdded는 private처리 및 내부 예외처리 구문 삭제
 *
 *<v4.0 - 2023_1108_최원준>
 *1- 아이템이 파괴될 때 정보를 저장하도록 구현하였으나, 모든아이템의 기록이 저장되지 않는 문제발생
 *=> 아이템쪽에서 파괴될떄마다 딕셔너리를 생성해서 CraftManager쪽에서 마지막에 한번 미리 생성해주도록 변경
 *
 *2- OnItemChanged 메서드 주석추가
 *
 *3- UpdateCountTxt 메서드 추가
 * 아이템 수량이 변경 될때 동적으로 텍스트를 수정해주도록 하였음.
 * item쪽에서 메서드를 가지고 있음으로 해서 편리한 접근성 확보.
 *
 *<v5.0 - 2023_1112_최원준>
 *1- OnItemAdded메서드 추가수정. (CreateManager쪽에서 중복코드 사용하고 있던 점 수정 및 통합, 주석 추가) 
 *
 *<v6.0 - 2023_1114_최원준>
 *1- OnItemAdded메서드를 OnItemChanged로 이름변경
 *2- ItemInfo 클래스 설명 주석 추가
 *3- private 메서드 public 메서드로 변경
 *4- 멤버 변수 item이 public되어있던 점을 private 처리. 반드시 프로퍼티를 통한 초기화를 위해
 *
 *<v7.0 - 2023_1116_최원준>
 *1- ItemInfo 클래스가 ItemImageCollection 멤버변수를 포함하여 외부이미지를 참조하도록 설정하였습니다. 
 *(CreateManager에 있던 참조 변수를 옮겨옴.)
 *
 *2- UpdateImage메서드를 수정하였습니다.
 *기존의 아이템 클래스가 ImageCollection 구조체 변수를 멤버로 포함하고 있던 점에서 ImageReferenceIndex 구조체 변수를 멤버로 포함하도록 바꾸었기 때문에
 *item의 ImageReferenceIndex 멤버변수로 부터 인덱스값을 받아와서 ImageCollection 변수에 접근하여 오브젝트에 이미지를 넣어주도록 수정.
 *
 *<v7.1 - 2023_1119_최원준>
 *1- OnDestroy()메서드 주석처리. 
 *Inventory클래스를 직렬화 가능하게 변경할 예정이므로
 *
 *2- RemoveItemObject 미구현 메서드 제거 - inventory클래스에서 구현
 *ItemInfo 클래스는 오브젝트의 정보만 최신화 시켜주는 역할을 하게 해야하기 때문이며, 
 *ItemInfo에서 item의 내부정보를 수정하는 메서드를 추가하기 시작하면, Inventory클래스에서의 기능을 중복 구현할 가능성이 커짐.
 *
 *<v8.0 - 2023_1216_최원준>
 *1- 아이템의 상태창 이미지 변수 statusImage 추가 및 UpdateImage메서드 내부 수정
 *
 *2- 아이템 파괴시 전달 로직 주석처리 되어있던 부분 제거
 *
 *3- slotList 변수명 slotListTr로 변경
 *
 *4- Transform imageCollectionsTr 임시변수 선언후 
 * GameObject.Find( "ImageCollections" ) 중복 호출 로직 수정
 *
 *<v8.1 - 2023_1217_최원준>
 *1- ItemImageCollection 변수들을 하나씩 참조하던 것을 배열로 만들어서 참조
 *
 *<v8.2 - 2023_1221_최원준>
 *1- GameObject.Find()메서드로 오브젝트를 검색하던 것을 빠른참조로 변경
 *
 *<v9.0 - 2023_1222_최원준>
 *1- 태그참조 철자오류 수정 (CANVAS_CHRACTER -> CANVAS_CHARACTER)
 *
 *2- ItemImageCollection[]의 배열을 참조만하고 생성을 안해서 뜨는 배열의 bounds오류 수정
 *
 *3- 아이템의 생성시점에 UpdateImage나 UpdatePosition을 호출하면 참조가 잡히지 않기 때문에 bounds오류가 뜨는데
 * OnItemChanged메서드를 아이템의 생성시점 호출이 아니라, 아이템의 등장 시점에 호출하도록 수정하였음.
 *
 *4 - SlotListTr이 뷰포트로 잡혀있던 점을 수정
 *
 *<v9.1 - 2023_1224_최원준>
 *1- Item프로퍼티의 주석일부 삭제
 *2- 컴포넌트 참조 구문 Start에서 OnEanble로 이동 및 정리
 *3- UpdataImage메서드 아이템 종류에 따른 중복로직 제거 후 간략화
 *
 */


/// <summary>
/// 게임 상의 아이템 오브젝트는 이 클래스를 컴포넌트로 가져야합니다.<br/><br/>
/// 
/// ItemInfo 스크립트가 컴포넌트로 붙은 아이템 오브젝트의 자체적인 기능은 다음과 같습니다.<br/>
/// (ItemInfo의 개념 아이템 인스턴스인 item이 할당될 때 자동으로 이루어집니다.)<br/><br/>
/// 
/// 1.오브젝트의 이미지를 개념 아이템의 정보와 대조하여 채웁니다.<br/>
/// 2.잡화아이템의 경우 중첩횟수를 아이템정보와 비교하여 표시하여 줍니다. 비잡화 아이템의 경우 텍스트를 끕니다.<br/>
/// 3.인벤토리 슬롯 상의 포지션을 아이템 정보와 대조하여 해당 슬롯에 위치시킵니다.<br/><br/>
/// 
/// 주의) 아이템의 내부 정보가 바뀔 때 마다 최신 정보를 오브젝트에 반영해야 합니다.<br/>
/// 1,2,3의 경우 각 메서드를 따로 호출 할 수 있으며 모든 것을 한번에 호출하는  OnItemChanged메서드가 있습니다.<br/>
/// </summary>
public class ItemInfo : MonoBehaviour
{
    private Item item;              // 모든 아이템 클래스를 관리 가능한 변수
    private Image itemImage;        // 아이템이 인벤토리에서 2D상에서 보여질 이미지     

    public Sprite innerSprite;      // 아이템이 인벤토리에서 보여질 이미지 스프라이트
    public Sprite statusSprite;     // 아이템이 상태창에서 보여질 이미지 스프라이트

    public Text countTxt;           // 잡화 아이템의 수량을 반영할 텍스트
    public Transform slotListTr;    // 아이템이 놓이게 될 슬롯 들의 부모인 슬롯리스트 트랜스폼 참조

    public ItemImageCollection[] iicArr;      // 인스펙터 뷰 상에서 등록할 아이템 이미지 집합 배열
    public enum eIIC { MiscBase,MiscAdd,MiscOther,Sword,Bow }    // 이미지 집합 배열의 인덱스 구분
    private readonly int iicNum = 5;                             // 이미지 집합 배열의 갯수

    

    /// <summary>
    /// 클론 한 Item 인스턴스를 저장하고, 저장 되어있는 인스턴스를 불러올 수 있습니다.
    /// </summary>
    public Item Item                // 외부에서 개념아이템을 참조시킬 때 호출해야할 프로퍼티
    {
        get; set;
        //set {
        //        item =  value;
        //    }
        //get {return item;}
    }


    /// <summary>
    /// 이미지 컴포넌트를 잡는 우선순위를 높이기 위해 OnEnable 사용
    /// </summary>
    private void OnEnable()
    {
        itemImage = GetComponent<Image>();
        countTxt = GetComponentInChildren<Text>();

        Transform canvasTr = GameObject.FindWithTag("CANVAS_CHARACTER").transform;
        slotListTr = canvasTr.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        
        // 인스펙터뷰 상에서 달아놓은 스프라이트 이미지 집합을 참조합니다.
        Transform imageCollectionsTr = GameObject.FindAnyObjectByType<CreateManager>().transform.GetChild(0);

        // 배열을 해당 갯수만큼 생성해줍니다.
        iicArr = new ItemImageCollection[iicNum];

        // 각 iicArr은 imageCollectionsTr의 하위 자식오브젝트로서 ItemImageCollection 스크립트를 컴포넌트로 가지고 있습니다
        for( int i = 0; i<iicNum; i++)
            iicArr[i] = imageCollectionsTr.GetChild(i).GetComponent<ItemImageCollection>();
        
        // 아이템 오브젝트가 씬에 생성되면 정보를 업데이트하는 메서드를호출하여야 합니다.
        //OnItemChanged(); 
    }

    //public void Start()
    //{

    //}

    /// <summary>
    /// 오브젝트에 item의 참조가 이루어졌다면 item이 가지고 있는 이미지를 반영하고 잡화아이템의 경우 중첩 횟수까지 최신화 합니다.<br/>
    /// 오브젝트의 아이템 값을 입력할 때는 내부 자동 호출이 이루어지는 메서드이기에 아이템 정보 변동이 있을 때만 따로 호출하시면 됩니다.
    /// </summary>
    public void OnItemChanged()
    {
        UpdateImage();
        UpdateCountTxt();
        UpdatePosition();
    }

    /// <summary>
    /// 아이템의 이미지 정보를 받아와서 오브젝트에 반영합니다.<br/>
    /// Item 클래스는 정의될 때 외부에서 참조할 이미지 인덱스를 저장하고 있습니다.<br/>
    /// 해당 인덱스를 참고하여 인스펙터뷰에 등록된 이미지를 참조합니다.
    /// </summary>
    public void UpdateImage()
    {
        if(iicArr.Length == 0 )     // 아이템 생성 시점에 iicArr을 참조하는 것을 방지하여 줍니다.
            return;


        int imgIdx = -1;            // 참조할 이미지 인덱스 선언
                   
        //Debug.Log(Item.Type);
        switch( Item.Type )         // 아이템의 메인타입을 구분합니다.
        {

            case ItemType.Weapon:
            //Item.ItemDeubgInfo();
                ItemWeapon weapItem = (ItemWeapon)Item;
                WeaponType weaponType = weapItem.eWeaponType;  // 아이템의 서브타입을 구분합니다.

                switch (weaponType)
                {
                    case WeaponType.Sword :             // 서브타입이 검이라면,
                        imgIdx = (int)eIIC.Sword;
                        break;
                    case WeaponType.Bow :               // 서브타입이 활이라면,
                        imgIdx = (int)eIIC.Bow;
                        break;
                }
                break;
                
            case ItemType.Misc:
            //Item.ItemDeubgInfo();

                ItemMisc miscItem = (ItemMisc)Item; 
                MiscType miscType = miscItem.eMiscType;

                switch (miscType)
                {
                    case MiscType.Basic :           // 서브타입이 기본 재료라면,
                        imgIdx = (int)eIIC.MiscBase;
                        break;
                    case MiscType.Additive :        // 서브타입이 추가 재료라면,
                        imgIdx = (int)eIIC.MiscAdd;
                        break;
                    default :                       // 서브타입이 기타 재료라면,
                        imgIdx = (int)eIIC.MiscOther;
                        break;
                }
                break;
        }

        // 아이템 오브젝트 이미지를 인스펙터뷰에 직렬화되어 있는 ItemImageCollection 클래스의 내부 구조체 배열 ImageColection[]에
        // 개념아이템이 고유 정보로 가지고 있는 ImageReferenceIndex 구조체의 인덱스를 가져와서 접근합니다.                
             
        innerSprite = iicArr[imgIdx].icArrImg[Item.sImageRefIndex.innerImgIdx].innerSprite;
        statusSprite = iicArr[imgIdx].icArrImg[Item.sImageRefIndex.statusImgIdx].statusSprite;

        // 참조한 스프라이트 이미지를 기반으로 아이템이 보여질 2D이미지를 장착합니다.
        itemImage.sprite = innerSprite;
    }


    /// <summary>
    /// 잡화 아이템의 중첩횟수를 동적으로 수정합니다. 잡화 아이템의 수량이 변경될 때 마다 호출해 주십시오.
    /// </summary>
    public void UpdateCountTxt()
    {
        if( Item.Type==ItemType.Misc )                // 잡화 아이템의 중첩 갯수를 표시합니다.
        {
            countTxt.enabled=true;
            countTxt.text = ((ItemMisc)Item).OverlapCount.ToString();
        }
        else
            countTxt.enabled = false;                // 잡화아이템이 아니라면 중첩 텍스트를 비활성화합니다.
    }

    
    // 아이템의 위치정보 반영  (현재 어디서 아이템 위치를 참조해서 슬롯에 넣어주고 있는가? => CreateManager에서 Instantiate할때 미리 붙인다.)
    public void UpdatePosition()
    {
        if( slotListTr.childCount==0 )
        {
            Debug.Log( "현재 슬롯이 생성되지 않은 상태입니다." );
            return;
        }
        transform.SetParent( slotListTr.GetChild(Item.SlotIndex) );  // 오브젝트의 현 부모를 아이템 정보상의 슬롯 위치로 변경한다.
        transform.localPosition = Vector3.zero;                    // 로컬위치를 부모기준으로부터 0,0,0으로 맞춘다.
    }


}