#define USE_NEWTONSOFT_JSON 
// #define���ù��� �����Ϸ����� Ư���� �ɺ��� ���ǵǾ����� �˸���,
// �����ϵǱ� ���� ����Ǵ� �ܰ迡�� Ư�� �ڵ� ����� �����ϰų� �����ϰ� �ϴ� ����
// ��Ű���� ����ϴ� �޼��� ����� �������� ������, �ƴϸ� ��ü �޼��� ����� �������� �������� ����

using UnityEngine;
using System.IO;
using Newtonsoft.Json;

/*
 * [�۾� ����]  
 * <v1.0 - 2023_1106_�ֿ���>
 * 1 - Binary��� ����
 * <v2.0 - 2023_1106_�ֿ���>
 * 1 - Json������� ����
 * 2 - ��ųʸ� ����ȭ �ȵǴ� ������ ���� Newtonsoft.Json ��Ű�� ������� ����
 * Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json �˻��� ���� Add���ֱ� �ٶ��ϴ�.
 * 
 * <v2.1 - 2023_1222_�ֿ���>
 * 1- �ּ��߰�
 * 
 * <v3.0 - 2023_1227_�ֿ���>
 * 1- SaveData�� LoadData�� �Ϲ�ȭ �޼���� ����
 * ������ GameData�� Ŭ���� �и��� �ʿ��Ͽ� GameData�� �������̽��� ����� ����ϴ� ���ο� Ŭ������ ���ڷ� �޵��� �����Ͽ���.
 * 2- ���� path���� ����, Path ������Ƽ set�߰�, SetPath�޼��� �߰�
 * 3- Extension(Ȯ����) �Ӽ��߰�
 * 4- Path�� Extension�� ������ �� �ִ� ������ �߰�
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
    /// dataManager.SaveData();                                                             <br/><br/>
    /// 
    /// �⺻ 0�� �������� �Ǿ�������, �����ε��Ͽ� �ι� ° ���ڿ� ���� ��ȣ�� �����Ͽ� �ٸ� �������� ������ �����մϴ�.<br/>
    /// 
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// ���� ����� �ε带 �ϰ��ִ� ���� ��θ� �����ϰ� ��ȯ�մϴ�. �������� �ʾҴٸ� �⺻ ��θ� ��ȯ�մϴ�.<br/>
        /// *****�ش� ������ ������ �������� ������ ����, �ε尡 ���� ������ �����մϴ�.*****
        /// </summary>
        public string Path { get; set; }           
        
        /// <summary>
        /// Ȯ���ڸ� �����ϰ� ��ȯ�մϴ�.
        /// </summary>
        public string Extension {get; set;}

        /// <summary>
        /// ����Ʈ �����ڿ��� �⺻ ��ο� �⺻Ȯ���ڸ� �ʱ�ȭ�մϴ�.
        /// </summary>
        public DataManager()
        {
            Path = Application.persistentDataPath + "/";
            Extension = ".json";
        }

        /// <summary>
        /// ����ڰ� ���ϴ� ��θ� ������ �� �ֽ��ϴ�.<br/>
        /// *****�ش� ������ ������ �������� ������ ����, �ε尡 ���� ������ �����մϴ�.*****
        /// </summary>
        /// <param name="path">������ �Ǵ� ������Դϴ�.</param>
        /// <param name="isRelative">��������� ����(�⺻�� true)</param>
        /// <param name="extension">Ȯ���ڸ�(�⺻�� .json)</param>
        public DataManager(string path, bool isRelative=true, string extension=".json")
        {
            if(isRelative)
                Path = Application.persistentDataPath + "/" + path + "/";	
            else
                Path = path;

            Extension = extension;
        }

        /// <summary>
        /// ����ڰ� ���ϴ� ��θ� ������ �� �ֽ��ϴ�.<br/>
        /// *****�ش� ������ ������ �������� ������ ����, �ε尡 ���� ������ �����մϴ�.*****
        /// </summary>
        /// <param name="path">������ �Ǵ� ������Դϴ�.</param>
        /// <param name="isRelative">��������� ����(�⺻�� true)</param>
        public void SetPath(string path, bool isRelative=true)
        {
            if(isRelative)
                Path = Application.persistentDataPath + "/" + path + "/";	
            else
                Path = path;
        }


        #if USE_NEWTONSOFT_JSON
        /// <summary>
        /// ���� ���Կ� ������ �����ϴ��� ���θ� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="slotNum"></param>
        /// <returns>true or false</returns>
        public bool IsDataExistInSlot(int slotNum)
        {
            return File.Exists(Path + $"S{slotNum}" + Extension);
        }

        /// <summary>
        /// Newtonsoft.Json ��Ű���� ��ġ�� ��� ����� �� �ֽ��ϴ�. ��ųʸ����� ����ȭ ���ݴϴ�.<br/>
        /// Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json <br/>
        /// </summary>
        /// <param name="gameData">����� ���� �ڷ���</param>
        /// <param name="saveSlot">���̺��� ����</param>
        public void SaveData<T>(T gameData, int saveSlot=0 ) where T : GameData
        {
            string data = JsonConvert.SerializeObject(gameData, Formatting.Indented);
            File.WriteAllText(Path + "S" + saveSlot.ToString() + Extension, data);
        }

        /// <summary>
        /// Newtonsoft.Json ��Ű���� ��ġ�� ��� ����� �� �ֽ��ϴ�. ��ųʸ����� ������ȭ ���ݴϴ�. <br/>
        /// Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json <br/>
        /// </summary>
        /// <param name="loadSlot">�ε��� ����</param>
        /// <returns>����� ���� �ڷ���</returns>
        public T LoadData<T>(int loadSlot=0 ) where T : GameData, new()  // ��������: GameData �������̽��� �����̸�, ����Ʈ �����ڰ� ����
        {
            if( IsDataExistInSlot(loadSlot) ) // ����� ������ �ֵ��� ���� ���ӵ����͸� ����� ��ȯ
            {
                string data = File.ReadAllText(Path + "S" + loadSlot.ToString() + Extension);
                return JsonConvert.DeserializeObject<T>(data); 
            }
            else //����� ������ ���ٸ� ���Ӱ� ���ӵ����͸� ����� ��ȯ
            {
                return new T();   
            }
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