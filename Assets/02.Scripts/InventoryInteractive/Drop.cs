using ItemData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/* [작업 사항]
 * <v1.0 - 2023_1105_최원준>
 * 1- d초기 로직 작성, 아이템 스위칭 구현
 * 
 * <v1.1 - 2023_1106_최원준>
 * 드랍 시 슬롯의 위치를 저장하도록 구현
 * 
 * <v2.0 - 2023_1109_최원준>
 * 1- 강화석, 속성석, 각인석에 대한 로직 구현
 * 2- 변수 캐싱처리하여 중복 로직제거
 * 
 * 
 * <v2.1 - 2023_1119_최원준>
 * 1- ItemInfo스크립트의 item에 직접 접근하고 있던 점을 Item(프로퍼티)에의 접근으로 수정
 * 
 * <v3.0 - 2023_1216_최원준>
 * 1- 속성석을 5개로 나누었고 각각의 이름에 따른 속성개방 로직 분기 처리, 장착이 이뤄지면 무조건 감소 삭제되도록 변경
 * 2- 아이템 갯수 1일때 잡화 리스트에서 감소하던 구문을 잡화 딕셔너리를 참조하여 감소하는 구문으로 변경
 * 
 * <v3.1 - 2023_1217_최원준>
 * 1- miscDic을 PlayerInven클래스에서 싱글톤에 접속해서 초기화 하던 것을 인스턴스 생성하여 초기화 하는 것으로 변경
 * (싱글톤은 씬전환시 초기화가 느리기 때문)
 * 2- switch문의 강화석에 각인로직이 있던것을 수정
 * 3- Drop스크립트에서 속성석이나 각인석을 적용할 때 직접 BasePerformance를 조정하던 것을 삭제
 * 
 * <v3.2 - 2023_1222_최원준>
 * 1- playerInvenInfo의 잘못된 참조 수정
 * 단순 GetComponent<InventoryInfo>()에서 GameObject.FindWithTag("Player").GetComponent<InventoryInfo>()으로 변경
 * 
 */


public class Drop : MonoBehaviour, IDropHandler
{
    Transform slotTr;   // 슬롯 자신의 트랜스폼
    Dictionary<string, List<GameObject>> miscDic;
    void Start()
    {
        slotTr=GetComponent<Transform>();
        //플레이어 인벤토리 정보에서 잡화 딕셔너리를 참조합니다
        InventoryInfo playerInvenInfo = GameObject.FindWithTag("Player").GetComponent<InventoryInfo>();

        miscDic=playerInvenInfo.inventory.miscDic;
    }

    /// <summary>
    /// OnDrop이벤트 다음에 OnDragEnd 이벤트가 호출되므로, 아이템 스위칭 처리가 가능합니다.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop( PointerEventData eventData )
    {
        GameObject dragObj = Drag.draggingItem;
        Item draggingItem = dragObj.GetComponent<ItemInfo>().Item;   // 자주 사용하므로 캐싱처리



        // 드래그 중인 아이템이 속성석이나 강화석, 각인석이라면, 수행하는 로직들.
        if( draggingItem.Type==ItemType.Misc )
        {
            ItemMisc draggingMisc = ( (ItemMisc)draggingItem );

            if( draggingMisc.eMiscType==MiscType.Engraving||
                draggingMisc.eMiscType==MiscType.Enhancement||
                draggingMisc.eMiscType==MiscType.Attribute )
            {
                Item applyItem = slotTr.GetChild( 0 ).gameObject.GetComponent<ItemInfo>().Item;   //강화를 적용할 아이템의 정보를 봅니다.
                if( applyItem.Type==ItemType.Weapon )                 // 적용 대상이 무기라면 강화 로직을 수행하고 아니라면 스위칭 로직을 수행합니다.
                {

                    switch( draggingItem.Name )
                    {
                        case "강화석":
                            break;
                        case "속성석-수":
                            ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Water ); // 수 속성을 개방합니다. 
                            break;
                        case "속성석-금":
                            ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Gold );  // 굼 속성을 개방합니다. 
                            break;
                        case "속성석-지":
                            ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Earth ); // 지 속성을 개방합니다. 
                            break;
                        case "속성석-화":
                            ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Fire );  // 화 속성을 개방합니다. 
                            break;
                        case "속성석-풍":
                            ( (ItemWeapon)applyItem ).AttributeUnlock( AttributeType.Wind );  // 풍 속성을 개방합니다. 
                            break;
                        // 속성석은 개방여부와 상관없이 소모되어야 합니다
                        case "각인석":
                            ( (ItemWeapon)applyItem ).Engrave( draggingItem.Name ); // 드래깅아이템을 각인하여 각인정보를 반영합니다.
                            break;
                    }


                    if( draggingMisc.OverlapCount==1 )    // 아이템 갯수가 1개라면,
                    {
                        miscDic[draggingItem.Name].Remove( dragObj );   // 아이템을 플레이어 인벤토리의 잡화목록에서 제거합니다.
                        Destroy( dragObj, 0.5f );                       // 0.5초후 삭제 시킵니다.
                        dragObj.SetActive( false );                     // 아이템을 바로 disable 시킵니다.
                    }
                    else //2개 이상이라면,
                    {
                        draggingMisc.OverlapCount-=1;       // 실제 수량을 뺀다.
                        dragObj.GetComponent<ItemInfo>().UpdateCountTxt();  // 중첩 텍스트를 수정한다.
                    }


                    return;     // 적용되고 스위칭은 하지 않는다.
                }
            }
        }




        if( transform.childCount==0 )   // 슬롯이 비어있다면, 부모를 변경하고
        {
            dragObj.transform.SetParent( transform );         // 해당슬롯으로 부모 변경
            dragObj.transform.localPosition=Vector3.zero;   // 정중앙 위치
            draggingItem.SlotIndex=transform.GetSiblingIndex();    //바뀐 슬롯의 위치를 저장한다.
        }
        else if( transform.childCount==1 ) // 슬롯에 아이템이 이미 있다면, 각 아이템의 위치를 교환한다.
        {
            int prevIdx = Drag.prevParentTrByDrag.GetSiblingIndex(); //드래깅 중인 아이템이 속한 부모(슬롯)의 인덱스를 저장

            draggingItem.SlotIndex=transform.GetSiblingIndex();    //드래그중인 아이템은 바뀐 슬롯 위치로 저장한다.
            dragObj.transform.SetParent( slotTr );                              // 드래그 중인 아이템은 해당 슬롯으로 위치
            dragObj.transform.localPosition=Vector3.zero;                     // 정중앙 위치

            Transform switchingItemTr = slotTr.GetChild( 0 );                     // 바꿀 아이템 (현재 자식이 2개가 중첩되어 있다. 그 중 1번째 기존의 자식)
            switchingItemTr.GetComponent<ItemInfo>().Item.SlotIndex=prevIdx;  //스위칭할 아이템의 슬롯 번호를 기록해둔 위치로 저장 
            switchingItemTr.SetParent( Drag.prevParentTrByDrag );               // 이미지 우선순위 문제로 부모를 일시적으로 바꾸므로, 이전 부모를 받아온다.            
            switchingItemTr.localPosition=Vector3.zero;                       // 정중앙 위치
        }

    }

}
