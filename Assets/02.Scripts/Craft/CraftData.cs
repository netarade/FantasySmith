using System;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using WorldItemData;
using Newtonsoft.Json;

/*
 * [���� ����]
 * �÷��̾� ���ۿ� �ʿ��� �������̸�, ����ȭ�Ǿ� Save �� Load �Ǿ�� �� ����
 * 
 * [�۾� ����]  
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- ���۰��� Ŭ���� ���Ϻи� 
 * 
 * <v2.0 - 2023_1106_�ֿ���>
 * 1- �÷��̾� �κ��丮 Ŭ���� �߰�, CraftProficiency ����ü�� itemList ����
 * 2- �ּ�ó��
 * 
 * 3- ���۸���� ���ո�� ����Ʈ���� Craftdic Ŭ������ �����Ͽ� �� ������ �̸��� �����ϵ��� ����
 * => ���� 1�ܰ迡�� ��Ӵٿ� ��ư���� ���� �������� ���� ����� ������ �ʿ伺�� �ֱ� ���� 
 * 
 * <v3.0 - 2023_1112_�ֿ���>
 * 1- �κ��丮 ���� �̸�����(InvnetoryMaxCount->TotalMaxCount)
 * 2- �κ��丮 Ŭ������ PlayerInventory���� Inventory�� ����
 * 
 * <v4.0 - 2023_1113_�ֿ���>
 * 1- �з� ����� Ivnentory Ŭ������ Inventory.cs���Ϸ� �и��Ͽ���. (namespace�� �״��)
 * 
 * <v5.0 - 2023_1119_�ֿ���>
 * 1- CraftProficiency�� ������� name�� �߰��ϰ� ������ ����
 * 
 * 2- CraftableWeaponList�� CraftDic Ŭ������ �̸� �����Ͽ�����, �������� ������ ����. 
 * ���� ����Ʈ�� ��ųʸ��� �����Ͽ� ���� ������ �����ϰ� �Ͽ���.
 * 
 * 3- CraftDic Ŭ������ ���ҿ� ���� ��� ���� ����.
 * �̸��� ���õ��� �Բ� ������ �ʿ伺 (�Ϸ�)
 * ���� ��Ͽ� �ִ��� ������ �˻��ϴ� ���
 * �ش��̸��� ���� ���õ��� �÷��ִ� ���.
 * ���� ����� ä�����ٸ� �ٸ� ���۸���� �ع��ϴ� ��ɰ� �ع��� �˷��ִ� ����� �־����.
 * ���� CreateManager���� ���� ����� ä���ְ� ������, CraftDic ��ü���� CreateManager�� ���� �������� �����Ͽ� ������ ä���� �Ѵ�.  (�Ϸ�)
 * 
 * 4- ��Ÿ �ּ� ����
 * 
 * <v5.1 - 2023_1122>
 * 1- ��ųʸ� ������� �ּ� ����
 * 
 * <v5.2 - 2023_1221>
 * 1- weaponDic������ CreateManager�� �̱��濡�� �����ϴ� ���� ���� MonoBehaviour�� ������� �ʴ� ��ũ��Ʈ�� WorldItemData_Weapon�� ������ ����
 * Craftdic��ũ��Ʈ�� Monobehviour�� ������� �ʱ� ������ �̱����� ������ ��Ʊ� ����
 * 
 * <v6.0 - 2023_1222_�ֿ���>
 * 1- private������ ����ȭ�ϱ� ���� [JsonProperty] ��Ʈ����Ʈ�� �߰��Ͽ���
 * 
 */


namespace CraftData
{   
    /// <summary>
    /// �÷��̾� ���� ���� ������ ����Դϴ�. ������ ���� ���� string name�� �����ϴ� ����Ʈ���� �����ϰ� �ֽ��ϴ�.
    /// </summary>
    [Serializable]
    public class Craftdic
    {
        /// <summary>
        /// ���۰����� ��-��� ����Դϴ�. ������ �̸����� �����ϸ� �ش� ����� ���õ� ������ ���� ����ü�� �Ҵ� �޽��ϴ�.
        /// </summary>
        public Dictionary<string, CraftProficiency> swordDic;

