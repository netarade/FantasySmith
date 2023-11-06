using UnityEngine;
using UnityEngine.UI;
using ItemData;
using CraftData;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CraftManagement : MonoBehaviour
{
    public Transform panelCraftLev1Tr;      // 생성할 위치 - Craft 1단계 판넬
    public Dropdown ddWeaponType;
    public Dropdown ddWeaponItem;

    public List<string> craftableList;


    void Start()
    {
        panelCraftLev1Tr = GameObject.Find("Panel-CraftLevel1").transform;
        ddWeaponType = panelCraftLev1Tr.GetChild(0).GetComponent<Dropdown>();
        ddWeaponItem = panelCraftLev1Tr.GetChild(1).GetComponent<Dropdown>();

        ddWeaponType.onValueChanged.AddListener( OnTypeChanged );
        ddWeaponType.onValueChanged.AddListener( OnItemChanged );


        
    }

    public void OnTypeChanged(int value)
    {
        switch(value)
        {
            case 0:
                
                break;

            case 1:
                CreateDropdownOptions(value);
                break;

            case 2:
                CreateDropdownOptions(value);
                break;

        }

        Debug.Log("Dropdown 값이 변경되었습니다. 선택된 옵션의 인덱스: " + value);

        // 선택된 옵션의 텍스트를 출력
        string selectedOption = ddWeaponType.options[value].text;
        Debug.Log("선택된 옵션: " + selectedOption);

    }

    public void CreateDropdownOptions(int value)
    {
        // 드롭다운에 옵션 추가
        //List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();
        //optionList.Add( new Dropdown.OptionData( "무기를 선택하세요" ) );
        //optionList.Add( new Dropdown.OptionData( "검" ) );
        //optionList.Add( new Dropdown.OptionData( "활" ) );
        //ddWeaponItem.options=optionList;
    }




     public void OnItemChanged(int value)
    {
        Debug.Log("Dropdown 값이 변경되었습니다. 선택된 옵션의 인덱스: " + value);

        // 선택된 옵션의 텍스트를 출력
        string selectedOption = ddWeaponType.options[value].text;
        Debug.Log("선택된 옵션: " + selectedOption);

    }


}
