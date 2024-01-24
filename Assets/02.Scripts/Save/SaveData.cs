
/*
 * [파일 목적]
 * 게임 세이브와 로드에 필요한 데이터 클래스 구성 
 * 
 * [작업 사항]  
 * <v1.0 - 2023_1106_최원준>
 * 1- 초기 GameData 설정
 * 
 * <v1.1 - 2023_1106_최원준
 * 1- Transform 변수 제거, 클래스 Craftdic 변수 추가
 * 2- 주석 수정
 * 
 * <v1.2 - 2023_1108_최원준>
 * 1- Inventory 클래스가 게임오브젝트 리스트를 포함하고 있기 때문에 저장안되는 문제가 있음을 알고
 * 내부적으로 ItemInfo 리스트로 만들어 보려고 시도하였음. 
 * 또 다른 문제점이 있는데 ItemInfo는 Image컴포넌트를 포함하고 있기 때문에 클래스 구조상 직렬화하기 힘들다고 생각됨.
 * 
 * <v2.0 - 2023_1119_최원준>
 * 1- Inventory 클래스의 직렬화 및 역직렬화 구현완료
 * 2- SerializableInventory 클래스를 Inventory 파일로 위치로 옮김.
 * 씬 전환시에도 사용하기 위해
 * 3- 플레이어 위치, 회전 정보를 담기 위한 STransform 클래스 정의
 * 
 * <v2.1 - 2023_1218_최원준>
 * 1- 금화 은화 변수 int형으로 변경
 * 
 * <v3.0 - 2023_1222_최원준>
 * 1- DataManager클래스의 Load메서드에서 GameData 기본생성자를 호출하는데 정의가 되어있지 않아 새롭게 정의 (STransform 기본 생성자 포함)
 * 
 * 2- saveInventory변수를 private에서 public으로 변경하였음.
 * public아닌 변수는 Json에서 직렬화가 불가능한 변수가 되기 때문
 * 
 * 3- 인벤토리 클래스를 프로퍼티로 직렬화 인벤토리 클래스로 변환하여 저장하던 것을 메서드를 통해 변환하여 저장하도록 수정함.
 * (Inventory프로퍼티 삭제, SaveInventory, LoadInventory메서드를 추가)
 * =>이유는 프로퍼티는 set을통해 직렬화가능한 변수에 저장한다고 하더라도 프로퍼티 자체에 받자마자 저장이 되기 때문에 
 * GameData에 해당 프로퍼티가 포함되어 있으면 직렬화 처리가 불가능한 것을 발견하였음. 
 * (Inventory 클래스는 List<GameObject>를 포함하기 때문에 프로퍼티로 GameData에 담기는 순간 직렬화처리 불가능해 저장이 안되었음.)
 * 
 * 
 * <v3.1 - 2023_1222_최원준>
 * 1- private변수를 직렬화하기 위해 [JsonProperty] 어트리뷰트를 추가하였음
 * 2- STransform 클래스에 Seiralize메서드를 추가하여 일관성을 주었으며, 주석 보완
 * 
 * <v4.0 - 2023_1227_최원준>
 * 1- GameData클래스의 변수와 메서드들을 PlayerBasicData와 PlayerInvenData로 나눈 후, GameData를 인터페이스 처리하였음.
 * 이유는 세이브와 로드를 관리할 작업자나 사용되어지는 스크립트 파일 위치가 틀리기 때문
 * 
 * 2- SerializableInventory의 LoadInventory 및 SaveInventory메서드를 삭제하고
 * 클래스명을 SInventory로 변경 후 해당 클래스 내부에 Serialize 메서드와 Deserialize메서드를 호출하게 하였음.
 * 이유는 STransform과 메서드를 비슷하게 만들어 사용에 일관성을 주기 위함.
 * 
 * <v4.1 - 2023_1229_최원준>
 * 1- 클래스 및 파일명 변경 GameData->SaveData
 * 2- 파일분리 - SaveData_Player, SaveData_Inventory, SaveData_Transform으로 분리 (관련 클래스를 모으기 위해)
 * 
 * 
 */



namespace DataManagement
{    
    /// <summary>
    /// 데이터 세이브 및 로드를 위한 인터페이스입니다. <br/>
    /// 세이브할 데이터 클래스를 만든 후 이 인터페이스를 상속해서 Save와 Load를 해야 합니다. <br/>
    /// </summary>
    public interface SaveData { }
          

}