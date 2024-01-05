using UnityEngine;
using InventoryManagement;
using DataManagement;
using ItemData;
using System;
using System.Collections.Generic;

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
 * 
 * <v3.0 - 2023_1120_�ֿ���>
 * 1- CraftManager Ŭ���� �� ���ϸ� ���� PlayerInven, ������ġ �̵� Common���� -> Player����
 * 
 * 2- PlayerInven Ŭ���� ���� ����
 * a. �÷��̾��� �κ��丮 �����͸� �����ϴ� ����
 * b. ���ӽ��� �� �������� �� �÷��̾� �κ��丮�� �ε� �� ���̺����ָ�, �� ��ȯ�� �κ��丮�� �����ϰ� ����� �Ѵ�.
 * c. ���� �÷��̾� ������Ʈ�� ��ũ��Ʈ�� �پ��־�� �Ѵ�. (�÷��̾ ����ִ� �����͸����� �� ���̴�.)
 * d. ������Ʈ�� �ı��� ��(���� ���� ��)
 * 
 * 3- weapList, miscList�� ���� (inventory�ʿ� ����� �߰��Ͽ� ���ټ��� ���� ��ȹ�̹Ƿ�)
 * 4- ��Ÿ ������ ���� �� �ּ�ó�� (�̿Ϸ�)
 * 5- UpdateInventoryText �޼��� �ּ�ó�� - inventory�ʿ� �ش� ����� �߰� �� �����̹Ƿ�
 * 
 * <v3.1 - 2023_1122_�ֿ���>
 * 1- �κ��丮 �� ���û��� �ּ� ���� ����
 * 
 * <v4.0 - 2023_1216_�ֿ���>
 * 1- slotListTr�׸��� �߰� - CreateManager�� PlayerInven ��ũ��Ʈ�� �����Ͽ� ������ų �ʿ伺 ����
 * 2- �̱��� ���� - �ٸ� ��ũ��Ʈ�� start ���� ��� ������ �� �� �̱����� �׺��� �� ������ �ʱ�ȭ���� ������ �ȵǴµ�
 * OnSceneLoad�� �ʱ�ȭ�� �ϴٺ��� ������ ����
 * 
 * <v4.1 - 2023_1217_�ֿ���>
 * 1- ��ũ��Ʈ ����ȭ, LoadPlayerData, SavePlayerData �޼��� �߰��Ͽ� 
 * Awake,OnDestroy,OnApplicationQuit���� ȣ��ǵ��� ����
 * 
 * 2- ���ӸŴ��� �̱��濡�� isNewGame���¸� �о�ͼ� ���ο� �κ��丮�� �����ϴ� ���� �߰�
 * 
 * <v4.2 - 2023_1221_�ֿ���>
 * 1- OnApplicationQuit �޼��峻�ο� �����Ϳ� ���α׷� ���� ������ �־��� �� ���� 
 * 2- ���ӸŴ����� �ν��Ͻ��� isNewGame�� �Ǻ��ϴ� ������ PlayerPrefs�� Ű�� ������ ����
 * ������ PlayerInven ��ũ��Ʈ�� Awake�� ���� �ʱ�ȭ�� �ؾ��ϴµ� �̱��������� ���ٽ����� ���Ƽ� �ҷ����� ��Ʊ� ���� 
 * 3- �׽�Ʈ�� Ű���� ���� PlayerPrefs.DeleteAll �߰�
 * 
 * <v5.0 - 2023_1222_�ֿ���>
 * 1- Ŭ������ ���ϸ��� PlayerInven���� InventoryInfo�� ���� 
 * (ItemInfo�� �̸��� �ϰ����� ���߱� ����)
 * 
 * <v5.1 - 2023_1222_�ֿ���>
 * 1- Awkae������ PlayerPrefs�� Ű���� ���� ó�����۰� �̾��ϱ⸦ �����ؼ� �޼��带 ȣ���ϴ� ������ �����ϰ�
 * LoadData�ϳ��� ȣ���ص� ó�� �����Ͱ� ����������� �����Ͽ����ϴ�.
 * 
 * <v5.2 - 2023_1224_�ֿ���>
 * 1- InitPlayerData() �޼��� ����
 * 2- LoadPlayerData() �޼��带 Start������ �ű�
 * ������ �κ��丮�� ������ȭ �� �� ���������� CreateManager�� �̱����� �޼��带 �ҷ����� ������ ȣ������� ���� �ʿ䰡 ����
 * 
 * <V5.3 - 2023_1226_�ֿ���>
 * 1- �κ��丮 �ε�� LoadAllItem�޼��忡�� UpdateItemInfo�޼��� ȣ��� ����
 * <v5.4 - 2023_1226_�ֿ���>
 * 1- �κ��丮 ���̺� �ε�޼��带 �Ϲ�ȭ �޼��� ȣ��� ����
 * 
 * <v6.0 - 2023_1229_�ֿ���>
 * 1- craftDic, gold, silver�� �������� ���� ���� �� ���� �ε嵵 inventory�� �ҷ������� ����
 * 
 * <v6.1 - 2023_1230_�ֿ���>
 * 1- slotListTr���� �߰� - �κ��丮�� �ڽ��� ���Ը�� �ּҸ� �����ϵ��� �Ͽ���
 * 
 * 2- InventoryInteractive���� ���� �������� �����ϴ� �ڵ带 �Űܿ�
 * ������ ������ �ø��ų� ���̰ų� �ϴ� �޼��带 �����, ������ �ݿ��ϱ� ����
 * 
 * 
 * <v6.2 - 2024_0101_�ֿ���>
 * 1-Inventory�� AddItem, RemoveItem�޼��带 �߰� (���� ���Ǵ� �߰�����)
 * ���� �κ��丮�� ����ó���ϰ�, ����� ������ �ֱ�����
 * 
 * 2-Inventory�� FindNearstSlotIdx�޼��� �߰�
 * (ItemInfo���� �ڽ��� ���� �κ��丮�� ���� ����� ���� �ε����� ��ȯ�ޱ� ���� �ʿ�)
 * 
 * 3- isSlotEnough�޼��� �߰�
 * ItemInfo���� �κ��丮 ������ ������Ʈ �ϱ� ������ ���� �� ������ �ִ����� Ȯ���ϰ� �����ϴ� ������ �ʿ�
 * 
 * 
 * (���� ����)
 * 1- �κ��丮�� ������ �����Ǿ����� �ݿ��ϴ� �̺�Ʈ ���� �� ȣ��
 * �κ��丮 ������ �����ϴ� Interactive��ũ��Ʈ���� ����� �� �ֵ��� �ϱ� ���Ͽ�
 * 
 * 
 * 
 * 
 * <v7.0 - 2024_0103_�ֿ���>
 * 1- AddItem, RemoveItem, CreateItem�޼��� Info2Ŭ������ ����ó��
 * 
 * 2- DataManager �ν��Ͻ� ������Ŀ��� ������Ʈ ���� ������� ����
 * (��Ƽ���� ��ũ��Ʈ���� �ν��Ͻ� ������ ����)
 * 
 * 3- �����ε��޼��� isLatestReduce �������� isLatestModify�� ���� (InventoryŬ������ ����)
 * 
 * 4- UpdateAllItemInfo�޼��� ���Ӱ� ����
 * �ε� �� ��� �������� ������Ʈ ������ �ֽ�ȭ�ϱ� ���� ȣ��
 * 
 * <7.1 - 2024_0104_�ֿ���>
 * 1- IsItemEnough���� ItemPair ����ü�ϳ��� �޴� �����ε� �޼��� ����
 * (������Ʈ 1���� �̸��� ������ �����ϴ� �޼��尡 ���� �ֱ� �����̰�, 
 * AddItem�޼��忡�� ItemPiar�迭�� ItemPair�� �޴� ����ü�� �߰��ϴٺ��� �ڵ尡 ������� ����)
 * 
 * <v7.2 - 2024_0105_�ֿ���>
 * 1- ������ �ϳ��� ������ ������Ʈ�ϴ� UpdateItemInfo�޼��� ����
 * ������ �߰� �� ���� ���� ������ �Ͼ �� ItemInfo ������ ���� ������Ʈ �޼��带 ȣ���ϸ�Ǹ�,
 * ���԰� ����� itemInfo ������ ������Ʈ�� ȣ������ ���̱� ����
 * 
 * 2- ��ŸƮ���� �κ��丮�� �ε��� �� Deserialize�޼��� ȣ�� �� createManager�������� �����ϵ��� ����
 * �̴� �κ��丮 Ŭ������ ���� createManager�� ã�� �� ���� ����
 * 
 * 3- UpdateAllItemInfo�޼��忡��
 * �ʱ� ��ųʸ��� ���� ���� ��쿡�� Update�޼��带 ȣ���ϴ� ���� ����
 * 
 * 4- IsSlotEnough���� ItemType�� �޴� �����ε� �޼��带 IsSlotEnoughIgnoreOverlap�κ���,
 * itemName�� overlapCount�� �޴� �޼��带 IsSlotEnough�� ����
 * ItemInfo�� �����޴� IsSlotEnough�߰�
 * 
 * 5- SetItemSlotIdxBothToNearstSlot���ο��� IsSlotEnough�� ItemType��� ȣ�⿡�� ItemInfo��� ȣ��� ����
 * -> ��ȭ�������� ��� �����ε����� �ʿ����� ���� ��찡 �ִ�.
 * 
 * (�̽�)
 * ���� �������� �ְ� �ε��� ������ �־��ִ� ��������
 * �ε��� ������ ���� ���ؼ� �־��ְ�, �������� �־��ִ� ������ �����ϰ� �Ǿ���.
 * ���� ��ȭ�������� �ε��� ������ ���� ���� �󽽷� �������� ���ϰ� �Ǵµ�,
 * �̸� ���� ���Կ� ������ ������ ���θ� Ȯ���ϰ� ������ ��á�� �� -1�� ��ȯ���� �ʵ��� �ؾ� �Ѵ�.
 * => (����) ��ȭ �������� ó��if������ SlotEnough���� ����Ͽ� �ε��� ������ -1�� ������
 * AddItem�� �� �ı��ǹǷ� �������δ�.
 * 
 * 
 * 
 * 
 */



