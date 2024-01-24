#define USE_NEWTONSOFT_JSON 
// #define���ù��� �����Ϸ����� Ư���� �ɺ��� ���ǵǾ����� �˸���,
// �����ϵǱ� ���� ����Ǵ� �ܰ迡�� Ư�� �ڵ� ����� �����ϰų� �����ϰ� �ϴ� ����
// ��Ű���� ����ϴ� �޼��� ����� �������� ������, �ƴϸ� ��ü �޼��� ����� �������� �������� ����

using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using CreateManagement;
using System;

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
 * 
 * <v4.0 - 2023_1229_�ֿ���>
 * 1- FileName�� SlotNum ������Ƽ �߰�
 * 2- ��μ��� �����ڸ� ����� �̸� �� ����, Ȯ���ڸ� �޴� ������ �߰�
 * 3- ���ڷ� ���� �����̸����� ���� �� �ε�ǵ��� ����
 * 4- �ּ� �Ϻ� ����
 * 5- IsDataExistInSlot, SaveData, LoadData �޼����� ���Թ�ȣ�� ���ڷ� �ִ� �� ����
 * 6- �޼���� ���� IsDataExistInSlot->IsFileExist
 * 
 * <v4.1 - 2023_1229_�ֿ���>
 * 1- �Ű����� �����ڿ��� Path�� �ʱ�ȭ�����ִ� �� ����
 * 
 * <v4.2 - 2024_0104_�ֿ���>
 * 1- �ν��Ͻ� ���� ��Ŀ��� ��ũ��Ʈ ������Ʈ ������� ����
 * (��Ƽ���� ��ũ��Ʈ���� �ν��Ͻ� ������ ����)
 * 
 * <v4.3 - 2024_0110_�ֿ���>
 * 1- Path���� �޼��带 �ּ�ó���Ͽ�����, Path�� ������ �б����� ������Ƽ�� �����Ͽ����ϴ�.
 * ������ Application��θ� �̴ϼȶ������� ���� �Ҵ��� �� ���� ������ Awake������ �ʱ�ȭ�� �ؾ��ϴµ�
 * �ٸ� ��ũ��Ʈ�� Awake������ Load�� �ع����� Path���� �ȵ��� ���¿��� �ҷ����Ⱑ ����Ǳ� �����Դϴ�.
 * 
 * <v4.4 - 2024_0112_�ֿ���>
 * 1- �κ��丮 ���� �ʱ�ȭ ���ڸ� ���޹޴� �ε�޼��带 ����.
 * ������ �Ű������� ���޹޴� TŸ�� ������ȣ���� ��Ʊ� ���� 
 * 
 * <v4.5 - 2024_0124_�ֿ���>
 * 1- �κ��丮���� �޼����� LoadData�� �޼������ LoadInventoryData�� ����
 * 2- LoadInventoryData�� initializer�� isReset�Ӽ��� �����Ǿ� ������ Id�� ���Ӱ� �ο��ϵ��� ����
 * 
 */

namespace DataManagement
{ 
    /// <summary>
    /// JsonUtility�� ����Ͽ� �����͸� �������ִ� Ŭ����                                        <br/>
    /// Newtonsoft.Json ��Ű���� ��ġ�� ��� ����� �� �ֽ��ϴ�. ��ųʸ����� ����ȭ ���ݴϴ�.<br/>
    /// --------------------------- ��ġ�� --------------------------------------------------<br/>
    /// �޴� > Window > Package Manager > Add Package from GIT URL > com.unity.nuget.newtonsoft-json ��Ű�� �ٿ� <br/><br/>
    /// --------------------------- ���� --------------------------------------------------<br/>
    /// 1. ���ϴ� ������ ������ �Ŵ��� �ν��Ͻ��� �����ϰ�, ���ӵ����͸� �ε��Ͽ� �����մϴ�. (���� ����� ����) <br/>
    /// DataManager dataManager = new DataManager();                                        <br/>
    /// SaveData gameData = LoadData();                                                     <br/>
    /// ����� �����Ͱ� ���� ��쿡 GameData �ν��Ͻ��� ���Ӱ� ����� ��ȯ�մϴ�.                    <br/><br/>
    /// 2. �ε� �� �����Ϳ� �����۾��� �մϴ�.                                                   <br/>
    /// gameData.playTime = timePassed;                                                     <br/><br/>
    /// 3. �ٽ� �����մϴ�.                                                                   <br/>
    /// dataManager.SaveData();                                                             <br/><br/>
    /// 
    /// 
    /// �⺻ 0�� �������� �Ǿ�������, �����ε��Ͽ� �ι� ° ���ڿ� ���� ��ȣ�� �����Ͽ� �ٸ� �������� ������ �����մϴ�.<br/>
    /// 
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        /// <summary>
        /// ���� ����� �ε带 �ϰ��ִ� ���� ��θ� �����ϰ� �� �� �ֽ��ϴ�. �������� �ʾҴٸ� �⺻ ��θ� ��ȯ�մϴ�.<br/>
        /// *****�ش� ������ ������ �������� ������ ����, �ε尡 ���� ������ �����մϴ�.*****
        /// </summary>
        public string Path { get; set; }           
        
        /// <summary>
        /// Ȯ���ڸ� �����ϰų� �� �� �ֽ��ϴ�.
        /// </summary>
        public string Extension {get; set;}

        /// <summary>
        /// ������ ���� �̸��� �����ϰų� �� �� �ֽ��ϴ�.
        /// </summary>
        public string FileName {get; set;}


