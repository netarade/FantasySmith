using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;

/*
 * [�۾� ����]  
 * <v1.0 - 2024_0124_�ֿ���>
 * 1- ���� ���� �� �Ҵ�Ǵ� ������ �ĺ���ȣ�� �����ϱ� ���� ������ Ŭ���� �ۼ�
 * 
 * <v1.1 - 2024_0125_�ֿ���>
 * 1- inventoryId�� JsonProperty�� �߰�
 * 
 * 2- keyPrefabName �Ű����� ���� rootName���� ���� 
 * (�ֻ��� �θ��� �ǹ�)
 * 
 */


namespace DataManagement
{ 
    /// <summary>
    /// Id�� ������ IdDataŬ������ ���Ǵ� �޼����� �������ڷ� ���˴ϴ�.<br/>
    /// </summary>
    public enum IdType { Inventory }

    /// <summary>
    /// ���� �ĺ���ȣ ���� ���� Ŭ�����Դϴ�.<br/>
    /// ���� ���� - ����Ƽ ���� Ŭ������ ���� �Ұ�. �⺻ �ڷ������� �����ϰų�, ����ü �Ǵ� Ŭ������ ����� �����ؾ� �մϴ�.
    /// </summary>
    public class IdData : SaveData
    {
        /// <summary>
        /// �κ��丮�� ���� �ĺ� ��ȣ�� ����ϱ� ���� ��ųʸ��Դϴ�.<br/>
        /// Ű ���� ������ ���̸�, ���� �� �̸��� ���������� �����Ǵ� �κ��丮�� �ٸ� �ĺ� ��ȣ�� int������ ������ �ֽ��ϴ�.
        /// </summary>
        [JsonProperty] Dictionary<string, List<int>> inventoryId;


        /// <summary>
        /// IdData �ν��Ͻ��� ���Ӱ� ����� ��� ���Ǵ� �������Դϴ�.<br/>
        /// DataManager���� Load�� �� ���������� ���� ä�� �ֱ� ���� ���˴ϴ�.
        /// </summary>
        public IdData()
        {
            inventoryId = new Dictionary<string, List<int>>();
        }

                
        /// <summary>
        /// ���ڷ� ������ Id������ ���� Id������ �����ɴϴ�.
        /// </summary>
        /// <returns>�ش� ���� �������� ��ȯ, �ش� ������ ���ٸ� null���� ��ȯ</returns>
        private Dictionary<string, List<int>> GetIdDic(IdType idDicType)
        {
            Dictionary<string, List<int>> idDic = null;

            // ��������
            if(idDicType == IdType.Inventory)
                idDic = inventoryId;

            return idDic;
        }

        /// <summary>
        /// ���ڷ� ������ Id������ ��ųʸ��� Ű�� ������ �ֻ��� ������ �� ���� Id����Ʈ�� �����ɴϴ�.<br/>
        /// ����° �������� �ɼ��� ���� Ű�� ���ٸ� ����Ʈ�� ���Ӱ� �����Ͽ� ��ȯ�� �� �ֽ��ϴ�.<br/>
        /// </summary>
        /// <returns>���� �����ϱ� �ɼ��� �����鼭 �ش� Ű ���� ����Ʈ�� �������� �ʴ°�� null���� ��ȯ, 
        /// �̿ܿ��� �ش� ����Ʈ ������ �Ǵ� ���ο� ����Ʈ ������ ��ȯ</returns>
        private List<int> GetIdList(IdType idDicType, string keyPrefabName, bool isNewIfNotExist)
        {
            Dictionary<string, List<int>> idDic = GetIdDic(idDicType);
            

            // �ش� Ű������ ������ �������� �� ����Ʈ�� �����Ѵٸ�,
            if( idDic.ContainsKey(keyPrefabName) )
                return idDic[keyPrefabName];

            // ����Ʈ�� �������� ������ ���� �����ϱ� �ɼ��� �ɷ��ִٸ�
            else if(isNewIfNotExist)
            {
                idDic.Add(keyPrefabName, new List<int>());
                return idDic[keyPrefabName];
            }

            // ����Ʈ�� �������� �ʰ�, ���� �����ϱ� �ɼǵ� �ɷ����� �ʴٸ�
            else
                return null;
        }


