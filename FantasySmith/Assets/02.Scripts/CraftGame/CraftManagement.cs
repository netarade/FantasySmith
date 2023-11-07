using UnityEngine;
using UnityEngine.UI;
using ItemData;
using CraftData;
using System.Collections.Generic;
using DataManagement;
using Unity.VisualScripting;
using System.Linq;
using UnityEditor.Build.Content;
using UnityEngine.SceneManagement;
/*
* [�۾� ����]  
* <v1.0 - 2023_1106_�ֿ���>
* 1- ���� 1�ܰ� ���� ����
* 
* <v1.1 - 2023_1106_�ֿ���>
* 1- ������ ������, partial Ŭ������ ���� ����
* 2- ��Ӵٿ����� �����۸� ǥ��
* 3- ��Ӵٿ� �̺�Ʈ �߸� �����ߴ� �� ����
* <v1.2 - 2023_1106_�ֿ���>
* 1- �븮�� �̺�Ʈ�� ���� �޼ҵ� ����
* ȣ������� �ռ��� craftableList�� �������� ���� �ذ�
* 2- �븮�� �޼ҵ� ���� , Start���� �޼ҵ�ȭ
* �׽�Ʈ �������� Start���� �� �۵��ϴ� ���� Ȯ���Ͽ�����,
* ���� ���ӿ����� �÷��̾��� �ൿ ���¿� ���� �ߵ��ϰ� �ǹǷ� �޼ҵ�ȭ �Ͽ���.
* 
*/

/// <summary>
/// ��� ���� ������ ����ϴ� Ŭ�����Դϴ�. ������Ʈ�� �ٿ� Ȱ���ϸ�, ������ �ν��Ͻ� ������ �ʿ����� �ʽ��ϴ�.
/// </summary>
public partial class CraftManagement : MonoBehaviour
{
    private Transform panelCraftLev1Tr;          // ������ ��ġ - Craft 1�ܰ� �ǳ�
    private Dropdown dropWeapType;               // ������ ������ ������ ���ϴ� ù��° ��Ӵٿ� 
    private Dropdown dropWeapItem;               // �ش� ������ �������� ���ϴ� �ι�° ��Ӵٿ�
    private CraftableWeaponList craftableList;   // ���� ���� ���
    private enum OptionIndex { Selecet, Sword, Bow } // �ɼ��� �б� �����ϱ� ���� enum ����

    private Image itemImg;       // �����۸��� ����Ʈ���� �� ǥ�� �� �̹���
    private Text[] itemTxtArr;   // �����۸��� ����Ʈ���� �� ǥ�� �� �ؽ�Ʈ �迭
    private Button nextButton;
    
    private Transform panelCraftLev2Tr;
    
    void Start()
    {
        panelCraftLev1Tr=GameObject.Find( "Panel-CraftLevel1" ).transform;
        panelCraftLev2Tr=GameObject.Find( "Panel-CraftLevel2" ).transform;
        dropWeapType=panelCraftLev1Tr.GetChild( 0 ).GetComponent<Dropdown>();
        dropWeapItem=panelCraftLev1Tr.GetChild( 1 ).GetComponent<Dropdown>();

        dropWeapItem.GetComponent<Image>().enabled=false;                   // Item��Ӵٿ��� �ʱ⿡ ���� ���·� �����Ѵ�.
        nextButton = panelCraftLev1Tr.GetComponentInChildren<Button>();     // ���� ��ư �ν�
        nextButton.onClick.AddListener(NextButtonClick);                    // ���� ��ư�� �̺�Ʈ ����

        dropWeapType.onValueChanged.AddListener( OnTypeChanged );       // ù��° ��Ӵٿ� �̺�Ʈ ����
        dropWeapItem.onValueChanged.AddListener( OnItemChanged );       // �ι�° ��Ӵٿ� �̺�Ʈ ����


        itemImg = panelCraftLev1Tr.GetChild(2).GetComponentInChildren<Image>();     // �������� �̹���
        itemTxtArr = panelCraftLev1Tr.GetChild(2).GetComponentsInChildren<Text>();  // �������� �����ϴ� �ؽ�Ʈ �迭

        // �ǳ��� ��� ���� ������Ʈ�� ���ش�.
        panelCraftLev1Tr.gameObject.SetActive(false);
        panelCraftLev2Tr.gameObject.SetActive(false);
    }

