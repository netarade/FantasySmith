#define USE_NEWTONSOFT_JSON 
// #define���ù��� �����Ϸ����� Ư���� �ɺ��� ���ǵǾ����� �˸���,
// �����ϵǱ� ���� ����Ǵ� �ܰ迡�� Ư�� �ڵ� ����� �����ϰų� �����ϰ� �ϴ� ����
// ��Ű���� ����ϴ� �޼��� ����� �������� ������, �ƴϸ� ��ü �޼��� ����� �������� �������� ����

using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

/*
 * [�۾� ����]  
 * <v1.0 - 2023_1106_�ֿ���>
 * 1 - Binary��� ����
 * <v2.0 - 2023_1106_�ֿ���>
 * 1 - Json������� ����
 * 2 - ��ųʸ� ����ȭ �ȵǴ� ������ ���� Newtonsoft.Json ��Ű�� ������� ����
 * Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json �˻��� ���� Add���ֱ� �ٶ��ϴ�.
 */

namespace DataManagement
{ 
    /// <summary>
    /// JsonUtility�� ����Ͽ� �����͸� �������ִ� Ŭ����                                        <br/>
    /// 1. ���ϴ� ������ ������ �Ŵ��� �ν��Ͻ��� �����ϰ�, ���ӵ����͸� �ε��Ͽ� �����մϴ�. (���� ����� ����) <br/>
    /// DataManager dataManager = new DataManager();                                        <br/>
    /// GameData gameData = LoadData();                                                     <br/>
    /// ����� �����Ͱ� ���� ��쿡 GameData �ν��Ͻ��� ���Ӱ� ����� ��ȯ�մϴ�.                    <br/><br/>
    /// 2. �ε� �� �����Ϳ� �����۾��� �մϴ�.                                                   <br/>
    /// gameData.playTime = timePassed;                                                     <br/><br/>
    /// 3. �ٽ� �����մϴ�.                                                                   <br/>
    /// dataManager.Save();                                                                 <br/><br/>
    /// 
    /// �⺻ 0�� �������� �Ǿ�������, �����ε��Ͽ� �ι� ° ���ڿ� ���� ��ȣ�� �����Ͽ� �ٸ� �������� ������ �����մϴ�.<br/>
    /// 
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// ���� ���� ��θ� ��ȯ�մϴ�.
        /// </summary>
        public string Path {get;}           
    
        /// <summary>
        /// ����Ʈ �����ڿ��� �⺻ ��θ� �����մϴ�.
        /// </summary>
        public DataManager()
        {
            Path = Application.persistentDataPath + "/save";	
        }

        /// <summary>
        /// ����ڰ� ���ϴ� ��θ� ������ �� �ֽ��ϴ�.
        /// </summary>
        public DataManager(string path)
        {
            Path = Application.persistentDataPath + path;	
        }


        #if USE_NEWTONSOFT_JSON
        /// <summary>
        /// ���� ���Կ� ������ �����ϴ��� ���θ� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="slotNum"></param>
        /// <returns>true or false</returns>
        public bool IsDataExistInSlot(int slotNum)
        {
            return File.Exists(Path + $"{slotNum}" +".json");
        }
        /// <summary>
        /// Newtonsoft.Json ��Ű���� ��ġ�� ��� ����� �� �ֽ��ϴ�. ��ųʸ����� ����ȭ ���ݴϴ�.<br/>
        /// Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json <br/>
        /// </summary>
        /// <param name="gameData">����� ���� �ڷ���</param>
        /// <param name="saveSlot">���̺��� ����</param>
        public void SaveData(GameData gameData, int saveSlot=0 )
        {
            string data = JsonConvert.SerializeObject(gameData, Formatting.Indented);
            File.WriteAllText(Path + saveSlot.ToString() + ".json", data);
        }
        /// <summary>
        /// Newtonsoft.Json ��Ű���� ��ġ�� ��� ����� �� �ֽ��ϴ�. ��ųʸ����� ������ȭ ���ݴϴ�. <br/>
        /// Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json <br/>
        /// </summary>
        /// <param name="loadSlot">�ε��� ����</param>
        /// <returns>����� ���� �ڷ���</returns>
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
        /// ���� ���Կ� ������ �����ϴ��� ���θ� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="slotNum"></param>
        /// <returns>true or false</returns>
        public bool IsDataExistInSlot(int slotNum)
        {
            return File.Exists(Path + $"{slotNum}");
        }
        /// <summary>
        /// ���̺��� ���� �����͸� �����մϴ�.
        /// </summary>
        /// <param name="gameData">����� ���� �ڷ���</param>
        /// <param name="saveSlot">���̺��� ����</param>
        public void SaveData(GameData gameData, int saveSlot=0 )
        {
            string data = JsonUtility.ToJson(gameData);
            File.WriteAllText(Path+ saveSlot.ToString(), data);
        }

        /// <summary>
        /// ���� �����͸� �ε� �մϴ�.
        /// </summary>
        /// <param name="loadSlot">�ε��� ����</param>
        /// <returns>����� ���� �ڷ���</returns>
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