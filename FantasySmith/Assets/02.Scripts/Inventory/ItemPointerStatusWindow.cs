using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ItemData;
using UnityEngine.UI;
using Unity.VisualScripting;

/* [작업 사항]
 * <v1.0 - 2023_1105_최원준>
 * 1- 초기 로직 작성
 * 커서를 갖다대는 순간 아이템 스테이터스 창을 볼 수 있으며
 * 떼는 순간 스테이터스 창이 꺼지도록 하였음.
 * 스테이터스 창은 아이템 정보를 반영
 * 
 * <v1.1 - 2023_1106_최원준>
 * 1- txtDesc와 txtSpec 순서를 변경
 * 2- 상태창이 무기와 잡화아이템의 정보를 반영하도록 변경
 * 
 * <v1.2 - 2023_1106_최원준>
 * 1- 잡화아이템의 상태창 설명에 이름과 갯수가 표시되도록 수정
 * 2- 상태창 참조가 동적할당으로는 제대로 잡히지 않아 게임 시작 시 참조하는 변수를 미리 잡아놓도록 InventoryManagement에 static 참조 변수를 선언.
 * 
 */



/// <summary>
/// 이 스크립트는 반드시 아이템 오브젝트(프리팹)에 위치해야 합니다. 아이템의 이벤트와 정보를 받아야 하기 때문입니다.
/// </summary>
public class ItemPointerStatusWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{    
    private GameObject statusWindow;    // 상태창 오브젝트
    private Image imageItem;            // 상태창의 아이템 이미지
    private Text txtEnhancement;        // 상태창의 아이템 강화 텍스트
    private Text txtName;               // 상태창의 아이템 이름
    private Text txtDesc;               // 상태창의 아이템 설명
    private Text txtSpec;               // 상태창의 아이템 스펙

    private Item item;                  // 개념 아이템을 참조하기 위한 변수


    
    /// <summary>
    /// 모든 아이템 인스턴스가 스테이터스 창이 꺼지기 전에 참조해야 하므로 반드시 Awake에 두어야 합니다.
    /// </summary>
    void Awake()
    {        
        statusWindow=InventoryManagement.statusWindow;
        imageItem = statusWindow.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        txtEnhancement = statusWindow.transform.GetChild(1).GetComponent<Text>();
        txtName = statusWindow.transform.GetChild(2).GetComponent<Text>();
        txtDesc = statusWindow.transform.GetChild(3).GetComponent<Text>();
        txtSpec = statusWindow.transform.GetChild(4).GetComponent<Text>();
    }
    

    void Start()
    {        
        item = GetComponent<ItemInfo>().item;
    }


    /// <summary>
    /// 아이템에 커서를 갖다 대는 순간 아이템의 정보를 볼 수 있습니다.
    /// </summary>
    public void OnPointerEnter( PointerEventData eventData )
    {
        if( item==null )                            // 아이템 정보가 비었다면 보여주지 않는다.
            return;



        /*** 아이템 종류 상관없이 공통 로직 ***/
        statusWindow.SetActive(true);               // 상태창 활성화
        statusWindow.transform.localPosition 
            = transform.position + Vector3.left*350f + Vector3.down*600f; //상태창의 위치
    
        imageItem.sprite = item.Image.statusSprite; // 이미지에 등록한 statusSprite 이미지를 보여준다.
        txtName.text = item.Name;                   // 이름 텍스트에 아이템 이름을 보여준다.
        txtDesc.text = item.Name;                   // 설명 텍스트에 아이템 이름을 임시적으로 보여준다.


        /*** 아이템 종류별 차이 ***/
        if( item.Type == ItemType.Misc )
        { 
            txtEnhancement.enabled = false;         // 강화 텍스트를 비활성화
            txtSpec.enabled = false;                // 상세 스펙 텍스트를 비활성화
            txtDesc.text += " " + ((ItemMisc)item).InventoryCount + "개";        // 중첩 횟수를 표시
        }
        else if(item.Type == ItemType.Weapon)
        {
            txtEnhancement.enabled = true;          // 강화 텍스트를 활성화
            txtSpec.enabled = true;                 // 상세 스펙 텍스트를 활성화

            ItemWeapon itemWeap = (ItemWeapon)item;                     // 아이템을 무기 자료형으로 형변환

            if(itemWeap.EnhanceNum>0)  // 무기 강화 단계가 1단계 이상인 경우 강화 텍스트 입력
                txtEnhancement.text = "+" + itemWeap.EnhanceNum.ToString(); 
            else
                txtEnhancement.text = "";

            string strGrade;                    // 무기의 등급 한글 문자열
            string strType;                     // 무기의 종류 한글 문자열
            string strAttr;                     // 무기의 속성 한글 문자열
            

            switch(itemWeap.LastGrade)          // 무기 등급의 한글 문자열을 지정
            {
                case Rarity.Normal :
                    strGrade="노말";
                    break;
                case Rarity.Magic :
                    strGrade="매직";
                    break;        
                case Rarity.Rare :
                    strGrade="레어";
                    break;
                case Rarity.Epic :
                    strGrade="에픽";
                    break;
                case Rarity.Unique :
                    strGrade="유니크";
                    break;
                case Rarity.Legend :
                    strGrade="레전드";
                    break;
                default :
                    strGrade="미정";
                    break;
            }

            switch(itemWeap.EnumWeaponType)     // 무기 종류의 한글 문자열을 지정
            {
                case WeaponType.Sword :
                    strType="검";
                    break;
                case WeaponType.Bow :
                    strType="활";
                    break;
                default :
                    strType="미정";
                    break;
            }
                        
            switch(itemWeap.CurAttribute)       // 무기 속성의 한글 문자열을 지정
            {
                case EnumAttribute.Water :
                    strAttr="수(水)";
                    break;
                case EnumAttribute.Wind :
                    strAttr="풍(風)";
                    break;
                case EnumAttribute.Earth :
                    strAttr="지(地)";
                    break;
                case EnumAttribute.Fire :
                    strAttr="화(火)";
                    break;
                case EnumAttribute.Gold :
                    strAttr="금(金)";
                    break;
                case EnumAttribute.None :
                    strAttr="무(無)";
                    break;
                default :
                    strAttr="미정";
                    break;
            }                                
                        
            txtSpec.text = string.Format(               // 무기의 상세 정보 텍스트
                $"등급: {strGrade}\n" +
                $"종류 : {strType}\n" +
                $"공격력 : {itemWeap.Power}\n" +
                $"내구도 : {itemWeap.Durability}\n" +
                $"공격속도 : {itemWeap.Speed:0.00}\n" +
                $"무게 : {itemWeap.Weight}\n" +
                $"속성 : {strAttr}");


        }
        
    }


    /// <summary>
    /// 아이템에서 커서를 떼는 순간 아이템 스테이터스 창이 사라집니다.
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerExit( PointerEventData eventData )
    {
        statusWindow.SetActive(false);      // 상태창 비활성화
    }


}
