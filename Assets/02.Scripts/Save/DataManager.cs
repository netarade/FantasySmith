#define USE_NEWTONSOFT_JSON 
// #define지시문은 컴파일러에게 특정한 심볼이 정의되었음을 알리고,
// 컴파일되기 전에 실행되는 단계에서 특정 코드 블록을 포함하거나 제외하게 하는 역할
// 패키지를 사용하는 메서드 블록을 컴파일할 것인지, 아니면 대체 메서드 블록을 컴파일할 것인지를 결정

using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using CreateManagement;
using System;

/*
 * [작업 사항]  
 * <v1.0 - 2023_1106_최원준>
 * 1 - Binary방식 저장
 * <v2.0 - 2023_1106_최원준>
 * 1 - Json방식으로 변경
 * 2 - 딕셔너리 직렬화 안되는 문제로 인해 Newtonsoft.Json 패키지 방식으로 구현
 * Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json 검색을 통해 Add해주기 바랍니다.
 * 
 * <v2.1 - 2023_1222_최원준>
 * 1- 주석추가
 * 
 * <v3.0 - 2023_1227_최원준>
 * 1- SaveData와 LoadData를 일반화 메서드로 변경
 * 이유는 GameData의 클래스 분리가 필요하여 GameData를 인터페이스로 만들고 상속하는 새로운 클래스를 인자로 받도록 설정하였음.
 * 2- 기존 path변수 삭제, Path 프로퍼티 set추가, SetPath메서드 추가
 * 3- Extension(확장자) 속성추가
 * 4- Path와 Extension을 변경할 수 있는 생성자 추가
 * 
 * <v4.0 - 2023_1229_최원준>
 * 1- FileName과 SlotNum 프로퍼티 추가
 * 2- 경로설정 생성자를 지우고 이름 및 슬롯, 확장자를 받는 생성자 추가
 * 3- 인자로 넣은 파일이름으로 저장 및 로드되도록 변경
 * 4- 주석 일부 수정
 * 5- IsDataExistInSlot, SaveData, LoadData 메서드의 슬롯번호를 인자로 넣던 것 삭제
 * 6- 메서드명 변경 IsDataExistInSlot->IsFileExist
 * 
 * <v4.1 - 2023_1229_최원준>
 * 1- 매개변수 생성자에서 Path를 초기화안해주던 점 수정
 * 
 * <v4.2 - 2024_0104_최원준>
 * 1- 인스턴스 생성 방식에서 스크립트 컴포넌트 방식으로 변경
 * (멀티에서 스크립트마다 인스턴스 생성을 방지)
 * 
 * <v4.3 - 2024_0110_최원준>
 * 1- Path지정 메서드를 주석처리하였으며, Path를 고정값 읽기전용 프로퍼티로 설정하였습니다.
 * 이유는 Application경로를 이니셜라이저를 통해 할당할 수 없기 때문에 Awake문에서 초기화를 해야하는데
 * 다른 스크립트의 Awake문에서 Load를 해버리면 Path값이 안들어온 상태에서 불러오기가 진행되기 때문입니다.
 * 
 * <v4.4 - 2024_0112_최원준>
 * 1- 인벤토리 전용 초기화 인자를 전달받는 로드메서드를 만듬.
 * 이유는 매개변수를 전달받는 T타입 생성자호출이 어렵기 때문 
 * 
 * <v4.5 - 2024_0124_최원준>
 * 1- 인벤토리전용 메서드인 LoadData의 메서드명을 LoadInventoryData로 변경
 * 2- LoadInventoryData에 initializer에 isReset속성이 결정되어 있으면 Id를 새롭게 부여하도록 결정
 * 
 */

