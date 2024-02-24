using ItemData;
using UnityEngine;

// <SUmmary> 현재 플레이어가 착용하고 있는 장비의 종류 체크하는 클래스
public class PlayerWeapon : MonoBehaviour
{
    // +++ 2024_0214_최원준(메서드추가)
    /// <summary>
    /// 인자로 전달한 무기의 종류에 해당 하는 플레이어 상태를 애니메이션을 재생하지 않고 즉시 설정합니다.<br/>
    /// 로드 시 아이템 자동 장착 및 퀵슬롯의 즉시 장착 해제 시 사용됩니다.
    /// </summary>
    public void ChangeWeaponDirectly(WeaponType weaponType)
    {
        
    }


    // +++ 2024_0214_최원준(메서드추가)
    /// <summary>
    /// 아이템의 무기 종류에 따라 해당 되는 weapon값을 직접 설정하고 관련 상태를 수정합니다.<br/>
    /// PlayerWeapon스크립트의 ChangeWeaponDirectly메서드에서 내부적으로 사용됩니다.
    /// </summary>
    private void SetWeaponState(WeaponType weaponType)
    {
        
    }



}
