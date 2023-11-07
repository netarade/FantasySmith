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
* [작업 사항]  
* <v1.0 - 2023_1106_최원준>
* 1- 제작 1단계 로직 구현
* 
* <v1.1 - 2023_1106_최원준>
* 1- 변수명 재정의, partial 클래스로 파일 분할
* 2- 드롭다운으로 아이템명 표시
* 3- 드롭다운 이벤트 잘못 연결했던 점 수정
* <v1.2 - 2023_1106_최원준>
* 1- 대리자 이벤트를 통한 메소드 연결
* 호출시점이 앞서서 craftableList가 안잡히는 문제 해결
* 2- 대리자 메소드 삭제 , Start로직 메소드화
* 테스트 로직으로 Start에서 잘 작동하는 것을 확인하였으며,
* 실제 게임에서는 플레이어의 행동 상태에 따라서 발동하게 되므로 메소드화 하였음.
* 
*/

/// <summary>
/// 장비 제작 로직을 담당하는 클래스입니다. 컴포넌트로 붙여 활용하며, 별도의 인스턴스 생성이 필요하지 않습니다.
/// </summary>
public partial class CraftManagement : MonoBehaviour
{
    private Transform panelCraftLev1Tr;          // 생성할 위치 - Craft 1단계 판넬
    private Dropdown dropWeapType;               // 제작할 무기의 종류를 정하는 첫번째 드롭다운 
    private Dropdown dropWeapItem;               // 해당 종류의 아이템을 정하는 두번째 드롭다운
    private CraftableWeaponList craftableList;   // 제작 가능 목록
    private enum OptionIndex { Selecet, Sword, Bow } // 옵션을 읽기 쉽게하기 위한 enum 선언

    private Image itemImg;       // 아이템명을 셀렉트했을 때 표시 할 이미지
    private Text[] itemTxtArr;   // 아이템명을 셀렉트했을 때 표시 할 텍스트 배열
    private Button nextButton;
    
    private Transform panelCraftLev2Tr;
    
    void Start()
    {
        panelCraftLev1Tr=GameObject.Find( "Panel-CraftLevel1" ).transform;
        panelCraftLev2Tr=GameObject.Find( "Panel-CraftLevel2" ).transform;
        dropWeapType=panelCraftLev1Tr.GetChild( 0 ).GetComponent<Dropdown>();
        dropWeapItem=panelCraftLev1Tr.GetChild( 1 ).GetComponent<Dropdown>();

        dropWeapItem.GetComponent<Image>().enabled=false;                   // Item드롭다운은 초기에 꺼진 상태로 시작한다.
        nextButton = panelCraftLev1Tr.GetComponentInChildren<Button>();     // 다음 버튼 인식
        nextButton.onClick.AddListener(NextButtonClick);                    // 다음 버튼에 이벤트 연결

        dropWeapType.onValueChanged.AddListener( OnTypeChanged );       // 첫번째 드롭다운 이벤트 연결
        dropWeapItem.onValueChanged.AddListener( OnItemChanged );       // 두번째 드롭다운 이벤트 연결


        itemImg = panelCraftLev1Tr.GetChild(2).GetComponentInChildren<Image>();     // 아이템의 이미지
        itemTxtArr = panelCraftLev1Tr.GetChild(2).GetComponentsInChildren<Text>();  // 아이템을 설명하는 텍스트 배열

        // 판넬의 모든 하위 오브젝트를 꺼준다.
        panelCraftLev1Tr.gameObject.SetActive(false);
        panelCraftLev2Tr.gameObject.SetActive(false);
    }

    /// <summary>
    /// 플레이어의 행동에 따른 제작 로직을 시작한다.
    /// </summary>
    public void OnCraftLevel1Start()
    {
        
        // 판넬 레벨1의 모든 하위 오브젝트를 켜준다.
        panelCraftLev1Tr.gameObject.SetActive(true);

        //모든 이미지와 텍스트는 꺼진상태로 시작한다.
        itemImg.enabled=false;
        foreach(var text in itemTxtArr)
            text.enabled = false;

        craftableList=CraftManager.instance.craftableList;        // 제작을 수행할 때 마다 실시간 제작 가능 목록을 불러온다.
    }

    




