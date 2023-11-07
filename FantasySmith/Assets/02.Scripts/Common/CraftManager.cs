using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using CraftData;
using DataManagement;
using UnityEngine.UI;


/*
 * [�۾� ����]  
 * <v1.0 - 2023_1106_�ֿ���>
 * 1- �ʱ� Ŭ���� ����
 * ���۰��� �������� �����ϵ��� ����, �̱������� ���ټ� ����
 * <v1.1 - 2023_1106_�ֿ���>
 * 1- ���۸���� List���� Ŭ������ ���� �������� ���� ����
 * 2- �ּ�����
 * 
 * <v1.2 - 2023_1106_�ֿ���>
 * 1- Start�� ������ OnEnable�� ����. 
 * �ٸ� ��ũ��Ʈ���� Start�������� instance�� �����Ͽ� ������ �޾ư��� �ֱ� ����.
 * 2- ����ȭ �ȵǴ� ������ �߻��Ͽ� �ٽ� OnEnable���� Start�� ����.
 * 
 * <v2.0 - 2023_1107_�ֿ���>
 * 1- ���� �ʱ�ȭ�� �̷������ ��ũ��Ʈ���� ������ �޾ư��Ƿ� Awake������ �ű�.
 * �븮�ڸ� �̿��Ͽ� �������� �ּ�ȭ�Ͽ� ������� �ʱ�ȭ�� �̷�������� ��. 
 */



/// <summary>
/// ���� ���� �� ���� ���� �ǽð� �÷��̾� ���� ���� �����ϰ� �ִ� Ŭ�����Դϴ�. �̱������� ���� �����մϴ�.
/// </summary>
public class CraftManager : MonoBehaviour
{
    public static CraftManager instance;

    /// <summary>
    /// �÷��̾ ���۰����� ��� �˷��ִ� ����Դϴ�. string name�� ������� �ϴ� ���� ������ ����Ʈ�� �����Ǿ��ִ� Ŭ�����Դϴ�.<br/> 
    /// ���� �߿� ��������� �ִٸ� �� ������ �����ؾ� �մϴ�.
    /// </summary>
    public CraftableWeaponList craftableList;

    /// <summary>
    /// �÷��̾ �����ϰ� �ִ� ��� ���õ� ����Դϴ�. name�� CraftProficincy����ü�� ���� �����Ͽ� ������ ������ �����ϰ� ���ݴϴ�<br/>
    /// ���� �߿� ��������� �ִٸ� �� ������ �����ؾ� �մϴ�. 
    /// </summary>
    public Dictionary<string, CraftProficiency> proficiencyDic;

    /// <summary>
    /// �÷��̾ �����ϰ� �ִ� �κ��丮 ���� ������ ���� Ŭ�����Դϴ�. <br/>
    /// ���� �߿� ��������� �ִٸ� �� ������ �����ؾ� �մϴ�.
    /// </summary>
    public PlayerInventory inventory;


    /*** ������ ������ ���� �߰� �����Ͽ���. ***/

    /// <summary>
    /// �÷��̾� �κ��丮 �߿��� �������� ���ӿ�����Ʈ�� �����ϴ� ����Ʈ�Դϴ�. <br/>
    /// ���� �߿� ��������� �ִٸ� �� ������ �����ؾ� �մϴ�. (inventory�� ���������� �Ǿ��ֽ��ϴ�.)
    /// </summary>
    public List<GameObject> weapList;

    /// <summary>
    /// �÷��̾� �κ��丮 �߿��� ��ȭ������ ���ӿ�����Ʈ�� �����ϴ� ����Ʈ�Դϴ�. <br/>
    /// ���� �߿� ��������� �ִٸ� �� ������ �����ؾ� �մϴ�. (inventory�� ���������� �Ǿ��ֽ��ϴ�.)
    /// </summary>
    public List<GameObject> miscList;

    /// <summary>
    /// ���� �߿� ����ϴ� ��� �÷��̾� ���� �����Դϴ�.
    /// </summary>
    public float playerGold; 
    /// <summary>
    /// ���� �߿� ����ϴ� �ǹ� �÷��̾� ���� �����Դϴ�.
    /// </summary>
    public float playerSilver;      

