using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;

/*
 * [작업 사항]  
 * <v1.0 - 2024_0124_최원준>
 * 1- 게임 진행 중 할당되는 고유의 식별번호를 저장하기 위한 목적의 클래스 작성
 * 
 * <v1.1 - 2024_0125_최원준>
 * 1- inventoryId의 JsonProperty를 추가
 * 
 * 2- keyPrefabName 매개변수 명을 rootName으로 변경 
 * (최상위 부모라는 의미)
 * 
 */


namespace DataManagement
{ 
    /// <summary>
    /// Id의 종류로 IdData클래스에 사용되는 메서드의 전달인자로 사용됩니다.<br/>
    /// </summary>
    public enum IdType { Inventory }

    /// <summary>
    /// 고유 식별번호 저장 전용 클래스입니다.<br/>
    /// 주의 사항 - 유니티 전용 클래스는 저장 불가. 기본 자료형으로 저장하거나, 구조체 또는 클래스를 만들어 저장해야 합니다.
    /// </summary>
    public class IdData : SaveData
    {
        /// <summary>
        /// 인벤토리의 고유 식별 번호를 기록하기 위한 딕셔너리입니다.<br/>
        /// 키 값은 프리팹 명이며, 동일 한 이름의 프리팹으로 생성되는 인벤토리는 다른 식별 번호를 int값으로 가지고 있습니다.
        /// </summary>
        [JsonProperty] Dictionary<string, List<int>> inventoryId;


        /// <summary>
        /// IdData 인스턴스를 새롭게 만드는 경우 사용되는 생성자입니다.<br/>
        /// DataManager에서 Load할 때 저장파일의 값을 채워 넣기 전에 사용됩니다.
        /// </summary>
        public IdData()
        {
            inventoryId = new Dictionary<string, List<int>>();
        }

                
        /// <summary>
        /// 인자로 전달한 Id종류에 따른 Id사전을 가져옵니다.
        /// </summary>
        /// <returns>해당 사전 참조값을 반환, 해당 사전이 없다면 null값을 반환</returns>
        private Dictionary<string, List<int>> GetIdDic(IdType idDicType)
        {
            Dictionary<string, List<int>> idDic = null;

            // 전달인자
            if(idDicType == IdType.Inventory)
                idDic = inventoryId;

            return idDic;
        }

        /// <summary>
        /// 인자로 전달한 Id종류와 딕셔너리의 키로 접근할 최상위 프리팹 명에 따른 Id리스트를 가져옵니다.<br/>
        /// 세번째 전달인자 옵션을 통해 키가 없다면 리스트를 새롭게 생성하여 반환할 수 있습니다.<br/>
        /// </summary>
        /// <returns>새로 생성하기 옵션이 없으면서 해당 키 값의 리스트가 존재하지 않는경우 null값을 반환, 
        /// 이외에는 해당 리스트 참조값 또는 새로운 리스트 참조값 반환</returns>
        private List<int> GetIdList(IdType idDicType, string keyPrefabName, bool isNewIfNotExist)
        {
            Dictionary<string, List<int>> idDic = GetIdDic(idDicType);
            

            // 해당 키값으로 사전에 접근했을 때 리스트가 존재한다면,
            if( idDic.ContainsKey(keyPrefabName) )
                return idDic[keyPrefabName];

            // 리스트가 존재하지 않지만 새로 생성하기 옵션이 걸려있다면
            else if(isNewIfNotExist)
            {
                idDic.Add(keyPrefabName, new List<int>());
                return idDic[keyPrefabName];
            }

            // 리스트가 존재하지 않고, 새로 생성하기 옵션도 걸려있지 않다면
            else
                return null;
        }


        /// <summary>
        /// 동일한 프리팹에 다른 고유의 식별번호를 부여하고 저장합니다.<br/>
        /// 전달 인자로 어떤 Id 딕셔너리를 참조할 것인지와 최상위 프리팹명을 전달해야 합니다.<br/> 
        /// 해당 사전에 저장되어있는 동일한 키값에 있는 식별번호 이외의 다른 번호를 새롭게 부여하고 사전에 저장합니다.  
        /// </summary>
        /// <returns>새롭게 부여된 식별번호를 반환</returns>
        public int GetNewId(IdType idDicType, string rootName)
        {            
            // 키값에 따른 idList를 반환받습니다. (없다면 새로 생성하여 반환받습니다.)      
            List<int> idList = GetIdList(idDicType, rootName, true);    
            
            
            // 식별번호 목록에 하나의 값도 없는 경우, 최초 식별번호를 0번부터 부여합니다.
            if(idList.Count==0)
            {
                // 리스트의 0번항목에 0을 집어넣고 메서드를 종료합니다.
                idList.Add(0);
                return 0;            
            }
            
           
            idList.Sort();      // idList를 오름차순으로 정렬합니다.
            int targetIdx = -1; // 목표 인덱스를 선언합니다.

            for(int i=0; i<idList.Count; i++)   // 0번항목 부터 순서대로 값을 읽습니다.
            {
                // i번째 식별번호가 i번째 인덱스보다 크다면 (틀리다면)
                if( idList[i] > i )
                {
                    targetIdx = i;  // 목표 인덱스로 설정하고 for문을 종료합니다.
                    break;
                }
            }

            // 목표 인덱스의 변동이 있는 경우는 해당 인덱스로 추가합니다. 
            if(targetIdx!=-1)
                idList.Add(targetIdx);
            // 목표 인덱스의 변동이 없는 경우는 마지막 인덱스에 1을 더한 값으로 추가합니다.
            else
                idList.Add(idList.Count);

            // 마지막으로 추가된 식별번호를 반환합니다.
            return idList[idList.Count-1];
        }

        /// <summary>
        /// 고유 식별 번호를 원하는 숫자로 등록합니다.<br/>
        /// 참조할 Id종류, 키로 접근할 최상위 프리팹 명, 등록할 식별 번호를 전달해야 합니다.<br/>
        /// 동일한 id가 존재한다면 실패를 반환받습니다.
        /// </summary>
        /// <returns>id 등록 성공 시 true를, 실패 시 false를 반환</returns>
        public bool RegisterId(IdType idDicType, string rootName, int id)
        {            
            // 키값에 따른 idList를 반환받습니다. (없다면 새로 생성하여 반환받습니다.)       
            List<int> idList = GetIdList(idDicType, rootName, true);

            // id가 이미 존재한다면 실패를 반환합니다.
            if( idList.Contains( id ) )
            {
                UnityEngine.Debug.Log("ID가 이미 존재합니다.");
                return false;
            }
            // id가 존재하지 않는다면 리스트에 추가하고 성공 반환합니다.
            idList.Add(id);
            return true;
        }

        /// <summary>
        /// 등록되어있는 고유 식별 번호의 등록을 해제합니다.<br/>
        /// 참조할 Id종류, 키로 접근할 최상위 프리팹 명, 해제할 식별 번호를 전달해야 합니다.
        /// </summary>
        public void UnregisterId( IdType idDicType, string rootName, int id )
        {
            // 키값에 따른 idList를 반환받습니다. (없다면 새로 생성하지 않습니다.)    
            List<int> idList = GetIdList(idDicType, rootName, false);

            // id리스트가 존재하지 않는다면 종료합니다.
            if(idList==null)
                return;

            // 해당 id가 존재한다면 제거합니다.
            if( idList.Contains(id) )
                idList.Remove(id);
        }


    }
    
}