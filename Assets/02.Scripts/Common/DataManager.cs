#define USE_NEWTONSOFT_JSON 
// #define지시문은 컴파일러에게 특정한 심볼이 정의되었음을 알리고,
// 컴파일되기 전에 실행되는 단계에서 특정 코드 블록을 포함하거나 제외하게 하는 역할
// 패키지를 사용하는 메서드 블록을 컴파일할 것인지, 아니면 대체 메서드 블록을 컴파일할 것인지를 결정

using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

/*
 * [작업 사항]  
 * <v1.0 - 2023_1106_최원준>
 * 1 - Binary방식 저장
 * <v2.0 - 2023_1106_최원준>
 * 1 - Json방식으로 변경
 * 2 - 딕셔너리 직렬화 안되는 문제로 인해 Newtonsoft.Json 패키지 방식으로 구현
 * Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json 검색을 통해 Add해주기 바랍니다.
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
    /// dataManager.Save();                                                                 <br/><br/>
    /// 
    /// 기본 0번 슬롯으로 되어있으며, 오버로딩하여 두번 째 인자에 슬롯 번호를 지정하여 다른 슬롯으로 저장이 가능합니다.<br/>
    /// 
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// 현재 폴더 경로를 반환합니다.
        /// </summary>
        public string Path {get;}           
    
        /// <summary>
        /// 디폴트 생성자에서 기본 경로를 지정합니다.
        /// </summary>
        public DataManager()
        {
            Path = Application.persistentDataPath + "/save";	
        }

        /// <summary>
        /// 사용자가 원하는 경로를 지정할 수 있습니다.
        /// </summary>
        public DataManager(string path)
        {
            Path = Application.persistentDataPath + path;	
        }


        #if USE_NEWTONSOFT_JSON
        /// <summary>
        /// 현재 슬롯에 파일이 존재하는지 여부를 반환합니다.
        /// </summary>
        /// <param name="slotNum"></param>
        /// <returns>true or false</returns>
        public bool IsDataExistInSlot(int slotNum)
        {
            return File.Exists(Path + $"{slotNum}" +".json");
        }
        /// <summary>
        /// Newtonsoft.Json 패키지를 설치한 경우 사용할 수 있습니다. 딕셔너리까지 직렬화 해줍니다.<br/>
        /// Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json <br/>
        /// </summary>
        /// <param name="gameData">사용자 정의 자료형</param>
        /// <param name="saveSlot">세이브할 슬롯</param>
        public void SaveData(GameData gameData, int saveSlot=0 )
        {
            string data = JsonConvert.SerializeObject(gameData, Formatting.Indented);
            File.WriteAllText(Path + saveSlot.ToString() + ".json", data);
        }
        /// <summary>
        /// Newtonsoft.Json 패키지를 설치한 경우 사용할 수 있습니다. 딕셔너리까지 역직렬화 해줍니다. <br/>
        /// Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json <br/>
        /// </summary>
        /// <param name="loadSlot">로드할 슬롯</param>
        /// <returns>사용자 정의 자료형</returns>
        public GameData LoadData( int loadSlot=0 )
        {
            if( IsDataExistInSlot(loadSlot) )
            {
                string data = File.ReadAllText(Path + loadSlot.ToString() + ".json");
                return JsonConvert.DeserializeObject<GameData>(data);                
            }
            else
                return new GameData();            
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