/// <summary>
/// ���� ���� �� ���� ���� �ǽð� �÷��̾� ���� ���� �����ϰ� �ִ� ���� ���� Ŭ�����Դϴ�.<br/>
/// �ν��Ͻ��� �����Ͽ� ������ Ȯ���Ͻñ� �ٶ��ϴ�.
/// </summary>
public partial class InventoryInfo : MonoBehaviour
{
    /// <summary>
    /// �÷��̾ �����ϰ� �ִ� �������� �����ϴ� �κ��丮�� ���õ� ������ ������ �ִ� Ŭ�����Դϴ�.<br/>
    /// �κ��丮�� �������� �����ϰ� �����ϰų� ���� �������� �˻� ��� ���� ������ �ֽ��ϴ�.<br/>
    /// ��ųʸ� ���ο� ���� ������Ʈ�� �����ϰ� �����Ƿ� �� ��ȯ�̳� ���̺� �ε� �ÿ� �ݵ�� Item ������ List���� Convert�� �ʿ��մϴ�.
    /// </summary>
    public Inventory inventory;

    /// <summary>
    /// ���� �κ��丮�� �����ϴ� ���� ����Ʈ�� Transform �����Դϴ�.
    /// </summary>
    public Transform slotListTr;
        
    GameObject slotPrefab;              // ������ �������� �����ϱ� ���� ������ ����
    InventoryInteractive interactive;   // �ڽ��� ���ͷ�Ƽ�� ��ũ��Ʈ�� �����Ͽ� Ȱ��ȭ �������� �޾ƿ��� ���� ���� ����
    DataManager dataManager;            // ����� �ε� ���� �޼��带 ȣ�� �� ��ũ��Ʈ ����
    CreateManager createManager;        // ������ ������ ��û�ϰ� ��ȯ���� ��ũ��Ʈ ����
    






