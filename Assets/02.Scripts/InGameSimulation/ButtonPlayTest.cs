using UnityEngine;
using UnityEngine.SceneManagement;
using CraftData;


/* [�۾� ����]
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- �׽�Ʈ�� ��ư �޼��尡 ������ִ� Ŭ������ ���Ӱ� �����Ͽ����ϴ�.
 * 
 */


/// <summary>
/// �ܼ� ��ư�� ���� �κ��丮 �۵��� �׽�Ʈ�ϴ� ��ũ��Ʈ�Դϴ�
/// </summary>
public class ButtonPlayTest : MonoBehaviour
{
    //Inventory playerInven;
    private void Start()
    {
        //playerInven = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        DataManagement.DataManager dataManager = new DataManagement.DataManager();
        Debug.Log(dataManager.Path);
    }


    public void btnCreateItem()
    {
        //playerInven.AddItem("ö", 10);
        //playerInven.AddItem("��ö", 10);
        //playerInven.AddItem("�̽���", 10);
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
            PlayerPrefs.SetInt("IsContinuedGamePlay", 0);   //���� ���̺� �ε尡 �Ҿ����ϹǷ� �Ͻ������� �ʱ�ȭ
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


}
