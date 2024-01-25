using UnityEngine;
using UnityEngine.SceneManagement;
using InventoryManagement;
using CreateManagement;


/* [�۾� ����]
 * <v1.0 - 2023_1221_�ֿ���>
 * 1- �׽�Ʈ�� ��ư �޼��尡 ������ִ� Ŭ������ ���Ӱ� �����Ͽ����ϴ�.
 * 
 * <v1.1 - 2023_1222_�ֿ���>
 * 1- playerInven�� �������� inventory�� ����
 * 2- GetComponent<Inventory>()�̴� ���� GetComponent<PlayerInven>().inventory�� ����
 * 
 * <v1.2 - 2024_0105_�ֿ���>
 * 1- �����Ͽ��� ������ ���� Inventory�� InventoryInfo�� ����
 * 2- inventoryInfo�� ������ GetComponent���� GetComponentInChildren���� ����
 */


/// <summary>
/// �ܼ� ��ư�� ���� �κ��丮 �۵��� �׽�Ʈ�ϴ� ��ũ��Ʈ�Դϴ�
/// </summary>
public class ButtonPlayTest : MonoBehaviour
{
    InventoryInfo playerInventoryInfo;
    CreateManager createManager;

    private void Start()
    {
        playerInventoryInfo = GameObject.FindWithTag("Player").GetComponentInChildren<InventoryInfo>();
        createManager = GameObject.FindWithTag("GameController").GetComponent<CreateManager>();
    }


    public void btnCreateItem()
    {
        //playerInventoryInfo.AddItem( "Mud", 1 );
        //playerInventoryInfo.AddItem( "Thread", 1 );
        //playerInventoryInfo.AddItem( "Vine", 1 );
        //playerInventoryInfo.AddItem( "StoneAxe", 1 );
        playerInventoryInfo.AddItem( "Food1", 3 );
        playerInventoryInfo.AddItem( "��", 5 );

        if(playerInventoryInfo.IsItemEnough("Mud",2) )
            Debug.Log($"Mud 2�� �ʰ�");
        
        //if(inventoryInfo.IsItemEnough("Thread",3) )
        //    Debug.Log($"Thread 3�� �ʰ�");

        //if(inventoryInfo.IsItemEnough("Vine",5) )
        //    Debug.Log($"Vine 5�� �ʰ�");

    }

    public void btnNext()
    {
        if(SceneManager.GetActiveScene().name=="InventoryTest")
            SceneManager.LoadScene("InventoryTest2");
        else
            SceneManager.LoadScene("InventoryTest");
    }

    public void btnExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void CraftStart()
    {

        createManager.CreateWorldItem("��").RegisterToWorld(playerInventoryInfo.OwnerTr).OnItemWorldDrop(playerInventoryInfo.baseDropTr);
        
        createManager.CreateWorldItem("��Ÿ��").RegisterToWorld(playerInventoryInfo.OwnerTr).OnItemWorldDrop(playerInventoryInfo.baseDropTr);
        
        createManager.CreateWorldItem("�κ��丮").RegisterToWorld(playerInventoryInfo.OwnerTr).OnItemWorldDrop(playerInventoryInfo.baseDropTr);

        
    }


}
