using UnityEngine;
using UnityEngine.SceneManagement;
using InventoryManagement;


/* [작업 사항]
 * <v1.0 - 2023_1221_최원준>
 * 1- 테스트용 버튼 메서드가 모아져있는 클래스를 새롭게 생성하였습니다.
 * 
 * <v1.1 - 2023_1222_최원준>
 * 1- playerInven의 변수명을 inventory로 수정
 * 2- GetComponent<Inventory>()이던 점을 GetComponent<PlayerInven>().inventory로 수정
 * 
 * <v1.2 - 2024_0105_최원준>
 * 1- 컴파일에러 수정을 위해 Inventory를 InventoryInfo로 수정
 * 2- inventoryInfo의 참조를 GetComponent에서 GetComponentInChildren으로 수정
 */


/// <summary>
/// 단순 버튼을 통한 인벤토리 작동을 테스트하는 스크립트입니다
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
        inventoryInfo.AddItem("가죽 후드");
        inventoryInfo.AddItem("돌 도끼");
    }

    public void BtnNext()
    {
        if(SceneManager.GetActiveScene().name=="InventoryTest")
            SceneManager.LoadScene("InventoryTest2");
        else
            SceneManager.LoadScene("InventoryTest");
    }

    public void BtnExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void BtnCraftStart()
    {                
        inventoryInfo.AddItem("물", 3);
        inventoryInfo.AddItem("진흙", 1);
    }


}