        /// <summary>
        /// ������ �����տ� �ٸ� ������ �ĺ���ȣ�� �ο��ϰ� �����մϴ�.<br/>
        /// ���� ���ڷ� � Id ��ųʸ��� ������ �������� �ֻ��� �����ո��� �����ؾ� �մϴ�.<br/> 
        /// �ش� ������ ����Ǿ��ִ� ������ Ű���� �ִ� �ĺ���ȣ �̿��� �ٸ� ��ȣ�� ���Ӱ� �ο��ϰ� ������ �����մϴ�.  
        /// </summary>
        /// <returns>���Ӱ� �ο��� �ĺ���ȣ�� ��ȯ</returns>
        public int GetNewId(IdType idDicType, string rootName)
        {            
            // Ű���� ���� idList�� ��ȯ�޽��ϴ�. (���ٸ� ���� �����Ͽ� ��ȯ�޽��ϴ�.)      
            List<int> idList = GetIdList(idDicType, rootName, true);    
            
            
            // �ĺ���ȣ ��Ͽ� �ϳ��� ���� ���� ���, ���� �ĺ���ȣ�� 0������ �ο��մϴ�.
            if(idList.Count==0)
            {
                // ����Ʈ�� 0���׸� 0�� ����ְ� �޼��带 �����մϴ�.
                idList.Add(0);
                return 0;            
            }
            
           
            idList.Sort();      // idList�� ������������ �����մϴ�.
            int targetIdx = -1; // ��ǥ �ε����� �����մϴ�.

            for(int i=0; i<idList.Count; i++)   // 0���׸� ���� ������� ���� �н��ϴ�.
            {
                // i��° �ĺ���ȣ�� i��° �ε������� ũ�ٸ� (Ʋ���ٸ�)
                if( idList[i] > i )
                {
                    targetIdx = i;  // ��ǥ �ε����� �����ϰ� for���� �����մϴ�.
                    break;
                }
            }

            // ��ǥ �ε����� ������ �ִ� ���� �ش� �ε����� �߰��մϴ�. 
            if(targetIdx!=-1)
                idList.Add(targetIdx);
            // ��ǥ �ε����� ������ ���� ���� ������ �ε����� 1�� ���� ������ �߰��մϴ�.
            else
                idList.Add(idList.Count);

            // ���������� �߰��� �ĺ���ȣ�� ��ȯ�մϴ�.
            return idList[idList.Count-1];
        }

        /// <summary>
        /// ���� �ĺ� ��ȣ�� ���ϴ� ���ڷ� ����մϴ�.<br/>
        /// ������ Id����, Ű�� ������ �ֻ��� ������ ��, ����� �ĺ� ��ȣ�� �����ؾ� �մϴ�.<br/>
        /// ������ id�� �����Ѵٸ� ���и� ��ȯ�޽��ϴ�.
        /// </summary>
        /// <returns>id ��� ���� �� true��, ���� �� false�� ��ȯ</returns>
        public bool RegisterId(IdType idDicType, string rootName, int id)
        {            
            // Ű���� ���� idList�� ��ȯ�޽��ϴ�. (���ٸ� ���� �����Ͽ� ��ȯ�޽��ϴ�.)       
            List<int> idList = GetIdList(idDicType, rootName, true);

            // id�� �̹� �����Ѵٸ� ���и� ��ȯ�մϴ�.
            if( idList.Contains( id ) )
            {
                UnityEngine.Debug.Log("ID�� �̹� �����մϴ�.");
                return false;
            }
            // id�� �������� �ʴ´ٸ� ����Ʈ�� �߰��ϰ� ���� ��ȯ�մϴ�.
            idList.Add(id);
            return true;
        }

        /// <summary>
        /// ��ϵǾ��ִ� ���� �ĺ� ��ȣ�� ����� �����մϴ�.<br/>
        /// ������ Id����, Ű�� ������ �ֻ��� ������ ��, ������ �ĺ� ��ȣ�� �����ؾ� �մϴ�.
        /// </summary>
        public void UnregisterId( IdType idDicType, string rootName, int id )
        {
            // Ű���� ���� idList�� ��ȯ�޽��ϴ�. (���ٸ� ���� �������� �ʽ��ϴ�.)    
            List<int> idList = GetIdList(idDicType, rootName, false);

            // id����Ʈ�� �������� �ʴ´ٸ� �����մϴ�.
            if(idList==null)
                return;

            // �ش� id�� �����Ѵٸ� �����մϴ�.
            if( idList.Contains(id) )
                idList.Remove(id);
        }


    }
    
}