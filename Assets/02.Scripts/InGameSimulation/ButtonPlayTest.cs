using UnityEngine;
using UnityEngine.SceneManagement;
using CraftData;


/* [작업 사항]
 * <v1.0 - 2023_1105_최원준>
 * 1- 테스트용 버튼 메서드가 모아져있는 클래스를 새롭게 생성하였습니다.
 * 
 */


/// <summary>
/// 단순 버튼을 통한 인벤토리 작동을 테스트하는 스크립트입니다
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
        //playerInven.AddItem("철", 10);
        //playerInven.AddItem("강철", 10);
        //playerInven.AddItem("미스릴", 10);
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
            PlayerPrefs.SetInt("IsContinuedGamePlay", 0);   //현재 세이브 로드가 불안정하므로 일시적으로 초기화
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


}
