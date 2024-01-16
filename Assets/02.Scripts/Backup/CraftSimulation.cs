//using UnityEngine;
//using UnityEngine.UI;
//using ItemData;
//using CraftData;
//using System.Collections.Generic;
//using DataManagement;
//using Unity.VisualScripting;
//using System.Linq;
//using UnityEditor.Build.Content;
//using UnityEngine.SceneManagement;
///*
//* [�۾� ����]  
//* <v1.0 - 2023_1106_�ֿ���>
//* 1- ���� 1�ܰ� ���� ����
//* 
//* <v1.1 - 2023_1106_�ֿ���>
//* 1- ������ ������, partial Ŭ������ ���� ����
//* 2- ��Ӵٿ����� �����۸� ǥ��
//* 3- ��Ӵٿ� �̺�Ʈ �߸� �����ߴ� �� ����
//* <v1.2 - 2023_1106_�ֿ���>
//* 1- �븮�� �̺�Ʈ�� ���� �޼ҵ� ����
//* ȣ������� �ռ��� craftableList�� �������� ���� �ذ�
//* 2- �븮�� �޼ҵ� ���� , Start���� �޼ҵ�ȭ
//* �׽�Ʈ �������� Start���� �� �۵��ϴ� ���� Ȯ���Ͽ�����,
//* ���� ���ӿ����� �÷��̾��� �ൿ ���¿� ���� �ߵ��ϰ� �ǹǷ� �޼ҵ�ȭ �Ͽ���.
//* 
//* <v1.3 - 2023_1108_�ֿ���>
//* 1- ��ũ��Ʈ ���� ��ġ�� CreateManager���� GameController�� �ű�.
//* 2- 1�ܰ� ��Ӵٿ� ������ ���õ� �̸� �˻翡�� Ű�� ���� ���� �ʴٸ� Ű�� ���õ� ����ü�� �ʱ�ȭ ���ִ� ���� ����
//* 3- ���� 1�ܰ� ���۹�ư �߰�
//* 
//* <1.4 - 2023_1109_�ֿ���>
//* 1- �� �׽�Ʈ ��ư �ű� (�� ��ȯ �� ������ �̵��� ����� �Ǵ��� �׽�Ʈ)
//* 
//* <v2.0 - 2023_1120_�ֿ���>
//* 1- ���ϸ� �� Ŭ������ ���� - CraftManagement->CraftSimulation
//*/

///// <summary>
///// ��� ���� �ùķ��̼��� ����ϴ� Ŭ�����Դϴ�. ������Ʈ�� �ٿ� Ȱ���ϸ�, ������ ������ ���� �ν��Ͻ� ������ �ʿ����� �ʽ��ϴ�.<br/>
///// ��ũ��Ʈ ���� Ÿ�̹��� ĳ���Ͱ� Ư�� ����, ������ ���� ��, Ʈ���� �ߵ������� �ִ� �� ���� ��ũ��Ʈ�̸�, 
///// ���� �ܺ� ������ �ؾ� �ϹǷ� ����ȯ �� �ı������� ������Ʈ�� �δ� ���� �����մϴ�.<br/> 
///// </summary>
//public partial class CraftSimulation : MonoBehaviour
//{
//    private Transform panelCraftLev1Tr;          // ������ ��ġ - Craft 1�ܰ� �ǳ�
//    private Dropdown dropWeapType;               // ������ ������ ������ ���ϴ� ù��° ��Ӵٿ� 
//    private Dropdown dropWeapItem;               // �ش� ������ �������� ���ϴ� �ι�° ��Ӵٿ�
//    private Craftdic craftableList;   // ���� ���� ���
//    private enum OptionIndex { Selecet, Sword, Bow } // �ɼ��� �б� �����ϱ� ���� enum ����

//    private Image itemImg;       // �����۸��� ����Ʈ���� �� ǥ�� �� �̹���
//    private Text[] itemTxtArr;   // �����۸��� ����Ʈ���� �� ǥ�� �� �ؽ�Ʈ �迭
//    private Button nextButton;
    
//    private Transform panelCraftLev2Tr; // 2��° ���� �ǳ�
//    private int lastOptIdx;             // ������ �ɼ��� ���
//    private Text panelCraftLev2Txt;        // 2��° �ǳ� �ؽ�Ʈ
    