    void Awake()
    {        
        slotListTr = transform.GetChild(0).GetChild(0).GetChild(0);
        slotPrefab = slotListTr.GetChild(0).gameObject;
        
        // �÷��̾� �κ��丮 ����(��ü �� ���� ĭ��)�� �����Ͽ� ������ �������� ���� (���� �κ��丮�� ������ �� �� ��������Ƿ� �ϳ��� ���ϰ� ����)
        for( int i = 0; i<inventory.SlotCountLimitAll-1; i++ )
            Instantiate( slotPrefab, slotListTr );

        interactive = gameObject.GetComponent<InventoryInteractive>();                          // �ڽ��� ���ͷ�Ƽ�� ��ũ��Ʈ�� �����մϴ�.
        dataManager = GameObject.FindWithTag("GameController").GetComponent<DataManager>();     // ������Ʈ�ѷ� �±װ� �ִ� ������Ʈ�� ������Ʈ ����
        dataManager.FileSettings("Inventory", 0);                                               // ���̺� ���ϸ�, ���Թ�ȣ
        createManager = dataManager.gameObject.GetComponent<CreateManager>();                   // ������ �Ŵ����� ������ ������Ʈ�� ������Ʈ ����
    }

    /// <summary>
    /// �ν��Ͻ��� ���Ӱ� ������ ������ ����� ������ �ҷ��ɴϴ�.<br/>
    /// �κ��丮�� ������ȭ �� �� ���������� CreateManager�� �̱����� �޼��带 ȣ���ϹǷ� Start������ �÷��̾� �����͸� �ҷ��ɴϴ�.
    /// </summary>
    void Start()
    {        
        LoadPlayerData();       // ����� �÷��̾� �����͸� �ҷ��ɴϴ�.        
    }
        

