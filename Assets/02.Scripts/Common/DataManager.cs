#define USE_NEWTONSOFT_JSON 
// #define지시문은 컴파일러에게 특정한 심볼이 정의되었음을 알리고,
// 컴파일되기 전에 실행되는 단계에서 특정 코드 블록을 포함하거나 제외하게 하는 역할
// 패키지를 사용하는 메서드 블록을 컴파일할 것인지, 아니면 대체 메서드 블록을 컴파일할 것인지를 결정

using UnityEngine;
using System.IO;
using Newtonsoft.Json;

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
 */

namespace DataManagement
{ 
    /// <summary>
    /// JsonUtility를 사용하여 데이터를 저장해주는 클래스                                        <br/>
    /// 1. 원하는 시점에 데이터 매니저 인스턴스를 생성하고, 게임데이터를 로드하여 생성합니다. (새로 덮어쓰기 방지) <br/>
    /// DataManager dataManager = new DataManager();                                        <br/>
    /// GameData gameData = LoadData();                                                     <br/>
    /// 저장된 데이터가 없는 경우에 GameData 인스턴스를 새롭게 만들어 반환합니다.                    <br/><br/>
    /// 2. 로드 된 데이터에 수정작업을 합니다.                                                   <br/>
    /// gameData.playTime = timePassed;                                                     <br/><br/>
    /// 3. 다시 저장합니다.                                                                   <br/>
    /// dataManager.SaveData();                                                             <br/><br/>
    /// 
    /// 기본 0번 슬롯으로 되어있으며, 오버로딩하여 두번 째 인자에 슬롯 번호를 지정하여 다른 슬롯으로 저장이 가능합니다.<br/>
    /// 
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// 현재 저장과 로드를 하고있는 폴더 경로를 설정하고 반환합니다. 설정하지 않았다면 기본 경로를 반환합니다.<br/>
        /// *****해당 폴더가 실제로 존재하지 않으면 저장, 로드가 되지 않으니 주의합니다.*****
        /// </summary>
        public string Path { get; set; }           
        
        /// <summary>
        /// 확장자를 설정하고 반환합니다.
        /// </summary>
        public string Extension {get; set;}

        /// <summary>
        /// 디폴트 생성자에서 기본 경로와 기본확장자를 초기화합니다.
        /// </summary>
        public DataManager()
        {
            Path = Application.persistentDataPath + "/";
            Extension = ".json";
        }

        /// <summary>
        /// 사용자가 원하는 경로를 지정할 수 있습니다.<br/>
        /// *****해당 폴더가 실제로 존재하지 않으면 저장, 로드가 되지 않으니 주의합니다.*****
        /// </summary>
        /// <param name="path">절대경로 또는 상대경로입니다.</param>
        /// <param name="isRelative">상대경로인지 여부(기본값 true)</param>
        /// <param name="extension">확장자명(기본값 .json)</param>
        public DataManager(string path, bool isRelative=true, string extension=".json")
        {
            if(isRelative)
                Path = Application.persistentDataPath + "/" + path + "/";	
            else
                Path = path;

            Extension = extension;
        }

        /// <summary>
        /// 사용자가 원하는 경로를 지정할 수 있습니다.<br/>
        /// *****해당 폴더가 실제로 존재하지 않으면 저장, 로드가 되지 않으니 주의합니다.*****
        /// </summary>
        /// <param name="path">절대경로 또는 상대경로입니다.</param>
        /// <param name="isRelative">상대경로인지 여부(기본값 true)</param>
        public void SetPath(string path, bool isRelative=true)
        {
            if(isRelative)
                Path = Application.persistentDataPath + "/" + path + "/";	
            else
                Path = path;
        }


        #if USE_NEWTONSOFT_JSON
        /// <summary>
        /// 현재 슬롯에 파일이 존재하는지 여부를 반환합니다.
        /// </summary>
        /// <param name="slotNum"></param>
        /// <returns>true or false</returns>
        public bool IsDataExistInSlot(int slotNum)
        {
            return File.Exists(Path + $"S{slotNum}" + Extension);
        }

        /// <summary>
        /// Newtonsoft.Json 패키지를 설치한 경우 사용할 수 있습니다. 딕셔너리까지 직렬화 해줍니다.<br/>
        /// Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json <br/>
        /// </summary>
        /// <param name="gameData">사용자 정의 자료형</param>
        /// <param name="saveSlot">세이브할 슬롯</param>
        public void SaveData<T>(T gameData, int saveSlot=0 ) where T : GameData
        {
            string data = JsonConvert.SerializeObject(gameData, Formatting.Indented);
            File.WriteAllText(Path + "S" + saveSlot.ToString() + Extension, data);
        }

        /// <summary>
        /// Newtonsoft.Json 패키지를 설치한 경우 사용할 수 있습니다. 딕셔너리까지 역직렬화 해줍니다. <br/>
        /// Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json <br/>
        /// </summary>
        /// <param name="loadSlot">로드할 슬롯</param>
        /// <returns>사용자 정의 자료형</returns>
        public T LoadData<T>(int loadSlot=0 ) where T : GameData, new()  // 제약조건: GameData 인터페이스의 종류이며, 디폴트 생성자가 존재
        {
            if( IsDataExistInSlot(loadSlot) ) // 저장된 슬롯이 있따면 기존 게임데이터를 만들어 반환
            {
                string data = File.ReadAllText(Path + "S" + loadSlot.ToString() + Extension);
                return JsonConvert.DeserializeObject<T>(data); 
            }
            else //저장된 슬롯이 없다면 새롭게 게임데이터를 만들어 반환
            {
                return new T();   
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