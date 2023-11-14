using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using CraftData;
using DataManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * [작업 사항]  
 * <v1.0 - 2023_1106_최원준>
 * 1- 초기 클래스 정의
 * 제작관련 변수들을 보유하도록 설계, 싱글톤으로 접근성 보유
 * <v1.1 - 2023_1106_최원준>
 * 1- 제작목록을 List에서 클래스로 구조 변경으로 인한 수정
 * 2- 주석보완
 * 
 * <v1.2 - 2023_1106_최원준>
 * 1- Start의 로직을 OnEnable로 수정. 
 * 다른 스크립트에서 Start구문에서 instance에 접근하여 정보를 받아가고 있기 때문.
 * 2- 직렬화 안되는 문제가 발생하여 다시 OnEnable에서 Start로 수정.
 * 
 * <v2.0 - 2023_1107_최원준>
 * 1- 빠른 초기화가 이루어져야 스크립트에서 정보를 받아가므로 Awake문으로 옮김.
 * 대리자를 이용하여 연관성을 최소화하여 순서대로 초기화가 이루어지도록 함. 
 * 
 * <v2.1 - 2023_1108_최원준>
 * 1- 아이템이 씬이 넘어갈때 파괴되기 때문에 스크립트를 저장하려고 하였으나 스크립트 또한 파괴되는 문제 발생
 * => 딕셔너리를 이용해서 이름과 수량만 저장하고 새롭게 생성하는 대체 방식으로 수정. 
 * 
 * 2- 씬 로드 시 무조건 하나의 재료만 들어있는 문제 발생
 * => 아이템쪽에서 파괴될떄마다 딕셔너리를 생성했었기 때문에 CraftManager쪽에서 마지막에 한번 미리 생성해주도록 변경
 */

    

/// <summary>
/// 게임 실행 중 제작 관련 실시간 플레이어 정보 들을 보유하고 있는 클래스입니다. 싱글톤으로 접근 가능합니다.
/// </summary>
public class CraftManager : MonoBehaviour
{
    public static CraftManager instance;

    /// <summary>
    /// 플레이어가 제작가능한 장비를 알려주는 목록입니다. string name을 기반으로 하는 무기 종류별 리스트가 보관되어있는 클래스입니다.<br/> 
    /// 게임 중에 변경사항이 있다면 이 변수를 수정해야 합니다.
    /// </summary>
    public CraftableWeaponList craftableList;

    /// <summary>
    /// 플레이어가 보유하고 있는 장비 숙련도 목록입니다. name과 CraftProficincy구조체형 으로 보관하여 빠르게 접근을 가능하게 해줍니다<br/>
    /// 게임 중에 변경사항이 있다면 이 변수를 수정해야 합니다. 
    /// </summary>
    public Dictionary<string, CraftProficiency> proficiencyDic;

    /// <summary>
    /// 플레이어가 보유하고 있는 인벤토리 관련 정보의 집합 클래스입니다. <br/>
    /// 게임 중에 변경사항이 있다면 이 변수를 수정해야 합니다.
    /// </summary>
    public Inventory inventory;


    /*** 접근의 편리함을 위해 추가 선언하였음. ***/

    /// <summary>
    /// 플레이어 인벤토리 중에서 장비아이템 게임오브젝트를 보관하는 리스트입니다. <br/>
    /// 게임 중에 변경사항이 있다면 이 변수를 수정해야 합니다. (inventory와 참조연결이 되어있습니다.)
    /// </summary>
    public List<GameObject> weapList;

    /// <summary>
    /// 플레이어 인벤토리 중에서 잡화아이템 게임오브젝트를 보관하는 리스트입니다. <br/>
    /// 게임 중에 변경사항이 있다면 이 변수를 수정해야 합니다. (inventory와 참조연결이 되어있습니다.)
    /// </summary>
    public List<GameObject> miscList;

    /// <summary>
    /// 게임 중에 기록하는 골드 플레이어 관련 변수입니다.
    /// </summary>
    public float playerGold; 
    /// <summary>
    /// 게임 중에 기록하는 실버 플레이어 관련 변수입니다.
    /// </summary>
    public float playerSilver;      

    public delegate void LoadEvent();
    // 이벤트 생성
    public static event LoadEvent CraftManagerLoaded;
        



    // 씬이 넘어갈 때 ItemInfo 스크립트를 넘겨주기 위한 딕셔너리
    public Dictionary<string, int> weapSaveDic;
    public Dictionary<string, int> miscSaveDic;


