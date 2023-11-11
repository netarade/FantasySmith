using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
 * 3- ���۸���� ���ո�� ����Ʈ���� CraftableWeaponList Ŭ������ �����Ͽ� �� ������ �̸��� �����ϵ��� ����
 * => ���� 1�ܰ迡�� ��Ӵٿ� ��ư���� ���� �������� ���� ����� ������ �ʿ伺�� �ֱ� ���� 
 */


namespace CraftData
{   
    /// <summary>
    /// �÷��̾� ���� ���� ������ ����Դϴ�. ������ ���� ���� string name�� �����ϴ� ����Ʈ���� �����ϰ� �ֽ��ϴ�.
    /// </summary>
    [Serializable]
    public class CraftableWeaponList
    {
        /// <summary>
        /// ���۰����� ��-��� ����Դϴ�. string weaponName���� �����Ͻʽÿ�.
        /// </summary>
        public List<string> swordList;

        /// <summary>
        /// ���۰����� Ȱ-���� ����Դϴ�. string weaponName���� �����Ͻʽÿ�.
        /// </summary>
        public List<string> bowList;

        public CraftableWeaponList()
        {
            swordList = new List<string>();
            bowList = new List<string>();
        }
    }

    /// <summary>
    /// �κ��丮�� �����ϴ� Ŭ����, ���ο� GameObject�� �����ϴ� weapList�� miscList�� �ִ�. save, Load�� �����ؾ� �Ѵ�.
    /// </summary>
    [Serializable]
    public class PlayerInventory
    {
        /// <summary>
        /// �÷��̾ �����ϰ� �ִ� ���� ������ ����Դϴ�. ���ӿ�����Ʈ�� �����մϴ�. ���� ������ �ִٸ� �����ؾ� �մϴ�.
        /// </summary>
        public List<GameObject> weapList;

        /// <summary>
        /// �÷��̾ �����ϰ� �ִ� ��ȭ ������ ��� �Դϴ�. ���ӿ�����Ʈ�� �����մϴ�. ���� ������ �ִٸ� �����ؾ� �մϴ�.
        /// </summary>
        public List<GameObject> miscList;

        /// <summary>
        /// �÷��̾� �κ��丮�� �ִ� ĭ�� �Դϴ�. ���� �߿� ���׷��̵� ������ ���� ������ �� �ֽ��ϴ�.
        /// </summary>
        public int InventoryMaxCount;

        public PlayerInventory()
        {            
            weapList = new List<GameObject>(){};
            miscList = new List<GameObject>(){};
            InventoryMaxCount = 50;
        }

        public PlayerInventory(List<GameObject> weapList, List<GameObject> miscList, int InventoryMaxCount )
        {
            this.weapList = weapList;
            this.miscList = miscList;
            this.InventoryMaxCount = InventoryMaxCount;
        }
    }    


    /// <summary>
    /// ���� ���� �Ӽ����� ��Ƴ��� Ŭ�����μ� ���ۿ� �ʿ��� �÷��̾��� �����Ϳ� �ش�
    /// </summary>
    [Serializable]
    public struct CraftProficiency
    {   
        private int iProficiency;
        /// <summary>
        /// ����� ���� ���õ��� ���
        /// </summary>
        public int Proficiency
        {
            set {  iProficiency = Mathf.Clamp(value, 0, 100); }    
            get { return iProficiency; }
        }

        private int iRecipieHitCount;
        /// <summary>
        /// �����ǰ� ��Ȯ�ϰ� ���� Ƚ��
        /// </summary>
        public int RecipieHitCount 
        { 
            get { return iRecipieHitCount; } 
            set { iRecipieHitCount = value; }
        }
        
        public CraftProficiency(int proficiency, int hitCount)
        {
            iProficiency= proficiency;
            iRecipieHitCount= hitCount;
        }
    }


}
