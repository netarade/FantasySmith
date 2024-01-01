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
 */


/// <summary>
/// 단순 버튼을 통한 인벤토리 작동을 테스트하는 스크립트입니다
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
        inventory.CreateItem( "철", 1 );
        inventory.CreateItem( "강철", 3 );
        inventory.CreateItem( "미스릴", 10 );
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
