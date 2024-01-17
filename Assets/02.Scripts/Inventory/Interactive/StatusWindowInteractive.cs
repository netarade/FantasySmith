using UnityEngine;
using UnityEngine.EventSystems;
using ItemData;
using UnityEngine.UI;
using System;
using WorldItemData;
using CreateManagement;

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
 * <v2.0 - 2023_1109_최원준>
 * 1- 각인 판넬 추가 및 각인 활성화 조절, 정보 반영
 * 
 * <v2.1 - 2023_1112_최원준>
 * 1- 상태창 위치를 아이템 바로 옆에서 보일 수 있게 수정
 * 2- 상태창이 다른 이미지와 겹치는 경우 가장 앞에 표시될 수 있도록 수정
 * 
 * <v3.0 - 2023_1216_최원준
 * 1- statusWindow의 참조를 PlayerInven에서 참조하던 것을 직접 캐릭터 캔버스를 태그로 찾아 참조하도록 수정
 * 2- 상태창이 떴을 때 포지션을 약간 수정
 * 
 * <v3.1 -2023_1217_최원준>
 * 1- 각인석 정보 구조체에서 인덱스를 받아서 IIC 직렬화 스크립트에서 이미지를 참조하여 각인석 이미지를 반영하도록 수정 
 * 2- iicMiscOther의 참조를 CreateManager의 싱글톤을 통한 참조로 변경
 * 
 * <v4.0 - 2023_1223_최원준>
 * 1- 상태창 위치 조절
 * 인벤토리 아이템이 상단 놓여 있을 때는 하단으로 내려서 보여주고, 하단에 놓여 있을 때는 상단으로 올려서 보여주게 변경
 * 
 * <v4.1 - 2023_1226_최원준>
 * 1- 슬롯리스트 참조가 아이템오브젝트 기준으로 잡혀있던 점 수정
 * 아이템이 슬롯리스트 외부에 생성되어지는 경우 슬롯리스트를 참조하지 못하게 되기 때문
 * 
 * 2- 변수명 SlotListTr을 SlotListRectTr로 수정
 * 
 * 3- GameObject형식의 statusWindow를 Transform형식의 statusWindowTr로 변경
 * 
 * <v4.2 - 2023_1227_최원준>
 * 1- statusWindowTr의 참조를 새로운 임시변수 Transform canvasTr기반으로 변경
 * 
 * <v4.3 - 2023_1229_최원준>
 * 1- 상태창 계층구조 변경 (인벤토리 하위 마지막 자식인덱스)으로 인한 참조 수정
 * 
 * <v4.4 - 2024_0105_최원준>
 * 1- iicMiscOther의 참조를 CreateManager의 싱글톤참조에서 FindWithTag참조로 임시 변경 (향후 수정가능성 있음)
 * 
 * <v5.0 - 2024_0106_최원준>
 * 1- 클래스및 파일명 ItemPointerStatusWindow에서 StatusWindowInteractive로 변경
 * 아이템에 부착하였던 스크립트를 인벤토리에 부착하기로 결정
 * 이유는 아이템은 매번 인벤토리를 옮겨다니기 때문에 상태창의 참조를 매번 다르게 받아서 상태창을 띄워야 하며,
 * 모든 아이템이 상태창 코드를 가지고 있는 것보다 상태창쪽 메서드를 두고 포인터 이벤트 발생 시 해당 메서드를 호출하기만 하면 되기 때문
 * 
 * 
 * 2- 필요없는 변수 삭제 및 변수명 변경
 * 변수명 변경 itemStatusImage->statusImage
 * 변수명 삭제 statusWIndorTr (statusRectTr과 겹치기 때문)
 * 멤버변수 itemInfo, item삭제 (상태창을 띄울 때 ItemInfo를 달리해서 받기 때문)
 * 멤버변수 slotListRectTr 삭제 (상태창을 띄울위치의 판단 기준이되는 변수이나 inventoryRectTr로 대체)
 * 
 * <5.1 - 2024_0107_최원준>
 * 1- InventoryInteractive의 IsItemSelecting 상태에 따라서 상태창을 띄우지 않도록 수정
 * 
 * <v5.2 - 2024_0108_최원준>
 * 1- 각인석의 Sprite이미지 참조값을 직접 찾아서 참조하는 방식에서
 * VisualManager를 통해 인덱스 넘버를 전달하여 참조값을 얻는 방식으로 구현
 * 
 * <v5.3 - 2024_01111_최원준>
 * 1- 서바이벌 장르에 맞게 아이템 클래스 구조 변경으로 인해 각인석관련 코드를 삭제 및 설명을 Desc로 대체
 * 
 */