    /// <summary>
    /// 첫 번째 드롭 다운에서 장비 종류 선택 시 호출 - 종류에 따라 아이템을 다르게 보여주는 역할을 한다. 각 종류에 맞게 드롭다운 옵션이 추가되어야 한다.
    /// </summary>
    public void OnTypeChanged( int optIdx )
    {
        switch( optIdx )
        {
            case (int)OptionIndex.Selecet:
                dropWeapItem.GetComponent<Image>().enabled=false;     // 무기를 선택하세요라는 텍스트를 누르면 두번째 아이템 드롭다운을 보여주지 않는다.
                break;
            case (int)OptionIndex.Sword:
            case (int)OptionIndex.Bow:
                dropWeapItem.GetComponent<Image>().enabled=true;      // 두번째 드롭다운을 킨다.
                CreateDropdownOptions( optIdx );                          // 드롭다운 인덱스에 맞게 인자를 다르게 줘서 호출시킨다.
                break;
        }
    }

    public void CreateDropdownOptions( int optIdx )
    {
        // 드롭다운에 옵션 추가
        List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();

        switch( optIdx )
        {
            case (int)OptionIndex.Sword:
                for( int i = 0; i<craftableList.swordList.Count; i++ )                        // 검-장검 리스트를 전부 읽어들입니다.
                {
                    print( craftableList.swordList[i] );
                    optionList.Add( new Dropdown.OptionData( craftableList.swordList[i] ) ); // 버튼의 옵션으로 생성합니다.
                }
                break;

            case (int)OptionIndex.Bow:
                for( int i = 0; i<craftableList.bowList.Count; i++ )                        // 보우-활 리스트를 전부 읽어들입니다.
                {
                    optionList.Add( new Dropdown.OptionData( craftableList.bowList[i] ) ); // 버튼의 옵션으로 생성합니다.
                }
                break;
        }
        dropWeapItem.options=optionList;    // 2번째 버튼의 옵션으로 등록합니다.
    }



    /// <summary>
    /// 2번째 드롭다운에서 해당 아이템 선택 시 호출
    /// </summary>
    public void OnItemChanged( int optIdx )
    {
        // 표시 할 이미지와 텍스트를 켜준다.
        itemImg.enabled=true;
        for(int i=0; i<itemTxtArr.Length; i++)
        {
            itemTxtArr[i].enabled = true;        // 모든 텍스트 활성화
            if(i<=2)
                itemTxtArr[i].text = "";         // 설명 텍스트 초기화
        }


        // 월드 사전에 드롭다운의 옵션 이름을 기반으로 접근하여 아이템 정보를 받는다.
        ItemCraftWeapon weapon = (ItemCraftWeapon)CreateManager.instance.weaponDic[dropWeapItem.options[optIdx].text]; 

        itemImg.sprite = weapon.Image.statusSprite;     // 아이템 이미지 지정
        itemTxtArr[0].text =  weapon.Name;              // 아이템명 텍스트 지정

        CraftMaterial[] baseMaterials = weapon.BaseMaterials;
        for(int i=0; i<baseMaterials.Length; i++)
        {
            itemTxtArr[1].text += baseMaterials[i].name + ": " + baseMaterials[i].count +"개 ";  // 재료 텍스트 지정
        }

        // 숙련도 텍스트 지정
        if(CraftManager.instance.proficiencyDic[weapon.Name].Equals(default) )  // 구조체가 default라면
            itemTxtArr[2].text = "0 / 100";
        else 
            itemTxtArr[2].text = CraftManager.instance.proficiencyDic[weapon.Name].Proficiency.ToString() + " / 100";   
    }


    /// <summary>
    /// 다음 버튼을 클릭했을 때의 로직
    /// </summary>
    public void NextButtonClick()
    {
        panelCraftLev1Tr.gameObject.SetActive(false);
        panelCraftLev2Tr.gameObject.SetActive(true);
    }

}
