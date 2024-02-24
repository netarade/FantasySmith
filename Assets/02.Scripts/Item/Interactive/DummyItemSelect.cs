using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * [작업 사항]
 * <v1.0 - 2024_0129_최원준>
 * 1- 최초 스크립트 작성
 * ItemSelect스크립트를 상속하여 더미 아이템이 셀렉팅 일어날 때 부모 메서드가 그대로 호출되도록 하였음 
 * 
 * <v1.1 - 2024_0214_최원준>
 * 1- ItemSelect스크립트 상속을 취소하고 MonoBehaviour를 상속하는 형태로 롤백
 * 이유는 셀렉팅이 일어날 때 더미가 셀렉팅이 일어나야 하는 것이 아니라 
 * 더미는 셀렉팅 종료되고 마우스 포지션만 넘겨서 원래의 2D 오브젝트가 셀렉팅이일어나도록 해야 하므로 
 * 
 * 2- 인터페이스를 OnSelect만 사용하여, 더미 아이템 셀렉팅 시 착용은 강제로 해제되고 바로 원본 아이템의 셀렉팅이 이루어질 수 있게 하였음
 * 
 * <v1.2 - 2024_0216_최원준>
 * 1- 인터페이스를 OnUpdateSelected를 사용하는 것으로 변경하였음
 * 이유는 OnSelect에서 EventSystem.current.SetSelectedGameObject(null);메서드를 호출하여 셀렉팅을 강제로 해제하는 경우
 * 셀렉팅 상태에서 셀렉팅을 시도했다고 오류가 발생하기 때문
 * 
 * => 
 * 셀렉트가 시작되면 이벤트 시스템이 동일 이벤트의 발생에 대하여 보호 혹은 락을 걸기 때문에 셀렉트 진행 중인 OnUpdateSelected에서 호출 해줘야 함. 
 * 참고로 특정 코드 부분 혹은 중복 코드로 셀렉팅에 영향을 주는 경우 에러가 발생할 수 있기 때문에 아래와 같이 셀렉팅여부를 확인후 호출하는 경우도 있음
 * if (!EventSystem.current.alreadySelecting) eventSystem.SetSelectedGameObject (null);
 * 
 * 
 * 
 * 
 * 
 * 
 */




public class DummyItemSelect : MonoBehaviour, IUpdateSelectedHandler
{ 
    DummyInfo dummyInfo;                // 아이템 정보 참조를 위한 더미 정보 참조 값 
    Button dummyBtn;                    // 셀렉팅 해제를 위한 더미 버튼

    private void Awake()
    {
        dummyBtn = GetComponent<Button>();
        dummyInfo = GetComponent<DummyInfo>();
    }
    
            
    public void OnUpdateSelected( BaseEventData eventData )
    {
        dummyBtn.OnDeselect( eventData );                   // 버튼을 Deselect상태로 만듭니다.
        EventSystem.current.SetSelectedGameObject( null );  // 이벤트 시스템의 셀렉트 상태를 null로 만듭니다.

        QuickSlot quickSlot = dummyInfo.EquipItemInfo.InventoryInfo as QuickSlot;
        
        // 아이템이 퀵슬롯에 속해있으며, 착용 중인 상태라면
        if( quickSlot!=null&&dummyInfo.EquipItemInfo.IsEquip )       
        {          
            // 퀵슬롯에 셀렉팅을 알리고 착용을 해제합니다.
            quickSlot.OnQuickSlotSelect( dummyInfo.EquipItemInfo ); 

            // 장착 상태였던 경우 시스템의 아이템의 원본 2D 오브젝트를 셀렉트 상태로 만듭니다.
            EventSystem.current.SetSelectedGameObject( dummyInfo.EquipItemInfo.Item2dTr.gameObject );
        }        
    }




}