    /// <summary>
    /// �ı� �Ǳ����� ���Ͽ� �����մϴ�.
    /// </summary>
    private void OnDestroy()
    {
        SavePlayerData();   // �÷��̾� ������ ����
    }
       
    /// <summary>
    /// �÷��̾ ������ ������ ��
    /// </summary>
    public void OnApplicationQuit()
    {
        SavePlayerData(); // �÷��̾� ������ ����
    }


    /// <summary>
    /// �κ��丮 ���� �÷��̾� �����͸� �ҷ��ɴϴ�
    /// </summary>
    void LoadPlayerData()
    {
        InventorySaveData loadData = dataManager.LoadData<InventorySaveData>();                
        inventory=loadData.savedInventory.Deserialize(createManager);
        UpdateAllItemInfo();
    }


    /// <summary>
    /// �κ��丮 ���� �÷��̾� �����͸� �����մϴ� 
    /// </summary>
    void SavePlayerData()
    {        
        // �޼��� ȣ�� ������ �ٸ� ��ũ��Ʈ���� save���� ���� �����Ƿ� ���Ӱ� �������� �ʰ� ���� ������ �ֽ�ȭ�մϴ�
        InventorySaveData saveData = dataManager.LoadData<InventorySaveData>();
        saveData.savedInventory.Serialize(inventory);
        dataManager.SaveData<InventorySaveData>(saveData);
    }


    /// <summary>
    /// �� �κ��丮�� �������� �ε��Ͽ��� ��, �ش� �κ��丮 ������ ��� �����ۿ� �κ��丮 ������ �����Ͽ� 
    /// �̹����� ��ġ ���� ���� �ֽ�ȭ�ϱ� ���� �޼����Դϴ�.<br/>
    /// ���������� ��� �����ۿ� OnItemCreated �޼��带 ȣ���Ͽ� ���Ӱ� �����Ǿ��� ���� ������ �Է��մϴ�.<br/>
    /// </summary>
    private void UpdateAllItemInfo()
    {   
        Dictionary<string, List<GameObject>> itemDic;                       // ������ ������ ������ �����մϴ�.

        for(int i=0; i<(int)ItemType.None; i++)                             // ������ ������ ���ڸ�ŭ (�κ��丮 ������ ������ŭ) �ݺ��մϴ�.
        {
            itemDic = inventory.GetItemDicIgnoreExsists((ItemType)i);       // ������ ������ ���� �κ��丮�� ������ �Ҵ�޽��ϴ�.
              

            if(itemDic.Count==0)   // ������ ������ ���� �ԷµǾ����� �ʴٸ� ���� ������ �����մϴ�.
                continue;

            foreach( List<GameObject> objList in itemDic.Values )           // �κ��丮 �������� ���ӿ�����Ʈ ����Ʈ�� �ϳ��� ������
            {
                foreach(GameObject itemObj in objList)                      // ����Ʈ�� ���ӿ�����Ʈ�� ��� �����ɴϴ�.
                    itemObj.GetComponent<ItemInfo>().OnItemCreated(this);   // OnItemChnaged�޼��带 ȣ���ϸ� ���� �κ��丮 �������� �����մϴ�.
            }

        }
    }