namespace DataManagement
{ 
    /// <summary>
    /// JsonUtility를 사용하여 데이터를 저장해주는 클래스                                        <br/>
    /// Newtonsoft.Json 패키지를 설치한 경우 사용할 수 있습니다. 딕셔너리까지 직렬화 해줍니다.<br/>
    /// --------------------------- 설치법 --------------------------------------------------<br/>
    /// 메뉴 > Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json 패키지 다운 <br/><br/>
    /// --------------------------- 사용법 --------------------------------------------------<br/>
    /// 1. 원하는 시점에 데이터 매니저 인스턴스를 생성하고, 게임데이터를 로드하여 생성합니다. (새로 덮어쓰기 방지) <br/>
    /// DataManager dataManager = new DataManager();                                        <br/>
    /// SaveData gameData = LoadData();                                                     <br/>
    /// 저장된 데이터가 없는 경우에 GameData 인스턴스를 새롭게 만들어 반환합니다.                    <br/><br/>
    /// 2. 로드 된 데이터에 수정작업을 합니다.                                                   <br/>
    /// gameData.playTime = timePassed;                                                     <br/><br/>
    /// 3. 다시 저장합니다.                                                                   <br/>
    /// dataManager.SaveData();                                                             <br/><br/>
    /// 
    /// 
    /// 기본 0번 슬롯으로 되어있으며, 오버로딩하여 두번 째 인자에 슬롯 번호를 지정하여 다른 슬롯으로 저장이 가능합니다.<br/>
    /// 
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        /// <summary>
        /// 현재 저장과 로드를 하고있는 폴더 경로를 설정하고 볼 수 있습니다. 설정하지 않았다면 기본 경로를 반환합니다.<br/>
        /// *****해당 폴더가 실제로 존재하지 않으면 저장, 로드가 되지 않으니 주의합니다.*****
        /// </summary>
        public string Path { get; set; }           
        
        /// <summary>
        /// 확장자를 설정하거나 볼 수 있습니다.
        /// </summary>
        public string Extension {get; set;}

        /// <summary>
        /// 저장할 파일 이름을 설정하거나 볼 수 있습니다.
        /// </summary>
        public string FileName {get; set;}


        /// <summary>
        /// 저장할 슬롯을 설정하거나 볼 수 있습니다.
        /// </summary>
        public int SlotNum {get; set; }


        private void Awake()
        {
            Path = Application.persistentDataPath+"/";
            FileName = "NoName";
            Extension = ".json";
            SlotNum = 0;
        }
                

        /// <summary>
        /// 사용자가 원하는 파일이름, 슬롯과, 확장자명을 지정할 수 있습니다.<br/>
        /// </summary>
        /// <param name="fileName">저장할 파일 이름</param>
        /// <param name="slotNum">저장할 슬롯 번호</param>
        /// <param name="extension">확장자명(기본값 .json)</param>
        public void FileSettings(string fileName, int slotNum=0, string extension=".json")
        {
            FileName = fileName;
            SlotNum = slotNum;
            Extension = extension;
        }



        /// <summary>
        /// 사용자가 원하는 경로를 지정할 수 있습니다.<br/>
        /// *****해당 폴더가 실제로 존재하지 않으면 저장, 로드가 되지 않으니 주의합니다.*****
        /// </summary>
        /// <param name="path">절대경로 또는 상대경로입니다.</param>
        /// <param name="isRelative">상대경로인지 여부(기본값 true)</param>
        public void SetPath( string path, bool isRelative = true )
        {
            if( isRelative )
                Path=Application.persistentDataPath+"/"+path+"/";
            else
                Path=path;
        }


        #if USE_NEWTONSOFT_JSON
        /// <summary>
        /// 현재 슬롯에 파일이 존재하는지 여부를 반환합니다.
        /// </summary>
        /// <param name="slotNum">저장된 슬롯 번호</param>
        /// <returns>true or false</returns>
        public bool IsFileExist()
        {
            return File.Exists(Path + FileName + "S" + SlotNum.ToString() + Extension);
        }

        /// <summary>
        /// 세이브 데이터를 저장합니다. 인자로 전달하는 T는 SaveData인터페이스를 상속하는 사용자정의 클래스여야 합니다.
        /// </summary>
        /// <param name="gameData">SaveData 인터페이스를 상속하는 사용자 정의 자료형</param>
        /// <param name="saveSlot">세이브할 슬롯</param>
        public void SaveData<T>(T gameData) where T : SaveData
        {
            //Debug.Log("저장진행 중");
            string data = JsonConvert.SerializeObject(gameData, Formatting.Indented);
            File.WriteAllText(Path + FileName + "S" + SlotNum.ToString() + Extension, data);
            //Debug.Log("저장완료");
        }