        /// <summary>
        /// ���۰����� Ȱ-���� ����Դϴ�. ������ �̸����� �����ϸ� �ش� ����� ���õ� ������ ���� ����ü�� �Ҵ� �޽��ϴ�.
        /// </summary>
        public Dictionary<string, CraftProficiency> bowDic;


        /// <summary>
        /// ������ ó�� �����Ͽ� ���� ����� ó�� �����ϴ� ��� �ʱ� �����۸� �ع�� ä�� ���� ����� �����˴ϴ�.
        /// </summary>
        public Craftdic()
        {
            swordDic = new Dictionary<string, CraftProficiency> ();
            bowDic = new Dictionary<string, CraftProficiency> ();           

            Dictionary<string, Item> weaponDic = new WorldItem().weapDic;         // ���� ��������� �����մϴ�.

            foreach( Item item in weaponDic.Values )                                // ��� ����������� �ϳ��� �����ϴ�.
            {
                ItemWeapon weapItem = (ItemWeapon)item;                             // �ϳ��� ������ �� value���� Item���̸�, �� ���� ���� ������ ���� ItemWeapon������ ��ȯ�մϴ�.

                if( weapItem.BasicGrade==ItemGrade.Low )                            // ���� �������� ���� �������� �ʱ� ������,
                {
                    if( weapItem.WeaponType==WeaponType.Sword )                             // ��-��� Ÿ���̶��,                
                        swordDic.Add( weapItem.Name, new CraftProficiency(weapItem.Name) );     // �÷��̾� ���� ��� ��-��� ��Ͽ� �߰��մϴ�.                
                    else if( weapItem.WeaponType==WeaponType.Bow )                          // Ȱ-���� Ÿ���̶��,
                        bowDic.Add( weapItem.Name, new CraftProficiency(weapItem.Name) );       // �÷��̾� ���� ��� Ȱ-���� ��Ͽ� �߰��մϴ�.
                }
            }
        }
    }

        


    /// <summary>
    /// ���� ���� �Ӽ����� ��Ƴ��� Ŭ�����μ� ���ۿ� �ʿ��� �÷��̾��� �����Ϳ� �ش�
    /// </summary>
    [Serializable]
    public struct CraftProficiency
    {   
        [JsonProperty] private string sName;               // ���� ����� �̸�
        [JsonProperty] private int iProficiency;           // ���� ����� ���� ���õ�
        [JsonProperty] private int iRecipieHitCount;       // ���� ����� ������ Ƚ�� 
        
        /// <summary>
        /// ���� ����� �̸��Դϴ�.
        /// </summary>
        public string Name { get { return sName; } }

        /// <summary>
        /// ���� ����� ���õ� �Դϴ�. ���� ������ 0~100 �̳��� ���� �����մϴ�.
        /// </summary>
        public int Proficiency
        {
            set {  iProficiency = Mathf.Clamp(value, 0, 100); }    
            get { return iProficiency; }
        }

        /// <summary>
        /// �÷��̾ ���� ����� �����Ǹ� ��Ȯ�ϰ� ���� Ƚ���Դϴ�.
        /// </summary>
        public int RecipieHitCount 
        { 
            get { return iRecipieHitCount; } 
            set { iRecipieHitCount = value; }
        }
        
        /// <summary>
        /// �̸��� ������ ���� ������ �����ϴ� ��� ó�� ������ ������ �Ǵ��ϸ�, ���õ��� ������ ���� Ƚ���� 0���� �ʱ�ȭ�˴ϴ�.
        /// </summary>
        public CraftProficiency(string name)
        {
            sName = name;
            iProficiency = 0;
            iRecipieHitCount = 0;
        }

        /// <summary>
        /// ������ ���� ������ �ִ� ��� �� ���ڸ� �Է��Ͽ� ���� �ʱ�ȭ �մϴ�.
        /// </summary>
        /// <param name="name">���� ����� �̸�</param>
        /// <param name="proficiency">���� ���õ�</param>
        /// <param name="hitCount">�����Ǹ� ���� Ƚ��</param>
        public CraftProficiency(string name, int proficiency, int hitCount)
        {
            sName = name;
            iProficiency= proficiency;
            iRecipieHitCount= hitCount;
        }
    }


}
