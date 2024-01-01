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
 */


/// <summary>
/// �ܼ� ��ư�� ���� �κ��丮 �۵��� �׽�Ʈ�ϴ� ��ũ��Ʈ�Դϴ�
/// </summary>
public class ButtonPlayTest : MonoBehaviour
{
    Inventory inventory;
    private void Start()
    {
        inventory=GameObject.FindWithTag( "Player" ).GetComponent<InventoryInfo>().inventory;
    }


    public void btnCreateItem()
    {
        inventory.CreateItem( "ö", 1 );
        inventory.CreateItem( "��ö", 3 );
        inventory.CreateItem( "�̽���", 10 );
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


}
