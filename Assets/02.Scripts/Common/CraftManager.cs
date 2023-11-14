using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using CraftData;
using DataManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
 * 
 * <v2.1 - 2023_1108_�ֿ���>
 * 1- �������� ���� �Ѿ�� �ı��Ǳ� ������ ��ũ��Ʈ�� �����Ϸ��� �Ͽ����� ��ũ��Ʈ ���� �ı��Ǵ� ���� �߻�
 * => ��ųʸ��� �̿��ؼ� �̸��� ������ �����ϰ� ���Ӱ� �����ϴ� ��ü ������� ����. 
 * 
 * 2- �� �ε� �� ������ �ϳ��� ��Ḹ ����ִ� ���� �߻�
 * => �������ʿ��� �ı��ɋ����� ��ųʸ��� �����߾��� ������ CraftManager�ʿ��� �������� �ѹ� �̸� �������ֵ��� ����
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
    public Inventory inventory;


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
        



    // ���� �Ѿ �� ItemInfo ��ũ��Ʈ�� �Ѱ��ֱ� ���� ��ųʸ�
    public Dictionary<string, int> weapSaveDic;
    public Dictionary<string, int> miscSaveDic;


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


        SceneManager.sceneLoaded += OnSceneLoaded; // ���̷ε�ɶ� ���Ӱ� ������ ����ش�.
        SceneManager.sceneUnloaded += OnSceneUnloaded; // ���̷ε�ɶ� ���Ӱ� ������ ����ش�.

        
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
    }



    
    /// <summary>
    /// ���� �ε�Ǿ��� �� �ٽ� �������ִ� ����
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if( miscSaveDic==null && weapSaveDic==null ) // ���ʿ� ��ųʸ��� ���ٸ� ���θ����.
        {
            weapSaveDic=new Dictionary<string, int>();
            miscSaveDic=new Dictionary<string, int>();
            return;
        }

        weapList.Clear();   // ������ ����Ʈ Ŭ����
        miscList.Clear();

        print(miscSaveDic.Count);
        print(weapSaveDic.Count);
        // ��ųʸ��� �����Ͽ� ������ ������Ʈ�� �����.
        foreach( KeyValuePair<string, int> pair in miscSaveDic )
        {
            CreateManager.instance.CreateItemToNearstSlot( miscList, pair.Key, pair.Value );
        }

        foreach( KeyValuePair<string, int> pair in weapSaveDic )
        {
            CreateManager.instance.CreateItemToNearstSlot( weapList, pair.Key );
        }
        
        CraftManager.instance.weapSaveDic=new Dictionary<string, int>(); // ���ο� ��ųʸ��� ����� �д�
        CraftManager.instance.miscSaveDic=new Dictionary<string, int>(); // ���ο� ��ųʸ��� ����� �д�.
    }
    
    private void OnSceneUnloaded( Scene scene )
    {
        Debug.Log("��ε� �Ǿ����ϴ�.");
        
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded-=OnSceneLoaded; // ���̷ε�ɶ� ���Ӱ� ������ ����ش�.
        SceneManager.sceneUnloaded-=OnSceneUnloaded; // ���̷ε�ɶ� ���Ӱ� ������ ����ش�.
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



    /// <summary>
    /// �׽�Ʈ ��ư ���� - ���� ���̺� �ε尡 ����� ���� �ʾ� ��ư�� ���� �׽�Ʈ�� ���� ���Դϴ�.
    /// </summary>
    public void OnApplicationQuit()
    {
        //DataManager dataManager = new DataManager();
        //GameData saveData = dataManager.LoadData();

        //saveData.proficiencyDic=proficiencyDic;     // �÷��̾� ���� ��� ����
        //saveData.craftableWeaponList=craftableList; // �÷��̾� ���� ��� ����
        //saveData.inventory=inventory;               // �÷��̾� �κ��丮 ����
        //saveData.gold = playerGold;
        //saveData.silver = playerSilver;                   // ��ȭ�� ��ȭ ����

        //dataManager.SaveData( saveData );

        #if UNITY_EDITOR
            PlayerPrefs.SetInt("IsContinuedGamePlay", 0);   //���� ���̺� �ε尡 �Ҿ����ϹǷ� �Ͻ������� �ʱ�ȭ
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
