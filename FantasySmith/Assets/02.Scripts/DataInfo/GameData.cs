using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using CraftData;

/*
 * [���� ����]
 * ���� ���̺�� �ε忡 �ʿ��� ������ Ŭ���� ���� 
 * 
 * [�۾� ����]  
 * <v1.0 - 2023_1106_�ֿ���>
 * 1- �ʱ� GameData ����
 * 
 * <v1.1 - 2023_1106_�ֿ���
 * 1- Transform ���� ����, Ŭ���� CraftableWeaponList ���� �߰�
 * 2- �ּ� ����
 */

namespace DataManagement
{
    /// <summary>
    /// ���� ���� - ����Ƽ ���� Ŭ������ ���� �Ұ�. �⺻ �ڷ������� �����ϰų�, ����ü �Ǵ� Ŭ������ ����� �����Ѵ�. 
    /// </summary>
    [Serializable]           // �ν����Ϳ� �Ʒ��� Ŭ������ ��������.
    public class GameData    // ������ ���� Ŭ���� (����� ���� ����Ƽ Ŭ����)
    {
        /// <summary>
        /// ���� �÷��� Ÿ��
        /// </summary>
        public float playTime;

        /// <summary>
        /// ��ȭ
        /// </summary>
        public float gold;

        /// <summary>
        /// ��ȭ
        /// </summary>
        public float silver;

        /// <summary>
        /// �÷��̾� ����
        /// </summary>
        public Transform playerTr;


        /// <summary>
        /// ���� �÷��̾ �����ϰ� �ִ� �κ��丮 �����Դϴ�. ���ӿ�����Ʈ�� �����ϴ� weapList�� playerMiscList �� InventoryMaxCount ���� �ֽ��ϴ�.
        /// </summary>
        public PlayerInventory inventory;

        /// <summary>
        /// ���� ���� ����� ���� �� string name���� �����ϴ� ����Ʈ���� ���� Ŭ���� �Դϴ�.
        /// </summary>
        public CraftableWeaponList craftableWeaponList;

        /// <summary>
        /// ��� ���õ� ����� name�� CraftProficincy����ü�� ���� �����Ͽ� ������ ������ �����ϰ� ���ݴϴ�.
        /// </summary>
        public Dictionary<string, CraftProficiency> proficiencyDic;
    }
}