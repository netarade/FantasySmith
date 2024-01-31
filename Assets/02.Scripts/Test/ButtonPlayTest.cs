using UnityEngine;
using UnityEngine.SceneManagement;
using InventoryManagement;
using CreateManagement;
using ItemData;


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
    UserInfo userInfo;

    private void Start()
    {
        playerInventoryInfo = GameObject.FindWithTag("Player").GetComponentInChildren<InventoryInfo>();
        createManager = GameObject.FindWithTag("GameController").GetComponent<CreateManager>();
        userInfo = GameObject.FindWithTag("Player").GetComponent<UserInfo>();
    }


    public void btnCreateItem()
    {
        playerInventoryInfo.AddItem( "Water", 5 );
        playerInventoryInfo.AddItem( "StoneAxe" );

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
         ItemPair[] itemPairs = { 
            new ItemPair( "Mud", 2 ), 
            new ItemPair( "Thread", 3),
            new ItemPair( "Vine", 5),
            new ItemPair( "StoneAxe", 1),
            };


        if(playerInventoryInfo.IsItemEnough(itemPairs))
        {
            playerInventoryInfo.ReduceItem(itemPairs);
        }
 
        
    }


}
