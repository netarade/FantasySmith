using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using CraftData;
using DataManagement;


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

    public delegate void LoadEvent();
    // �̺�Ʈ ����
    public static event LoadEvent CraftManagerLoaded;

    void Awake()
    {
        if( instance==null )
            instance=this;
        else
            Destroy( this.gameObject );

        DontDestroyOnLoad( instance );

    }

    /// <summary>
    /// Start�޼ҵ带 OnEnable�̻����� ��ġ�� ����ȭ �ȵǴ� ������ �߻��մϴ�.
    /// </summary>
    void Start()
    {
        DataManager dataManager = new DataManager();
        GameData loadData = dataManager.LoadData();

        proficiencyDic=loadData.proficiencyDic;   // �÷��̾� ���� ��� �ҷ�����
        craftableList=loadData.craftableWeaponList; // �÷��̾� ���� ��� �ҷ�����

        inventory=loadData.inventory;           // �÷��̾� �κ��丮 �ҷ�����
        miscList=inventory.miscList;
        weapList=inventory.weapList;          // ������ ������ ���� ���� �߰� ���� �Ͽ���.

        //CraftManagerLoaded();   // ��� �۾��� �������� �˷��� ����� �޼ҵ带 ȣ���Ͽ��ش�.
    }

    private void OnApplicationQuit()
    {
        DataManager dataManager = new DataManager();
        GameData saveData = dataManager.LoadData();

        saveData.proficiencyDic=proficiencyDic;     // �÷��̾� ���� ��� ����
        saveData.craftableWeaponList=craftableList; // �÷��̾� ���� ��� ����
        saveData.inventory=inventory;               // �÷��̾� �κ��丮 ����

        dataManager.SaveData( saveData );
    }

}