        /// <summary>
        /// 세이브 데이터를 불러옵니다. 인자로 전달하는 T는 SaveData인터페이스를 상속하는 사용자정의 클래스여야 합니다.
        /// </summary>
        /// <returns>SaveData 인터페이스를 상속하는 사용자 정의 자료형</returns>
        public T LoadData<T>() where T : SaveData, new()  // 제약조건: SaveData 인터페이스의 종류이며, 디폴트 생성자가 존재
        {
            
            if( IsFileExist() ) // 저장된 파일이 있다면 기존 게임데이터를 만들어 반환
            {
                Debug.Log("파일이 존재 " + Path);
                string data = File.ReadAllText(Path + FileName + "S" + SlotNum.ToString() + Extension);
                return JsonConvert.DeserializeObject<T>(data); 
            }
            else //저장된 슬롯이 없다면 새롭게 게임데이터를 만들어 반환
            {
                Debug.Log("파일이 없음");                                    
                return new T();   
            }
        }


        /// <summary>
        /// 인벤토리 전용 로드 메서드입니다. 초기화 전용 클래스 전달인자를 토대로 인벤토리의 초기화를 진행합니다.<br/>
        /// </summary>
        /// <returns>인벤토리 사용자 정의 클래스를 반환</returns>
        public InventorySaveData LoadInventoryData(InventoryInitializer initializer) 
        {
            if(initializer==null)
                throw new System.Exception("이니셜라이져가 전달되지 않았습니다.");

            
            // Id를 등록하기위한 인스턴스를 생성합니다.
            CreateManager createManager = GetComponent<CreateManager>();
            string keyPrefabName = initializer.inventoryInfo.OwnerTr.name;
            int newId = initializer.inventoryId;

            // 리셋 옵션이 전달 된 경우
            if(initializer.isReset)
            {
                Debug.Log("파일 초기화 " + Path);

                // Id중복으로 인해 등록에 실패했다면
                if( !createManager.RegisterId( IdType.Inventory, keyPrefabName, newId) )
                    throw new Exception("Id중복으로 인해 등록에 실패했습니다. 새로운 Id를 부여해 주세요.");

                return new InventorySaveData(initializer); 
            }
            
            if( IsFileExist() ) // 저장된 파일이 있다면 기존 게임데이터를 만들어 반환
            {
                Debug.Log("파일이 존재 " + Path);
                string data = File.ReadAllText(Path + FileName + "S" + SlotNum.ToString() + Extension);
                return JsonConvert.DeserializeObject<InventorySaveData>(data); 
            }
            else //저장된 슬롯이 없다면 새롭게 게임데이터를 만들어 반환
            {
                Debug.Log("파일이 없음");
                
                // Id를 새롭게 등록합니다.
                createManager.RegisterId( IdType.Inventory, keyPrefabName, newId);

                // 인벤토리 데이터를 반환합니다.
                return new InventorySaveData(initializer);   
            }
        }



        





        #else
        /// <summary>
        /// 현재 슬롯에 파일이 존재하는지 여부를 반환합니다.
        /// </summary>
        /// <param name="slotNum"></param>
        /// <returns>true or false</returns>
        public bool IsDataExistInSlot(int slotNum)
        {
            return File.Exists(Path + $"{slotNum}");
        }
        /// <summary>
        /// 세이브할 게임 데이터를 저장합니다.
        /// </summary>
        /// <param name="gameData">사용자 정의 자료형</param>
        /// <param name="saveSlot">세이브할 슬롯</param>
        public void SaveData(GameData gameData, int saveSlot=0 )
        {
            string data = JsonUtility.ToJson(gameData);
            File.WriteAllText(Path+ saveSlot.ToString(), data);
        }

        /// <summary>
        /// 게임 데이터를 로드 합니다.
        /// </summary>
        /// <param name="loadSlot">로드할 슬롯</param>
        /// <returns>사용자 정의 자료형</returns>
        public GameData LoadData( int loadSlot=0 )
        {
            if( IsDataExistInSlot(loadSlot) )
            {
                string data = File.ReadAllText(Path + loadSlot.ToString());             
                return JsonUtility.FromJson<GameData>(data);
            }
            else
                return new GameData();            
        }
        #endif

        
    }
}