    void Awake()
    {
        if( instance==null )
            instance=this;
        else
            Destroy( this.gameObject );

        DontDestroyOnLoad( instance );

        if( PlayerPrefs.GetInt("IsContinuedGamePlay") != 1 )   
            CreateManager.PlayerInitCompleted += CraftManagerInit;    // 게임의 처음시작은 이벤트 연결을 통해 호출하고,
        else
            CraftManagerInit();                                       // 이후에는 Awake문에서 바로 정보를 불러들인다.   


        SceneManager.sceneLoaded += OnSceneLoaded; // 씬이로드될때 새롭게 참조를 잡아준다.
        SceneManager.sceneUnloaded += OnSceneUnloaded; // 씬이로드될때 새롭게 참조를 잡아준다.

        
    }


    /// <summary>
    /// CreateManager의 Awake문 이후에 데이터를 로드하여 초기화를 이루는 로직입니다.
    /// </summary>
    private void CraftManagerInit()
    {
        DataManager dataManager = new DataManager();
        GameData loadData = dataManager.LoadData();

        proficiencyDic=loadData.proficiencyDic;     // 플레이어 숙련 목록 불러오기
        craftableList=loadData.craftableWeaponList; // 플레이어 제작 목록 불러오기
        
        inventory=loadData.inventory;                // 플레이어 인벤토리 불러오기
        miscList=inventory.miscList;
        weapList=inventory.weapList;                // 접근의 편리함을 위해 따로 추가 선언 하였음.

        playerGold = loadData.gold;
        playerSilver = loadData. silver;
    }



    
    /// <summary>
    /// 씬이 로드되었을 때 다시 참조해주는 로직
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if( miscSaveDic==null && weapSaveDic==null ) // 최초에 딕셔너리가 없다면 새로만든다.
        {
            weapSaveDic=new Dictionary<string, int>();
            miscSaveDic=new Dictionary<string, int>();
            return;
        }

        weapList.Clear();   // 기존의 리스트 클리어
        miscList.Clear();

        print(miscSaveDic.Count);
        print(weapSaveDic.Count);
        // 딕셔너리를 참조하여 아이템 오브젝트를 만든다.
        foreach( KeyValuePair<string, int> pair in miscSaveDic )
        {
            CreateManager.instance.CreateItemToNearstSlot( miscList, pair.Key, pair.Value );
        }

        foreach( KeyValuePair<string, int> pair in weapSaveDic )
        {
            CreateManager.instance.CreateItemToNearstSlot( weapList, pair.Key );
        }
        
        CraftManager.instance.weapSaveDic=new Dictionary<string, int>(); // 새로운 딕셔너리를 만들어 둔다
        CraftManager.instance.miscSaveDic=new Dictionary<string, int>(); // 새로운 딕셔너리를 만들어 둔다.
    }
    
    private void OnSceneUnloaded( Scene scene )
    {
        Debug.Log("언로드 되었습니다.");
        
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded-=OnSceneLoaded; // 씬이로드될때 새롭게 참조를 잡아준다.
        SceneManager.sceneUnloaded-=OnSceneUnloaded; // 씬이로드될때 새롭게 참조를 잡아준다.
    }








    /// <summary>
    /// 플레이어의 인벤토리에 담겨 있는 아이템의 갯수 정보를 최신화 하여 줍니다. removeMode가 true일 때 중첩 횟수가 음수인 아이템을 삭제합니다.
    /// </summary>
    public void UpdateInventoryText(bool removeMode=true)
    {      
            
        Text countText;
        for(int i=0; i<miscList.Count; i++)
        { 
            ItemMisc item = (ItemMisc)miscList[i].GetComponent<ItemInfo>().item;           
           
            countText = miscList[i].GetComponentInChildren<Text>();
            countText.text = item.InventoryCount.ToString();        // 중첩 카운트 텍스트를 동기화

            if(removeMode && item.InventoryCount<=0)   // 삭제모드이고 중첩횟수가 0이하이면
            {
                GameObject obj = miscList[i];   // 현재 탐색중인 오브젝트를 기록
                miscList.RemoveAt(i);           // 리스트에서 제거
                Destroy(obj);                   // 실제 오브젝트 삭제
            }            
        }
    }



    /// <summary>
    /// 테스트 버튼 로직 - 현재 세이브 로드가 제대로 되지 않아 버튼에 동작 테스트를 진행 중입니다.
    /// </summary>
    public void OnApplicationQuit()
    {
        //DataManager dataManager = new DataManager();
        //GameData saveData = dataManager.LoadData();

        //saveData.proficiencyDic=proficiencyDic;     // 플레이어 숙련 목록 저장
        //saveData.craftableWeaponList=craftableList; // 플레이어 제작 목록 저장
        //saveData.inventory=inventory;               // 플레이어 인벤토리 저장
        //saveData.gold = playerGold;
        //saveData.silver = playerSilver;                   // 금화와 은화 저장

        //dataManager.SaveData( saveData );

        #if UNITY_EDITOR
            PlayerPrefs.SetInt("IsContinuedGamePlay", 0);   //현재 세이브 로드가 불안정하므로 일시적으로 초기화
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
