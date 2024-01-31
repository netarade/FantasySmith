using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public void OnInventoryConnect(bool isConnect)
    {

    }

    public void OnItemEquip(bool isEquip)
    {
        Debug.Log("장착 액션 실행");
    }
}