    /// <summary>
    /// �κ��丮�� �����ϴ� �ش� �̸��� *����* ��ȭ ������ ������ ������Ű�ų� ���ҽ�ŵ�ϴ�.<br/>
    /// ���ڷ� �ش� ������ �̸��� ������ �����ؾ� �մϴ�.<br/><br/>
    /// ������ ������ ���ҽ�Ű���� ���� ���ڷ� ������ �����Ͽ��� �ϸ�,<br/>
    /// ���� ������ ���ҷ� ���� 0�̵Ǹ� �������� �κ��丮 ��Ͽ��� ���ŵǸ�, �ı��˴ϴ�.<br/><br/>
    /// ������ ������ ������Ű���� ���� ���ڷ� ����� �����Ͽ��� �ϸ�,<br/>
    /// ������ �ִ� ���� �������� ���� �� �̻� ������ ������Ű�� ���ϴ� ���� ������ �ʰ� ������ ��ȯ�մϴ�.<br/><br/>
    /// �ֽ� ��, ������ ������ ���ҿ��θ� ������ ���ֽ��ϴ�. (�⺻��: �ֽż�)<br/><br/>
    /// ** ������ �̸��� �ش� �κ��丮�� �������� �ʰų�, ��ȭ�������� �ƴ� ��� ���ܸ� �߻���ŵ�ϴ�. **
    /// </summary>
    /// <returns></returns>
    public int SetItemOverlapCount( string itemName, int inCount, bool isLatestModify = true )
    {
        return inventory.SetOverlapCount(itemName, inCount, isLatestModify, null);
    }


    /// <summary>
    /// �κ��丮�� �ش� �̸��� �������� ��ø������ ��������� Ȯ���ϴ� �޼����Դϴ�.<br/>
    /// ���ڷ� ������ �̸��� ������ �ʿ��մϴ�.<br/><br/>
    /// ���� ° ���ڷ� ���� ���Ҹ�带 �����ϸ� �������� ��ø������ ����ϴٸ� ���ڷ� ���� ������ŭ ���ҽ�Ű��, <br/>
    /// 0�� ������ ��� �������� �κ��丮���� �����ϰ� �ı� ��ŵ�ϴ�.<br/>
    /// �ֽ� ��, ������ ������ ���ҿ��θ� ������ ���ֽ��ϴ�. (�⺻��: �ֽż�)<br/><br/>
    /// ** �ش� �̸��� �������� �������� �ʰų�, ��ȭ �������� �ƴϰų�, ������ �߸� �����ߴٸ� ���ܸ� �߻���ŵ�ϴ�. **
    /// </summary>
    /// <returns>������ ��ø������ ����ϸ� true��, ������� ������ false�� ��ȯ</returns>
    public bool IsItemEnoughMisc( string itemName, int overlapCount, bool isReduce = false, bool isLatestModify = true )
    {
        return inventory.IsEnoughOverlapCount(itemName, overlapCount, isReduce, isLatestModify);
    }

    /// <summary>
    /// �������� �κ��丮�� �����ϴ��� ���θ� ��ȯ�մϴ�.<br/>
    /// �ֽ� ��, ������ ������ �������θ� ������ ���ֽ��ϴ�. (�⺻��: ��������, �ֽż�)<br/><br/>
    /// *** �������� ������ ������ ������� ������Ʈ ������ �����ϴ��� ���θ� ��ȯ�մϴ�. ***<br/>
    /// </summary>
    /// <returns>�������� �������� �ʴ� ��� false��, �������� �����ϰų� ������ ������ ��� true�� ��ȯ</returns>
    public bool IsItemExist( string itemName, bool isRemove = false, bool isLatestModify = true )
    {
        return inventory.IsExist(itemName, isRemove, isLatestModify);
    }


    /// <summary>
    /// �������� ������ ������� �������� �κ��丮�� �����ϴ���, ��ȭ�������̶�� ���������� ������� ���θ� ��ȯ�մϴ�.<br/>
    /// ���� �������� �����ϰų�, ��ȭ�������� ��� �������� ����ϴٸ� ���� �Ǵ� ���Ҹ� ������ �� �ֽ��ϴ�.<br/>
    /// (�⺻��: ���Ҹ�� ����, �ֽż� ����)<br/><br/>
    /// *** ����ȭ �������� ��� ���� ���� �����մϴ�. ��ȭ�������� ��� ������ 1�̻��� �ƴϸ� ���ܸ� �߻���ŵ�ϴ�. ***
    /// </summary>
    /// <returns>���� �� ������ ��� ������ �����ϴ� ��� true��, ������ �������� �ʴ� ��� false�� ��ȯ, ������ ������ ��� ���Ҹ� ����</returns>
    public bool IsItemsEnough( ItemPair[] pairs, bool isReduce = false, bool isLatestModify = true )
    {
        return inventory.IsEnough(pairs, isReduce, isLatestModify);
    }



    







