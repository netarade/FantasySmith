using UnityEngine;
using UnityEngine.SceneManagement;
using InventoryManagement;
using CreateManagement;


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
        playerInventoryInfo.AddItem( "흙", 5 );

        if(playerInventoryInfo.IsItemEnough("Mud",2) )
            Debug.Log($"Mud 2개 초과");
        
        //if(inventoryInfo.IsItemEnough("Thread",3) )
        //    Debug.Log($"Thread 3개 초과");

        //if(inventoryInfo.IsItemEnough("Vine",5) )
        //    Debug.Log($"Vine 5개 초과");

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

        createManager.CreateWorldItem("벽").RegisterToWorld(playerInventoryInfo.OwnerTr).OnItemWorldDrop(playerInventoryInfo.baseDropTr);
        
        createManager.CreateWorldItem("울타리").RegisterToWorld(playerInventoryInfo.OwnerTr).OnItemWorldDrop(playerInventoryInfo.baseDropTr);
        
        createManager.CreateWorldItem("인벤토리").RegisterToWorld(playerInventoryInfo.OwnerTr).OnItemWorldDrop(playerInventoryInfo.baseDropTr);

        
    }


}
