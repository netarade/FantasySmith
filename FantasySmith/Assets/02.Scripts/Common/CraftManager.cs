using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using CraftData;
using DataManagement;


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
    public PlayerInventory inventory;


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

    public delegate void LoadEvent();
    // 이벤트 생성
    public static event LoadEvent CraftManagerLoaded;

    void Awake()
    {
        if( instance==null )
            instance=this;
        else
            Destroy( this.gameObject );

        DontDestroyOnLoad( instance );

    }

    /// <summary>
    /// Start메소드를 OnEnable이상으로 고치면 직렬화 안되는 문제가 발생합니다.
    /// </summary>
    void Start()
    {
        DataManager dataManager = new DataManager();
        GameData loadData = dataManager.LoadData();

        proficiencyDic=loadData.proficiencyDic;   // 플레이어 숙련 목록 불러오기
        craftableList=loadData.craftableWeaponList; // 플레이어 제작 목록 불러오기

        inventory=loadData.inventory;           // 플레이어 인벤토리 불러오기
        miscList=inventory.miscList;
        weapList=inventory.weapList;          // 접근의 편리함을 위해 따로 추가 선언 하였음.

        //CraftManagerLoaded();   // 모든 작업이 끝났음을 알려서 연결된 메소드를 호출하여준다.
    }

    private void OnApplicationQuit()
    {
        DataManager dataManager = new DataManager();
        GameData saveData = dataManager.LoadData();

        saveData.proficiencyDic=proficiencyDic;     // 플레이어 숙련 목록 저장
        saveData.craftableWeaponList=craftableList; // 플레이어 제작 목록 저장
        saveData.inventory=inventory;               // 플레이어 인벤토리 저장

        dataManager.SaveData( saveData );
    }

}
