using UnityEngine;
using UnityEngine.SceneManagement;
using InventoryManagement;


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
    InventoryInfo inventoryInfo;
    private void Start()
    {
        inventoryInfo=GameObject.FindWithTag("Player").GetComponentInChildren<InventoryInfo>();
    }


    public void btnCreateItem()
    {
        inventoryInfo.AddItem( "ö", 1 );
        inventoryInfo.AddItem( "��ö", 3 );
        inventoryInfo.AddItem( "�̽���", 10 );
        inventoryInfo.AddItem( "�յ���" );
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
        if( inventoryInfo.IsItemEnough("ö", 1) )
        {
            print("IsItemEnoughMisc ö�� 1�� �����մϴ�.");
            //inventoryInfo.RemoveItem("ö", 1); 
        }

        if(inventoryInfo.IsItemEnough("�յ���"))
        {
            print("�յ����� 1�� �����մϴ�.");
            //inventoryInfo.IsItemEnough("�յ���", 1);
        }

        
        //if(inventoryInfo.Is("�յ���", true))
        //{
        //    print("�յ����� 1�� �����մϴ�.");
        //}
    }


}