    /// <summary>
    /// ���� Ȱ��ȭ ���� ���� ������ �ִ� ���� ������ �ε����� ��ȯ�մϴ�.<br/>
    /// ������ ������, ���ڸ� ���� �ʽ��ϴ�.<br/><br/>
    /// *** Ȱ��ȭ ���� ���� �ε��� �ۿ� ���� �� ���� ������ ���� ���԰��� �̵� �ÿ� ȣ���ϴ� �뵵�� ����մϴ�. ***<br/>
    /// </summary>
    /// <returns>Ȱ��ȭ ���� �ǿ��� ����ִ� ������ ���� ���� �ε����Դϴ�. ���� ������ ���ٸ� -1�� ��ȯ�մϴ�.</returns>
    public int FindNearstRemainActiveSlotIdx()
    {
        int findIdx = -1;

        for( int i = 0; i<slotListTr.childCount; i++ )  // ������ �ε��� 0������ ���ϴ�
        {
            if( slotListTr.GetChild(i).childCount!=0 )  // �ش� ���Ը���Ʈ�� �ڽ��� �ִٸ� ���� ���Ը���Ʈ�� �Ѿ�ϴ�.
                continue;

            findIdx = i;                                // ã�� �ε����� �����մϴ�.
            break;
        }

        // findIdx�� �������� �ʾҴٸ� -1�� ��ȯ�մϴ�. �����Ǿ��ٸ� 0�̻��� �ε������� ��ȯ�մϴ�.
        return findIdx;
    }






    /// <summary>
    /// �� �κ��丮�� ���Կ� �������� �� �ڸ��� �ִ��� ���θ� ��ȯ�ϴ� �޼����Դϴ�.<br/>
    /// ��ȭ �������� ��ø�� �����ϰ� ������ ������Ʈ�� �� ���� ���θ� ��ȯ�մϴ�.<br/><br/>
    /// ���ڷ� ������ ������ �����Ͽ��� �ϸ�, �߰� ���ڷ� ��� �������� �ְ� �������� ������ �� �ֽ��ϴ�. (�⺻��: 1��)<br/><br/>
    /// </summary>
    /// <returns>������ �ڸ��� ���´ٸ� true��, ���Կ� �ڸ��� ���ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool IsSlotEnoughIgnoreOverlap(ItemType itemType, int objectCount=1)
    {        
        if(inventory.GetCurRemainSlotCount(itemType) >= objectCount)
            return true;
        else
            return false;       
    }

    /// <summary>
    /// �� �κ��丮�� ���Կ� ��ȭ �������� ���ϴ� ������ŭ �������� ��,
    /// �� �ڸ��� �ִ��� ���θ� ��ȯ�ϴ� �޼����Դϴ�.<br/>
    /// ���ڷ� ������ �̸��� �������ڸ� �����ؾ� �մϴ�. (���������� �⺻���� 1�Դϴ�.)<br/><br/>
    /// 
    /// ����ȭ�������� ��� �������ڸ�ŭ ������Ʈ�� �����ϴ�. (��, �������ڰ� ������Ʈ�� ������ ���մϴ�.)<br/>
    /// ��ȭ �������� ��� �������ڸ� �־ �ִ� ��ø������ �����ϱ� �������� ������Ʈ�� 1���� �����մϴ�. (��, �������ڴ� ��ø������ ���մϴ�.)<br/><br/>
    /// </summary>
    /// <returns>������ �ڸ��� ���´ٸ� true��, ���Կ� �ڸ��� ���ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool IsSlotEnough(string itemName, int overlapCount=1)
    {
        ItemType itemType = createManager.GetWorldItemType(itemName);

        if( itemType==ItemType.Misc )
            return inventory.IsAbleToAddMisc( itemName, overlapCount ); 
        else
            return inventory.GetCurRemainSlotCount(itemType) >= overlapCount; // ���� ���� ĭ ���� overlapCount�̻�
    }

