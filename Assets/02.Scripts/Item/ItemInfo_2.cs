using ItemData;
using System;
using UnityEngine;


public partial class ItemInfo : MonoBehaviour
{
    /*
     * [작업 사항]
     * 
     * <v1.0 - 2023_1102_최원준>
     * 1- 아이템의 SetOverlapCount, Remove, IsEnoughOverlapCount메서드 ItemInfo클래스에서 옮겨옴
     * 아이템 수량을 증감시키거나, 삭제시키거나, 수량이 충분환지 확인하는 기능
     * 
     * 2- 아이템의 SetOverlapCount, IsEnoughOverlapCount 메서드 삭제
     * 이유는 아이템 자체적인 삭제나, 정보검색 기능을 넣게되면, 인벤토리가 있는 상태와 없는상태를 구분해서 코드를 넣어야 하기 때문이고,
     * 인벤토리를 통하지 않으면 정보의 동기화 오류가 발생할 가능성이 크기 때문
     * 
     * 3- Remove메서드도 삭제
     * Inventory 내부적으로 제거후 ItemInfo를 반환하면 그다음 삭제하면 딜레이를 줄 필요가 없기때문 
     * 
     */


    


    

}
