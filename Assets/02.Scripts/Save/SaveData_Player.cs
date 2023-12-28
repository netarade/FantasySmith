using CraftData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataManagement
{ 
    /// <summary>
    /// ���� ���� - ����Ƽ ���� Ŭ������ ���� �Ұ�. �⺻ �ڷ������� �����ϰų�, ����ü �Ǵ� Ŭ������ ����� �����ؾ� �մϴ�.
    /// </summary>
    public class PlayerBasicData : SaveData
    {
        /// <summary>
        /// ���� �÷��� Ÿ��
        /// </summary>
        public float playTime;

        /// <summary>
        /// ��ȭ
        /// </summary>
        public int gold;

        /// <summary>
        /// ��ȭ
        /// </summary>
        public int silver;

        /// <summary>
        /// �÷��̾��� ���� ���� ��ϰ� ���õ�, ������ ���� Ƚ�� ������ ��� �ִ� ���� ���� Ŭ���� �Դϴ�.
        /// </summary>
        public Craftdic craftDic;        
             

        /// <summary>
        /// DataManager���� Load�޼��忡�� ���ο� GameData�� �����ϱ� ���� �������Դϴ�.<br/>
        /// ������ �����Ͱ� ���� ��� ���˴ϴ�.
        /// </summary>
        public PlayerBasicData()
        {            
            playTime = 0f;
            gold = 0;
            silver = 0;
            craftDic = new Craftdic();
        }

    }
    
}