    /// <summary>
    /// �ش� �������� �� �� ���� �� ���θ� ��ȯ�մϴ�.<br/>
    /// ���� �������� �����ؾ� �մϴ�.
    /// </summary>
    /// <returns>������ �ڸ��� ���´ٸ� true��, ���Կ� �ڸ��� ���ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool IsSlotEnough(ItemInfo itemInfo)
    {
        ItemType itemType = itemInfo.Item.Type;

        if( itemType == ItemType.Misc )
        {
            ItemMisc itemMisc = (ItemMisc)itemInfo.Item;
            return inventory.IsAbleToAddMisc(itemMisc.Name, itemMisc.OverlapCount);
        }
        else
            return inventory.GetCurRemainSlotCount(itemType) >= 1;  // ���� ���� ĭ���� 1�� �̻�
    }





    /// <summary>
    /// ������ Info ������Ʈ�� ���ڷ� �޾Ƽ� � ���Կ� ���� �ֽ�ȭ ���ִ� �޼����Դϴ�. <br/>
    /// ���������� FindNearstSlotIdx�޼��带 ȣ���Ͽ� ���� ������ �Է����ݴϴ�.<br/>
    /// *** ���Կ� ���ڸ��� ���ų�, ������ ������ ���ٸ� ���ܸ� �߻���ŵ�ϴ�. ***
    /// </summary>
    public void SetItemSlotIdxBothToNearstSlot( ItemInfo itemInfo )
    {        
        // ���� ���� ���� ó��
        if( itemInfo==null )
            throw new Exception("ItemInfo ��ũ��Ʈ�� �������� �ʴ� �������Դϴ�.");
        // ���Կ� ������� ���� ��
        else if( !IsSlotEnough(itemInfo) )     
            throw new Exception("�������� �� �ڸ��� ������� �ʽ��ϴ�. Ȯ���Ͽ� �ּ���.");    
                
        
        // ������ ������ ������ �����մϴ�.
        Item item = itemInfo.Item;
        ItemType itemType = item.Type;
  
        // ������ ���ڷ� �־ ���� ���� �ε����� ��ȯ �޽��ϴ�.
        int slotIdxEach = inventory.FindNearstSlotIdx(itemType);  
        print("Each " + slotIdxEach);

        // ��ü ���� �ε����� ���մϴ�.
        int slotIdxAll = inventory.FindNearstSlotIdx(ItemType.None);
        print("All : " + slotIdxAll);
                

        // ���� ���� �ε����� ������ ������ �Է��մϴ�.
        item.SlotIndex = slotIdxEach;
        item.SlotIndexAll = slotIdxAll;

    }



    /// <summary>
    /// Ư�� ������ ��ųʸ��� �����ϴ� �������� ���� ������ ������Ʈ���ִ� �޼����Դϴ�.<br/>
    /// �������� ������ ���� ��ųʸ��� �����Ͽ� 
    /// �ش� ��ųʸ� ������ ��� �������� ������� UpdatePositionInSlotList�޼��带 ȣ���մϴ�.<br/><br/>
    /// ����� interactiveŬ�������� �� �޼��带 Ȱ���ϰ� �ֽ��ϴ�.<br/>
    /// </summary>
    /// <param name="itemType"></param>
    public void UpdateDicItemPosition(ItemType itemType)
    {            
        //�κ��丮�� ���� Ȱ��ȭ �������� ��ġ�ϴ� ��ųʸ��� �����մϴ�.
        Dictionary<string, List<GameObject>> itemDic = inventory.GetItemDicIgnoreExsists(itemType);

        foreach( List<GameObject> itemObjList in itemDic.Values )  // �ش� ��ųʸ��� ������Ʈ����Ʈ�� �����ɴϴ�.
        {
            foreach( GameObject itemObj in itemObjList )           // ������Ʈ����Ʈ���� ������Ʈ�� �ϳ��� �����ɴϴ�.
            {
                ItemInfo itemInfo = itemObj.GetComponent<ItemInfo>();  // ������ ������ �о���Դϴ�.
                itemInfo.UpdatePositionInSlotList();                   // Ȱ��ȭ �� ������� �ش� ������ ��ġ������ ������Ʈ�մϴ�.
            }
        }
    }



}
