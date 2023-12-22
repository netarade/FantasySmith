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
 * 
 * <v3.0 - 2023_1120_최원준>
 * 1- CraftManager 클래스 및 파일명 변경 PlayerInven, 파일위치 이동 Common폴더 -> Player폴더
 * 
 * 2- PlayerInven 클래스 역할 설정
 * a. 플레이어의 인벤토리 데이터를 보관하는 역할
 * b. 게임시작 및 게임종료 시 플레이어 인벤토리를 로드 및 세이브해주며, 씬 전환시 인벤토리를 유지하게 해줘야 한다.
 * c. 실제 플레이어 오브젝트에 스크립트가 붙어있어야 한다. (플레이어가 들고있는 데이터모음이 될 것이다.)
 * d. 오브젝트가 파괴될 때(게임 종료 시)
 * 
 * 3- weapList, miscList를 제거 (inventory쪽에 기능을 추가하여 접근성을 높일 계획이므로)
 * 4- 기타 로직들 제거 및 주석처리 (미완료)
 * 5- UpdateInventoryText 메서드 주석처리 - inventory쪽에 해당 기능을 추가 할 예정이므로
 * 
 * <v3.1 - 2023_1122_최원준>
 * 1- 인벤토리 및 숙련사전 주석 수정 보완
 * 
 * <v4.0 - 2023_1216_최원준>
 * 1- slotListTr항목을 추가 - CreateManager에 PlayerInven 스크립트를 전달하여 참조시킬 필요성 때문
 * 2- 싱글톤 제거 - 다른 스크립트의 start 구문 등에서 참조를 할 때 싱글톤이 그보다 더 빠르게 초기화되지 않으면 안되는데
 * OnSceneLoad로 초기화를 하다보면 느리기 때문
 * 
 * <v4.1 - 2023_1217_최원준>
 * 1- 스크립트 간소화, LoadPlayerData, SavePlayerData 메서드 추가하여 
 * Awake,OnDestroy,OnApplicationQuit에서 호출되도록 수정
 * 
 * 2- 게임매니저 싱글톤에서 isNewGame상태를 읽어와서 새로운 인벤토리를 생성하는 구문 추가
 * 
 * <v4.2 - 2023_1221_최원준>
 * 1- OnApplicationQuit 메서드내부에 에디터와 프로그램 종료 로직을 넣었던 점 삭제 
 * 2- 게임매니저의 인스턴스로 isNewGame을 판별하던 구문을 PlayerPrefs의 키값 참조로 변경
 * 이유는 PlayerInven 스크립트가 Awake로 빠른 초기화를 해야하는데 싱글톤으로의 접근시점이 같아서 불러오기 어렵기 때문 
 * 3- 테스트용 키삭제 구문 PlayerPrefs.DeleteAll 추가
 * 
 * <v5.0 - 2023_1222_최원준>
 * 1- 클래스와 파일명을 PlayerInven에서 InventoryInfo로 수정 
 * (ItemInfo와 이름의 일관성을 맞추기 위함)
 * 
 * <v5.1 - 2023_1222_최원준>
 * 1- Awkae문에서 PlayerPrefs의 키값을 통해 처음시작과 이어하기를 구분해서 메서드를 호출하던 구문을 삭제하고
 * LoadData하나만 호출해도 처음 데이터가 만들어지도록 변경하였습니다.
 * 
 */

    

/// <summary>
/// 게임 실행 중 제작 관련 실시간 플레이어 정보 들을 보유하고 있는 전용 정보 클래스입니다.<br/>
/// 인스턴스를 생성하여 정보를 확인하시기 바랍니다.
/// </summary>
public class InventoryInfo : MonoBehaviour
{
    /// <summary>
    /// 플레이어가 보유하고 있는 아이템을 보관하는 인벤토리와 관련된 정보를 가지고 있는 클래스입니다.<br/>
    /// 인벤토리에 아이템을 생성하고 제거하거나 현재 아이템의 검색 기능 등을 가지고 있습니다.<br/>
    /// 딕셔너리 내부에 게임 오브젝트를 보유하고 있으므로 씬 전환이나 세이브 로드 시에 반드시 Item 형식의 List로의 Convert가 필요합니다.
    /// </summary>
    public Inventory inventory;

    /// <summary>
    /// 플레이어가 현재 제작가능한 장비와 해당 장비의 숙련도를 알려주는 딕셔너리를 보유한 클래스입니다.<br/>
    /// 해당 종류의 딕셔너리에 아이템 이름으로 접근하여 구조체를 할당받아 정보를 확인합니다.
    /// </summary>
    public Craftdic craftDic;


    /// <summary>
    /// 게임 중에 기록하는 골드 플레이어 관련 변수입니다.
    /// </summary>
    public int gold; 
    /// <summary>
    /// 게임 중에 기록하는 실버 플레이어 관련 변수입니다.
    /// </summary>
    public int silver;      
         

    // 인스턴스가 새롭게 생성될 때마다 저장된 파일을 불러옵니다.
    void Awake()
    {        
        LoadPlayerData();       // 저장된 플레이어 데이터를 불러옵니다.        
    }
        

    // 파괴 되기전에 파일에 저장합니다.
    private void OnDestroy()
    {
        SavePlayerData();   // 플레이어 데이터 저장
    }
       
    /// <summary>
    /// 플레이어가 게임을 종료할 때
    /// </summary>
    public void OnApplicationQuit()
    {
        SavePlayerData(); // 플레이어 데이터 저장
    }


    /// <summary>
    /// 플레이어 변수들을 초기화 시켜주는 메서드
    /// </summary>
    private void InitPlayerData()
    {        
        inventory=new Inventory();
        craftDic=new Craftdic();
        gold=0;
        silver=0;            
    }

    /// <summary>
    /// 인벤토리 관련 플레이어 데이터를 불러옵니다
    /// </summary>
    void LoadPlayerData()
    {
        DataManager dataManager = new DataManager();
        GameData loadData = dataManager.LoadData(9);

        inventory=loadData.LoadInventory();
        craftDic = loadData.craftDic;
        gold = loadData.gold;
        silver = loadData.silver;
    }


    /// <summary>
    /// 인벤토리 관련 플레이어 데이터를 저장합니다 
    /// </summary>
    void SavePlayerData()
    {
        DataManager dataManager = new DataManager();
        
        // 메서드 호출 시점에 다른 스크립트에서 save했을 수도 있으므로 새롭게 생성하지 않고 기존 데이터 최신화합니다
        GameData saveData = dataManager.LoadData(9);

        saveData.SaveInventory(inventory);
        saveData.craftDic = craftDic;
        saveData.gold = gold;
        saveData.silver = silver;
        dataManager.SaveData(saveData,9);
    }

    

}