    /// <summary>
    /// �÷��̾��� �ൿ�� ���� ���� ������ �����Ѵ�.
    /// </summary>
    public void OnCraftLevel1Start()
    {
        
        // �ǳ� ����1�� ��� ���� ������Ʈ�� ���ش�.
        panelCraftLev1Tr.gameObject.SetActive(true);

        //��� �̹����� �ؽ�Ʈ�� �������·� �����Ѵ�.
        itemImg.enabled=false;
        foreach(var text in itemTxtArr)
            text.enabled = false;

        craftableList=CraftManager.instance.craftableList;        // ������ ������ �� ���� �ǽð� ���� ���� ����� �ҷ��´�.
    }

    




    /// <summary>
    /// ù ��° ��� �ٿ�� ��� ���� ���� �� ȣ�� - ������ ���� �������� �ٸ��� �����ִ� ������ �Ѵ�. �� ������ �°� ��Ӵٿ� �ɼ��� �߰��Ǿ�� �Ѵ�.
    /// </summary>
    public void OnTypeChanged( int optIdx )
    {
        switch( optIdx )
        {
            case (int)OptionIndex.Selecet:
                dropWeapItem.GetComponent<Image>().enabled=false;     // ���⸦ �����ϼ����� �ؽ�Ʈ�� ������ �ι�° ������ ��Ӵٿ��� �������� �ʴ´�.
                break;
            case (int)OptionIndex.Sword:
            case (int)OptionIndex.Bow:
                dropWeapItem.GetComponent<Image>().enabled=true;      // �ι�° ��Ӵٿ��� Ų��.
                CreateDropdownOptions( optIdx );                          // ��Ӵٿ� �ε����� �°� ���ڸ� �ٸ��� �༭ ȣ���Ų��.
                break;
        }
    }

    public void CreateDropdownOptions( int optIdx )
    {
        // ��Ӵٿ �ɼ� �߰�
        List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();

        switch( optIdx )
        {
            case (int)OptionIndex.Sword:
                for( int i = 0; i<craftableList.swordList.Count; i++ )                        // ��-��� ����Ʈ�� ���� �о���Դϴ�.
                {
                    print( craftableList.swordList[i] );
                    optionList.Add( new Dropdown.OptionData( craftableList.swordList[i] ) ); // ��ư�� �ɼ����� �����մϴ�.
                }
                break;

            case (int)OptionIndex.Bow:
                for( int i = 0; i<craftableList.bowList.Count; i++ )                        // ����-Ȱ ����Ʈ�� ���� �о���Դϴ�.
                {
                    optionList.Add( new Dropdown.OptionData( craftableList.bowList[i] ) ); // ��ư�� �ɼ����� �����մϴ�.
                }
                break;
        }
        dropWeapItem.options=optionList;    // 2��° ��ư�� �ɼ����� ����մϴ�.
    }



    /// <summary>
    /// 2��° ��Ӵٿ�� �ش� ������ ���� �� ȣ��
    /// </summary>
    public void OnItemChanged( int optIdx )
    {
        // ǥ�� �� �̹����� �ؽ�Ʈ�� ���ش�.
        itemImg.enabled=true;
        for(int i=0; i<itemTxtArr.Length; i++)
        {
            itemTxtArr[i].enabled = true;        // ��� �ؽ�Ʈ Ȱ��ȭ
            if(i<=2)
                itemTxtArr[i].text = "";         // ���� �ؽ�Ʈ �ʱ�ȭ
        }


        // ���� ������ ��Ӵٿ��� �ɼ� �̸��� ������� �����Ͽ� ������ ������ �޴´�.
        ItemCraftWeapon weapon = (ItemCraftWeapon)CreateManager.instance.weaponDic[dropWeapItem.options[optIdx].text]; 

        itemImg.sprite = weapon.Image.statusSprite;     // ������ �̹��� ����
        itemTxtArr[0].text =  weapon.Name;              // �����۸� �ؽ�Ʈ ����

        CraftMaterial[] baseMaterials = weapon.BaseMaterials;
        for(int i=0; i<baseMaterials.Length; i++)
        {
            itemTxtArr[1].text += baseMaterials[i].name + ": " + baseMaterials[i].count +"�� ";  // ��� �ؽ�Ʈ ����
        }

        // ���õ� �ؽ�Ʈ ����
        //if(CraftManager.instance.proficiencyDic[weapon.Name].Equals(default) )  // ����ü�� default���
        //    itemTxtArr[2].text = "0 / 100";
        //else 
            itemTxtArr[2].text = CraftManager.instance.proficiencyDic[weapon.Name].Proficiency.ToString() + " / 100";   
    }


    /// <summary>
    /// ���� ��ư�� Ŭ������ ���� ����
    /// </summary>
    public void NextButtonClick()
    {
        panelCraftLev1Tr.gameObject.SetActive(false);
        panelCraftLev2Tr.gameObject.SetActive(true);
    }

}