//    //private Transform gamePlayTestBtn;          //���� �׽�Ʈ ��ư
//    //private Transform itemCreateBtn;           // ������ ���� ��ư 
//    //private GameObject attributeCreateBtn;      // ��ȭ ������ ���� ��ư

//    void Start()
//    {
//        panelCraftLev1Tr=GameObject.Find( "Panel-CraftLevel1" ).transform;
//        panelCraftLev2Tr=GameObject.Find( "Panel-CraftLevel2" ).transform;
//        panelCraftLev2Txt = panelCraftLev2Tr.GetChild(0).GetComponent<Text>();


//        dropWeapType=panelCraftLev1Tr.GetChild( 0 ).GetComponent<Dropdown>();
//        dropWeapItem=panelCraftLev1Tr.GetChild( 1 ).GetComponent<Dropdown>();

//        dropWeapItem.GetComponent<Image>().enabled=false;                   // Item��Ӵٿ��� �ʱ⿡ ���� ���·� �����Ѵ�.
//        nextButton = panelCraftLev1Tr.GetComponentInChildren<Button>();     // ���� ��ư �ν�
//        nextButton.onClick.AddListener(Craft1MakeBtnClick);                    // ���� ��ư�� �̺�Ʈ ����

//        dropWeapType.onValueChanged.AddListener( OnTypeChanged );       // ù��° ��Ӵٿ� �̺�Ʈ ����
//        dropWeapItem.onValueChanged.AddListener( OnItemChanged );       // �ι�° ��Ӵٿ� �̺�Ʈ ����


//        itemImg = panelCraftLev1Tr.GetChild(2).GetComponentInChildren<Image>();     // �������� �̹���
//        itemTxtArr = panelCraftLev1Tr.GetChild(2).GetComponentsInChildren<Text>();  // �������� �����ϴ� �ؽ�Ʈ �迭

//        // �ǳ��� ��� ���� ������Ʈ�� ���ش�.
//        panelCraftLev1Tr.gameObject.SetActive(false);
//        panelCraftLev2Tr.gameObject.SetActive(false);

//    }

//    /// <summary>
//    /// �÷��̾��� �ൿ�� ���� ���� ������ �����Ѵ�.
//    /// </summary>
//    public void OnCraftLevel1Start()
//    {
//        //�κ��丮�� ���ش�.
//        InventoryManagement manager = GameObject.Find("GameController").GetComponent<InventoryManagement>();
//        manager.InventoryOnOffSwitch(true); 

//        // �ǳ� ����1�� ��� ���� ������Ʈ�� ���ش�.
//        panelCraftLev1Tr.gameObject.SetActive(true);

//        //��� �̹����� �ؽ�Ʈ�� �������·� �����Ѵ�.
//        itemImg.enabled=false;
//        foreach(var text in itemTxtArr)
//            text.enabled = false;

//        craftableList=PlayerInven.instance.craftableList;        // ������ ������ �� ���� �ǽð� ���� ���� ����� �ҷ��´�.    
//     }

        
//    /// <summary>
//    /// ������ �׽�Ʈ ��ư
//    /// </summary>
//    public void BtnNextScene()
//    {
//        switch( SceneManager.GetActiveScene().name )    // �� 1, 2�� ��ȯ�ϴ� ���
//        {
//            case "InventoryTest":
//                SceneManager.LoadScene( "InventoryTest2" );
//                break;
//            case "InventoryTest2":
//                SceneManager.LoadScene( "InventoryTest" );
//                break;
//        }
//    }














//    /// <summary>
//    /// ù ��° ��� �ٿ�� ��� ���� ���� �� ȣ�� - ������ ���� �������� �ٸ��� �����ִ� ������ �Ѵ�. �� ������ �°� ��Ӵٿ� �ɼ��� �߰��Ǿ�� �Ѵ�.
//    /// </summary>
//    public void OnTypeChanged( int optIdx )
//    {
//        switch( optIdx )
//        {
//            case (int)OptionIndex.Selecet:
//                dropWeapItem.GetComponent<Image>().enabled=false;     // ���⸦ �����ϼ����� �ؽ�Ʈ�� ������ �ι�° ������ ��Ӵٿ��� �������� �ʴ´�.
//                break;
//            case (int)OptionIndex.Sword:
//            case (int)OptionIndex.Bow:
//                dropWeapItem.GetComponent<Image>().enabled=true;      // �ι�° ��Ӵٿ��� Ų��.
//                CreateDropdownOptions( optIdx );                          // ��Ӵٿ� �ε����� �°� ���ڸ� �ٸ��� �༭ ȣ���Ų��.
//                break;
//        }
//    }