/// <summary>
/// 이 스크립트는 반드시 아이템 오브젝트(프리팹)에 위치해야 합니다. 아이템의 이벤트와 정보를 받아야 하기 때문입니다.
/// </summary>
public class StatusWindowInteractive : MonoBehaviour
{    
    RectTransform statusRectTr; // 상태창의 렉트 트랜스폼
    Image statusImage;          // 상태창의 아이템 이미지
    Text txtName;               // 상태창의 아이템 이름
    Text txtDesc;               // 상태창의 아이템 설명
    Text txtSpec;               // 상태창의 아이템 고유 수치

    RectTransform inventoryRectTr;                  // 자기자신 렉트 트랜스폼
    InventoryInteractive inventoryInteractive;      // 아이템 셀렉팅 상태를 확인할 스크립트 참조


    void Start()
    {        
        // 하위 오브젝트의 모든 참조는 0번째 자식인 인벤토리 오브젝트의 마지막 자식인 상태창 오브젝트 참조를 기반으로 합니다.
        inventoryRectTr = GetComponent<RectTransform>();
        inventoryInteractive = GetComponent<InventoryInteractive>();
        statusRectTr = inventoryRectTr.GetChild(inventoryRectTr.childCount-1).GetComponent<RectTransform>();

        // 상태창 및 하위 컴포넌트 참조 설정
        statusImage = statusRectTr.GetChild(0).GetChild(0).GetComponent<Image>();
        txtName = statusRectTr.GetChild(1).GetComponent<Text>();
        txtDesc = statusRectTr.GetChild(2).GetComponent<Text>();
        txtSpec = statusRectTr.GetChild(3).GetComponent<Text>();
                      
        // 게임 시작 시 상태창을 꺼둡니다.
        statusRectTr.gameObject.SetActive(false);
    }
    

    /// <summary>
    /// 아이템에 커서를 갖다 대는 순간 아이템의 정보를 볼 수 있습니다.
    /// </summary>
    public void OnItemPointerEnter( ItemInfo itemInfo )
    {
        if(itemInfo == null)
            throw new Exception("해당 아이템의 정보가 전달되지 않았습니다. 확인하여 주세요.");
        
        // 아이템이 셀렉팅 상태라면 보여주지 않습니다.
        if( inventoryInteractive.IsItemSelecting )
            return;


        RectTransform itemRectTr = itemInfo.gameObject.GetComponent<RectTransform>();
        Item item = itemInfo.Item;


        /*** 아이템 종류 상관없이 공통 로직 ***/

        statusRectTr.gameObject.SetActive( true );        // 상태창 활성화


        float delta = inventoryRectTr.position.y-itemRectTr.position.y;         // 상태창을 띄울 위치 판단기준(인벤토리와 아이템의 y위치 차이)
        Vector3 rightPadding = Vector3.right*( statusRectTr.sizeDelta.x/2+30f );  // 상태창 오른쪽 여백
        Vector3 upPadding = Vector3.up*( statusRectTr.sizeDelta.y/2 );              // 상태창 상단 여백


        if( delta<statusRectTr.sizeDelta.y/3 )          // 인벤토리 상단에서 1/3미만 떨어져있다면,
            statusRectTr.position=itemRectTr.position+rightPadding-upPadding;
        else if( delta<statusRectTr.sizeDelta.y*2/3 )   // 인벤토리 상단에서 2/3미만 떨어져 있다면,
            statusRectTr.position=itemRectTr.position+rightPadding;
        else                                             // 인벤토리 상단에서 2/3이상 떨어져 있다면,
            statusRectTr.position=itemRectTr.position+rightPadding+upPadding;

        statusImage.sprite=itemInfo.statusSprite;         // 이미지에 등록한 statusSprite 이미지를 보여준다.
        txtName.text = item.Name;                         // 이름 텍스트에 아이템 이름을 보여준다.
        txtDesc.text = item.Desc;                         // 설명 텍스트에 아이템 설명을 보여준다.
        txtSpec.text = itemInfo.Spec;
    }






    



    /// <summary>
    /// 아이템에서 커서를 떼는 순간 아이템 스테이터스 창이 사라집니다.
    /// </summary>
    public void OnItemPointerExit()
    {
        statusRectTr.gameObject.SetActive( false );      // 상태창 비활성화
    }


}