    public delegate void LoadEvent();
    // �̺�Ʈ ����
    public static event LoadEvent CraftManagerLoaded;
        
    int beforeCount;

    void Awake()
    {
        if( instance==null )
            instance=this;
        else
            Destroy( this.gameObject );

        DontDestroyOnLoad( instance );

        if( PlayerPrefs.GetInt("IsContinuedGamePlay") != 1 )   
            CreateManager.PlayerInitCompleted += CraftManagerInit;    // ������ ó�������� �̺�Ʈ ������ ���� ȣ���ϰ�,
        else
            CraftManagerInit();                                       // ���Ŀ��� Awake������ �ٷ� ������ �ҷ����δ�.   
    }

    /// <summary>
    /// CreateManager�� Awake�� ���Ŀ� �����͸� �ε��Ͽ� �ʱ�ȭ�� �̷�� �����Դϴ�.
    /// </summary>
    private void CraftManagerInit()
    {
        DataManager dataManager = new DataManager();
        GameData loadData = dataManager.LoadData();

        proficiencyDic=loadData.proficiencyDic;     // �÷��̾� ���� ��� �ҷ�����
        craftableList=loadData.craftableWeaponList; // �÷��̾� ���� ��� �ҷ�����
        
        inventory=loadData.inventory;                // �÷��̾� �κ��丮 �ҷ�����
        miscList=inventory.miscList;
        weapList=inventory.weapList;                // ������ ������ ���� ���� �߰� ���� �Ͽ���.

        playerGold = loadData.gold;
        playerSilver = loadData. silver;

        beforeCount = miscList.Count;
    }


    

    /// <summary>
    /// �÷��̾��� �κ��丮�� ��� �ִ� �������� ���� ������ �ֽ�ȭ �Ͽ� �ݴϴ�. removeMode�� true�� �� ��ø Ƚ���� ������ �������� �����մϴ�.
    /// </summary>
    public void UpdateInventoryText(bool removeMode=true)
    {      
            
        Text countText;
        for(int i=0; i<miscList.Count; i++)
        { 
            ItemMisc item = (ItemMisc)miscList[i].GetComponent<ItemInfo>().item;           
           
            countText = miscList[i].GetComponentInChildren<Text>();
            countText.text = item.InventoryCount.ToString();        // ��ø ī��Ʈ �ؽ�Ʈ�� ����ȭ

            if(removeMode && item.InventoryCount<=0)   // ��������̰� ��øȽ���� 0�����̸�
            {
                GameObject obj = miscList[i];   // ���� Ž������ ������Ʈ�� ���
                miscList.RemoveAt(i);           // ����Ʈ���� ����
                Destroy(obj);                   // ���� ������Ʈ ����
            }            
        }
    }





    void Update()
    {
        if( beforeCount != miscList.Count)
        {
            print( "misc����Ʈ�� ������ �Ͼ���ϴ�." );
            print(miscList[miscList.Count-1].GetComponent<ItemInfo>().item.Name);
            beforeCount = miscList.Count;
        }
    }


    /// <summary>
    /// �׽�Ʈ ��ư ���� - ���� ���̺� �ε尡 ����� ���� �ʾ� ��ư�� ���� �׽�Ʈ�� ���� ���Դϴ�.
    /// </summary>
    public void OnApplicationQuit()
    {
        DataManager dataManager = new DataManager();
        GameData saveData = dataManager.LoadData();

        saveData.proficiencyDic=proficiencyDic;     // �÷��̾� ���� ��� ����
        saveData.craftableWeaponList=craftableList; // �÷��̾� ���� ��� ����
        saveData.inventory=inventory;               // �÷��̾� �κ��丮 ����
        saveData.gold = playerGold;
        saveData.silver = playerSilver;                   // ��ȭ�� ��ȭ ����

        dataManager.SaveData( saveData );

        #if UNITY_EDITOR
            PlayerPrefs.SetInt("IsContinuedGamePlay", 0);   //���� ���̺� �ε尡 �Ҿ����ϹǷ� �Ͻ������� �ʱ�ȭ
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