//    public void CreateDropdownOptions( int optIdx )
//    {
//        // ��Ӵٿ �ɼ� �߰�
//        List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();

//        switch( optIdx )
//        {
//            case (int)OptionIndex.Sword:
//                for( int i = 0; i<craftableList.swordDic.Count; i++ )                        // ��-��� ����Ʈ�� ���� �о���Դϴ�.
//                {
//                    optionList.Add( new Dropdown.OptionData( craftableList.swordDic[i] ) ); // ��ư�� �ɼ����� �����մϴ�.
//                }
//                break;

//            case (int)OptionIndex.Bow:
//                for( int i = 0; i<craftableList.bowDic.Count; i++ )                        // ����-Ȱ ����Ʈ�� ���� �о���Դϴ�.
//                {
//                    optionList.Add( new Dropdown.OptionData( craftableList.bowDic[i] ) ); // ��ư�� �ɼ����� �����մϴ�.
//                }
//                break;
//        }
//        dropWeapItem.options=optionList;    // 2��° ��ư�� �ɼ����� ����մϴ�.
//    }



//    /// <summary>
//    /// 2��° ��Ӵٿ�� �ش� ������ ���� �� ȣ��
//    /// </summary>
//    public void OnItemChanged( int optIdx )
//    {
        

//        // ǥ�� �� �̹����� �ؽ�Ʈ�� ���ش�.
//        itemImg.enabled=true;
//        for(int i=0; i<itemTxtArr.Length; i++)
//        {
//            itemTxtArr[i].enabled = true;        // ��� �ؽ�Ʈ Ȱ��ȭ
//            if(i<=2)
//                itemTxtArr[i].text = "";         // ���� �ؽ�Ʈ �ʱ�ȭ
//        }


//        // ���� ������ ��Ӵٿ��� �ɼ� �̸��� ������� �����Ͽ� ������ ������ �޴´�.
//        ItemCraftWeapon weapon = (ItemCraftWeapon)CreateManager.instance.weaponDic[dropWeapItem.options[optIdx].text]; 

//        itemImg.sprite = weapon.Image.statusSprite;     // ������ �̹��� ����
//        itemTxtArr[0].text =  weapon.Name;              // �����۸� �ؽ�Ʈ ����

//        CraftMaterial[] baseMaterials = weapon.BaseMaterials;
//        for(int i=0; i<baseMaterials.Length; i++)
//        {
//            itemTxtArr[1].text += baseMaterials[i].name + ": " + baseMaterials[i].count +"�� ";  // ��� �ؽ�Ʈ ����
//        }

//        // ���õ� �ؽ�Ʈ ����
//        if( PlayerInven.instance.proficiencyDic.ContainsKey(weapon.Name) )  // ����ü�� �ش� �̸��� Ű�� ���� ���� �ʴٸ�,
//        {
//            PlayerInven.instance.proficiencyDic[weapon.Name] = new CraftProficiency(0,0);  // ��ųʸ��� �ش� �̸��� Ű�� �־��ش�.
//            itemTxtArr[2].text = "0 / 100";
//        }
//        else
//            itemTxtArr[2].text = PlayerInven.instance.proficiencyDic[weapon.Name].Proficiency.ToString() + " / 100";
        
//        lastOptIdx = optIdx;    // ������ �ɼ��� ����Ѵ�.
//    }


//    /// <summary>
//    /// ���� ��ư�� Ŭ������ ���� ����
//    /// </summary>
//    public void Craft1MakeBtnClick()
//    {
//        // �÷��̾� ��� �κ��丮 ����
//        List<GameObject> miscList = PlayerInven.instance.miscList;

