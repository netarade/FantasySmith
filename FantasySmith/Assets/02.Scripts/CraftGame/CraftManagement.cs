using UnityEngine;
using UnityEngine.UI;
using ItemData;
using CraftData;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CraftManagement : MonoBehaviour
{
    public Transform panelCraftLev1Tr;      // ������ ��ġ - Craft 1�ܰ� �ǳ�
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

        Debug.Log("Dropdown ���� ����Ǿ����ϴ�. ���õ� �ɼ��� �ε���: " + value);

        // ���õ� �ɼ��� �ؽ�Ʈ�� ���
        string selectedOption = ddWeaponType.options[value].text;
        Debug.Log("���õ� �ɼ�: " + selectedOption);

    }

    public void CreateDropdownOptions(int value)
    {
        // ��Ӵٿ �ɼ� �߰�
        //List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();
        //optionList.Add( new Dropdown.OptionData( "���⸦ �����ϼ���" ) );
        //optionList.Add( new Dropdown.OptionData( "��" ) );
        //optionList.Add( new Dropdown.OptionData( "Ȱ" ) );
        //ddWeaponItem.options=optionList;
    }




     public void OnItemChanged(int value)
    {
        Debug.Log("Dropdown ���� ����Ǿ����ϴ�. ���õ� �ɼ��� �ε���: " + value);

        // ���õ� �ɼ��� �ؽ�Ʈ�� ���
        string selectedOption = ddWeaponType.options[value].text;
        Debug.Log("���õ� �ɼ�: " + selectedOption);

    }


}