        /// <summary>
        /// ������ ������ �����ϰų� �� �� �ֽ��ϴ�.
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
        /// ����ڰ� ���ϴ� �����̸�, ���԰�, Ȯ���ڸ��� ������ �� �ֽ��ϴ�.<br/>
        /// </summary>
        /// <param name="fileName">������ ���� �̸�</param>
        /// <param name="slotNum">������ ���� ��ȣ</param>
        /// <param name="extension">Ȯ���ڸ�(�⺻�� .json)</param>
        public void FileSettings(string fileName, int slotNum=0, string extension=".json")
        {
            FileName = fileName;
            SlotNum = slotNum;
            Extension = extension;
        }



        /// <summary>
        /// ����ڰ� ���ϴ� ��θ� ������ �� �ֽ��ϴ�.<br/>
        /// *****�ش� ������ ������ �������� ������ ����, �ε尡 ���� ������ �����մϴ�.*****
        /// </summary>
        /// <param name="path">������ �Ǵ� ������Դϴ�.</param>
        /// <param name="isRelative">��������� ����(�⺻�� true)</param>
        public void SetPath( string path, bool isRelative = true )
        {
            if( isRelative )
                Path=Application.persistentDataPath+"/"+path+"/";
            else
                Path=path;
        }


        #if USE_NEWTONSOFT_JSON
        /// <summary>
        /// ���� ���Կ� ������ �����ϴ��� ���θ� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="slotNum">����� ���� ��ȣ</param>
        /// <returns>true or false</returns>
        public bool IsFileExist()
        {
            return File.Exists(Path + FileName + "S" + SlotNum.ToString() + Extension);
        }

        /// <summary>
        /// ���̺� �����͸� �����մϴ�. ���ڷ� �����ϴ� T�� SaveData�������̽��� ����ϴ� ��������� Ŭ�������� �մϴ�.
        /// </summary>
        /// <param name="gameData">SaveData �������̽��� ����ϴ� ����� ���� �ڷ���</param>
        /// <param name="saveSlot">���̺��� ����</param>
        public void SaveData<T>(T gameData) where T : SaveData
        {
            //Debug.Log("�������� ��");
            string data = JsonConvert.SerializeObject(gameData, Formatting.Indented);
            File.WriteAllText(Path + FileName + "S" + SlotNum.ToString() + Extension, data);
            //Debug.Log("����Ϸ�");
        }

        /// <summary>
        /// ���̺� �����͸� �ҷ��ɴϴ�. ���ڷ� �����ϴ� T�� SaveData�������̽��� ����ϴ� ��������� Ŭ�������� �մϴ�.
        /// </summary>
        /// <returns>SaveData �������̽��� ����ϴ� ����� ���� �ڷ���</returns>
        public T LoadData<T>() where T : SaveData, new()  // ��������: SaveData �������̽��� �����̸�, ����Ʈ �����ڰ� ����
        {
            
            if( IsFileExist() ) // ����� ������ �ִٸ� ���� ���ӵ����͸� ����� ��ȯ
            {
                Debug.Log("������ ���� " + Path);
                string data = File.ReadAllText(Path + FileName + "S" + SlotNum.ToString() + Extension);
                return JsonConvert.DeserializeObject<T>(data); 
            }
            else //����� ������ ���ٸ� ���Ӱ� ���ӵ����͸� ����� ��ȯ
            {
                Debug.Log("������ ����");                                    
                return new T();   
            }
        }


        /// <summary>
        /// �κ��丮 ���� �ε� �޼����Դϴ�. �ʱ�ȭ ���� Ŭ���� �������ڸ� ���� �κ��丮�� �ʱ�ȭ�� �����մϴ�.<br/>
        /// </summary>
        /// <returns>�κ��丮 ����� ���� Ŭ������ ��ȯ</returns>
        public InventorySaveData LoadInventoryData(InventoryInitializer initializer) 
        {
            if(initializer==null)
                throw new System.Exception("�̴ϼȶ������� ���޵��� �ʾҽ��ϴ�.");

            
            // Id�� ����ϱ����� �ν��Ͻ��� �����մϴ�.
            CreateManager createManager = GetComponent<CreateManager>();
            string keyPrefabName = initializer.inventoryInfo.OwnerTr.name;
            int newId = initializer.inventoryId;

            // ���� �ɼ��� ���� �� ���
            if(initializer.isReset)
            {
                Debug.Log("���� �ʱ�ȭ " + Path);

                // Id�ߺ����� ���� ��Ͽ� �����ߴٸ�
                if( !createManager.RegisterId( IdType.Inventory, keyPrefabName, newId) )
                    throw new Exception("Id�ߺ����� ���� ��Ͽ� �����߽��ϴ�. ���ο� Id�� �ο��� �ּ���.");

                return new InventorySaveData(initializer); 
            }
            
            if( IsFileExist() ) // ����� ������ �ִٸ� ���� ���ӵ����͸� ����� ��ȯ
            {
                Debug.Log("������ ���� " + Path);
                string data = File.ReadAllText(Path + FileName + "S" + SlotNum.ToString() + Extension);
                return JsonConvert.DeserializeObject<InventorySaveData>(data); 
            }
            else //����� ������ ���ٸ� ���Ӱ� ���ӵ����͸� ����� ��ȯ
            {
                Debug.Log("������ ����");
                
                // Id�� ���Ӱ� ����մϴ�.
                createManager.RegisterId( IdType.Inventory, keyPrefabName, newId);

                // �κ��丮 �����͸� ��ȯ�մϴ�.
                return new InventorySaveData(initializer);   
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