//        // ���� ������ ��Ӵٿ��� �ɼ� �̸��� ������� �����Ͽ� ������ ������ �޴´�.
//        ItemCraftWeapon targetWeapon = (ItemCraftWeapon)CreateManager.instance.weaponDic[dropWeapItem.options[lastOptIdx].text];
//        CraftMaterial[] baseMaterials = targetWeapon.BaseMaterials;                   // �⺻ ��� ����ü �迭�� ����
//        Dictionary<string, int> dicMaterial = new Dictionary<string, int>();    // �⺻ ����� �̸��� ���� ������� �ϴ� ��ųʸ� ����
                
//        for(int i=0; i<baseMaterials.Length; i++)
//        {
//             dicMaterial.Add(baseMaterials[i].name, baseMaterials[i].count);    // ��ųʸ��� �ٽ� ��´�.
//        }
//        ItemMisc[] targetMiscItem = new ItemMisc[dicMaterial.Count];            // ��� Ű ���� ������ŭ ��ǥ�� ã�� �������� ������ ���Ѵ�.
//        int foundCount = 0;                                                     // ã�� �� ���� ī��Ʈ�� ����Ѵ�.

        


        


//        // ���� ���������� ������� ���� ����
//        foreach( GameObject itemObject in miscList )  // ������ ������Ʈ�� �ϳ��� ������.
//        {
//            Debug.Log(PlayerInven.instance.inventory.miscList.Count);
//            ItemMisc playerItem = (ItemMisc)( itemObject.GetComponent<ItemInfo>().item ); // ���� ������ ������ �޾ƿ´�.
//            print( playerItem.Name );
//            if( dicMaterial.ContainsKey( playerItem.Name ) ) // ���� ������ �߿� �̸��� ��ġ�ϴ� ���� �ִ��� ����.
//            {
//                if( playerItem.OverlapCount>=dicMaterial[playerItem.Name] )   // ��ᰡ �÷��̾� ���� �� ���ٸ�
//                {
//                    playerItem.OverlapCount-=dicMaterial[playerItem.Name];    // �ϴ� ������ ���� ���ش�.
//                    targetMiscItem[foundCount]=playerItem;                      // ã�� Ÿ�� �迭�� ����Ѵ�.
//                    foundCount++;                                               // ã�� ������ ������Ų��.
//                }
//            }
//        }

//        if( foundCount==dicMaterial.Count )   // ���������� ���� �� Ÿ�� ī��Ʈ�� ����� ī��Ʈ�� ��ġ�Ѵٸ�
//        {
//            panelCraftLev1Tr.gameObject.SetActive( false );     // �ǳ� 1�� ���� 
//            panelCraftLev2Tr.gameObject.SetActive( true );      // �ǳ�2���� ���ش�.
                        
//            panelCraftLev2Txt.text = "���ۿ� �����Ͽ����ϴ�!";
//            CreateManager.instance.CreateItemToNearstSlot(PlayerInven.instance.weapList, targetWeapon.Name);
//            Debug.Log( "���ۿ� �����߽��ϴ�." );
//        }
//        else
//        {
//            for(int i=0; i<foundCount; i++)                             // Ÿ������ ���� Ƚ�������� �ؾ� �Ѵ�.
//            {
//                ItemMisc failItem = targetMiscItem[i];                      // ��ᰡ �����ϴٸ� Ÿ������ �������ٰ� ������ �������� ���̴�.
//                failItem.OverlapCount += dicMaterial[failItem.Name];  // �ٽ� �κ��丮 ī��Ʈ�� ���������� �����ش�.
//            }
            
//            panelCraftLev1Tr.gameObject.SetActive(false);  // �ǳ� 1�� ���� 
//            panelCraftLev2Tr.gameObject.SetActive(true);     // �ǳ�2���� ���ش�.
//            panelCraftLev2Txt.text = "���ۿ� �����Ͽ����ϴ�.";
//            Debug.Log( "���ۿ� �����߽��ϴ�." );
//        }
//        PlayerInven.instance.UpdateInventoryText(true);    // (���� ���ܷ� ����) �������� ������ �ֽ�ȭ ������� �Ѵ�.
//    }

    
//    /// <summary>
//    /// Ȯ�� ��ư �Ǵ� x��ư ������ ��� â�� ������ �� �ִ�.
//    /// </summary>
//    public void BtnPanel2ConfirmExit()
//    {
//        panelCraftLev1Tr.gameObject.SetActive(false);
//        panelCraftLev2Tr.gameObject.SetActive(false);
//    }



//}