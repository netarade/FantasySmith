using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ItemData;
using UnityEngine.UI;

/* [작업 사항]
 * <v1.0 - 2023_1105_최원준>
 * 1- 초기 로직 작성
 * 커서를 갖다대는 순간 아이템 스테이터스 창을 볼 수 있으며
 * 떼는 순간 스테이터스 창이 꺼지도록 하였음.
 * 스테이터스 창은 아이템 정보를 반영
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
    private Text txtSpec;               // 상태창의 아이템 스펙
    private Text txtDesc;               // 상태창의 아이템 설명

    private Item item;                  // 개념 아이템을 참조하기 위한 변수


    
    /// <summary>
    /// 모든 아이템 인스턴스가 스테이터스 창이 꺼지기 전에 참조해야 하므로 반드시 Awake에 두어야 합니다.
    /// </summary>
    void Awake()
    {        
        statusWindow = GameObject.Find("Panel-ItemStatus");
        imageItem = statusWindow.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        txtEnhancement = statusWindow.transform.GetChild(1).GetComponent<Text>();
        txtName = statusWindow.transform.GetChild(2).GetComponent<Text>();
        txtSpec = statusWindow.transform.GetChild(3).GetComponent<Text>();
        txtDesc = statusWindow.transform.GetChild(4).GetComponent<Text>();
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
        if(item == null)
            return;

        statusWindow.SetActive(true);       // 상태창 활성화
        statusWindow.transform.localPosition 
            = transform.position + Vector3.left*350f + Vector3.down*600f; //상태창의 위치
    
        imageItem.sprite = item.Image.statusSprite;
        txtName.text = item.Name;
        txtDesc.text = item.Name;

        if(item.Type == ItemType.Weapon)
        {
            ItemWeapon weap = (ItemWeapon)item;
            txtEnhancement.enabled = true;
            //txtEnhancement.text 

        }
        else if( item.Type == ItemType.Misc )
        { 
            txtEnhancement.enabled = false;
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
