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
 * 2- 
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
        
    private GameObject slotPrefab;              // ������ �������� �����ϱ� ���� ������ ����
    private InventoryInteractive interactive;   // �ڽ��� ���ͷ�Ƽ�� ��ũ��Ʈ�� �����Ͽ� Ȱ��ȭ �������� �޾ƿ��� ���� ���� ����

    void Awake()
    {        
        slotListTr = transform.GetChild(0).GetChild(0).GetChild(0);
        slotPrefab = slotListTr.GetChild(0).gameObject;
        
        // �÷��̾� �κ��丮 ����(��ü �� ���� ĭ��)�� �����Ͽ� ������ �������� ���� (���� �κ��丮�� ������ �� �� ��������Ƿ� �ϳ��� ���ϰ� ����)
        for( int i = 0; i<inventory.SlotCountLimitAll-1; i++ )
            Instantiate( slotPrefab, slotListTr );

        interactive = gameObject.GetComponent<InventoryInteractive>();  // �ڽ��� ���ͷ�Ƽ�� ��ũ��Ʈ�� �����մϴ�.
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
        DataManager dataManager = new DataManager("Inventory",0); //���̺� ���ϸ�, ���Թ�ȣ
        InventorySaveData loadData = dataManager.LoadData<InventorySaveData>();
                
        inventory=loadData.savedInventory.Deserialize();
        inventory.UpdateAllItemInfo();
    }


    /// <summary>
    /// �κ��丮 ���� �÷��̾� �����͸� �����մϴ� 
    /// </summary>
    void SavePlayerData()
    {
        DataManager dataManager = new DataManager("Inventory",0); //���̺� ���ϸ�, ���Թ�ȣ
        
        // �޼��� ȣ�� ������ �ٸ� ��ũ��Ʈ���� save���� ���� �����Ƿ� ���Ӱ� �������� �ʰ� ���� ������ �ֽ�ȭ�մϴ�
        InventorySaveData saveData = dataManager.LoadData<InventorySaveData>();

        saveData.savedInventory.Serialize(inventory);

        dataManager.SaveData<InventorySaveData>(saveData);
    }




    /// <summary>
    /// �κ��丮�� �����ϴ� �ش� �̸��� ������ ������ ������Ű�ų� ���ҽ�ŵ�ϴ�.<br/>
    /// ���ڷ� �ش� ������ �̸��� ������ �����ؾ� �մϴ�.<br/><br/>
    /// ������ ������ ���ҽ�Ű���� ���� ���ڷ� ������ �����Ͽ��� �ϸ�,<br/>
    /// ���� ������ ���ҷ� ���� 0�̵Ǹ� �������� �κ��丮 ��Ͽ��� ���ŵǸ�, �ı��˴ϴ�.<br/><br/>
    /// ������ ������ ������Ű���� ���� ���ڷ� ����� �����Ͽ��� �ϸ�,<br/>
    /// ������ �ִ� ���� �������� ���� �� �̻� ������ ������Ű�� ���ϴ� ���� ������ �ʰ� ������ ��ȯ�մϴ�.<br/><br/>
    /// �ֽ� ��, ������ ������ ���ҿ��θ� ������ ���ֽ��ϴ�. (�⺻��: �ֽż�)<br/><br/>
    /// ** ������ �̸��� �ش� �κ��丮�� �������� �ʰų�, ��ȭ�������� �ƴ� ��� ���ܸ� �߻���ŵ�ϴ�. **
    /// </summary>
    /// <returns></returns>
    public int SetItemOverlapCount( string itemName, int inCount, bool isLatestReduce = true )
    {
        return inventory.SetOverlapCount(itemName, inCount, isLatestReduce);
    }


    /// <summary>
    /// �κ��丮�� �ش� �̸��� �������� ��ø������ ��������� Ȯ���ϴ� �޼����Դϴ�.<br/>
    /// ���ڷ� ������ �̸��� ������ �ʿ��մϴ�. (���� ������ �� �⺻ ������ 1���Դϴ�.)<br/><br/>
    /// ���� ° ���ڷ� ���� ���Ҹ�带 �����ϸ� �������� ��ø������ ����ϴٸ� ���ڷ� ���� ������ŭ ���ҽ�Ű��, <br/>
    /// 0�� ������ ��� �������� �κ��丮���� �����ϰ� �ı� ��ŵ�ϴ�.<br/>
    /// �ֽ� ��, ������ ������ ���ҿ��θ� ������ ���ֽ��ϴ�. (�⺻��: �ֽż�)<br/><br/>
    /// ** �ش� �̸��� �������� �������� �ʰų�, ��ȭ �������� �ƴϰų�, ������ �߸� �����ߴٸ� ���ܸ� �߻���ŵ�ϴ�. **
    /// </summary>
    /// <returns>������ ��ø������ ����ϸ� true��, ������� ������ false�� ��ȯ</returns>
    public bool IsItemEnoughOverlapCount( string itemName, int overlapCount = 1, bool isReduce = false, bool isLatestReduce = true )
    {
        return inventory.IsEnoughOverlapCount(itemName, overlapCount, isReduce, isLatestReduce);
    }

    /// <summary>
    /// �������� �κ��丮�� �����ϴ��� ���θ� ��ȯ�մϴ�.<br/>
    /// �ֽ� ��, ������ ������ �������θ� ������ ���ֽ��ϴ�. (�⺻��: ��������, �ֽż�)<br/><br/>
    /// *** �������� ������ ������ ������� ������Ʈ ������ �����ϴ��� ���θ� ��ȯ�մϴ�. ***<br/>
    /// </summary>
    /// <returns>�������� �������� �ʴ� ��� false��, �������� �����ϰų� ������ ������ ��� true�� ��ȯ</returns>
    public bool IsItemExist( string itemName, bool isRemove = false, bool isLatestRemove = true )
    {
        return inventory.IsExist(itemName, isRemove, isLatestRemove);
    }


    /// <summary>
    /// �������� ������ ������� �������� �κ��丮�� �����ϴ���, ��ȭ�������̶�� ���������� ������� ���θ� ��ȯ�մϴ�.<br/>
    /// ���� �������� �����ϰų�, ��ȭ�������� ��� �������� ����ϴٸ� ���� �Ǵ� ���Ҹ� ������ �� �ֽ��ϴ�.<br/>
    /// (�⺻��: ���Ҹ�� ����, �ֽż� ����)<br/><br/>
    /// *** ����ȭ �������� ��� ���� ���� �����մϴ�. ��ȭ�������� ��� ������ 1�̻��� �ƴϸ� ���ܸ� �߻���ŵ�ϴ�. ***
    /// </summary>
    /// <returns>���� �� ������ ��� ������ �����ϴ� ��� true��, ������ �������� �ʴ� ��� false�� ��ȯ, ������ ������ ��� ���Ҹ� ����</returns>
    public bool IsItemEnough( ItemPair pair, bool isReduce = false, bool isLatestReduce = true )
    {
        return inventory.IsEnough(pair, isReduce, isLatestReduce);
    }

    /// <summary>
    /// �������� ������ ������� �������� �κ��丮�� �����ϴ���, ��ȭ�������̶�� ���������� ������� ���θ� ��ȯ�մϴ�.<br/>
    /// ���� �������� �����ϰų�, ��ȭ�������� ��� �������� ����ϴٸ� ���� �Ǵ� ���Ҹ� ������ �� �ֽ��ϴ�.<br/>
    /// (�⺻��: ���Ҹ�� ����, �ֽż� ����)<br/><br/>
    /// *** ����ȭ �������� ��� ���� ���� �����մϴ�. ��ȭ�������� ��� ������ 1�̻��� �ƴϸ� ���ܸ� �߻���ŵ�ϴ�. ***
    /// </summary>
    /// <returns>���� �� ������ ��� ������ �����ϴ� ��� true��, ������ �������� �ʴ� ��� false�� ��ȯ, ������ ������ ��� ���Ҹ� ����</returns>
    public bool IsItemsEnough( ItemPair[] pairs, bool isReduce = false, bool isLatestReduce = true )
    {
        return inventory.IsEnough(pairs, isReduce, isLatestReduce);
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
    /// ���� ����� ������ �ε����� ���մϴ�. � ������ �������� ���� ������ ���ڷ� �����Ͽ��� �մϴ�.<br/>
    /// ItemType���ڿ� �ش��ϴ� ������ ���� �ε����� ��ȯ�մϴ�. ItemType.None�� ������ ��� ��ü���� ���� �ε����� ��ȯ�մϴ�. (�⺻��: ��ü��)
    /// </summary>
    /// <returns>ItemType.None�� ������ ��� ��ü�� ���� �ε����� ��ȯ�ϸ�, �̿��� ���ڴ� ������ ���� �ε����� ��ȯ�մϴ�. 
    /// ���Կ� �ڸ��� ���ٸ� -1�� ��ȯ�մϴ�.</returns>
    public int FindNearstSlotIdx( ItemType itemType = ItemType.None )
    {
        return inventory.FindNearstSlotIdx(itemType);
    }
    
    /// <summary>
    /// ���� ����� ������ �ε����� ���մϴ�. � �̸��� �������� ���� �������� <br/>
    /// ������ Ȥ�� ��ü���� �ε����� ��ȯ�ް��� �ϴ��� ���θ� �����Ͽ��� �մϴ�. (�⺻��: ��ü��)<br/>
    /// </summary>
    /// <returns>true�� ������ ��� ��ü�� ���� �ε����� ��ȯ�ϸ�, false�� ������ ���� �ε����� ��ȯ�մϴ�. 
    /// ���Կ� �ڸ��� ���ٸ� -1�� ��ȯ�մϴ�.</returns>
    public int FindNearstSlotIdx( string itemName, bool isIndexAll=true)
    {
        return inventory.FindNearstSlotIdx(itemName, isIndexAll);
    }






    /// <summary>
    /// �� �κ��丮�� ���Կ� �������� �� �ڸ��� �ִ��� ���θ� ��ȯ�ϴ� �޼����Դϴ�.<br/>
    /// ���ڷ� ������ ������ �����Ͽ��� �մϴ�.
    /// </summary>
    /// <returns>������ �ڸ��� ���´ٸ� true��, ���Կ� �ڸ��� ���ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool IsSlotEnough(ItemType itemType)
    {
        if(FindNearstSlotIdx(itemType) == -1)
            return false;
        else
            return true;        
    }

    /// <summary>
    /// �� �κ��丮�� ���Կ� �������� �� �ڸ��� �ִ��� ���θ� ��ȯ�ϴ� �޼����Դϴ�.<br/>
    /// ���ڷ� ������ �̸��� �����Ͽ��� �մϴ�.
    /// </summary>
    /// <returns>������ �ڸ��� ���´ٸ� true��, ���Կ� �ڸ��� ���ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool IsSlotEnough(string itemName)
    {
        if(FindNearstSlotIdx(itemName, false)==-1)
            return false;
        else
            return true;
    }

    /// <summary>
    /// �� �κ��丮�� ���Կ� �������� �� �ڸ��� �ִ��� ���θ� ��ȯ�ϴ� �޼����Դϴ�.<br/>
    /// ���ڷ� ���������� ��ũ��Ʈ�� �����Ͽ��� �մϴ�.
    /// </summary>
    /// <returns>������ �ڸ��� ���´ٸ� true��, ���Կ� �ڸ��� ���ٸ� false�� ��ȯ�մϴ�.</returns>
    public bool IsSlotEnough(ItemInfo itemInfo)
    {
        if(FindNearstSlotIdx(itemInfo.Item.Type) == -1)
            return false;
        else
            return true;  
    }







    /// <summary>
    /// ������ Info ������Ʈ�� ���ڷ� �޾Ƽ� � ���Կ� ���� �ֽ�ȭ ���ִ� �޼����Դϴ�. <br/>
    /// ���������� FindNearstSlotIdx�޼��带 ȣ���Ͽ� ���� ������ �Է����ݴϴ�.<br/>
    /// </summary>
    /// <returns>���Կ� ���ڸ��� ���ٸ� false�� ��ȯ�մϴ�. �������� �� ������ ���ٴ� ���Դϴ�.</returns>
    public bool SetItemSlotIdxBothToNearstSlot( ItemInfo itemInfo )
    {        
        // ȣ�� ���� �׸� ó��
        if( itemInfo==null )
            throw new Exception("ItemInfo ��ũ��Ʈ�� �������� �ʴ� �������Դϴ�.");

        // ������ ������ ������ �����մϴ�.
        Item item = itemInfo.Item;
        ItemType itemType = item.Type;

        // ������ ���ڷ� �־ ���� ���� �ε����� ��ȯ �޽��ϴ�.
        int slotIdxEach = FindNearstSlotIdx(itemType);
        
        // ���� ���� �ε����� -1�̶�� ���ڸ��� ���� ������ �Ǵ��Ͽ� ���и� ��ȯ�մϴ�.
        if(slotIdxEach==-1)     
            return false;        
        
        // ��ü ���� �ε����� ���մϴ�.
        int slotIdxAll = FindNearstSlotIdx(ItemType.None);;

        // ���� ���� �ε����� ������ ������ �Է��մϴ�.
        item.SlotIndex = slotIdxEach;
        item.SlotIndexAll = slotIdxAll;

        // ������ ��ȯ�մϴ�.
        return true;
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
